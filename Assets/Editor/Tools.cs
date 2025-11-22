using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Tools
{
    public static MyGameObject[] GetGameObjects()
    {
        return SceneManager
            .GetActiveScene()
            .GetRootGameObjects()
            .Where(x => x.TryGetComponent(out MyGameObject _))
            .Select(x => x.GetComponent<MyGameObject>())
            .ToArray();
    }

    public static IEnumerable<GameObject> GetPrefabs()
    {
        string[] folders = new string[] { Path.Join("Assets", "Prefabs", "Objects"), Path.Join("Assets", "Prefabs", "Parts") };
        string[] assets = AssetDatabase.FindAssets("t:prefab", folders);

        IEnumerable<string> paths = assets.Select(x => AssetDatabase.GUIDToAssetPath(x));
        IEnumerable<GameObject> gameObjects = paths.Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(x));

        return gameObjects;
    }

    public static string GetPortraitPath(string name)
    {
        return Path.Combine(new string[] { "Assets", "Resources", "Portraits", string.Format("{0}.png", name) });
    }

    public static void ClearLog()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(Editor));
        Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo method = type.GetMethod("Clear");

        method.Invoke(new object(), null);
    }

    public static string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static void RestoreScene(string sceneName)
    {
        EditorSceneManager.OpenScene(Path.Join("Assets", "Scenes", string.Format("{0}.unity", sceneName)));
    }

    public static void SaveScene()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
    }
}
