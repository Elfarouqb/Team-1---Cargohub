import pytest
from httpx import Client
from datetime import datetime
import time

@pytest.fixture
def client():
    return Client()

@pytest.fixture
def base_url():
    return 'http://localhost:3000/api/v1/'

@pytest.fixture
def headers():
    return {'API_KEY': 'a1b2c3d4e5'}

def test_post_warehouse(client, base_url, headers):
    new_warehouse = {
        "id": 123456,
        "name": "Advanced Warehouse",
        "location": "789 Advanced Blvd",
        "capacity": 5000
    }

    response = client.post(f"{base_url}warehouses", headers=headers, json=new_warehouse)
    assert response.status_code == 201, f"Expected 201, got {response.status_code}"

    created_warehouse_response = client.get(f"{base_url}warehouses/{new_warehouse['id']}", headers=headers)
    print(f"POST response content: {created_warehouse_response.content}")
    assert created_warehouse_response.status_code == 201

    created_warehouse = created_warehouse_response.json()
    assert created_warehouse["id"] == new_warehouse["id"]
    assert created_warehouse["name"] == new_warehouse["name"]
    assert created_warehouse["location"] == new_warehouse["location"]
    assert created_warehouse["capacity"] == new_warehouse["capacity"]

    get_response = client.get(f"{base_url}warehouses", headers=headers)
    assert get_response.status_code == 200
    warehouses = get_response.json()
    assert any(wh["id"] == created_warehouse["id"] for wh in warehouses)

def test_get_single_warehouse(client, base_url, headers):
    valid_warehouse_id = 1  
    invalid_warehouse_id = 9999  

    valid_response = client.get(f"{base_url}warehouses/{valid_warehouse_id}", headers=headers)
    assert valid_response.status_code == 200

    warehouse = valid_response.json()
    assert warehouse["id"] == valid_warehouse_id
    assert "name" in warehouse
    assert "location" in warehouse
    assert "capacity" in warehouse

    invalid_response = client.get(f"{base_url}warehouses/{invalid_warehouse_id}", headers=headers)
    assert invalid_response.status_code == 404, "Invalid ID did not return a 404 status"

def test_update_warehouse(client, base_url, headers):
    warehouse_id = 1
    original_response = client.get(f"{base_url}warehouses/{warehouse_id}", headers=headers)
    original_warehouse = original_response.json()

    update_data = {
        "name": "Updated Advanced Warehouse",
        "location": "1000 Updated Blvd",
        "capacity": original_warehouse["capacity"] + 500
    }

    response = client.put(f"{base_url}warehouses/{warehouse_id}", headers=headers, json=update_data)
    assert response.status_code == 200, f"Expected 200, got {response.status_code}"

    updated_response = client.get(f"{base_url}warehouses/{warehouse_id}", headers=headers)
    updated_warehouse = updated_response.json()
    assert updated_warehouse["name"] == update_data["name"]
    assert updated_warehouse["location"] == update_data["location"]
    assert updated_warehouse["capacity"] == update_data["capacity"]

    assert updated_warehouse["created_at"] == original_warehouse["created_at"], "'created_at' was modified"

def test_delete_warehouse(client, base_url, headers):
    warehouse_to_delete = {
        "name": "Warehouse to Delete",
        "location": "999 Delete St",
        "capacity": 3000
    }

    create_response = client.post(f"{base_url}warehouses", headers=headers, json=warehouse_to_delete)
    assert create_response.status_code == 201
    created_warehouse = create_response.json()
    warehouse_id = created_warehouse["id"]

    delete_response = client.delete(f"{base_url}warehouses/{warehouse_id}", headers=headers)
    assert delete_response.status_code == 204, "Delete request did not return 204"

    get_response = client.get(f"{base_url}warehouses/{warehouse_id}", headers=headers)
    assert get_response.status_code == 404, "Deleted warehouse still accessible"

def test_updated_at_field(client, base_url, headers):
    warehouse_id = 1 

    initial_response = client.get(f"{base_url}warehouses/{warehouse_id}", headers=headers)
    assert initial_response.status_code == 200
    initial_updated_at = datetime.strptime(initial_response.json()["updated_at"], "%Y-%m-%d %H:%M:%S")

    time.sleep(2)

    update_data = {
        "name": "Timestamp Updated Warehouse",
        "location": "999 Timestamp Rd",
        "capacity": 5000
    }
    update_response = client.put(f"{base_url}warehouses/{warehouse_id}", headers=headers, json=update_data)
    assert update_response.status_code == 200

    updated_response = client.get(f"{base_url}warehouses/{warehouse_id}", headers=headers)
    assert updated_response.status_code == 200
    updated_updated_at = datetime.strptime(updated_response.json()["updated_at"], "%Y-%m-%d %H:%M:%S")

    assert updated_updated_at > initial_updated_at, "'updated_at' was not updated"

def test_warehouse_field_validation(client, base_url, headers):
    invalid_warehouse = {
        "name": "",  
        "location": "123 Invalid Blvd",
        "capacity": -100  
    }

    response = client.post(f"{base_url}warehouses", headers=headers, json=invalid_warehouse)
    assert response.status_code == 400, "should return a 400 status code"
    errors = response.json()
    assert "name" in errors, "'name' validation error missing"
    assert "capacity" in errors, "'capacity' validation error missing"

def test_warehouse_max_capacity(self):
    new_warehouse = {
        "name": "Max Capacity Warehouse",
        "location": "999 Max Blvd",
        "capacity": 999999999  # Intentionally set a very large capacity
    }
    
    response = self.client.post(self.baseUrl + "warehouses", headers=self.headers, json=new_warehouse)
    assert (response.status_code == 400, "Expected 400 for exceeding max capacity")
    errors = response.json()
    self.assertIn("capacity", errors, "'capacity' max validation error missing")

def test_warehouse_invalid_name_characters(self):
    new_warehouse = {
        "name": "Invalid@Name#!",
        "location": "123 Invalid St",
        "capacity": 1000
    }
    
    response = self.client.post(self.baseUrl + "warehouses", headers=self.headers, json=new_warehouse)
    assert (response.status_code == 400, "Expected 400 for invalid characters in name")
    errors = response.json()
    self.assertIn("name", errors, "'name' character validation error missing")