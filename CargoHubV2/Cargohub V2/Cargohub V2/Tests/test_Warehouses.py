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


def create_warehouse(client, base_url, headers, warehouse_data):
    response = client.post(f"{base_url}warehouses", headers=headers, json=warehouse_data)
    assert response.status_code == 201, f"Expected 201, got {response.status_code}"
    return warehouse_data["id"]


def get_warehouse(client, base_url, headers, warehouse_id):
    response = client.get(f"{base_url}warehouses/{warehouse_id}", headers=headers)
    assert response.status_code == 200, f"Expected 200, got {response.status_code}"
    return response.json()


def update_warehouse(client, base_url, headers, warehouse_id, update_data):
    response = client.put(f"{base_url}warehouses/{warehouse_id}", headers=headers, json=update_data)
    assert response.status_code == 200, f"Expected 200, got {response.status_code}"
    return get_warehouse(client, base_url, headers, warehouse_id)


def delete_warehouse(client, base_url, headers, warehouse_id):
    response = client.delete(f"{base_url}warehouses/{warehouse_id}", headers=headers)
    assert response.status_code == 204, "Delete request did not return 204"


def test_post_warehouse(client, base_url, headers):
    new_warehouse = {
        "id": 123456,
        "name": "Advanced Warehouse",
        "location": "789 Advanced Blvd",
        "capacity": 5000
    }

    warehouse_id = create_warehouse(client, base_url, headers, new_warehouse)
    created_warehouse = get_warehouse(client, base_url, headers, warehouse_id)
    assert created_warehouse["name"] == new_warehouse["name"]
    assert created_warehouse["location"] == new_warehouse["location"]
    assert created_warehouse["capacity"] == new_warehouse["capacity"]

    delete_warehouse(client, base_url, headers, warehouse_id)


def test_get_single_warehouse(client, base_url, headers):
    warehouse_id = 1
    warehouse = get_warehouse(client, base_url, headers, warehouse_id)
    assert warehouse["id"] == warehouse_id


def test_update_warehouse(client, base_url, headers):
    warehouse_id = 1
    original_warehouse = get_warehouse(client, base_url, headers, warehouse_id)

    update_data = {
        "id": 1,
        "name": "Updated Advanced Warehouse",
        "location": "1000 Updated Blvd",
        "capacity": original_warehouse["capacity"] + 500
    }

    updated_warehouse = update_warehouse(client, base_url, headers, warehouse_id, update_data)
    assert updated_warehouse["id"] == warehouse_id
    assert updated_warehouse["name"] == update_data["name"]
    assert updated_warehouse["location"] == update_data["location"]
    assert updated_warehouse["capacity"] == update_data["capacity"]
    assert updated_warehouse["created_at"] == original_warehouse["created_at"], "'created_at' was modified"


def test_delete_warehouse(client, base_url, headers):
    warehouse_to_delete = {
        "id": 654321,  # Ensure the ID is unique
        "name": "Warehouse to Delete",
        "location": "999 Delete St",
        "capacity": 3000
    }

    # Create the warehouse to delete
    warehouse_id = create_warehouse(client, base_url, headers, warehouse_to_delete)

    # Now delete the warehouse
    delete_warehouse(client, base_url, headers, warehouse_id)

    # Optionally, try to fetch the warehouse after deletion to confirm it no longer exists
    fetch_response = client.get(f"{base_url}warehouses/{warehouse_id}", headers=headers)
    assert fetch_response.status_code == 404, "Expected 404 after deletion, but got a different status."


def test_updated_at_field(client, base_url, headers):
    warehouse_id = 1

    initial_response = get_warehouse(client, base_url, headers, warehouse_id)
    initial_updated_at = datetime.strptime(initial_response["updated_at"], "%Y-%m-%d %H:%M:%S")

    time.sleep(2)

    update_data = {
        "id": 1,
        "name": "Timestamp Updated Warehouse",
        "location": "999 Timestamp Rd",
        "capacity": 5000
    }
    update_warehouse(client, base_url, headers, warehouse_id, update_data)

    updated_response = get_warehouse(client, base_url, headers, warehouse_id)
    updated_updated_at = datetime.strptime(updated_response["updated_at"], "%Y-%m-%d %H:%M:%S")

    assert updated_updated_at > initial_updated_at, "'updated_at' was not updated"


def test_warehouse_field_validation(client, base_url, headers):
    invalid_warehouse = {
        "id": 789012,
        "name": "",
        "location": "123 Invalid Blvd",
        "capacity": -100
    }

    response = client.post(f"{base_url}warehouses", headers=headers, json=invalid_warehouse)
    assert response.status_code == 400, "should return a 400 status code"


def test_warehouse_max_capacity(client, base_url, headers):
    new_warehouse = {
        "id": 210987,
        "name": "Max Capacity Warehouse",
        "location": "999 Max Blvd",
        "capacity": 999999999
    }

    response = client.post(f"{base_url}warehouses", headers=headers, json=new_warehouse)
    assert response.status_code == 400, "Expected 400 for exceeding max capacity"


def test_warehouse_invalid_name_characters(client, base_url, headers):
    new_warehouse = {
        "id": 345678,
        "name": "Invalid@Name#!",
        "location": "123 Invalid St",
        "capacity": 1000
    }

    response = client.post(f"{base_url}warehouses", headers=headers, json=new_warehouse)
    assert response.status_code == 400, "Expected 400 for invalid characters in name"
