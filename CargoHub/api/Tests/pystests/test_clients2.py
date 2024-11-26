import pytest
import requests

  
@pytest.fixture
def _url():
    return 'http://localhost:3000/api/v1/'


def test_get_clients(_url):
    url = _url + 'clients'
    headers = {"API_KEY": "a1b2c3d4e5"}

    response = requests.get(url, headers=headers)
    status_code = response.status_code 
    assert status_code == 200 


def test_get_client(_url):
    url = _url + 'clients/-8'
    headers = {"API_KEY": "a1b2c3d4e5"}

    response = requests.get(url, headers=headers)
    status_code = response.status_code

    assert status_code == 500



def test_add_client(_url):
    url = _url + 'clients'
    new_client = {
        "name": "test_name",
        "address": "test_address",
        "city": "test_city",
        "zip_code": "test_zip_code",
        "province": "test_province",
        "country": "test_country",
        "contact_name": "test_contact_name",
        "contact_phone": "test_contact_phone",
        "contact_email": "test_contact_email"
    }
    headers = {"API_KEY": "a1b2c3d4e5"}

    response = requests.post(url, json=new_client, headers=headers)
    status_code = response.status_code

    assert status_code == 201



def test_remove_client(_url):
    client_id = 90
    url = _url + f"clients/{client_id}"
    headers = {"API_KEY": "a1b2c3d4e5"}

    client_data = requests.get(url, headers=headers).text

    response = requests.delete(url, headers=headers)
    status_code = response.status_code
    requests.post(url, json=client_data, headers=headers)
     
    assert status_code == 200
   

