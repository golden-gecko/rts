using UnityEngine;

public class MainCamera : MonoBehaviour
{
    protected void Update()
    {
        Translate();
        Rotate();

        if (Input.GetKeyDown(KeyCode.Home))
        {
            Vector3 localEulerAngles = transform.localEulerAngles;

            localEulerAngles.y = 275.0f;

            transform.localEulerAngles = localEulerAngles;
        }
        else if (Input.GetKeyDown(KeyCode.Insert))
        {
            Vector3 localEulerAngles = transform.localEulerAngles;

            localEulerAngles.y += 90.0f;

            transform.localEulerAngles = localEulerAngles;
        }
        else if (Input.GetKeyDown(KeyCode.PageUp))
        {
            Vector3 localEulerAngles = transform.localEulerAngles;

            localEulerAngles.y -= 90.0f;

            transform.localEulerAngles = localEulerAngles;
        }

        UpdatePosition();
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

        if (Input.GetKey(KeyCode.E))
        {
            direction.y = Speed.y;
        }
        else if (Input.GetKey(KeyCode.Q))
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

    private void Rotate()
    {
        if (Input.GetMouseButton(2))
        {
            Vector3 localEulerAngles = transform.localEulerAngles;

            localEulerAngles.x -= Input.GetAxis("Mouse Y") * Sensitivity.y;
            localEulerAngles.y += Input.GetAxis("Mouse X") * Sensitivity.x;

            transform.localEulerAngles = localEulerAngles;
        }
    }

    private void UpdatePosition()
    {
        Vector3 position;

        if (Map.Instance.GetPositionForCamera(transform.position, out position, out _))
        {
            Position = new Vector3(Position.x, Mathf.Clamp(Position.y, position.y + Config.CameraMinHeight, position.y + Config.CameraMaxHeight), Position.z);
        }
    }

    public Vector3 Position { get => transform.position; set => transform.position = value; }

    [field: SerializeField]
    public Vector3 Speed { get; set; } = new Vector3(10.0f, 10.0f, 10.0f);

    [field: SerializeField]
    public Vector2 Sensitivity { get; set; } = new Vector2(3.0f, 3.0f);
}
