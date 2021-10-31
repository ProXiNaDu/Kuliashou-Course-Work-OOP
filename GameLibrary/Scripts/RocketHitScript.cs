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
        /// Анимация взрыва ракеты.
        /// </summary>
        private Animation2D explosionAnim;

        private double windowHeight;
        private double windowWidth;

        /// <summary>
        /// Создание скрипта, отвечающего за обрапотку попаданий ракеты.
        /// </summary>
        /// <param name="scene">Сцена, в которой будет проверяться столкновения.</param>
        /// <param name="explosionAnim">Анимация взрыва.</param>
        public RocketHitScript(Scene scene, Animation2D explosionAnim)
        {
            this.scene = scene;
            this.explosionAnim = explosionAnim;

            windowWidth = (scene.GameWindow != null) ? scene.GameWindow.Width : 800;
            windowHeight = (scene.GameWindow != null) ? scene.GameWindow.Height : 450;
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            controlledObject.UpdateColliderToTexture();
            Transform transform = controlledObject.GetComponent("transform") as Transform;
            if (CheckBounds(transform))
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
                    GameObject explosion = new GameObject(new Animation2D(explosionAnim), 
                        new OpenTK.Vector2(transform.Position.X + transform.RotationPoint.X *
                                           transform.Scale.X - explosionAnim.Width / 2 * transform.Scale.X,
                                           transform.Position.Y + transform.RotationPoint.Y *
                                           transform.Scale.Y - explosionAnim.Height / 2 * transform.Scale.Y), 
                        new OpenTK.Vector2(explosionAnim.Width / 2, explosionAnim.Height / 2),
                        transform.Scale, 0);

                    explosion.AddScript(new AutoDestroyScript(scene, explosionAnim.AnimationTime));
                    scene.AddGameObject(explosion);
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
        /// <param name="transform">Проверяемая позиция.</param>
        /// <returns>True, если ракета за границами экрана.</returns>
        private bool CheckBounds(Transform transform)
        {
            return transform.Position.X > windowWidth ||
                   transform.Position.X < -windowWidth ||
                   transform.Position.Y > windowHeight ||
                   transform.Position.Y < -windowHeight;
        }
    }
}
