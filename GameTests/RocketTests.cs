using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLibrary.Components;
using GameLibrary.Components.RocketDecorators;

namespace GameTests
{
    [TestClass]
    public class RocketTests
    {
        [TestMethod]
        public void DoubleDamageRocketTest()
        {
            Rocket rocket = new BaseRocket();
            for (int i = 0; i < 10; i++)
            {
                int expected = rocket.Damage * 2;
                rocket = new DoubleDamageRocket(rocket);
                int actual = rocket.Damage;

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void HalfDamageRocketTest()
        {
            Rocket rocket = new BaseRocket();
            for (int i = 0; i < 10; i++)
            {
                int expected = rocket.Damage / 2;
                rocket = new HalfDamageRocket(rocket);
                int actual = rocket.Damage;

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void DoubleCooldownRocketTest()
        {
            Rocket rocket = new BaseRocket();
            for (int i = 0; i < 10; i++)
            {
                int expected = rocket.Cooldown * 2;
                rocket = new DoubleCooldownRocket(rocket);
                int actual = rocket.Cooldown;

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void HalfCooldownRocketTest()
        {
            Rocket rocket = new BaseRocket();
            for (int i = 0; i < 10; i++)
            {
                int expected = rocket.Cooldown / 2;
                rocket = new HalfCooldownRocket(rocket);
                int actual = rocket.Cooldown;

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void DoubleDamageAndCooldownRocketTest()
        {
            Rocket rocket = new BaseRocket();
            for (int i = 0; i < 10; i++)
            {
                int expectedDamage = rocket.Damage * 2;
                int expectedCooldown = rocket.Cooldown * 2;

                rocket = new DoubleDamageRocket(rocket);
                rocket = new DoubleCooldownRocket(rocket);

                int actualDamage = rocket.Damage;
                int actualCooldown = rocket.Cooldown;

                Assert.AreEqual(expectedDamage, actualDamage);
                Assert.AreEqual(expectedCooldown, actualCooldown);
            }
        }

        [TestMethod]
        public void HalfDamageAndCooldownRocketTest()
        {
            Rocket rocket = new BaseRocket();
            for (int i = 0; i < 10; i++)
            {
                int expectedDamage = rocket.Damage / 2;
                int expectedCooldown = rocket.Cooldown / 2;

                rocket = new HalfDamageRocket(rocket);
                rocket = new HalfCooldownRocket(rocket);

                int actualDamage = rocket.Damage;
                int actualCooldown = rocket.Cooldown;

                Assert.AreEqual(expectedDamage, actualDamage);
                Assert.AreEqual(expectedCooldown, actualCooldown);
            }
        }

        [TestMethod]
        public void DoubleDamageAndHalfCooldownRocketTest()
        {
            Rocket rocket = new BaseRocket();
            for (int i = 0; i < 10; i++)
            {
                int expectedDamage = rocket.Damage * 2;
                int expectedCooldown = rocket.Cooldown / 2;

                rocket = new DoubleDamageRocket(rocket);
                rocket = new HalfCooldownRocket(rocket);

                int actualDamage = rocket.Damage;
                int actualCooldown = rocket.Cooldown;

                Assert.AreEqual(expectedDamage, actualDamage);
                Assert.AreEqual(expectedCooldown, actualCooldown);
            }
        }

        [TestMethod]
        public void HalfDamageAndDoubleCooldownRocketTest()
        {
            Rocket rocket = new BaseRocket();
            for (int i = 0; i < 10; i++)
            {
                int expectedDamage = rocket.Damage / 2;
                int expectedCooldown = rocket.Cooldown * 2;

                rocket = new HalfDamageRocket(rocket);
                rocket = new DoubleCooldownRocket(rocket);

                int actualDamage = rocket.Damage;
                int actualCooldown = rocket.Cooldown;

                Assert.AreEqual(expectedDamage, actualDamage);
                Assert.AreEqual(expectedCooldown, actualCooldown);
            }
        }
    }
}
