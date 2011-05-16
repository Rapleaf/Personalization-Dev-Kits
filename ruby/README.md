How to Use
==========

Installation
------------
> gem install rapleaf_api

This gem depends on the "json" gem.

Usage
-----
    > require 'rapleaf_api'
    => true
    > api = RapleafApi::Api.new('my secret API key')
    => #<RapleafApi::Api:0x101b7f5f0 @API_KEY="my secret API key", @CA_FILE=nil, @TIMEOUT=2, @BASE_PATH="/v4/dr?api_key=my secret API key">
    > h = api.query_by_email('test@example.com')
    => {"location"=>"Fakesville, California, United States", "gender"=>"Male", "age"=>"25-34"}

Constructor Options
-------------------
You can pass in an options hash to the API constructor, like so:

    > api = RapleafApi::Api.new('my secret API key', :timeout => 10)
    => #<RapleafApi::Api:0x101b7f5f0 @API_KEY="my secret API key", @CA_FILE=nil, @TIMEOUT=10, @BASE_PATH="/v4/dr?api_key=my secret API key">

The possible options/keys accepted by the constructor are:

 - :timeout => The max amount of time to wait for a request to finish. Defaults to 2.
 - :ca_file => Set this to your system-wide root CA cert path if you're having SSL verification issues. Defaults to nil.
 
Query Options
-------------
The gem supports several ways to query Rapleaf's API: email, hashed email (either MD5 or SHA1 hash), name and postal (NAP), or name and ZIP+4 (NAZ).

### query_by_email(email, options)

This method queries Rapleaf's API with the specified email. The options hash accepts the following keys:

 - :hash_email    => Whether to (SHA1) hash the email before querying Rapleaf's API with it. Defaults to nil.
 - :show_availble => Controls whether the response will include information about available data---i.e., data for fields the API account is not subscribed to but for which Rapleaf has data. If this option is turned on, fields will be filled in with the string "Data Available". Defaults to nil.

### query_by_md5(md5_email, options)
### query_by_sha1(sha1_email, options)

These methods query Rapleaf's API with the specified email hashes (either MD5 or SHA1, respectively). Both methods accept an options hash with the following keys:

 - :show_available => Controls whether the response will include information about available data. Defaults to nil.
 
### query_by_nap(first, last, street, city, state, options)

This method queries Rapleaf's API with a name and postal address: first name, last name, street, city, and state acronym (i.e., the state's 2-character postal code). It also accepts the following options hash:

 - :email          => You can include an email in your NAP query to increase the hit rate. Defaults to nil.
 - :show_available => Controls whether the response will include information about available data. Defaults to nil.

### query_by_naz(first, last, zip4, options)

This method queries Rapleaf's API with a name and ZIP+4 code. The ZIP+4 is a string with a 5-digit ZIP code and 4-digit extension separated by a dash. This method accepts the following options:

- :email          => You can include an email in your NAP query to increase the hit rate. Defaults to nil.
- :show_available => Controls whether the response will include information about available data. Defaults to nil.


Contributing
============
If you have suggestions or patches, feel free to email us at
<developer at rapleaf dot com>. We look forward to hearing from you!


Contributors
============
Greg Poulos <greg at rapleaf dot com>
