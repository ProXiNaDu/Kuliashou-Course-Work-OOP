using System;
using System.Collections.Generic;
using System.Windows;

namespace GameEngineLibrary
{
    /// <summary>
    /// Интерфейс, описывающий пользовательскую сцену.
    /// </summary>
    public abstract class Scene : IDisposable
    {
        /// <summary>
        /// Флаг, говорящий о том, удалена ли сцена
        /// </summary>
        public bool IsDiposed { get; private set; }

        /// <summary>
        /// Окно, в котором отрисовывается сцена.
        /// </summary>
        public Window GameWindow { get; }

        /// <summary>
        /// Список созданных текстур.
        /// </summary>
        private List<Texture2D> textures;

        /// <summary>
        /// Словарь созданных текстур.
        /// </summary>
        private Dictionary<string, Texture2D> namedTextures;

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
        /// Список объектов на сцене.
        /// </summary>
        public List<GameObject> GameObjects { get => objects; set => objects = value; }

        /// <summary>
        /// Создание сцены.
        /// </summary>
        /// <param name="window">Окно, в котором отрисовывается сцена.</param>
        public Scene(Window window)
        {
            GameWindow = window;

            textures = new List<Texture2D>();
            namedTextures = new Dictionary<string, Texture2D>();
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
            var temp = new List<GameObject>(objects);
            if (objectsToRemove.Count != 0)
            {
                foreach (GameObject gameObject in objectsToRemove)
                {
                    temp.Remove(gameObject);
                }
                objectsToRemove.Clear();
            }
            if (objectsToAdd.Count != 0)
            {
                temp.AddRange(objectsToAdd);
                objectsToAdd.Clear();
            }
            objects = temp;
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
            namedTextures[texture.Name] = texture;
        }

        /// <summary>
        /// Получение текстуры по имени
        /// </summary>
        /// <param name="name">Имя текстуры</param>
        /// <returns>Текстура</returns>
        public Texture2D GetTexture(string name)
        {
            return namedTextures[name];
        }
        
        /// <summary>
        /// Уничтожение сцены.
        /// </summary>
        public void Dispose()
        {
            if (IsDiposed) return;

            foreach (Texture2D texture in textures)
            {
                texture.Dispose();
            }

            foreach (GameObject gameObject in objects)
            {
                gameObject.Dispose();
            }

            IsDiposed = true;
        }
    }
}
