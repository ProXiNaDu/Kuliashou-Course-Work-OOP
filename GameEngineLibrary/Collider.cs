using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngineLibrary
{
    /// <summary>
    /// Класс, описывающий выпуклую фигуру,
    /// которая способна совершать проверки на
    /// пересечение с другими выпуклыми фигурами.
    /// </summary>
    public class Collider : IComponent
    {
        /// <summary>
        /// Массив вершин выпуклой фигры.
        /// </summary>
        private Vector2[] verteces;

        /// <summary>
        /// Создание коллайдера.
        /// </summary>
        /// <param name="verteces">Вершины выпуклой фигуры, расположенные против часовой стрелки.</param>
        public Collider(params Vector2[] verteces)
        {
            this.verteces = verteces;
        }

        /// <summary>
        /// Проверка пересечения между двумя коллайдерами.
        /// </summary>
        /// <param name="collider">Коллайдер, с которым будет осуществляться проверка.</param>
        /// <returns>True, если есть пересечение, иначе false.</returns>
        public bool CheckCollision(Collider collider)
        {
            int count = verteces.Length + collider.verteces.Length;

            Vector2[] allVertices = new Vector2[count];
            verteces.CopyTo(allVertices, 0);
            collider.verteces.CopyTo(allVertices, verteces.Length);

            Vector2 normal;

            for (int i = 0; i < count; i++)
            {
                normal = GetNormal(allVertices, i);

                Vector2 firstProjection = GetProjection(normal);
                Vector2 secondProjection = collider.GetProjection(normal);

                if (firstProjection.X < secondProjection.Y ||
                    secondProjection.X < firstProjection.Y)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Получение нормального вектора к заданной стороне фигуры.
        /// </summary>
        /// <param name="verteces">Вершины фигуры.</param>
        /// <param name="num">Номер стороны.</param>
        /// <returns></returns>
        private Vector2 GetNormal(Vector2[] verteces, int num)
        {
            int next = num + 1;
            next = next == verteces.Length ? 0 : next;

            Vector2 firstPoint = verteces[num];
            Vector2 secondPoint = verteces[next];

            Vector2 edge = new Vector2(
                secondPoint.X - firstPoint.X,
                secondPoint.Y - firstPoint.Y);

            return new Vector2(-edge.Y, edge.X);
        }

        /// <summary>
        /// Получение вектора проекции фигуры на плоскость.
        /// </summary>
        /// <param name="vector">Вектор плоскости.</param>
        /// <returns>Вектор проекции.</returns>
        private Vector2 GetProjection(Vector2 vector)
        {
            Vector2 result = new Vector2();
            bool isNull = true;

            foreach (Vector2 current in verteces)
            {
                float projection = vector.X * current.X +
                                   vector.Y * current.Y;

                if (isNull)
                {
                    result = new Vector2(projection, projection);
                    isNull = false;
                }

                if (projection > result.X)
                {
                    result.X = projection;
                }
                if (projection < result.Y)
                {
                    result.Y = projection;
                }
            }

            return result;
        }
    }
}