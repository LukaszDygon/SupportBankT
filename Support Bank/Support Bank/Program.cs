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
            string csvFilename = @"C:\Users\LUD\Documents\Training\SupportBankT\Support Bank\Support Bank\resources\Transactions2015.csv";
            var transactionList = ReadTransactionsFromCSV(csvFilename);
            var personalAccounts = GenerateAccountsFromTransactionList(transactionList);

            RunConsolePrompt(personalAccounts);

            
            Console.ReadKey();
            logger.Info("Program Terminated");
        }

        private static void RunConsolePrompt(List<PersonalAccount> personalAccounts)
        {
            logger.Info("Stariting the user input console.");
            while (true)
            {
                Console.Write("> ");
                string userinput = Console.ReadLine();
                logger.Info($"New user request: {userinput}");

                var userInputArray = userinput.Split(" "[0]);
                

                if (userInputArray[0] == "List")
                {
                    string userName = string.Join(" ", userInputArray.Skip(1));

                    if (userName == "All")
                    {
                        ShowAllAccountsInformation(personalAccounts);
                    }
                    else
                    {
                        ShowSingleAccountInformation(userName, personalAccounts);
                    }
                }
                else
                {
                    Console.WriteLine("Unrecognized command");
                }

            }
        }
        private static List<PersonalAccount> GenerateAccountsFromTransactionList(List<Transaction> transactionList)
        {
            logger.Info("Generating User Accounts from the Transaction list.");
            var personalAccounts = new List<PersonalAccount>();
            foreach (var transaction in transactionList)
            {
                AddTransactionToAccount(transaction, personalAccounts);
            }

            logger.Info($"{personalAccounts.Count} User Accounts generated successfully.");

            return personalAccounts;
        }

        private static List<Transaction> ReadTransactionsFromCSV(string fileName)
        {
            logger.Info($"Reading Transactions from the CSV file {fileName}");
            try
            {
                var csv = InitializeCSVReader(fileName);

                var transactions = csv.GetRecords<Transaction>();

                logger.Info($"Transactions Read successfully.");

                return transactions.ToList();
            }
            catch (Exception e)
            {
                logger.Error($"While attempting to read CSV: {e}");
                throw (e);
            }
        }

        private static void AddTransactionToAccount(Transaction transaction, List<PersonalAccount> personalAccounts)
        {
            int outgoingAccountIndex = personalAccounts.FindIndex(p => p.AccountName == transaction.From);
            int receivingAccountIndex = personalAccounts.FindIndex(p => p.AccountName == transaction.To);
            if (outgoingAccountIndex < 0)
            {
                outgoingAccountIndex = personalAccounts.Count;
                personalAccounts.Add(new PersonalAccount(transaction.From));
                
            }
            if (receivingAccountIndex < 0)
            {
                receivingAccountIndex = personalAccounts.Count;
                personalAccounts.Add(new PersonalAccount(transaction.To));
            }
            personalAccounts[outgoingAccountIndex].OutgoingTransactionLog.Add(transaction);
            personalAccounts[receivingAccountIndex].IncomingTransactionLog.Add(transaction);
        }

        private static void ShowAllAccountsInformation(List<PersonalAccount> personalAccounts)
        {
            foreach(var account in personalAccounts)
            {
                Console.WriteLine($"\n{account.AccountName}");
                ShowSingleAccountInformation(account.AccountName, personalAccounts);
            }
        }

        private static void ShowSingleAccountInformation(string accountName, List<PersonalAccount> personalAccounts)
        {
            try
            {
                var personalAccount = personalAccounts.Where(x => x.AccountName == accountName).ToArray()[0];
                Console.WriteLine($"Owes {personalAccount.Owes().ToString("N2")}");
                Console.WriteLine($"Owed {personalAccount.Owed().ToString("N2")}");
            }
            catch
            {
                Console.WriteLine("No account with given name found.");
            }
        }

        private static CsvReader InitializeCSVReader(string fileName)
        {
            var csv = new CsvReader(File.OpenText(fileName));
            csv.Configuration.IgnoreReadingExceptions = true;
            csv.Configuration.ReadingExceptionCallback = (ex, row) =>
            {

                logger.Warn($"Could not format row {row.Row}: {row.Row}. Skipping.");
            };

            return csv;
        }
    }
}
