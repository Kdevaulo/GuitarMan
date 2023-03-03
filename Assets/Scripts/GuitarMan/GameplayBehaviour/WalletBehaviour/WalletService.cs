using UnityEngine;
using UnityEngine.Assertions;

namespace GuitarMan.GameplayBehaviour.WalletBehaviour
{
    public class WalletService
    {
        private readonly WalletView _walletView;

        private int _currentMoneyValue;

        public WalletService(WalletView walletView)
        {
            _walletView = walletView;
        }

        public void AddMoney(int value)
        {
            Assert.IsTrue(value > 0);

            _currentMoneyValue += value;
            LogMoneyChange(value, true);

            UpdateShowingValue();
        }

        public void RemoveMoney(int value)
        {
            Assert.IsTrue(value > 0);

            if (_currentMoneyValue < value)
            {
                LogMoneyChange(_currentMoneyValue);
                _currentMoneyValue = 0;
            }
            else
            {
                LogMoneyChange(value);
                _currentMoneyValue -= value;
            }

            UpdateShowingValue();
        }

        private void LogMoneyChange(int value, bool added = false)
        {
            if (added)
            {
                Debug.Log("added " + value);
            }
            else
            {
                Debug.Log("removed " + value);
            }
        }

        private void UpdateShowingValue()
        {
            _walletView.SetMoneyValue(_currentMoneyValue);
        }
    }
}