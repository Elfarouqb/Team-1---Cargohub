import pytest
import responses
import requests

MOCK_SHIPMENTS = [
    {
        "id": 1,
        "order_id": 1,
        "source_id": 33,
        "order_date": "2023-10-01",
        "request_date": "2023-10-03",
        "shipment_date": "2023-10-05",
        "shipment_type": "I",
        "shipment_status": "Pending",
        "notes": "Shipment pending for review.",
        "carrier_code": "UPS",
        "carrier_description": "United Parcel Service",
        "service_code": "Standard",
        "payment_type": "Manual",
        "transfer_mode": "Ground",
        "total_package_count": 5,
        "total_package_weight": 100.0,
        "created_at": "2023-10-01T10:00:00Z",
        "updated_at": "2023-10-01T12:00:00Z",
        "items": [
            {"item_id": "P001", "amount": 2},
            {"item_id": "P002", "amount": 1},
        ],
    },
    {
        "id": 2,
        "order_id": 2,
        "source_id": 34,
        "order_date": "2023-10-02",
        "request_date": "2023-10-04",
        "shipment_date": "2023-10-06",
        "shipment_type": "E",
        "shipment_status": "Shipped",
        "notes": "Shipment has been dispatched.",
        "carrier_code": "FedEx",
        "carrier_description": "Federal Express",
        "service_code": "Express",
        "payment_type": "Prepaid",
        "transfer_mode": "Air",
        "total_package_count": 3,
        "total_package_weight": 50.5,
        "created_at": "2023-10-02T11:00:00Z",
        "updated_at": "2023-10-02T13:00:00Z",
        "items": [
            {"item_id": "P003", "amount": 3},
        ],
    },
]


BASE_URL = 'http://localhost:3000/api/v1/'


@pytest.fixture
def _url():
    return BASE_URL


# Get all shipments (Happy Flow)
@responses.activate
def test_get_shipments(_url):
    url = f"{_url}shipments"
    responses.add(responses.GET, url, json=MOCK_SHIPMENTS, status=200)

    response = requests.get(url)

    assert response.status_code == 200
    shipments = response.json()

    assert len(shipments) == 2
    assert shipments[0]['id'] == 1
    assert shipments[1]['shipment_status'] == "Shipped"


# shipment by ID (Happy Flow)
@responses.activate
def test_get_shipment(_url):
    shipment_id = 1
    url = f"{_url}shipments/{shipment_id}"
    mock_shipment = next((s for s in MOCK_SHIPMENTS if s['id'] == shipment_id), None)

    responses.add(responses.GET, url, json=mock_shipment, status=200)

    response = requests.get(url)

    assert response.status_code == 200
    shipment = response.json()

    assert shipment['id'] == shipment_id
    assert shipment['carrier_code'] == "UPS"
    assert len(shipment['items']) == 2


# items in a shipment
@responses.activate
def test_get_items_in_shipment(_url):
    shipment_id = 1
    url = f"{_url}shipments/{shipment_id}/items"
    mock_items = next((s['items'] for s in MOCK_SHIPMENTS if s['id'] == shipment_id), [])

    responses.add(responses.GET, url, json=mock_items, status=200)

    response = requests.get(url)

    assert response.status_code == 200
    items = response.json()

    assert len(items) == 2
    assert items[0]['item_id'] == "P001"
    assert items[1]['amount'] == 1


# Add a new shipment
@responses.activate
def test_add_shipment(_url):
    url = f"{_url}shipments"
    new_shipment = {
        "id": 3,
        "order_id": 3,
        "source_id": 35,
        "order_date": "2023-10-03",
        "request_date": "2023-10-07",
        "shipment_date": "2023-10-09",
        "shipment_type": "I",
        "shipment_status": "Scheduled",
        "notes": "Shipment scheduled for pickup.",
        "carrier_code": "DHL",
        "carrier_description": "DHL Express",
        "service_code": "Overnight",
        "payment_type": "Credit",
        "transfer_mode": "Sea",
        "total_package_count": 4,
        "total_package_weight": 120.0,
        "items": [
            {"item_id": "P004", "amount": 2},
        ],
    }

    responses.add(responses.POST, url, json=new_shipment, status=201)

    response = requests.post(url, json=new_shipment)

    assert response.status_code == 201
    shipment = response.json()

    assert shipment['id'] == 3
    assert shipment['shipment_status'] == "Scheduled"
    assert len(shipment['items']) == 1


# Update existing shipment
@responses.activate
def test_update_shipment(_url):
    shipment_id = 2
    url = f"{_url}shipments/{shipment_id}"
    updated_shipment = {
        "id": 2,
        "order_id": 2,
        "source_id": 34,
        "order_date": "2023-10-02",
        "request_date": "2023-10-04",
        "shipment_date": "2023-10-06",
        "shipment_type": "E",
        "shipment_status": "Delivered",  # van "Shipped" naar "Delivered"
        "notes": "Shipment has been delivered.",
        "carrier_code": "FedEx",
        "carrier_description": "Federal Express",
        "service_code": "Express",
        "payment_type": "Prepaid",
        "transfer_mode": "Air",
        "total_package_count": 3,
        "total_package_weight": 50.5,
        "created_at": "2023-10-02T11:00:00Z",
        "updated_at": "2023-10-06T18:00:00Z",  # new time
        "items": [
            {"item_id": "P003", "amount": 3},
        ],
    }

    responses.add(responses.PUT, url, json=updated_shipment, status=200)

    response = requests.put(url, json=updated_shipment)

    assert response.status_code == 200
    shipment = response.json()

    assert shipment['id'] == shipment_id
    assert shipment['shipment_status'] == "Delivered"
    assert shipment['updated_at'] == "2023-10-06T18:00:00Z"


# Test: Remove a shipment
@responses.activate
def test_remove_shipment(_url):
    shipment_id = 1
    url = f"{_url}shipments/{shipment_id}"

    responses.add(responses.DELETE, url, status=204)

    response = requests.delete(url)

    assert response.status_code == 204
