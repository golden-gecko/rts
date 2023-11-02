using UnityEngine.UIElements;

public class MenuOptions : UI_Element<MenuOptions>
{
    protected override void Awake()
    {
        base.Awake();

        Root.Q<Label>("Header").text = "Options";
    }
}
