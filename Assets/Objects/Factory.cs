using UnityEngine;

public class Factory : MyGameObject
{
    void Start()
    {
        Metal = 0;
    }

    void Update()
    {
        Metal += 10 * Time.deltaTime;
    }

    public float Metal { get; set; }
}
