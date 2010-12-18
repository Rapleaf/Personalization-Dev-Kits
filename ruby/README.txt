==================
RAPLEAF DIRECT API
==================

For general information regarding the personalization API, visit http://www.rapleaf.com/developers/api_docs/personalization/direct. The personalization API's terms and conditions are stated at http://www.rapleaf.com/developers/api_usage.

The API is queried by calling the query_by_email function belonging to either the rapleaf_api file. An example script, rapleaf_example, is provided. The example script takes an e-mail as a command line parameter, connects to Rapleaf's database, and returns (and sends to stdout) a collection of associated key-value pairs.

You need each of the "json", "net/https", and "timeout" gems to run the api. To find out which gems are installed on your machine, run the command "gem list". For each of the gems <gem_name> from the previous list that aren't listed, run the command 'gem install <gem_name>'.

Note that, on unsuccessful requests, we raise an error. Unsuccessful requests are any requests which send back an http response status outside of the range 200 <= status < 300.