using GameEngineLibrary;
using GameLibrary;
using GameLibrary.Scenes;
using GameLibrary.Scripts.RemoteKeyboardControlScripts;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using WcfServiceLibrary.Serialization;

namespace WcfServiceLibrary
{
    /// <summary>
    /// Сервис для подключения к игровому серверу
    /// </summary>
    public class ConnectService : IConnectService
    {
        private static int connectedUsersCount;
        private static bool isSecondReady;
        private static bool isInited;
        private static BattleSceneSettings sceneSettings;
        private static Scene scene;
        private static Thread sceneUpdateThread;
        private static RemoteState firstPanzerRemoteState;
        private static RemoteState secondPanzerRemoteState;


        /// <summary>
        /// Попытка подключиться к игровому серверу
        /// </summary>
        /// <returns>true - если подключиться удалось; в противном случае - false</returns>
        public bool ConnectToServer()
        {
            connectedUsersCount++;
            return true;
        }

        /// <summary>
        /// Отклчение клиента от сервера
        /// </summary>
        public void DisconnectFromServer()
        {
            connectedUsersCount = 0;
        }

        /// <summary>
        /// Получение количества подключенных устройств
        /// </summary>
        /// <returns>Количество подключенных устройств</returns>
        public int GetConnectedUsersCount()
        {
            return connectedUsersCount;
        }

        /// <summary>
        /// Задание инвентаря первому танку.
        /// </summary>
        /// <param name="amounts">Количество ракет.</param>
        public void SetFirstPanzerAmounts(params int[] amounts)
        {
            scene?.Dispose();
            sceneSettings.SetFirstPanzerAmounts(amounts);
            scene = new BattleScene(null, sceneSettings);
            scene.Init();
            sceneUpdateThread = new Thread(new ThreadStart(UpdateScene));
            sceneUpdateThread.Start();
            isInited = true;
        }

        /// <summary>
        /// Задание инвентаря второму танку.
        /// </summary>
        /// <param name="amounts">Количество ракет.</param>
        public void SetSecondPanzerAmounts(params int[] amounts)
        {
            sceneSettings = new BattleSceneSettings()
            {
                FirstPanzerControlType = BattleSceneSettings.PanzerControlType.Remote,
                SecondPanzerControlType = BattleSceneSettings.PanzerControlType.Remote,

                FirstPanzerHealth = 100,
                SecondPanzerHealth = 100,
            };

            firstPanzerRemoteState = sceneSettings.FirstPanzerRemoteState;
            secondPanzerRemoteState = sceneSettings.SecondPanzerRemoteState;

            sceneSettings.SetSecondPanzerAmounts(amounts);

            isSecondReady = true;
        }

        /// <summary>
        /// Флаг, свидетельствующий о готовности второго игрока
        /// </summary>
        /// <returns>true - если второй игрок готов; в противном случае - false</returns>
        public bool IsSecondReady()
        {
            return isSecondReady;
        }

        /// <summary>
        /// Получение текущих игровых объектов на сцене
        /// </summary>
        /// <returns>Текущие игровые объекты на сцене в виде JSON массива</returns>
        public string GetCurrentGameObjects()
        {
            if (scene == null || !isInited) return "[]";
            return JsonConvert.SerializeObject(scene.GetGameObjects(),
                Formatting.None, new Vector2Converter());
        }

        /// <summary>
        /// Установка состояния клавиатуры для первого игрока
        /// </summary>
        /// <param name="keyboard">Состояние клавиатуры</param>
        public void SetFirstPlayerKeyboardState(RemoteKeyboardState keyboard)
        {
            firstPanzerRemoteState.RemoteKeyboardState = keyboard;
        }

        /// <summary>
        /// Установка состояния клавиатуры для второго игрока
        /// </summary>
        /// <param name="keyboard">Состояние клавиатуры</param>
        public void SetSecondPlayerKeyboardState(RemoteKeyboardState keyboard)
        {
            secondPanzerRemoteState.RemoteKeyboardState = keyboard;
        }

        private void UpdateScene()
        {
            TimeSpan delta = new TimeSpan(0);
            var sw = new Stopwatch();
            while (!scene.IsDiposed && connectedUsersCount > 0)
            {
                scene.Update(delta);
                sw.Restart();
                Thread.Sleep(10);
                sw.Stop();
                delta = new TimeSpan(sw.ElapsedTicks);
            }
        }
    }
}
