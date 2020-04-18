using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLibrary.Components;
using GameEngineLibrary;

namespace GameTests
{
    [TestClass]
    public class InventoryTests
    {
        [TestMethod]
        public void InventoryAmountTest()
        {
            Random random = new Random();
            Inventory.RocketBuilder[] rockets =
            {
                new Inventory.RocketBuilder(null, null, null, null)
            };
            Inventory inventory = new Inventory(rockets);

            Assert.AreEqual(0, inventory.GetAmount());

            for (int i = 0; i < 1000; i++)
            {
                int expected = random.Next(100);

                inventory.SetAmount(expected);

                Assert.AreEqual(expected, inventory.GetAmount());
            }
        }

        [TestMethod]
        public void InventoryGetRocketTest()
        {
            Random random = new Random();
            Inventory.RocketBuilder[] rockets =
            {
                new Inventory.RocketBuilder(null, new Texture2D(0, 0, 0), null, null)
            };
            Inventory inventory = new Inventory(rockets);
            GameObject rocket;

            for (int i = 0; i < 1000; i++)
            {
                int amount = random.Next(100);
                inventory.SetAmount(amount);
                for (int j = amount; j > 0; j--)
                {
                    Assert.AreEqual(j, inventory.GetAmount());
                    rocket = inventory.GetRocket();
                    Assert.IsNotNull(rocket);
                }

                rocket = inventory.GetRocket();
                Assert.IsNull(rocket);
            }
        }
    }
}
