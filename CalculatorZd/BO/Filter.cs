using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BO
{
    public class Filter
    {
        [JsonProperty("id")]
        public string id;

        [JsonProperty("name")]
        public string name;

    }
}
