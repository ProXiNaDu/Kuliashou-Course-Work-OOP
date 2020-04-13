namespace GameLibrary.Components.RocketDecorators
{
    /// <summary>
    /// Декоратор компонента Rocket,
    /// который уменьшает время перезарядки в 2 раза.
    /// </summary>
    public class HalfCooldownRocket : Rocket
    {
        /// <summary>
        /// Декорируемый экземпляр.
        /// </summary>
        private Rocket rocket;

        /// <summary>
        /// Урон ракеты.
        /// </summary>
        public override int Damage => rocket.Damage;

        /// <summary>
        /// Перезарядка, уменьшенная в 2 раза.
        /// </summary>
        public override int Cooldown
        {
            get
            {
                return rocket.Cooldown / 2;
            }
        }

        /// <summary>
        /// Создание нового декоратора, который уменьшает время перезарядки в 2 раза.
        /// </summary>
        /// <param name="rocket">Декорируемый экземпляр.</param>
        public HalfCooldownRocket(Rocket rocket)
        {
            this.rocket = rocket;
        }
    }
}
