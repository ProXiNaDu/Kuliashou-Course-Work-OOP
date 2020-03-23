using OpenTK;

namespace GameEngineLibrary
{
    /// <summary>
    /// Класс, описывающий выпуклую фигуру,
    /// которая способна совершать проверки на
    /// пересечение с другими выпуклыми фигурами.
    /// </summary>
    public class Collider
    {
        /// <summary>
        /// Массив вершин выпуклой фигры.
        /// </summary>
        private Vector2[] verteces;

        public Collider(params Vector2[] verteces)
        {
            this.verteces = verteces;
        }

        public bool CheckCollision(Collider collider)
        {
            int count = verteces.Length + collider.verteces.Length;

            Vector2[] allVertices = new Vector2[count];
            verteces.CopyTo(allVertices, 0);
            collider.verteces.CopyTo(allVertices, verteces.Length);

            Vector2 normal = new Vector2();

            for (int i = 0; i < count; i++)
            {
                normal = GetNormal(allVertices, i);

                // Находим проекции фигур на нормали сторон.
                // X - максимальная координата Y - минимальная.
                Vector2 firstProjection = GetProjection(normal);
                Vector2 secondProjection = collider.GetProjection(normal);

                // Если хотя бы на одной проекции фигуры не пересекаются, 
                // значит существует разделяющая ось, и фигуры вообще не 
                // пересекаются.
                if (firstProjection.X < secondProjection.Y ||
                    secondProjection.X < firstProjection.Y)
                {
                    return false;
                }
            }

            return true;
        }

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