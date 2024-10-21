import unittest
from httpx import Client
from datetime import datetime
import time

class TestWarehouseApi(unittest.TestCase):
    def setUp(self):
        self.baseUrl = 'http://localhost:3000/api/v1/'
        self.headers = {'API_KEY': 'a1b2c3d4e5'}
        self.client = Client()
    
    def test_post_warehouse(self):
        new_warehouse = {
            "id": 123456,
            "name": "Advanced Warehouse",
            "location": "789 Advanced Blvd",
            "capacity": 5000
        }

        response = self.client.post(self.baseUrl + "warehouses", headers=self.headers, json=new_warehouse)

        self.assertEqual(response.status_code, 201, f"Expected 201, got {response.status_code}")

        created_warehouse_response = self.client.get(self.baseUrl + "warehouses/" + str(new_warehouse["id"]), headers=self.headers)
        print(f"POST response content: {created_warehouse_response.content}")

        self.assertEqual(created_warehouse_response.status_code, 201)

        created_warehouse = created_warehouse_response.json()
        self.assertEqual(created_warehouse["id"], new_warehouse["id"])
        self.assertEqual(created_warehouse["name"], new_warehouse["name"])
        self.assertEqual(created_warehouse["location"], new_warehouse["location"])
        self.assertEqual(created_warehouse["capacity"], new_warehouse["capacity"])

        get_response = self.client.get(self.baseUrl + "warehouses", headers=self.headers)
        self.assertEqual(get_response.status_code, 200)
        warehouses = get_response.json()
        self.assertTrue(any(wh["id"] == created_warehouse["id"] for wh in warehouses))
        
    def test_get_single_warehouse(self):
        valid_warehouse_id = 1  
        invalid_warehouse_id = 9999  

        valid_response = self.client.get(f"{self.baseUrl}warehouses/{valid_warehouse_id}", headers=self.headers)
        self.assertEqual(valid_response.status_code, 200)
        
        warehouse = valid_response.json()
        self.assertEqual(warehouse["id"], valid_warehouse_id)
        self.assertIn("name", warehouse)
        self.assertIn("location", warehouse)
        self.assertIn("capacity", warehouse)
        
        invalid_response = self.client.get(f"{self.baseUrl}warehouses/{invalid_warehouse_id}", headers=self.headers)
        self.assertEqual(invalid_response.status_code, 404, "Invalid ID did not return a 404 status")
    
    def test_update_warehouse(self):
        warehouse_id = 1
        original_response = self.client.get(f"{self.baseUrl}warehouses/{warehouse_id}", headers=self.headers)
        original_warehouse = original_response.json()

        update_data = {
            "name": "Updated Advanced Warehouse",
            "location": "1000 Updated Blvd",
            "capacity": original_warehouse["capacity"] + 500
        }
        
        response = self.client.put(f"{self.baseUrl}warehouses/{warehouse_id}", headers=self.headers, data=update_data)
        self.assertEqual(response.status_code, 200, f"Expected 200, got {response.status_code}")
        
        updated_response = self.client.get(f"{self.baseUrl}warehouses/{warehouse_id}", headers=self.headers)
        updated_warehouse = updated_response.json()
        self.assertEqual(updated_warehouse["name"], update_data["name"])
        self.assertEqual(updated_warehouse["location"], update_data["location"])
        self.assertEqual(updated_warehouse["capacity"], update_data["capacity"])
        
        self.assertEqual(updated_warehouse["created_at"], original_warehouse["created_at"], "'created_at' was modified")
    
    def test_delete_warehouse(self):
        warehouse_to_delete = {
            "name": "Warehouse to Delete",
            "location": "999 Delete St",
            "capacity": 3000
        }
        
        create_response = self.client.post(self.baseUrl + "warehouses", headers=self.headers, json=warehouse_to_delete)
        self.assertEqual(create_response.status_code, 201)
        created_warehouse = create_response.json()
        warehouse_id = created_warehouse["id"]
        
        delete_response = self.client.delete(f"{self.baseUrl}warehouses/{warehouse_id}", headers=self.headers)
        self.assertEqual(delete_response.status_code, 204, "Delete request did not return 204")

        get_response = self.client.get(f"{self.baseUrl}warehouses/{warehouse_id}", headers=self.headers)
        self.assertEqual(get_response.status_code, 404, "Deleted warehouse still accessible")
    
    def test_updated_at_field(self):
        warehouse_id = 1 
        
        initial_response = self.client.get(f"{self.baseUrl}warehouses/{warehouse_id}", headers=self.headers)
        self.assertEqual(initial_response.status_code, 200)
        initial_updated_at = datetime.strptime(initial_response.json()["updated_at"], "%Y-%m-%d %H:%M:%S")
        
        time.sleep(2)
        
        update_data = {
            "name": "Timestamp Updated Warehouse",
            "location": "999 Timestamp Rd",
            "capacity": 5000
        }
        update_response = self.client.put(f"{self.baseUrl}warehouses/{warehouse_id}", headers=self.headers, json=update_data)
        self.assertEqual(update_response.status_code, 200)
        
        updated_response = self.client.get(f"{self.baseUrl}warehouses/{warehouse_id}", headers=self.headers)
        self.assertEqual(updated_response.status_code, 200)
        updated_updated_at = datetime.strptime(updated_response.json()["updated_at"], "%Y-%m-%d %H:%M:%S")
        
        self.assertGreater(updated_updated_at, initial_updated_at, "'updated_at' was not updated")

    def test_warehouse_field_validation(self):
        invalid_warehouse = {
            "name": "",  
            "location": "123 Iddeasdfaeswdfsafasrfrefgarefg zzfdgsdrzgnvalid Blvd",
            "capacity": -100  
        }
        
        response = self.client.post(self.baseUrl + "warehouses", headers=self.headers, json=invalid_warehouse)
        self.assertEqual(response.status_code, 400, "should return a 400 status code")
        errors = response.json()
        self.assertIn("name", errors, "'name' validation error missing")
        self.assertIn("capacity", errors, "'capacity' validation error missing")

if __name__ == '__main__':
    unittest.main()