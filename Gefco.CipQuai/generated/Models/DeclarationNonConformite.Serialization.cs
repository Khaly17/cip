// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Gefco.CipQuai.ApiClient.Models
{
    public partial class DeclarationNonConformite : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(AgenceConcernE))
            {
                writer.WritePropertyName("AgenceConcernée");
                writer.WriteObjectValue(AgenceConcernE);
            }
            if (Optional.IsDefined(CurrentWorkflowStep))
            {
                writer.WritePropertyName("CurrentWorkflowStep");
                writer.WriteNumberValue(CurrentWorkflowStep.Value);
            }
            if (Optional.IsCollectionDefined(MotifNCs))
            {
                writer.WritePropertyName("MotifNCs");
                writer.WriteStartArray();
                foreach (var item in MotifNCs)
                {
                    writer.WriteObjectValue(item);
                }
                writer.WriteEndArray();
            }
            if (Optional.IsDefined(AutreMotifNC))
            {
                writer.WritePropertyName("AutreMotifNC");
                writer.WriteStringValue(AutreMotifNC);
            }
            if (Optional.IsDefined(NumVoyage))
            {
                writer.WritePropertyName("NumVoyage");
                writer.WriteStringValue(NumVoyage);
            }
            if (Optional.IsCollectionDefined(Pictures))
            {
                writer.WritePropertyName("Pictures");
                writer.WriteStartArray();
                foreach (var item in Pictures)
                {
                    writer.WriteObjectValue(item);
                }
                writer.WriteEndArray();
            }
            if (Optional.IsDefined(CurrentStatus))
            {
                writer.WritePropertyName("CurrentStatus");
                writer.WriteObjectValue(CurrentStatus);
            }
            if (Optional.IsDefined(CurrentStatusId))
            {
                writer.WritePropertyName("CurrentStatus_Id");
                writer.WriteStringValue(CurrentStatusId);
            }
            writer.WritePropertyName("Id");
            writer.WriteStringValue(Id);
            writer.WritePropertyName("CreationDate");
            writer.WriteStringValue(CreationDate, "O");
            writer.WriteEndObject();
        }
    }
}
