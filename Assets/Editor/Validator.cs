using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Validator : EditorWindow
{
    [MenuItem("Validator/Validate Resources")]
    public static void DoSomething()
    {
        ClearLog();

        CheckPhysics(ConfigPrefabs.Instance.Disasters);
        // CheckPhysics(plants);
        // CheckPhysics(rocks);
        CheckPhysics(ConfigPrefabs.Instance.Structures);
        CheckPhysics(ConfigPrefabs.Instance.Units);

        CheckStorage(ConfigPrefabs.Instance.Disasters);
        // CheckStorage(plants);
        // CheckStorage(rocks);
        CheckStorage(ConfigPrefabs.Instance.Structures);
        CheckStorage(ConfigPrefabs.Instance.Units);
    }

    private static void CheckPhysics(List<GameObject> gameObjects)
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

    private static void CheckStorage(List<GameObject> gameObjects)
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

    private static void ClearLog()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(Editor));
        Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo method = type.GetMethod("Clear");

        method.Invoke(new object(), null);
    }
}
