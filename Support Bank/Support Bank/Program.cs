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

            var directory = new DirectoryInfo(@"..\..\resources");
            string[] transactionFileNames = directory.GetFiles().Select(x => x.FullName).ToArray();

            var transactions = new List<Transaction>();     // ReadAllTransactions(transactionFileNames);
            var personalAccounts = new List<PersonalAccount>();     // AccountGenerator.GenerateAccountsFromTransactionList(transactions);

            TransactionCommandConsole.RunConsolePrompt(personalAccounts, transactions);

            Console.ReadKey();
            logger.Info("Program Terminated");
        }

        public static List<Transaction> ReadAllTransactions(string[] transactionFileNames)
        {
            var transactionFileReader = new TransactionFileReader();
            var transactions = transactionFileReader.ReadAllTransactions(transactionFileNames);

            return transactions;
        }
    }
}
