do 'rapleaf_api.pl';

	# This example script takes an e-mail as a command line argument 
	# and queries Rapleaf's database for any data associated with
	# the provided e-mail (unknown fields are left blank) 
	# The hash returned from query_by_email is iterated through
	# and each k/v pair is sent to std out 

my $email = $ARGV[0];
my $response = query_by_email($email);
while(my ($k, $v) = each %$response) {
    print "$k = $v.\n";
}