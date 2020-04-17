using System;
using System.Windows.Controls;
using GameEngineLibrary;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, отображающий состояние перезарядки.
    /// </summary>
    public class WpfShootControlScript : ShootKeyboardControlScript
    {
        /// <summary>
        /// Индикатор, отображающий состояние перезарядки.
        /// </summary>
        private ProgressBar cooldown;
        private ShootKeyboardControlScript shootControlScript;

        /// <summary>
        /// Создание нового скрипта, который отображает состояние перезарядки на окно WPF.
        /// </summary>
        /// <param name="scene">Сцена, в которой происходит стрельба.</param>
        /// <param name="cooldown">Индикатор перезарядки.</param>
        /// <param name="shootControlScript">Декорируемый экземпдяр.</param>
        public WpfShootControlScript(Scene scene, ProgressBar cooldown, ShootKeyboardControlScript shootControlScript) : base(scene)
        {
            this.cooldown = cooldown;
            this.shootControlScript = shootControlScript;
        }

        /// <summary>
        /// Инициализация скрипта.
        /// </summary>
        public override void Init()
        {
            shootControlScript.SetControlledObject(controlledObject);
            shootControlScript.Init();
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            shootControlScript.Update(delta);
            cooldown.Maximum = shootControlScript.Cooldown;
            cooldown.Value = shootControlScript.LastShoot;
        }
    }
}
