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
            bar.Value = health.Value;
        }

        /// <summary>
        /// Нанесение повреждения объекту.
        /// </summary>
        /// <param name="damage">Количество повреждений, нанесенных объекту.</param>
        public override void Damage(int damage)
        {
            health.Damage(damage);
            bar.Value = health.Value;
        }

        /// <summary>
        /// Увеличение здоровья объекта на заданное число.
        /// </summary>
        /// <param name="health">Количество исцеленного здоровья.</param>
        public override void Heal(int health)
        {
            this.health.Heal(health);
            bar.Value = this.health.Value;
        }

        /// <summary>
        /// Проверка, жив ли объект.
        /// </summary>
        /// <returns>True, если объект ещё жив.</returns>
        public override bool IsAlive()
        {
            return health.IsAlive();
        }
    }
}
