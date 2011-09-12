using System.Collections.Generic;
using System;
using System.Net;

namespace personalization
{
  class RapleafExample
  {
    // outlined below are three separate ways of querying the API using the RapleafApi library
    public static void Main(string[] args) {
      RapleafApi api = new RapleafApi("SET_ME"); // Set API key here

      // Method 1
      try
      {
        Dictionary<string, Object> response = api.queryByEmail("pete@rapleafdemo.com");
        foreach (KeyValuePair<string, Object> kvp in response)
        {
          printKeyValuePair(kvp);
        }
      }
      catch (WebException e)
      {
        Console.WriteLine(e.Message);
      }
      Console.WriteLine("\nHit Enter for the next method");
      Console.ReadLine();

      // Method 2
      try
      {
        List<Dictionary<string, Object>> response = api.genericBulkQuery(new List<Dictionary<String, String>> {
          new Dictionary<String, String> {
            {"email", "steve@rapleafdemo.com" }
          },
          new Dictionary<String, String> {
            {"first", "Pete" },
            {"last", "Schlick" },
            {"street", "112134 Leavenworth Rd" },
            {"city", "San Francisco" },
            {"state", "CA" }
          }
        });
        foreach (Dictionary<string, Object> entry in response)
        {
          Console.WriteLine("--- entry ---");
          foreach (KeyValuePair<string, Object> kvp in entry)
          {
            printKeyValuePair(kvp);
          }
        }
      }
      catch (WebException e)
      {
        Console.WriteLine(e.Message);
      }
      Console.WriteLine("\nHit Enter for the next method");
      Console.ReadLine();

      // Method 3
      try
      {
        List<Dictionary<string, Object>> response = api.queryByEmail(new List<string> { "pete@rapleafdemo.com", "steve@rapleafdemo.com", "caitlin@rapleafdemo.com" }, true);
        foreach (Dictionary<string, Object> entry in response)
        {
          Console.WriteLine("--- entry ---");
          foreach (KeyValuePair<string, Object> kvp in entry)
          {
            printKeyValuePair(kvp);
          }
        }
      }
      catch (WebException e)
      {
        Console.WriteLine(e.Message);
      }
      Console.ReadLine();
    }

    private static void printKeyValuePair(KeyValuePair<String, Object> kvp)
    {
      if (kvp.Value is Dictionary<String, Object>)
      {
        Console.WriteLine("--" + kvp.Key + "--");
        foreach (KeyValuePair<string, Object> sub_kvp in (Dictionary<String, Object>)kvp.Value)
        {
          printKeyValuePair(sub_kvp);
        }
        Console.WriteLine("--" + kvp.Key + "--");
      }
      else
      {
        Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
      }
    }
  }
}
