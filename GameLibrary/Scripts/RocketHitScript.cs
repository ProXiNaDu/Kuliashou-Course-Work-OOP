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
        /// <summary>
        /// Сцена, на которой обрабатываются попадания.
        /// </summary>
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
            Collider thisCollider = controlledObject.GetComponent("collider") as Collider;
            foreach (GameObject gameObject in objects)
            {
                Collider collider = gameObject.GetComponent("collider") as Collider;
                if (gameObject != controlledObject &&
                    collider != null &&
                    collider.CheckCollision(thisCollider))
                {
                    scene.RemoveGameObject(controlledObject);
                    scene.RemoveGameObject(gameObject);
                }
            }
        }
    }
}
