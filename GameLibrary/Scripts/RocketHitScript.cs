using GameEngineLibrary;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Класс, отвечающий за проверку поведение 
    /// при попадании ракеты по цели.
    /// </summary>
    public class RocketHitScript : Script
    {
        private IScene scene;

        /// <summary>
        /// Создание скрипта, отвечающего за обрапотку попаданий ракеты.
        /// </summary>
        /// <param name="scene">Сцена, в которой будет проверяться столкновения.</param>
        public RocketHitScript(IScene scene)
        {
            this.scene = scene;
        }

        public override void Update(TimeSpan delta)
        {
            GameObject[] objects = scene.GetGameObjects().ToArray();
            foreach (GameObject gameObject in objects)
            {
                if (gameObject != controlledObject &&
                    gameObject.Collider != null &&
                    gameObject.Collider.CheckCollision(controlledObject.Collider))
                {
                    //
                }
            }
        }
    }
}
