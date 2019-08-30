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
      int iValue = 0;
      displayAirportPage(iValue);
      while (true)
      {
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        
        if (keyInfo.Key == ConsoleKey.DownArrow) {
          if (displayAirportPage(iValue)) {
            iValue++;
          } else {
            iValue = listOfAirPorts.Count - 1;
          }
        } else if (keyInfo.Key == ConsoleKey.UpArrow) {
         
          if (displayAirportPage(iValue))
          {
            iValue--;
          } else {
            iValue = 0;
          }
          
        } else {
          return;
        }
        Console.WriteLine("Page " + iValue);
      }
    }

    private bool displayAirportPage(int index) {

      Console.Clear();
      int startPos = index * maxItems;
      int endPos = index * maxItems + maxItems;
      
      for (int i = startPos; i <= endPos; i++) {
        if (i < listOfAirPorts.Count && i >= 0)
        {
          Console.WriteLine(listOfAirPorts[i].Code + " " + listOfAirPorts[i].Name);

        } else {
          return false;
        }
      }
     
      return true;
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
