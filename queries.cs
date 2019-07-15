using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SearchFlights
{
  /* This static class will handle all our API calls */
  public static class Queries {
    const string URI_AIRPORTS = "https://desktopapps.ryanair.com/en-gb/res/stations";

    public static async Task<string> getAirports() {
      // Call API endpoint to get gets us a JSON Object of all the airport destinations for RyanAir
      using (HttpClient client = new HttpClient()){
        try {
          return await client.GetStringAsync(URI_AIRPORTS);
        } catch (Exception ex) {
          Console.WriteLine(ex.Message);
          return null;
        }
      }
    }
  }
}
