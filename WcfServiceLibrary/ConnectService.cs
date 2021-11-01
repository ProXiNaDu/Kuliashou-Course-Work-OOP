using GameEngineLibrary;
using GameLibrary;
using GameLibrary.Scenes;
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
        private static BattleSceneSettings sceneSettings;
        private static Scene scene;
        private static Thread sceneUpdateThread;

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
        }

        /// <summary>
        /// Задание инвентаря второму танку.
        /// </summary>
        /// <param name="amounts">Количество ракет.</param>
        public void SetSecondPanzerAmounts(params int[] amounts)
        {
            sceneSettings = new BattleSceneSettings()
            {
                FirstPanzerControlType = BattleSceneSettings.PanzerControlType.AI,
                SecondPanzerControlType = BattleSceneSettings.PanzerControlType.AI,

                FirstPanzerHealth = 100,
                SecondPanzerHealth = 100,
            };

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
            if (scene == null) return "[]";
            return JsonConvert.SerializeObject(scene.GetGameObjects(),
                Formatting.None, new Vector2Converter());
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
