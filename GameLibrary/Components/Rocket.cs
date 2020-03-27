using GameEngineLibrary;

namespace GameLibrary.Components
{
    public class Rocket : IComponent
    {
        public int Damage { get; }

        public Rocket()
        {
            Damage = 20;
        }
    }
}
