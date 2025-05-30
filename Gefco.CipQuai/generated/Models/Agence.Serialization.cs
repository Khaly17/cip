// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using Azure.Core;

namespace Gefco.CipQuai.ApiClient.Models
{
    public partial class Agence : IUtf8JsonSerializable, IXmlSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(IsUnderWatch))
            {
                writer.WritePropertyName("IsUnderWatch");
                writer.WriteBooleanValue(IsUnderWatch.Value);
            }
            if (Optional.IsDefined(AgenceType))
            {
                writer.WritePropertyName("AgenceType");
                writer.WriteObjectValue(AgenceType);
            }
            if (Optional.IsDefined(OtherName))
            {
                writer.WritePropertyName("OtherName");
                writer.WriteStringValue(OtherName);
            }
            if (Optional.IsCollectionDefined(DeclarationBonnePratiques))
            {
                writer.WritePropertyName("DeclarationBonnePratiques");
                writer.WriteStartArray();
                foreach (var item in DeclarationBonnePratiques)
                {
                    writer.WriteObjectValue(item);
                }
                writer.WriteEndArray();
            }
            writer.WritePropertyName("Name");
            writer.WriteStringValue(Name);
            writer.WritePropertyName("Id");
            writer.WriteStringValue(Id);
            writer.WritePropertyName("CreationDate");
            writer.WriteStringValue(CreationDate, "O");
            writer.WriteEndObject();
        }

        internal static Agence DeserializeAgence(JsonElement element)
        {
            Optional<bool> isUnderWatch = default;
            Optional<AgenceType> agenceType = default;
            Optional<string> otherName = default;
            Optional<IList<DeclarationBonnePratique>> declarationBonnePratiques = default;
            string name = default;
            string id = default;
            DateTimeOffset creationDate = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("IsUnderWatch"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    isUnderWatch = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("AgenceType"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    agenceType = AgenceType.DeserializeAgenceType(property.Value);
                    continue;
                }
                if (property.NameEquals("OtherName"))
                {
                    otherName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("DeclarationBonnePratiques"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    List<DeclarationBonnePratique> array = new List<DeclarationBonnePratique>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(DeclarationBonnePratique.DeserializeDeclarationBonnePratique(item));
                    }
                    declarationBonnePratiques = array;
                    continue;
                }
                if (property.NameEquals("Name"))
                {
                    name = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("Id"))
                {
                    id = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("CreationDate"))
                {
                    creationDate = property.Value.GetDateTimeOffset("O");
                    continue;
                }
            }
            return new Agence(Optional.ToNullable(isUnderWatch), agenceType.Value, otherName.Value, Optional.ToList(declarationBonnePratiques), name, id, creationDate);
        }

        void IXmlSerializable.Write(XmlWriter writer, string nameHint)
        {
            writer.WriteStartElement(nameHint ?? "Agence");
            if (Optional.IsDefined(IsUnderWatch))
            {
                writer.WriteStartElement("IsUnderWatch");
                writer.WriteValue(IsUnderWatch.Value);
                writer.WriteEndElement();
            }
            if (Optional.IsDefined(AgenceType))
            {
                writer.WriteObjectValue(AgenceType, "AgenceType");
            }
            if (Optional.IsDefined(OtherName))
            {
                writer.WriteStartElement("OtherName");
                writer.WriteValue(OtherName);
                writer.WriteEndElement();
            }
            writer.WriteStartElement("Name");
            writer.WriteValue(Name);
            writer.WriteEndElement();
            writer.WriteStartElement("Id");
            writer.WriteValue(Id);
            writer.WriteEndElement();
            writer.WriteStartElement("CreationDate");
            writer.WriteValue(CreationDate, "O");
            writer.WriteEndElement();
            if (Optional.IsCollectionDefined(DeclarationBonnePratiques))
            {
                foreach (var item in DeclarationBonnePratiques)
                {
                    writer.WriteObjectValue(item, "DeclarationBonnePratique");
                }
            }
            writer.WriteEndElement();
        }

        internal static Agence DeserializeAgence(XElement element)
        {
            bool? isUnderWatch = default;
            AgenceType agenceType = default;
            string otherName = default;
            string name = default;
            string id = default;
            DateTimeOffset creationDate = default;
            IList<DeclarationBonnePratique> declarationBonnePratiques = default;
            if (element.Element("IsUnderWatch") is XElement isUnderWatchElement)
            {
                isUnderWatch = (bool?)isUnderWatchElement;
            }
            if (element.Element("AgenceType") is XElement agenceTypeElement)
            {
                agenceType = AgenceType.DeserializeAgenceType(agenceTypeElement);
            }
            if (element.Element("OtherName") is XElement otherNameElement)
            {
                otherName = (string)otherNameElement;
            }
            if (element.Element("Name") is XElement nameElement)
            {
                name = (string)nameElement;
            }
            if (element.Element("Id") is XElement idElement)
            {
                id = (string)idElement;
            }
            if (element.Element("CreationDate") is XElement creationDateElement)
            {
                creationDate = creationDateElement.GetDateTimeOffsetValue("O");
            }
            var array = new List<DeclarationBonnePratique>();
            foreach (var e in element.Elements("DeclarationBonnePratique"))
            {
                array.Add(DeclarationBonnePratique.DeserializeDeclarationBonnePratique(e));
            }
            declarationBonnePratiques = array;
            return new Agence(isUnderWatch, agenceType, otherName, declarationBonnePratiques, name, id, creationDate);
        }
    }
}
