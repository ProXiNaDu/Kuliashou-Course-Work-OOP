using OpenTK.Input;
using GameEngineLibrary;
using OpenTK;
using System;
using GameLibrary.Scripts.RemoteKeyboardControlScripts;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, отвечающий за передвижение танка при помощи клавиатуры.
    /// </summary>
    public class RemoteTrackKeyboardControlScript : Script
    {
        private RemoteState remoteState;
        private Vector2 speed;
        Scene scene;

        /// <summary>
        /// Создание контроллера для танка.
        /// </summary>
        /// <param name="scene">Сцена, в которой происходит перемещение объекта.</param>
        /// <param name="speed">Скорость движения.</param>
        public RemoteTrackKeyboardControlScript(Scene scene, float speed)
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

            if (remoteState.RemoteKeyboardState.KeyA)
            {
                translate -= speed * (float)delta.TotalSeconds;
            }
            if (remoteState.RemoteKeyboardState.KeyD)
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
        /// Установка состояния удаленной машины
        /// </summary>
        /// <param name="remoteState">Состояние удаленной машины</param>
        public void SetRemoteState(RemoteState remoteState)
        {
            this.remoteState = remoteState;
        }
    }
}
