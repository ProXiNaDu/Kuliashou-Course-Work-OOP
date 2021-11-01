using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace GameEngineLibrary
{
    /// <summary>
    /// Класс, отвечающий за анимированные 2d изображения.
    /// </summary>
    public class Animation2D : Texture2D
    {
        /// <summary>
        /// Массив идентификаторов текстур анимации.
        /// </summary>
        private int[] animationId;

        /// <summary>
        /// Индекс текущей текстуры.
        /// </summary>
        private int index;

        /// <summary>
        /// Время, прошедшее с начала анимации.
        /// </summary>
        private int currentTime;

        /// <summary>
        /// Уникальный идентификатор текстуры,
        /// которая должна отображаться в текущий
        /// момент анимации.
        /// </summary>
        public override int ID
        {
            get
            {
                if (animationId.Length <= index) return animationId[0];
                return animationId[index];
            }
        }

        /// <summary>
        /// Индекс текущей текстуры.
        /// </summary>
        public int Index { get => index; set => index = value; }

        /// <summary>
        /// Время анимации.
        /// </summary>
        public int AnimationTime { get; set; }

        /// <summary>
        /// Создание анимации.
        /// </summary>
        /// <param name="animationId">Массив идентификаторов изображений в анимации.</param>
        /// <param name="width">Размер одного изображения.</param>
        /// <param name="height">Высота одного изображения.</param>
        public Animation2D(int[] animationId, int width, int height) 
            : base(animationId[0], width, height)
        {
            index = 0;
            AnimationTime = 1;
            currentTime = 0;
            this.animationId = animationId;
        }

        /// <summary>
        /// Конструктор копирования анимации.
        /// Его стоит использовать для передачи анимаций объектам,
        /// чтобы не захватывать лишние неуправляемые ресурсы.
        /// </summary>
        /// <param name="animation"></param>
        public Animation2D(Animation2D animation) 
            : this(animation.animationId, animation.Width, animation.Height)
        {
            disposed = true;
            AnimationTime = animation.AnimationTime;
            Name = animation.Name;
        }

        /// <summary>
        /// Обновить состояние анимации.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public void Update(TimeSpan delta)
        {
            currentTime += delta.Milliseconds;
            if (currentTime >= AnimationTime)
            {
                currentTime = 0;
            }

            double deltaTime = (double)currentTime / AnimationTime;
            index = (int)(animationId.Length * deltaTime);
        }

        /// <summary>
        /// Загрузить анимацию из файла.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="columns">Количество колонок со спрайтами.</param>
        /// <param name="rows">Количество рядов со спрайтами.</param>
        /// <returns>Новая анимация.</returns>
        public static Animation2D LoadAnimation(string path, int columns = 1, int rows = 1)
        {
            int length = columns * rows;
            int[] idArr = new int[length];

            Bitmap bitmap = new Bitmap(path);

            int spriteWidth = bitmap.Width / columns;
            int spriteHeight = bitmap.Height / rows;

            Rectangle rectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);

            for (int i = 0; i < length; i++)
            {
                rectangle.X = spriteWidth * (i % columns);
                rectangle.Y = spriteHeight *(i / columns);

                int id = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, id);

                Bitmap sprite = bitmap.Clone(rectangle,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                BitmapData data = sprite.LockBits(
                new Rectangle(0, 0, sprite.Width, sprite.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0,
                    PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte, data.Scan0);

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

                sprite.UnlockBits(data);

                idArr[i] = id;
            }

            return new Animation2D(idArr, spriteWidth, spriteHeight) { Name = path };
        }

        /// <summary>
        /// Уничтожение анимации.
        /// </summary>
        public override void Dispose()
        {
            if (disposed)
                return;
            
            foreach (int id in animationId)
            {
                GL.DeleteTexture(id);
            }
            disposed = true;
        }
    }
}
