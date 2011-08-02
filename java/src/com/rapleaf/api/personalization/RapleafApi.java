package com.rapleaf.api.personalization;
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

import java.net.*;
import java.io.*;

import org.json.JSONArray;
import org.json.JSONObject;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Collection;
import java.util.Map;

/**
 * For general information regarding the personalization API, 
 * visit http://www.rapleaf.com/developers/api_docs/personalization/direct. 
 * The personalization API's terms and conditions 
 * are stated at http://www.rapleaf.com/developers/api_usage.
 */
public class RapleafApi {
  private String apiKey;
  private final static String BASE_URL = "https://personalize.rapleaf.com/v4/dr";
  private final static String BULK_URL = "https://personalize.rapleaf.com/v4/bulk";
  private final static int DEFAULT_TIMEOUT = 2000;
  private final int timeout;
  
  /**
   * Constructor for RapleafApi class
   * Used to access query member functions
   * @param apiKey    String given by individual's API key
   * The default timeout is set to 2000 ms
   */
  public RapleafApi(String apiKey) {
    this(apiKey, DEFAULT_TIMEOUT);
  }
  
  /**
   * Constructor for RapleafApi class
   * Used to access query member functions
   * @param apiKey    String given by individual's API key
   * @param timeout   Supplied integer (ms) overrides the default timeout
   */
  public RapleafApi(String apiKey, int timeout) {
    this.apiKey = apiKey;
    this.timeout = timeout;
  }

  /**
   * @param email       String email for query
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryByEmail(String email) throws Exception {
    return queryByEmail(email, false);
  }

  /**
   * 
   * @param email
   * @param hash_email
   * @return
   * @throws Exception
   */
  public JSONObject queryByEmail(String email, boolean hash_email) throws Exception {
    return queryByEmail(email,hash_email,false);
  }

  /**
   * @param email       String email for query
   * @param hash_email  If true, md5 hash the email before sending
   * @param upsell      If true, show upsale information in response
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryByEmail(String email, boolean hash_email, boolean upsell) throws Exception {
    if (hash_email) {
      return queryByMd5(MD5Hex(email.toLowerCase()),upsell);
    } else {
      String url = BASE_URL + "?email=" + URLEncoder.encode(email, "UTF-8") + "&api_key=" + apiKey;
      return getJsonResponse(url, upsell);
    }
  }
  
  /**
   * @param md5Email    Md5 hashed string email for query
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryByMd5(String md5Email) throws Exception {
    return queryByMd5(md5Email, false);
  }

  /**
   * @param md5Email    Md5 hashed string email for query
   * @param upsell      If true, show upsale information in reply
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryByMd5(String md5Email, boolean upsell) throws Exception {
    String url = BASE_URL + "?md5_email=" + URLEncoder.encode(md5Email, "UTF-8") + "&api_key=" + apiKey;
    return getJsonResponse(url, upsell);
  }

  /**
   * @param sha1Email   Sha1 hashed string email for query
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryBySha1(String sha1Email) throws Exception {
    return queryBySha1(sha1Email, false);
  }
  
  /**
   * @param sha1Email   Sha1 hashed string email for query
   * @param upsell      If true, show upsale information in reply
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryBySha1(String sha1Email, boolean upsell) throws Exception {
    String url = BASE_URL + "?sha1_email=" + URLEncoder.encode(sha1Email, "UTF-8") + "&api_key=" + apiKey;
    return getJsonResponse(url, upsell);
  }

  /**
   * @param first       First name
   * @param last        Last name
   * @param street      Street address
   * @param city        City name
   * @param state       State initials
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryByNap(String first, String last, String street, String city, String state) throws Exception {
    return queryByNap(first, last, street, city, state, null, false);
  }
  
  /**
   * @param first       First name
   * @param last        Last name
   * @param street      Street address
   * @param city        City name
   * @param state       State initials
   * @param email       Email address
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryByNap(String first, String last, String street, String city, String state, String email) throws Exception {
    return queryByNap(first, last, street, city, state, email, false);
  }

  /**
   * @param first       First name
   * @param last        Last name
   * @param street      Street address
   * @param city        City name
   * @param state       State initials
   * @param email       Email address
   * @param upsell      If true, show upsale information in response
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryByNap(String first, String last, String street, String city, String state, String email, boolean upsell) throws Exception {
    String url;
    if (email != null) {
      url = BASE_URL + "?email=" + URLEncoder.encode(email, "UTF-8") + "&api_key=" + apiKey +
      "?first=" + URLEncoder.encode(first, "UTF-8") + "?last=" + URLEncoder.encode(last, "UTF-8") + 
      "?street=" + URLEncoder.encode(street, "UTF-8") + "?city=" + URLEncoder.encode(city, "UTF-8") + 
      "?state=" + URLEncoder.encode(state, "UTF-8");
    } else {
      url = BASE_URL + "&api_key=" + apiKey + "?state=" + URLEncoder.encode(state, "UTF-8") +
      "?first=" + URLEncoder.encode(first, "UTF-8") + "?last=" + URLEncoder.encode(last, "UTF-8") + 
      "?street=" + URLEncoder.encode(street, "UTF-8") + "?city=" + URLEncoder.encode(city, "UTF-8");
    }
    return getJsonResponse(url, upsell);
  }

  /**
   * @param first       First name
   * @param last        Last name
   * @param zip         String containing 5 digit Zipcode + 4 digit extension separated by dash
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryByNaz(String first, String last, String zip4) throws Exception {
    return queryByNaz(first, last, zip4, null, false);
  }

  /**
   * @param first       First name
   * @param last        Last name
   * @param zip         String containing 5 digit Zipcode + 4 digit extension separated by dash
   * @param email       Email address
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryByNaz(String first, String last, String zip4, String email) throws Exception {
    return queryByNaz(first, last, zip4, email, false);
  }
  
  /**
   * @param first       First name
   * @param last        Last name
   * @param zip         String containing 5 digit Zipcode + 4 digit extension separated by dash
   * @param email       Email address
   * @param upsell      If true, show upsale information in response
   * @return            Returns a JSONObject associated with the parameter(s)
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryByNaz(String first, String last, String zip4, String email, boolean upsell) throws Exception {
    String url;
    if (email != null) {
      url = BASE_URL + "?email=" + URLEncoder.encode(email, "UTF-8") + "&api_key=" + apiKey +
      "?first=" + URLEncoder.encode(first, "UTF-8") + "?last=" + URLEncoder.encode(last, "UTF-8") +
      "?zip4=" + zip4;
    } else {
      url = BASE_URL + "&api_key=" + apiKey + "?zip4=" + zip4 +
      "?first=" + URLEncoder.encode(first, "UTF-8") + "?last=" + URLEncoder.encode(last, "UTF-8");
    }
    return getJsonResponse(url, upsell);
  }

  /**
   * @param set
   * @return
   * @throws Exception
   */
  public JSONArray bulkQuery(Collection<Map<String,String>> set ) throws Exception {
    // default to false
    return bulkQuery(set, false);
  }
  
