public class SingleOrArrayConverter : JsonConverter<List<int>>
{
    public override List<int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Handle empty or invalid JSON gracefully
        if (reader.TokenType == JsonTokenType.Null)
        {
            return new List<int>(); // Return an empty list if the value is null
        }

        // Handle the case where we get a JSON array
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            // Deserialize the array of integers into a List<int>
            return JsonSerializer.Deserialize<List<int>>(ref reader, options);
        }
        // Handle the case where we get a single number
        else if (reader.TokenType == JsonTokenType.Number)
        {
            int value = reader.GetInt32();
            return new List<int> { value }; // Wrap the single integer in a List<int>
        }
        // Handle the case where we get a string representing an integer
        else if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return new List<int>(); // Return an empty list if it's an empty string
            }
            else
            {
                return new List<int> { int.Parse(value) }; // Convert string to a single integer and wrap in List<int>
            }
        }
        else
        {
            // Handle unexpected JSON token types
            throw new JsonException("Unexpected JSON token type.");
        }
    }

    public override void Write(Utf8JsonWriter writer, List<int> value, JsonSerializerOptions options)
    {
        // Always write as an array
        JsonSerializer.Serialize(writer, value, options);
    }
}



