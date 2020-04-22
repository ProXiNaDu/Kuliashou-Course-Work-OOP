namespace GameLibrary.Components.RocketDecorators
{
    /// <summary>
    /// Декоратор компонента Rocket,
    /// который увеличивает время перезарядки в 2 раза.
    /// </summary>
    public class DoubleCooldownRocket : RocketDecorator
    {
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
        public DoubleCooldownRocket(Rocket rocket) : base(rocket)
        {
        }
    }
}
