using UnityEngine;

public class Gun
{
    public Gun(string name, float damage, float range, float reload)
    {
        Name = name;
        Damage = damage;
        Range = range;
        Reload.Max = reload;
    }

    public virtual void Fire(MyGameObject myGameObject, Vector3 position)
    {
    }

    public virtual void Update()
    {
        Reload.Update(Time.deltaTime);
    }

    public virtual string GetInfo()
    {
        return string.Format("Name: {0}, Reload: {0:0.}/{1:0.}", Name, Reload.Current, Reload.Max);
    }

    public string Name { get; }

    public float Damage { get; }

    public float Range { get; }

    public Timer Reload { get; } = new Timer();
}
