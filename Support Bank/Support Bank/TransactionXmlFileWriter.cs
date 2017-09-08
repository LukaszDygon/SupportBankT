using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Support_Bank
{
    class TransactionXmlFileWriter : ITransactionFileWriter
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public void Write(string fileName, List<Transaction> transactions)
        {
            using (var file = System.IO.File.Create(fileName))
            {
                var writer = new XmlSerializer(typeof(List<Transaction>));
                writer.Serialize(file, transactions);
            }
        }
    }
}
