using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameObjectBuilder : EditorWindow
{
    [MenuItem("Tools/Align", false, 10)]
    public static void Align()
    {
        MyGameObject[] gameObjects = Tools.GetGameObjects();

        // System.Array.Sort(gameObjects, (a, b) => a.transform.position.y < b.transform.position.y ? -1 : 1);

        foreach (MyGameObject myGameObject in gameObjects)
        {
            /* TODO: Fix.
            if (myGameObject.TryGetComponent(out Foundation _))
            {
                AlignGameObject(myGameObject, true);
            }
            else
            {
                AlignGameObject(myGameObject);
            }
            */

            AlignGameObject(myGameObject);
        }

        Tools.SaveScene();
    }

    [MenuItem("Tools/Build", false, 20)]
    public static void Build()
    {
        Tools.ClearLog();

        string sceneName = Tools.GetSceneName();

        List<Blueprint> blueprints = Utils.CreateBlueprints();

        foreach (Blueprint blueprint in blueprints)
        {
            MyGameObject myGameObject = Utils.CreateGameObject(blueprint, Vector3.zero, Quaternion.identity, null, MyGameObjectState.Operational);

            PrefabUtility.SaveAsPrefabAsset(myGameObject.gameObject, Path.Join(Config.Objects.Directory, string.Format("{0}.prefab", myGameObject.name)));

            DestroyImmediate(myGameObject.gameObject);
        }

        Tools.RestoreScene(sceneName);
    }

    [MenuItem("Tools/Render", false, 30)]
    public static void Render()
    {
        Tools.ClearLog();

        string sceneName = Tools.GetSceneName();

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(Path.Join("Assets", "Scenes", "Editor.unity"));

        foreach (string file in Directory.EnumerateFiles(Path.Combine(new string[] { "Assets", "Resources", "Portraits" })))
        {
            File.Delete(file);
        }

        Transform setup = GameObject.Find("Setup").transform;
        Transform editor = setup.Find("Editor").transform;
        Transform placeholder = editor.Find("Placeholder").transform;
        Transform cameraTransform = editor.Find("Camera").transform;

        Camera camera = cameraTransform.GetComponent<Camera>();

        Texture2D texture = new Texture2D(camera.targetTexture.width, camera.targetTexture.height, TextureFormat.RGB24, false, false);

        foreach (GameObject gameObject in Tools.GetPrefabs())
        {
            GameObject instance = Instantiate(gameObject, placeholder);

            camera.Render();

            RenderTexture.active = camera.targetTexture;
            texture.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);

            byte[] bytes = texture.EncodeToPNG();
            string filename = Tools.GetPortraitPath(gameObject.name);
            File.WriteAllBytes(filename, bytes);

            Debug.Log(string.Format("Saving portrait to {0}", filename));

            DestroyImmediate(instance);
        }

        RenderTexture.active = null;

        Tools.RestoreScene(sceneName);
    }

    private static void AlignGameObject(MyGameObject myGameObject, bool yAxis = false)
    {
        RaycastHit[] hits = Utils.RaycastAllFromTop(myGameObject.Position, Utils.GetMapMask(), RaycastSortOrder.Descending);

        foreach (RaycastHit hitInfo in hits)
        {
            if (hitInfo.transform.gameObject == myGameObject)
            {
                continue;
            }

            myGameObject.Position = Utils.SnapToGrid(hitInfo.point, myGameObject.SizeGrid, Config.Map.Scale, yAxis);

            break;
        }
    }
}
