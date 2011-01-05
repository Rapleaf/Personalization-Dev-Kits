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

The API is queried by calling any of the query functions belonging to RapleafApi.py with the appropriate parameters. An example script, RapleafExample.py, is provided. The example script takes an e-mail as a command line parameter, connects to Rapleaf's database, and returns (and sends to stdout) a collection of associated key-value pairs.

To run the Python version, you'll need to download and install the urllib3 library. We choose this one since it recycles https connections, unlike the built in modules (urllib and urllib2). If you've installed easy_install, you need only run 'easy_install urllib3'. Else, to download urllib3, visit the url http://urllib3.googlecode.com/files/urllib3-0.3.1.tar.gz. Once you've unzipped the download, open a terminal window and navigate to the folder into which you unzipped the download. When you open the folder, one of the subdirectories is 'urllib3.' Open it. It contains a script called setup.py which you'll run via the command 'python setup.py install'. The license for the urllib3 license is posted at http://www.opensource.org/licenses/mit-license.php.

Note that, on unsuccessful requests, we raise an error. Unsuccessful requests are any requests which send back an http response status outside of the range 200 <= status < 300.
