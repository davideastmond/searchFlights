using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Diagnostics;
namespace SearchFlights
{
  public static class Validations
  {
    public static JObject airportDataObject;
    /* These are our validation functions that will return false if the user input is invalid */
    public static Tuple<bool, DateTime> validateUserDateInput(string dataInput)
    {
      // Validate the adult count
      dataInput = dataInput.Trim();
      if (dataInput == "")
      {
        return new Tuple<bool, DateTime>(false, DateTime.Now);
      }

      DateTime value;
      if (DateTime.TryParse(dataInput, out value))
      {
        return new Tuple<bool, DateTime>(true, value);
      }
      return new Tuple<bool, DateTime>(false, DateTime.Now);
    }

    public static Tuple<bool, int> validateUserAdultCountInput(string dataInput)
    {
      // Validate the adult count
      dataInput = dataInput.Trim();
      if (dataInput == "")
      {
        return new Tuple<bool, int>(false, -1);
      }

      int value;

      if (int.TryParse(dataInput, out value))
      {
        return new Tuple<bool, int>(true, value);
      }
      return new Tuple<bool, int>(false, -1);
    }

    public static Tuple<bool, string> validateOriginDestination(string dataInput)
    {
      // Make sure the user's input for origin or destination string isn't empty.
      // API call to RyanAir to get a list of their destinations / origins to make sure that the user entered a valid dest/origin.
      dataInput = dataInput.Trim().ToUpper();
      if (dataInput == "")
      {
        return new Tuple<bool, string>(false, null);
      }

      // Do an API call to get a list of airport destinations. Parse the info and covert to a JS object.
      Task<string> result = Task.Run(() =>
        Queries.getAirports()
      );

      // Check airport codes
      if (airportDataObject.ContainsKey(dataInput)) 
      {
        return new Tuple<bool, string>(true, dataInput);
      }
      return new Tuple<bool, string>(false, null);
    }
    public static void getAirPorts() {
      // Do an API call to get a list of airport destinations. Parse the info and covert to a JS object.
      Task<string> result = Task.Run(() =>
        Queries.getAirports()
      );

      airportDataObject = JObject.Parse(result.Result);
    }
  }
}
