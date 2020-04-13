using GameEngineLibrary;
using OpenTK;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Класс, описывающий физику объекта,
    /// на который действуют различные силы.
    /// </summary>
    public class PhysicScript : Script
    {
        /// <summary>
        /// Импульс объекта.
        /// </summary>
        private Vector2 impulse;

        /// <summary>
        /// Силы, действующие на объект.
        /// </summary>
        private Vector2[] forces;

        /// <summary>
        /// Создание нового объекта, на которого действуют силы.
        /// </summary>
        /// <param name="impulse">Начальный импульс объекта.</param>
        /// <param name="forces">Силы, действующие на объект.</param>
        public PhysicScript(Vector2 impulse, params Vector2[] forces)
        {
            this.impulse = impulse;
            this.forces = (Vector2[])forces.Clone();
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            Transform transform = controlledObject.GetComponent("transform") as Transform;
            transform.Position += impulse;
            transform.Rotation = Math.Sign(transform.Scale.X) *
                                        Math.Atan(impulse.Y / impulse.X);

            foreach (Vector2 force in forces)
            {
                impulse += force;
            }
        }
    }
}
