using GameEngineLibrary;

namespace GameLibrary.Components
{
    /// <summary>
    /// Компонент, который отвечает за здоровье у объекта.
    /// </summary>
    public class Health : IComponent
    {
        /// <summary>
        /// Количество здоровья у объекта.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Создание компонента здоровья.
        /// </summary>
        public Health()
        {
            Value = 100;
        }

        /// <summary>
        /// Создание компонента здоровья.
        /// </summary>
        /// <param name="health">Начальное количество здоровья.</param>
        public Health(int health)
        {
            Value = health;
        }

        /// <summary>
        /// Нанесение повреждения объекту.
        /// </summary>
        /// <param name="damage">Количество повреждений, нанесенных объекту.</param>
        public virtual void Damage(int damage)
        {
            Value -= damage;
        }

        /// <summary>
        /// Увеличение здоровья объекта на заданное число.
        /// </summary>
        /// <param name="health">Количество исцеленного здоровья.</param>
        public virtual void Heal(int health)
        {
            this.Value += health;
        }

        /// <summary>
        /// Проверка, жив ли объект.
        /// </summary>
        /// <returns>True, если объект ещё жив.</returns>
        public virtual bool IsAlive()
        {
            return Value > 0;
        }
    }
}
