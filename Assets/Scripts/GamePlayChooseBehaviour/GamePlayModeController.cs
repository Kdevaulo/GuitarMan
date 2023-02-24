using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GuitarMan.GamePlayChooseBehaviour
{
    public class GamePlayModeController
    {
        private readonly GamePlayModeView _gamePlayModeView;

        private readonly List<Dropdown.OptionData> _optionData = new List<Dropdown.OptionData>
        {
            // todo: move to static readonly field in Constants class
            // create enum?
            new Dropdown.OptionData("Standart"),
            new Dropdown.OptionData("Without Keys"),
            new Dropdown.OptionData("Keys Only"),
        };

        public GamePlayModeController(GamePlayModeView gamePlayModeView)
        {
            _gamePlayModeView = gamePlayModeView;

            Initialize();
        }

        private void Initialize()
        {
            _gamePlayModeView.ValueChanged += HandleDropdownChanged;

            _gamePlayModeView.Initialize(_optionData);
        }

        private void HandleDropdownChanged(Dropdown.OptionData optionData)
        {
            Debug.Log("Value changed to " + optionData.text);
        }
    }
}