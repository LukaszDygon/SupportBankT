using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support_Bank
{
    class TransactionLog
    {
        List<Transaction> transactions;
        public TransactionLog()
        {
            this.transactions = new List<Transaction>();
        }
    }
}
