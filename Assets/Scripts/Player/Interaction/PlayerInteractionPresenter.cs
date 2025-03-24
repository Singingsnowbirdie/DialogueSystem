using DialogueSystem;
using InteractionSystem;
using System;
using UI;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Player
{
    public class PlayerInteractionPresenter : IStartable, IDisposable
    {
        [Inject] private readonly PlayerInteractionModel _model;
        [Inject] private readonly PlayerInput _playerInput;
        [Inject] private readonly Camera _camera;
        [Inject] private readonly DialogueModel _dialogueModel;

        private CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private const float InteractionDistance = 3f;

        public void Start()
        {
            _playerInput.actions["Interact"].started += OnInteract;

            Observable.EveryUpdate()
                .Subscribe(_ => DetectInteractable())
                .AddTo(_compositeDisposable);

            _dialogueModel.IsDialogueOccurs
                .Subscribe(val => OnDialogueOccurs(val))
                .AddTo(_compositeDisposable);
        }

        private void OnDialogueOccurs(bool val)
        {
            if (!val && _model.IsInteracting.Value)
            {
                _model.IsInteracting.Value = false;
            }
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (_model.CurrentInteractable.Value != null)
            {
                Interact(_model.CurrentInteractable.Value);
            }
        }

        private void DetectInteractable()
        {
            if (_model.IsInteracting.Value)
                return;

            Ray ray = new(_camera.transform.position, _camera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, InteractionDistance))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    _model.CurrentInteractable.Value = interactable;
                    return;
                }
            }

            _model.CurrentInteractable.Value = null;
        }

        private void Interact(IInteractable interactable)
        {
            interactable.Interact(this);
            _model.IsInteracting.Value = true;
        }

        internal void OnInteractionCompleted()
        {
            _model.IsInteracting.Value = false;
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}