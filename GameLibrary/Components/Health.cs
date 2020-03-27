using GameEngineLibrary;

namespace GameLibrary.Components
{
    public class Health : IComponent
    {
        private int health;

        public Health()
        {
            health = 100;
        }

        public void Damage(int damage)
        {
            health -= damage;
        }

        public void Heal(int health)
        {
            this.health += health;
        }

        public bool IsAlive()
        {
            return health > 0;
        }
    }
}
