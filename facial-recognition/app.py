from flask import Flask, request, jsonify
import cv2
import numpy as np
import dlib
import os
from sqlalchemy.orm import Session
from models import Base, User, engine, SessionLocal
from flask_talisman import Talisman
from flask_cors import CORS

app = Flask(__name__)


CORS(app, resources={r"/*": {"origins": "http://localhost:3000"}})

talisman = Talisman(app, content_security_policy=None, force_https=False)

IMAGE_FOLDER = 'faces'
if not os.path.exists(IMAGE_FOLDER):
    os.makedirs(IMAGE_FOLDER)

detector = dlib.get_frontal_face_detector()
sp = dlib.shape_predictor('models/shape_predictor_68_face_landmarks.dat')
facerec = dlib.face_recognition_model_v1('models/dlib_face_recognition_resnet_model_v1.dat')

def recognize_face(image):
    dets = detector(image, 1)
    if len(dets) > 0:
        shape = sp(image, dets[0])
        face_descriptor = facerec.compute_face_descriptor(image, shape)
        return np.array(face_descriptor)
    return None

def compare_faces(face_descriptor, known_face_descriptor, tolerance=0.6):
    return np.linalg.norm(face_descriptor - known_face_descriptor) <= tolerance

def get_user_by_face_descriptor(face_descriptor):
    with SessionLocal() as session:
        users = session.query(User).all()
        for user in users:
            known_face_descriptor = np.fromstring(user.face_descriptor, sep=',')
            if compare_faces(face_descriptor, known_face_descriptor):
                return user
    return None

def add_user_to_db(dni, name, face_descriptor, image_path):
    face_descriptor_str = ','.join(map(str, face_descriptor))
    with SessionLocal() as session:
        new_user = User(dni=dni, name=name, face_descriptor=face_descriptor_str, image_path=image_path)
        session.add(new_user)
        session.commit()

@app.route('/recognize', methods=['POST'])
def recognize():
    image = request.files['image'].read()
    nparr = np.frombuffer(image, np.uint8)
    img_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)

    face_descriptor = recognize_face(img_np)
    if face_descriptor is not None:
        user = get_user_by_face_descriptor(face_descriptor)
        if user:
            return jsonify({'user': {'name': user.name, 'dni': user.dni}})
        else:
            return jsonify({'user': None})
    return jsonify({'user': None})

@app.route('/register', methods=['POST'])
def register():
    dni = request.form['dni']
    name = request.form['name']
    image = request.files['image'].read()
    nparr = np.frombuffer(image, np.uint8)
    img_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
    print(img_np.shape)
    print("Working")
    face_descriptor = recognize_face(img_np)
    if face_descriptor is not None:
        image_path = os.path.join(IMAGE_FOLDER, f"{dni}.jpg")
        cv2.imwrite(image_path, img_np)

        add_user_to_db(dni, name, face_descriptor, image_path)
        print("User added")
        return jsonify({'success': True})
    print("User not added")
    return jsonify({'success': False})
    

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001)
