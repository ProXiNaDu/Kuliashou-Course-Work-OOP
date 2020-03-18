using System;

namespace GameEngineLibrary
{
    /// <summary>
    /// Интерфейс, описывающий пользовательскую сцену.
    /// </summary>
    public interface IScene : IDisposable
    {
        /// <summary>
        /// Метод, который вызывается 1 раз для инициализации сцены.
        /// </summary>
        void Init();

        /// <summary>
        /// Метод, который вызывается перед отрисовкой кадра.
        /// В этом методе параметры сцены должны обновляться.
        /// </summary>
        void Update();
    }
}
