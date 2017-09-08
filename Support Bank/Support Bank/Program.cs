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

            var transactions = ReadAllTransactions(transactionFileNames);
            var personalAccounts = AccountGenerator.GenerateAccountsFromTransactionList(transactions);

            TransactionCommandConsole.RunConsolePrompt(personalAccounts, transactions);

            Console.ReadKey();
            logger.Info("Program Terminated");
        }

        public static List<Transaction> ReadAllTransactions(string[] transactionFileNames)
        {
            var transactionReader = new TransactionFileReader();
            var transactions = new List<Transaction>();

            foreach (var fileName in transactionFileNames)
            {
                transactions.AddRange(transactionReader.Read(fileName));
            }
            return transactions;
        }
    }
}
