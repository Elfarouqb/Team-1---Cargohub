import datetime
import pytest
import requests


@pytest.fixture
def test_get_item_groups():
    url = 'http://localhost:3000/api/v1/item_groups'
    headers = {
        'API_KEY': 'a1b2c3d4e5'
    }

    response = requests.get(url, headers=headers)

    status_code = response.status_code

    assert status_code == 200


def test_get_item_group():
    url = 'http://localhost:3000/api/v1/item_groups/1'
    headers = {
        'API_KEY': 'a1b2c3d4e5'
    }

    response = requests.get(url, headers=headers)

    status_code = response.status_code

    assert status_code == 200


def test_add_item_group():
    url = 'http://localhost:3000/api/v1/item_groups'
    headers = {
        'API_KEY': 'a1b2c3d4e5',
    }

    new_item_group = {
        "name": "Johns stuff",
        "description": ""
    }

    response = requests.post(url, json=new_item_group, headers=headers)

    assert response.status_code == 201


"""    test = requests.get(url + "/" + str(new_item_group["id"]), headers=headers)

    assert test.status_code == 200
    test = test.json()
    assert test["name"] == new_item_group["name"]"""
"""
    created_datetime = datetime.strptime(created_at, "%Y-%m-%d %H:%M:%S")
    updated_datetime = datetime.strptime(updated_at, "%Y-%m-%d %H:%M:%S")

    created_at = test["created_at"]
    updated_at = test["updated_at"]

    assert created_at == created_datetime
    assert updated_at == updated_datetime"""


def test_update_item_group():
    url = 'http://localhost:3000/api/v1/item_groups/1'
    headers = {
        'API_KEY': 'a1b2c3d4e5',
    }

    updated_item_group = {
        "id": 1,
        "name": "New Electronics",
        "description": "",
        "created_at": "2023-05-15 19:52:53",
    }

    response = requests.put(url, json=updated_item_group, headers=headers)

    assert response.status_code == 200, f"{response.status_code}"

    test = requests.get(url, headers=headers)

    assert test.status_code == 200

    test = test.json()

    assert test["id"] == updated_item_group["id"]
    assert test["name"] == updated_item_group["name"]
    assert test["description"] == updated_item_group["description"]
    assert test["created_at"] == updated_item_group["created_at"]


def test_delete_item_group():
    url = 'http://localhost:3000/api/v1/item_groups/1'
    headers = {
        'API_KEY': 'a1b2c3d4e5',
    }

    response = requests.delete(url, headers=headers)

    assert response.status_code == 200
