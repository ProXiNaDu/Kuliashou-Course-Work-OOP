using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace GameEngineLibrary
{
    /// <summary>
    /// Класс, который описывает двухмерную текстуру.
    /// </summary>
    public class Texture2D : IComponent, IDisposable
    {
        /// <summary>
        /// Переменная, отвечающая за состояние объекта.
        /// </summary>
        protected bool disposed = false;

        /// <summary>
        /// Идентификатор текстуры.
        /// </summary>
        public virtual int ID { get; private set; }
        /// <summary>
        /// Название текстуры.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ширина текстуры.
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// Высота текстуры.
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// Цвет, который будет наложен на текстуру.
        /// </summary>
        public Color Color { get; set; }

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
            Color = Color.Transparent;
        }

        /// <summary>
        /// Загрузка текстуры из файла.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Загруженная текстура.</returns>
        public static Texture2D LoadTexture(string path)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bitmap = new Bitmap(path);
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Nearest);

            return new Texture2D(id, bitmap.Width, bitmap.Height) { Name = path };
        }

        /// <summary>
        /// Сравнение двух объектов.
        /// </summary>
        /// <param name="obj">Объект для сравнения.</param>
        /// <returns>True, если объекты равны и false в противном случае.</returns>
        public override bool Equals(object obj)
        {
            return obj is Texture2D texture &&
                   ID == texture.ID &&
                   Width == texture.Width &&
                   Height == texture.Height;
        }

        /// <summary>
        /// Получение hash-кода объекта.
        /// </summary>
        /// <returns>Целое число.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1463928665;
            hashCode = hashCode * -1521134295 + ID.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Уничтожение текстуры.
        /// </summary>
        public virtual void Dispose()
        {
            if (disposed)
                return;

            GL.DeleteTexture(ID);
            disposed = true;
        }
    }
}
