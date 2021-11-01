using System;
using OpenTK;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GameEngineLibrary
{
    /// <summary>
    /// Класс, который описывает объект на сцене.
    /// </summary>
    [DataContract]
    public class GameObject : IDisposable
    {
        private bool disposed = false;

        private Dictionary<string, IComponent> components;

        private List<GameObject> innerObjects;

        /// <summary>
        /// Список добавленных скриптов.
        /// </summary>
        private List<Script> scripts;

        /// <summary>
        /// Список внутренних объектов.
        /// </summary>
        [DataMember]
        public List<GameObject> InnerObjects 
        { 
            get => innerObjects; 
            set 
            { 
                innerObjects = value;
            } 
        }

        /// <summary>
        /// Список компонентов объекта
        /// </summary>
        [DataMember]
        public Dictionary<string, IComponent> Components 
        { 
            get => components; 
            set 
            { 
                components = value; 
            } 
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
            InnerObjects = new List<GameObject>();
            scripts = new List<Script>();
            components = new Dictionary<string, IComponent>();

            Transform transform = new Transform();
            transform.Position = position;
            transform.RotationPoint = rotationPoint;
            transform.Scale = scale;
            transform.Rotation = rotation;

            components["transform"] = transform;

            if (texture != null)
            {
                components["texture"] = texture;
            }
        }

        /// <summary>
        /// Создание нового игрового объекта.
        /// </summary>
        public GameObject() : this(null, Vector2.Zero, Vector2.Zero, Vector2.One, 0)
        {
        }

        /// <summary>
        /// Создание нового игрового объекта с текстурой.
        /// </summary>
        /// <param name="texture">Текстура объекта.</param>
        public GameObject(Texture2D texture) : this()
        {
            components["texture"] = texture;
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
            Transform transform = gameObject.GetComponent("transform") as Transform;
            if (transform != null)
            {
                transform.Parent = this;
            }
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
            Transform transform = gameObject.GetComponent("transform") as Transform;
            if (transform != null)
            {
                transform.Parent = null;
            }
        }

        /// <summary>
        /// Добавление скрипта для объекта.
        /// Скрипт определяет основное поведение объекта.
        /// </summary>
        /// <param name="script">Скрипт для добавления.</param>
        public void AddScript(Script script)
        {
            var temp = new List<Script>(scripts.Count + 1);
            temp.AddRange(scripts);
            temp.Add(script);
            scripts = temp;
            script.SetControlledObject(this);
            script.Init();
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
            AddComponent("collider", collider);
        }

        /// <summary>
        /// Привязать коллайдер к текстуре.
        /// </summary>
        public void UpdateColliderToTexture()
        {
            Transform transform = GetComponent("transform") as Transform;
            Texture2D texture = GetComponent("texture") as Texture2D;

            float x = transform.Position.X;
            float y = transform.Position.Y;
            float width = texture.Width * transform.Scale.X;
            float height = texture.Height * transform.Scale.Y;
            float rotationX = transform.RotationPoint.X * transform.Scale.X;
            float rotationY = transform.RotationPoint.Y * transform.Scale.Y;
            double angle = transform.Rotation * Math.Sign(transform.Scale.X);

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

            SetCollider(new Collider(vertices));
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
            Animation2D animation = GetComponent("texture") as Animation2D;
            if (animation != null)
            {
                animation.Update(delta);
            }
        }

        /// <summary>
        /// Получение компонентов объекта по названию.
        /// </summary>
        /// <param name="key">Название компонента.</param>
        /// <returns></returns>
        public IComponent GetComponent(string key)
        {
            IComponent component;
            components.TryGetValue(key, out component);
            return component;
        }

        /// <summary>
        /// Добавление компонента объекту.
        /// </summary>
        /// <param name="key">Название компонента.</param>
        /// <param name="component">Компонент для добавления.</param>
        public void AddComponent(string key, IComponent component)
        {
            var temp = new Dictionary<string, IComponent>(components);
            temp[key] = component;
            components = temp;
        }

        /// <summary>
        /// Уничтожение игрового объекта.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Уничтожение игрового объекта.
        /// </summary>
        /// <param name="disposing">Если уже уничтожен.</param>
        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (GetComponent("texture") is Texture2D texture)
                    {
                        texture.Dispose();
                    }
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Деструктор игрового объекта.
        /// </summary>
        ~GameObject()
        {
            Dispose(false);
        }
    }
}
