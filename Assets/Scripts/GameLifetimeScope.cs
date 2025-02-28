using Player;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        RegisterPlayerRelated(builder);
        RegisterUIRelated(builder);
    }

    private void RegisterUIRelated(IContainerBuilder builder)
    {
        // InteractionPrompt
        builder.RegisterComponentInHierarchy<InteractionPromptView>();
        builder.RegisterEntryPoint<InteractionPromptPresenter>();
        builder.Register<InteractionPromptUIModel>(Lifetime.Singleton);
    }

    private void RegisterPlayerRelated(IContainerBuilder builder)
    {
        // General
        builder.RegisterComponentInHierarchy<PlayerView>();
        builder.RegisterComponentInHierarchy<PlayerInput>();
        builder.RegisterComponentInHierarchy<Camera>();

        // Player Movement Related
        builder.Register<PlayerLocomotionModel>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerLocomotionPresenter>();

        // Player Interaction Related
        builder.Register<PlayerInteractionModel>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PlayerInteractionPresenter>();
    }
}
