using GameLibrary.Components;

namespace GameLibrary.Scenes
{
    /// <summary>
    /// Настройки сцены.
    /// </summary>
    public class BattleSceneSettings
    {
        /// <summary>
        /// Перечисление типов управления танком.
        /// </summary>
        public enum PanzerControlType
        {
            /// <summary>
            /// Управление искусственным интеллектом.
            /// </summary>
            AI,
            /// <summary>
            /// Управление с клавиатуры.
            /// </summary>
            Keyboard,
            /// <summary>
            /// Управление по сети
            /// </summary>
            Remote
        }

        private const int ROCKET_TYPES = 3;

        private int[] firstPanzerRocketAmounts;
        private int[] secondPanzerRocketAmounts;

        /// <summary>
        /// Здоровье первого танка.
        /// </summary>
        public int FirstPanzerHealth { get; set; }

        /// <summary>
        /// Здоровье второго танка.
        /// </summary>
        public int SecondPanzerHealth { get; set; }

        /// <summary>
        /// Тип управления первого танка.
        /// </summary>
        public PanzerControlType FirstPanzerControlType { get; set; } 
            = PanzerControlType.AI;

        /// <summary>
        /// Тип управления второго танка.
        /// </summary>
        public PanzerControlType SecondPanzerControlType { get; set; }
            = PanzerControlType.AI;

        /// <summary>
        /// Создание настроек сцены.
        /// </summary>
        public BattleSceneSettings()
        {
            firstPanzerRocketAmounts = new int[ROCKET_TYPES];
            secondPanzerRocketAmounts = new int[ROCKET_TYPES];
        }

        /// <summary>
        /// Задание инвентаря первому танку.
        /// </summary>
        /// <param name="amounts">Количество ракет.</param>
        public void SetFirstPanzerAmounts(params int[] amounts)
        {
            SetAmounts(firstPanzerRocketAmounts, amounts);
        }

        /// <summary>
        /// Задание инвентаря второму танку.
        /// </summary>
        /// <param name="amounts">Количество ракет.</param>
        public void SetSecondPanzerAmounts(params int[] amounts)
        {
            SetAmounts(secondPanzerRocketAmounts, amounts);
        }

        /// <summary>
        /// Заполнение инвентаря первого танка.
        /// </summary>
        /// <param name="inventory">Инвентарь танка.</param>
        public void FillFirtsPanzerInventory(Inventory inventory)
        {
            FillInventory(inventory, firstPanzerRocketAmounts);
        }

        /// <summary>
        /// Заполнение инвентаря второго танка.
        /// </summary>
        /// <param name="inventory">Инвентарь танка.</param>
        public void FillSecondPanzerInventory(Inventory inventory)
        {
            FillInventory(inventory, secondPanzerRocketAmounts);
        }

        /// <summary>
        /// Заполнение инвенторя танка.
        /// </summary>
        /// <param name="inventory">Инвентарь для заполнения.</param>
        /// <param name="amounts">Количества ракет.</param>
        private void FillInventory(Inventory inventory, int[] amounts)
        {
            foreach (int amount in amounts)
            {
                inventory.SetAmount(amount);
                inventory.SelectNext();
            }
        }

        /// <summary>
        /// Установка количеств ракет у танка.
        /// </summary>
        /// <param name="panzerAmounts">Количество ракет танка.</param>
        /// <param name="amounts">Новое количество ракет танка.</param>
        private void SetAmounts(int[] panzerAmounts, int[] amounts)
        {
            for (int i = 0; i < amounts.Length; i++)
            {
                panzerAmounts[i] = amounts[i];
            }
        }
    }
}
