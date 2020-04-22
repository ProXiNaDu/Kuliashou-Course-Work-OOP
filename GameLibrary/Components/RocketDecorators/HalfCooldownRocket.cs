namespace GameLibrary.Components.RocketDecorators
{
    /// <summary>
    /// Декоратор компонента Rocket,
    /// который уменьшает время перезарядки в 2 раза.
    /// </summary>
    public class HalfCooldownRocket : RocketDecorator
    {
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
        public HalfCooldownRocket(Rocket rocket) : base(rocket)
        {
        }
    }
}
