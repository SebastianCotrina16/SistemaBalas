import requests

def test_recognize(image_path):
    url = 'http://localhost:5001/recognize'
    with open(image_path, 'rb') as img:
        files = {'image': img}
        response = requests.post(url, files=files)
        print(response.json())
def test_register(image_path, dni, name):
    url = 'http://localhost:5001/register'
    with open(image_path, 'rb') as img:
        files = {'image': img}
        data = {'dni': dni, 'name': name}
        response = requests.post(url, files=files, data=data)
        print(response.json())
test_recognize('test2.jpg')
