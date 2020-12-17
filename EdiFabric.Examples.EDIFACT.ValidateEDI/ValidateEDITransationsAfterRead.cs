﻿using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.ErrorContexts;
using EdiFabric.Examples.EDIFACT.Common;
using EdiFabric.Framework.Readers;
using EdiFabric.Templates.EdifactD96A;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EdiFabric.Examples.EDIFACT.ValidateEDI
{
    class ValidateEDITransationsAfterRead
    {
        /// <summary>
        /// Validate EDI transactions from file after read
        /// </summary>
        public static void Run()
        {
            Debug.WriteLine("******************************");
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine("******************************");

            Stream ediStream = File.OpenRead(Directory.GetCurrentDirectory() + @"\..\..\..\Files\EDIFACT\MixedTransactions.txt");

            List<IEdiItem> ediItems;
            using (var reader = new EdifactReader(ediStream, "EdiFabric.Examples.EDIFACT.Templates.D96A", new EdifactReaderSettings { SerialNumber = TrialLicense.SerialNumber }))
                ediItems = reader.ReadToEnd().ToList();

            var purchaseOrders = ediItems.OfType<TSORDERSFull>();

            foreach (var purchaseOrder in purchaseOrders)
            {
                //  Validate using EDI codes map
                MessageErrorContext errorContext;
                if (!purchaseOrder.IsValid(out errorContext, new ValidationSettings { SerialNumber = TrialLicense.SerialNumber }))
                {
                    //  Report it back to the sender, log, etc.
                    var errors = errorContext.Flatten();
                }
                else
                {
                    //  purchaseOrder is valid, handle it downstream
                }
            }
        }
    }
}
