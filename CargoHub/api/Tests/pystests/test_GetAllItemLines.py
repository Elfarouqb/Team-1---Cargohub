import pytest
import requests


@pytest.fixture
def _url():
    return 'http://localhost:3000/api/v1/item_lines'


def test_get_all_item_lines(_url):
    url = _url
    headers = {
        'API_KEY': 'a1b2c3d4e5'
    }

    response = requests.get(url, headers=headers)

    status_code = response.status_code

    assert status_code == 200
