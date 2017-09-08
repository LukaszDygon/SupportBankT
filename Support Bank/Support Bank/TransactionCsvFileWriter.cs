using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using NLog;


namespace Support_Bank
{
    class TransactionCsvFileWriter : ITransactionFileWriter
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public void Write(string fileName, List<Transaction> transactions)
        {
            var csvWriter = new CsvWriter(new StreamWriter(fileName));
            csvWriter.WriteRecords(transactions);
        }

        
    }
}
