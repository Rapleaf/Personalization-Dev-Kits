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

The API is queried by calling the query_by_email function belonging to the rapleaf_api file. An example script, rapleaf_example, is provided. The example script takes an e-mail as a command line parameter, connects to Rapleaf's database, and returns (and sends to stdout) a collection of associated key-value pairs.

You need each of the "json", "net/https", and "timeout" gems to run the api. To find out which gems are installed on your machine, run the command "gem list". For each of the gems <gem_name> from the previous list that aren't listed, run the command 'gem install <gem_name>'.

Note that, on unsuccessful requests, we raise an error. Unsuccessful requests are any requests which send back an http response status outside of the range 200 <= status < 300.