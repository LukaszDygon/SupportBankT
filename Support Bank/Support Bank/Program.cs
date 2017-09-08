using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using NLog;

namespace Support_Bank
{
    class Program
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Info("Starting the program.");
            string csv2015Filename = @"..\..\resources\Transactions2015.csv";
            string csv2014Filename = @"..\..\resources\Transactions2014.csv";
            string jsonFilename = @"..\..\resources\Transactions2013.json";
            string xmlFilename = @"..\..\resources\Transactions2012.xml";

            var transactionReader = new TransactionFileReader();
            var transactionList = transactionReader.Read(jsonFilename);
            transactionList.AddRange(transactionReader.Read(csv2014Filename));
            transactionList.AddRange(transactionReader.Read(csv2015Filename));
            transactionList.AddRange(transactionReader.Read(xmlFilename));
            var personalAccounts = AccountGenerator.GenerateAccountsFromTransactionList(transactionList);

            TransactionCommandConsole.RunConsolePrompt(personalAccounts, transactionList);

            Console.ReadKey();
            logger.Info("Program Terminated");
        }
    }
}
