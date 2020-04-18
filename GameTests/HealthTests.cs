using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLibrary.Components;

namespace GameTests
{
    [TestClass]
    public class HealthTests
    {
        [TestMethod]
        public void HealthDamageTest()
        {
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int startHealth = random.Next(10000);
                int damage = random.Next(100);

                Health health = new Health(startHealth);
                health.Damage(damage);

                int expected = startHealth - damage;
                int actual = health.Value;

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void HealthHealTest()
        {
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int startHealth = random.Next(10000);
                int heal = random.Next(100);

                Health health = new Health(startHealth);
                health.Heal(heal);

                int expected = startHealth + heal;
                int actual = health.Value;

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void HealthIsAliveTest()
        {
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int startHealth = random.Next(100);
                int damage = random.Next(100);

                Health health = new Health(startHealth);
                health.Damage(damage);

                bool expected = (startHealth - damage) > 0;
                bool actual = health.IsAlive();

                Assert.AreEqual(expected, actual);
            }
        }
    }
}
