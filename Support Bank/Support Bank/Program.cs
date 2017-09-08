using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using NLog;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace Support_Bank
{
    
    class Program
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Info("Starting the program.");
            string csvFilename = @"C:\Users\LUD\Documents\Training\SupportBankT\Support Bank\Support Bank\resources\Transactions2015.csv";
            string jsonFilename = @"C:\Users\LUD\Documents\Training\SupportBankT\Support Bank\Support Bank\resources\Transactions2013.json";
            string xmlFilename = @"C:\Users\LUD\Documents\Training\SupportBankT\Support Bank\Support Bank\resources\Transactions2012.xml";

            //var transactionList = ReadTransactionsFromJSON(jsonFilename);
            //var transactionList = ReadTransactionsFromCSV(csvFilename);
            var transactionList = ReadTransactionsFromXML(xmlFilename);
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
                string userInput = Console.ReadLine();
                logger.Info($"New user request: {userInput}");

                var userInputArray = userInput.Split(' ');
                

                if (userInputArray[0] == "List")
                {
                    string userNameOrAll = string.Join(" ", userInputArray.Skip(1));

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

        private static List<PersonalAccount> GenerateAccountsFromTransactionList(List<Transaction> transactionList)
        {
            logger.Info("Generating User Accounts from the Transaction list.");

            var personalAccounts = new List<PersonalAccount>();
            foreach (var transaction in transactionList)
            {
                AddTransactionToRelevantAccounts(transaction, personalAccounts);
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

        private static List<Transaction> ReadTransactionsFromJSON(string fileName)
        {
            logger.Info($"Reading Transactions from the JSON file {fileName}");
            try
            {
                string jsonText = File.ReadAllText(fileName);
                var transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonText);

                logger.Info($"Transactions Read successfully.");

                return transactions;
            }
            catch (Exception e)
            {
                logger.Error($"While attempting to read JSON: {e}");
                throw (e);
            }
        }

        private static List<Transaction> ReadTransactionsFromXML(string fileName)
        {
            XDocument document = XDocument.Load(fileName);
            List<Transaction> transactions = new List<Transaction>();

            foreach (XElement transactionXML in document.Element("TransactionList").Elements("SupportTransaction"))
            {
                transactions.Add(ConvertXmlNodeToTransaction(transactionXML));
            }

            return transactions;
        }

        private static Transaction ConvertXmlNodeToTransaction(XElement transactionXML)
        {
            DateTime date = DateTime.FromOADate(Double.Parse(transactionXML.Attribute("Date").Value));
            string from = transactionXML.Element("Parties").Element("From").Value;
            string to = transactionXML.Element("Parties").Element("To").Value;
            string narrative = transactionXML.Element("Description").Value;
            double amount = Double.Parse(transactionXML.Element("Value").Value);
            var transaction = new Transaction(date, to, from, narrative, amount);

            return transaction;
        }

        private static void AddTransactionToRelevantAccounts(Transaction transaction, List<PersonalAccount> personalAccounts)
        {
            //change to avoid using index
            var outgoingAccount = personalAccounts.FirstOrDefault(p => p.AccountName == transaction.From);
            var receivingAccount = personalAccounts.FirstOrDefault(p => p.AccountName == transaction.To);

            if (outgoingAccount == null)
            {
                outgoingAccount = new PersonalAccount(transaction.From);
                personalAccounts.Add(outgoingAccount);
                
            }
            if (receivingAccount == null)
            {
                receivingAccount = new PersonalAccount(transaction.To);
                personalAccounts.Add(receivingAccount);
            }

            receivingAccount.AddOutgoingTransaction(transaction);
            outgoingAccount.AddIncomingTransaction(transaction);
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
                var personalAccount = personalAccounts.First(x => x.AccountName == accountName);
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
