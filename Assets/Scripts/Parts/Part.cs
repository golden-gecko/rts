using System;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    protected virtual void Awake()
    {
        Parent = GetComponentInParent<MyGameObject>();

        GetJoints();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    public virtual void OnDestroyHandler()
    {
    }

    public virtual string GetInfo()
    {
        return string.Format("Mass: {0:0.}", Mass);
    }

    private void GetJoints()
    {
        Transform jointsTransform = transform.Find("Joints");

        if (jointsTransform == null)
        {
            return;
        }

        for (int i = 0; i < jointsTransform.childCount; i++)
        {
            Transform child = jointsTransform.GetChild(i);

            if (Enum.TryParse(child.name, false, out PartType partType) == false)
            {
                continue;
            }

            Joints[partType] = child;
        }
    }

    [field: SerializeField]
    public float Mass { get; private set; } = 10.0f;

    [field: SerializeField]
    public ResourceContainer ConstructionResources { get; private set; } = new ResourceContainer();

    public Dictionary<PartType, Transform> Joints = new Dictionary<PartType, Transform>();

    public Progress Health { get => new Progress(ConstructionResources.CurrentSum, ConstructionResources.MaxSum); }

    public MyGameObject Parent { get; private set; }

    public bool Alive { get => /* TODO: Parent != null && Parent.Player != null && */ Health.Empty == false; }

    public Vector3 Center { get => new Vector3(Position.x, Position.y + Size.y / 2.0f, Position.z); } 

    public Vector3 Direction { get => transform.forward; }

    public Vector3 Position { get => transform.position; set => transform.position = value; }

    public Quaternion Rotation { get => transform.rotation; set => transform.rotation = value; }

    public Vector3 Size
    {
        get
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            if (colliders.Length <= 0)
            {
                return Vector3.zero;
            }

            Quaternion rotation = Utils.ResetRotation(transform);

            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (Collider collider in colliders)
            {
                min.x = Mathf.Min(min.x, collider.bounds.min.x);
                min.y = Mathf.Min(min.y, collider.bounds.min.y);
                min.z = Mathf.Min(min.z, collider.bounds.min.z);

                max.x = Mathf.Max(max.x, collider.bounds.max.x);
                max.y = Mathf.Max(max.y, collider.bounds.max.y);
                max.z = Mathf.Max(max.z, collider.bounds.max.z);
            }

            Utils.RestoreRotation(transform, rotation);

            return max - min;
        }
    }
}
