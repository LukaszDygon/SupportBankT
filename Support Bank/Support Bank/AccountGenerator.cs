using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Support_Bank
{
    class AccountGenerator
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static List<PersonalAccount> GenerateAccountsFromTransactionList(List<Transaction> transactionList)
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

        public static List<PersonalAccount> GenerateAccountsFromTransactionList(List<Transaction> transactionList, List<PersonalAccount> personalAccounts)
        {
            logger.Info("Generating User Accounts from the Transaction list.");

            foreach (var transaction in transactionList)
            {
                AddTransactionToRelevantAccounts(transaction, personalAccounts);
            }

            logger.Info($"{personalAccounts.Count} User Accounts generated successfully.");

            return personalAccounts;
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
    }
}
