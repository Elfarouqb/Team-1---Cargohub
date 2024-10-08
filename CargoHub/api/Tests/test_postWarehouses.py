import unittest
from httpx import Client

class TestPostApi(unittest.TestCase):
    def setUp(self):
        self.baseUrl = "http://localhost:3000/api/v1/"
        self.client = Client()
    
    def test_post_warehouses(self):
        response = self.client.post(self.baseUrl + "warehouses", headers={"API_KEY":"a1b2c3d4e5"}, json={
        "name": "New Warehouse",
        "location": "123 Warehouse Ave",
        "capacity": 1000
    })
        self.assertEqual(response.status_code, 201)

if __name__ == '__main__':
    unittest.main()