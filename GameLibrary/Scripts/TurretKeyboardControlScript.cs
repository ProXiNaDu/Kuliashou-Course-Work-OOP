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
        private const double MIN_ANGLE = 0;

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

        public override void Update(TimeSpan delta)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard[up] && controlledObject.Rotation < MAX_ANGLE)
            {
                controlledObject.Rotation += speed * delta.TotalSeconds;
            }
            if (keyboard[down] && controlledObject.Rotation > MIN_ANGLE)
            {
                controlledObject.Rotation -= speed * delta.TotalSeconds;
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
