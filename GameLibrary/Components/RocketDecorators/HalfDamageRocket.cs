namespace GameLibrary.Components.RocketDecorators
{
    /// <summary>
    /// Декоратор компонента Rocket,
    /// который уменьшает урон от ракеты в 2 раза.
    /// </summary>
    public class HalfDamageRocket : RocketDecorator
    {
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


        /// <summary>
        /// Создание нового декоратора, который уменьшает урон в 2 раза.
        /// </summary>
        /// <param name="rocket">Декорируемый экземпляр.</param>
        public HalfDamageRocket(Rocket rocket) : base(rocket)
        {
        }
    }
}
