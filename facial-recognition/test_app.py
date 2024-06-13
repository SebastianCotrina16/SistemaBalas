import unittest
from io import BytesIO
from flask import json
from app import app, SessionLocal, User, Base, engine

class FaceRecognitionTestCase(unittest.TestCase):

    def setUp(self):
        self.app = app.test_client()
        self.app.testing = True

        # Create a new database session for each test
        Base.metadata.create_all(bind=engine)
        self.session = SessionLocal()

    def tearDown(self):
        # Close the session and drop all tables after each test
        self.session.close()
        Base.metadata.drop_all(bind=engine)

    def test_register_user(self):
        with open('Fotos/SebastianCotrina.png', 'rb') as img:
            response = self.app.post('/register', data={
                'dni': '12345678',
                'name': 'John Doe',
                'image': img
            })

        data = json.loads(response.data)
        self.assertTrue(data['success'])

        # Check if user is added to the database
        user = self.session.query(User).filter_by(dni='12345678').first()
        self.assertIsNotNone(user)
        self.assertEqual(user.name, 'John Doe')

    def test_recognize_user(self):
        # First, register a user
        with open('Fotos/Keanu.webp', 'rb') as img:
            self.app.post('/register', data={
                'dni': '12345678',
                'name': 'John Doe',
                'image': img
            })

        # Then, try to recognize the same user
        with open('Fotos/Keanu2.webp', 'rb') as img:
            response = self.app.post('/recognize', data={'image': img})

        data = json.loads(response.data)
        self.assertIsNotNone(data['user'])
        self.assertEqual(data['user']['dni'], '12345678')
        self.assertEqual(data['user']['name'], 'John Doe')

    def test_recognize_unknown_user(self):
        with open('Fotos/Adam.jpg', 'rb') as img:
            response = self.app.post('/recognize', data={'image': img})

        data = json.loads(response.data)
        self.assertIsNone(data['user'])

if __name__ == '__main__':
    unittest.main()
