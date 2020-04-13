using GameEngineLibrary;
using GameLibrary.Components;
using System;
using System.Windows.Controls;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, проверяющий, победил ли кто-либо из игроков.
    /// </summary>
    public class WinCheckerScript : Script
    {
        private Health firstHealth;
        private Health secondHealth;
        private StackPanel winMenu;
        private bool isWin = false;

        /// <summary>
        /// Создание скрипта для проверки побед.
        /// </summary>
        /// <param name="first">Первый танк.</param>
        /// <param name="second">Второй танк.</param>
        /// <param name="winMenu">Меню, которое будет отображено в случае победы.</param>
        public WinCheckerScript(GameObject first, GameObject second, StackPanel winMenu)
        {
            firstHealth = first.GetComponent("health") as Health;
            secondHealth = second.GetComponent("health") as Health;
            this.winMenu = winMenu;
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            if (!isWin && (!firstHealth.IsAlive() || !secondHealth.IsAlive()))
            {
                winMenu.Visibility = System.Windows.Visibility.Visible;
                isWin = true;
            }
        }
    }
}
