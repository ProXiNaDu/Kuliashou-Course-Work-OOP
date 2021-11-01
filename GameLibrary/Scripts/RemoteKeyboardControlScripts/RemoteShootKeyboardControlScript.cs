using OpenTK.Input;
using GameEngineLibrary;
using OpenTK;
using System;
using GameLibrary.Components;
using GameLibrary.Scripts.RemoteKeyboardControlScripts;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, отвечающий за стрельбу танка при помощи клавиатуры.
    /// </summary>
    public class RemoteShootKeyboardControlScript : Script
    {
        private RemoteState remoteState;
        private readonly Scene scene;

        /// <summary>
        /// Поле, указывающее, находится ли на перезарядке танк.
        /// </summary>
        protected bool isCooldown;

        /// <summary>
        /// Время перезарядки.
        /// </summary>
        public int Cooldown { get; set; }

        /// <summary>
        /// Время, прошедшее с последнего выстрела.
        /// </summary>
        public int LastShoot { get; set; }

        /// <summary>
        /// Создание контроллера для выстрелов танка.
        /// </summary>
        /// <param name="scene">Сцена, в котрой будут осуществляться выстрелы.</param>
        public RemoteShootKeyboardControlScript(Scene scene)
        {
            Cooldown = 0;
            this.scene = scene;
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (isCooldown)
            {
                UpdateCooldown(delta);
            }
            else if (remoteState.RemoteKeyboardState.KeySpace)
            {
                Shoot();
            }
        }

        /// <summary>
        /// Обновление состояния перезарядки.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        protected void UpdateCooldown(TimeSpan delta)
        {
            LastShoot += delta.Milliseconds;
            if (LastShoot >= Cooldown)
            {
                isCooldown = false;
            }
        }

        /// <summary>
        /// Совершить выстрел ракетой.
        /// </summary>
        protected void Shoot()
        {
            Inventory inventory = controlledObject.GetComponent("inventory") as Inventory;
            GameObject rocket = inventory.GetRocket();
            if (rocket == null)
            {
                return;
            }

            Transform transform = controlledObject.GetComponent("transform") as Transform;
            Texture2D texture = controlledObject.GetComponent("texture") as Texture2D;
            Texture2D rocketTex = rocket.GetComponent("texture") as Texture2D;

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


            Transform rocketTransform = rocket.GetComponent("transform") as Transform;
            rocketTransform.Position = transform.Position + spawnPoint;
            rocketTransform.Rotation = transform.Rotation;
            rocketTransform.Scale = transform.Scale;

            rocket.AddScript(new PhysicScript(
                new Vector2((float)(-Math.Sign(transform.Scale.X) * 15 * Math.Cos(transform.Rotation)),
                            (float)(-15 * Math.Sin(transform.Rotation))),
                new Vector2(0, 0.2f)));

            scene.AddGameObject(rocket);

            Rocket rocketComponent = rocket.GetComponent("rocket") as Rocket;
            Cooldown = rocketComponent.Cooldown;
            LastShoot = 0;
            isCooldown = true;
        }

        /// <summary>
        /// Установка состояния удаленной машины
        /// </summary>
        /// <param name="remoteState">Состояние удаленной машины</param>
        public void SetRemoteState(RemoteState remoteState)
        {
            this.remoteState = remoteState;
        }
    }
}
