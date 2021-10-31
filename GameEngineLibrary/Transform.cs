using Newtonsoft.Json;
using OpenTK;

namespace GameEngineLibrary
{
    /// <summary>
    /// Объект, отвечающий за расположение объекта на экране.
    /// </summary>
    public class Transform : IComponent
    {
        /// <summary>
        /// Объект-родитель для объекта, которому принадлежит данный экземпляр Transform.
        /// </summary>
        [JsonIgnore]
        public GameObject Parent { get; set; }

        /// <summary>
        /// Точка, вокруг которой будет поворачиваться объект.
        /// </summary>
        public Vector2 RotationPoint { get; set; }

        /// <summary>
        /// Локальные координаты объекта относительно родительского объекта.
        /// </summary>
        private Vector2 localPosition;

        /// <summary>
        /// Локальные координаты объекта относительно родительского объекта.
        /// </summary>
        public Vector2 LocalPosition 
        { 
            get => localPosition;
            set 
            {
                localPosition = value;
            }
        }

        /// <summary>
        /// Позиция объекта.
        /// </summary>
        [JsonIgnore]
        public Vector2 Position
        {
            get
            {
                if (Parent == null)
                {
                    return localPosition;
                }

                Transform transform = Parent.GetComponent("transform") as Transform;
                if (transform != null)
                {
                    return transform.Position + localPosition;
                }

                return localPosition;
            }
            set
            {
                localPosition = value;
            }
        }

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
