﻿// -----------------------------------------------------------------------
// <copyright file="GetPartnerInvoiceSummary.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.PowerShell.Commands
{
    using System.Linq;
    using System.Management.Automation;
    using PartnerCenter.Models;
    using PartnerCenter.Models.Invoices;
    using PartnerCenter.PowerShell.Models.Invoices;

    [Cmdlet(VerbsCommon.Get, "PartnerInvoiceSummary"), OutputType(typeof(PSInvoiceSummary))]
    public class GetPartnerInvoiceSummary : PartnerPSCmdlet
    {
        /// <summary>
        /// Executes the operations associated with the cmdlet.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            ResourceCollection<InvoiceSummary> summaries;

            try
            {
                summaries = Partner.Invoices.Summaries.Get();

                WriteObject(summaries.Items.Select(s => new PSInvoiceSummary(s)), true);
            }
            finally
            {
                summaries = null;
            }
        }
    }
}