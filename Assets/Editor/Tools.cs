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

        Transform setup = GameObject.Find("Setup").transform;
        Transform editor = setup.Find("Editor").transform;
        Transform placeholder = editor.Find("Placeholder").transform;
        Transform cameraTransform = editor.Find("Camera").transform;

        Camera camera = cameraTransform.GetComponent<Camera>();

        Texture2D texture = new Texture2D(camera.targetTexture.width, camera.targetTexture.height, TextureFormat.RGB24, false, false);


        foreach (GameObject gameObject in GetGameObjects())
        {
            GameObject instance = Instantiate(gameObject, placeholder);

            camera.Render();

            RenderTexture.active = camera.targetTexture;
            texture.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);

            byte[] bytes = texture.EncodeToPNG();
            string filename = GetPortraitPath(gameObject.name);
            File.WriteAllBytes(filename, bytes);

            Debug.Log(string.Format("Saving portrait to {0}", filename));

            DestroyImmediate(instance);
        }
    }

    [MenuItem("Tools/Validate")]
    public static void Validate()
    {
        ClearLog();

        IEnumerable<GameObject> gameObjects = GetGameObjects();

        ValidateNames(gameObjects);
        ValidateProperties(gameObjects);
        ValidateStorage(gameObjects);
        ValidatePhysics(gameObjects);
    }

    private static IEnumerable<GameObject> GetGameObjects()
    {
        string[] folders = new string[] { Path.Join("Assets", "Prefabs", "Objects"), Path.Join("Assets", "Prefabs", "Parts") };
        string[] assets = AssetDatabase.FindAssets("t:prefab", folders);

        IEnumerable<string> paths = assets.Select(x => AssetDatabase.GUIDToAssetPath(x));
        IEnumerable<GameObject> gameObjects = paths.Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(x));

        return gameObjects;
    }

    private static string GetPortraitPath(string name)
    {
        return Path.Combine(new string[] { "Assets", "Resources", "Portraits", string.Format("{0}.png", name) });
    }

    private static void ClearLog()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(Editor));
        Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo method = type.GetMethod("Clear");

        method.Invoke(new object(), null);
    }

    private static void ValidateNames(IEnumerable<GameObject> gameObjects)
    {
        Dictionary<string, int> names = new Dictionary<string, int>();

        foreach (GameObject gameObject in gameObjects)
        {
            if (names.TryGetValue(gameObject.name, out int _))
            {
                names[gameObject.name] += 1;
            }
            else
            {
                names[gameObject.name] = 1;
            }
        }

        IEnumerable<KeyValuePair<string, int>> duplicates = names.Where(x => x.Value > 1);

        if (duplicates.Count() > 0)
        {
            Debug.Log(string.Format("Some game objects have duplicates names: {0}", string.Join(", ", names.Where(x => x.Value > 1))));
        }
    }

    private static void ValidateProperties(IEnumerable<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.TryGetComponent(out MyGameObject myGameObject))
            {
                if (myGameObject.DestroyEffect == null)
                {
                    Debug.Log(string.Format("Resource {0} has no destroy effect.", gameObject.name));
                }

                if (myGameObject.MapLayers.Count <= 0)
                {
                    Debug.Log(string.Format("Resource {0} has no map layers.", gameObject.name));
                }

                foreach (string skill in myGameObject.Skills.Keys)
                {
                    if (skill.Length <= 0)
                    {
                        Debug.Log(string.Format("Resource {0} has invalid skill name ({1}).", gameObject.name, skill));
                    }
                }
            }
        }
    }

    private static void ValidateStorage(IEnumerable<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            foreach (Storage storage in gameObject.GetComponentsInChildren<Storage>())
            {
                foreach (Resource resource in storage.Resources.Items)
                {
                    if (resource.Name.Length <= 0)
                    {
                        Debug.Log(string.Format("Resource {0} has invalid resource name ({1}).", gameObject.name, resource));
                    }

                    if (resource.Current < 0)
                    {
                        Debug.Log(string.Format("Resource {0} has invalid current value ({1}, {2}).", gameObject.name, resource.Name, resource.Current));
                    }

                    if (resource.Max <= 0)
                    {
                        Debug.Log(string.Format("Resource {0} has invalid max value ({1}, {2}).", gameObject.name, resource.Name, resource.Max));
                    }
                }
            }
        }
    }

    private static void ValidatePhysics(IEnumerable<GameObject> gameObjects)
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

}
