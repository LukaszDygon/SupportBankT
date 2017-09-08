using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Newtonsoft.Json;

namespace Support_Bank
{

    public class Transaction
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        [JsonProperty("Date")]
        public DateTime Date { get; set; }

        [JsonProperty("FromAccount")]
        public string From { get; set; }

        [JsonProperty("ToAccount")]
        public string To { get; set; }

        [JsonProperty("Narrative")]
        public string Narrative { get; set; }

        [JsonProperty("Amount")]
        public double Amount { get; set; }

        public Transaction()
        {
            logger.Debug($"Creading an empty transaction object.");
        }

        public Transaction(DateTime date, string from, string to,
                            string narrative, double amount)
        {
            logger.Debug($"Creading a Object {date}, {from}, {to}, {narrative}, {amount}.");
            Date = date;
            From = from;
            To = to;
            Narrative = narrative;
            Amount = amount;
        }
    }
}
