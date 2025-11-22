using UnityEngine;

public class struct_Refinery_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Produce);

        Resources.Add("Metal", new Resource("Metal", 0));
        Resources.Add("Wood", new Resource("Wood", 0));

    }

    protected override void Update()
    {
        base.Update();

        Resources["Metal"].Value += 2 * Time.deltaTime;
        Resources["Wood"].Value += 8 * Time.deltaTime;
    }
}
