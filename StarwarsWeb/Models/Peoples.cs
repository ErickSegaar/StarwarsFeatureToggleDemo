using Newtonsoft.Json;

namespace StarwarsWeb.Models
{
    public class Peoples
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "mass")]
        public int Mass { get; set; }
    }
}
