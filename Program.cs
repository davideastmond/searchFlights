using System;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SearchFlights
{
  class Program
  {
    static void Main(string[] args)
    {
      /* Call API to get a list of airport codes.
       * Ask the user for the required information in order to process the query
       * 1. Origin
         2. Destination
         3. Departure Date
         4. ADT (Number of adults)

         Query the API and get some results. Parse it into a JObject and then process the object
      */

      Task<string> airportResults = Task.Run(()=> Queries.GetAirports());

      if (airportResults.Result != null)
      {
        UserInput input = CollectUserInformation();
        Console.WriteLine($"Origin: {input.origin} \n Destination: {input.destination} \n Departure Date: {input.departureDate.ToString("yyyy-MM-dd")} \n Adult Count: {input.numberAdults}");
        Task<string> airportQueryResults = Task.Run(() => Queries.GetFlights(input));
        if (airportQueryResults.Result != null)
        {
          JObject resultsJSON = JObject.Parse(airportQueryResults.Result);
          if (resultsJSON != null)
          {
            Queries.ProcessResults(resultsJSON, input);
          } else {
            Console.WriteLine("Unable to obtain results");
          }
        }
        else
        {
          Console.WriteLine("Unable to obtain flight information because the result is empty.");
        }
      }
    }

    static UserInput CollectUserInformation() {
      /* Prompt user for required information and store it in a struct, which will be returned from this function */
      UserInput uInformation;

      /* We need to validate input information */
      while (true)
      {
        Console.WriteLine("Enter Flight Origin:");
        string originInfo = Console.ReadLine();
        Tuple<bool, string> resultInfo = Validations.ValidateOriginDestination(originInfo);
        if (resultInfo.Item1) 
        {
          uInformation.origin = resultInfo.Item2;
          break;
        } else 
        {
          Console.WriteLine("Please enter a valid airport code.");
        }
      }

      while (true)
      {
        Console.WriteLine("Enter Flight Destination:");
        string destinationInfo = Console.ReadLine();
        Tuple<bool, string> resultInfo = Validations.ValidateOriginDestination(destinationInfo);
        if (resultInfo.Item1) 
        {
          uInformation.destination = resultInfo.Item2;
          break;
        } else {
          Console.WriteLine("Please enter a valid airport code.");
        }
      }

      while (true)
      {
        Console.WriteLine("Enter DepartureDate: (YYYY-MM-DD)");
        string departureDate = Console.ReadLine();
        Tuple<bool, DateTime> resultInfo = Validations.ValidateUserDateInput(departureDate);
        if (resultInfo.Item1) 
        {
          uInformation.departureDate = resultInfo.Item2;
          break;
        } else 
        {
          Console.WriteLine("Please enter a valid date.");
        }
      }

      // Ensure a valid integer is received. Keep prompting user until valid integer is entered
      while (true) 
      {
        Console.WriteLine("Enter number of adults:");
        string numAdultValue = Console.ReadLine();
        Tuple<bool, int> resultInfo = Validations.ValidateUserAdultCountInput(numAdultValue);
        if (resultInfo.Item1) {
          uInformation.numberAdults = resultInfo.Item2;
          break;
        } else 
        {
          Console.WriteLine("Please enter a valid integer.");
        }
      }
      return uInformation;
    }
  }
}
