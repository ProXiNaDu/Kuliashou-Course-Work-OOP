using OpenTK.Input;
using GameEngineLibrary;
using OpenTK;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, отвечающий за передвижение танка при помощи клавиатуры.
    /// </summary>
    public class TrackKeyboardControlScript : Script
    {
        private Key left;
        private Key right;
        private Vector2 speed;

        /// <summary>
        /// Создание контроллера для танка.
        /// </summary>
        /// <param name="speed">Скорость движения.</param>
        public TrackKeyboardControlScript(float speed)
        {
            this.speed = new Vector2(speed, 0);
        }

        public override void Update(TimeSpan delta)
        {
            KeyboardState keyboard = Keyboard.GetState();
            Transform transform = controlledObject.GetComponent("transform") as Transform;

            if (keyboard[left])
            {
                transform.Position -= speed * (float)delta.TotalSeconds;
            }
            if (keyboard[right])
            {
                transform.Position += speed * (float)delta.TotalSeconds;
            }

            controlledObject.UpdateColliderToTexture();
        }

        /// <summary>
        /// Установить кнопку для движения влево.
        /// </summary>
        /// <param name="key">Кнопка на клавиатуре.</param>
        public void SetKeyToMoveLeft(Key key)
        {
            left = key;
        }

        /// <summary>
        /// Установить кнопку для движения вправо.
        /// </summary>
        /// <param name="key">Кнопка на клавиатуре.</param>
        public void SetKeyToMoveRight(Key key)
        {
            right = key;
        }
    }
}
