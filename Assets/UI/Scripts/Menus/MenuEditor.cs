using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuEditor : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        Root.Q<Label>("Header").text = "Editor";

        Blueprints = Root.Q<DropdownField>("Blueprints");
        Blueprints.RegisterValueChangedCallback(x => OnBlueprintsChange(x.newValue));

        ButtonDelete = Root.Q<Button>("Delete");
        ButtonDelete.RegisterCallback<ClickEvent>(x => OnButtonDelete());

        Name = Root.Q<TextField>("Name");

        ButtonSave = Root.Q<Button>("Save");
        ButtonSave.RegisterCallback<ClickEvent>(x => OnButtonSave());

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
    }

    private void Start()
    {
        LoadBlueprints();
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

    private void OnBlueprintsChange(string name)
    {
        if (name.Length <= 0)
        {
            return;
        }

        Blueprint newBlueprint = BlueprintManager.Instance.Get(name);

        if (newBlueprint == null)
        {
            return;
        }

        foreach (BlueprintComponent i in blueprint.Parts)
        {
            Destroy(i.Instance);
        }

        blueprint = newBlueprint.Clone() as Blueprint;

        BuildGameObjectFromBlueprint();
    }

    private void OnButtonDelete()
    {
        string name = Blueprints.text;

        if (name.Length <= 0)
        {
            return;
        }

        Blueprints.choices.Remove(name);
        Blueprints.choices.Sort();
        Blueprints.index = Blueprints.choices.Count - 1;

        File.Delete(Path.Join(Config.Blueprints.Directory, string.Format("{0}.json", name)));
    }

    private void OnButtonSave()
    {
        string name = Name.text;

        if (name.Length <= 0)
        {
            return;
        }

        int index = Blueprints.choices.FindIndex(x => x == name);

        if (index < 0)
        {
            Blueprints.choices.Add(name);
            Blueprints.choices.Sort();
        }

        Blueprints.index = Blueprints.choices.FindIndex(x => x == name);

        blueprint.Name = name;

        BlueprintManager.Instance.Save(blueprint.Clone() as Blueprint);

        SaveBlueprintToFile(blueprint);
    }

    private void OnSelectionChanged(PartType partType, IEnumerable<object> objects)
    {
        if (objects.Count() <= 0)
        {
            return;
        }

        BlueprintComponent component = blueprint.Parts.Find(x => x.PartType == partType);

        if (component != null)
        {
            Destroy(component.Instance);
        }

        GameObject part = objects.First() as GameObject;

        blueprint.Parts.Remove(component);
        blueprint.Parts.Add(new BlueprintComponent { PartType = partType, Name = part.name, Part = part });

        BuildGameObjectFromBlueprint();
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
        UI.Instance.GoToMenu(MenuType.Game);
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
        };

        listView.itemsSource = parts;
    }

    private void BuildGameObjectFromBlueprint()
    {
        Transform setup = GameObject.Find("Setup").transform;
        Transform editor = setup.Find("Editor").transform;
        Transform placeholder = editor.Find("Placeholder").transform;

        foreach (BlueprintComponent i in blueprint.Parts)
        {
            Destroy(i.Instance);
        }

        foreach (BlueprintComponent i in blueprint.Parts)
        {
            if (i.Part == null)
            {
                i.Part = LoadPart(i.PartType, i.Name);
            }

            i.Instance = Instantiate(i.Part, placeholder);
        }

        PositionChassis(placeholder);
        PositionGun(placeholder);

        SavePosition();
    }

    private void PositionChassis(Transform parent)
    {
        BlueprintComponent chassis = blueprint.Parts.Find(x => x.PartType == PartType.Chassis);
        BlueprintComponent drive = blueprint.Parts.Find(x => x.PartType == PartType.Drive);

        if (chassis != null && drive != null)
        {
            Quaternion rotation = Utils.ResetRotation(parent);

            Vector3 position = chassis.Instance.transform.position;
            position.y = drive.Instance.GetComponentInChildren<Collider>().bounds.max.y - drive.Instance.GetComponentInChildren<Collider>().bounds.extents.y;
            chassis.Instance.transform.position = position;

            Utils.RestoreRotation(parent, rotation);
        }
    }

    private void PositionGun(Transform parent)
    {
        BlueprintComponent chassis = blueprint.Parts.Find(x => x.PartType == PartType.Chassis);
        BlueprintComponent gun = blueprint.Parts.Find(x => x.PartType == PartType.Gun);

        if (chassis != null && gun != null)
        {
            Quaternion rotation = Utils.ResetRotation(parent);

            Vector3 position = gun.Instance.transform.position;
            position.y = chassis.Instance.GetComponent<Collider>().bounds.max.y;
            gun.Instance.transform.position = position;

            Utils.RestoreRotation(parent, rotation);
        }
    }

    private void SavePosition()
    {
        BlueprintComponent chassis = blueprint.Parts.Find(x => x.PartType == PartType.Chassis);
        BlueprintComponent drive = blueprint.Parts.Find(x => x.PartType == PartType.Drive);
        BlueprintComponent gun = blueprint.Parts.Find(x => x.PartType == PartType.Gun);

        if (chassis != null)
        {
            chassis.Position = chassis.Instance.transform.localPosition;
        }

        if (drive != null)
        {
            drive.Position = drive.Instance.transform.localPosition;
        }

        if (gun != null)
        {
            gun.Position = gun.Instance.transform.localPosition;
        }
    }

    private void LoadBlueprints()
    {
        Blueprints.choices = BlueprintManager.Instance.Blueprints.Keys.ToList();
    }

    private void SaveBlueprintToFile(Blueprint blueprint)
    {
        string directory = Config.Blueprints.Directory;
        string path = Path.Join(directory, string.Format("{0}.json", blueprint.Name));
        string json = JsonUtility.ToJson(blueprint, true);

        Directory.CreateDirectory(directory);
        File.WriteAllText(path, json);
    }

    private GameObject LoadPart(PartType partType, string name)
    {
        switch (partType)
        {
            case PartType.Chassis:
                return ConfigPrefabs.Instance.Chassis.Find(x => x.name == name);

            case PartType.Drive:
                return ConfigPrefabs.Instance.Drives.Find(x => x.name == name);

            case PartType.Gun:
                return ConfigPrefabs.Instance.Guns.Find(x => x.name == name);
        }

        return null;
    }

    [SerializeField]
    private VisualTreeAsset TemplatePart;

    private DropdownField Blueprints;
    private Button ButtonDelete;
    private TextField Name;
    private Button ButtonSave;

    private ListView PartsChassis;
    private ListView PartsDrive;
    private ListView PartsGun;

    private VisualElement Preview;
    private bool MouseInside = false;

    private Button ButtonPrevious;
    private Button ButtonNext;
    private Button ButtonClose;

    private PartType PartsListVisible = PartType.Chassis;
    private Blueprint blueprint = new Blueprint();
}
