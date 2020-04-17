using GameEngineLibrary;
using GameLibrary.Components;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, отвечающий за переключение ракет танка с искусственным интеллектом.
    /// </summary>
    public class AIRocketSwitcherScript : Script
    {
        private bool isCooldown = false;
        private int lastPressTime = 0;
        private int cooldown;
        private int minCooldown;
        private int maxCooldown;
        private Inventory inventory;

        /// <summary>
        /// Создание скрипта, управляющего сменой ракет у
        /// танка с искусственным интеллектом.
        /// </summary>
        /// <param name="minCooldown">Минимальная частота смены.</param>
        /// <param name="maxCooldown">Максимальная частота смены.</param>
        public AIRocketSwitcherScript(int minCooldown, int maxCooldown)
        {
            this.minCooldown = minCooldown;
            this.maxCooldown = maxCooldown;
        }

        /// <summary>
        /// Инициализация скрипта.
        /// </summary>
        public override void Init()
        {
            inventory = controlledObject.GetComponent("inventory") as Inventory;
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            if (isCooldown)
            {
                lastPressTime += delta.Milliseconds;
                if (lastPressTime > cooldown)
                    isCooldown = false;
                return;
            }

            if (inventory.GetTotalAmount() == 0)
                return;

            if (RandomManager.Next(2) == 0)
                do
                    inventory.SelectNext();
                while (inventory.GetAmount() == 0);
            else
                do
                    inventory.SelectPrevious();
                while (inventory.GetAmount() == 0);

            cooldown = RandomManager.Next(minCooldown, maxCooldown);
            lastPressTime = 0;
            isCooldown = true;
        }
    }
}