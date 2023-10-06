using UnityEngine;
using UnityEngine.UIElements;

public class UI_Minimap : MonoBehaviour
{
    void Awake()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;

        panel = rootVisualElement.Q<VisualElement>("Panel_Minimap");

        centerOnMainCamera = panel.Q<Button>("CenterOnMainCamera");
        centerOnMainCamera.RegisterCallback<ClickEvent>(ev => OnCenterOnMainCamera());

        zoomIn = panel.Q<Button>("ZoomIn");
        zoomIn.RegisterCallback<ClickEvent>(ev => OnZoomIn());

        zoomOut = panel.Q<Button>("ZoomOut");
        zoomOut.RegisterCallback<ClickEvent>(ev => OnZoomOut());
    }

    void Update()
    {
        if (CenterOnMainCamera)
        {
            cameraComponent.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        }
    }

    private void OnCenterOnMainCamera()
    {
        if (CenterOnMainCamera)
        {
            CenterOnMainCamera = false;

            centerOnMainCamera.text = "Center (Off)";
        }
        else
        {
            CenterOnMainCamera = true;

            centerOnMainCamera.text = "Center (On)";
        }
    }

    private void OnZoomIn()
    {
        cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize - ZoomStep, MinZoom, MaxZoom);
    }

    private void OnZoomOut()
    {
        cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize + ZoomStep, MinZoom, MaxZoom);
    }

    [field: SerializeField]
    private Camera cameraComponent;

    [field: SerializeField]
    private bool CenterOnMainCamera { get; set; } = true;

    [field: SerializeField]
    private float MinZoom { get; set; } = 10.0f;

    [field: SerializeField]
    private float MaxZoom { get; set; } = 100.0f;

    [field: SerializeField]
    private float ZoomStep { get; set; } = 10.0f;

    private VisualElement panel;

    private Button centerOnMainCamera;
    private Button zoomIn;
    private Button zoomOut;
}
