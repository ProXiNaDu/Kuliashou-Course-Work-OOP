using GameEngineLibrary;

namespace GameLibrary.Components
{
    /// <summary>
    /// Компонент, описывающий ракету со стандартными параметрами.
    /// Ракета наносит 20 урона и имеет скорострельность 500 мс.
    /// </summary>
    public class Rocket : IComponent
    {
        /// <summary>
        /// Урон ракеты.
        /// </summary>
        public virtual int Damage { get; }

        /// <summary>
        /// Время до следующего выстрела ракетой.
        /// </summary>
        public virtual int Cooldown { get; }

        /// <summary>
        /// Создание новой ракеты.
        /// </summary>
        public Rocket()
        {
            Damage = 20;
            Cooldown = 500;
        }
    }
}
