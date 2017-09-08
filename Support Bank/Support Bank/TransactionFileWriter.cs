using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Support_Bank
{
    class TransactionFileWriter : ITransactionFileWriter
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public void Write(string fileName, List<Transaction> transactions)
        {
            logger.Info($"Writing transactions to {fileName}");

            string extension = fileName.Split('.').Last();
            ITransactionFileWriter fileWriter = GetWriterForExtension(extension);
            fileWriter.Write(fileName, transactions);

            logger.Info($"Successfully written transactions to {fileName}");
            Console.WriteLine($"Successfully written transactions to {fileName}");
        }

        private ITransactionFileWriter GetWriterForExtension(string extension)
        {
            extension = extension.ToLower();
            var transactions = new List<Transaction>();

            if (extension == "csv")
            {
                return new TransactionCsvFileWriter();
            }
            if (extension == "json")
            {
                return new TransactionJsonFileWriter();
            }
            if (extension == "xml")
            {
                return new TransactionXmlFileWriter();
            }
            else
            {
                logger.Error($"File Name with invalid extension {extension} provided");

                throw (new Exception($"File Name with invalid extension {extension} provided. Program supports only JSON, CSV and XML formats"));
            }
        }
    }
}
