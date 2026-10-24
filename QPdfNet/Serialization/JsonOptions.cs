//
// JsonOptions.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2021-2026 Kees van Spelde.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace QPdfNet.Serialization;

/// <summary>
///     Centralizes the <see cref="JsonSerializerOptions" /> that are used throughout QPdfNet. These options
///     reproduce the behaviour that was previously provided by Newtonsoft.Json: private fields that are annotated
///     with <see cref="JsonPropertyNameAttribute" /> are serialized, <c>null</c> values are omitted, enum values
///     are written using their <see cref="EnumMemberAttribute" /> and enum members that have not been set
///     (<c>NotSet</c>, the default value) are skipped.
/// </summary>
internal static class JsonOptions
{
    #region Fields
    private static readonly EnumMemberConverter EnumConverter = new();
    #endregion

    #region Serialize
    /// <summary>
    ///     The options used to serialize a <c>Job</c> into the JSON that is passed to qpdf
    /// </summary>
    public static readonly JsonSerializerOptions Serialize = CreateSerializeOptions();
    #endregion

    #region Deserialize
    /// <summary>
    ///     The options used to deserialize qpdf JSON output
    /// </summary>
    public static readonly JsonSerializerOptions Deserialize = CreateDeserializeOptions();
    #endregion

    #region JsonNodeWrite
    /// <summary>
    ///     The options used to write a <see cref="System.Text.Json.Nodes.JsonNode" /> back to disk
    /// </summary>
    public static readonly JsonSerializerOptions JsonNodeWrite = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
    #endregion

    #region CreateSerializeOptions
    private static JsonSerializerOptions CreateSerializeOptions()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { IncludePrivateFields, IgnoreUnsetEnums }
            }
        };

        options.Converters.Add(EnumConverter);

        return options;
    }
    #endregion

    #region CreateDeserializeOptions
    private static JsonSerializerOptions CreateDeserializeOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { IncludePrivateFields }
            }
        };

        options.Converters.Add(EnumConverter);

        return options;
    }
    #endregion

    #region IncludePrivateFields
    /// <summary>
    ///     Adds private fields that are annotated with <see cref="JsonPropertyNameAttribute" /> to the serialization
    ///     contract. System.Text.Json does not serialize private fields out of the box, while Newtonsoft.Json did.
    /// </summary>
    private static void IncludePrivateFields(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        var fields = typeInfo.Type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields)
        {
            var attribute = field.GetCustomAttribute<JsonPropertyNameAttribute>();

            if (attribute == null)
                continue;

            var alreadyMapped = false;

            foreach (var existing in typeInfo.Properties)
            {
                if (existing.Name == attribute.Name)
                {
                    alreadyMapped = true;
                    break;
                }
            }

            if (alreadyMapped)
                continue;

            var jsonProperty = typeInfo.CreateJsonPropertyInfo(field.FieldType, attribute.Name);
            var localField = field;

            jsonProperty.Get = localField.GetValue;
            jsonProperty.Set = localField.SetValue;

            typeInfo.Properties.Add(jsonProperty);
        }
    }
    #endregion

    #region IgnoreUnsetEnums
    /// <summary>
    ///     Skips enum properties whose value equals the default (<c>NotSet</c>) value so that unset options are not
    ///     written into the qpdf JSON.
    /// </summary>
    private static void IgnoreUnsetEnums(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        foreach (var property in typeInfo.Properties)
        {
            var propertyType = property.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (!underlyingType.IsEnum)
                continue;

            var defaultValue = Enum.ToObject(underlyingType, 0);

            property.ShouldSerialize = (_, value) => value != null && !value.Equals(defaultValue);
        }
    }
    #endregion
}