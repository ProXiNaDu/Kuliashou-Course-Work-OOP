namespace GameEngineLibrary
{
    /// <summary>
    /// Класс, который описывает двухмерную текстуру.
    /// </summary>
    public class Texture2D
    {
        /// <summary>
        /// Идентификатор текстуры.
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// Ширина текстуры.
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Высота текстуры.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Создание новой текстуры.
        /// </summary>
        /// <param name="id">Идентификатор текстуры.</param>
        /// <param name="width">Ширина текстуры.</param>
        /// <param name="height">Высота текстуры.</param>
        public Texture2D(int id, int width, int height)
        {
            ID = id;
            Width = width;
            Height = height;
        }
    }
}
