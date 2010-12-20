use LWP::UserAgent;
use JSON;

$API_KEY = 'SET_ME';			# Set your API key here
$ua = LWP::UserAgent -> new;
$ua -> timeout(2);
$ua -> agent("RapleafApi/Perl/1.0");  

# Takes an e-mail and returns a hash mapping attribute fields onto attributes
sub query_by_email {
	my $email = $_[0];
	my $url = 'https://personalize.rlcdn.com/v4/dr?api_key=' . $API_KEY . '&email=' . $email;
	my $json_response = $ua -> get($url);
	$json_response -> is_success or
		die 'Error Code: ' . $json_response -> status_line . "\n" .
			'Error Body: ' . $json_response;
	$json = JSON -> new -> allow_nonref;
	my $personalization = $json -> decode($json_response -> content);
}