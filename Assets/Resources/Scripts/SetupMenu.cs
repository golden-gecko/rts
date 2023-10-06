using UnityEngine;

public class SetupMenu : MonoBehaviour
{
    void Start()
    {
        DiplomacyMenu.Instance.Show(false);
        GameMenu.Instance.Show(false);
        MainMenu.Instance.Show(true);
        SceneMenu.Instance.Show(false);
    }
}
