using GameEngineLibrary;
using GameLibrary.Scripts.RemoteKeyboardControlScripts;
using OpenTK.Input;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, контролирующий управление башней танка.
    /// </summary>
    public class RemoteTurretKeyboardControlScript : Script
    {
        private RemoteState remoteState;
        private const double MAX_ANGLE = Math.PI * 3 / 8;
        private const double MIN_ANGLE = Math.PI / 8;
        private Transform transform;

        private double speed;

        /// <summary>
        /// Создание контроллера для башни танка.
        /// </summary>
        /// <param name="speed">Скорость поворота башни.</param>
        public RemoteTurretKeyboardControlScript(double speed)
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

            if (remoteState.RemoteKeyboardState.KeyW && transform.Rotation < MAX_ANGLE)
            {
                transform.Rotation += speed * delta.TotalSeconds;
            }
            if (remoteState.RemoteKeyboardState.KeyS && transform.Rotation > MIN_ANGLE)
            {
                transform.Rotation -= speed * delta.TotalSeconds;
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
