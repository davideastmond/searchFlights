using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;

namespace SearchFlights
{
  /* This static class will handle all our API calls and process data received */
  public static class Queries {
    const string URI_AIRPORTS = "https://desktopapps.ryanair.com/en-gb/res/stations";
    public static JObject airportDataObject;

    public static async Task<string> GetAirports(string uri_default = URI_AIRPORTS) {
      // Call API endpoint to get gets us a JSON Object of all the airport destinations for RyanAir.
      // We'll stash this info in a static object so it can be queried without making an API call
      using (HttpClient client = new HttpClient()){
        try {
          var returnValue = await client.GetStringAsync(uri_default);
          airportDataObject = JObject.Parse(returnValue);
          return returnValue;
        } catch (Exception ex) {
          Console.WriteLine(ex.Message);
          return null;
        }
      }
    }

    public static async Task<string> GetFlights(UserInput flightParams) {
      // Create an API end point based on the user's validated input using string interpolation
      string dateFormatForURL = flightParams.departureDate.ToString("yyyy-MM-dd");
      string api_endPoint = $"https://desktopapps.ryanair.com/v4/en-gb/availability?ADT={flightParams.numberAdults.ToString()}&CHD=0&DateOut={dateFormatForURL}&Destination={flightParams.destination}&FlexDaysOut=2&INF=0&IncludeConnectingFlights=true&Origin={flightParams.origin}&RoundTrip=false&TEEN=0&ToUs=AGREED&exists=false";
      using (HttpClient client = new HttpClient()) 
      {
        try {
          return await client.GetStringAsync(api_endPoint);
        } catch (Exception ex) {
          Console.WriteLine(ex.Message);
          return null;
        }
      }
    }


    public static Tuple<bool, List<Itinerary>> ProcessResults (JObject parsedResultsObj, UserInput input) {
      // This method processes the results from a flight search, parses the JSON etc

      // Grab the trips from the parsedResultsObj variable
      var trips = parsedResultsObj["trips"];
      foreach (var trip in trips) {
        foreach (var tripDate in trip["dates"]) {
          // Grab a date from the date object
          DateTime flightDateOut = GrabDate(tripDate["dateOut"].ToString());

          // Find the dateOut that matches inputDate and grab the flights
          if (DateTime.Compare(flightDateOut.Date, input.departureDate.Date) == 0) {
            /* We have to iterate through flights based on the inputted date that is matching. 
             * We'll get back a tuple<bool, List<Itinerary>> */
            Console.WriteLine($"Flight(s) for {flightDateOut.Date.ToString("yyyy-MM-dd")}");
            return MakeListFromFlightFareResults(tripDate, input.origin, input.destination, parsedResultsObj["currency"].ToString());
            
          }
        }
      }
      // Nothing is found, return false and a null list
      return new Tuple<bool, List<Itinerary>>(false, null);
    }

    static DateTime GrabDate (string DateData) {
      /* Helper function that converts a date in string format to DateTime */
      DateTime retDate;
      DateTime.TryParse(DateData, out retDate);
      return retDate;
    }

    static Tuple<bool, List<Itinerary>> MakeListFromFlightFareResults (JToken tripDateFlights, string pOrigin, string pDestination, string pCurrency) {
      // This function creates a list object that is going to store all of our flight info
      // making it easier to display it later

      var obj = (JArray)tripDateFlights["flights"];
      if (obj.Count <= 0) {
        return new Tuple<bool, List<Itinerary>>(false, null);
      }

      List<Itinerary> listOfFlights = new List<Itinerary>();
      foreach (var flight in tripDateFlights["flights"])
      {
        // If there are fares available, then we can build up an itinerary object to return
        // I use fareObject to ensure that the key is not null so I can iterate over regularFare.fares
        var fareObject = flight["regularFare"];

        if (fareObject != null)
        {
          foreach (var fare in flight["regularFare"]["fares"])
          {
            Itinerary it = new Itinerary();
            it.flightNumber = flight["flightNumber"].ToString();
            it.origin = pOrigin;
            it.destination = pDestination;
            it.departureTime = GrabDate(flight["time"][0].ToString());
            it.arrivalTime = GrabDate(flight["time"][1].ToString());
            it.price = decimal.Parse(fare["amount"].ToString());
            it.currency = pCurrency;
            listOfFlights.Add(it);
          }
        }
      }

      /* At the end of the above loop, if the list count is zero, return false and null,
       * otherwise return true, and the list object itself.
      */
      if (listOfFlights.Count > 0) {
        return new Tuple<bool, List<Itinerary>>(true, listOfFlights);
      }
      return new Tuple<bool, List<Itinerary>>(false, null);
    }

    public static void PrintOutFlights(List<Itinerary> itin) {
      // Sort the List first, then print it out
      itin.Sort();
      foreach(Itinerary i in itin) {
        Console.WriteLine($"{i.flightNumber} {i.origin} --> {i.destination} ({i.departureTime} --> {i.arrivalTime}) - {i.price} {i.currency}");
      }
    }
  }
}
