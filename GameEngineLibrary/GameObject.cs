using OpenTK;

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
    }
}
