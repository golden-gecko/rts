using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void Update()
    {
        Translate();
        Rotate();
    }

    void Translate()
    {
        var direction = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            direction.z = Speed.z;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.z = -Speed.z;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            direction.y = Speed.y;
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            direction.y = -Speed.y;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction.x = Speed.x;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction.x = -Speed.x;
        }

        transform.Translate(direction * Time.deltaTime);
    }

    void Rotate()
    {
        if (Input.GetMouseButton(2))
        {
            Rotation.x = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Sensitivity.x;
            Rotation.y += Input.GetAxis("Mouse Y") * Sensitivity.y;

            transform.localEulerAngles = new Vector3(-Rotation.y, Rotation.x, 0);
        }
    }

    Vector3 Speed { get; } = new Vector3(10, 10, 10);

    Vector2 Rotation = new Vector2(0, 0);

    Vector2 Sensitivity { get; } = new Vector2(3, 3);
}
