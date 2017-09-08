using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.IO;
using Newtonsoft.Json;

namespace Support_Bank
{
    class TransactionJsonFileWriter : ITransactionFileWriter
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public void Write(string fileName, List<Transaction> transactions)
        {
            var json = JsonConvert.SerializeObject(transactions, Formatting.Indented);
            using (var writer = new StreamWriter(fileName))
            {
                writer.Write(json);
            }
        }
    }
}
