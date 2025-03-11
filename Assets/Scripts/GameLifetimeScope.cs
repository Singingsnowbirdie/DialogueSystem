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
    [SerializeField] private PlayerInfoView _playerInfoView;

    [Header("PLAYER")]
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerInput _playerInput;

    [Header("CAMERA")]
    [SerializeField] private Camera _camera;
    [SerializeField] private DialogueCameraView _dialogueCameraView;

    [Header("NPC")]
    [SerializeField] private NPCManagerView _npcManagerView;

    [Header("DATABASE")]
    [SerializeField] private DialogueDatabase _dialogueDatabase;

    protected override void Configure(IContainerBuilder builder)
    {
        // Register Views
        builder.RegisterComponent(_playerView).AsSelf();
        builder.RegisterComponent(_dialogueCameraView).AsSelf();
        builder.RegisterComponent(_npcManagerView).AsSelf();

        // Register UI Views
        builder.RegisterComponent(_dialogueView).AsSelf();
        builder.RegisterComponent(_interactionPromptView).AsSelf();
        builder.RegisterComponent(_playerInfoView).AsSelf();

        // Register Other Components
        builder.RegisterComponent(_playerInput).AsSelf();
        builder.RegisterComponent(_camera).AsSelf();
        builder.RegisterComponent(_dialogueDatabase).AsSelf();

        // Register Models
        builder.Register<PlayerModel>(Lifetime.Singleton);
        builder.Register<PlayerLocomotionModel>(Lifetime.Singleton);
        builder.Register<PlayerInteractionModel>(Lifetime.Singleton);
        builder.Register<DialogueModel>(Lifetime.Singleton);
        builder.Register<NPCManagerModel>(Lifetime.Singleton);
        builder.Register<DialogueCameraModel>(Lifetime.Singleton);

        // Register UI Models
        builder.Register<InteractionPromptUIModel>(Lifetime.Singleton);
        builder.Register<DialogueUIModel>(Lifetime.Singleton);
        builder.Register<PlayerInfoUIModel>(Lifetime.Singleton);

        // Register Presenters
        builder.RegisterEntryPoint<PlayerPresenter>(Lifetime.Singleton);
        builder.RegisterEntryPoint<DialoguePresenter>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerLocomotionPresenter>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerInteractionPresenter>(Lifetime.Singleton);
        builder.RegisterEntryPoint<NPCManagerPresenter>(Lifetime.Singleton);
        builder.RegisterEntryPoint<DialogueCameraPresenter>(Lifetime.Singleton);

        // Register UI Presenters
        builder.RegisterEntryPoint<InteractionPromptPresenter>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerInfoUIPresenter>(Lifetime.Singleton);
    }
}
