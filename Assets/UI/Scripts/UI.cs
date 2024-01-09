using UnityEngine;

public class UI : Singleton<UI>
{
    public void GoToMenu(MenuType menuType)
    {
        Hide();

        switch (menuType)
        {
            case MenuType.Diplomacy:
                MenuDiplomacy.Show(true);
                break;

            case MenuType.Editor:
                MenuEditor.Show(true);
                break;

            case MenuType.Game:
                MenuGame.Show(true);
                break;

            case MenuType.Main:
                MenuMain.Show(true);
                break;

            case MenuType.Options:
                MenuOptions.Show(true);
                break;

            case MenuType.Scene:
                MenuScene.Show(true);
                break;
        }
    }

    private void Hide()
    {
        MenuDiplomacy.Show(false);
        MenuEditor.Show(false);
        MenuGame.Show(false);
        MenuMain.Show(false);
        MenuOptions.Show(false);
        MenuScene.Show(false);
    }

    [SerializeField]
    public MenuDiplomacy MenuDiplomacy { get; private set; }

    [SerializeField]
    public MenuEditor MenuEditor { get; private set; }

    [field: SerializeField]
    public MenuGame MenuGame { get; private set; }

    [SerializeField]
    public MenuMain MenuMain { get; private set; }

    [SerializeField]
    public MenuOptions MenuOptions { get; private set; }

    [SerializeField]
    public MenuScene MenuScene { get; private set; }
}
