using Player;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Player Movement Related
        builder.Register<PlayerLocomotionModel>(Lifetime.Singleton);
        builder.RegisterComponentInHierarchy<PlayerLocomotionView>();
        builder.RegisterEntryPoint<PlayerLocomotionPresenter>();

        // PlayerInput Related
        builder.RegisterComponentInHierarchy<PlayerInput>();
    }
}
