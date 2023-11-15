using System;
using System.Collections.Generic;
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

        Preview = Root.Q<VisualElement>("Preview");
        Preview.RegisterCallback<MouseEnterEvent>(x => OnMouseEnterEvent());
        Preview.RegisterCallback<MouseLeaveEvent>(x => OnMouseLeaveEvent());

        Info = Root.Q<Label>("Info");
        Info.text = string.Empty;

        ButtonPrevious = Root.Q<Button>("Previous");
        ButtonPrevious.RegisterCallback<ClickEvent>(x => OnButtonPrevious());

        ButtonNext = Root.Q<Button>("Next");
        ButtonNext.RegisterCallback<ClickEvent>(x => OnButtonNext());

        ButtonClose = Root.Q<Button>("Close");
        ButtonClose.RegisterCallback<ClickEvent>(x => OnButtonClose());

        DialogDelete = Root.Q<VisualElement>("DialogDelete");
        DialogDelete.Q<Label>("Header").text = "Delete blueprint?";
        DialogDelete.Q<Button>("Yes").RegisterCallback<ClickEvent>(x => OnButtonDeleteYes());
        DialogDelete.Q<Button>("No").RegisterCallback<ClickEvent>(x => OnButtonDeleteNo());

        DialogOverwrite = Root.Q<VisualElement>("DialogOverwrite");
        DialogOverwrite.Q<Label>("Header").text = "Overwrite blueprint?";
        DialogOverwrite.Q<Button>("Yes").RegisterCallback<ClickEvent>(x => OnButtonOverwriteYes());
        DialogOverwrite.Q<Button>("No").RegisterCallback<ClickEvent>(x => OnButtonOverwriteNo());
    }

    private void Start()
    {
        CreateBlueprints();

        CreatePartList("PartsChassis", PartType.Chassis, Game.Instance.Config.Chassis);
        CreatePartList("PartsDrive", PartType.Drive, Game.Instance.Config.Drives);
        CreatePartList("PartsEngine", PartType.Engine, Game.Instance.Config.Engines);
        CreatePartList("PartsGun", PartType.Gun, Game.Instance.Config.Guns);
        CreatePartList("PartsArm", PartType.Arm, Game.Instance.Config.Arms);
        CreatePartList("PartsShield", PartType.Shield, Game.Instance.Config.Shields);
        CreatePartList("PartsSight", PartType.Sight, Game.Instance.Config.Sights);
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

        Name.value = newBlueprint.Name;
        blueprint = newBlueprint.Clone() as Blueprint;

        BuildGameObjectFromBlueprint();
    }

    private void OnButtonDelete()
    {
        DialogDelete.style.display = DisplayStyle.Flex;
    }

    private void OnButtonSave()
    {
        string name = Name.text;

        if (name.Length <= 0)
        {
            return;
        }

        if (Blueprints.choices.Find(x => x == name) == null)
        {
            SaveBlueprint();
        }
        else
        {
            DialogOverwrite.style.display = DisplayStyle.Flex;
        }
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

        DestroyGameObjectFromBlueprint();
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
        if (VisiblePartsListIndex <= 0)
        {
            return;
        }

        VisiblePartsList[VisiblePartsListIndex].Value.style.display = DisplayStyle.None;
        VisiblePartsListIndex -= 1;
        VisiblePartsList[VisiblePartsListIndex].Value.style.display = DisplayStyle.Flex;
    }

    private void OnButtonNext()
    {
        if (VisiblePartsListIndex >= VisiblePartsList.Count - 1)
        {
            return;
        }

        VisiblePartsList[VisiblePartsListIndex].Value.style.display = DisplayStyle.None;
        VisiblePartsListIndex += 1;
        VisiblePartsList[VisiblePartsListIndex].Value.style.display = DisplayStyle.Flex;
    }

    private void OnButtonClose()
    {
        UI.Instance.GoToMenu(MenuType.Game);
    }

    private void OnButtonDeleteYes()
    {
        string name = Blueprints.text;

        if (name.Length <= 0)
        {
            return;
        }

        Blueprints.choices.Remove(name);
        Blueprints.choices.Sort();
        Blueprints.index = 0;

        Game.Instance.BlueprintManager.Delete(name);

        Refresh();

        DialogDelete.style.display = DisplayStyle.None;
    }

    private void OnButtonDeleteNo()
    {
        DialogDelete.style.display = DisplayStyle.None;
    }

    private void OnButtonOverwriteYes()
    {
        SaveBlueprint();

        DialogOverwrite.style.display = DisplayStyle.None;
    }

    private void OnButtonOverwriteNo()
    {
        DialogOverwrite.style.display = DisplayStyle.None;
    }

    private void CreateBlueprints()
    {
        Blueprints.choices = Game.Instance.BlueprintManager.Blueprints.Select(x => x.Name).ToList();
    }

    private void CreatePartList(string name, PartType partType, List<GameObject> parts)
    {
        List<GameObject> partsWithNoneOption = new List<GameObject>(parts);
        partsWithNoneOption.Insert(0, null);

        ListView listView = Root.Q<ListView>(name);

        listView.selectionChanged += (IEnumerable<object> objects) =>
        {
            OnSelectionChanged(partType, objects);
        };

        listView.makeItem = () =>
        {
            return TemplatePart.Instantiate();
        };

        listView.bindItem = (VisualElement item, int index) =>
        {
            if (partsWithNoneOption[index])
            {
                item.Q<Label>("Name").text = partsWithNoneOption[index].name;
                item.Q<VisualElement>("Image").style.backgroundImage = new StyleBackground(Utils.GetPortrait(partsWithNoneOption[index].name));
            }
            else
            {
                item.Q<Label>("Name").text = "None";
                item.Q<VisualElement>("Image").style.backgroundImage = null;
            }
        };

        listView.itemsSource = partsWithNoneOption;

        VisiblePartsList.Add(new KeyValuePair<PartType, ListView>(partType, listView));
    }

    private void SaveBlueprint()
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

    private void BuildGameObjectFromBlueprint()
    {
        Transform setup = GameObject.Find("Setup").transform;
        Transform editor = setup.Find("Editor").transform;
        Transform placeholder = editor.Find("Placeholder").transform;

        blueprint.BaseGameObject = Utils.CreateGameObject(blueprint, placeholder.transform.position, Quaternion.identity, null, MyGameObjectState.Preview, placeholder);

        PositionChassis(placeholder);
        PositionGun(placeholder);
        PositionArm(placeholder);

        SavePosition();
        UpdateInfo();
    }

    private void DestroyGameObjectFromBlueprint()
    {
        if (blueprint.BaseGameObject)
        {
            Destroy(blueprint.BaseGameObject.gameObject);
        }
    }

    private void PositionArm(Transform parent)
    {
        BlueprintComponent arm = blueprint.Parts.Find(x => x.PartType == PartType.Arm);
        BlueprintComponent chassis = blueprint.Parts.Find(x => x.PartType == PartType.Chassis);

        if (arm != null && arm.Instance != null && chassis != null && chassis.Instance != null)
        {
            Quaternion rotation = Utils.ResetRotation(parent);

            Collider armCollider = arm.Instance.GetComponent<Collider>();
            Collider chassisCollider = chassis.Instance.GetComponent<Collider>();

            Vector3 position = arm.Instance.transform.position;
            position.y = chassisCollider.bounds.min.y + chassisCollider.bounds.extents.y;
            position.z = chassisCollider.bounds.max.z;
            arm.Instance.transform.position = position;

            Utils.RestoreRotation(parent, rotation);
        }
    }

    private void PositionChassis(Transform parent)
    {
        BlueprintComponent chassis = blueprint.Parts.Find(x => x.PartType == PartType.Chassis);
        BlueprintComponent drive = blueprint.Parts.Find(x => x.PartType == PartType.Drive);

        if (chassis != null && chassis.Instance != null && drive != null && drive.Instance != null)
        {
            Quaternion rotation = Utils.ResetRotation(parent);

            Collider driveCollider = drive.Instance.GetComponent<Collider>();

            Vector3 position = chassis.Instance.transform.position;
            position.y = driveCollider.bounds.min.y + driveCollider.bounds.extents.y;
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

            Collider chassisCollider = chassis.Instance.GetComponent<Collider>();

            Vector3 position = gun.Instance.transform.position;
            position.y = chassisCollider.bounds.max.y;
            gun.Instance.transform.position = position;

            Utils.RestoreRotation(parent, rotation);
        }
    }

    private void SavePosition()
    {
        BlueprintComponent arm = blueprint.Parts.Find(x => x.PartType == PartType.Arm);
        BlueprintComponent chassis = blueprint.Parts.Find(x => x.PartType == PartType.Chassis);
        BlueprintComponent drive = blueprint.Parts.Find(x => x.PartType == PartType.Drive);
        BlueprintComponent gun = blueprint.Parts.Find(x => x.PartType == PartType.Gun);
        BlueprintComponent shield = blueprint.Parts.Find(x => x.PartType == PartType.Shield);

        if (arm != null && arm.Instance != null)
        {
            arm.Position = arm.Instance.transform.localPosition;
        }

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

        if (shield != null && shield.Instance != null)
        {
            shield.Position = shield.Instance.transform.localPosition;
        }
    }

    private void UpdateInfo()
    {
        string info = string.Empty;

        foreach (BlueprintComponent i in blueprint.Parts)
        {
            info += string.Format("{0}: {1}\n", Enum.GetName(typeof(PartType), i.PartType), i.Name);
        }

        info += string.Format("\n\nHealth: {0:0.}", blueprint.BaseGameObject.Health.Max);
        info += string.Format("\nMass: {0:0.}", blueprint.BaseGameObject.Mass);

        Armour armour = blueprint.BaseGameObject.GetComponentInChildren<Armour>();

        if (armour)
        {
            info += string.Format("\n\nArmour: {0:0.}", armour.Value.Max);
        }

        Engine engine = blueprint.BaseGameObject.GetComponentInChildren<Engine>();

        if (engine)
        {
            info += string.Format("\n\nRange: {0:0.}", engine.Fuel.Max / engine.FuelUsage);
            info += string.Format("\nSpeed: {0:0.}", engine.Speed);
        }

        Gun gun = blueprint.BaseGameObject.GetComponentInChildren<Gun>();

        if (gun)
        {
            info += string.Format("\n\nAttack damage: {0:0.}", gun.Damage.Value);
            info += string.Format("\nAttack range: {0:0.}", gun.Range.Value);
        }

        Info.text = info;
    }

    [SerializeField]
    private VisualTreeAsset TemplatePart;

    private DropdownField Blueprints;
    private Button ButtonDelete;
    private TextField Name;
    private Button ButtonSave;

    private VisualElement Preview;
    private Label Info;
    private bool MouseInside = false;

    private Button ButtonPrevious;
    private Button ButtonNext;
    private Button ButtonClose;

    private VisualElement DialogOverwrite;
    private VisualElement DialogDelete;

    private List<KeyValuePair<PartType, ListView>> VisiblePartsList = new List<KeyValuePair<PartType, ListView>>();
    private int VisiblePartsListIndex = 0;

    private Blueprint blueprint = new Blueprint();
}
