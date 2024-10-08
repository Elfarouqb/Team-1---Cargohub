import pytest
import requests


@pytest.fixture
def _url():
    return 'http://localhost:3000/api/v1/'


def test_get_item_group(_url):
    url = _url + 'item_groups'
    params = {'id': 1}

    # Send a GET request to the API
    response = requests.get(url, params=params)

    # Get the status code and response data
    status_code = response.status_code
    # response_data = response.json()

    # Verify that the status code is 200 (OK)
    assert status_code == 200
