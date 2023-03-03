using UnityEngine;

namespace GuitarMan.MainMenuBehaviour
{
    [CreateAssetMenu(fileName = nameof(CreditsButtonBehaviourHandler),
        menuName = nameof(MainMenuBehaviour) + "/" + nameof(CreditsButtonBehaviourHandler))]
    public class CreditsButtonBehaviourHandler : AbstractButtonBehaviourHandler
    {
        protected override void HandleButtonClick()
        {
            Debug.Log($"{nameof(CreditsButtonBehaviourHandler)} {nameof(HandleButtonClick)} — ShowCredits");
        }
    }
}