import unittest
from httpx import Client

class TestGetApi(unittest.TestCase):
    def setUp(self):
        self.baseUrl = "http://localhost:3000/api/v1/"
        self.client = Client()
    
    def test_get_clients(self):
        response = self.client.get(self.baseUrl + "clients", headers={"API_KEY":"a1b2c3d4e5"})
        self.assertEqual(response.status_code, 200)

if __name__ == '__main__':
    unittest.main()