using UnityEngine;
using UnityEngine.UI;

namespace GuitarMan.WalletBehaviour
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private Text _currentMoneyText;

        public void SetMoneyValue(int value)
        {
            _currentMoneyText.text = value.ToString();
        }
    }
}