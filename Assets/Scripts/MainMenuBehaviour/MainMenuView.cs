using UnityEngine;

namespace GuitarMan.MainMenuBehaviour
{
    public class MainMenuView : MonoBehaviour
    {
        public MenuButtonView[] ButtonViews => _buttonViews;

        [SerializeField] private MenuButtonView[] _buttonViews;
    }
}