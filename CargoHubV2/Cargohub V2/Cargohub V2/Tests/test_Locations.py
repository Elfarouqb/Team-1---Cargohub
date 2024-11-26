import pytest
import requests


@pytest.fixture
def _url():
    return "http://localhost:3000/api/v1/"


def test_get_locations(_url):
    url = _url + 'locations'
    headers = {"API_KEY": "a1b2c3d4e5"}

    response = requests.get(url, headers=headers)
    assert response.status_code == 404


def test_get_location(_url):
    url = _url + 'locations/30'
    headers = {"API_KEY": "a1b2c3d4e5"}

    response = requests.get(url, headers=headers)
    assert response.status_code == 200


def test_post_locations(_url):
    url = _url + 'locations'
    headers = {"API_KEY": "a1b2c3d4e5"}

    response_data = requests.post(url, headers=headers).json()

    new_location = {
        "id": response_data[-1]["id"] + 1,
        "code": "A.1.0",
        "name": "naam",
        "created_at": "",
        "updated_at": ""
    }

    response = requests.post(url, headers=headers, json=new_location)
    assert response.status_code == 400


def test_delete_location(_url):
    location_id = 82
    url = _url + f'locations/{location_id}'
    headers = {"API_KEY": "a1b2c3d4e5"}

    location_data = requests.get(url, headers=headers)
    response = requests.delete(url, headers=headers)
    requests.post(url, headers=headers, json=location_data)

    assert response.status_code == 200


def test_delete_already_deleted_location(_url):
    location_id = 20
    url = _url + f'locations/{location_id}'
    headers = {"API_KEY": "a1b2c3d4e5"}
    response = requests.delete(url, headers=headers)
    response2 = requests.delete(url, headers=headers)
    assert response2.status_code == 404


def test_update_location(_url):
    id = 12
    url = _url + f'locations/{id}'
    headers = {"API_KEY": "a1b2c3d4e5"}

    updated_json_data = {
        "id": id,
        "code": "updated_code",
        "name": "updated_name",
        "created_at": "",
        "updated_at": ""
    }

    response = requests.put(url, headers=headers, json=updated_json_data)
    assert response.status_code == 200


def test_update_non_existent_location(_url):
    id = 190190920
    url = _url + f'locations/{id}'
    headers = {"API_KEY": "a1b2c3d4e5"}

    updated_json_data = {
        "id": id,
        "code": "updated_code",
        "name": "updated_name",
        "created_at": "",
        "updated_at": ""
    }

    response = requests.put(url, headers=headers, json=updated_json_data)
    assert response.status_code == 404
