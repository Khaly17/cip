// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Gefco.CipQuai.ApiClient.Models
{
    public partial class RemorqueStatusListServiceResult
    {
        internal static RemorqueStatusListServiceResult DeserializeRemorqueStatusListServiceResult(JsonElement element)
        {
            Optional<IReadOnlyList<RemorqueStatus>> values = default;
            Optional<string> errorCode = default;
            Optional<bool> isSuccess = default;
            Optional<string> errorMessage = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("Values"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    List<RemorqueStatus> array = new List<RemorqueStatus>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(RemorqueStatus.DeserializeRemorqueStatus(item));
                    }
                    values = array;
                    continue;
                }
                if (property.NameEquals("ErrorCode"))
                {
                    errorCode = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("IsSuccess"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    isSuccess = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("ErrorMessage"))
                {
                    errorMessage = property.Value.GetString();
                    continue;
                }
            }
            return new RemorqueStatusListServiceResult(Optional.ToList(values), errorCode.Value, Optional.ToNullable(isSuccess), errorMessage.Value);
        }
    }
}
