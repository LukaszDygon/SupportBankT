using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Xml.Linq;

namespace Support_Bank
{
    class TransactionXmlFileReader : ITransactionFileReader
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public List<Transaction> Read(string fileName)
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
            var transaction = new Transaction(
                    DateTime.FromOADate(Double.Parse(transactionXML.Attribute("Date").Value)),
                    transactionXML.Element("Parties").Element("From").Value,
                    transactionXML.Element("Parties").Element("To").Value,
                    transactionXML.Element("Description").Value,
                    Double.Parse(transactionXML.Element("Value").Value));

            return transaction;
        }
    }
}
