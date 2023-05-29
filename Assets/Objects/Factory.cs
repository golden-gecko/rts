using UnityEngine;

public class Factory : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        metal += 10 * Time.deltaTime;
    }

    private float metal = 0;
}
