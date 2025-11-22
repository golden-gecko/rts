using UnityEngine;

public class Engine : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}, Speed: {1:0.}", base.GetInfo(), Speed);
    }

    [field: SerializeField]
    public float Power { get; set; } = 10.0f;

    public float Speed { get => Power; } // TODO: Include mass here.
}
