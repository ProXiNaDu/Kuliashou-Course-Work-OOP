using System.Runtime.Serialization;

namespace GameLibrary.Scripts.RemoteKeyboardControlScripts
{
    /// <summary>
    /// Объект для передачи состояния клавиатуры по сети
    /// </summary>
    [DataContract]
    public class RemoteKeyboardState
    {
        /// <summary>
        /// Кнопка Q
        /// </summary>
        [DataMember]
        public bool KeyQ { get; set; }

        /// <summary>
        /// Кнопка W
        /// </summary>
        [DataMember]
        public bool KeyW { get; set; }

        /// <summary>
        /// Кнопка E
        /// </summary>
        [DataMember]
        public bool KeyE { get; set; }

        /// <summary>
        /// Кнопка A
        /// </summary>
        [DataMember]
        public bool KeyA { get; set; }

        /// <summary>
        /// Кнопка S
        /// </summary>
        [DataMember]
        public bool KeyS { get; set; }

        /// <summary>
        /// Кнопка D
        /// </summary>
        [DataMember]
        public bool KeyD { get; set; }

        /// <summary>
        /// Кнопка Space
        /// </summary>
        [DataMember]
        public bool KeySpace { get; set; }
    }
}
