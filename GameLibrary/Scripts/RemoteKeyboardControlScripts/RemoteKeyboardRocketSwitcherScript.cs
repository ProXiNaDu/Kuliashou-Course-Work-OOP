using GameEngineLibrary;
using GameLibrary.Components;
using GameLibrary.Scripts.RemoteKeyboardControlScripts;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт для управления переключением ракет при помощи клавиатуры.
    /// </summary>
    public class RemoteKeyboardRocketSwitcherScript : Script
    {
        private RemoteState remoteState;
        private bool isCooldown = false;
        private int lastPressTime = 0;
        private const int COOLDOWN = 100;

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            if (isCooldown)
            {
                lastPressTime += delta.Milliseconds;
                if (lastPressTime > COOLDOWN)
                {
                    isCooldown = false;
                }
                return;
            }

            Inventory inventory = controlledObject.GetComponent("inventory") as Inventory;

            if (remoteState.RemoteKeyboardState.KeyE)
            {
                inventory.SelectNext();
                lastPressTime = 0;
                isCooldown = true;
                return;
            }
            if (remoteState.RemoteKeyboardState.KeyQ)
            {
                inventory.SelectPrevious();
                lastPressTime = 0;
                isCooldown = true;
                return;
            }
        }

        /// <summary>
        /// Установка состояния удаленной машины
        /// </summary>
        /// <param name="remoteState">Состояние удаленной машины</param>
        public void SetRemoteState(RemoteState remoteState)
        {
            this.remoteState = remoteState;
        }
    }
}
