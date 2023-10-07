using UnityEngine;

public class MyComponent : MonoBehaviour
{
    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    public virtual string GetInfo()
    {
        return string.Format("Mass: {0:0.}", Mass);
    }

    public virtual void OnDestroy_(MyGameObject myGameObject)
    {
    }

    [field: SerializeField]
    public float Mass { get; set; } = 10.0f;
}
