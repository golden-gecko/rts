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
    private MenuDiplomacy MenuDiplomacy;

    [SerializeField]   
    private MenuEditor MenuEditor;

    [SerializeField]
    private MenuGame MenuGame;

    [SerializeField]
    private MenuMain MenuMain;

    [SerializeField]
    private MenuOptions MenuOptions;

    [SerializeField]
    private MenuScene MenuScene;
}
