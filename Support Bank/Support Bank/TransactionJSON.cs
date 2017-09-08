using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Support_Bank
{
    public class TransactionJSON
    {
        [JsonProperty("Transaction")]
        public Transaction Transaction { get; set; }
    }
}
