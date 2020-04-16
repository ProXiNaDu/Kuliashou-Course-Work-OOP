using System;

namespace GameEngineLibrary
{
    /// <summary>
    /// Класс, отвечающий за случайные числа в игре.
    /// </summary>
    public static class RandomManager
    {
        private static Random random = new Random();

        /// <summary>
        /// Возвращает неотрицательное случайное целое число.
        /// </summary>
        /// <returns>Неотрицательное случайное целое число</returns>
        public static int Next()
        {
            return random.Next();
        }

        /// <summary>
        /// Возвращает неотрицательное случайное целое число, 
        /// которое меньше определенного максимального значения.
        /// </summary>
        /// <param name="maxValue">Максимальное значение.</param>
        /// <returns>Неотрицательное случайное целое число</returns>
        public static int Next(int maxValue)
        {
            return random.Next(maxValue);
        }

        /// <summary>
        /// Возвращает неотрицательное случайное целое число 
        /// в указанном диапазоне.
        /// </summary>
        /// <param name="minValue">Минимальное значение.</param>
        /// <param name="maxValue">Максимальное значение.</param>
        /// <returns>Неотрицательное случайное целое число</returns>
        public static int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Возвращает случайное число с плавающей точкой, 
        /// которое больше или равно 0,0 и меньше 1,0.
        /// </summary>
        /// <returns></returns>
        public static double NextDouble()
        {
            return random.NextDouble();
        }
    }
}
