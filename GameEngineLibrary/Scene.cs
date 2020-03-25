using System;
using System.Collections.Generic;

namespace GameEngineLibrary
{
    /// <summary>
    /// Интерфейс, описывающий пользовательскую сцену.
    /// </summary>
    public abstract class Scene : IDisposable
    {
        /// <summary>
        /// Ширина окна, в котором отрисовывается сцена.
        /// </summary>
        protected readonly double windowWidth;

        /// <summary>
        /// Высота окна, в котором отрисовывается сцена. 
        /// </summary>
        protected readonly double windowHeight;

        /// <summary>
        /// Список созданных текстур.
        /// </summary>
        private List<Texture2D> textures;

        /// <summary>
        /// Список объектов на сцене.
        /// </summary>
        private List<GameObject> objects;

        /// <summary>
        /// Временный массив с объектами, который нужен
        /// для возможности добавления объектов на сцену
        /// во время выполнения скриптов.
        /// </summary>
        private List<GameObject> objectsToAdd;

        /// <summary>
        /// Временный массив с объектами, который нужен
        /// для возможности удаления объектов со сцены
        /// во время выполнения скриптов.
        /// </summary>
        private List<GameObject> objectsToRemove;

        /// <summary>
        /// Создание сцены.
        /// </summary>
        /// <param name="windowWidth">Ширина окна, в котором будет отображаться сцена.</param>
        /// <param name="windowHeight">Высота окна, в котором будет отображаться сцена.</param>
        public Scene(double windowWidth, double windowHeight)
        {
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            textures = new List<Texture2D>();
            objects = new List<GameObject>();
            objectsToRemove = new List<GameObject>();
            objectsToAdd = new List<GameObject>();
        }

        /// <summary>
        /// Метод, который вызывается 1 раз для инициализации сцены.
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Метод, который вызывается перед отрисовкой кадра.
        /// В этом методе параметры сцены должны обновляться.
        /// </summary>
        /// <param name="delta">Время, прошедшее между кадрами.</param>
        public void Update(TimeSpan delta)
        {
            foreach (GameObject gameObject in objects)
            {
                gameObject.Update(delta);
            }
            UpdateObjectsArray();
        }

        /// <summary>
        /// Метод, позволяющий обновлять количество объектов для обработки динамически.
        /// </summary>
        private void UpdateObjectsArray()
        {
            if (objectsToRemove.Count != 0)
            {
                foreach (GameObject gameObject in objectsToRemove)
                {
                    objects.Remove(gameObject);
                }
                objectsToRemove.Clear();
            }
            if (objectsToAdd.Count != 0)
            {
                objects.AddRange(objectsToAdd);
                objectsToAdd.Clear();
            }
        }

        /// <summary>
        /// Получение списка с объектами на сцене.
        /// </summary>
        /// <returns>Список объектов на сцене.</returns>
        public List<GameObject> GetGameObjects()
        {
            return objects;
        }

        /// <summary>
        /// Метод добавления объекта на сцену.
        /// </summary>
        /// <param name="gameObject">Объект для добавления.</param>
        public void AddGameObject(GameObject gameObject)
        {
            objectsToAdd.Add(gameObject);
        }

        /// <summary>
        /// Метод удаления объекта со сцены.
        /// </summary>
        /// <param name="gameObject">Объект для удаления.</param>
        public void RemoveGameObject(GameObject gameObject)
        {
            objectsToRemove.Add(gameObject);
        }

        /// <summary>
        /// Метод добавления текстур на сцену.
        /// </summary>
        /// <param name="texture">Текстура для добавления.</param>
        public void AddTexture(Texture2D texture)
        {
            textures.Add(texture);
        }

        public void Dispose()
        {
            foreach (Texture2D texture in textures)
            {
                texture.Dispose();
            }

            foreach (GameObject gameObject in objects)
            {
                gameObject.Dispose();
            }
        }
    }
}
