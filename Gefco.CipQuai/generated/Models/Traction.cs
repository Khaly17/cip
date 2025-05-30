// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Gefco.CipQuai.ApiClient.Models
{
    /// <summary> The Traction. </summary>
    public partial class Traction
    {
        /// <summary> Initializes a new instance of Traction. </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="creationDate"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="name"/> or <paramref name="id"/> is null. </exception>
        public Traction(string name, string id, DateTimeOffset creationDate)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Name = name;
            Id = id;
            CreationDate = creationDate;
        }

        /// <summary> Initializes a new instance of Traction. </summary>
        /// <param name="agenceDepart"></param>
        /// <param name="agenceArrivee"></param>
        /// <param name="numeroBorderau"></param>
        /// <param name="idVoyage"></param>
        /// <param name="dueDate"></param>
        /// <param name="isCreated"></param>
        /// <param name="isCancelled"></param>
        /// <param name="cancelReason"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="creationDate"></param>
        internal Traction(Agence agenceDepart, Agence agenceArrivee, string numeroBorderau, string idVoyage, DateTimeOffset? dueDate, bool? isCreated, bool? isCancelled, string cancelReason, string name, string id, DateTimeOffset creationDate)
        {
            AgenceDepart = agenceDepart;
            AgenceArrivee = agenceArrivee;
            NumeroBorderau = numeroBorderau;
            IdVoyage = idVoyage;
            DueDate = dueDate;
            IsCreated = isCreated;
            IsCancelled = isCancelled;
            CancelReason = cancelReason;
            Name = name;
            Id = id;
            CreationDate = creationDate;
        }

        /// <summary> Gets or sets the agence depart. </summary>
        public Agence AgenceDepart { get; set; }
        /// <summary> Gets or sets the agence arrivee. </summary>
        public Agence AgenceArrivee { get; set; }
        /// <summary> Gets or sets the numero borderau. </summary>
        public string NumeroBorderau { get; set; }
        /// <summary> Gets or sets the id voyage. </summary>
        public string IdVoyage { get; set; }
        /// <summary> Gets or sets the due date. </summary>
        public DateTimeOffset? DueDate { get; set; }
        /// <summary> Gets or sets the is created. </summary>
        public bool? IsCreated { get; set; }
        /// <summary> Gets or sets the is cancelled. </summary>
        public bool? IsCancelled { get; set; }
        /// <summary> Gets or sets the cancel reason. </summary>
        public string CancelReason { get; set; }
        /// <summary> Gets or sets the name. </summary>
        public string Name { get; set; }
        /// <summary> Gets or sets the id. </summary>
        public string Id { get; set; }
        /// <summary> Gets or sets the creation date. </summary>
        public DateTimeOffset CreationDate { get; set; }
    }
}
