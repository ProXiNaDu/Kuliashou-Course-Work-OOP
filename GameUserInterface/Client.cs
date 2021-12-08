using GameEngineLibrary;
using GameLibrary.Scripts.RemoteKeyboardControlScripts;
using GameUserInterface.ConnectServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WcfServiceLibrary.Serialization;

namespace GameUserInterface
{
    /// <summary>
    /// Игровой клиент
    /// </summary>
    public class Client
    {
        private const string ConnectServiceEndpoint = "NetTcpBinding_IConnectService";

        private const string ConnectService = "/connect";

        private readonly string address;

        private bool closed;

        private readonly ConnectServiceClient connectServiceClient;

        /// <summary>
        /// Создает новый клиент и подключает его к серверу по указанному адресу
        /// </summary>
        /// <param name="address">Адрес для подключения</param>
        public Client(string address)
        {
            this.address = "net.tcp://" + address;

            connectServiceClient = new ConnectServiceClient(ConnectServiceEndpoint, this.address + ConnectService);
        }

        /// <summary>
        /// Попытка подключиться к игровому серверу
        /// </summary>
        /// <returns>true - если подключиться удалось; в противно млучае - false</returns>
        public bool ConnectToServer()
        {
            if (closed) return false;

            try
            {
                return connectServiceClient.ConnectToServer();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Отклчение клиента от сервера
        /// </summary>
        public void DisconnectFromServer()
        {
            if (closed) return;

            try
            {
                connectServiceClient.DisconnectFromServer();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Получение количества подключенных устройств
        /// </summary>
        /// <returns>Количество подключенных устройств</returns>
        public int GetConnectedUsersCount()
        {
            if (closed) return 0;

            try
            {
                return connectServiceClient.GetConnectedUsersCount();
            }
            catch (Exception)
            {
                return 0;
            }

        }

        /// <summary>
        /// Задание инвентаря первому танку.
        /// </summary>
        /// <param name="amounts">Количество ракет.</param>
        public void SetFirstPanzerAmounts(params int[] amounts)
        {
            if (closed) return;

            try
            {
                connectServiceClient.SetFirstPanzerAmounts(amounts);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Задание инвентаря второму танку.
        /// </summary>
        /// <param name="amounts">Количество ракет.</param>
        public void SetSecondPanzerAmounts(params int[] amounts)
        {
            if (closed) return;

            try
            {
                connectServiceClient.SetSecondPanzerAmounts(amounts);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Освобождает занятые клиентом ресурсы
        /// </summary>
        public void Close()
        {
            if (closed) return;

            try
            {
                connectServiceClient.Close();
            }
            catch (Exception)
            {

            }

            closed = true;
        }

        /// <summary>
        /// Флаг, свидетельствующий о готовности второго игрока
        /// </summary>
        /// <returns>true - если второй игрок готов; в противном случае - false</returns>
        public bool IsSecondReady()
        {
            if (closed) return false;

            try
            {
                return connectServiceClient.IsSecondReady();
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Установка состояния клавиатуры для первого игрока
        /// </summary>
        /// <param name="keyboard">Состояние клавиатуры</param>
        public void SetFirstPlayerKeyboardState(RemoteKeyboardState keyboard)
        {
            if (closed) return;

            try
            {
                connectServiceClient.SetFirstPlayerKeyboardState(keyboard);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Установка состояния клавиатуры для второго игрока
        /// </summary>
        /// <param name="keyboard">Состояние клавиатуры</param>
        public void SetSecondPlayerKeyboardState(RemoteKeyboardState keyboard)
        {
            if (closed) return;

            try
            {
                connectServiceClient.SetSecondPlayerKeyboardState(keyboard);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Получение текущих игровых объектов на сцене
        /// </summary>
        /// <returns>Текущин игровые объекты на сцене</returns>
        public List<GameObject> GetCurrentGameObjects()
        {
            if (closed) return null;

            try
            {
                List<GameObject> gameObjects = JsonConvert.DeserializeObject<List<GameObject>>(
                    connectServiceClient.GetCurrentGameObjects(),
                    new Vector2Converter(), new ComponentConverter());
                UpdateInnerObjects(gameObjects);
                return gameObjects;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private void UpdateInnerObjects(List<GameObject> gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.InnerObjects.Count == 0) continue;

                UpdateInnerObjects(gameObject.InnerObjects, gameObject);
            }
        }

        private void UpdateInnerObjects(List<GameObject> gameObjects, GameObject parent)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                var transform = (Transform) gameObject.GetComponent("transform");
                transform.Parent = parent;

                if (gameObject.InnerObjects.Count == 0) continue;

                UpdateInnerObjects(gameObject.InnerObjects, gameObject);
            }
        }

        /// <summary>
        /// Освобождает занятые клиентом ресурсы
        /// </summary>
        ~Client()
        {
            Close();
        }
    }
}
