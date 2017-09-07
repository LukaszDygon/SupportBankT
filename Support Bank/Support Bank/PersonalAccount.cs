using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support_Bank
{
    class PersonalAccount
    {
        public string AccountName { get; }
        public List<Transaction> IncomingTransactionLog { get; set; }
        public List<Transaction> OutgoingTransactionLog { get; set; }


        public PersonalAccount(string accountName)
        {
            AccountName = accountName;
            IncomingTransactionLog = new List<Transaction>();
            OutgoingTransactionLog = new List<Transaction>();
        }

        public double Owed()
        {
            return IncomingTransactionLog.Sum(x => x.Amount);
        }

        public double Owes()
        {
            return OutgoingTransactionLog.Sum(x => x.Amount);
        }
    }
}
