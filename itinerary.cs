using System;
using System.Collections.Generic;
using System.Text;

namespace SearchFlights
{
  /* This class holds all of the valid itineraries that are retrieved from the API call */
  public class Itinerary : IComparable<Itinerary>
  {
    public string flightNumber;
    public string origin;
    public string destination;
    public DateTime departureTime;
    public DateTime arrivalTime;
    public decimal price;
    public string currency;


    public int CompareTo(Itinerary other)
    {
      // This should sort by price, cheapest first. If price is the same, sort by departure time, earliest first
      if (this.price == other.price)
      {
        return this.departureTime.CompareTo(other.departureTime);
      } else {
        return this.price.CompareTo(other.price);
      }
    }
  }
}
