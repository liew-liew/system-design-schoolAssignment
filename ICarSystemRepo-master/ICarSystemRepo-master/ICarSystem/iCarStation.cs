using System;
using System.Collections.Generic;

namespace ICarSystem
{
    public class iCarStation
    {
        public string StationID { get; private set; }
        public string Location { get; private set; }
        private List<Vehicle> availableCars;

        public iCarStation(string stationID, string location)
        {
            StationID = stationID;
            Location = location;
            availableCars = new List<Vehicle>();
        }
    }
}
