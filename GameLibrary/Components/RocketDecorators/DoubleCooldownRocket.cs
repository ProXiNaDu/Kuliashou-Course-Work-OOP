namespace GameLibrary.Components.RocketDecorators
{
    /// <summary>
    /// Декоратор компонента Rocket,
    /// который увеличивает время перезарядки в 2 раза.
    /// </summary>
    public class DoubleCooldownRocket : Rocket
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
        /// Перезарядка, увеличенный в 2 раза.
        /// </summary>
        public override int Cooldown
        {
            get
            {
                return rocket.Cooldown * 2;
            }
        }

        /// <summary>
        /// Создание нового декоратора, который увеичивает время перезарядки в 2 раза.
        /// </summary>
        /// <param name="rocket">Декорируемый экземпляр.</param>
        public DoubleCooldownRocket(Rocket rocket)
        {
            this.rocket = rocket;
        }
    }
}
