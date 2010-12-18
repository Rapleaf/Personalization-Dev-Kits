require 'rapleaf_api'

=begin
  This example script takes an e-mail as a command line argument 
	and queries Rapleaf's database for any data associated with
	the provided e-mail (unknown fields are left blank)
=end

email = ARGV.first
begin
  response = RapleafApi.query_by_email(email)
  if response.empty?
    puts "No Data Found"
  else
    response.each do |k,v|
      puts "#{k} = #{v}"
    end
  end
rescue Exception => e
  puts e.message
end
