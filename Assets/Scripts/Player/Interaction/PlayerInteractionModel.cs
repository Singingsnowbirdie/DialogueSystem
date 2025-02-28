using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace UI
{
}

namespace Player
{
    public class PlayerInteractionModel
    {
        public ReactiveProperty<GameObject> CurrentInteractable = new ReactiveProperty<GameObject>();
        public ReactiveProperty<bool> IsInteracting = new ReactiveProperty<bool>(false);
    }

    public class PlayerInteractionPresenter : IStartable
    {
        [Inject] private readonly PlayerInteractionModel _model;
        [Inject] private readonly PlayerInput _playerInput;
        [Inject] private readonly Camera _camera;
        [Inject] private readonly InteractionPromptView _view; // TEMP

        private const float InteractionDistance = 3f;

        public void Start()
        {
            _playerInput.actions["Interact"].performed += OnInteract;

            Observable.EveryUpdate()
                .Subscribe(_ => DetectInteractable())
                .AddTo(_view);

            _model.CurrentInteractable
                .Subscribe(interactable =>
                {
                    if (interactable != null)
                    {
                        _view.ShowInteractionPrompt("Нажми E");
                    }
                    else
                    {
                        _view.HideInteractionPrompt();
                    }
                })
                .AddTo(_view);
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (_model.CurrentInteractable.Value != null)
            {
                InteractWithObject(_model.CurrentInteractable.Value);
            }
        }

        private void DetectInteractable()
        {
            // Бросаем луч из камеры
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, InteractionDistance))
            {
                // Проверяем, есть ли у объекта компонент для взаимодействия
                if (hit.collider.GetComponent<IInteractable>() != null)
                {
                    _model.CurrentInteractable.Value = hit.collider.gameObject;
                    return;
                }
            }

            // Если ничего не найдено
            _model.CurrentInteractable.Value = null;
        }

        private void InteractWithObject(GameObject interactable)
        {
            // Вызываем метод взаимодействия
            interactable.GetComponent<IInteractable>().Interact();
            _model.IsInteracting.Value = true;
        }
    }
}