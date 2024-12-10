import pytest
import requests
import responses


MOCK_ORDERS = [
    {
        "id": 1,
        "source_id": 33,
        "order_date": "2023-10-01T12:30:00Z",
        "request_date": "2023-10-05T15:00:00Z",
        "reference": "ORD12345",
        "order_status": "Delivered",
        "shipment_id": 1,
        "ship_to": 101,
        "bill_to": 102,
        "total_amount": 1200.50,
        "total_discount": 50.00,
        "total_tax": 150.00,
        "total_surcharge": 20.00,
        "items": [
            {"item_id": "P001", "amount": 3},
            {"item_id": "P002", "amount": 2},
        ],
    },
    {
        "id": 2,
        "source_id": 34,
        "order_date": "2023-10-02T10:00:00Z",
        "request_date": "2023-10-06T16:00:00Z",
        "reference": "ORD12346",
        "order_status": "Scheduled",
        "shipment_id": 2,
        "ship_to": 103,
        "bill_to": 104,
        "total_amount": 800.00,
        "total_discount": 20.00,
        "total_tax": 80.00,
        "total_surcharge": 10.00,
        "items": [
            {"item_id": "P003", "amount": 1},
            {"item_id": "P004", "amount": 5},
        ],
    },
    {
        "id": 3,
        "source_id": 35,
        "order_date": "2023-10-03T09:00:00Z",
        "request_date": "2023-10-07T14:00:00Z",
        "reference": "ORD12347",
        "order_status": "Packed",
        "shipment_id": 3,
        "ship_to": 105,
        "bill_to": 106,
        "total_amount": 500.00,
        "total_discount": 10.00,
        "total_tax": 50.00,
        "total_surcharge": 5.00,
        "items": [
            {"item_id": "P005", "amount": 10},
            {"item_id": "P006", "amount": 7},
        ],
    },
]

BASE_URL = 'http://localhost:3000/api/v1/'


@pytest.fixture
def _url():
    return BASE_URL


@responses.activate
def test_get_orders(_url):
    url = f"{_url}orders"
    responses.add(responses.GET, url, json=MOCK_ORDERS, status=200)

    response = requests.get(url)

    assert response.status_code == 200
    data = response.json()

    assert len(data) == 3
    assert data[0]['id'] == 1
    assert data[1]['order_status'] == "Scheduled"
    assert data[2]['shipment_id'] == 3


@responses.activate
def test_get_order(_url):
    order_id = 1
    url = f"{_url}orders/{order_id}"
    mock_order = next((order for order in MOCK_ORDERS if order['id'] == order_id), None)

    responses.add(responses.GET, url, json=mock_order, status=200)

    response = requests.get(url)

    assert response.status_code == 200
    data = response.json()

    assert data['id'] == order_id
    assert data['order_status'] == "Delivered"
    assert data['items'][0]['item_id'] == "P001"


@responses.activate
def test_get_nonexistent_order(_url):
    order_id = 99  # order id bestaat niet
    url = f"{_url}orders/{order_id}"

    responses.add(responses.GET, url, json={"error": "Order not found"}, status=404)

    response = requests.get(url)

    assert response.status_code == 404
    data = response.json()

    assert "error" in data
    assert data['error'] == "Order not found"


@responses.activate
def test_add_order(_url):
    url = f"{_url}orders"
    new_order = {
        "id": 4,
        "source_id": 36,
        "order_date": "2023-10-04T10:00:00Z",
        "request_date": "2023-10-08T15:00:00Z",
        "reference": "ORD12348",
        "order_status": "Scheduled",
        "shipment_id": None,
        "ship_to": 107,
        "bill_to": 108,
        "total_amount": 600.00,
        "total_discount": 30.00,
        "total_tax": 60.00,
        "total_surcharge": 15.00,
        "items": [
            {"item_id": "P007", "amount": 4},
        ],
    }

    responses.add(responses.POST, url, json=new_order, status=201)

    response = requests.post(url, json=new_order)

    assert response.status_code == 201
    data = response.json()

    assert data['id'] == new_order['id']
    assert data['order_status'] == "Scheduled"


@responses.activate
def test_update_order(_url):
    order_id = 2
    url = f"{_url}orders/{order_id}"
    updated_order = {
        "id": 2,
        "source_id": 34,
        "order_date": "2023-10-02T10:00:00Z",
        "request_date": "2023-10-06T16:00:00Z",
        "reference": "ORD12346",
        "order_status": "Delivered",  # van "Scheduled" naar "Delivered"
        "shipment_id": 2,
        "ship_to": 103,
        "bill_to": 104,
        "total_amount": 800.00,
        "total_discount": 20.00,
        "total_tax": 80.00,
        "total_surcharge": 10.00,
        "items": [
            {"item_id": "P003", "amount": 1},
            {"item_id": "P004", "amount": 5},
        ],
    }

    responses.add(responses.PUT, url, json=updated_order, status=200)

    response = requests.put(url, json=updated_order)

    assert response.status_code == 200
    data = response.json()

    assert data['id'] == updated_order['id']
    assert data['order_status'] == "Delivered"
