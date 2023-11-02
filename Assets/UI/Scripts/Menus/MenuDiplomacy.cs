using UnityEngine.UIElements;

public class MenuDiplomacy : UI_Element<MenuDiplomacy>
{
    protected override void Awake()
    {
        base.Awake();

        Root.Q<Label>("Header").text = "Diplomacy";
    }
}
