import torch
from torch.utils.data import DataLoader, Dataset
from torchmetrics.detection.mean_ap import MeanAveragePrecision
from torchvision.transforms import functional as F
from PIL import Image
import os
import json
import torchvision
from torchvision.models.detection.faster_rcnn import FastRCNNPredictor
import numpy as np
from itertools import combinations

center_target = (680, 1140)
head_center = (680, 300)
heart_center = (680, 700)
img_height = 1920
img_width = 1335


def is_lethal_impact(impact, head_center, heart_center, head_lethal_radius=250, heat_lethal_radius=50):
    is_head_impact = np.linalg.norm(
        np.array(impact) - np.array(head_center)) <= head_lethal_radius
    is_heart_impact = np.linalg.norm(
        np.array(impact) - np.array(heart_center)) <= heat_lethal_radius
    return is_head_impact or is_heart_impact


def calculate_individual_accuracy(impact, center_target, head_center, heart_center):
    if is_lethal_impact(impact, head_center, heart_center):
        return 0.0

    distance_from_center = np.linalg.norm(
        np.array(impact) - np.array(center_target))
    max_distance = np.linalg.norm(
        np.array(center_target) - np.array([0, img_height])) / 1.5
    accuracy = 1 - (distance_from_center / max_distance) ** 2
    return round(accuracy * 100, 2)


def calculate_dispersion(bullet_positions):
    if len(bullet_positions) > 1:
        pairwise_distances = [np.linalg.norm(
            np.array(a) - np.array(b)) for a, b in combinations(bullet_positions, 2)]
        avg_distance_between_bullets = np.mean(pairwise_distances)
        max_bullet_dispersion = np.linalg.norm(
            np.array([0, 0]) - np.array([img_width, img_height]))
        dispersion_factor = 1 - \
            (avg_distance_between_bullets / max_bullet_dispersion)
    else:
        dispersion_factor = 1
    return round(dispersion_factor * 100, 2)


def calculate_final_accuracy(bullet_positions, center_target, head_center, heart_center):
    individual_accuracies = [calculate_individual_accuracy(
        bullet, center_target, head_center, heart_center) for bullet in bullet_positions]
    average_individual_accuracy = sum(
        individual_accuracies) / len(individual_accuracies)
    dispersion_factor = calculate_dispersion(bullet_positions)
    final_accuracy = average_individual_accuracy * 0.8 + dispersion_factor * 0.2
    return round(final_accuracy, 2)


class BulletDataset(Dataset):
    def __init__(self, root, annotation_file, transforms=None):
        self.root = root
        self.transforms = transforms
        with open(annotation_file) as f:
            self.annotations = json.load(f)

    def __getitem__(self, idx):
        annotation = self.annotations[idx]
        img_path = os.path.join(self.root, annotation['image'])
        img = Image.open(img_path).convert("RGB")

        boxes = []
        labels = []
        for ann in annotation['annotations']:
            bbox = ann['bbox']
            boxes.append(bbox)
            labels.append(1)

        boxes = torch.as_tensor(boxes, dtype=torch.float32)
        labels = torch.as_tensor(labels, dtype=torch.int64)

        target = {"boxes": boxes, "labels": labels}

        if self.transforms is not None:
            img = self.transforms(img)

        return img, target

    def __len__(self):
        return len(self.annotations)


def get_transform():
    transforms = []
    transforms.append(F.to_tensor)
    return torchvision.transforms.Compose(transforms)


def get_model(num_classes):
    weights = torchvision.models.detection.FasterRCNN_ResNet50_FPN_Weights.COCO_V1
    model = torchvision.models.detection.fasterrcnn_resnet50_fpn(
        weights=weights)
    in_features = model.roi_heads.box_predictor.cls_score.in_features
    model.roi_heads.box_predictor = FastRCNNPredictor(in_features, num_classes)
    return model


def load_model(model_path, num_classes, device):
    model = get_model(num_classes)
    model.load_state_dict(torch.load(model_path, map_location=device))
    model.to(device)
    model.eval()
    return model


def evaluate_model(model, data_loader_test, device):
    results = []

    with torch.no_grad():
        for batch_idx, (images, targets) in enumerate(data_loader_test):
            images = list(image.to(device) for image in images)
            outputs = model(images)

            for i in range(len(outputs)):
                preds = {
                    'boxes': outputs[i]['boxes'].cpu(),
                    'scores': outputs[i]['scores'].cpu(),
                    'labels': outputs[i]['labels'].cpu()
                }
                target = {
                    'boxes': targets[i]['boxes'].cpu(),
                    'labels': targets[i]['labels'].cpu()
                }

                print(f"\nBatch {batch_idx + 1}, Image {i + 1}")

                # Calcular individual_accuracy para cada predicción
                individual_accuracies = []
                bullet_positions = []

                for pred_box in preds['boxes']:
                    x = (pred_box[0].item() + pred_box[2].item()) / 2
                    y = (pred_box[1].item() + pred_box[3].item()) / 2
                    bullet_positions.append((x, y))

                    # Calcular precisión individual
                    individual_accuracy = calculate_individual_accuracy(
                        (x, y), center_target, head_center, heart_center)
                    individual_accuracies.append(individual_accuracy)

                    # Imprimir los puntos y la precisión individual
                    print(
                        f"Punto de impacto: ({x:.2f}, {y:.2f}), Precisión individual: {individual_accuracy:.2f}")

                # Calcular final_accuracy
                final_accuracy = calculate_final_accuracy(
                    bullet_positions, center_target, head_center, heart_center)

                # Imprimir los puntos y la precisión final
                print(f"Puntos de impacto: {bullet_positions}")
                print(f"Precisión final: {final_accuracy:.2f}")

                # Preparar el resultado final en el formato esperado
                result = {
                    "image": f"image_{batch_idx + 1}.jpg",
                    "annotations": [
                        {"class": "bullet_hole", "bbox": box.tolist(),
                         "individual_accuracy": acc}
                        for box, acc in zip(preds['boxes'], individual_accuracies)
                    ],
                    "final_accuracy": round(final_accuracy, 2)
                }

                results.append(result)

    return results


def collate_fn(batch):
    return tuple(zip(*batch))


if __name__ == "__main__":
    model_path = 'bullet_detection_model.pth'
    data_root = "dataset/validation/images"
    annotation_file = "dataset/validation/annotations.json"
    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")

    num_classes = 2

    dataset_test = BulletDataset(data_root, annotation_file, get_transform())
    data_loader_test = DataLoader(
        dataset_test, batch_size=2, shuffle=False, num_workers=2, collate_fn=collate_fn)
    model = load_model(model_path, num_classes, device)

    results = evaluate_model(model, data_loader_test, device)

    # Imprimir los resultados en formato JSON
    for result in results:
        print(json.dumps(result, indent=4))
