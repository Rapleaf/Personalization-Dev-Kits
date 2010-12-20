package com.rapleaf.api.personalization;
import org.json.JSONObject;

public class RapleafExample {
  public static void main(String[] args) {
    RapleafApi api = new RapleafApi("SET_ME");        // Set API key here
    try {
      JSONObject response = api.queryByEmail("dummy@rapleaf.com");
      System.out.println(response);
    } catch (Exception e) {
      e.printStackTrace();
    }
  }
}