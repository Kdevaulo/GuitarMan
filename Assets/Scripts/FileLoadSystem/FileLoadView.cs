using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace GuitarMan.FileLoadSystem
{
    [AddComponentMenu(nameof(FileLoadSystem) + "/" + nameof(FileLoadView))]
    public class FileLoadView : MonoBehaviour, IDisposable
    {
        public event Action LoadButtonClicked = delegate { };

        [SerializeField] private Button _loadFilesButton;

        [SerializeField] private Transform _loadingInProgressIcon;

        [SerializeField] private GameObject _loadedIcon;

        public AudioSource AudioSource;
        
        private readonly Vector3 _loadingRotation = new Vector3(0f, 0f, -360f);

        private CancellationTokenSource _cts;

        private Tween _currentTween;

        private void Awake()
        {
            _loadFilesButton.onClick.AddListener(HandleButtonClick);
        }

        void IDisposable.Dispose()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            _loadFilesButton.onClick.RemoveListener(HandleButtonClick);
        }

        public void VisualizeLoadFinished()
        {
            TryKillTween();

            SwitchIcon();
        }

        public void VisualizeLoadStarted()
        {
            SwitchIcon(false);

            VisualizeLoadingAsync().Forget();
        }

        private async UniTask VisualizeLoadingAsync()
        {
            TryKillTween();

            _currentTween = _loadingInProgressIcon.DORotate(_loadingRotation, 2, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear).SetLoops(-1);

            await _currentTween;
        }

        private void HandleButtonClick()
        {
            LoadButtonClicked.Invoke();
        }

        private void SwitchIcon(bool loadFinished = true)
        {
            _loadingInProgressIcon.gameObject.SetActive(!loadFinished);
            _loadedIcon.SetActive(loadFinished);
        }

        private void TryKillTween()
        {
            _currentTween?.Kill();
        }
    }
}