using UnityEngine;

public class SetupLevel : MonoBehaviour
{
    void Start()
    {
        GameMenu.Instance.gameObject.SetActive(true);
        MainMenu.Instance.gameObject.SetActive(false);
        SceneMenu.Instance.gameObject.SetActive(false);
    }
}
