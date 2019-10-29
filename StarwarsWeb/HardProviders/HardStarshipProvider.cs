using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarwarsWeb.HardProviders
{
    public class HardStarshipProvider
    {
        public Task<Starships> GetStarships()
        {
            return Task.FromResult(new Starships
            {
                List = new List<Starship> {
                    new Starship { Name = "Executor", Model = "Executor-class star dreadnought", Manufacturer = "Kuat Drive Yards, Fondor Shipyard" },
                    new Starship { Name = "Sentinel-class landing craft", Model = "Sentinel-class landing craft", Manufacturer = "Kuat Drive Yards, Fondor Shipyard" },
                    new Starship { Name ="Death Star", Model = "Playmobil", Manufacturer = "Jean-Luc Piccard" }
                }
            });
        }
    }
}
