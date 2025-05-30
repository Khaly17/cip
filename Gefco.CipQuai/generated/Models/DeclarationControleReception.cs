// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace Gefco.CipQuai.ApiClient.Models
{
    /// <summary> The DeclarationControleReception. </summary>
    public partial class DeclarationControleReception
    {
        /// <summary> Initializes a new instance of DeclarationControleReception. </summary>
        /// <param name="id"></param>
        /// <param name="creationDate"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        public DeclarationControleReception(string id, DateTimeOffset creationDate)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Pictures = new ChangeTrackingList<Picture>();
            Id = id;
            CreationDate = creationDate;
        }

        /// <summary> Gets or sets the is cr. </summary>
        public bool? IsCr { get; set; }
        /// <summary> Gets or sets the traction. </summary>
        public Traction Traction { get; set; }
        /// <summary> Gets or sets the traction id. </summary>
        public string TractionId { get; set; }
        /// <summary> Gets or sets the remorque. </summary>
        public Remorque Remorque { get; set; }
        /// <summary> Gets or sets the current status. </summary>
        public RemorqueStatus CurrentStatus { get; set; }
        /// <summary> Gets or sets the current status id. </summary>
        public string CurrentStatusId { get; set; }
        /// <summary> Gets or sets the current workflow step. </summary>
        public int? CurrentWorkflowStep { get; set; }
        /// <summary> Gets or sets the autre agence arrivee. </summary>
        public string AutreAgenceArrivee { get; set; }
        /// <summary> Gets the pictures. </summary>
        public IList<Picture> Pictures { get; }
        /// <summary> Gets the id. </summary>
        public string Id { get; }
        /// <summary> Gets the creation date. </summary>
        public DateTimeOffset CreationDate { get; }
    }
}
