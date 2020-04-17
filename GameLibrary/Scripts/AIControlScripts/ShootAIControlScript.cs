using GameEngineLibrary;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, управляющий выстрелами танка с искусственным интеллектом.
    /// </summary>
    public class ShootAIControlScript : ShootKeyboardControlScript
    {
        private int range;

        /// <summary>
        /// Создание скрипта.
        /// </summary>
        /// <param name="scene">Сцена, в которой будут спавниться ракеты.</param>
        /// <param name="range">Время между выстрелами.</param>
        public ShootAIControlScript(Scene scene, int range)
            : base(scene)
        {
            this.range = range;
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            if (isCooldown)
            {
                UpdateCooldown(delta);
                return;
            }
            Shoot();
            Cooldown += RandomManager.Next(range);
        }
    }
}
