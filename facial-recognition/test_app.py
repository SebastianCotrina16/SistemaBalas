import unittest
from flask import json
from unittest.mock import patch, MagicMock
from io import BytesIO
from app import app, upload_image_to_blob, extract_face_descriptor, SessionLocal
from models import User
import xmlrunner

class FaceRecognitionTestCase(unittest.TestCase):
    def setUp(self):
        self.app = app.test_client()
        self.app.testing = True
        self.session = SessionLocal()

    @patch('app.upload_image_to_blob', return_value='http://example.com/image.jpg')
    @patch('app.extract_face_descriptor', return_value=[0.1] * 128)
    def test_register_user(self, mock_extract_face_descriptor, mock_upload_image_to_blob):
        with open('../Fotos/SebastianCotrina.png', 'rb') as img:
            img_bytes = img.read()
        response = self.app.post('/register', data={
            'dni': '67812345',
            'name': 'John Doe',
            'image': (BytesIO(img_bytes), 'SebastianCotrina.png')
        })

        data = json.loads(response.data)
        self.assertTrue(data['success'])
        self.assertEqual(data['image_url'], 'http://example.com/image.jpg')
        user = self.session.query(User).filter_by(dni='67812345').first()
        self.assertIsNotNone(user)
        self.assertEqual(user.name, 'John Doe')

    @patch('app.extract_face_descriptor', return_value=[0.1] * 128) 
    def test_recognize_user(self, mock_extract_face_descriptor):
        
        with open('../Fotos/Keanu2.webp', 'rb') as img:
            img_bytes = img.read()
        response = self.app.post('/recognize', data={'image': (BytesIO(img_bytes), 'Keanu2.webp')})

        data = json.loads(response.data)
        self.assertIsNotNone(data['user'])
        self.assertEqual(data['user']['dni'], '33333333')
        self.assertEqual(data['user']['name'], 'Consistent User')

    @patch('app.extract_face_descriptor', return_value=[0.4] * 128)
    def test_recognize_unknown_user(self, mock_extract_face_descriptor):
        with open('../Fotos/Adam.jpg', 'rb') as img:
            img_bytes = img.read()
        response = self.app.post('/recognize', data={'image': (BytesIO(img_bytes), 'Adam.jpg')})

        data = json.loads(response.data)
        self.assertIsNone(data['user'])

    @patch('app.extract_face_descriptor')
    def test_unique_descriptor(self, mock_extract_face_descriptor):
        descriptor_user1 = [0.1] * 128 
        descriptor_user2 = [0.4] * 128 
        
        mock_extract_face_descriptor.side_effect = [descriptor_user1, descriptor_user2]

        with open('../Fotos/Adam.jpg', 'rb') as img1:
            img1_bytes = img1.read()
        self.app.post('/register', data={
            'dni': '11111111',
            'name': 'User One',
            'image': (BytesIO(img1_bytes), 'Adam.jpg')
        })

        with open('../Fotos/Adam2.jpg', 'rb') as img2:
            img2_bytes = img2.read()
        self.app.post('/register', data={
            'dni': '22222222',
            'name': 'User Two',
            'image': (BytesIO(img2_bytes), 'Adam2.jpg')
        })

        self.assertNotEqual(descriptor_user1, descriptor_user2, "Descriptors should be unique for different users")


    @patch('app.extract_face_descriptor')
    def test_descriptor_consistency(self, mock_extract_face_descriptor):
        mock_extract_face_descriptor.return_value = [0.1] * 128

        with open('../Fotos/SebastianCotrina.png', 'rb') as img:
            img_bytes = img.read()
        self.app.post('/register', data={
            'dni': '33333333',
            'name': 'Consistent User',
            'image': (BytesIO(img_bytes), 'SebastianCotrina.png')
        })

        with open('../Fotos/SebastianCotrina2.jpeg', 'rb') as img:
            img_bytes = img.read()
        response = self.app.post('/register', data={
            'dni': '33333334',
            'name': 'Consistent User',
            'image': (BytesIO(img_bytes), 'SebastianCotrina2.jpeg')
        })

        self.assertEqual(mock_extract_face_descriptor.call_count, 2, "Descriptor should be generated twice")
        descriptor1 = mock_extract_face_descriptor.return_value
        descriptor2 = mock_extract_face_descriptor.return_value
        self.assertEqual(descriptor1, descriptor2, "Descriptors should be consistent for the same person")

if __name__ == '__main__':
    unittest.main(testRunner=xmlrunner.XMLTestRunner(output='test-reports'))