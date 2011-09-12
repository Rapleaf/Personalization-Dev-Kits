using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;
using System.Security.Cryptography;

/*
 * Copyright 2011 Rapleaf
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
    private const String BASE_URL_DIRECT = "https://personalize.rapleaf.com/v4/dr";
    private const String BASE_URL_BULK = "https://personalize.rapleaf.com/v4/bulk";
    private const int DEFAULT_DIRECT_TIMEOUT = 5000;
    private const int DEFAULT_BULK_TIMEOUT = 30000;
    private const int MAX_BULK = 10000;          // maximum number of identifiers allowed per request in the bulk api
    private const int MAX_JSON_LENGTH = 5242880; // maximum parsable JSON response length. xMB*1024*1024/2 (2 for unicode)
    private int direct_timeout;
    private int bulk_timeout;
    private bool show_available;
    private JavaScriptSerializer serializer;
    private MD5CryptoServiceProvider md5Crypto;
    private UTF8Encoding encoding;

    /// <summary>
    /// Constructor for RapleafApi class
    /// Used to access query member functions
    /// </summary>
    /// <param name="apiKey"> String given by individual's API key </param>
    /// The default timeout is set to 5000 ms
    /// show_available is set to false
    public RapleafApi(String apiKey) : this(apiKey, DEFAULT_DIRECT_TIMEOUT, DEFAULT_BULK_TIMEOUT) { }

    /// <summary>
    /// Constructor for RapleafApi class
    /// Used to access query member functions
    /// </summary>
    /// <param name="apiKey"> String given by individual's API key </param>
    /// <param name="direct_timeout"> Supplied integer (ms) overrides the default direct timeout </param>
    /// <param name="bulk_timeout"> Supplied integer (ms) overrides the default bulk timeout </param>
    /// show_available is set to false
    public RapleafApi(String apiKey, int direct_timeout, int bulk_timeout) : this(apiKey, direct_timeout, bulk_timeout, false) { }

    /// <summary>
    /// Constructor for RapleafApi class
    /// Used to access query member functions
    /// </summary>
    /// <param name="apiKey"> String given by individual's API key </param>
    /// <param name="direct_timeout"> Supplied integer (ms) overrides the default direct timeout </param>
    /// <param name="bulk_timeout"> Supplied integer (ms) overrides the default bulk timeout </param>
    /// <param name="show_available">
    /// Supplied boolean overrides show_available
    /// Indicates whether to return "Data Available" when data is available for fields the current
    /// key isn't configured to receive
    /// </param>
    public RapleafApi(String apiKey, int direct_timeout, int bulk_timeout, bool show_available)
    {
      this.encoding = new System.Text.UTF8Encoding();
      this.md5Crypto = new MD5CryptoServiceProvider();
      this.serializer = new JavaScriptSerializer();
      this.apiKey = apiKey;
      this.direct_timeout = direct_timeout;
      this.bulk_timeout = bulk_timeout;
      this.show_available = show_available;

      serializer.MaxJsonLength = MAX_JSON_LENGTH;
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
        String url = BASE_URL_DIRECT + "?email=" + HttpUtility.UrlEncode(email) + "&api_key=" + apiKey;
        return getJsonResponse(url);
      }
    }
    /// <summary>
    /// Fast convenience method for using the Bulk API to query with a list of emails
    /// Automatically splits the emails list as required into batches equal in size to the maximum allowed by the API
    /// </summary>
    /// <param name="emails"> List of emails </param>
    /// <param name="hash_email">  If true, md5 hash the email before sending </param>
    /// <returns> Returns a List of Dictionaries associated with the API response values for each email </returns>
    public List<Dictionary<String, Object>> queryByEmail(List<String> emails, bool hash_email)
    {
      int i = 0;
      List<Dictionary<String, Object>> results = new List<Dictionary<String, Object>>();
      while (i <= emails.Count / MAX_BULK)
      {
        List<String> emailSubset = emails.GetRange(i * MAX_BULK, Math.Min(MAX_BULK, emails.Count - i * MAX_BULK));
        String urlStr = BASE_URL_BULK + "?api_key=" + apiKey;
        StringBuilder builder = new StringBuilder("[");
        foreach (String email in emailSubset)
        {
          if (hash_email)
          {
            builder.Append("{\"md5_email\":\"").Append(MD5Hex(email)).Append("\"},");
          }
          else
          {
            builder.Append("{\"email\":\"").Append(email.ToLower()).Append("\"},");
          }
        }
        builder.Remove(builder.Length - 1, 1); // remove trailing comma
        builder.Append("]");
        results.AddRange(getBulkJsonResponse(urlStr, builder.ToString()));
        i++;
      }
      return results;
    }

    /// <param name="md5Email">  Md5 hashed string email for query </param>
    /// <returns> Returns a Dictionary associated with the parameter(s) </returns>
    public Dictionary<String, Object> queryByMd5(String md5Email)
    {
      String url = BASE_URL_DIRECT + "?md5_email=" + HttpUtility.UrlEncode(md5Email) + "&api_key=" + apiKey;
      return getJsonResponse(url);
    }

    /// <param name="sha1Email"> Sha1 hashed string email for query </param>
    /// <returns>  Returns a Dictionary associated with the parameter(s) </returns>
    public Dictionary<String, Object> queryBySha1(String sha1Email)
    {
      String url = BASE_URL_DIRECT + "?sha1_email=" + HttpUtility.UrlEncode(sha1Email) + "&api_key=" + apiKey;
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
        url = BASE_URL_DIRECT + "?email=" + HttpUtility.UrlEncode(email) + "&api_key=" + apiKey +
        "?first=" + HttpUtility.UrlEncode(first) + "?last=" + HttpUtility.UrlEncode(last) +
        "?street=" + HttpUtility.UrlEncode(street) + "?city=" + HttpUtility.UrlEncode(city) +
        "?state=" + HttpUtility.UrlEncode(state);
      }
      else
      {
        url = BASE_URL_DIRECT + "&api_key=" + apiKey + "?state=" + HttpUtility.UrlEncode(state) +
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
        url = BASE_URL_DIRECT + "?email=" + HttpUtility.UrlEncode(email) + "&api_key=" + apiKey +
        "?first=" + HttpUtility.UrlEncode(first) + "?last=" + HttpUtility.UrlEncode(last) +
             "?zip4=" + zip4;
      }
      else
      {
        url = BASE_URL_DIRECT + "&api_key=" + apiKey + "?zip4=" + zip4 +
        "?first=" + HttpUtility.UrlEncode(first) + "?last=" + HttpUtility.UrlEncode(last);
      }
      return getJsonResponse(url);
    }

    /// <param name="identifiers">
    /// List of Dictionaries containing allowed Rapleaf identifiers
    /// ex. {"email"}, {"first","last",zip4"}, etc
    /// </param>
    /// <returns> Returns a List of Dictionaries associated with the API response values of each input identifier </returns>
    public List<Dictionary<String, Object>> genericBulkQuery(List<Dictionary<String, String>> identifiers)
    {
      String urlStr = BASE_URL_BULK + "?api_key=" + apiKey;
      return getBulkJsonResponse(urlStr, serializer.Serialize(identifiers));
    }

    /// <param name="urlStr"> String email built in query with URLEncoded email </param>
    /// <returns> Returns a Dictionary from fields onto field values </returns>
    /// for a complete list of possible identifiers, see the developer documentation
    private Dictionary<String, Object> getJsonResponse(String urlStr)
    {
      String s = getResponse(urlStr);
      Dictionary<String, Object> jsonMap = serializer.Deserialize<Dictionary<String, Object>>(s);
      return jsonMap;
    }

    private List<Dictionary<String, Object>> getBulkJsonResponse(String urlStr, String queryStr)
    {
      String s = getResponse(urlStr, true, queryStr);
      return serializer.Deserialize<List<Dictionary<String, Object>>>(s);
    }

    private String getResponse(String urlStr)
    {
      return getResponse(urlStr, false, "");
    }
    private String getResponse(String urlStr, bool post, String queryStr)
    {
      if (show_available)
      {
        urlStr += "&show_available=true";
      }

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlStr);
      request.UserAgent = "RapleafApi/NET/1.0";
      request.Timeout = direct_timeout;
      request.KeepAlive = true;

      if (post)
      {
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Timeout = bulk_timeout;
        if (!queryStr.Equals(String.Empty))
        {
          Stream dataStream = request.GetRequestStream();
          dataStream.Write(encoding.GetBytes(queryStr), 0, queryStr.Length);
          dataStream.Close();
        }
      }

      HttpWebResponse response = (HttpWebResponse)request.GetResponse();
      StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
      String s = reader.ReadToEnd();
      reader.Close();
      response.Close();
      return s;
    }

    public String MD5Hex(String s)
    {
      return toHex(md5Crypto.ComputeHash(encoding.GetBytes(s.ToLower())));
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