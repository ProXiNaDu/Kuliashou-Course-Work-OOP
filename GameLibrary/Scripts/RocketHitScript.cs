using GameEngineLibrary;
using GameLibrary.Components;
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
            
            if (CheckBounds())
            {
                scene.RemoveGameObject(controlledObject);
                return;
            }

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

                    if (gameObject.GetComponent("rocket") is Rocket)
                    {
                        scene.RemoveGameObject(gameObject);
                        return;
                    }

                    Rocket rocket = controlledObject.GetComponent("rocket") as Rocket;
                    Health health = gameObject.GetComponent("health") as Health;
                    if (health != null)
                    {
                        health.Damage(rocket.Damage);
                        if (!health.IsAlive())
                        {
                            scene.RemoveGameObject(gameObject);
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Проверка рокины на выход за границы экрана.
        /// </summary>
        /// <returns>True, если ракета за границами экрана.</returns>
        private bool CheckBounds()
        {
            Transform transform = controlledObject.GetComponent("transform") as Transform;
            return transform.Position.X >  scene.GameWindow.Width  ||
                   transform.Position.X < -scene.GameWindow.Width  ||
                   transform.Position.Y >  scene.GameWindow.Height ||
                   transform.Position.Y < -scene.GameWindow.Height;
        }
    }
}
