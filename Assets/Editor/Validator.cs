using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Validator : EditorWindow
{
    [MenuItem("Validator/Validate Resources")]
    public static void DoSomething()
    {
        ClearLog();

        GameObject[] disasters = Resources.LoadAll<GameObject>(Config.Asset.Disasters);
        GameObject[] plants = Resources.LoadAll<GameObject>(Config.Asset.Plants);
        GameObject[] rocks = Resources.LoadAll<GameObject>(Config.Asset.Rocks);
        GameObject[] structures = Resources.LoadAll<GameObject>(Config.Asset.Structures);
        GameObject[] units = Resources.LoadAll<GameObject>(Config.Asset.Units);

        CheckPhysics(disasters);
        CheckPhysics(plants);
        CheckPhysics(rocks);
        CheckPhysics(structures);
        CheckPhysics(units);

        CheckStorage(disasters);
        CheckStorage(plants);
        CheckStorage(rocks);
        CheckStorage(structures);
        CheckStorage(units);
    }

    private static void CheckPhysics(GameObject[] resources)
    {
        foreach (GameObject gameObject in resources)
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
            }
        }
    }

    private static void CheckStorage(GameObject[] resources)
    {
        foreach (GameObject gameObject in resources)
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
