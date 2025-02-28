﻿namespace Gameplay.UI.General.ReactiveViews
{
    public class TextMeshProReactiveInt : TextMeshProReactiveNumber<int>
    {
        protected override string GetTextValue(int value)
        {
            return (ShowPositivePlus && value > 0 ? "+" : "") + value;
        }
    }


}