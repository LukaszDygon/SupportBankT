﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using NLog;


namespace Support_Bank
{
    
    class Program
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Info("Starting the program.");
            string csv2015Filename = @"C:\Users\LUD\Documents\Training\SupportBankT\Support Bank\Support Bank\resources\Transactions2015.csv";
            string csv2014Filename = @"C:\Users\LUD\Documents\Training\SupportBankT\Support Bank\Support Bank\resources\Transactions2014.csv";
            string jsonFilename = @"C:\Users\LUD\Documents\Training\SupportBankT\Support Bank\Support Bank\resources\Transactions2013.json";
            string xmlFilename = @"C:\Users\LUD\Documents\Training\SupportBankT\Support Bank\Support Bank\resources\Transactions2012.xml";

            var transactionList = TransactionFileReader.Read(jsonFilename);
            transactionList.AddRange(TransactionFileReader.Read(csv2014Filename));
            transactionList.AddRange(TransactionFileReader.Read(csv2015Filename));
            transactionList.AddRange(TransactionFileReader.Read(xmlFilename));
            var personalAccounts = AccountGenerator.GenerateAccountsFromTransactionList(transactionList);

            TransactionCommandConsole.RunConsolePrompt(personalAccounts);

            Console.ReadKey();
            logger.Info("Program Terminated");
        }
    }
}
