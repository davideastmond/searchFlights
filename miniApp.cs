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
      // This is a 'mini-app' that allows scrolling through a list of airports and codes as fetched by the API.
      // Use the up and down arrow to scroll. Any other key will abort the app and force airport code to be entered.
      int iValue = 0;

      // Show the first page of the airport destinations and codes as soon as the mini-app is initialized.
      // Then start a loop that accepts up, down or other input to scroll up, down or abort the interface.
      displayAirportPage(iValue);
      while (true)
      {
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        
        // When the up or down keyboard input is accepted, we'll bump up or down the iValue - basically our pages, allowing us to scroll
        // through the pages of codes and destinations, back and forth. We call a function that displays the items based on the page. If there was
        // an error displaying the items, then we don't refresh the page with new items, otherwise, we bump up or down the iValue allowing a page
        // change.
        if (keyInfo.Key == ConsoleKey.DownArrow) {
          iValue++;
          if (displayAirportPage(iValue)) {
            
            Console.WriteLine("Page " + iValue);
          } else {
            iValue--;
          }
        } else if (keyInfo.Key == ConsoleKey.UpArrow) {
          iValue--;
          if (displayAirportPage(iValue))
          {
            Console.WriteLine("Page " + iValue);
          } else {
            iValue++;
          }
          
        } else {
          Console.WriteLine("Page " + iValue);
          return;
        }
 
      }
    }

    private bool displayAirportPage(int index) {
      int startPos = index * maxItems;
      int endPos = index * maxItems + maxItems;
      
      if (index >= 0 && index < listOfAirPorts.Count) {
        Console.Clear();
      }
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
