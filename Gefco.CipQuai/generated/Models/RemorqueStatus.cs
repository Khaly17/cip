// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Gefco.CipQuai.ApiClient.Models
{
    /// <summary> The RemorqueStatus. </summary>
    public partial class RemorqueStatus
    {
        /// <summary> Initializes a new instance of RemorqueStatus. </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="creationDate"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="name"/> or <paramref name="id"/> is null. </exception>
        public RemorqueStatus(string name, string id, DateTimeOffset creationDate)
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

        /// <summary> Initializes a new instance of RemorqueStatus. </summary>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="creationDate"></param>
        internal RemorqueStatus(string description, string name, string id, DateTimeOffset creationDate)
        {
            Description = description;
            Name = name;
            Id = id;
            CreationDate = creationDate;
        }

        /// <summary> Gets or sets the description. </summary>
        public string Description { get; set; }
        /// <summary> Gets or sets the name. </summary>
        public string Name { get; set; }
        /// <summary> Gets or sets the id. </summary>
        public string Id { get; set; }
        /// <summary> Gets or sets the creation date. </summary>
        public DateTimeOffset CreationDate { get; set; }
    }
}
