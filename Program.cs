using System;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SearchFlights
{
  class Program
  {
    static void Main(string[] args)
    {
      /* Ask the user for the required information in order to process the query
       * 1. Origin
         2. Destination
         3. Departure Date
         4. ADT (Number of adults)
      */
 
      UserInput input = CollectUserInformation();
    }

    static UserInput CollectUserInformation() {
      /* Prompt user for required information and store it in a struct, which will be returned from this function */
      UserInput uInformation;

      /* We need to validate input information */
      while (true)
      {
        Console.WriteLine("Enter Flight Origin:");
        string originInfo = Console.ReadLine();
        Tuple<bool, string> resultInfo = Validations.validateOriginDestination(originInfo);
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
        Tuple<bool, string> resultInfo = Validations.validateOriginDestination(destinationInfo);
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
        Tuple<bool, DateTime> resultInfo = Validations.validateUserDateInput(departureDate);
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
        Tuple<bool, int> resultInfo = Validations.validateUserAdultCountInput(numAdultValue);
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
