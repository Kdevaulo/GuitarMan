using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GuitarMan.SoundMenuBehaviour.ChooseGameplaySystem
{
    [AddComponentMenu(nameof(ChooseGameplaySystem) + "/" + nameof(GamePlayModeView))]
    public class GamePlayModeView : MonoBehaviour, IDisposable
    {
        public event Action<Dropdown.OptionData> ValueChanged = delegate { };

        [SerializeField] private Dropdown _dropdown;

        void IDisposable.Dispose()
        {
            _dropdown.onValueChanged.RemoveListener(HandleDropdownValueChanged);
        }

        public void Initialize(List<Dropdown.OptionData> data)
        {
            _dropdown.ClearOptions();
            _dropdown.AddOptions(data);
            _dropdown.onValueChanged.AddListener(HandleDropdownValueChanged);
        }

        private void HandleDropdownValueChanged(int index)
        {
            ValueChanged.Invoke(_dropdown.options[index]);
        }
    }
}