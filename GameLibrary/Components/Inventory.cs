using GameEngineLibrary;
using GameLibrary.Scripts;
using OpenTK;

namespace GameLibrary.Components
{
    /// <summary>
    /// Инвентарь, отвечающий за выдачу новых ракет,
    /// а также за слежку за количеством оставшихся ракет.
    /// </summary>
    public class Inventory : IComponent
    {
        /// <summary>
        /// Сборщики ракет, которые предназначены для создания новых ракет.
        /// </summary>
        private RocketBuilder[] rockets;

        /// <summary>
        /// Общее количество ракет в инвентаре.
        /// </summary>
        protected int totalAmount;

        /// <summary>
        /// Количества рокет в инвентаре.
        /// </summary>
        protected int[] amounts;

        /// <summary>
        /// Текущий индекс ракеты для создания.
        /// </summary>
        protected int current;

        /// <summary>
        /// Общее количество ракет в инвентаре.
        /// </summary>
        public int TotalAmount { get => totalAmount; set => totalAmount = value; }

        /// <summary>
        /// Количества рокет в инвентаре.
        /// </summary>
        public int[] Amounts { get => amounts; set => amounts = value; }

        /// <summary>
        /// Текущий индекс ракеты для создания.
        /// </summary>
        public int Current { get => current; set => current = value; }

        /// <summary>
        /// Создание нового инвентаря.
        /// </summary>
        /// <param name="rockets">Сборщики ракет.</param>
        public Inventory(params RocketBuilder[] rockets)
        {
            this.rockets = rockets;
            amounts = new int[rockets.Length];
            current = 0;
            totalAmount = 0;
            for (int i = 0; i < amounts.Length; i++)
            {
                amounts[i] = 0;
            }
        }

        /// <summary>
        /// Выбрать следующую ракету.
        /// </summary>
        public virtual void SelectNext()
        {
            current = (current < rockets.Length - 1) ? current + 1 : 0;
        }

        /// <summary>
        /// Выбрать предыдущую ракету.
        /// </summary>
        public virtual void SelectPrevious()
        {
            current = (current > 0 ? current : rockets.Length) - 1;
        }

        /// <summary>
        /// Установить количество выбранных ракет.
        /// </summary>
        /// <param name="amount">Новое количество выбранных ракет.</param>
        public virtual void SetAmount(int amount)
        {
            totalAmount -= amounts[current];
            amounts[current] = amount;
            totalAmount += amounts[current];
        }

        /// <summary>
        /// Получение количества текущих ракет.
        /// </summary>
        /// <returns>Количество текущих ракет.</returns>
        public virtual int GetAmount()
        {
            return amounts[current];
        }

        /// <summary>
        /// Получить выбранную ракету.
        /// </summary>
        /// <returns>Ракета из инвентаря.</returns>
        public virtual GameObject GetRocket()
        {
            if (amounts[current] > 0)
            {
                amounts[current]--;
                totalAmount--;
                return rockets[current].Build();
            }
            return null;
        }

        /// <summary>
        /// Получение общего количества ракет в инвентаре.
        /// </summary>
        /// <returns>Общее количество ракет в инвентаре.</returns>
        public virtual int GetTotalAmount()
        {
            return totalAmount;
        }

        /// <summary>
        /// Сборщик ракет.
        /// </summary>
        public class RocketBuilder
        {
            /// <summary>
            /// Сцена, в которой будут создаваться ракеты.
            /// </summary>
            private Scene scene;

            /// <summary>
            /// Компонент создаваемой ракеты.
            /// </summary>
            private Rocket rocket;

            /// <summary>
            /// Текстура создаваемой ракеты.
            /// </summary>
            private Texture2D rocketTex;

            /// <summary>
            /// Анимация взрыва создаваемой ракеты.
            /// </summary>
            private Animation2D explosionAnim;

            /// <summary>
            /// Создание объекта, отвечающего за сборку ракет по заданным правилам.
            /// </summary>
            /// <param name="scene">Сцена, в которой будут создаваться ракеты.</param>
            /// <param name="rocketTex">Текстура создаваемой ракеты.</param>
            /// <param name="explosionAnim">Анимация взрыва создаваемой ракеты.</param>
            /// <param name="rocket">Компонент создаваемой ракеты.</param>
            public RocketBuilder(Scene scene, Texture2D rocketTex, Animation2D explosionAnim, Rocket rocket)
            {
                this.scene = scene;
                this.rocketTex = rocketTex;
                this.explosionAnim = explosionAnim;
                this.rocket = rocket;
            }

            /// <summary>
            /// Создание ракеты.
            /// </summary>
            /// <returns>Созданная ракета.</returns>
            public GameObject Build()
            {
                GameObject rocket = new GameObject(rocketTex, Vector2.Zero,
                    new Vector2(rocketTex.Width / 2, rocketTex.Height / 2),
                    Vector2.One, 0);
                rocket.AddScript(new RocketHitScript(scene, explosionAnim));
                rocket.AddComponent("rocket", this.rocket);
                return rocket;
            }
        }
    }
}
