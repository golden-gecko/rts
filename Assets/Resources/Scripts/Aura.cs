using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{
    protected virtual void Awake()
    {
        transform.localScale = Vector3.one * Range;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        MyGameObject myGameObject;

        if (other.transform.TryGetComponent<MyGameObject>(out myGameObject))
        {
            InRange.Add(myGameObject);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        MyGameObject myGameObject;

        if (other.transform.TryGetComponent<MyGameObject>(out myGameObject))
        {
            InRange.Remove(myGameObject);
        }
    }

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;

    protected HashSet<MyGameObject> InRange = new HashSet<MyGameObject>();
}
