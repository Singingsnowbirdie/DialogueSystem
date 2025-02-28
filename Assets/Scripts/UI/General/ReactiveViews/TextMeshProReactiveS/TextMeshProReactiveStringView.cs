namespace Gameplay.UI.General.ReactiveViews
{
    public class TextMeshProReactiveStringView : TextMeshProReactiveView<string>
    {
        protected override string GetTextValue(string value)
        {
            return value;
        }
    }
}