namespace GameLibrary.Scripts.RemoteKeyboardControlScripts
{
    /// <summary>
    /// Состояние удаленного устройства
    /// </summary>
    public class RemoteState
    {
        /// <summary>
        /// Состояние удаленной клавиатуры
        /// </summary>
        public RemoteKeyboardState RemoteKeyboardState { get; set; } = new RemoteKeyboardState();
    }
}
