using GameEngineLibrary;
using OpenTK;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, контролирующий управление башней танка.
    /// </summary>
    public class TurretAIControlScript : Script
    {
        private const double MAX_ANGLE = Math.PI * 3 / 8;
        private const double MIN_ANGLE = Math.PI / 4;

        private double speed;

        private Transform transform;
        private Texture2D texture;
        private Transform targetTransform;
        private Texture2D targetTexture;

        /// <summary>
        /// Создание контроллера для башни танка.
        /// </summary>
        /// <param name="speed">Скорость поворота башни.</param>
        public TurretAIControlScript(double speed)
        {
            this.speed = speed;
        }

        /// <summary>
        /// Инициализация скрипта.
        /// </summary>
        public override void Init()
        {
            transform = controlledObject.GetComponent("transform") as Transform;
            texture = controlledObject.GetComponent("texture") as Texture2D;
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            Vector2 aim = new Vector2(0, texture.Height / 2);
            aim -= transform.RotationPoint;
            aim = new Vector2(
                    (float)(Math.Cos(transform.Rotation) * aim.X -
                            Math.Sin(transform.Rotation) * aim.Y),
                    (float)(Math.Sin(transform.Rotation) * aim.X +
                            Math.Cos(transform.Rotation) * aim.Y));
            aim += transform.RotationPoint;
            aim *= transform.Scale;
            aim += transform.Position;

            Vector2 enemy = new Vector2(
                targetTransform.Position.X + targetTexture.Width * targetTransform.Scale.X / 2,
                targetTransform.Position.Y + targetTexture.Height * targetTransform.Scale.Y / 2);

            float height = enemy.Y - aim.Y;
            float gravity = 0.2f;
            Vector2 impuls = new Vector2(
                (float)(-Math.Sign(transform.Scale.X) * 15 *
                         Math.Cos(transform.Rotation)),
                (float)(-15 * Math.Sin(transform.Rotation)));
            float riseTime = -impuls.Y / gravity;
            float path = -(impuls.Y * riseTime - 
                gravity * riseTime * riseTime / 2);
            float fallTime = (float)Math.Sqrt(2 * (path + height) / gravity);
            float time = riseTime + fallTime;
            float targetX = aim.X + impuls.X * time / 1.25f;

            if (targetX * targetX < enemy.X * enemy.X && transform.Rotation > MIN_ANGLE)
            {
                transform.Rotation -= speed * delta.TotalSeconds;
            }
            if (targetX * targetX > enemy.X * enemy.X && transform.Rotation < MAX_ANGLE)
            {
                transform.Rotation += speed * delta.TotalSeconds;
            }
        }

        /// <summary>
        /// Установка цели, по которой будет осуществляться стрельба.
        /// </summary>
        /// <param name="target">Цель стрельбы.</param>
        public void SetTarget(GameObject target)
        {
            targetTransform = target.GetComponent("transform") as Transform;
            targetTexture = target.GetComponent("texture") as Texture2D;
        }
    }
}
