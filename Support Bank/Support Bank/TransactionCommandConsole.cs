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

        public static void RunConsolePrompt(List<PersonalAccount> personalAccounts)
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
                    string userNameOrAll = string.Join(" ", userInputArray.Skip(1));    // format remainder of the command to a single name or "All" keyword 

                    if (userNameOrAll == "All")
                    {
                        ShowAllAccountsInformation(personalAccounts);
                    }
                    else
                    {
                        ShowSingleAccountInformation(userNameOrAll, personalAccounts);
                    }
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
    }
}
