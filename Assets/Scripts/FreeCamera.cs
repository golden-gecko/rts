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

        // Do not allow camera to go below terrain.
        /*
        RaycastHit raycastHit = new RaycastHit();

        bool hit = Physics.Raycast(
            transform.position + Vector3.up * 10.0f,
            Vector3.down,
            out raycastHit,
            Mathf.Infinity,
            1 << 10
        );

        if (hit)
        {
            Vector3 position = GetComponent<Camera>().transform.position;
            position.y = raycastHit.point.y + height;
            GetComponent<Camera>().transform.position = position;
        }
        */
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
