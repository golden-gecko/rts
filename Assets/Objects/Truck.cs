using UnityEngine;

public class Unit : MonoBehaviour
{
    void Start()
    {
        target = new Vector3(10, 0, 10);
    }

    void Update()
    {
        transform.Translate((target - transform.position).normalized * 10 * Time.deltaTime);
    }

    private Vector3 target;
}
