using Database;
using DialogueSystem;
using NPC;
using Player;
using UI;
using UI.DialogueUI;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [Header("UI")]
    [SerializeField] private InteractionPromptView _interactionPromptView;
    [SerializeField] private DialogueView _dialogueView;

    [Header("PLAYER")]
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Camera _camera;

    [Header("NPC")]
    [SerializeField] private NPCCollectionView _npcCollectionView;

    [Header("DATABASE")]
    [SerializeField] private DialogueDatabase _dialogueDatabase;

    protected override void Configure(IContainerBuilder builder)
    {
        // Register Components
        builder.RegisterComponent(_playerView).AsSelf();
        builder.RegisterComponent(_playerInput).AsSelf();
        builder.RegisterComponent(_camera).AsSelf();
        builder.RegisterComponent(_dialogueDatabase).AsSelf();
        builder.RegisterComponent(_dialogueView).AsSelf();
        builder.RegisterComponent(_interactionPromptView).AsSelf();
        builder.RegisterComponent(_npcCollectionView).AsSelf();

        // Register Models
        builder.Register<PlayerModel>(Lifetime.Singleton);
        builder.Register<PlayerLocomotionModel>(Lifetime.Singleton);
        builder.Register<PlayerInteractionModel>(Lifetime.Singleton);
        builder.Register<DialogueModel>(Lifetime.Singleton);
        builder.Register<DialogueUIModel>(Lifetime.Singleton);
        builder.Register<InteractionPromptUIModel>(Lifetime.Singleton);
        builder.Register<NPCCollectionModel>(Lifetime.Singleton);

        // Register Presenters
        builder.RegisterEntryPoint<DialoguePresenter>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerLocomotionPresenter>(Lifetime.Singleton);
        builder.RegisterEntryPoint<InteractionPromptPresenter>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerInteractionPresenter>(Lifetime.Singleton);
        builder.RegisterEntryPoint<NPCCollectionPresenter>(Lifetime.Singleton);
    }
}
