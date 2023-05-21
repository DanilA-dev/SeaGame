using DG.Tweening;
using Player;
using StageSystem;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image _healthFill;
        [SerializeField] private Image _damageEffect;
        [SerializeField] private CanvasGroup _winMenu;
        [SerializeField] private CanvasGroup _loseMenu;
        [SerializeField] private RectTransform _aimSight;

        private Camera _cam;
        
        private void Start()
        {
            _cam = Camera.main;

            MessageBroker.Default.Receive<GameStateSignal>()
                .Subscribe(signal => UpdateOnState(signal.NewState)).AddTo(gameObject);
            
            MessageBroker.Default.Receive<PlayerHealthChangeSignal>()
                .Subscribe(signal => OnPlayerDamage(signal.CurrentHealth, signal.MaxHealth, signal.IsDamaged)).AddTo(gameObject);
        }


        public void Update()
        {
            UpdateAimSight();
        }

        private void UpdateAimSight()
        {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;

            _aimSight.position = new Vector3(x, y);
        }

        private void UpdateOnState(GameState signalNewState)
        {
            if (signalNewState == GameState.Lose)
            {
                _loseMenu.gameObject.SetActive(true);
                _loseMenu.DOFade(1f, 1f).From(0);
            }

            if (signalNewState == GameState.Win)
            {
                _winMenu.gameObject.SetActive(true);
                _winMenu.DOFade(1f, 1f).From(0);
            }
        }

        private void OnPlayerDamage(float signalCurrentHealth, float signalMaxHealth, bool isDamaged)
        {
            _healthFill.fillAmount = signalCurrentHealth / signalMaxHealth;

            if (isDamaged)
            {
                var seq = DOTween.Sequence();
                seq.Append(_damageEffect.DOFade(0.25f, 0.25f).From(0));
                seq.Append(_damageEffect.DOFade(0, 0.25f).From(0.25f));
            }
        }
    }
}