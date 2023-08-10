using UnityEngine;

public class Gun : MyComponent
{
    public Gun(MyGameObject parent, string name, float damage, float range, float reload) : base(parent, name)
    {
        Damage = damage;
        Range = range;
        Reload.Max = reload;
    }

    public virtual void Fire(MyGameObject myGameObject, Vector3 position)
    {
    }

    public override void Update()
    {
        Reload.Update(Time.deltaTime);
    }

    public override string GetInfo()
    {
        return string.Format("Name: {0}, Reload: {1:0.}/{2:0.}", Name, Reload.Current, Reload.Max);
    }

    public float Damage { get; }

    public float Range { get; }

    public Timer Reload { get; } = new Timer();

    public string MissilePrefab { get; set; } = string.Empty; // TODO: Hide setter.
}
