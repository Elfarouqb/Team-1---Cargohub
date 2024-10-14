import unittest
from httpx import Client
from datetime import datetime
import time

class TestTransferApi(unittest.TestCase):
    def setUp(self):
        self.baseUrl = "http://localhost:3000/api/v1/"
        self.client = Client()
        self.headers = {"API_KEY": "a1b2c3d4e5"}

    def test_post_transfer(self):
        new_transfer = {
            "id": 1,
            "reference": "TR00001",
            "transfer_from": None,
            "transfer_to": 9229,
            "transfer_status": "Completed",
            "items": [
                {
                    "item_id": "P007435",
                    "amount": 23
                }
            ]
        }

        # Send POST request to create a new transfer
        response = self.client.post(self.baseUrl + "transfers", headers=self.headers, json=new_transfer)

        # Verify that the response status code is 201
        self.assertEqual(response.status_code, 201, f"Expected 201, got {response.status_code}")

        # Fetch the created transfer by ID
        created_transfer_response = self.client.get(self.baseUrl + "transfers/" + str(new_transfer["id"]), headers=self.headers)
        print(f"POST response content: {created_transfer_response.content}")

        # Verify that the response status code is 200
        self.assertEqual(created_transfer_response.status_code, 200)

        # Check that the details match what was sent
        created_transfer = created_transfer_response.json()
        self.assertEqual(created_transfer["id"], new_transfer["id"])
        self.assertEqual(created_transfer["reference"], new_transfer["reference"])
        self.assertEqual(created_transfer["transfer_status"], new_transfer["transfer_status"])

        # Verify that the transfer exists in the list of transfers
        get_response = self.client.get(self.baseUrl + "transfers", headers=self.headers)
        self.assertEqual(get_response.status_code, 200)
        transfers = get_response.json()
        self.assertTrue(any(tr["id"] == created_transfer["id"] for tr in transfers))

    def test_get_single_transfer(self):
        valid_transfer_id = 1
        invalid_transfer_id = 9999

        valid_response = self.client.get(f"{self.baseUrl}transfers/{valid_transfer_id}", headers=self.headers)
        self.assertEqual(valid_response.status_code, 200)

        transfer = valid_response.json()
        self.assertEqual(transfer["id"], valid_transfer_id)
        self.assertIn("reference", transfer)
        self.assertIn("transfer_status", transfer)

        invalid_response = self.client.get(f"{self.baseUrl}transfers/{invalid_transfer_id}", headers=self.headers)
        self.assertEqual(invalid_response.status_code, 404, "Invalid ID did not return a 404 status")

    def test_update_transfer(self):
        transfer_id = 1
        original_response = self.client.get(f"{self.baseUrl}transfers/{transfer_id}", headers=self.headers)
        original_transfer = original_response.json()

        update_data = {
            "reference": "TR00002",
            "transfer_status": "In Progress",
        }

        response = self.client.put(f"{self.baseUrl}transfers/{transfer_id}", headers=self.headers, json=update_data)
        self.assertEqual(response.status_code, 200, f"Expected 200, got {response.status_code}")

        updated_response = self.client.get(f"{self.baseUrl}transfers/{transfer_id}", headers=self.headers)
        updated_transfer = updated_response.json()
        self.assertEqual(updated_transfer["reference"], update_data["reference"])
        self.assertEqual(updated_transfer["transfer_status"], update_data["transfer_status"])

        self.assertEqual(updated_transfer["created_at"], original_transfer["created_at"], "'created_at' was modified")

    def test_delete_transfer(self):
        transfer_to_delete = {
            "reference": "TR00003",
            "transfer_from": 1111,
            "transfer_to": 2222,
            "transfer_status": "Pending",
            "items": [
                {
                    "item_id": "P007436",
                    "amount": 5
                }
            ]
        }

        create_response = self.client.post(self.baseUrl + "transfers", headers=self.headers, json=transfer_to_delete)
        self.assertEqual(create_response.status_code, 201)
        created_transfer = create_response.json()
        transfer_id = created_transfer["id"]

        delete_response = self.client.delete(f"{self.baseUrl}transfers/{transfer_id}", headers=self.headers)
        self.assertEqual(delete_response.status_code, 204, "Delete request did not return 204")

        get_response = self.client.get(f"{self.baseUrl}transfers/{transfer_id}", headers=self.headers)
        self.assertEqual(get_response.status_code, 404, "Deleted transfer still accessible")

    def test_transfer_field_validation(self):
        invalid_transfer = {
            "reference": "",  # Empty reference
            "transfer_status": "Completed",
            "items": []
        }

        response = self.client.post(self.baseUrl + "transfers", headers=self.headers, json=invalid_transfer)
        self.assertEqual(response.status_code, 400, "should return a 400 status code")
        errors = response.json()
        self.assertIn("reference", errors, "'reference' validation error missing")


if __name__ == '__main__':
    unittest.main()