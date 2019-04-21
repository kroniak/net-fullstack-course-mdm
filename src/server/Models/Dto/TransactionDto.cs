using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.Dto
{
    public class TransactionDto
    {
        [JsonProperty]
        public string CardNumber_From { get; set; }
        [JsonProperty]
        public string CardNumber_To { get; set; }
        [JsonProperty]
        public decimal Card_Number { get; set; }
        [JsonProperty]
        public decimal Card_Debit { get; set; }
        [JsonProperty]
        public DateTime EntryDate { get; set; }
    }
}
