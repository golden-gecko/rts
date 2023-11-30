using System;
using System.Collections.Generic;
using UnityEngine;

public class Part : MyMonoBehaviour
{
    protected virtual void Awake()
    {
        Parent = GetComponentInParent<MyGameObject>();

        InitializeJoints();
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
        return string.Format("Mass: {0}", Mass);
    }

    public bool TryGetJoint(PartType partType, out Transform transform)
    {
        return Joints.TryGetValue(partType, out transform);
    }

    private void InitializeJoints()
    {
        Transform jointsTransform = transform.Find("Joints"); // TODO: Hide outside editor.

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
    public ResourceContainer ConstructionResources { get; private set; } = new ResourceContainer();

    public MyGameObject Parent { get; private set; }

    public int Mass { get => ConstructionResources.MaxSum; }

    public Progress Health { get => new Progress(ConstructionResources.CurrentSum, ConstructionResources.MaxSum); }

    public bool Alive { get => Health.Empty == false; }

    private Dictionary<PartType, Transform> Joints = new Dictionary<PartType, Transform>();
}
