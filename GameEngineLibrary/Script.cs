using System;

namespace GameEngineLibrary
{
    /// <summary>
    /// Интерфейс, описывающий скрипт, который определяет поведение игрового объекта.
    /// </summary>
    public abstract class Script
    {
        /// <summary>
        /// Объект, которым управляет скрипт.
        /// </summary>
        protected GameObject controlledObject;

        /// <summary>
        /// Метод, инициализирующий данные скрипта.
        /// </summary>
        public virtual void Init()
        { 
        }

        /// <summary>
        /// Метод, который содержит основную логику программы.
        /// Данный метод вызывается в каждом кадре игры.
        /// </summary>
        /// <param name="delta">Время, прошедшее между кадрами.</param>
        public abstract void Update(TimeSpan delta);

        /// <summary>
        /// Устанавливает объект, который будет контролировать скрипт.
        /// </summary>
        /// <param name="controlledObject">Объект, контролируемый скриптом.</param>
        public void SetControlledObject(GameObject controlledObject)
        {
            this.controlledObject = controlledObject;
        }
    }
}
