﻿namespace GameLibrary.Components.RocketDecorators
{
    /// <summary>
    /// Декоратор компонента Rocket,
    /// который увеличивает урон от ракеты в 2 раза.
    /// </summary>
    public class DoubleDamageRocket : Rocket
    {
        /// <summary>
        /// Декорируемый экземпляр.
        /// </summary>
        private Rocket rocket;

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
        /// Время до следующего выстрела ракетой.
        /// </summary>
        public override int Cooldown => rocket.Cooldown;

        /// <summary>
        /// Создание нового декоратора, который увеичивает урон в 2 раза.
        /// </summary>
        /// <param name="rocket">Декорируемый экземпляр.</param>
        public DoubleDamageRocket(Rocket rocket)
        {
            this.rocket = rocket;
        }
    }
}
