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
        Scene scene;

        /// <summary>
        /// Создание контроллера для танка.
        /// </summary>
        /// <param name="scene">Сцена, в которой происходит перемещение объекта.</param>
        /// <param name="speed">Скорость движения.</param>
        public TrackKeyboardControlScript(Scene scene, float speed)
        {
            this.scene = scene;
            this.speed = new Vector2(speed, 0);
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            KeyboardState keyboard = Keyboard.GetState();
            Transform transform = controlledObject.GetComponent("transform") as Transform;

            Vector2 translate = Vector2.Zero;

            if (keyboard[left])
            {
                translate -= speed * (float)delta.TotalSeconds;
            }
            if (keyboard[right])
            {
                translate += speed * (float)delta.TotalSeconds;
            }

            transform.Position += translate;

            controlledObject.UpdateColliderToTexture();

            Collider thisCollider = controlledObject.GetComponent("collider") as Collider;
            foreach (GameObject gameObject in scene.GetGameObjects())
            {
                Collider collider = gameObject.GetComponent("collider") as Collider;
                if (gameObject != controlledObject &&
                    collider != null &&
                    collider.CheckCollision(thisCollider))
                {
                    transform.Position -= translate;
                    controlledObject.UpdateColliderToTexture();
                    break;
                }
            }

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
