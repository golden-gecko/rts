using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Builder : EditorWindow
{
    [MenuItem("Tools/Build", false, 1)]
    public static void Build()
    {
        Tools.ClearLog();

        string sceneName = Tools.GetSceneName();

        List<Blueprint> blueprints = Utils.CreateBlueprints();

        foreach (Blueprint blueprint in blueprints)
        {
            MyGameObject myGameObject = Utils.CreateGameObject(blueprint, Vector3.zero, Quaternion.identity, null, MyGameObjectState.Operational);

            PrefabUtility.SaveAsPrefabAsset(myGameObject.gameObject, Path.Join(Config.Prefabs.Directory, "Blueprints", string.Format("{0}.prefab", myGameObject.name)));

            DestroyImmediate(myGameObject.gameObject);
        }

        Tools.RestoreScene(sceneName);
    }
}
