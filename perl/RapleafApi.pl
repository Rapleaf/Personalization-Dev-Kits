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
