import pytest
import requests


@pytest.fixture
def _url():
    return 'http://localhost:3000/api/v1/'  # Base URL


def test_get_orders(_url):
    url = f"{_url}orders"
    headers = {
        'API_KEY': 'a1b2c3d4e5'  # Add the API key to the headers
    }

    response = requests.get(url, headers=headers)

    response.raise_for_status()
    status_code = response.status_code
    response_data = response.json()  # Get the response data as JSON

    # Verify that the status code is 200 (OK)
    assert status_code == 200


def test_get_order(_url):
    order_id = 1  # The ID of the order
    url = f"{_url}orders/{order_id}"  # full URL for order 1

    headers = {
        'API_KEY': 'a1b2c3d4e5'  # Add the API key to the headers
    }

    # Send a GET request to the API
    response = requests.get(url, headers=headers)

    # Raise an error for bad responses (4xx or 5xx)
    response.raise_for_status()

    # Get the status code and response data
    status_code = response.status_code
    response_data = response.json()  # Get the response data as JSON

    # Verify that the status code is 200 (OK)
    assert status_code == 200

    # verify that the response data is as expected
    assert 'id' in response_data  # Check if 'id' is in the response
    assert response_data['id'] == order_id  # Check if the returned id matches the requested id
