using System.ServiceModel;

namespace WcfServiceLibrary
{
    /// <summary>
    /// Интерфейс сервиса для подключения к игровому серверу
    /// </summary>
    [ServiceContract]
    public interface IConnectService
    {
        /// <summary>
        /// Попытка подключиться к игровому серверу
        /// </summary>
        /// <returns>true - если подключиться удалось; в противном случае - false</returns>
        [OperationContract]
        bool ConnectToServer();

        /// <summary>
        /// Отклчение клиента от сервера
        /// </summary>
        [OperationContract]
        void DisconnectFromServer();

        /// <summary>
        /// Получение количества подключенных устройств
        /// </summary>
        /// <returns>Количество подключенных устройств</returns>
        [OperationContract]
        int GetConnectedUsersCount();

        /// <summary>
        /// Задание инвентаря первому танку.
        /// </summary>
        /// <param name="amounts">Количество ракет.</param>
        [OperationContract]
        void SetFirstPanzerAmounts(params int[] amounts);

        /// <summary>
        /// Задание инвентаря второму танку.
        /// </summary>
        /// <param name="amounts">Количество ракет.</param>
        [OperationContract]
        void SetSecondPanzerAmounts(params int[] amounts);

        /// <summary>
        /// Флаг, свидетельствующий о готовности второго игрока
        /// </summary>
        /// <returns>true - если второй игрок готов; в противном случае - false</returns>
        [OperationContract]
        bool IsSecondReady();
    }
}
