import unittest
from httpx import Client

class TestPostApi(unittest.TestCase):
    def setUp(self):
        self.baseUrl = "http://localhost:3000/api/v1/"
        self.client = Client()
    
    def test_post_warehouses(self):
        new_warehouse = {
        "name": "New Warehouse",
        "location": "123 Warehouse Ave",
        "capacity": 1000
    }
        response = self.client.post(self.baseUrl + "warehouses", headers={"API_KEY":"a1b2c3d4e5"}, json=new_warehouse)
        self.assertEqual(response.status_code, 201)
        get_response = self.client.get(self.baseUrl + "warehouses", headers={"API_KEY": "a1b2c3d4e5"})
        assert get_response.status_code == 200, f"Wrong status code: {get_response.status_code}"
        warehouses = get_response.json()
        assert any(
            warehouse["name"] == new_warehouse["name"] and
            warehouse["location"] == new_warehouse["location"] and
            warehouse["capacity"] == new_warehouse["capacity"]
            for warehouse in warehouses
        ), "Not found"

if __name__ == '__main__':
    unittest.main()