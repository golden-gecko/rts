using UnityEngine;

public class Part : MonoBehaviour
{
    protected virtual void Awake()
    {
        Parent = GetComponentInParent<MyGameObject>();
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

    [field: SerializeField]
    public Progress Health { get; private set; } = new Progress(10.0f, 10.0f);

    [field: SerializeField]
    public float Mass { get; private set; } = 10.0f;

    [field: SerializeField]
    public ResourceContainer ConstructionResources { get; private set; } = new ResourceContainer();

    public MyGameObject Parent { get; private set; }

    public bool Alive { get => Parent != null && Parent.Player != null && Health.Empty == false; }
}
