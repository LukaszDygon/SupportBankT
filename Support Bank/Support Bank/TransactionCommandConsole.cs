using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Support_Bank
{
    class TransactionCommandConsole
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static void RunConsolePrompt(List<PersonalAccount> personalAccounts, List<Transaction> transactions)
        {
            logger.Info("Stariting the user input console.");
            while (true)
            {
                Console.Write("> ");
                string userInput = Console.ReadLine();
                logger.Info($"New user request: {userInput}");

                var userInputArray = userInput.Split(' ');
                if (userInputArray[0] == "List")
                {
                    string argument = string.Join(" ", userInputArray.Skip(1));    // format remainder of the command to a single name or "All" keyword 
                    if (argument == "All")
                    {
                        ShowAllAccountsInformation(personalAccounts);
                    }
                    else
                    {
                        ShowSingleAccountInformation(argument, personalAccounts);
                    }
                }
                else if ($"{userInputArray[0]} {userInputArray[1]}" == "Import File")
                {
                    string argument = string.Join(" ", userInputArray.Skip(2));    // format remainder of the command to a single name or "All" keyword 
                    ReadFromFile(argument, transactions, personalAccounts);
                }
                else if ($"{userInputArray[0]} {userInputArray[1]}" == "Export File")
                {
                    string argument = string.Join(" ", userInputArray.Skip(2));    // format remainder of the command to a single name or "All" keyword 
                    WriteToFile(argument, transactions);
                }
                else
                {
                    Console.WriteLine("Unrecognized command");
                }
            }
        }

        private static void ShowAllAccountsInformation(List<PersonalAccount> personalAccounts)
        {
            foreach (var account in personalAccounts)
            {
                Console.WriteLine($"\n{account.AccountName}");
                ShowSingleAccountInformation(account.AccountName, personalAccounts);
            }
        }

        private static void ShowSingleAccountInformation(string accountName, List<PersonalAccount> personalAccounts)
        {
            try
            {
                var personalAccount = personalAccounts.First(x => x.AccountName == accountName);
                Console.WriteLine($"Owes {personalAccount.Owes().ToString("N2")}");
                Console.WriteLine($"Owed {personalAccount.Owed().ToString("N2")}");
            }
            catch
            {
                Console.WriteLine("No account with given name found.");
            }
        }

        private static void WriteToFile(string fileName, List<Transaction> transactions)
        {
            var fileWriter = new TransactionFileWriter();
            fileWriter.Write(fileName, transactions);
        }

        private static void ReadFromFile(string fileName, List<Transaction> transactions, List<PersonalAccount> accounts)
        {
            var newTransactions = new TransactionFileReader().Read(fileName);
            AccountGenerator.GenerateAccountsFromTransactionList(newTransactions, accounts);
            transactions.AddRange(newTransactions);
        }
    }
}
