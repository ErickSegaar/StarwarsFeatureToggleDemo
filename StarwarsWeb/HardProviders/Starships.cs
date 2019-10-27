using Newtonsoft.Json;
using System.Collections.Generic;

namespace StarwarsWeb.HardProviders
{
    public class Starships
    {
        [JsonProperty(PropertyName = "results")]
        public IList<Starship> List { get; set; }

    }
}
