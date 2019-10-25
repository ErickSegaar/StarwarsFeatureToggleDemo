using System.Collections.Generic;

namespace StarwarsWeb.HardProviders
{
    public class HardStarshipProvider
    {
        public IList<Starship> GetStarships()
        {
            return new List<Starship> 
            { 
                new Starship { Name ="Executor", Model = "Executor-class star dreadnought", Manufacturer = "Kuat Drive Yards, Fondor Shipyard" },
                new Starship { Name = "Sentinel-class landing craft", Model = "Sentinel-class landing craft", Manufacturer = "Kuat Drive Yards, Fondor Shipyard" },
                new Starship { Name ="Death Star", Model = "Playmobil", Manufacturer = "Jean-Luc Piccard" } 
            };
        }
    }
}
