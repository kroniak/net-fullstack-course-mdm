using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.Dto
{
    public class InputDataDtoCards
    {
        /// <summary>
        /// Input > Card 
        /// </summary>
        /// <returns>string card  DTO</returns>
        //[JsonProperty]
        //public string CardNumber { get; set; }
        [JsonProperty]
        public string CardName { get; set; }
        [JsonProperty]
        public int UserId { get; set; }
    }
}
