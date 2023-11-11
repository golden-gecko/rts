using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    }

    private void Start()
    {
        CreatePartList(PartsChassis, Game.Instance.Config.Chassis);
        CreatePartList(PartsDrive, Game.Instance.Config.Drives);
        CreatePartList(PartsGun, Game.Instance.Config.Guns);

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

    private void Refresh()
    {
        UI.Instance.GetComponentInChildren<UI_Commands_Prefabs>().Refresh(); // TODO: Refactor.

        foreach (Assembler assembler in FindObjectsByType<Assembler>(FindObjectsInactive.Include, FindObjectsSortMode.None)) // TODO: Refactor.
        {
            assembler.Parent.Orders.PrefabWhitelist.Clear();

            foreach (string prefab in assembler.Prefabs)
            {
                assembler.Parent.Orders.AllowPrefab(prefab);
            }

            foreach (string prefab in Game.Instance.BlueprintManager.Blueprints.Keys)
            {
                assembler.Parent.Orders.AllowPrefab(prefab);
            }
        }
    }

    private void OnBlueprintsChange(string name)
    {
        if (name.Length <= 0)
        {
            return;
        }

        Blueprint newBlueprint = Game.Instance.BlueprintManager.Get(name);

        if (newBlueprint == null)
        {
            return;
        }

        DestroyGameObjectFromBlueprint();

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

        Game.Instance.BlueprintManager.Delete(name);

        Refresh();
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

        Game.Instance.BlueprintManager.Save(blueprint.Clone() as Blueprint);

        blueprint.Save();

        Refresh();
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
            if (parts[index])
            {
                item.Q<Label>("Name").text = parts[index].name;
                item.Q<VisualElement>("Image").style.backgroundImage = new StyleBackground(Utils.LoadPortrait(parts[index].name));
            }
            else
            {
                item.Q<Label>("Name").text = "None";
            }
        };

        listView.itemsSource = parts;
    }

    private void BuildGameObjectFromBlueprint()
    {
        DestroyGameObjectFromBlueprint();

        Transform setup = GameObject.Find("Setup").transform;
        Transform editor = setup.Find("Editor").transform;
        Transform placeholder = editor.Find("Placeholder").transform;

        blueprint.BaseGameObject = Utils.CreateGameObject(blueprint, placeholder.transform.position, Quaternion.identity, null, MyGameObjectState.Preview, placeholder);

        PositionChassis(placeholder);
        PositionGun(placeholder);

        SavePosition();
    }

    private void DestroyGameObjectFromBlueprint()
    {
        if (blueprint.BaseGameObject)
        {
            Destroy(blueprint.BaseGameObject.gameObject);
        }
    }

    private void PositionChassis(Transform parent)
    {
        BlueprintComponent chassis = blueprint.Parts.Find(x => x.PartType == PartType.Chassis);
        BlueprintComponent drive = blueprint.Parts.Find(x => x.PartType == PartType.Drive);

        if (chassis != null && chassis.Instance != null && drive != null && drive.Instance != null)
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

        if (chassis != null && chassis.Instance != null && gun != null && gun.Instance != null)
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

        if (chassis != null && chassis.Instance != null)
        {
            chassis.Position = chassis.Instance.transform.localPosition;
        }

        if (drive != null && drive.Instance != null)
        {
            drive.Position = drive.Instance.transform.localPosition;
        }

        if (gun != null && gun.Instance != null)
        {
            gun.Position = gun.Instance.transform.localPosition;
        }
    }

    private void LoadBlueprints()
    {
        Blueprints.choices = Game.Instance.BlueprintManager.Blueprints.Keys.ToList();
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
