using UnityEngine;

public class Factory : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Extract);

        Resources.Add("Metal", new Resource("Metal", 0));
    }

    protected override void Update()
    {
        base.Update();

        Resources["Metal"].Value += 10 * Time.deltaTime;
    }
}
