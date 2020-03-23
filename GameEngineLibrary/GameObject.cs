using System;
using OpenTK;
using System.Collections.Generic;

namespace GameEngineLibrary
{
    /// <summary>
    /// Класс, который описывает объект на сцене.
    /// </summary>
    public class GameObject : IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Объект-родитель для данного объекта.
        /// </summary>
        public GameObject Parent { get; protected set; }

        /// <summary>
        /// Список внутренних объектов.
        /// </summary>
        public List<GameObject> InnerObjects { get; private set; }

        /// <summary>
        /// Список добавленных скриптов.
        /// </summary>
        public List<Script> Scripts { get; private set; }

        /// <summary>
        /// Текстура объекта.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Коллайдер объекта.
        /// </summary>
        public Collider Collider { get; private set; }

        /// <summary>
        /// Точка, вокруг которой будет поворачиваться объект.
        /// </summary>
        public Vector2 RotationPoint { get; set; }

        private Vector2 localPosition;
        /// <summary>
        /// Позиция объекта.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return (Parent == null) ? 
                    localPosition :
                    Parent.Position + localPosition;
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

        /// <summary>
        /// Создание нового игрового объекта.
        /// </summary>
        public GameObject()
        {
            Position = Vector2.Zero;
            RotationPoint = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0;
            InnerObjects = new List<GameObject>();
            Scripts = new List<Script>();
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
            InnerObjects = new List<GameObject>();
            Scripts = new List<Script>();
        }

        /// <summary>
        /// Добавление объекта внутрь другого объекта.
        /// Объекты внутри других объектов должны отрисовываться
        /// относительно позиции родительского объекта.
        /// </summary>
        /// <param name="gameObject">Объект для вставки.</param>
        public void AddInnerObject(GameObject gameObject)
        {
            InnerObjects.Add(gameObject);
            gameObject.Parent = this;
        }

        /// <summary>
        /// Удаление объекта из другого объекта.
        /// Объекты внутри других объектов должны отрисовываться
        /// относительно позиции родительского объекта.
        /// </summary>
        /// <param name="gameObject">Объект для удаления.</param>
        public void RemoveInnerObject(GameObject gameObject)
        {
            InnerObjects.Remove(gameObject);
            gameObject.Parent = null;
        }

        /// <summary>
        /// Добавление скрипта для объекта.
        /// Скрипт определяет основное поведение объекта.
        /// </summary>
        /// <param name="script">Скрипт для добавления.</param>
        public void AddScript(Script script)
        {
            Scripts.Add(script);
            script.SetControlledObject(this);
        }

        /// <summary>
        /// Удаление скрипта объекта.
        /// Скрипт определяет основное поведение объекта.
        /// </summary>
        /// <param name="script">Скрипт для удаления.</param>
        public void RemoveScript(Script script)
        {
            Scripts.Remove(script);
            script.SetControlledObject(null);
        }

        public void SetCollider(Collider collider)
        {
            Collider = collider;
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Texture.Dispose();
                }

                disposed = true;
            }
        }

        ~GameObject()
        {
            Dispose(false);
        }
    }
}
