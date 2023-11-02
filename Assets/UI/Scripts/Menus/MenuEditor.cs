using UnityEngine;
using UnityEngine.UIElements;

public class MenuEditor : UI_Element<MenuEditor>
{
    protected override void Awake()
    {
        base.Awake();

        Root.Q<Label>("Header").text = "Editor";

        Parts = Root.Q<ListView>("Parts");
        Preview = Root.Q<VisualElement>("Preview");

        ButtonOK = Root.Q<Button>("OK");
        ButtonOK.RegisterCallback<ClickEvent>(ev => OnButtonOK());

        ButtonCancel = Root.Q<Button>("Cancel");
        ButtonCancel.RegisterCallback<ClickEvent>(ev => OnButtonCancel());

        CreateChasisList();
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

    private void CreateChasisList()
    {
        Parts.makeItem = () =>
        {
            return TemplatePart.Instantiate();
        };

        Parts.bindItem = (VisualElement item, int index) =>
        {
            item.Q<Label>("Name").text = ConfigPrefabs.Instance.Chassis[index].name;
            item.userData = ConfigPrefabs.Instance.Chassis[index];
        };

        Parts.itemsSource = ConfigPrefabs.Instance.Chassis;
    }

    private void CreateDriveList()
    {
    }

    private void CreateGunList()
    {
    }

    [SerializeField]
    private VisualTreeAsset TemplatePart;

    private ListView Parts;
    private VisualElement Preview;

    private Button ButtonOK;
    private Button ButtonCancel;
}
