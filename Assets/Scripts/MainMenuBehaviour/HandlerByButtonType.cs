using System;

using UnityEngine;

namespace GuitarMan.MainMenuBehaviour
{
    [Serializable]
    public class HandlerByButtonType
    {
        public ScriptableObject BehaviourHandler;

        public ButtonType ButtonType;
    }
}