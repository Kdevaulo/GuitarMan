using System;

using UnityEditor;

namespace GuitarMan.MainMenuBehaviour
{
    [Serializable]
    public class HandlerByButtonType
    {
        public MonoScript BehaviourHandler;

        public ButtonType ButtonType;
    }
}