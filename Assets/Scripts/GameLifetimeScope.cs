using Database;
using Player;
using System.Threading.Tasks;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override async void Configure(IContainerBuilder builder)
    {
        await RegisterDatabaseAsync(builder);

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

    private static async Task RegisterDatabaseAsync(IContainerBuilder builder)
    {
        DialogueDatabase dialogueDatabase = await Addressables.LoadAssetAsync<DialogueDatabase>("DialogueDatabase").Task;

        builder.RegisterInstance(dialogueDatabase);
    }
}
