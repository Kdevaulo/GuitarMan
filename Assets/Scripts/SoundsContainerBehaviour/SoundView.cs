using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace GuitarMan.SoundsContainerBehaviour
{
    public class SoundView : MonoBehaviour, IDisposable
    {
        public event Action SoundPlayClicked = delegate { };

        [SerializeField] private TextMeshProUGUI _textMeshPro;

        [SerializeField] private Button _playSoundButton;

        void IDisposable.Dispose()
        {
            _playSoundButton.onClick.RemoveListener(HandleButtonClick);
        }

        public void Initialize()
        {
            _playSoundButton.onClick.AddListener(HandleButtonClick);
        }

        public void SetText(string text)
        {
            _textMeshPro.text = text;
        }

        private void HandleButtonClick()
        {
            SoundPlayClicked.Invoke();
        }
    }
}