using UnityEngine;

public class SetupMenu : MonoBehaviour
{
    void Start()
    {
        DiplomacyMenu.Instance.gameObject.SetActive(false);
        GameMenu.Instance.gameObject.SetActive(false);
        MainMenu.Instance.gameObject.SetActive(true);
        SceneMenu.Instance.gameObject.SetActive(false);
    }
}
