using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuEditor : UI_Element<MenuEditor>
{
    protected override void Awake()
    {
        base.Awake();

        Root.Q<Label>("Header").text = "Editor";

        PartsChassis = Root.Q<ListView>("PartsChassis");
        PartsChassis.selectionChanged += (IEnumerable<object> objects) => OnSelectionChanged(PartType.Chassis, objects);

        PartsDrive = Root.Q<ListView>("PartsDrive");
        PartsDrive.selectionChanged += (IEnumerable<object> objects) => OnSelectionChanged(PartType.Drive, objects);

        PartsGun = Root.Q<ListView>("PartsGun");
        PartsGun.selectionChanged += (IEnumerable<object> objects) => OnSelectionChanged(PartType.Gun, objects);

        Preview = Root.Q<VisualElement>("Preview");
        Preview.RegisterCallback<MouseEnterEvent>(x => OnMouseEnterEvent());
        Preview.RegisterCallback<MouseLeaveEvent>(x => OnMouseLeaveEvent());

        ButtonPrevious = Root.Q<Button>("Previous");
        ButtonPrevious.RegisterCallback<ClickEvent>(x => OnButtonPrevious());

        ButtonNext = Root.Q<Button>("Next");
        ButtonNext.RegisterCallback<ClickEvent>(x => OnButtonNext());

        ButtonClose = Root.Q<Button>("Close");
        ButtonClose.RegisterCallback<ClickEvent>(x => OnButtonClose());

        CreatePartList(PartsChassis, ConfigPrefabs.Instance.Chassis);
        CreatePartList(PartsDrive, ConfigPrefabs.Instance.Drives);
        CreatePartList(PartsGun, ConfigPrefabs.Instance.Guns);

        PartsSelected.Add(PartType.Chassis, null);
        PartsSelected.Add(PartType.Drive, null);
        PartsSelected.Add(PartType.Gun, null);
    }

    private void Update()
    {
        if (Visible && MouseInside && Input.GetMouseButton(0))
        {
            Transform setup = GameObject.Find("Setup").transform;
            Transform editor = setup.Find("Editor").transform;
            Transform placeholder = editor.Find("Placeholder").transform;

            placeholder.Rotate(Vector3.forward, Input.GetAxis("Mouse Y") * Config.Editor.RotateSpeed, Space.World);
            placeholder.Rotate(Vector3.down, Input.GetAxis("Mouse X") * Config.Editor.RotateSpeed, Space.World);
        }
    }

    private void OnSelectionChanged(PartType type, IEnumerable<object> objects)
    {
        if (objects.Count() > 0)
        {
            PartsSelected[type] = objects.First() as GameObject;
        }

        BuildGameObject();
    }

    private void OnMouseEnterEvent()
    {
        MouseInside = true;
    }

    private void OnMouseLeaveEvent()
    {
        MouseInside = false;
    }

    private void OnButtonPrevious()
    {
        switch (PartsListVisible)
        {
            case PartType.Drive:
                PartsListVisible = PartType.Chassis;

                PartsChassis.style.display = DisplayStyle.Flex;
                PartsDrive.style.display = DisplayStyle.None;
                break;

            case PartType.Gun:
                PartsListVisible = PartType.Drive;

                PartsDrive.style.display = DisplayStyle.Flex;
                PartsGun.style.display = DisplayStyle.None;
                break;
        }
    }

    private void OnButtonNext()
    {
        switch (PartsListVisible)
        {
            case PartType.Chassis:
                PartsListVisible = PartType.Drive;

                PartsChassis.style.display = DisplayStyle.None;
                PartsDrive.style.display = DisplayStyle.Flex;
                break;

            case PartType.Drive:
                PartsListVisible = PartType.Gun;

                PartsDrive.style.display = DisplayStyle.None;
                PartsGun.style.display = DisplayStyle.Flex;
                break;
        }
    }

    private void OnButtonClose()
    {
        Show(false);
    }

    private void CreatePartList(ListView listView, List<GameObject> parts)
    {
        listView.makeItem = () =>
        {
            return TemplatePart.Instantiate();
        };

        listView.bindItem = (VisualElement item, int index) =>
        {
            item.Q<Label>("Name").text = parts[index].name;
            item.Q<VisualElement>("Image").style.backgroundImage = new StyleBackground(Utils.LoadPortrait(parts[index].name));

            item.userData = parts[index];
        };

        listView.itemsSource = parts;
    }

    private void BuildGameObject()
    {
        Transform setup = GameObject.Find("Setup").transform;
        Transform editor = setup.Find("Editor").transform;
        Transform placeholder = editor.Find("Placeholder").transform;

        foreach (KeyValuePair<PartType, GameObject> i in PartsInstantiated)
        {
            if (i.Value == null)
            {
                continue;
            }

            Destroy(i.Value.gameObject);
        }

        foreach (KeyValuePair<PartType, GameObject> i in PartsSelected)
        {
            if (i.Value == null)
            {
                continue;
            }

            PartsInstantiated[i.Key] = Instantiate(i.Value, placeholder);
        }

        PartsInstantiated.TryGetValue(PartType.Chassis, out GameObject chassis);
        PartsInstantiated.TryGetValue(PartType.Drive, out GameObject drive);
        PartsInstantiated.TryGetValue(PartType.Gun, out GameObject gun);

        // Position chassis.
        if (chassis && drive)
        {
            Quaternion rotation = Utils.ResetRotation(placeholder);

            Vector3 position = chassis.transform.position;
            position.y = drive.GetComponentInChildren<Collider>().bounds.max.y - drive.GetComponentInChildren<Collider>().bounds.extents.y;
            chassis.transform.position = position;

            Utils.RestoreRotation(placeholder, rotation);
        }

        // Position gun.
        if (chassis && gun)
        {
            Quaternion rotation = Utils.ResetRotation(placeholder);

            Vector3 position = gun.transform.position;
            position.y = chassis.GetComponent<Collider>().bounds.max.y;
            gun.transform.position= position;

            Utils.RestoreRotation(placeholder, rotation);
        }
    }

    [SerializeField]
    private VisualTreeAsset TemplatePart;

    private ListView PartsChassis;
    private ListView PartsDrive;
    private ListView PartsGun;

    private VisualElement Preview;
    private bool MouseInside = false;

    private Button ButtonPrevious;
    private Button ButtonNext;
    private Button ButtonClose;

    private PartType PartsListVisible = PartType.Chassis;
    private Dictionary<PartType, GameObject> PartsSelected = new Dictionary<PartType, GameObject>();
    private Dictionary<PartType, GameObject> PartsInstantiated = new Dictionary<PartType, GameObject>();
}
