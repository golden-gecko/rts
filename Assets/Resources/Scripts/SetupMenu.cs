using UnityEngine;

public class SetupMenu : MonoBehaviour
{
    void Start()
    {
        GameMenu.Instance.gameObject.SetActive(false);
        MainMenu.Instance.gameObject.SetActive(true);
        SceneMenu.Instance.gameObject.SetActive(false);
    }
}
