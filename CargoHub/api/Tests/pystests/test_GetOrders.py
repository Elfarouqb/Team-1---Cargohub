import pytest
import requests


@pytest.fixture
def _url():
    return 'http://localhost:3000/api/v1/'


def test_get_orders(_url):
    url = f"{_url}orders"
    headers = {
        'API_KEY': 'a1b2c3d4e5'
    }

    response = requests.get(url, headers=headers)

    response.raise_for_status()
    status_code = response.status_code

    assert status_code == 200


def test_get_order(_url):
    order_id = 1
    url = f"{_url}orders/{order_id}"

    headers = {
        'API_KEY': 'a1b2c3d4e5'
    }

    response = requests.get(url, headers=headers)

    response.raise_for_status()

    status_code = response.status_code
    response_data = response.json()

    assert status_code == 200

    assert 'id' in response_data  # Check if id is in the response json
    assert response_data['id'] == order_id  # Check if the returned id matches the requested id


def test_get_orders_in_shipment(_url):
    shipment_id = 1
    url = f"{_url}shipments/{shipment_id}/orders"

    headers = {
        'API_KEY': 'a1b2c3d4e5'
    }

    response = requests.get(url, headers=headers)

    response.raise_for_status()

    status_code = response.status_code
    response_data = response.json()

    assert status_code == 200
    assert isinstance(response_data, list)
    assert all('id' in order for order in response_data)  # Each order should have an 'id' field


def test_get_orders_for_client(_url):
    client_id = 123
    url = f"{_url}clients/{client_id}/orders"

    headers = {
        'API_KEY': 'a1b2c3d4e5'
    }

    response = requests.get(url, headers=headers)

    response.raise_for_status()

    status_code = response.status_code
    response_data = response.json()

    assert status_code == 200
    assert isinstance(response_data, list)  # Response should be a list
    assert all('id' in order for order in response_data)  # Each order should have an 'id' field


def test_remove_order(_url):
    order_id = 1
    url = f"{_url}orders/{order_id}"

    headers = {
        'API_KEY': 'a1b2c3d4e5'
    }

    response = requests.delete(url, headers=headers)

    response.raise_for_status()

    status_code = response.status_code

    assert status_code == 204  # 204 No Content

    # Verify the order no longer exists
    get_response = requests.get(url, headers=headers)
    assert get_response.status_code == 404
