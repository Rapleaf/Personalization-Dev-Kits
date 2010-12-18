==================
RAPLEAF DIRECT API
==================

For general information regarding the personalization API, visit http://www.rapleaf.com/developers/api_docs/personalization/direct. The personalization API's terms and conditions are stated at http://www.rapleaf.com/developers/api_usage.

The API is queried by calling the query_by_email function belonging to either the rapleaf_api file. An example script, rapleaf_example, is provided. The example script takes an e-mail as a command line parameter, connects to Rapleaf's database, and returns (and sends to stdout) a collection of associated key-value pairs.

For the PHP version, we avoid making our code object-oriented to appeal to users running older versions of PHP.
