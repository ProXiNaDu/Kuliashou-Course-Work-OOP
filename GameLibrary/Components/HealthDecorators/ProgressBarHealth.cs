using System.Windows.Controls;

namespace GameLibrary.Components.HealthDecorators
{
    /// <summary>
    /// Декоратор, который привязывает ProgressBar к здоровью
    /// </summary>
    public class ProgressBarHealth : Health
    {
        /// <summary>
        /// Декорируемый объект.
        /// </summary>
        private Health health;

        /// <summary>
        /// Привязанный ProgressBar.
        /// </summary>
        private ProgressBar bar;

        /// <summary>
        /// Создание декоратора, привязывающего здоровье к ProgressBar.
        /// </summary>
        /// <param name="health">Декорируемый объект.</param>
        /// <param name="bar">ProgressBar для привязки.</param>
        public ProgressBarHealth(Health health, ProgressBar bar)
        {
            this.health = health;
            this.bar = bar;
        }

        public override void Damage(int damage)
        {
            health.Damage(damage);
            bar.Value = health.Value;
        }

        public override void Heal(int health)
        {
            this.health.Heal(health);
            bar.Value = this.health.Value;
        }

        public override bool IsAlive()
        {
            return health.IsAlive();
        }
    }
}
