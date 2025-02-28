using UnityEngine;

namespace Gameplay.UI.ReactiveViews
{
    public class TextMeshProReactiveFloat : TextMeshProReactiveNumber<float>
    {
        [SerializeField] protected int countFloating = 0;

        protected override string GetTextValue(float value)
        {
            string formatString = "0." + new string('#', countFloating);

            return (ShowPositivePlus && value > 0 ? "+" : "") + value.ToString(formatString);
        }
    }


}