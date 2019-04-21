using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.Dto
{
    public class OtputDataDtoCards
    {

        [JsonProperty]
        public string CardNumber { get; set; }
        [JsonProperty]
        public string CardName { get; set; }
        [JsonProperty]
        public DateTime CardValidDate { get; set; }
        [JsonProperty]
        public string User { get; set; }

    }
}