  /**
   * @param set
   * @param upsell      If true, show upsell information in response
   * @return            Returns a JSONArray of the responses
   * @throws Exception
   */
  public JSONArray bulkQuery(Collection<Map<String,String>> set, boolean upsell) throws Exception {
    String urlStr = BULK_URL + "?api_key=" + apiKey;
    if ( upsell ) {
      urlStr = urlStr + "&show_available=true";
    }
    return new JSONArray(bulkJsonResponse(urlStr, new JSONArray(set).toString()));
  }  
  
  private String bulkJsonResponse(String urlStr, String list) throws Exception {
    URL url = new URL(urlStr);
    HttpURLConnection handle = (HttpURLConnection) url.openConnection();
    handle.setRequestProperty("User-Agent", "RapleafApi/Java/1.0");
    handle.setRequestProperty("Content-Type", "application/json");
    handle.setConnectTimeout(timeout);
    handle.setReadTimeout(timeout);
    handle.setDoOutput(true);
    handle.setRequestMethod("POST");
    OutputStreamWriter wr = new OutputStreamWriter(handle.getOutputStream());
    wr.write(list);
    wr.flush();
    BufferedReader rd = new BufferedReader(new InputStreamReader(handle.getInputStream()));
    String line = rd.readLine();
    StringBuilder sb = new StringBuilder();
    while (line != null) {
      sb.append(line);
      line = rd.readLine();
    }
    wr.close();
    rd.close();
    
    return sb.toString();
  }
  
  /**
   * @param urlStr      String email built in query with URLEncoded email
   * @param upsell      If true, show upsale information in response
   * @return            Returns a JSONObject hash from fields onto field values
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  private JSONObject getJsonResponse(String urlStr, boolean upsell) throws Exception {
    if ( upsell )
    {
      urlStr = urlStr + "&show_available=true";
    }
    URL url = new URL(urlStr);
    HttpURLConnection handle = (HttpURLConnection) url.openConnection();
    handle.setRequestProperty("User-Agent", "RapleafApi/Java/1.0");
    handle.setConnectTimeout(timeout);
    handle.setReadTimeout(timeout);
    BufferedReader in = new BufferedReader(new InputStreamReader(handle.getInputStream()));
    String responseBody = in.readLine();
    in.close();
    int responseCode = handle.getResponseCode();
    if (responseCode < 200 || responseCode > 299) {
      throw new Exception("Error Code " + responseCode + ": " + responseBody);
    }
    if(responseBody == null || responseBody.equals("")) {
      responseBody = "{}";
    }
    return new JSONObject(responseBody);
  }
  
  private String MD5Hex(String s) {
    String result = null;
    try {
      MessageDigest md5 = MessageDigest.getInstance("MD5");
      byte[] digest = md5.digest(s.getBytes());
      result = toHex(digest);
    } catch (NoSuchAlgorithmException e) {  }
    return result;
  }

  private String toHex(byte[] a) {
    StringBuilder sb = new StringBuilder(a.length * 2);
    for (int i = 0; i < a.length; i++) {
        sb.append(Character.forDigit((a[i] & 0xf0) >> 4, 16));
        sb.append(Character.forDigit(a[i] & 0x0f, 16));
    }
    return sb.toString();
  }
  
}
