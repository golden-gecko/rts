using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Tools : EditorWindow
{
    [MenuItem("Tools/Render")]
    public static void Render()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(Path.Join("Assets", "Scenes", "Editor.unity"));

        int size = 256;
        RenderTexture renderTexture = new RenderTexture(size, size, 24);

        Camera camera = GameObject.Find("Setup").transform.Find("Cameras").transform.Find("MainCamera").GetComponent<Camera>();
        camera.transform.position = new Vector3(1.5f, 1.0f, 1.5f);
        camera.transform.localEulerAngles = new Vector3(15.0f, 235.0f, 0.0f);
        camera.targetTexture = renderTexture;

        Texture2D texture = new Texture2D(size, size, TextureFormat.RGB24, false);

        foreach (GameObject gameObject in GetGameObjects())
        {
            GameObject instance = Instantiate(gameObject);

            camera.Render();

            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, size, size), 0, 0);

            byte[] bytes = texture.EncodeToPNG();
            string filename = GetPortraitPath(gameObject.name);
            File.WriteAllBytes(filename, bytes);

            Debug.Log(string.Format("Saving portrait to {0}", filename));

            DestroyImmediate(instance);
        }

        camera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(renderTexture);
    }

    [MenuItem("Tools/Validate")]
    public static void Validate()
    {
        ClearLog();

        IEnumerable<GameObject> gameObjects = GetGameObjects();

        CheckPhysics(gameObjects);
        CheckStorage(gameObjects);
    }

    private static IEnumerable<GameObject> GetGameObjects()
    {
        string[] folders = new string[] { Path.Join("Assets", "Prefabs", "Components"), Path.Join("Assets", "Prefabs", "Objects") };
        string[] assets = AssetDatabase.FindAssets("t:prefab", folders);

        IEnumerable<string> paths = assets.Select(x => AssetDatabase.GUIDToAssetPath(x));
        IEnumerable<GameObject> gameObjects = paths.Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(x));

        return gameObjects;
    }

    private static string GetPortraitPath(string name)
    {
        return Path.Combine(new string[] { "Assets", "UI", "Materials", "Portraits", string.Format("{0}.png", name) });
    }

    private static void ClearLog()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(Editor));
        Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo method = type.GetMethod("Clear");

        method.Invoke(new object(), null);
    }

    private static void CheckPhysics(IEnumerable<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            foreach (Collider collider in gameObject.GetComponentsInChildren<Collider>())
            {
                if (collider.isTrigger)
                {
                    Debug.Log(string.Format("Resource {0} has collider trigger enabled ({1}).", gameObject.name, collider.isTrigger));
                }
            }

            foreach (Rigidbody rigidbody in gameObject.GetComponentsInChildren<Rigidbody>())
            {
                if (rigidbody.isKinematic == false)
                {
                    Debug.Log(string.Format("Resource {0} is not kinematic ({1}).", gameObject.name, rigidbody.isKinematic));
                }

                if (rigidbody.useGravity)
                {
                    Debug.Log(string.Format("Resource {0} is using gravity ({1}).", gameObject.name, rigidbody.useGravity));
                }
            }
        }
    }

    private static void CheckStorage(IEnumerable<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            foreach (Storage storage in gameObject.GetComponentsInChildren<Storage>())
            {
                foreach (Resource resource in storage.Resources.Items)
                {
                    if (resource.Max <= 0)
                    {
                        Debug.Log(string.Format("Resource {0} has invalid max value ({1}, {2}).", gameObject.name, resource.Name, resource.Max));
                    }
                }
            }
        }
    }
}
