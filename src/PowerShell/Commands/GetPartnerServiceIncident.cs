﻿// -----------------------------------------------------------------------
// <copyright file="GetPartnerServiceIncident.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.PowerShell.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using PartnerCenter.Models;
    using PartnerCenter.Models.ServiceIncidents;

    /// <summary>
    /// Gets a list of service incidents from Partner Center.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "PartnerServiceIncident"), OutputType(typeof(ServiceIncidentDetail))]
    public class GetPartnerServiceIncident : PartnerPSCmdlet
    {
        /// <summary>
        /// Gets or sets the optional status type.
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "Specifies which status types to return.")]
        [ValidateSet(nameof(ServiceIncidentStatus.Critical), nameof(ServiceIncidentStatus.Information), nameof(ServiceIncidentStatus.Normal), nameof(ServiceIncidentStatus.Warning))]
        public ServiceIncidentStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the optional Resolved switch.
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "If specified resolved incidents are also returned.")]
        public SwitchParameter Resolved { get; set; }

        /// <summary>
        /// Executes the operations associated with the cmdlet.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            ResourceCollection<ServiceIncidents> incidents;
            IEnumerable<ServiceIncidentDetail> results;

            try
            {
                incidents = Partner.ServiceIncidents.Get();

                if (incidents.TotalCount > 0)
                {
                    results = incidents.Items.SelectMany(i => i.Incidents);

                    if (Status.HasValue)
                    {
                        results = results.Where(i => i.Status == Status);
                    }

                    if (!Resolved)
                    {
                        results = results.Where(i => i.Resolved == false);
                    }

                    WriteObject(results, true);
                }
            }
            finally
            {
                incidents = null;
                results = null;
            }
        }
    }
}