using GameEngineLibrary;
using GameLibrary.Components;
using OpenTK.Input;
using System;

namespace GameLibrary.Scripts
{
    /// <summary>
    /// Скрипт для управления переключением ракет при помощи клавиатуры.
    /// </summary>
    public class KeyboardRocketSwitcherScript : Script
    {
        private Key next;
        private Key previous;
        private bool isCooldown = false;
        private int lastPressTime = 0;
        private const int COOLDOWN = 100;

        /// <summary>
        /// Обновление состояния скрипта.
        /// </summary>
        /// <param name="delta">Время, прошедшее с предыдущего кадра.</param>
        public override void Update(TimeSpan delta)
        {
            if (isCooldown)
            {
                lastPressTime += delta.Milliseconds;
                if (lastPressTime > COOLDOWN)
                {
                    isCooldown = false;
                }
                return;
            }

            KeyboardState keyboard = Keyboard.GetState();
            Inventory inventory = controlledObject.GetComponent("inventory") as Inventory;

            if (keyboard[next])
            {
                inventory.SelectNext();
                lastPressTime = 0;
                isCooldown = true;
                return;
            }
            if (keyboard[previous])
            {
                inventory.SelectPrevious();
                lastPressTime = 0;
                isCooldown = true;
                return;
            }
        }

        /// <summary>
        /// Установить кнопку для выбора следующей ракеты.
        /// </summary>
        /// <param name="key">Кнопка на клавиатуре.</param>
        public void SetKeyToSelectNext(Key key)
        {
            next = key;
        }

        /// <summary>
        /// Установить кнопку для выбора предыдущей ракеты.
        /// </summary>
        /// <param name="key">Кнопка на клавиатуре.</param>
        public void SetKeyToSelectPrevious(Key key)
        {
            previous = key;
        }
    }
}
