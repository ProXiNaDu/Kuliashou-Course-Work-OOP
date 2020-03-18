using System;
using System.Collections.Generic;

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
        /// <param name="delta">Время, прошедшее между кадрами.</param>
        void Update(TimeSpan delta);

        /// <summary>
        /// Получение списка с объектами на сцене.
        /// </summary>
        /// <returns>Список объектов на сцене.</returns>
        List<GameObject> GetGameObjects();
    }
}
