using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameEngineLibrary;
using OpenTK;
using GameLibrary.Scripts;

namespace GameTests
{
    [TestClass]
    public class PhysicTests
    {
        [TestMethod]
        public void FallingTest()
        {
            Random random = new Random();
            TimeSpan delta = new TimeSpan(1);

            for (int i = 0; i < 1000; i++)
            {
                Vector2 gravity = new Vector2(0, -random.Next(10, 100));
                PhysicScript physic = new PhysicScript(
                    Vector2.Zero, gravity);

                int height = random.Next(100, 1000);
                GameObject go = new GameObject(
                    null, new Vector2(0, height),
                    Vector2.Zero, Vector2.One, 0);
                go.AddScript(physic);

                int time = (int)Math.Ceiling(
                    Math.Sqrt(-2 * height / gravity.Y));

                for (int j = 0; j <= time; j++)
                {
                    go.Update(delta);
                }
                
                Transform transform = go.GetComponent("transform") as Transform;

                Assert.IsTrue(transform.Position.Y <= 0);
            }
        }

        [TestMethod]
        public void FallingWithStartSpeedTest()
        {
            Random random = new Random();
            TimeSpan delta = new TimeSpan(1);

            for (int i = 0; i < 1000; i++)
            {
                Vector2 speed = new Vector2(random.Next(10, 100), 0);
                Vector2 gravity = new Vector2(0, -random.Next(10, 100));
                PhysicScript physic = new PhysicScript(
                    speed, gravity);

                int height = random.Next(100, 1000);
                GameObject go = new GameObject(
                    null, new Vector2(0, height),
                    Vector2.Zero, Vector2.One, 0);
                go.AddScript(physic);

                int time = (int)Math.Ceiling(
                    Math.Sqrt(-2 * height / gravity.Y)) + 1;

                for (int j = 0; j < time; j++)
                {
                    go.Update(delta);
                }

                Transform transform = go.GetComponent("transform") as Transform;
                int expected = (int)speed.X * time;

                Assert.IsTrue(transform.Position.Y <= 0);
                Assert.AreEqual(expected, transform.Position.X);
            }
        }

        [TestMethod]
        public void UniformlyAcceleratedMotionTest()
        {
            Random random = new Random();
            TimeSpan delta = new TimeSpan(1);

            for (int i = 0; i < 1000; i++)
            {
                Vector2 acceleration = new Vector2(-random.Next(10, 100), 0);
                PhysicScript physic = new PhysicScript(
                    Vector2.Zero, acceleration);

                int moving = random.Next(100, 1000);
                GameObject go = new GameObject(
                    null, new Vector2(moving, 0),
                    Vector2.Zero, Vector2.One, 0);
                go.AddScript(physic);

                int time = (int)Math.Ceiling(
                    Math.Sqrt(-2 * moving / acceleration.X));

                for (int j = 0; j <= time; j++)
                {
                    go.Update(delta);
                }

                Transform transform = go.GetComponent("transform") as Transform;

                Assert.IsTrue(transform.Position.X <= 0);
                Assert.AreEqual(0, transform.Position.Y);
            }
        }
    }
}
