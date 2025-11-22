using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameObjectRenderer : EditorWindow
{
    [MenuItem("Tools/Render", false, 2)]
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
}
