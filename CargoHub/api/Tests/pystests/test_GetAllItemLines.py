import pytest
from httpx import Client

@pytest.fixture
def client():
    return Client()

@pytest.fixture
def _url():
    return 'http://localhost:3000/api/v1/item_lines'


def test_get_all_item_lines(_url, client):
    headers = {
        'API_KEY': 'a1b2c3d4e5'
    }
    _url = 'http://localhost:3000/api/v1/item_lines'

    response = client.get(_url, headers=headers)

    status_code = response.status_code

    assert status_code == 200