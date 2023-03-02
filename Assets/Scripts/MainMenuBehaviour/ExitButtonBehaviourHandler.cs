using UnityEngine;

namespace GuitarMan.MainMenuBehaviour
{
    [CreateAssetMenu(fileName = nameof(ExitButtonBehaviourHandler),
        menuName = nameof(MainMenuBehaviour) + "/" + nameof(ExitButtonBehaviourHandler))]
    public class ExitButtonBehaviourHandler : AbstractButtonBehaviourHandler
    {
        protected override void HandleButtonClick()
        {
            Application.Quit();
        }
    }
}