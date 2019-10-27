using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarwarsWeb.Models
{
    public class Planet
    {
        [JsonProperty(PropertyName = "name")] 
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "climate")] 
        public string Climate { get; set; }
        
        [JsonProperty(PropertyName = "terrain")] 
        public string Terrain { get; set; }

    }

}
