using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    void Update()
    {
        Translate();
        Rotate();
    }

    void Translate()
    {
        var direction = Vector3.zero;

        if (Input.GetKey(KeyCode.D))
        {
            direction.x = Speed.x;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction.x = -Speed.x;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            direction.y = Speed.y;
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            direction.y = -Speed.y;
        }

        if (Input.GetKey(KeyCode.W))
        {
            direction.z = Speed.z;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.z = -Speed.z;
        }

        transform.Translate(direction * Time.deltaTime);
    }

    void Rotate()
    {
        if (Input.GetMouseButton(2))
        {
            var localEulerAngles = transform.localEulerAngles;

            localEulerAngles.x -= Input.GetAxis("Mouse Y") * Sensitivity.x;
            localEulerAngles.y += Input.GetAxis("Mouse X") * Sensitivity.y;

            transform.localEulerAngles = localEulerAngles;
        }
    }

    Vector3 Speed = new Vector3(10, 10, 10);

    Vector2 Sensitivity = new Vector2(3, 3);
}
