using System.Collections.Generic;
using System;
using System.Net;

namespace personalization
{
  class RapleafExample
  {
    public static void Main(string[] args) {
      RapleafApi api = new RapleafApi("SET_ME"); // Set API key here
      try {
        Dictionary<string, Object> response = api.queryByEmail("dummy@rapleaf.com", true);
        foreach (KeyValuePair<string, Object> kvp in response)
        {
          printKeyValuePair(kvp);
        }
      } catch (WebException e) {
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
