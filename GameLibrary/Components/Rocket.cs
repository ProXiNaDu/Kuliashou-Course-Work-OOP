using GameEngineLibrary;

namespace GameLibrary.Components
{
    /// <summary>
    /// Абстрактный класс, описывающий ракеты в игре.
    /// </summary>
    public abstract class Rocket : IComponent
    {
        /// <summary>
        /// Урон ракеты.
        /// </summary>
        public abstract int Damage { get; }

        /// <summary>
        /// Время до следующего выстрела ракетой.
        /// </summary>
        public abstract int Cooldown { get; }
    }
}
