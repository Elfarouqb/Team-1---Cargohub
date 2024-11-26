import pytest
import requests


@pytest.fixture
def _url():
    return "http://localhost:3000/api/v1/"


def test_get_inventories(_url):
    url = _url + 'inventories'
    headers = {"API_KEY": "a1b2c3d4e5"}

    response = requests.get(url, headers=headers)
    assert response.status_code == 200


def test_get_inventory(_url):
    url = _url + 'inventories/3'
    headers = {"API_KEY": "a1b2c3d4e5"}

    response = requests.get(url, headers=headers)
    assert response.status_code == 200


def test_add_inventory(_url):
    url = _url + 'inventories'
    new_inventory = {
        "id": 2,
        "item_id": "P000002",
        "description": "Focused transitional alliance",
        "item_reference": "nyg48736S",
        "locations": [20477, 20524, 22717],
        "total_on_hand": 190,
        "total_expected": 0,
        "total_ordered": 140,
        "total_allocated": 0,
        "total_available": 56,
        "created_at": "2020-05-31 16:00:08",
        "updated_at": "2020-11-08 12:49:21"
    }
    headers = {"API_KEY": "a1b2c3d4e5"}

    response = requests.post(url, json=new_inventory, headers=headers)
    assert response.status_code == 201


def test_get_inventory_by_id(_url):
    url = _url + "inventories/13"
    headers = {"API_KEY": "a1b2c3d4e5"}

    response = requests.get(url, headers=headers)
    return len(response.json())


def test_remove_inventory(_url):
    url = _url + "inventories/70"
    headers = {"API_KEY": "a1b2c3d4e5"}

    response = requests.delete(url, headers=headers)
    assert response.status_code == 200
