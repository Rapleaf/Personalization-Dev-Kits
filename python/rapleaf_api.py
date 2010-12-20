import urllib
from urllib3 import HTTPSConnectionPool, TimeoutError		# See README for download instructions
import json

class RapleafApi:
	API_KEY = 'SET_ME'		# Set your API key here
		 
	HEADERS = {'User-Agent' : 'RapleafApi/Python/1.0'}
	BASE_PATH = '/v4/dr?api_key=%s' %(API_KEY)
	HOST = 'personalize.rlcdn.com'
	TIMEOUT = 2.0
	
	# Constructor creates a reusable connection to HOST
	def __init__(self):
		self.handle = HTTPSConnectionPool(RapleafApi.HOST, timeout = RapleafApi.TIMEOUT)
	
	# Takes an e-mail and returns a hash mapping attribute fields onto attributes
	def query_by_email(self, email):
		url = '%s&email=%s' %(RapleafApi.BASE_PATH, urllib.quote(email))	# Build url
		return self.get_json_response(url)
	
	# Pre: path is an extension to personalize.rlcdn.com
	# Note that an exception is raised if an HTTP response code
	# other than 200 is sent back. In this case, both the error code
	# the error code and error body are accessible from the exception raised
	def get_json_response(self, path):
		json_response = self.handle.get_url(path, headers = RapleafApi.HEADERS)
		if 200 <= json_response.status < 300:
			if json_response.data:
				return json.JSONDecoder().decode(json_response.data)
			else:
				return {}
		else:
			raise Exception(json_response.status, json_response.data)
            
