using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GuitarMan.MainMenuBehaviour
{
    [AddComponentMenu(nameof(MainMenuBehaviour) + "/" + nameof(MenuButtonView)),
     RequireComponent(typeof(Button))]
    public class MenuButtonView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action ButtonClicked = delegate { };
        public event Action ButtonPointerEnter = delegate { };
        public event Action ButtonPointerExit = delegate { };

        public ButtonType ButtonType => _buttonType;

        [SerializeField] private ButtonType _buttonType;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            ButtonPointerEnter.Invoke();
            Debug.Log("PointerEnter");
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            ButtonPointerExit.Invoke();
            Debug.Log("PointerExit");
        }

        private void HandleButtonClick()
        {
            ButtonClicked.Invoke();
        }
    }
}