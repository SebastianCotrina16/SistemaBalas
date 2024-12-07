import unittest
import os
import io
from unittest.mock import patch, MagicMock
from PIL import Image
from flask import json
from app import app
import torch
import xmlrunner


class BulletDetectionTestCase(unittest.TestCase):
    """
    Unit test suite for the Flask bullet detection application.
    """

    def setUp(self):
        self.app = app.test_client()
        self.app.testing = True
        os.makedirs(app.config['UPLOAD_FOLDER'], exist_ok=True)

    def tearDown(self):
        files = os.listdir(app.config['UPLOAD_FOLDER'])
        for file in files:
            os.remove(os.path.join(app.config['UPLOAD_FOLDER'], file))
        os.rmdir(app.config['UPLOAD_FOLDER'])

    def test_no_image_uploaded(self):
        response = self.app.post('/detect', data={})
        data = json.loads(response.data)
        self.assertEqual(response.status_code, 400)
        self.assertEqual(data['status'], 'error')
        self.assertEqual(data['message'], 'El campo de imagen es obligatorio')

    def test_detection_with_invalid_image(self):
        response = self.app.post('/detect', data={
            'image': (io.BytesIO(b"Not an image"), 'test.txt')
        }, content_type='multipart/form-data')
        data = json.loads(response.data)
        self.assertEqual(response.status_code, 400)
        self.assertEqual(data['status'], 'error')
        self.assertEqual(data['message'], 'El archivo no es una imagen válida')

    def test_no_bullet_detected(self):
        img = Image.new('RGB', (1335, 1920), color=(73, 109, 137))
        img_byte_arr = io.BytesIO()
        img.save(img_byte_arr, format='PNG')
        img_byte_arr.seek(0)
        response = self.app.post('/detect', data={
            'image': (img_byte_arr, 'no_bullets_image.png'),
            'dni': '12345678',
            'numShots': '5'
        }, content_type='multipart/form-data')
        data = json.loads(response.data)
        self.assertEqual(response.status_code, 400)
        self.assertEqual(data['status'], 'error')
        self.assertEqual(data['message'], 'No se detectaron balas')

    @patch('app.model')
    def test_detection_with_excess_bullets(self, mock_model):
        mock_predictions = [{
            'boxes': torch.tensor([[10, 10, 20, 20]] * 6), 
            'scores': torch.tensor([0.9] * 6)
        }]
        mock_model.return_value = mock_predictions

        img = Image.open("test_images/excess_bullets_image.jpg")
        img_byte_arr = io.BytesIO()
        img.save(img_byte_arr, format='JPEG')
        img_byte_arr.seek(0)
        response = self.app.post('/detect', data={
            'image': (img_byte_arr, 'excess_bullets_image.jpg'),
            'dni': '12345678',
            'numShots': '5' 
        }, content_type='multipart/form-data')
        data = json.loads(response.data)
        self.assertEqual(response.status_code, 400)
        self.assertEqual(data['status'], 'error')
        self.assertEqual(data['message'], 'Se detectaron más balas de las permitidas')

    @patch('app.model')
    def test_successful_bullet_detection_with_score(self, mock_model):
        mock_predictions = [{
            'boxes': torch.tensor([
                [864, 1620, 884, 1640],
                [1030, 1431, 1050, 1451],
                [669, 78, 689, 98],
                [551, 1167, 571, 1187],
                [796, 706, 816, 726]
            ]),
            'scores': torch.tensor([0.9] * 5)
        }]
        mock_model.return_value = mock_predictions

        img = Image.open("test_images/valid_bullets_image.jpg")
        img_byte_arr = io.BytesIO()
        img.save(img_byte_arr, format='JPEG')
        img_byte_arr.seek(0)
        
        response = self.app.post('/detect', data={
            'image': (img_byte_arr, 'valid_bullets_image.jpg'),
            'dni': '70508277',
            'numShots': '5'
        }, content_type='multipart/form-data')
        
        try:
            data = json.loads(response.data)
            self.assertEqual(response.status_code, 200)
            self.assertIn('disparos', data)
            self.assertIn('final_accuracy', data)
            self.assertTrue(isinstance(data['final_accuracy'], float))
            self.assertAlmostEqual(data['final_accuracy'], 53.11, places=1) 
        except AssertionError:
            print("Response data:", response.data)
            raise

if __name__ == '__main__':
    unittest.main(testRunner=xmlrunner.XMLTestRunner(output='test-reports'))
