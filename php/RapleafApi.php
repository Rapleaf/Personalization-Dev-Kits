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
  
  define("RAPLEAF_API_KEY", "SET_ME");      // Set your API key here
  define("RAPLEAF_BASE_PATH", "https://personalize.rapleaf.com/v4/dr?api_key=" . RAPLEAF_API_KEY);
  define("RAPLEAF_BULK_PATH", "https://personalize.rapleaf.com/v4/bulk?api_key=" . RAPLEAF_API_KEY);
  $rapleaf_api_curl = curl_init();
  curl_setopt($rapleaf_api_curl, CURLOPT_RETURNTRANSFER, TRUE);
  curl_setopt($rapleaf_api_curl, CURLOPT_SSL_VERIFYPEER, TRUE);
  curl_setopt($rapleaf_api_curl, CURLOPT_USERAGENT, "RapleafApi/PHP/1.1");
  
  function query_by_email($email, $hash_email = false, $show_available = false) {
    /* Takes an e-mail and returns a hash which maps attribute fields onto attributes
     * If the hash_email option is set, then the email will be hashed before it's sent to Rapleaf
     */
    if ($hash_email) {
      $sha1_email = sha1(strtolower($email));
      return query_by_sha1($sha1_email);
    } else {
      $url = RAPLEAF_BASE_PATH . "&email=" . urlencode($email);
      return get_json_response($url, $show_available);
    }
  }
  
  function query_by_md5($md5_email, $show_available = false) {
    /* Takes an e-mail that has already been hashed by md5
     * returns a hash which maps attribute fields onto attributes
     */
    $url = RAPLEAF_BASE_PATH . "&md5_email=" . urlencode($md5_email);
    return get_json_response($url, $show_available);
  }
  
  function query_by_sha1($sha1_email, $show_available = false) {
    /* Takes an e-mail that has already been hashed by sha1
     * and returns a hash which maps attribute fields onto attributes
     */
    $url = RAPLEAF_BASE_PATH . "&sha1_email=" . urlencode($sha1_email);
    return get_json_response($url, $show_available);
  }
  
  function query_by_nap($first, $last, $street, $city, $state, $email = null, $show_available = false) {
    /* Takes first name, last name, and postal (street, city, and state acronym),
     * and returns a hash which maps attribute fields onto attributes
     * Though not necessary, adding an e-mail increases hit rate
     */
    if ($email) {
      $url = RAPLEAF_BASE_PATH . "&email=" . urlencode($email) .
      "&first=" . urlencode($first) . "&last=" . urlencode($last) . 
      "&street=" . urlencode($street) . "&city=" . urlencode($city) . "&state=" . urlencode($state);
    } else {
      $url = RAPLEAF_BASE_PATH . "&first=" . urlencode($first) . "&last=" . urlencode($last) . 
      "&street=" . urlencode($street) . "&city=" . urlencode($city) . "&state=" . urlencode($state);
    }
    return get_json_response($url, $show_available);
  }
  
  function query_by_naz($first, $last, $zip4, $email = null, $show_available = false) {
    /* Takes first name, last name, and zip4 code (5-digit zip 
     * and 4-digit extension separated by a dash as a string),
     * and returns a hash which maps attribute fields onto attributes
     * Though not necessary, adding an e-mail increases hit rate
     */
    if ($email) {
      $url = RAPLEAF_BASE_PATH . "&email=" . urlencode($email) .
      "&first=" . urlencode($first) . "&last=" . urlencode($last) . "&zip4=" . $zip4;
    } else {
      $url = RAPLEAF_BASE_PATH . "&zip4=" . $zip4 .
      "&first=" . urlencode($first) . "&last=" . urlencode($last);
    }
    return get_json_response($url, $show_available);
  }

  function bulk_query($set, $show_available = false) {
    $url = RAPLEAF_BULK_PATH;
    if ( $show_available ) {
      $url = $url . "&show_available=true";
    }
    return get_bulk_query(json_encode($set), $url);
  }

  function get_bulk_query($data, $url) {
    global $rapleaf_api_curl;
    curl_setopt($rapleaf_api_curl, CURLOPT_HTTPHEADER, array("Content-Type: application/json"));
    curl_setopt($rapleaf_api_curl, CURLOPT_URL, $url);
    curl_setopt($rapleaf_api_curl, CURLOPT_TIMEOUT, 30.0);
    curl_setopt($rapleaf_api_curl, CURLOPT_POST, true);
    curl_setopt($rapleaf_api_curl, CURLOPT_POSTFIELDS, $data);
    
    $json_string = curl_exec($rapleaf_api_curl);
    
    if ( ! $json_string ) { 
      $json_string = curl_exec($rapleaf_api_curl);
    }
    $response_code = curl_getinfo($rapleaf_api_curl, CURLINFO_HTTP_CODE);
    if ($response_code < 200 || $response_code >= 300) {
      throw new \Exception("Error Code: " . $response_code . "\nError Body: " . $json_string);
    } 
    else {
      $personalization = json_decode($json_string, TRUE);
      return $personalization;
    }
  }

  function get_json_response($url, $show_available) {
    /* Pre: Path is an extension to personalize.rapleaf.com
     * Note that an exception is raised if an HTTP response code
     * other than 200 is sent back. In this case, both the error code
     * the error code and error body are accessible from the exception raised
     */
    global $rapleaf_api_curl;
    if ( $show_available ) {
      $url = $url . $show_available; 
    }
    curl_setopt($rapleaf_api_curl, CURLOPT_URL, $url);
    curl_setopt($rapleaf_api_curl, CURLOPT_TIMEOUT, 2.0);
    $json_string = curl_exec($rapleaf_api_curl);
    $response_code = curl_getinfo($rapleaf_api_curl, CURLINFO_HTTP_CODE);
    if ($response_code < 200 || $response_code >= 300) {
      trigger_error("Error Code: " . $response_code . "\nError Body: " . $json_string);
    } else {
      $personalization = json_decode($json_string, TRUE);
      return $personalization;
    }
  }
?>
