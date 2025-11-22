using UnityEngine;
using UnityEngine.UIElements;

public class MinimapCamera : MonoBehaviour
{
    private void Start()
    {
        cameraComponent = GetComponent<Camera>();

        UIDocument uiDocument = GameMenu.Instance.GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;

        centerOnMainCamera = rootVisualElement.Q<Button>("CenterOnMainCamera");
        centerOnMainCamera.RegisterCallback<ClickEvent>(ev => OnCenterOnMainCamera());

        zoomIn = rootVisualElement.Q<Button>("ZoomIn");
        zoomIn.RegisterCallback<ClickEvent>(ev => OnZoomIn());

        zoomOut = rootVisualElement.Q<Button>("ZoomOut");
        zoomOut.RegisterCallback<ClickEvent>(ev => OnZoomOut());
    }

    private void Update()
    {
        if (CenterOnMainCamera)
        {
            transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        }
    }

    private void OnCenterOnMainCamera()
    {
        CenterOnMainCamera = !CenterOnMainCamera;
    }

    private void OnZoomIn()
    {
        cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize - 10.0f, 10.0f, 100.0f);
    }

    private void OnZoomOut()
    {
        cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize + 10.0f, 10.0f, 100.0f);
    }

    private Camera cameraComponent;

    private Button centerOnMainCamera;
    private Button zoomIn;
    private Button zoomOut;

    private bool CenterOnMainCamera { get; set; } = true;
}
