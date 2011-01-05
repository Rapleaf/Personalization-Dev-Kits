require "rubygems"
require "json"
require "net/https"
require "timeout"
require "erb"
include ERB::Util

class RapleafApi
  API_KEY = "d4c02b8148d1cca0a2632aa08ccd3908"    # Set your API key here
  
  HOST = "personalize.rlcdn.com"
  PORT = 443
  BASE_PATH = "/v4/dr?api_key=#{API_KEY}"
  HEADERS = {'User-Agent' => 'RapleafApi/Ruby/1.0'}
  TIMEOUT = 2
  
  def self.query_by_email(email)
    get_json_response("#{BASE_PATH}&email=#{url_encode(email)}")
  end
  
  private
  
  # Takes an e-mail and returns a hash mapping attribute fields onto attributes
  # Note that an exception is raised in the case that
  # an HTTP response code other than 200 is sent back
  # The error code and error body are put in the exception's message
  def self.get_json_response(path)
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
