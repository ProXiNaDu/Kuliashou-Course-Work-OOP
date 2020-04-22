namespace GameLibrary.Components
{
    /// <summary>
    /// Абстрактный класс, описывающий декораторы ракеты.
    /// </summary>
    public class RocketDecorator : Rocket
    {
        /// <summary>
        /// Декорируемая ракета.
        /// </summary>
        protected Rocket rocket;

        /// <summary>
        /// Урон ракеты.
        /// </summary>
        public override int Damage => rocket.Damage;

        /// <summary>
        /// Время до следующего выстрела ракетой.
        /// </summary>
        public override int Cooldown => rocket.Cooldown;


        /// <summary>
        /// Создание декоратора ракеты.
        /// </summary>
        /// <param name="rocket">Декорируемая ракеты.</param>
        public RocketDecorator(Rocket rocket)
        {
            this.rocket = rocket;
        }
    }
}
