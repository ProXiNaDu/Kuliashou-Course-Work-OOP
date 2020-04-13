using System;
using GameEngineLibrary;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт, который отвечает за автоматическое уничтожение объекта через определенный промежуток времени.
    /// </summary>
    public class AutoDestroyScript : Script
    {
        /// <summary>
        /// Сцена, из которой будет удален объект.
        /// </summary>
        private Scene scene;

        /// <summary>
        /// Время с создания объекта в миллисекундах.
        /// </summary>
        private int currentTime;

        /// <summary>
        /// Время уничтожения объекта.
        /// </summary>
        private int destroyTime;

        /// <summary>
        /// Создание скрипта для автоудаления объекта через определенный промежуток времени.
        /// </summary>
        /// <param name="scene">Сцена из которой будет удален объект.</param>
        /// <param name="milliseconds">Количество миллисекунд, через которое будет удален объект.</param>
        public AutoDestroyScript(Scene scene, int milliseconds)
        {
            this.scene = scene;
            destroyTime = milliseconds;
        }

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            currentTime += delta.Milliseconds;
            if (currentTime >= destroyTime)
            {
                scene.RemoveGameObject(controlledObject);
            }
        }
    }
}
