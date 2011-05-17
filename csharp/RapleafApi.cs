﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;
using System.Security.Cryptography;

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

namespace personalization
{
  /**
   * For general information regarding the personalization API, 
   * visit http://www.rapleaf.com/developers/api_docs/personalization/direct. 
   * The personalization API's terms and conditions 
   * are stated at http://www.rapleaf.com/developers/api_usage.
   */
  public class RapleafApi
  {
    private String apiKey;
    private const String BASE_URL = "https://personalize.rapleaf.com/v4/dr";
    private const int DEFAULT_TIMEOUT = 5000;
    private int timeout;
    private JavaScriptSerializer serializer;
    private MD5CryptoServiceProvider md5Crypto;
    private UTF8Encoding encoding;


    /// <summary>
    /// Constructor for RapleafApi class
    /// Used to access query member functions
    /// </summary>
    /// <param name="apiKey"> String given by individual's API key </param>
    /// The default timeout is set to 5000 ms
    public RapleafApi(String apiKey) : this(apiKey, DEFAULT_TIMEOUT) { }

    /// <summary>
    /// Constructor for RapleafApi class
    /// Used to access query member functions
    /// </summary>
    /// <param name="apiKey"> String given by individual's API key </param>
    /// <param name="timeout"> Supplied integer (ms) overrides the default timeout </param>
    public RapleafApi(String apiKey, int timeout)
    {
      this.encoding = new System.Text.UTF8Encoding();
      this.md5Crypto = new MD5CryptoServiceProvider();
      this.serializer = new JavaScriptSerializer();
      this.apiKey = apiKey;
      this.timeout = timeout;
    }

    /// <param name="email"> String email for query </param>
    /// <returns>  Returns a Dictionary associated with the parameter(s) </returns>
    public Dictionary<String, Object> queryByEmail(String email)
    {
      return queryByEmail(email, false);
    }

    /// <param name="email"> String email for query </param>
    /// <param name="hash_email">  If true, md5 hash the email before sending </param>
    /// <returns> Returns a Dictionary associated with the parameter(s) </returns>
    public Dictionary<String, Object> queryByEmail(String email, bool hash_email)
    {
      email = email.ToLower();
      if (hash_email)
      {
        return queryByMd5(MD5Hex(email));
      }
      else
      {
        String url = BASE_URL + "?email=" + HttpUtility.UrlEncode(email) + "&api_key=" + apiKey;
        return getJsonResponse(url);
      }
    }

    /// <param name="md5Email">  Md5 hashed string email for query </param>
    /// <returns> Returns a Dictionary associated with the parameter(s) </returns>
    public Dictionary<String, Object> queryByMd5(String md5Email)
    {
      String url = BASE_URL + "?md5_email=" + HttpUtility.UrlEncode(md5Email) + "&api_key=" + apiKey;
      return getJsonResponse(url);
    }

    /// <param name="sha1Email"> Sha1 hashed string email for query </param>
    /// <returns>  Returns a Dictionary associated with the parameter(s) </returns>
    public Dictionary<String, Object> queryBySha1(String sha1Email)
    {
      String url = BASE_URL + "?sha1_email=" + HttpUtility.UrlEncode(sha1Email) + "&api_key=" + apiKey;
      return getJsonResponse(url);
    }

    /// <param name="first"> First name </param>
    /// <param name="last"> Last name </param>
    /// <param name="street"> Street address </param>
    /// <param name="city"> City name </param>
    /// <param name="state"> State initials </param>
    /// <returns> Returns a Dictionary associated with the parameter(s) </returns>
    public Dictionary<String, Object> queryByNap(String first, String last, String street, String city, String state)
    {
      return queryByNap(first, last, street, city, state, null);
    }

    /// <param name="first"> First name </param>
    /// <param name="last"> Last name </param>
    /// <param name="street"> Street address </param>
    /// <param name="city"> City name </param>
    /// <param name="state"> State initials </param>
    /// <returns> Returns a Dictionary associated with the parameter(s) </returns>
    public Dictionary<String, Object> queryByNap(String first, String last, String street, String city, String state, String email)
    {
      String url;
      if (email != null)
      {
        url = BASE_URL + "?email=" + HttpUtility.UrlEncode(email) + "&api_key=" + apiKey +
        "?first=" + HttpUtility.UrlEncode(first) + "?last=" + HttpUtility.UrlEncode(last) +
        "?street=" + HttpUtility.UrlEncode(street) + "?city=" + HttpUtility.UrlEncode(city) +
        "?state=" + HttpUtility.UrlEncode(state);
      }
      else
      {
        url = BASE_URL + "&api_key=" + apiKey + "?state=" + HttpUtility.UrlEncode(state) +
        "?first=" + HttpUtility.UrlEncode(first) + "?last=" + HttpUtility.UrlEncode(last) +
        "?street=" + HttpUtility.UrlEncode(street) + "?city=" + HttpUtility.UrlEncode(city);
      }
      return getJsonResponse(url);
    }

    /// <param name="first"> First name </param>
    /// <param name="last"> Last name </param>
    /// <param name="zip4"> String containing 5 digit Zipcode + 4 digit extension separated by dash </param>
    /// <returns> Returns a Dictionary associated with the parameter(s) </returns>
    public Dictionary<String, Object> queryByNaz(String first, String last, String zip4)
    {
      return queryByNaz(first, last, zip4, null);
    }

    /// <param name="first"> First name </param>
    /// <param name="last"> Last name </param>
    /// <param name="zip4"> String containing 5 digit Zipcode + 4 digit extension separated by dash </param>
    /// <returns> Returns a Dictionary associated with the parameter(s) </returns>
    public Dictionary<String, Object> queryByNaz(String first, String last, String zip4, String email)
    {
      String url;
      if (email != null)
      {
        url = BASE_URL + "?email=" + HttpUtility.UrlEncode(email) + "&api_key=" + apiKey +
        "?first=" + HttpUtility.UrlEncode(first) + "?last=" + HttpUtility.UrlEncode(last) +
             "?zip4=" + zip4;
      }
      else
      {
        url = BASE_URL + "&api_key=" + apiKey + "?zip4=" + zip4 +
        "?first=" + HttpUtility.UrlEncode(first) + "?last=" + HttpUtility.UrlEncode(last);
      }
      return getJsonResponse(url);
    }

    /// <param name="urlStr"> String email built in query with URLEncoded email </param>
    /// <returns> Returns a Dictionary from fields onto field values </returns>
    private Dictionary<String, Object> getJsonResponse(String urlStr)
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlStr);
      request.UserAgent = "RapleafApi/NET/1.0";
      request.Timeout = timeout;
      request.KeepAlive = true;

      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
      StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
      String s = reader.ReadToEnd();
      reader.Close();
      response.Close();

      Dictionary<String, Object> jsonMap = serializer.Deserialize<Dictionary<String, Object>>(s);
      return jsonMap;
    }

    private String MD5Hex(String s)
    {
      return toHex(md5Crypto.ComputeHash(encoding.GetBytes(s)));
    }

    private String toHex(byte[] a)
    {
      String hexString = String.Empty;
      for (int i = 0; i < a.Length; i++)
      {
        hexString += a[i].ToString("X2");
      }
      return hexString;
    }
  }
}