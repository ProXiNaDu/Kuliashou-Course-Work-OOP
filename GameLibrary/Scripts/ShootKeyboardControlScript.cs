using OpenTK.Input;
using GameEngineLibrary;
using OpenTK;
using System;
using GameLibrary.Components;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, отвечающий за стрельбу танка при помощи клавиатуры.
    /// </summary>
    public class ShootKeyboardControlScript : Script
    {
        private readonly Scene scene;
        private readonly Texture2D rocketTex;
        private Key shoot;
        private readonly double cooldown;
        private double lastShoot;
        private bool isCooldown;

        /// <summary>
        /// Создание контроллера для выстрелов танка.
        /// </summary>
        public ShootKeyboardControlScript(Scene scene, Texture2D rocketTex, double cooldown)
        {
            this.cooldown = cooldown;
            this.scene = scene;
            this.rocketTex = rocketTex;
        }

        public override void Update(TimeSpan delta)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (isCooldown)
            {
                lastShoot += delta.TotalMilliseconds;
                if (lastShoot >= cooldown)
                {
                    isCooldown = false;
                }
            }
            else if (keyboard[shoot])
            {
                Transform transform = controlledObject.GetComponent("transform") as Transform;
                Texture2D texture = controlledObject.GetComponent("texture") as Texture2D;

                float x = rocketTex.Width * transform.Scale.X;
                float y = rocketTex.Height * transform.Scale.Y;

                Vector2 spawnPoint = new Vector2(
                    -texture.Width * transform.Scale.X,
                    -texture.Height * transform.Scale.Y);

                spawnPoint = new Vector2(
                    (float)(Math.Cos(transform.Rotation *
                            Math.Sign(transform.Scale.X)) * spawnPoint.X -
                            Math.Sin(transform.Rotation *
                            Math.Sign(transform.Scale.X)) * spawnPoint.Y),
                    (float)(Math.Sin(transform.Rotation * 
                            Math.Sign(transform.Scale.X)) * spawnPoint.X +
                            Math.Cos(transform.Rotation *
                            Math.Sign(transform.Scale.X)) * spawnPoint.Y));

                Vector2 rocketPoint = new Vector2(
                    (float)(Math.Cos(transform.Rotation *
                            -Math.Sign(transform.Scale.X)) * -x -
                            Math.Sin(transform.Rotation *
                            -Math.Sign(transform.Scale.X)) * -y),
                    (float)(Math.Sin(transform.Rotation *
                            -Math.Sign(transform.Scale.X)) * -x +
                            Math.Cos(transform.Rotation *
                            -Math.Sign(transform.Scale.X)) * -y));

                spawnPoint.Y += texture.Height * transform.Scale.Y / 2;
                spawnPoint.X -= rocketPoint.X / 2;
                spawnPoint.Y -= rocketPoint.Y / 2;

                scene.AddGameObject(CreateRocket(
                    transform.Position + spawnPoint,
                    transform.Rotation));

                lastShoot = 0;
                isCooldown = true;
            }
        }

        private GameObject CreateRocket(Vector2 position, double rotation)
        {
            Transform transform = controlledObject.GetComponent("transform") as Transform;
            GameObject rocket = new GameObject(rocketTex, position,
                new Vector2(rocketTex.Width / 2, rocketTex.Height / 2),
                transform.Scale, rotation);
            rocket.AddComponent("rocket", new Rocket());
            rocket.AddScript(new RocketHitScript(scene));
            rocket.AddScript(new PhysicScript(
                new Vector2((float) (-Math.Sign(transform.Scale.X) * 3000 * Math.Cos(rotation)),
                            (float) (-3000 * Math.Sin(rotation))),
                new Vector2(0, 40)));
            
            return rocket;
        }

        /// <summary>
        /// Установить кнопку выстрела.
        /// </summary>
        /// <param name="key">Кнопка на клавиатуре.</param>
        public void SetKey(Key key)
        {
            shoot = key;
        }
    }
}
