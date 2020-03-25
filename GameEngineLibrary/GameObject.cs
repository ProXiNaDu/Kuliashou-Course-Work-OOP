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
        /// Список добавленных скриптов.
        /// </summary>
        private List<Script> scripts;

        /// <summary>
        /// Объект-родитель для данного объекта.
        /// </summary>
        public GameObject Parent { get; protected set; }

        /// <summary>
        /// Список внутренних объектов.
        /// </summary>
        public List<GameObject> InnerObjects { get; private set; }

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
            scripts = new List<Script>();
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
            scripts = new List<Script>();
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
            scripts.Add(script);
            script.SetControlledObject(this);
        }

        /// <summary>
        /// Удаление скрипта объекта.
        /// Скрипт определяет основное поведение объекта.
        /// </summary>
        /// <param name="script">Скрипт для удаления.</param>
        public void RemoveScript(Script script)
        {
            scripts.Remove(script);
            script.SetControlledObject(null);
        }

        /// <summary>
        /// Установить коллайдер объекту.
        /// </summary>
        /// <param name="collider">Новый коллайдер.</param>
        public void SetCollider(Collider collider)
        {
            Collider = collider;
        }

        /// <summary>
        /// Привязать коллайдер к текстуре.
        /// </summary>
        public void UpdateColliderToTexture()
        {
            float x = Position.X;
            float y = Position.Y;
            float width = Texture.Width * Scale.X;
            float height = Texture.Height * Scale.Y;
            float rotationX = RotationPoint.X * Scale.X;
            float rotationY = RotationPoint.Y * Scale.Y;
            double angle = Rotation * Math.Sign(Scale.X);

            Vector2[] vertices = new Vector2[]
            {
                new Vector2(width, 0),
                new Vector2(width, height),
                new Vector2(0, height),
                new Vector2(0, 0),
            };

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].X -= rotationX;
                vertices[i].Y -= rotationY;
                vertices[i] = new Vector2(
                    (float)(Math.Cos(angle) * vertices[i].X -
                            Math.Sin(angle) * vertices[i].Y),
                    (float)(Math.Sin(angle) * vertices[i].X +
                            Math.Cos(angle) * vertices[i].Y));
                vertices[i].X += rotationX;
                vertices[i].Y += rotationY;

                vertices[i].X += x;
                vertices[i].Y += y;
            }

            Collider = new Collider(vertices);
        }

        /// <summary>
        /// Обновить состояние объекта.
        /// </summary>
        /// <param name="delta">Время, прошедшее между кадрами.</param>
        public void Update(TimeSpan delta)
        {
            foreach (Script script in scripts)
            {
                script.Update(delta);
            }
            foreach (GameObject gameObject in InnerObjects)
            {
                gameObject.Update(delta);
            }
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
