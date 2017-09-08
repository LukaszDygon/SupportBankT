using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog;
using Newtonsoft.Json;

namespace Support_Bank
{
    class TransactionJsonFileReader : ITransactionFileReader
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

         public List<Transaction> Read(string fileName)
         {
            try
            {
                string jsonText = File.ReadAllText(fileName);
                var transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonText);

                return transactions;
            }
            catch (Exception e)
            {
                logger.Error($"While attempting to read JSON: {e}");

                throw (e);
            }
        }

    }
}
