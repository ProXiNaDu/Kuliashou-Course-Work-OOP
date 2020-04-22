namespace GameLibrary.Components.RocketDecorators
{
    /// <summary>
    /// Декоратор компонента Rocket,
    /// который увеличивает урон от ракеты в 2 раза.
    /// </summary>
    public class DoubleDamageRocket : RocketDecorator
    {
        /// <summary>
        /// Урон, увеличенный в 2 раза.
        /// </summary>
        public override int Damage
        {
            get
            {
                return rocket.Damage * 2;
            }
        }


        /// <summary>
        /// Создание нового декоратора, который увеичивает урон в 2 раза.
        /// </summary>
        /// <param name="rocket">Декорируемый экземпляр.</param>
        public DoubleDamageRocket(Rocket rocket) : base(rocket)
        {
        }
    }
}
