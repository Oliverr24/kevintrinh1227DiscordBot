using Newtonsoft.Json;

namespace kevintrinh1227 {
    public class ConfigJson {

        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }

    }
}
