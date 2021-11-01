using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngineLibrary
{
    /// <summary>
    /// Класс, отвечающий за отрисовку объектов на сцене.
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// Сцена для отрисовки.
        /// </summary>
        private Scene scene;

        /// <summary>
        /// Создание нового объекта для отрисовки объектов на сцене.
        /// </summary>
        /// <param name="scene">Сцена, которую будет рендерить объект.</param>
        public Renderer(Scene scene)
        {
            this.scene = scene;
        }

        /// <summary>
        /// Отрисовать объекты на сцене.
        /// </summary>
        public void Render()
        {
            Render(scene.GetGameObjects());
        }

        /// <summary>
        /// Отрисовать объекты из переданного массива.
        /// </summary>
        /// <param name="objectsToRender">Массив объектов для отрисовки.</param>
        private void Render(List<GameObject> objectsToRender)
        {
            foreach (GameObject gameObject in objectsToRender)
            {
                Render(gameObject.InnerObjects);
                RenderObject(gameObject);
            }
        }

        /// <summary>
        /// Отрисовать объект на сцене.
        /// </summary>
        /// <param name="gameObject">Объект для отрисовки.</param>
        private void RenderObject(GameObject gameObject)
        {
            Texture2D texture = gameObject.GetComponent("texture") as Texture2D;
            Transform transform = gameObject.GetComponent("transform") as Transform;
            if (texture == null || transform == null)
            {
                return;
            }

            Vector2 rotationPoint = transform.RotationPoint;
            Vector2 position = transform.Position;
            double rotation = transform.Rotation;
            Vector2[] vertices = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };

            int id = texture.ID;
            if (id == 0)
            {
                var sourceAnimation = texture as Animation2D;
                if (sourceAnimation != null)
                {
                    var savedAnimation = scene.GetTexture(texture.Name) as Animation2D;
                    savedAnimation.Index = sourceAnimation.Index;
                    id = savedAnimation.ID;
                }
                else
                {
                    id = scene.GetTexture(texture.Name).ID;
                }
            }

            GL.BindTexture(TextureTarget.Texture2D, id);
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(texture.Color);

            for (int i = 0; i < 4; i++)
            {
                GL.TexCoord2(vertices[i]);

                vertices[i].X *= texture.Width;
                vertices[i].Y *= texture.Height;
                vertices[i].X -= rotationPoint.X;
                vertices[i].Y -= rotationPoint.Y;
                vertices[i] = new Vector2(
                    (float)(Math.Cos(rotation) * vertices[i].X -
                            Math.Sin(rotation) * vertices[i].Y),
                    (float)(Math.Sin(rotation) * vertices[i].X + 
                            Math.Cos(rotation) * vertices[i].Y));
                vertices[i].X += rotationPoint.X;
                vertices[i].Y += rotationPoint.Y;
                vertices[i] *= transform.Scale;
                vertices[i] += position;

                GL.Vertex2(vertices[i]);
            }

            GL.End();
        }

        /// <summary>
        /// Устанавливает сцену для отрисовки.
        /// </summary>
        /// <param name="scene">Новая сцена для отрисовки.</param>
        public void SetSceneToRender(Scene scene)
        {
            this.scene = scene;
        }
    }
}
