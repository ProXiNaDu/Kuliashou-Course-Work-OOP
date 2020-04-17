using GameEngineLibrary;
using OpenTK.Input;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, контролирующий управление башней танка.
    /// </summary>
    public class TurretKeyboardControlScript : Script
    {
        private const double MAX_ANGLE = Math.PI * 3 / 8;
        private const double MIN_ANGLE = Math.PI / 8;
        private Transform transform;

        private Key up;
        private Key down;
        private double speed;

        /// <summary>
        /// Создание контроллера для башни танка.
        /// </summary>
        /// <param name="speed">Скорость поворота башни.</param>
        public TurretKeyboardControlScript(double speed)
        {
            this.speed = speed;
        }

        /// <summary>
        /// Инициализация скрипта.
        /// </summary>
        public override void Init()
        {
            transform = controlledObject.GetComponent("transform") as Transform;
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard[up] && transform.Rotation < MAX_ANGLE)
            {
                transform.Rotation += speed * delta.TotalSeconds;
            }
            if (keyboard[down] && transform.Rotation > MIN_ANGLE)
            {
                transform.Rotation -= speed * delta.TotalSeconds;
            }
        }

        /// <summary>
        /// Установить кнопку для поворота вверх.
        /// </summary>
        /// <param name="key">Кнопка на клавиатуре.</param>
        public void SetKeyToTurnUp(Key key)
        {
            up = key;
        }

        /// <summary>
        /// Установить кнопку для поворота вниз.
        /// </summary>
        /// <param name="key">Кнопка на клавиатуре.</param>
        public void SetKeyToTurnDown(Key key)
        {
            down = key;
        }
    }
}
