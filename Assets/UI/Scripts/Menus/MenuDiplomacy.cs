using UnityEngine.UIElements;

public class MenuDiplomacy : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        Root.Q<Label>("Header").text = "Diplomacy";

        ButtonOK = Root.Q<Button>("OK");
        ButtonOK.RegisterCallback<ClickEvent>(x => OnButtonOK());

        ButtonCancel = Root.Q<Button>("Cancel");
        ButtonCancel.RegisterCallback<ClickEvent>(x => OnButtonCancel());
    }

    private void OnButtonOK()
    {
        UI.Instance.GoToMenu(MenuType.Game);
    }

    private void OnButtonCancel()
    {
        UI.Instance.GoToMenu(MenuType.Game);
    }

    private Button ButtonOK;
    private Button ButtonCancel;
}
