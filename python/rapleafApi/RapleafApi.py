# Copyright 2010 Rapleaf
#
#  Licensed under the Apache License, Version 2.0 (the "License");
#  you may not use this file except in compliance with the License.
#  You may obtain a copy of the License at
#
#      http://www.apache.org/licenses/LICENSE-2.0
#
#  Unless required by applicable law or agreed to in writing, software
#  distributed under the License is distributed on an "AS IS" BASIS,
#  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
#  See the License for the specific language governing permissions and
#  limitations under the License.

try:
    from urllib import quote
except ImportError:
    from urllib.parse import quote

from urllib3 import HTTPSConnectionPool # See README for download instructions
import json
import hashlib

class RapleafApi:
    
    HEADERS = {'User-Agent' : 'RapleafApi/Python/1.1'}
    BASE_PATH = '/v4/dr'
    HOST = 'personalize.rapleaf.com'
    TIMEOUT = 2.0
    
    def __init__(self, api_key):
        self.handle = HTTPSConnectionPool(RapleafApi.HOST, timeout=RapleafApi.TIMEOUT)
        self.base_path = RapleafApi.BASE_PATH + '?api_key=%s' % (api_key)
    
    def query_by_email(self, email, hash_email=False, show_available=False):
        """
        Takes an e-mail and returns a hash which maps attribute fields onto attributes
        If the hash_email option is set, then the email will be hashed before it's sent to Rapleaf.
        If the show_available option is set, then the string "Data Available" will be returned for 
        those fields which the API account is not subscribed but for which RapLeaf has data.
        """
        if hash_email:
            s = hashlib.sha1()
            s.update(email.lower())
            return self.query_by_sha1(s.hexdigest(), show_available)
        url = '%s&email=%s' % (self.base_path, quote(email))
        return self.__get_json_response(url, show_available)
    
    def query_by_md5(self, md5_email, show_available=False):
        """
        Takes an e-mail that has already been hashed by md5
        and returns a hash which maps attribute fields onto attributes.
        If the show_available option is set, then the string "Data Available" will be returned for 
        those fields which the API account is not subscribed but for which RapLeaf has data.
        """
        url = '%s&md5_email=%s' % (self.base_path, quote(md5_email))
        return self.__get_json_response(url, show_available)
    
    def query_by_sha1(self, sha1_email, show_available=False):
        """
        Takes an e-mail that has already been hashed by sha1
        and returns a hash which maps attribute fields onto attributes.
        If the show_available option is set, then the string "Data Available" will be returned for 
        those fields which the API account is not subscribed but for which RapLeaf has data.
        """
        url = '%s&sha1_email=%s' % (self.base_path, quote(sha1_email))
        return self.__get_json_response(url, show_available)
        
    def query_by_nap(self, first, last, street, city, state, email=None, show_available=False):
        """
        Takes first name, last name, and postal (street, city, and state acronym),
        and returns a hash which maps attribute fields onto attributes
        Though not necessary, adding an e-mail increases hit rate.
        If the show_available option is set, then the string "Data Available" will be returned for 
        those fields which the API account is not subscribed but for which RapLeaf has data.
        """
        url = '%s&first=%s&last=%s&street=%s&city=%s&state=%s' % (
            self.base_path, quote(first), quote(last), 
            quote(street), quote(city), quote(state))
        if email:
            url = '%s&email=%s' % (url, quote(email))
        return self.__get_json_response(url, show_available)
    
    def query_by_naz(self, first, last, zip4, email=None, show_available=False):
        """
        Takes first name, last name, and zip4 code (5-digit zip 
        and 4-digit extension separated by a dash as a string),
        and returns a hash which maps attribute fields onto attributes
        Though not necessary, adding an e-mail increases hit rate.
        If the show_available option is set, then the string "Data Available" will be returned for 
        those fields which the API account is not subscribed but for which RapLeaf has data.
        """
        url = '%s&first=%s&last=%s&zip4=%s' % (
            self.base_path, quote(first), quote(last), zip4)
        if email:
            url = '%s&email=%s' % (url, quote(email))
        return self.__get_json_response(url, show_available)
        
    def __get_json_response(self, path, show_available=False):
        """
        Pre: Path is an extension to personalize.rapleaf.com
        Note that an exception is raised if an HTTP response code
        other than 200 is sent back. In this case, both the error code
        the error code and error body are accessible from the exception raised
        """
        if show_available:
            path += '&show_available'
        json_response = self.handle.request('GET', path, headers=RapleafApi.HEADERS)
        if 200 <= json_response.status < 300:
            if json_response.data:
                return json.JSONDecoder().decode(json_response.data)
            else:
                return {}
        else:
            raise Exception(json_response.status, json_response.data.decode("utf-8"))
