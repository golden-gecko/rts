using UnityEngine;
using UnityEngine.UIElements;

public class MenuEditor : UI_Element<MenuEditor>
{
    protected override void Awake()
    {
        base.Awake();

        Blueprints = Root.Q<VisualElement>("Blueprints");
        Parts = Root.Q<VisualElement>("Parts");
        Preview = Root.Q<VisualElement>("Preview");

        ButtonOK = Root.Q<Button>("OK");
        ButtonOK.RegisterCallback<ClickEvent>(ev => OnButtonOK());

        ButtonCancel = Root.Q<Button>("Cancel");
        ButtonCancel.RegisterCallback<ClickEvent>(ev => OnButtonCancel());
    }

    private void Start()
    {
        foreach (GameObject part in ConfigPrefabs.Instance.Drives)
        {
        }

        foreach (GameObject part in ConfigPrefabs.Instance.Guns)
        {
        }

        foreach (GameObject part in ConfigPrefabs.Instance.Shields)
        {
        }
    }

    private void Update()
    {
        if (Visible && Input.GetMouseButton(0))
        {
            GameObject.Find("Setup").transform.Find("Editor").transform.Find("GameObject").Rotate(Input.GetAxis("Mouse Y") * 3.0f, Input.GetAxis("Mouse X") * 3.0f, 0.0f, Space.Self);
        }
    }

    private void OnButtonOK()
    {
        Show(false);
    }

    private void OnButtonCancel()
    {
        Show(false);
    }

    [SerializeField]
    private VisualTreeAsset TemplatePart;

    private VisualElement Blueprints;
    private VisualElement Parts;
    private VisualElement Preview;

    private Button ButtonOK;
    private Button ButtonCancel;
}
