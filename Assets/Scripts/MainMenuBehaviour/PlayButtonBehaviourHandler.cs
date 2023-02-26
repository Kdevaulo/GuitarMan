using UnityEngine.SceneManagement;

namespace GuitarMan.MainMenuBehaviour
{
    public sealed class PlayButtonBehaviourHandler : AbstractButtonBehaviourHandler
    {
        public PlayButtonBehaviourHandler(MenuButtonView buttonView) : base(buttonView)
        {
        }

        protected override void HandleButtonClick()
        {
            // todo: change to async load|unload with loading screen
            SceneManager.LoadScene("Scenes/FileDialog");
        }
    }
}