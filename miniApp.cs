using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SearchFlights
{
  /* This class will:
   * display airport items (airport code, city, and country a few pages at a time.
   Allow page scrolling via keyboard, one page at a time:
   perhaps allowing airport code entry
   */
  public class selectAirportsMiniApp
  {
    List<Airport> listOfAirPorts = new List<Airport>();
    int maxItems;
    public selectAirportsMiniApp(JObject Airports, int maxItemsPage = 10) {
      // Constructor, load the list - divide it into pages. Allow scrolling.
      
      foreach(var kvp in Airports) {
        Airport item = new Airport(kvp.Key, kvp.Value["name"].ToString());
        listOfAirPorts.Add(item);
      }

      maxItems = maxItemsPage;
    }

    public void Start() 
    {
      int count = 0;
      foreach (Airport ap in listOfAirPorts)
      {
        count++;
        if (count <= maxItems)
        {
          Console.WriteLine(ap.Code + " : " + ap.Name);
        }
        else
        {
          Console.WriteLine("Press a key to continue");
          Console.ReadKey();
          count = 0;

          Console.Clear();
          Console.WriteLine("Airports and Codes");
          Console.WriteLine("=================");
        }
      }
    }
  }

  public class Airport
  {
    private string _airportCode;
    public string Code {
      get {
        return _airportCode;
      }
    }

    private string _name;
    public string Name {
      get {
        return _name;
      }
    }

    public Airport(string p_code, string p_name)
    {
      _airportCode = p_code;
      _name = p_name;
    }
  }
}
