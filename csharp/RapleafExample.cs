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
        Dictionary<string, string> response = api.queryByEmail("dummy@rapleaf.com", true);
        foreach (KeyValuePair<string, string> kvp in response)
        {
          Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
        }
      } catch (WebException e) {
        Console.WriteLine(e.Message);
      }
      Console.ReadLine();
    }
  }
}
