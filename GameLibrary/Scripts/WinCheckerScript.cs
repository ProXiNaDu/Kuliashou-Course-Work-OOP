using GameEngineLibrary;
using GameLibrary.Components;
using System;
using System.Windows.Controls;

namespace GameLibrary.Scripts
{
    class WinCheckerScript : Script
    {
        private Health firstHealth;
        private Health secondHealth;
        private StackPanel winMenu;
        private bool isWin = false;

        public WinCheckerScript(GameObject first, GameObject second, StackPanel winMenu)
        {
            firstHealth = first.GetComponent("health") as Health;
            secondHealth = second.GetComponent("health") as Health;
            this.winMenu = winMenu;
        }
        
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
