using UnityEngine;

public class MyCamera : MonoBehaviour
{
    void Start()
    {
        Speed = 10;
    }

    void Update()
    {
        var speed = Speed* Time.deltaTime;
        var movement = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.D))
        {
            movement.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            movement.z = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement.z = -1;
        }

        transform.position = transform.position + transform.TransformDirection(movement * speed);
    }

    public float Speed { get; set; }
}
