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

require "rubygems"
require "json"
require "net/https"
require "timeout"
require "erb"
require "digest"

include ERB::Util

module RapleafApi
  HOST = "personalize.rapleaf.com"
  PORT = 443
  HEADERS = {'User-Agent' => 'RapleafApi/Ruby/1.2.2'}

  class Api
    def initialize(api_key, options = {})
      @API_KEY = api_key
      @BASE_PATH = "/v4/dr?api_key=#{@API_KEY}"
      @TIMEOUT = options[:timeout] || 2
      @CA_FILE = options[:ca_file] # set to your system-wide root ca cert file 
                                   # if you're having ssl verification issues
    end
  
    # Takes an e-mail and returns a hash which maps attribute fields onto attributes
    # Options:
    #  :hash_email     - the email will be hashed before it's sent to Rapleaf
    #  :show_available - return the string "Data Available" for fields the API 
    #                    account is not subscribed to but for which Rapleaf has data
    def query_by_email(email, options = {})
      if options[:hash_email]
        query_by_sha1(Digest::SHA1.hexdigest(email.downcase), :show_available => options[:show_available])
      else
        get_json_response("#{@BASE_PATH}&email=#{url_encode(email)}", options[:show_available])
      end
    end
  
    # Takes an e-mail that has already been hashed by md5
    # and returns a hash which maps attribute fields onto attributes,
    # optionally showing available data in the response
    def query_by_md5(md5_email, options = {})
      get_json_response("#{@BASE_PATH}&md5_email=#{url_encode(md5_email)}", options[:show_available])
    end
  
    # Takes an e-mail that has already been hashed by sha1
    # and returns a hash which maps attribute fields onto attributes,
    # optionally showing available data in the response
    def query_by_sha1(sha1_email, options = {})
      get_json_response("#{@BASE_PATH}&sha1_email=#{url_encode(sha1_email)}", options[:show_available])
    end
  
    # Takes first name, last name, and postal (street, city, and state acronym),
    # and returns a hash which maps attribute fields onto attributes
    # Options:
    #  :email          - query with an email to increase the hit rate
    #  :show_available - return the string "Data Available" for fields 
    #                    the API account is not subscribed to but for 
    #                    which Rapleaf has data
    def query_by_nap(first, last, street, city, state, options = {})
      if options[:email]
        url = "#{@BASE_PATH}&email=#{url_encode(options[:email])}&first=#{url_encode(first)}&last=#{url_encode(last)}" +
        "&street=#{url_encode(street)}&city=#{url_encode(city)}&state=#{url_encode(state)}"
      else
        url = "#{@BASE_PATH}&first=#{url_encode(first)}&last=#{url_encode(last)}" +
        "&street=#{url_encode(street)}&city=#{url_encode(city)}&state=#{url_encode(state)}"
      end
      get_json_response(url, options[:show_available])
    end
  
    # Takes first name, last name, and zip4 code (5-digit zip 
    # and 4-digit extension separated by a dash as a string),
    # and returns a hash which maps attribute fields onto attributes
    # Options:
    #  :email          - query with an email to increase the hit rate
    #  :show_available - return the string "Data Available" for fields 
    #                    the API account is not subscribed to but for 
    #                    which Rapleaf has data
    def query_by_naz(first, last, zip4, options = {})
      if options[:email]
        url = "#{@BASE_PATH}&email=#{url_encode(options[:email])}&first=#{url_encode(first)}&last=#{url_encode(last)}&zip4=#{zip4}"
      else
        url = "#{@BASE_PATH}&first=#{url_encode(first)}&last=#{url_encode(last)}&zip4=#{zip4}"
      end
      get_json_response(url, options[:show_available])
    end
  
    private
  
    # Takes a url and returns a hash mapping attribute fields onto attributes
    # Note that an exception is raised in the case that
    # an HTTP response code other than 200 is sent back
    # The error code and error body are put in the exception's message
    def get_json_response(path, show_available = false)
      path += "&show_available" if show_available
      response = Timeout::timeout(@TIMEOUT) do
        begin
          http_client.get(path, HEADERS)
        rescue EOFError # Connection cut out. Just try a second time.
          http_client.get(path, HEADERS)
        end
      end
      if response.code =~ /^2\d\d/
        (response.body && response.body != "") ? JSON.parse(response.body) : {}
      else
        raise "Error Code #{response.code}: \"#{response.body}\""
      end
    end
  
    # Returns http connection to HOST on PORT
    def http_client
      unless defined?(@@http_client)
        @@http_client = Net::HTTP.new(HOST, PORT)
        @@http_client.use_ssl = true
        @@http_client.ca_file = @CA_FILE if @CA_FILE
        @@http_client.verify_mode = OpenSSL::SSL::VERIFY_PEER
        @@http_client.start
      end
      @@http_client
    end
  end
end
