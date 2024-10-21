import unittest
from httpx import Client
 
 
class TestCargoHubAPI(unittest.TestCase):
    def setUp(self):
        self.baseUrl = 'http://localhost:3000/api/v1/'
        self.headers = {'API_KEY': 'a1b2c3d4e5'}
        self.client = Client()
 
    def test_get_warehouses(self):
        response = self.client.get(
            self.baseUrl + 'warehouses', headers=self.headers)
 
        self.assertEqual(response.status_code, 200)
 
    def test_post_warehouses(self):
        response = self.client.post(
            self.baseUrl + 'warehouses', headers=self.headers, json={"dadwadawd":"awdwadawd", "aaaaaaaaaaaaaa":"daeeeeeeeeeee"})
        
        self.assertEqual(response.status_code, 201)
 
    # def test_subtract(self):
    #     response = self.client.post('/subtract', json={'a': 6, 'b': 4})
    #     self.assertEqual(response.json()['result'], 2)
 
 
if __name__ == '__main__':
    unittest.main()
 