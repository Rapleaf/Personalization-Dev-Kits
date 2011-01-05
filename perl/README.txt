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

The API is queried by calling any of the query functions belonging to the RapleafApi file. An example script, RapleafExample, is provided. The example script takes an e-mail as a command line parameter, connects to Rapleaf's database, and returns (and sends to stdout) a collection of associated key-value pairs.

In order to run the Perl API, you need to grab JSON-2.50.tar.gz from http://search.cpan.org/~makamaka/JSON-2.50/lib/JSON.pm (flushed right). The installation is described in the README file. It consists of navigating to the download directory and running Makefile.pl, make, make test, and make install (in that order).
