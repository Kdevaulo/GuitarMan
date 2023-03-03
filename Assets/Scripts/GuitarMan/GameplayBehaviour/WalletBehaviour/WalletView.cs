using UnityEngine;
using UnityEngine.UI;

namespace GuitarMan.GameplayBehaviour.WalletBehaviour
{
    [AddComponentMenu(nameof(WalletBehaviour) + "/" + nameof(WalletView))]
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private Text _currentMoneyText;

        public void SetMoneyValue(int value)
        {
            _currentMoneyText.text = value.ToString();
        }
    }
}