using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GameEngineLibrary;

namespace GameUserInterface
{
    /// <summary>
    /// Класс, отвечающий за отрисовку объектов на сцене.
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// Список объектов для отрисовки.
        /// </summary>
        private List<GameObject> objectsToRender;

        /// <summary>
        /// Создание нового объекта для отрисовки объектов на сцене.
        /// </summary>
        public Renderer()
        {
            objectsToRender = new List<GameObject>();
        }

        /// <summary>
        /// Отрисовать все объекты.
        /// </summary>
        public void Render()
        {
            Render(objectsToRender);
        }

        /// <summary>
        /// Отрисовать объекты из переданного массива.
        /// </summary>
        /// <param name="objectsToRender">Массив объектов для отрисовки.</param>
        private void Render(List<GameObject> objectsToRender)
        {
            foreach (GameObject gameObject in objectsToRender)
            {
                RenderObject(gameObject);
                Render(gameObject.InnerObjects);
            }
        }

        /// <summary>
        /// Отрисовать объект на сцене.
        /// </summary>
        /// <param name="gameObject">Объект для отрисовки.</param>
        private void RenderObject(GameObject gameObject)
        {
            Texture2D texture = gameObject.Texture;
            Vector2 rotationPoint = gameObject.RotationPoint;
            double rotation = gameObject.Rotation;
            Vector2[] vertices = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };

            GL.BindTexture(TextureTarget.Texture2D, texture.ID);
            GL.Begin(PrimitiveType.Quads);

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
                vertices[i] *= gameObject.Scale;
                vertices[i] += gameObject.Position;

                GL.Vertex2(vertices[i]);
            }

            GL.End();
        }

        /// <summary>
        /// Добавить объект для отрисовки.
        /// </summary>
        /// <param name="gameObject">Объект для отрисовки.</param>
        public void AddObjectToRender(GameObject gameObject)
        {
            objectsToRender.Add(gameObject);
        }

        /// <summary>
        /// Удалить объект для отрисовки.
        /// </summary>
        /// <param name="gameObject">Объект для отрисовки.</param>
        public void RemoveObjectToRender(GameObject gameObject)
        {
            objectsToRender.Remove(gameObject);
        }
    }
}
