﻿using System;
using System.Windows.Controls;
using GameEngineLibrary;

namespace GameLibrary.Scripts
{
    public class WpfShootControlScript : ShootKeyboardControlScript
    {
        /// <summary>
        /// Индикатор, отображающий состояние перезарядки.
        /// </summary>
        ProgressBar cooldown;

        /// <summary>
        /// Создание нового скрипта, который отображает состояние перезарядки на окно WPF.
        /// </summary>
        /// <param name="scene">Сцена, в которой происходит стрельба.</param>
        /// <param name="cooldown">Индикатор перезарядки.</param>
        public WpfShootControlScript(Scene scene, ProgressBar cooldown) : base(scene)
        {
            this.cooldown = cooldown;
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
            cooldown.Maximum = Cooldown;
            cooldown.Value = LastShoot;
        }
    }
}
