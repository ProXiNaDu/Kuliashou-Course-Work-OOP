namespace GameLibrary.Components
{
    /// <summary>
    /// Компонент, описывающий ракету со стандартными параметрами.
    /// Ракета наносит 20 урона и имеет скорострельность 500 мс.
    /// </summary>
    public class BaseRocket : Rocket
    {
        private int damage;
        private int cooldown;

        /// <summary>
        /// Урон ракеты.
        /// </summary>
        public override int Damage => damage;

        /// <summary>
        /// Время до следующего выстрела ракетой.
        /// </summary>
        public override int Cooldown => cooldown;


        /// <summary>
        /// Создание новой ракеты.
        /// </summary>
        public BaseRocket()
        {
            damage = 20;
            cooldown = 500;
        }
    }
}
