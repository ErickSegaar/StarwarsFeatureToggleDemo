using Newtonsoft.Json;
using System.Collections.Generic;

namespace StarwarsWeb.Models
{
    public class StarwarsPeopleViewModel
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "results")]
        public IList<Peoples> Peoples { get; set;}
    }
}
    