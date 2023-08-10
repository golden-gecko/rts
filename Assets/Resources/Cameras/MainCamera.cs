using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private void Rotate()
    {
        if (Input.GetMouseButton(2))
        {
            Vector3 localEulerAngles = transform.localEulerAngles;

            localEulerAngles.x -= Input.GetAxis("Mouse Y") * sensitivity.y;
            localEulerAngles.y += Input.GetAxis("Mouse X") * sensitivity.x;

            transform.localEulerAngles = localEulerAngles;
        }
    }

    private void Translate()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.D))
        {
            direction.x = speed.x;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction.x = -speed.x;
        }

        if (Input.GetKey(KeyCode.E))
        {
            direction.y = speed.y;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            direction.y = -speed.y;
        }

        if (Input.GetKey(KeyCode.W))
        {
            direction.z = speed.z;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.z = -speed.z;
        }

        transform.Translate(direction * Time.deltaTime);
    }

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

        AlignPositionToTerrain();
    }

    private void AlignPositionToTerrain()
    {
        RaycastHit hitInfo;

        if (Map.Instance.GetTerrainPosition(transform.position, out hitInfo))
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, hitInfo.point.y + Config.CameraMinHeight, hitInfo.point.y + Config.CameraMaxHeight), transform.position.z);
        }
    }

    private Vector3 speed = new Vector3(10.0f, 10.0f, 10.0f);

    private Vector2 sensitivity = new Vector2(3.0f, 3.0f);
}
