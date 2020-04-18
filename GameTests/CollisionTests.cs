using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameEngineLibrary;
using OpenTK;

namespace GameTests
{
    [TestClass]
    public class CollisionTests
    {
        [TestMethod]
        public void SameVertecesTest()
        {
            Vector2[] firstVerteces =
            {
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
            };
            Vector2[] secondVerteces =
            {
                new Vector2(1, 0),
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
            };

            Collider first = new Collider(firstVerteces);
            Collider second = new Collider(secondVerteces);

            Assert.IsTrue(first.CheckCollision(second));
        }

        [TestMethod]
        public void DifferentVertecesTest()
        {
            Vector2[] firstVerteces =
            {
                new Vector2(-10, -1),
                new Vector2(-1, -1),
                new Vector2(-1, -10),
                new Vector2(-10, -10),
            };
            Vector2[] secondVerteces =
            {
                new Vector2(10, 1),
                new Vector2(1, 0),
                new Vector2(1, 10),
                new Vector2(10, 10),
            };

            Collider first = new Collider(firstVerteces);
            Collider second = new Collider(secondVerteces);

            Assert.IsFalse(first.CheckCollision(second));
        }

        [TestMethod]
        public void RandomVertecesTest()
        {
            Random random = new Random();

            int side = 10;
            Vector2[] firstVerteces =
            {
                new Vector2(10, 0),
                new Vector2(0, 0),
                new Vector2(0, 10),
                new Vector2(10, 10),
            };
            Vector2[] secondVerteces =
            {
                new Vector2(10, 0),
                new Vector2(0, 0),
                new Vector2(0, 10),
                new Vector2(10, 10),
            };

            for (int i = 0; i < 10000; i++)
            {
                Vector2 firstTranslate = new Vector2(
                    random.Next(-10, 10), 
                    random.Next(-10, 10));
                Vector2 secondTranslate = new Vector2(
                    random.Next(-10, 10),
                    random.Next(-10, 10));

                MoveVerteces(firstVerteces, firstTranslate);
                MoveVerteces(secondVerteces, secondTranslate);

                Collider first = new Collider(firstVerteces);
                Collider second = new Collider(secondVerteces);

                bool expected = 
                    Math.Abs(firstVerteces[1].X - secondVerteces[1].X) <= side &&
                    Math.Abs(firstVerteces[1].Y - secondVerteces[1].Y) <= side;

                Assert.AreEqual(expected, first.CheckCollision(second));
            }
        }

        [TestMethod]
        public void SameTrianglesTest()
        {
            Vector2[] firstVerteces =
            {
                new Vector2(0, 3),
                new Vector2(-2, 0),
                new Vector2(2, 0),
            };
            Vector2[] secondVerteces =
            {
                new Vector2(0, 3),
                new Vector2(-2, 0),
                new Vector2(2, 0),
            };

            Collider first = new Collider(firstVerteces);
            Collider second = new Collider(secondVerteces);

            Assert.IsTrue(first.CheckCollision(second));
        }

        [TestMethod]
        public void IntersectedTrianglesTest()
        {
            Vector2[] firstVerteces =
            {
                new Vector2(0, 3),
                new Vector2(-2, 0),
                new Vector2(2, 0),
            };
            Vector2[] secondVerteces =
            {
                new Vector2(0, 1),
                new Vector2(2, 4),
                new Vector2(-2, 4),
            };

            Collider first = new Collider(firstVerteces);
            Collider second = new Collider(secondVerteces);

            Assert.IsTrue(first.CheckCollision(second));
        }

        [TestMethod]
        public void DifferentTrianglesTest()
        {
            Vector2[] firstVerteces =
            {
                new Vector2(0, 3),
                new Vector2(-2, 0),
                new Vector2(2, 0),
            };
            Vector2[] secondVerteces =
            {
                new Vector2(5, 4),
                new Vector2(7, 7),
                new Vector2(3, 7),
            };

            Collider first = new Collider(firstVerteces);
            Collider second = new Collider(secondVerteces);

            Assert.IsFalse(first.CheckCollision(second));
        }

        private void MoveVerteces(Vector2[] verteces, Vector2 translate)
        {
            for (int i = 0; i < verteces.Length; i++)
            {
                verteces[i] += translate;
            }
        }
    }
}
