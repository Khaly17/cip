// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Gefco.CipQuai.ApiClient.Models
{
    public partial class ResourceListServiceResult
    {
        internal static ResourceListServiceResult DeserializeResourceListServiceResult(JsonElement element)
        {
            Optional<IReadOnlyList<Resource>> values = default;
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
                    List<Resource> array = new List<Resource>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(Resource.DeserializeResource(item));
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
            return new ResourceListServiceResult(Optional.ToList(values), errorCode.Value, Optional.ToNullable(isSuccess), errorMessage.Value);
        }
    }
}
