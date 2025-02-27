using Player;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class FirstPersonLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Player Movement Related
        builder.Register<FirstPersonModel>(Lifetime.Singleton);
        builder.RegisterComponentInHierarchy<FirstPersonView>();
        builder.RegisterEntryPoint<FirstPersonPresenter>();

        // PlayerInput Related
        builder.RegisterComponentInHierarchy<PlayerInput>();
    }
}
