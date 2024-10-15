import unittest
from httpx import Client
from datetime import datetime
import time

class TestSupplierApi(unittest.TestCase):
    def setUp(self):
        self.baseUrl = "http://localhost:3000/api/v1/"
        self.client = Client()
        self.headers = {"API_KEY": "a1b2c3d4e5"}

    def test_post_supplier(self):
        new_supplier = {
            "id": 1,
            "code": "SUP0001",
            "name": "Lee, Parks and Johnson",
            "address": "5989 Sullivan Drives",
            "address_extra": "Apt. 996",
            "city": "Port Anitaburgh",
            "zip_code": "91688",
            "province": "Illinois",
            "country": "Czech Republic",
            "contact_name": "Toni Barnett",
            "phonenumber": "363.541.7282x36825",
            "reference": "LPaJ-SUP0001",
        }

        response = self.client.post(self.baseUrl + "suppliers", headers=self.headers, json=new_supplier)

        self.assertEqual(response.status_code, 201, f"Expected 201, got {response.status_code}")

        created_supplier_response = self.client.get(self.baseUrl + "suppliers/" + str(new_supplier["id"]), headers=self.headers)
        print(f"POST response content: {created_supplier_response.content}")


        self.assertEqual(created_supplier_response.status_code, 200)

        created_supplier = created_supplier_response.json()
        self.assertEqual(created_supplier["id"], new_supplier["id"])
        self.assertEqual(created_supplier["code"], new_supplier["code"])
        self.assertEqual(created_supplier["name"], new_supplier["name"])
        self.assertEqual(created_supplier["address"], new_supplier["address"])

        get_response = self.client.get(self.baseUrl + "suppliers", headers=self.headers)
        self.assertEqual(get_response.status_code, 200)
        suppliers = get_response.json()
        self.assertTrue(any(sup["id"] == created_supplier["id"] for sup in suppliers))

    def test_get_single_supplier(self):
        valid_supplier_id = 1
        invalid_supplier_id = 9999

        valid_response = self.client.get(f"{self.baseUrl}suppliers/{valid_supplier_id}", headers=self.headers)
        self.assertEqual(valid_response.status_code, 200)

        supplier = valid_response.json()
        self.assertEqual(supplier["id"], valid_supplier_id)
        self.assertIn("name", supplier)
        self.assertIn("address", supplier)

        invalid_response = self.client.get(f"{self.baseUrl}suppliers/{invalid_supplier_id}", headers=self.headers)
        self.assertEqual(invalid_response.status_code, 404, "Invalid ID did not return a 404 status")

    def test_update_supplier(self):
        supplier_id = 1
        original_response = self.client.get(f"{self.baseUrl}suppliers/{supplier_id}", headers=self.headers)
        original_supplier = original_response.json()

        update_data = {
            "name": "Updated Supplier Name",
            "address": "Updated Address",
        }

        response = self.client.put(f"{self.baseUrl}suppliers/{supplier_id}", headers=self.headers, json=update_data)
        self.assertEqual(response.status_code, 200, f"Expected 200, got {response.status_code}")

        updated_response = self.client.get(f"{self.baseUrl}suppliers/{supplier_id}", headers=self.headers)
        updated_supplier = updated_response.json()
        self.assertEqual(updated_supplier["name"], update_data["name"])
        self.assertEqual(updated_supplier["address"], update_data["address"])

        self.assertEqual(updated_supplier["created_at"], original_supplier["created_at"], "'created_at' was modified")

    def test_delete_supplier(self):
        supplier_to_delete = {
            "code": "SUP0004",
            "name": "Supplier to Delete",
            "address": "123 Delete St",
            "city": "Delete City",
            "zip_code": "00000",
            "province": "Delete State",
            "country": "Delete Country",
            "contact_name": "Delete Contact",
            "phonenumber": "123-456-7890",
            "reference": "D-SUP0004",
        }

        create_response = self.client.post(self.baseUrl + "suppliers", headers=self.headers, json=supplier_to_delete)
        self.assertEqual(create_response.status_code, 201)
        created_supplier = create_response.json()
        supplier_id = created_supplier["id"]

        delete_response = self.client.delete(f"{self.baseUrl}suppliers/{supplier_id}", headers=self.headers)
        self.assertEqual(delete_response.status_code, 204, "Delete request did not return 204")

        get_response = self.client.get(f"{self.baseUrl}suppliers/{supplier_id}", headers=self.headers)
        self.assertEqual(get_response.status_code, 404, "Deleted supplier still accessible")

    def test_supplier_field_validation(self):
        invalid_supplier = {
            "name": "",
            "address": "123 Invalid St",
            "city": "Invalid City",
            "zip_code": "Invalid Zip",
            "province": "Invalid State",
            "country": "Invalid Country",
            "contact_name": "Invalid Contact",
            "phonenumber": "Invalid Phone",
            "code": "SUP0005",
        }

        response = self.client.post(self.baseUrl + "suppliers", headers=self.headers, json=invalid_supplier)
        self.assertEqual(response.status_code, 400, "should return a 400 status code")
        errors = response.json()
        self.assertIn("name", errors, "'name' validation error missing")

if __name__ == '__main__':
    unittest.main()