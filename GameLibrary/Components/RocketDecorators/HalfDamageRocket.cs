namespace GameLibrary.Components.RocketDecorators
{
    /// <summary>
    /// Декоратор компонента Rocket,
    /// который уменьшает урон от ракеты в 2 раза.
    /// </summary>
    public class HalfDamageRocket : Rocket
    {
        /// <summary>
        /// Декорируемый экземпляр.
        /// </summary>
        private Rocket rocket;

        /// <summary>
        /// Урон, уменьшенный в 2 раза.
        /// </summary>
        public override int Damage
        {
            get
            {
                return rocket.Damage / 2;
            }
        }

        public override int Cooldown => rocket.Cooldown;

        /// <summary>
        /// Создание нового декоратора, который уменьшает урон в 2 раза.
        /// </summary>
        /// <param name="rocket">Декорируемый экземпляр.</param>
        public HalfDamageRocket(Rocket rocket)
        {
            this.rocket = rocket;
        }
    }
}
