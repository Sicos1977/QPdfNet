//
// EnumMemberConverter.cs
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
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QPdfNet.Serialization;

/// <summary>
///     A <see cref="JsonConverterFactory" /> that serializes and deserializes enum values by using the string
///     specified in their <see cref="EnumMemberAttribute" />. This restores the behaviour that Newtonsoft.Json
///     provided through its <c>StringEnumConverter</c> combined with <see cref="EnumMemberAttribute" />, so that
///     qpdf JSON values such as <c>preserve</c>, <c>auto</c> or <c>hex-bytes</c> are written and read correctly.
///     Enum members without an <see cref="EnumMemberAttribute" /> (for example <c>NotSet</c>) fall back to their
///     literal name.
/// </summary>
internal sealed class EnumMemberConverter : JsonConverterFactory
{
    #region CanConvert
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }
    #endregion

    #region CreateConverter
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(EnumMemberConverterInner<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
    #endregion

    #region EnumMemberConverterInner
    private sealed class EnumMemberConverterInner<T> : JsonConverter<T> where T : struct, Enum
    {
        private readonly Dictionary<T, string> _enumToString = new();
        private readonly Dictionary<string, T> _stringToEnum = new(StringComparer.OrdinalIgnoreCase);

        public EnumMemberConverterInner()
        {
            foreach (var value in (T[])Enum.GetValues(typeof(T)))
            {
                var name = value.ToString();
                var field = typeof(T).GetField(name);
                var enumMember = field?.GetCustomAttribute<EnumMemberAttribute>();
                var serializedName = enumMember?.Value ?? name;

                _enumToString[value] = serializedName;
                _stringToEnum[serializedName] = value;
            }
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return default;

            var value = reader.GetString();

            if (value != null && _stringToEnum.TryGetValue(value, out var result))
                return result;

            if (value != null && Enum.TryParse(value, true, out T parsed))
                return parsed;

            throw new JsonException($"Cannot convert '{value}' to enum '{typeof(T).Name}'");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (_enumToString.TryGetValue(value, out var serializedName))
                writer.WriteStringValue(serializedName);
            else
                writer.WriteStringValue(value.ToString());
        }
    }
    #endregion
}