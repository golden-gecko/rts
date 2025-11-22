using UnityEngine;

public class Behaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        Parent = GetComponent<MyGameObject>();
    }

    protected MyGameObject Parent { get; private set; }
}
