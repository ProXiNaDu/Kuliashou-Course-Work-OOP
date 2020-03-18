﻿namespace GameEngineLibrary
{
    /// <summary>
    /// Интерфейс, описывающий скрипт, который определяет поведение игрового объекта.
    /// </summary>
    public interface IScript
    {
        /// <summary>
        /// Метод, который вызывается 1 раз 
        /// при привязывании скрипта к объекту.
        /// </summary>
        void Start();

        /// <summary>
        /// Метод, который содержит основную логику программы.
        /// Данный метод вызывается в каждом кадре игры.
        /// </summary>
        void Update();
    }
}
