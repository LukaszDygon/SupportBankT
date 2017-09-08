using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support_Bank
{
    interface ITransactionFileWriter
    {
        void Write(string fileName, List<Transaction> transactions);
    }
}
