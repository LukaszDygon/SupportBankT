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
        static void Main(string[] args)
        {
            string csvFilename = @"C:\Users\LUD\Documents\Training\SupportBankT\Support Bank\Support Bank\resources\Transactions2014.csv";
            var transactionList = ReadTransactionsCSV(csvFilename);
            var personalAccounts = GenerateAccountsFromTransactionList(transactionList);

            RunConsolePrompt(personalAccounts);

            
            Console.ReadKey();
        }

        private static void RunConsolePrompt(List<PersonalAccount> personalAccounts)
        {
            while (true)
            {
                Console.Write("> ");
                string userinput = Console.ReadLine();
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
            var personalAccounts = new List<PersonalAccount>();
            foreach (var transaction in transactionList)
            {
                AddTransactionToAccount(transaction, personalAccounts);
            }
            return personalAccounts;
        }

        private static List<Transaction> ReadTransactionsCSV(string fileName)
        {
            var csv = new CsvReader(File.OpenText(fileName));
            var transactions = csv.GetRecords<Transaction>();

            return transactions.ToList();
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
                Console.WriteLine($"Owes {personalAccount.Owes().ToString()}");
                Console.WriteLine($"Owed {personalAccount.Owed().ToString()}");
            }
            catch
            {
                Console.WriteLine("No account with given name found.");
            }
        }
    }
}
