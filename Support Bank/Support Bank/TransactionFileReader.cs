using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using CsvHelper;

namespace Support_Bank
{
    class TransactionFileReader : ITransactionFileReader
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public List<Transaction> Read(string fileName)
        {
            string fileExtension = fileName.Split('.').Last();

            logger.Info($"Reading Transactions from the {fileExtension.ToUpper()} file {fileName}");

            var reader = GetReaderForExtension(fileExtension);
            var transactions = reader.Read(fileName);

            logger.Info($"Transactions Read successfully.");

            return transactions;
        }

        public List<Transaction> ReadAllTransactions(string[] transactionFileNames)
        {
            var transactions = new List<Transaction>();

            foreach (var fileName in transactionFileNames)
            {
                transactions.AddRange(Read(fileName));
            }
            return transactions;
        }

        private ITransactionFileReader GetReaderForExtension(string extension)
        {
            extension = extension.ToLower();
            var transactions = new List<Transaction>();

            if (extension == "csv")
            {
                return new TransactionCsvFileReader();
            }
            if (extension == "json")
            {
                return new TransactionJsonFileReader();
            }
            if (extension == "xml")
            {
                return new TransactionXmlFileReader();
            }
            else
            {
                logger.Error($"File with invalid extension {extension} provided");

                throw (new Exception($"File with invalid extension {extension} provided. Program supports only JSON, CSV and XML formats"));
            }
        }

    }

}
