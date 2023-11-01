using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum ECharacterClass
{
    Knight, Ranger, Wizard
}

[CreateAssetMenu] //This adds an entry to the **Create** menu
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public ECharacterClass Class;
    public Sprite PortraitImage;
}

public class CharacterListController
{
    // UXML template for list entries
    VisualTreeAsset ListEntryTemplate;

    // UI element references
    ListView CharacterList;
    Label CharClassLabel;
    Label CharNameLabel;
    VisualElement CharPortrait;

    public void InitializeCharacterList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllCharacters();

        // Store a reference to the template for the list entries
        ListEntryTemplate = listElementTemplate;

        // Store a reference to the character list element
        CharacterList = root.Q<ListView>("Parts");

        // Store references to the selected character info elements
        CharClassLabel = root.Q<Label>("character-class");
        CharNameLabel = root.Q<Label>("character-name");
        CharPortrait = root.Q<VisualElement>("character-portrait");

        FillCharacterList();

        // Register to get a callback when an item is selected
        CharacterList.onSelectionChange += OnCharacterSelected;
    }

    List<CharacterData> AllCharacters;

    void EnumerateAllCharacters()
    {
        AllCharacters = new List<CharacterData>();
        AllCharacters.Add(new CharacterData { CharacterName = "abc" });
        AllCharacters.Add(new CharacterData { CharacterName = "dfd" });
        AllCharacters.Add(new CharacterData { CharacterName = "asd" });
        AllCharacters.Add(new CharacterData { CharacterName = "qwe" });
        AllCharacters.Add(new CharacterData { CharacterName = "123" });
    }

    void FillCharacterList()
    {
        // Set up a make item function for a list entry
        CharacterList.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = ListEntryTemplate.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new CharacterListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        CharacterList.bindItem = (item, index) =>
        {
            (item.userData as CharacterListEntryController).SetCharacterData(AllCharacters[index]);
        };

        // Set a fixed item height
        CharacterList.fixedItemHeight = 45;

        // Set the actual item's source list/array
        CharacterList.itemsSource = AllCharacters;
    }

    void OnCharacterSelected(IEnumerable<object> selectedItems)
    {
        // Get the currently selected item directly from the ListView
        var selectedCharacter = CharacterList.selectedItem as CharacterData;

        // Handle none-selection (Escape to deselect everything)
        if (selectedCharacter == null)
        {
            // Clear
            /*
            CharClassLabel.text = "";
            CharNameLabel.text = "";
            CharPortrait.style.backgroundImage = null;
            */

            return;
        }

        // Fill in character details
        /*
        CharClassLabel.text = selectedCharacter.Class.ToString();
        CharNameLabel.text = selectedCharacter.CharacterName;
        CharPortrait.style.backgroundImage = new StyleBackground(selectedCharacter.PortraitImage);
        */
    }
}




public class CharacterListEntryController
{
    Label NameLabel;

    //This function retrieves a reference to the 
    //character name label inside the UI element.

    public void SetVisualElement(VisualElement visualElement)
    {
        NameLabel = visualElement.Q<Label>("Name");
    }

    //This function receives the character whose name this list 
    //element displays. Since the elements listed 
    //in a `ListView` are pooled and reused, it's necessary to 
    //have a `Set` function to change which character's data to display.

    public void SetCharacterData(CharacterData characterData)
    {
        NameLabel.text = characterData.CharacterName;
    }
}


public class MenuEditor : UI_Element<MenuEditor>
{
    protected override void Awake()
    {
        base.Awake();

        Blueprints = Root.Q<VisualElement>("Blueprints");
        Parts = Root.Q<ListView>("Parts");
        Preview = Root.Q<VisualElement>("Preview");

        ButtonOK = Root.Q<Button>("OK");
        ButtonOK.RegisterCallback<ClickEvent>(ev => OnButtonOK());

        ButtonCancel = Root.Q<Button>("Cancel");
        ButtonCancel.RegisterCallback<ClickEvent>(ev => OnButtonCancel());


        var characterListController = new CharacterListController();
        characterListController.InitializeCharacterList(Root, TemplatePart);
    }

    private void Start()
    {
        foreach (GameObject part in ConfigPrefabs.Instance.Drives)
        {
            TemplateContainer container = TemplatePart.Instantiate();
            Label name = container.Q<Label>("Name");
            Button preview = container.Q<Button>("Preview");

            name.text = part.name;
            // preview.style.backgroundImage = "";

            //Parts.Add(container);
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
