using Newtonsoft.Json;
using OpenTK;
using System;

namespace WcfServiceLibrary.Serialization
{
    /// <summary>
    /// Класс сериализации Vector2
    /// </summary>
    public class Vector2Converter : JsonConverter<Vector2>
    {
        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = (string) reader.Value;
            var values = value.Split(';');
            var x = float.Parse(values[0]);
            var y = float.Parse(values[1]);
            return new Vector2(x, y);
        }

        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            writer.WriteValue(value.X + ";" + value.Y);
        }
    }
}
