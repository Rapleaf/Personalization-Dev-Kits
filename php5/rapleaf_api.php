<?php
	/*
	* Copyright 2010 Rapleaf
	*
	*  Licensed under the Apache License, Version 2.0 (the "License");
	*  you may not use this file except in compliance with the License.
	*  You may obtain a copy of the License at
	*
	*      http://www.apache.org/licenses/LICENSE-2.0
	*
	*  Unless required by applicable law or agreed to in writing, software
	*  distributed under the License is distributed on an "AS IS" BASIS,
	*  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	*  See the License for the specific language governing permissions and
	*  limitations under the License.
	*/

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
