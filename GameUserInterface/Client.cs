using GameUserInterface.ConnectServiceReference;
using System;

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
            catch (Exception)
            {
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
        /// Освобождает занятые клиентом ресурсы
        /// </summary>
        ~Client()
        {
            Close();
        }
    }
}
