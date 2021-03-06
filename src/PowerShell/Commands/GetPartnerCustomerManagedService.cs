﻿// -----------------------------------------------------------------------
// <copyright file="GetPartnerCustomerManagedService.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.PowerShell.Commands
{
    using System.Linq;
    using System.Management.Automation;
    using System.Text.RegularExpressions;
    using Common;
    using PartnerCenter.Models;
    using PartnerCenter.Models.ManagedServices;
    using PartnerCenter.PowerShell.Models.ManagedServices;

    /// <summary>
    /// Gets the customer's managed services from Partner Center.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "PartnerCustomerManagedService"), OutputType(typeof(PSManagedService))]
    public class GetPartnerCustomerManagedService : PartnerPSCmdlet
    {
        /// <summary>
        /// Gets or sets the required customer identifier.
        /// </summary>
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "The identifier for the customer.")]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled)]
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the managed service identifier.
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "A string that identifies the managed service.")]
        [ValidateNotNullOrEmpty]
        public string ManagedServiceId { get; set; }

        /// <summary>
        /// Executes the operations associated with the cmdlet.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            if (!string.IsNullOrEmpty(ManagedServiceId))
            {
                GetManagedService(CustomerId, ManagedServiceId);
            }
            else
            {
                GetManagedServices(CustomerId);
            }
        }

        /// <summary>
        /// Gets the managed services for the customer.
        /// </summary>
        /// <param name="customerId">Identifier for the customer.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="customerId"/> is empty or null.
        /// </exception>
        private void GetManagedServices(string customerId)
        {
            ResourceCollection<ManagedService> managedServices;

            customerId.AssertNotEmpty(nameof(customerId));

            try
            {
                managedServices = Partner.Customers.ById(CustomerId).ManagedServices.Get();

                if (managedServices.TotalCount > 0)
                {
                    WriteObject(managedServices.Items.Select(s => new PSManagedService(s)), true);
                }
            }
            finally
            {
                managedServices = null;
            }
        }

        /// <summary>
        /// Gets a specific managed service for a customer.
        /// </summary>
        /// <param name="customerId">The idnentifier for the customer.</param>
        /// <param name="managedServiceId">The identifier of the managed service.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="customerId" /> is empty or null.
        /// or
        /// <paramref name="managedServiceId" /> is empty or null.
        /// </exception>
        private void GetManagedService(string customerId, string managedServiceId)
        {
            ResourceCollection<ManagedService> managedServices;

            customerId.AssertNotEmpty(nameof(customerId));
            managedServiceId.AssertNotEmpty(nameof(managedServiceId));

            try
            {
                managedServices = Partner.Customers.ById(CustomerId).ManagedServices.Get();

                if (managedServices.TotalCount > 0)
                {
                    WriteObject(managedServices.Items.Where(s => s.Id == managedServiceId).Select(i => new PSManagedService(i)), true);
                }
            }
            finally
            {
                managedServices = null;
            }
        }
    }
}