using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.IO;
using CsvHelper;

namespace Support_Bank
{
    class TransactionCsvFileReader : ITransactionFileReader
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public List<Transaction> Read(string fileName)
        {
            try
            {
                var csvReader = InitializeCSVReader(fileName);
                var transactions = csvReader.GetRecords<Transaction>();

                return transactions.ToList();
            }
            catch (Exception e)
            {
                logger.Error($"While attempting to read CSV: {e}");

                throw (e);
            }
        }

        private static CsvReader InitializeCSVReader(string fileName)
        {
            var csvReader = new CsvReader(File.OpenText(fileName));
            csvReader.Configuration.IgnoreReadingExceptions = true;
            csvReader.Configuration.ReadingExceptionCallback = (ex, row) =>
            {

                logger.Warn($"Could not format row: {row.Row}. Skipping.");
            };

            return csvReader;
        }
    }
}
