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
        private Scene scene;

        /// <summary>
        /// Создание скрипта, отвечающего за обрапотку попаданий ракеты.
        /// </summary>
        /// <param name="scene">Сцена, в которой будет проверяться столкновения.</param>
        public RocketHitScript(Scene scene)
        {
            this.scene = scene;
        }

        public override void Update(TimeSpan delta)
        {
            controlledObject.UpdateColliderToTexture();

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
