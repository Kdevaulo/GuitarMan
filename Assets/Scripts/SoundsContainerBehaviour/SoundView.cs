using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace GuitarMan.SoundsContainerBehaviour
{
    public class SoundView : MonoBehaviour, IDisposable
    {
        public event Action SoundPlayClicked = delegate { };
        public event Action RemoveSoundClicked = delegate { };

        [SerializeField] private TextMeshProUGUI _textMeshPro;

        [SerializeField] private Button _playSoundButton;

        [SerializeField] private Button _removeSoundButton;

        void IDisposable.Dispose()
        {
            _playSoundButton.onClick.RemoveListener(HandlePlayButtonClick);
            _removeSoundButton.onClick.RemoveListener(HandleRemoveButtonClick);
        }

        public void Initialize()
        {
            _playSoundButton.onClick.AddListener(HandlePlayButtonClick);
            _removeSoundButton.onClick.AddListener(HandleRemoveButtonClick);
        }

        public void SetText(string text)
        {
            _textMeshPro.text = text;
        }

        private void HandlePlayButtonClick()
        {
            SoundPlayClicked.Invoke();
        }

        private void HandleRemoveButtonClick()
        {
            RemoveSoundClicked.Invoke();
        }
    }
}