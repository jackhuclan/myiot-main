// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vegalot.External.XianjinIot.Drill;
public class StringToDecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 如果是字符串类型，则尝试转换为decimal
        if (reader.TokenType == JsonTokenType.String)
        {
            string value = reader.GetString();
            if (decimal.TryParse(value, out decimal result))
            {
                return result;
            }

        }
        // 如果是数字类型，直接获取
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDecimal();
        }
        return default(decimal);
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
