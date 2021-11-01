using GameLibrary.Scripts.RemoteKeyboardControlScripts;
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

        /// <summary>
        /// Получение текущих игровых объектов на сцене
        /// </summary>
        /// <returns>Текущие игровые объекты на сцене в виде JSON массива</returns>
        [OperationContract]
        string GetCurrentGameObjects();

        /// <summary>
        /// Установка состояния клавиатуры для первого игрока
        /// </summary>
        /// <param name="keyboard">Состояние клавиатуры</param>
        [OperationContract]
        void SetFirstPlayerKeyboardState(RemoteKeyboardState keyboard);

        /// <summary>
        /// Установка состояния клавиатуры для второго игрока
        /// </summary>
        /// <param name="keyboard">Состояние клавиатуры</param>
        [OperationContract]
        void SetSecondPlayerKeyboardState(RemoteKeyboardState keyboard);
    }
}
