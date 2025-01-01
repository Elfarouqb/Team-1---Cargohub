using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cargohub_V2.DataConverters
{
    public class SingleOrArrayConverter : JsonConverter<List<int>>
    {
        public override List<int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                // If it's an array, deserialize as List<int>
                return JsonSerializer.Deserialize<List<int>>(ref reader, options);
            }
            else
            {
                // If it's a single value, create a List<int> from it
                int value = reader.GetInt32();
                return new List<int> { value };
            }
        }

        public override void Write(Utf8JsonWriter writer, List<int> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}

