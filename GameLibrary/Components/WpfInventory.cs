using System.Windows.Controls;
using System.Windows.Media;
using GameEngineLibrary;

namespace GameLibrary.Components
{
    /// <summary>
    /// Инвентарь, который способен отображать информацию о 
    /// содержимом в специальном поле приложения WPF.
    /// </summary>
    public class WpfInventory : Inventory
    {
        /// <summary>
        /// Поле, в котором будет отобрааться информация о содержимом инвентаря.
        /// </summary>
        private StackPanel inventory;

        private Brush unselected;
        private Brush selected;

        /// <summary>
        /// Создание нового WPF инвентаря.
        /// </summary>
        /// <param name="inventory">Поле, в котором будет отображаться содержимое.</param>
        /// <param name="rockets">Ракеты в инвентаре.</param>
        public WpfInventory(StackPanel inventory, params RocketBuilder[] rockets)
            : base(rockets)
        {
            this.inventory = inventory;
            unselected = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            selected = new SolidColorBrush(Color.FromRgb(191, 55, 160));
        }

        /// <summary>
        /// Выбор следующего предмета в инвентаре.
        /// </summary>
        public override void SelectNext()
        {
            ((Label)inventory.Children[current]).Foreground = unselected;
            base.SelectNext();
            ((Label)inventory.Children[current]).Foreground = selected;
        }

        /// <summary>
        /// Выбор предыдущего предмета в инвентаре.
        /// </summary>
        public override void SelectPrevious()
        {
            ((Label)inventory.Children[current]).Foreground = unselected;
            base.SelectPrevious();
            ((Label)inventory.Children[current]).Foreground = selected;
        }

        /// <summary>
        /// Установка количества предмета в инвентаре.
        /// </summary>
        public override void SetAmount(int amount)
        {
            base.SetAmount(amount);
            ((Label)inventory.Children[current]).Content = amount;
        }

        /// <summary>
        /// Получение предмета из инвентаря.
        /// </summary>
        public override GameObject GetRocket()
        {
            GameObject rocket = base.GetRocket();
            ((Label)inventory.Children[current]).Content = amounts[current];
            return rocket;
        }
    }
}
