==================
RAPLEAF DIRECT API
==================

Copyright 2010 Rapleaf

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.

For general information regarding the personalization API, visit http://www.rapleaf.com/developers/api_docs/personalization/direct. The personalization API's terms and conditions are stated at http://www.rapleaf.com/developers/api_usage.

How to Use
==========

Installation
------------

::

	easy_install rapleafApi

Usage
-----

>>> from rapleafApi import RapleafApi
>>> api = RapleafApi.RapleafApi('API_KEY')
>>> api.query_by_email('test@example.com')
{u'gender': u'Male', u'age': u'25-34'}


Query Options
-------------
The egg supports several ways to query Rapleaf's API: email, hashed email (either MD5 or SHA1 hash), name and postal (NAP), or name and ZIP+4 (NAZ).

**query_by_email(self, email, hash_email = False, show_available = False)**

| This method queries Rapleaf's API with the specified email. 
| If the hash_email option is set, then the email will be hashed before it's sent to Rapleaf.
| If the show_available option is set, then the string "Data Available" will be returned for those fields which the API account is not subscribed but for which RapLeaf has data.

| **query_by_md5(self, md5_email, show_available = False)**
| **query_by_sha1(self, sha1_email, show_available = False)**

| These methods query Rapleaf's API with the specified email hashes (either MD5 or SHA1, respectively). 
| If the show_available option is set, then the string "Data Available" will be returned for those fields which the API account is not subscribed but for which RapLeaf has data.
 
**query_by_nap(self, first, last, street, city, state, email = None, show_available = False)**

| This method queries Rapleaf's API with a name and postal address: first name, last name, street, city, and state acronym (i.e., the state's 2-character postal code).
| Though not necessary, adding an e-mail increases hit rate.
| If the show_available option is set, then the string "Data Available" will be returned for those fields which the API account is not subscribed but for which RapLeaf has data.


**query_by_naz(self, first, last, zip4, email = None, show_available = False)**

| This method queries Rapleaf's API with a name and ZIP+4 code. The ZIP+4 is a string with a 5-digit ZIP code and 4-digit extension separated by a dash.
| Though not necessary, adding an e-mail increases hit rate.
| If the show_available option is set, then the string "Data Available" will be returned for those fields which the API account is not subscribed but for which RapLeaf has data.


Contributing
============
If you have suggestions or patches, feel free to email us at
<developer at rapleaf dot com>. We look forward to hearing from you!


Contributors
============
Nicole Allard <nicole at rapleaf dot com>


Dependencies
============
urllib3
::

	easy_install urllib3

or visit the url https://github.com/shazow/urllib3/zipball/master.
Once you've unzipped the download, open a terminal window and navigate to the folder into which you unzipped the download. When you open the folder, one of the subdirectories is 'urllib3.' Open it. It contains a script called setup.py which you'll run via the command 'python setup.py install'.

Note that, on unsuccessful requests, we raise an error. Unsuccessful requests are any requests which send back an http response status outside of the range 200 <= status < 300.
