<?php
	namespace Rapleaf;
	class RapleafApi {
		private static $BASE_PATH = "https://personalize.rlcdn.com/v4/dr?api_key=";
		private static $handle;
		private static $API_KEY = "SET_ME";
	
		// Note that an exception is raised in the case that
	  	// an HTTP response code other than 200 is sent back
	  	// The error code and error body are displayed
	
		function __construct() {
			self::$handle = curl_init();
			curl_setopt(self::$handle, CURLOPT_RETURNTRANSFER, TRUE);
			curl_setopt(self::$handle, CURLOPT_TIMEOUT, 2.0);
			curl_setopt(self::$handle, CURLOPT_SSL_VERIFYPEER, TRUE);
			curl_setopt(self::$handle, CURLOPT_USERAGENT, "RapleafApi/PHP5/1.0");
		}
	
		function query_by_email($email) {
			$url = self::$BASE_PATH . self::$API_KEY . "&email=" . urlencode($email);
			return self::get_json_response($url);
		}
	
		function get_json_response($url) {
			curl_setopt(self::$handle, CURLOPT_URL, $url);
			$json_string = curl_exec(self::$handle);
			$response_code = curl_getinfo(self::$handle, CURLINFO_HTTP_CODE);
			if ($response_code < 200 || $response_code >= 300) {
				trigger_error("Error Code: " . $response_code . "\nError Body: " . $json_string);
			} else {
				$personalization = json_decode($json_string, TRUE);
				return $personalization;
			}
		}
	}
?>
