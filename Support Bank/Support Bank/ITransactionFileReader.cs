using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support_Bank
{
    interface ITransactionFileReader
    {
        List<Transaction> Read(string fileName);
    }
}
