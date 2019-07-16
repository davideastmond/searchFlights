using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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

    public static async Task<string> getFlights(UserInput flightParams) {
      // Create an API end point based on the user's validated input
      string dateFormatForURL = flightParams.departureDate.ToString("yyyy-MM-dd");
      string api_endpoint = $"https://desktopapps.ryanair.com/v4/en-gb/availability?ADT={flightParams.numberAdults.ToString()}&CHD=0&DateOut={dateFormatForURL}&Destination={flightParams.destination}&FlexDaysOut=2&INF=0&IncludeConnectingFlights=true&Origin={flightParams.origin}&RoundTrip=false&TEEN=0&ToUs=AGREED&exists=false";
      using (HttpClient client = new HttpClient()) 
      {
        try {
          return await client.GetStringAsync(api_endpoint);
        } catch (Exception ex) {
          Console.WriteLine(ex.Message);
          return null;
        }
      }
    }

    public static void ProcessResults (JObject parsedResultsObj, UserInput input) {
      /* This method processes the results from a flight search */

      // As a test, let's print out the dates
      var trips = parsedResultsObj["trips"];
      foreach (var trip in trips) {
        foreach (var tripDate in trip["dates"]) {
          Console.WriteLine("Here are the trip dates: " + tripDate);
        }
      }
    }
  }
}
