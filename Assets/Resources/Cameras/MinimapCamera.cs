using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private void Update()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    private void OnCenterOnMainCamera()
    {

    }

    private void OnZoomIn()
    {
    }

    private void OnZoomOut()
    {
    }
}
