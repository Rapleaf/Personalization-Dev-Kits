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
import org.json.JSONObject;

/**
 * For general information regarding the personalization API, 
 * visit http://www.rapleaf.com/developers/api_docs/personalization/direct. 
 * The personalization API's terms and conditions 
 * are stated at http://www.rapleaf.com/developers/api_usage.
 */
public class RapleafApi {
  private String apiKey;
  private final static String BASE_URL = "https://personalize.rlcdn.com/v4/dr";
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
   * @return            Returns a JSONObject associated with the email parameter
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  public JSONObject queryByEmail(String email) throws Exception {
    String url = BASE_URL + "?email=" + URLEncoder.encode(email, "UTF-8") + "&api_key=" + apiKey;
    return getJsonResponse(url);
  }
  
  /**
   * @param urlStr      String email built in query with URLEncoded email
   * @return            Returns a JSONObject hash from fields onto field values
   * @throws Exception  Throws error code on all HTTP statuses outside of 200 <= status < 300
   */
  private JSONObject getJsonResponse(String urlStr) throws Exception {
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
}
