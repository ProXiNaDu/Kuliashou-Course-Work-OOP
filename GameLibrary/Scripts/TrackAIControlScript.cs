using GameEngineLibrary;
using OpenTK;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, отвечающий за движение танка.
    /// </summary>
    public class TrackAIControlScript : Script
    {
        private Transform transform;
        private Vector2 speed;
        private int range;
        private int pointToMove;
        private int minPosition;
        private int maxPosition;

        /// <summary>
        /// Создание контроллера для танка.
        /// </summary>
        /// <param name="speed">Скорость движения.</param>
        /// <param name="range">Максимальное расстояние, на которое может проехать танк.</param>
        public TrackAIControlScript(float speed, int range)
        {
            this.speed = new Vector2(speed, 0);
            this.range = range;
        }

        /// <summary>
        /// Инициализация объекта.
        /// </summary>
        public override void Init()
        {
            transform = controlledObject.GetComponent("transform") as Transform;

            minPosition = range;
            maxPosition = (int) Math.Abs(transform.Position.X);

            if (minPosition > maxPosition)
                (minPosition, maxPosition) = (maxPosition, minPosition);

            pointToMove = RandomManager.Next(minPosition, maxPosition);
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            int x = (int)Math.Abs(transform.Position.X);
            
            if (x > pointToMove)
            {
                transform.Position -= speed * (float)delta.TotalSeconds *
                    Math.Sign(transform.Position.X);
                x = (int)Math.Abs(transform.Position.X);
                if (x < pointToMove)
                    pointToMove = RandomManager.Next(minPosition, maxPosition);
            }
            else
            {
                transform.Position += speed * (float)delta.TotalSeconds *
                    Math.Sign(transform.Position.X);
                x = (int)Math.Abs(transform.Position.X);
                if (x > pointToMove)
                    pointToMove = RandomManager.Next(minPosition, maxPosition);
            }

            controlledObject.UpdateColliderToTexture();
        }
    }
}
