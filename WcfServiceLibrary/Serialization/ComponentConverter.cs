using GameEngineLibrary;
using GameLibrary.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK;
using System;
using System.Drawing;

namespace WcfServiceLibrary.Serialization
{
    /// <summary>
    /// Класс сериализации IComponent
    /// </summary>
    public class ComponentConverter : JsonConverter<IComponent>
    {
        public override bool CanWrite => false;

        public override IComponent ReadJson(JsonReader reader, Type objectType, IComponent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var path = reader.Path.Split('.');
            var componentName = path[path.Length - 1];

            switch (componentName)
            {
                case "transform":
                    return GetTransform(reader, serializer);
                case "texture":
                    return GetTexture(reader, serializer);
                case "health":
                    return GetHealth(JObject.Load(reader));
                case "inventory":
                    return GetInventory(reader, serializer);
                default:
                    ReadToEndObject(reader);
                    return null;
            }
        }

        private void ReadToEndObject(JsonReader reader)
        {
            while (reader.TokenType != JsonToken.EndObject)
            {
                reader.Read();
            }
        }

        private Transform GetTransform(JsonReader reader, JsonSerializer serializer)
        {
            var transform = new Transform();

            reader.Read();

            do
            {
                var name = reader.Value.ToString();
                reader.Read();
                switch (name)
                {
                    case "RotationPoint":
                        transform.RotationPoint = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "LocalPosition":
                        transform.LocalPosition = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "Scale":
                        transform.Scale = serializer.Deserialize<Vector2>(reader);
                        break;
                    case "Rotation":
                        transform.Rotation = serializer.Deserialize<double>(reader);
                        break;
                    default:
                        throw new JsonException("Unexpected token name: " + name);
                }
                reader.Read();
            }
            while (reader.TokenType != JsonToken.EndObject);

            return transform;
        }

        private Texture2D GetTexture(JsonReader reader, JsonSerializer serializer)
        {
            int id = 0;
            int width = 0;
            int height = 0;
            int animationTime = -1;
            int index = 0;
            string textureName = "";
            Color color = default;

            reader.Read();

            do
            {
                var name = reader.Value.ToString();
                reader.Read();
                switch (name)
                {
                    case "ID":
                        id = serializer.Deserialize<int>(reader);
                        break;
                    case "Width":
                        width = serializer.Deserialize<int>(reader);
                        break;
                    case "Height":
                        height = serializer.Deserialize<int>(reader);
                        break;
                    case "Color":
                        color = serializer.Deserialize<Color>(reader);
                        break;
                    case "Name":
                        textureName = serializer.Deserialize<string>(reader);
                        break;
                    case "Index":
                        index = serializer.Deserialize<int>(reader);
                        break;
                    case "AnimationTime":
                        animationTime = serializer.Deserialize<int>(reader);
                        break;
                    default:
                        throw new JsonException("Unexpected token name: " + name);
                }
                reader.Read();
            }
            while (reader.TokenType != JsonToken.EndObject);

            if (animationTime >= 0)
            {
                return new Animation2D(new[] { id }, width, height)
                {
                    AnimationTime = animationTime,
                    Color = color,
                    Name = textureName,
                    Index = index
                };
            }
            else
            {
                return new Texture2D(id, width, height)
                { 
                    Color = color,
                    Name = textureName
                };
            }
        }

        private Health GetHealth(JObject json)
        {
            return new Health(json["Value"].Value<int>());
        }

        private Inventory GetInventory(JsonReader reader, JsonSerializer serializer)
        {
            var inventory = new Inventory();

            reader.Read();

            do
            {
                var name = reader.Value.ToString();
                reader.Read();
                switch (name)
                {
                    case "TotalAmount":
                        inventory.TotalAmount = serializer.Deserialize<int>(reader);
                        break;
                    case "Amounts":
                        inventory.Amounts = serializer.Deserialize<int[]>(reader);
                        break;
                    case "Current":
                        inventory.Current = serializer.Deserialize<int>(reader);
                        break;
                    default:
                        throw new JsonException("Unexpected token name: " + name);
                }
                reader.Read();
            }
            while (reader.TokenType != JsonToken.EndObject);

            return inventory;
        }

        public override void WriteJson(JsonWriter writer, IComponent value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
