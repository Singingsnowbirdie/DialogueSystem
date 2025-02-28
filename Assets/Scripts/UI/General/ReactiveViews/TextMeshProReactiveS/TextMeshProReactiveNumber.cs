using UnityEngine;

namespace Gameplay.UI.General.ReactiveViews
{
    public abstract class TextMeshProReactiveNumber<T> : TextMeshProReactiveView<T>
    {
        [field: SerializeField] public bool ShowPositivePlus { get; private set; } = false;

    }


}