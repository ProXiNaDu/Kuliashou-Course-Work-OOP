using GameLibrary.Components;

namespace GameLibrary.Scenes
{
    public class BattleSceneSettings
    {
        private const int ROCKET_TYPES = 3;

        private int[] firstPanzerRocketAmounts;
        private int[] secondPanzerRocketAmounts;

        public BattleSceneSettings()
        {
            firstPanzerRocketAmounts = new int[ROCKET_TYPES];
            secondPanzerRocketAmounts = new int[ROCKET_TYPES];
        }

        public void SetFirstPanzerAmounts(params int[] amounts)
        {
            SetAmounts(firstPanzerRocketAmounts, amounts);
        }

        public void SetSecondPanzerAmounts(params int[] amounts)
        {
            SetAmounts(secondPanzerRocketAmounts, amounts);
        }

        public void FillFirtsPanzerInventory(Inventory inventory)
        {
            FillInventory(inventory, firstPanzerRocketAmounts);
        }

        public void FillSecondPanzerInventory(Inventory inventory)
        {
            FillInventory(inventory, secondPanzerRocketAmounts);
        }

        private void FillInventory(Inventory inventory, int[] amounts)
        {
            foreach (int amount in amounts)
            {
                inventory.SetAmount(amount);
                inventory.SelectNext();
            }
        }

        private void SetAmounts(int[] panzerAmounts, int[] amounts)
        {
            for (int i = 0; i < amounts.Length; i++)
            {
                panzerAmounts[i] = amounts[i];
            }
        }
    }
}
