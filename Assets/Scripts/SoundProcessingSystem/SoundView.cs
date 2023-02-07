using TMPro;

using UnityEngine;

namespace GuitarMan.SoundProcessingSystem
{
    public class SoundView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        public void SetText(string text)
        {
            _textMeshPro.text = text;
        }
    }
}