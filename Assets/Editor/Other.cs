using UnityEditor;
using UnityEngine;

public class Other : EditorWindow
{
    [MenuItem("Tools/Assign To Player/Gaia", false, 0)]
    public static void AssignToPlayerGaia()
    {
        AssignToPlayer(GameObject.Find("Gaia").GetComponent<Player>());
    }

    [MenuItem("Tools/Assign To Player/CPU", false, 0)]
    public static void AssignToPlayerCPU()
    {
        AssignToPlayer(GameObject.Find("CPU").GetComponent<Player>());
    }

    [MenuItem("Tools/Assign To Player/Human", false, 0)]
    public static void AssignToPlayerHuman ()
    {
        AssignToPlayer(GameObject.Find("Human").GetComponent<Player>());
    }

    private static void AssignToPlayer(Player player)
    {
        foreach (GameObject gameObject in Selection.gameObjects)
        {
            if (gameObject.TryGetComponent(out MyGameObject myGameObject))
            {
                myGameObject.SetPlayer(player);
            }
        }
    }
}
