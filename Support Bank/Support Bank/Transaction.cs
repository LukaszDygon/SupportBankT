using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Support_Bank
{
    class Transaction
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Narrative { get; set; }
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
