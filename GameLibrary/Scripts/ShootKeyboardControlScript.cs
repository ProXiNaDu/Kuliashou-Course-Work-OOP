using OpenTK.Input;
using GameEngineLibrary;
using OpenTK;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, отвечающий за стрельбу танка при помощи клавиатуры.
    /// </summary>
    public class ShootKeyboardControlScript : Script
    {
        private readonly IScene scene;
        private readonly Texture2D rocketTex;
        private Key shoot;
        private readonly double cooldown;
        private double lastShoot;
        private bool isCooldown;

        /// <summary>
        /// Создание контроллера для выстрелов танка.
        /// </summary>
        public ShootKeyboardControlScript(IScene scene, Texture2D rocketTex, double cooldown)
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
                float x = rocketTex.Width * controlledObject.Scale.X;
                float y = rocketTex.Height * controlledObject.Scale.Y;

                Vector2 spawnPoint = new Vector2(
                    -controlledObject.Texture.Width * controlledObject.Scale.X,
                    -controlledObject.Texture.Height * controlledObject.Scale.Y);

                spawnPoint = new Vector2(
                    (float)(Math.Cos(controlledObject.Rotation *
                            Math.Sign(controlledObject.Scale.X)) * spawnPoint.X -
                            Math.Sin(controlledObject.Rotation *
                            Math.Sign(controlledObject.Scale.X)) * spawnPoint.Y),
                    (float)(Math.Sin(controlledObject.Rotation * 
                            Math.Sign(controlledObject.Scale.X)) * spawnPoint.X +
                            Math.Cos(controlledObject.Rotation *
                            Math.Sign(controlledObject.Scale.X)) * spawnPoint.Y));

                Vector2 rocketPoint = new Vector2(
                    (float)(Math.Cos(controlledObject.Rotation *
                            -Math.Sign(controlledObject.Scale.X)) * -x -
                            Math.Sin(controlledObject.Rotation *
                            -Math.Sign(controlledObject.Scale.X)) * -y),
                    (float)(Math.Sin(controlledObject.Rotation *
                            -Math.Sign(controlledObject.Scale.X)) * -x +
                            Math.Cos(controlledObject.Rotation *
                            -Math.Sign(controlledObject.Scale.X)) * -y));

                spawnPoint.Y += controlledObject.Texture.Height * controlledObject.Scale.Y / 2;
                spawnPoint.X -= rocketPoint.X / 2;
                spawnPoint.Y -= rocketPoint.Y / 2;

                scene.AddGameObject(CreateRocket(
                    controlledObject.Position + spawnPoint,
                    controlledObject.Rotation));

                lastShoot = 0;
                isCooldown = true;
            }
        }

        private GameObject CreateRocket(Vector2 position, double rotation)
        {
            GameObject rocket = new GameObject(rocketTex);
            rocket.RotationPoint = new Vector2(rocketTex.Width / 2, rocketTex.Height / 2);
            rocket.Scale = controlledObject.Scale;
            rocket.Position = position;
            rocket.Rotation = rotation;

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
