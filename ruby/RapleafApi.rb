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

class RapleafApi
  API_KEY = "SET_ME"    # Set your API key
  
  HOST = "personalize.rlcdn.com"
  PORT = 443
  BASE_PATH = "/v4/dr?api_key=#{API_KEY}"
  HEADERS = {'User-Agent' => 'RapleafApi/Ruby/1.1'}
  TIMEOUT = 2
  
  def self.query_by_email(email, hash_email = false)
    # Takes an e-mail and returns a hash which maps attribute fields onto attributes
    # If the hash_email option is set, then the email will be hashed before it's sent to Rapleaf
    if hash_email
      query_by_sha1(Digest::SHA1.hexdigest(email))
    else
      get_json_response("#{BASE_PATH}&email=#{url_encode(email)}")
    end
  end
  
  def self.query_by_md5(md5_email)
    # Takes an e-mail that has already been hashed by md5
    # and returns a hash which maps attribute fields onto attributes
    get_json_response("#{BASE_PATH}&md5_email=#{url_encode(md5_email)}")
  end
  
  def self.query_by_sha1(sha1_email)
    # Takes an e-mail that has already been hashed by sha1
    # and returns a hash which maps attribute fields onto attributes
    get_json_response("#{BASE_PATH}&sha1_email=#{url_encode(sha1_email)}")
  end
  
  def self.query_by_nap(first, last, street, city, state, email = nil)
    # Takes first name, last name, and postal (street, city, and state acronym),
    # and returns a hash which maps attribute fields onto attributes
    # Though not necessary, adding an e-mail increases hit rate
    if email
      url = "#{BASE_PATH}&email=#{url_encode(email)}&first=#{url_encode(first)}&last=#{url_encode(last)}" +
      "&street=#{url_encode(street)}&city=#{url_encode(city)}&state=#{url_encode(state)}"
    else
      url = "#{BASE_PATH}&first=#{url_encode(first)}&last=#{url_encode(last)}" +
      "&street=#{url_encode(street)}&city=#{url_encode(city)}&state=#{url_encode(state)}"
    end
    get_json_response(url)
  end
  
  def self.query_by_naz(first, last, zip4, email = nil)
    # Takes first name, last name, and zip4 code (5-digit zip 
    # and 4-digit extension separated by a dash as a string),
    # and returns a hash which maps attribute fields onto attributes
    # Though not necessary, adding an e-mail increases hit rate
    if email
      url = "#{BASE_PATH}&email=#{url_encode(email)}&first=#{url_encode(first)}&last=#{url_encode(last)}&zip4=#{zip4}"
    else
      url = "#{BASE_PATH}&first=#{url_encode(first)}&last=#{url_encode(last)}&zip4=#{zip4}"
    end
    get_json_response(url)
  end
  
  private
  
  def self.get_json_response(path)
    # Takes a url and returns a hash mapping attribute fields onto attributes
    # Note that an exception is raised in the case that
    # an HTTP response code other than 200 is sent back
    # The error code and error body are put in the exception's message
    response = Timeout::timeout(TIMEOUT){http_client.get(path, HEADERS)}
    p response.code
    if response.code =~ /^2\d\d/
      (response.body && response.body != "") ? JSON.parse(response.body) : {}
    else
      raise "Error Code #{response.code}: \"#{response.body}\""
    end
  end
  
  # Returns http connection to HOST on PORT
  def self.http_client
    unless defined?(@@http_client)
      @@http_client = Net::HTTP.new(HOST, PORT)
      @@http_client.use_ssl = true
      @@http_client.verify_mode = OpenSSL::SSL::VERIFY_PEER
    end
    @@http_client
  end

end
