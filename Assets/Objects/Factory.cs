using UnityEngine;

public class Factory : MyGameObject
{
    void Start()
    {
    }

    void Update()
    {
        Metal += 10 * Time.deltaTime;
    }

    public float Metal { get => _metal; set => _metal = value; }

    [SerializeField]
    private float _metal = 0;
}
