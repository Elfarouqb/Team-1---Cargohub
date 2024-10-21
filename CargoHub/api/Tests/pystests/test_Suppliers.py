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

def test_post_supplier(client, base_url, headers):
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

    response = client.post(f"{base_url}suppliers", headers=headers, json=new_supplier)
    assert response.status_code == 201, f"Expected 201, got {response.status_code}"

    created_supplier_response = client.get(f"{base_url}suppliers/{new_supplier['id']}", headers=headers)
    print(f"POST response content: {created_supplier_response.content}")
    assert created_supplier_response.status_code == 200

    created_supplier = created_supplier_response.json()
    assert created_supplier["id"] == new_supplier["id"]
    assert created_supplier["code"] == new_supplier["code"]
    assert created_supplier["name"] == new_supplier["name"]
    assert created_supplier["address"] == new_supplier["address"]

    get_response = client.get(f"{base_url}suppliers", headers=headers)
    assert get_response.status_code == 200
    suppliers = get_response.json()
    assert any(sup["id"] == created_supplier["id"] for sup in suppliers)

def test_get_single_supplier(client, base_url, headers):
    valid_supplier_id = 1
    invalid_supplier_id = 9999

    valid_response = client.get(f"{base_url}suppliers/{valid_supplier_id}", headers=headers)
    assert valid_response.status_code == 200

    supplier = valid_response.json()
    assert supplier["id"] == valid_supplier_id
    assert "name" in supplier
    assert "address" in supplier

    invalid_response = client.get(f"{base_url}suppliers/{invalid_supplier_id}", headers=headers)
    assert invalid_response.status_code == 404, "Invalid ID did not return a 404 status"

def test_update_supplier(client, base_url, headers):
    supplier_id = 1
    original_response = client.get(f"{base_url}suppliers/{supplier_id}", headers=headers)
    original_supplier = original_response.json()

    update_data = {
        "name": "Updated Supplier Name",
        "address": "Updated Address",
    }

    response = client.put(f"{base_url}suppliers/{supplier_id}", headers=headers, json=update_data)
    assert response.status_code == 200, f"Expected 200, got {response.status_code}"

    updated_response = client.get(f"{base_url}suppliers/{supplier_id}", headers=headers)
    updated_supplier = updated_response.json()
    assert updated_supplier["name"] == update_data["name"]
    assert updated_supplier["address"] == update_data["address"]
    assert updated_supplier["created_at"] == original_supplier["created_at"], "'created_at' was modified"

def test_delete_supplier(client, base_url, headers):
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

    create_response = client.post(f"{base_url}suppliers", headers=headers, json=supplier_to_delete)
    assert create_response.status_code == 201
    created_supplier = create_response.json()
    supplier_id = created_supplier["id"]

    delete_response = client.delete(f"{base_url}suppliers/{supplier_id}", headers=headers)
    assert delete_response.status_code == 204, "Delete request did not return 204"

    get_response = client.get(f"{base_url}suppliers/{supplier_id}", headers=headers)
    assert get_response.status_code == 404, "Deleted supplier still accessible"

def test_supplier_field_validation(client, base_url, headers):
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

    response = client.post(f"{base_url}suppliers", headers=headers, json=invalid_supplier)
    assert response.status_code == 400, "should return a 400 status code"
    errors = response.json()
    assert "name" in errors, "'name' validation error missing"

def test_supplier_duplicate_name(client, base_url, headers):
    existing_supplier = {
        "name": "Unique Supplier",
        "address": "123 Unique St",
        "city": "Unique City",
        "zip_code": "11111",
        "province": "Unique Province",
        "country": "Unique Country",
        "contact_name": "Unique Contact",
        "phonenumber": "123-456-7890",
        "code": "SUP0010"
    }

    # Create the first supplier
    create_response = client.post(f"{base_url}suppliers", headers=headers, json=existing_supplier)
    assert create_response.status_code == 201, f"Expected 201, got {create_response.status_code}"

    # Attempt to create a second supplier with the same name
    duplicate_supplier = {
        "name": "Unique Supplier",  # Same name as the previous supplier
        "address": "456 Another St",
        "city": "Another City",
        "zip_code": "22222",
        "province": "Another Province",
        "country": "Another Country",
        "contact_name": "Another Contact",
        "phonenumber": "987-654-3210",
        "code": "SUP0011"
    }

    response = client.post(f"{base_url}suppliers", headers=headers, json=duplicate_supplier)
    assert response.status_code == 400, f"Expected 400 for duplicate supplier name, got {response.status_code}"

    errors = response.json()
    assert "name" in errors, "'name' field is not unique"

def test_supplier_invalid_email(client, base_url, headers):
    invalid_supplier = {
        "name": "Invalid Email Supplier",
        "address": "123 Invalid Email St",
        "city": "Email City",
        "zip_code": "33333",
        "province": "Email Province",
        "country": "Email Country",
        "contact_name": "Email Contact",
        "phonenumber": "555-555-5555",
        "code": "SUP0012",
        "contact_email": "invalid-email-format"  # Invalid email format
    }

    response = client.post(f"{base_url}suppliers", headers=headers, json=invalid_supplier)

    # Ensure that the server returns a 400 Bad Request due to invalid email format
    assert response.status_code == 400, f"Expected 400 for invalid email format, got {response.status_code}"

    # Check if the validation error is returned for the email field
    errors = response.json()
    assert "contact_email" in errors, "'contact_email' validation error missing"
    assert "invalid" in errors["contact_email"], "Expected 'invalid' error for invalid email format"