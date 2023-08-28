using UnityEngine;

public class Engine : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Power: {1:0.}, Speed: {2:0.}", base.GetInfo(), Power, Speed);
    }

    [field: SerializeField]
    public float Power { get; set; } = 50.0f;

    public float Speed { get => Power / GetComponent<MyGameObject>().Mass; }
}
