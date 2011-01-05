<?php
	namespace Rapleaf;
	include "rapleaf_api.php";
	
	/* This example script takes an e-mail as a command line argument 
	 * and queries Rapleaf's database for any data associated with
	 * the provided e-mail (unknown fields are left blank) 
	 * The hash returned from query_by_email is iterated through
	 * and each k/v pair is sent to std out 
	 */
	
	$person = $argv[1];
	$response = query_by_email($person);
	foreach ($response as $key => $value) {
		echo $key . " = " . $value . "\n";
	}
?>