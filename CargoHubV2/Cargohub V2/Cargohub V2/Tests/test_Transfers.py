import pytest
from httpx import Client


@pytest.fixture
def client():
    return Client()


@pytest.fixture
def base_url():
    return "http://localhost:3000/api/v1/"


@pytest.fixture
def headers():
    return {"API_KEY": "a1b2c3d4e5"}


def test_post_transfer(client, base_url, headers):
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

    response = client.post(f"{base_url}transfers", headers=headers, json=new_transfer)
    assert response.status_code == 201, f"Expected 201, got {response.status_code}"

    created_transfer_response = client.get(f"{base_url}transfers/{new_transfer['id']}", headers=headers)
    print(f"POST response content: {created_transfer_response.content}")
    assert created_transfer_response.status_code == 200

    created_transfer = created_transfer_response.json()
    assert created_transfer["id"] == new_transfer["id"]
    assert created_transfer["reference"] == new_transfer["reference"]
    assert created_transfer["transfer_status"] == new_transfer["transfer_status"]

    get_response = client.get(f"{base_url}transfers", headers=headers)
    assert get_response.status_code == 200
    transfers = get_response.json()
    assert any(tr["id"] == created_transfer["id"] for tr in transfers)


def test_get_single_transfer(client, base_url, headers):
    valid_transfer_id = 1
    invalid_transfer_id = 9999

    valid_response = client.get(f"{base_url}transfers/{valid_transfer_id}", headers=headers)
    assert valid_response.status_code == 200

    transfer = valid_response.json()
    assert transfer["id"] == valid_transfer_id
    assert "reference" in transfer
    assert "transfer_status" in transfer

    invalid_response = client.get(f"{base_url}transfers/{invalid_transfer_id}", headers=headers)
    assert invalid_response.status_code == 404, "Invalid ID did not return a 404 status"


def test_update_transfer(client, base_url, headers):
    transfer_id = 1
    original_response = client.get(f"{base_url}transfers/{transfer_id}", headers=headers)
    original_transfer = original_response.json()

    update_data = {
        "reference": "TR00002",
        "transfer_status": "In Progress",
    }

    response = client.put(f"{base_url}transfers/{transfer_id}", headers=headers, json=update_data)
    assert response.status_code == 200, f"Expected 200, got {response.status_code}"

    updated_response = client.get(f"{base_url}transfers/{transfer_id}", headers=headers)
    updated_transfer = updated_response.json()
    assert updated_transfer["reference"] == update_data["reference"]
    assert updated_transfer["transfer_status"] == update_data["transfer_status"]
    assert updated_transfer["created_at"] == original_transfer["created_at"], "'created_at' was modified"


def test_delete_transfer(client, base_url, headers):
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

    create_response = client.post(f"{base_url}transfers", headers=headers, json=transfer_to_delete)
    assert create_response.status_code == 201
    created_transfer = create_response.json()
    transfer_id = created_transfer["id"]

    delete_response = client.delete(f"{base_url}transfers/{transfer_id}", headers=headers)
    assert delete_response.status_code == 204, "Delete request did not return 204"

    get_response = client.get(f"{base_url}transfers/{transfer_id}", headers=headers)
    assert get_response.status_code == 404, "Deleted transfer still accessible"


def test_transfer_field_validation(client, base_url, headers):
    invalid_transfer = {
        "reference": "",
        "transfer_status": "Completed",
        "items": []
    }

    response = client.post(f"{base_url}transfers", headers=headers, json=invalid_transfer)
    assert response.status_code == 400, "should return a 400 status code"
    errors = response.json()
    assert "reference" in errors, "'reference' validation error missing"
