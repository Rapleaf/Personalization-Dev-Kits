using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using personalization;

namespace MyApplication
{
  class RapleafExample
  {
    public static void Main(string[] args)
    {
      RapleafApi api = new RapleafApi("SET_ME"); // Set API key here
      try
      {
        Dictionary<string, string> response = api.queryByEmail("dummy@rapleaf.com", true);
        foreach (KeyValuePair<string, string> kvp in response)
        {
          Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
        }
      }
      catch (System.Net.WebException e)
      {
        Console.WriteLine(e.Message);
      }
      Console.ReadLine();
    }
  }
}
