using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private void Rotate()
    {
        if (Input.GetMouseButton(2))
        {
            Vector3 localEulerAngles = transform.localEulerAngles;

            localEulerAngles.x -= Input.GetAxis("Mouse Y") * Sensitivity.x;
            localEulerAngles.y += Input.GetAxis("Mouse X") * Sensitivity.y;

            transform.localEulerAngles = localEulerAngles;
        }
    }

    private void Translate()
    {
        Vector3 direction = Vector3.zero;

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

    private void Update()
    {
        Translate();
        Rotate();
    }

    private Vector3 Speed = new Vector3(10.0f, 10.0f, 10.0f);

    private Vector2 Sensitivity = new Vector2(3.0f, 3.0f);
}
