public class MenuGame : UI_Element<MenuGame>
{
    protected override void Awake()
    {
        base.Awake();

        // button = GameMenu.Instance.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Panel_Indicators").Q<Button>("Button");
    }

    // TODO: Implement.

    /*
    private void Update()
    {
        if (HUD.Instance.ActivePlayer.Selection.Items.Count > 0)
        {
            var position = Camera.main.WorldToScreenPoint(HUD.Instance.ActivePlayer.Selection.First().Position);

            button.style.left = position.x;
            button.style.right = StyleKeyword.Auto;
            button.style.bottom = position.y;
            button.style.top = StyleKeyword.Auto;
        }
    }

    Button button;
    */
}
