import pytest
import requests


@pytest.fixture
def _url():
    return 'http://localhost:3000/api/v1/item_groups/1'


def test_get_item_group(_url):
    url = _url
    headers = {
        'API_KEY': 'a1b2c3d4e5'  # Ensure this API key is valid and has access to the endpoint
    }

    # Send a GET request to the API
    response = requests.get(url, headers=headers)

    # Get the status code
    status_code = response.status_code

    # Check if the status code is 200 (OK)
    assert status_code == 200, f"Unexpected status code: {status_code}. Response body: {response.text}"
