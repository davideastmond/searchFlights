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
    /* The Validations Class sanitizes the user's input from the command line. 
     I also store the airport parsed data here so I don't have to make more than one API calls to get the airports list.
     The validate functions all return Tuple<bool, someValue> where someValue is sanitized info we'll put into the UserInput struct */

    
    /* These are our validation functions that will return false if the user input is invalid */
    public static Tuple<bool, DateTime> ValidateUserDateInput(string dataInput)
    {
      dataInput = dataInput.Trim();
      if (dataInput == "")
      {
        return new Tuple<bool, DateTime>(false, DateTime.Now);
      }
      DateTime value;
      if (DateTime.TryParse(dataInput, out value))
      {
        if (DateTime.Compare(value.Date, DateTime.Now.Date) >= 0)
        {
          return new Tuple<bool, DateTime>(true, value);
        } else {
          // Date entered seems to be in the past
          Console.WriteLine("Date is in the past.");
          return new Tuple<bool, DateTime>(false, DateTime.Now);
        }
      }
      return new Tuple<bool, DateTime>(false, DateTime.Now);
    }

    public static Tuple<bool, int> ValidateUserAdultCountInput(string dataInput)
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
        if (value > 0 && value <= 25)
        {
          return new Tuple<bool, int>(true, value);
        } else {
          Console.WriteLine("Number has to be valid. Greater than zero and less than 25");
        }
      }
      return new Tuple<bool, int>(false, -1);
    }

    public static Tuple<bool, string> ValidateOriginDestination(string dataInput)
    {
      // Make sure the user's input for origin or destination string isn't empty.
      // An API call to RyanAir is done when the App starts. It's stored in the airportDataObject variable.
      dataInput = dataInput.Trim().ToUpper();
      if (dataInput == "")
      {
        return new Tuple<bool, string>(false, null);
      }

      // Check airport codes
      if (Queries.airportDataObject.ContainsKey(dataInput)) 
      {
        return new Tuple<bool, string>(true, dataInput);
      }
      return new Tuple<bool, string>(false, null);
    }
  }
}
