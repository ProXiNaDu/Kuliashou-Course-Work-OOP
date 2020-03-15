﻿using OpenTK;
using System.Collections.Generic;

namespace GameEngineLibrary
{
    /// <summary>
    /// Класс, который описывает объект на сцене.
    /// </summary>
    public class GameObject
    {
        /// <summary>
        /// Текстура объекта.
        /// </summary>
        public Texture2D Texture { get; private set; }
        /// <summary>
        /// Точка, вокруг которой будет поворачиваться объект.
        /// </summary>
        public Vector2 RotationPoint { get; private set; }
        /// <summary>
        /// Позиция объекта.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Масштабирование объект.
        /// </summary>
        public Vector2 Scale { get; set; }
        /// <summary>
        /// Угол поворота объекта.
        /// </summary>
        public double Rotation { get; set; }

        /// <summary>
        /// Создание нового игрового объекта.
        /// </summary>
        public GameObject()
        {
            Position = Vector2.Zero;
            RotationPoint = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0;
        }

        /// <summary>
        /// Создание нового игрового объекта с текстурой.
        /// </summary>
        /// <param name="texture">Текстура объекта.</param>
        public GameObject(Texture2D texture) : this()
        {
            Texture = texture;
        }

        /// <summary>
        /// Создание нового игрового объекта.
        /// </summary>
        /// <param name="texture">Текстура объекта.</param>
        /// <param name="position">Положение объекта на сцене.</param>
        /// <param name="rotationPoint">Точка поворота объекта.</param>
        /// <param name="scale">Масштаб объекта.</param>
        /// <param name="rotation">Угол поворота объекта.</param>
        public GameObject(Texture2D texture, Vector2 position,
            Vector2 rotationPoint, Vector2 scale, double rotation)
        {
            Texture = texture;
            Position = position;
            RotationPoint = rotationPoint;
            Scale = scale;
            Rotation = rotation;
        }

        public override bool Equals(object obj)
        {
            return obj is GameObject go &&
                   EqualityComparer<Texture2D>.Default.Equals(Texture, go.Texture) &&
                   RotationPoint.Equals(go.RotationPoint) &&
                   Position.Equals(go.Position) &&
                   Scale.Equals(go.Scale) &&
                   Rotation == go.Rotation;
        }

        public override int GetHashCode()
        {
            var hashCode = 421840917;
            hashCode = hashCode * -1521134295 + EqualityComparer<Texture2D>.Default.GetHashCode(Texture);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(RotationPoint);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(Position);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(Scale);
            hashCode = hashCode * -1521134295 + Rotation.GetHashCode();
            return hashCode;
        }
    }
}
