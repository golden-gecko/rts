using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class GameMenu : Menu
{
    public static GameMenu Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;

        bottomPanel = rootVisualElement.Q<VisualElement>("BottomPanel");
        infoPanel = rootVisualElement.Q<VisualElement>("InfoPanel");

        log = rootVisualElement.Q<Label>("Log");
        info = rootVisualElement.Q<Label>("Info");

        menu = rootVisualElement.Q<Button>("Menu");
        menu.RegisterCallback<ClickEvent>(ev => OnMenu());

        orders = rootVisualElement.Q<VisualElement>("OrderList");
        prefabs = rootVisualElement.Q<VisualElement>("PrefabList");
        technologies = rootVisualElement.Q<VisualElement>("TechnologyList");

        CreateOrders();
        CreatePrefabs();
        CreateTechnologies();

        Log("");
    }

    private void Update()
    {
        Player activePlayer = HUD.Instance.ActivePlayer;

        if (activePlayer.Selected.Count > 0 && activePlayer.Selected[0] != null)
        {
            info.text = activePlayer.Selected[0].GetInfo();
        }
        else
        {
            info.text = string.Empty;
        }

        UpdateOrders();
        UpdatePrefabs();
        UpdateTechnologies();

        if (activePlayer.Selected.Count > 0)
        {
            bottomPanel.style.display = DisplayStyle.Flex;
            infoPanel.style.display = DisplayStyle.Flex;
        }
        else
        {
            bottomPanel.style.display = DisplayStyle.None;
            infoPanel.style.display = DisplayStyle.None;
        }
    }

    public void Log(string message)
    {
        log.text = message;
    }

    private void CreateOrders()
    {
        List<OrderType> forbiddenInUI = new List<OrderType>()
        {
            OrderType.Assemble,
            OrderType.Construct,
            OrderType.Idle,
            OrderType.None,
        };

        orders.Clear();

        foreach (string i in Enum.GetNames(typeof(OrderType)))
        {
            if (forbiddenInUI.Contains(Enum.Parse<OrderType>(i)))
            {
                continue;
            }

            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnOrder(Enum.Parse<OrderType>(i)));
            button.style.display = DisplayStyle.None;
            button.text = i;
            button.userData = Enum.Parse<OrderType>(i);

            orders.Add(buttonContainer);
            ordersButtons[Enum.Parse<OrderType>(i)] = button;
        }
    }

    private void CreatePrefabs()
    {
        prefabs.Clear();

        MyGameObject[] structures = Resources.LoadAll<MyGameObject>("Objects/Structures");
        MyGameObject[] units = Resources.LoadAll<MyGameObject>("Objects/Units");

        foreach (MyGameObject myGameObject in structures)
        {
            string path = "Objects/Structures/" + myGameObject.name;

            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnConstruct(path));
            button.style.display = DisplayStyle.None;
            button.text = Path.GetFileName(path).Replace("_", " ");
            button.userData = path;

            prefabs.Add(buttonContainer);
            prefabsButtons[path] = button;
        }

        foreach (MyGameObject myGameObject in units)
        {
            string path = "Objects/Units/" + myGameObject.name;

            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnAssemble(path));
            button.style.display = DisplayStyle.None;
            button.text = Path.GetFileName(path).Replace("_", " ");
            button.userData = path;

            prefabs.Add(buttonContainer);
            prefabsButtons[path] = button;
        }
    }

    private void CreateTechnologies()
    {
        technologies.Clear();

        foreach (KeyValuePair<string, Technology> i in HUD.Instance.ActivePlayer.TechnologyTree.Technologies)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnResearch(i.Key));
            button.style.display = DisplayStyle.None;
            button.text = i.Key;
            button.userData = i.Key;

            technologies.Add(buttonContainer);
            technologiesButtons[i.Key] = button;
        }
    }

    private void OnAssemble(string prefab)
    {
        HUD.Instance.Assemble(prefab);
    }

    private void OnConstruct(string prefab)
    {
        HUD.Instance.Order = OrderType.Construct;
        HUD.Instance.Prefab = prefab;
        HUD.Instance.Technology = string.Empty;
    }

    private void OnMenu()
    {
        MainMenu.Instance.gameObject.SetActive(true);
    }

    private void OnOrder(OrderType orderType)
    {
        HUD.Instance.Order = orderType;
        HUD.Instance.Prefab = string.Empty;
        HUD.Instance.Technology = string.Empty;

    }

    private void OnResearch(string technology)
    {
        HUD.Instance.Technology = technology; // TODO: Order is important. Fix that.

        HUD.Instance.Order = OrderType.Research;
        HUD.Instance.Prefab = string.Empty;
    }

    private void UpdateOrders()
    {
        HashSet<OrderType> whitelist = new HashSet<OrderType>();

        foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selected)
        {
            if (selected.State != MyGameObjectState.Operational)
            {
                continue;
            }

            whitelist.UnionWith(selected.Orders.OrderWhitelist);
        }

        foreach (KeyValuePair<OrderType, Button> button in ordersButtons)
        {
            button.Value.style.display = DisplayStyle.None;
        }

        foreach (OrderType i in whitelist)
        {
            if (ordersButtons.ContainsKey(i))
            {
                ordersButtons[i].style.display = DisplayStyle.Flex;
            }
        }
    }

    private void UpdatePrefabs()
    {
        HashSet<string> whitelist = new HashSet<string>();

        foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selected)
        {
            if (selected.State != MyGameObjectState.Operational)
            {
                continue;
            }

            foreach (string prefab in selected.Orders.PrefabWhitelist)
            {
                whitelist.Add(prefab);
            }
        }

        foreach (KeyValuePair<string, Button> button in prefabsButtons)
        {
            button.Value.style.display = DisplayStyle.None;
        }

        foreach (string i in whitelist)
        {
            if (prefabsButtons.ContainsKey(i))
            {
                prefabsButtons[i].style.display = DisplayStyle.Flex;
                prefabsButtons[i].SetEnabled(
                    HUD.Instance.ActivePlayer.TechnologyTree.IsUnlocked(
                        Path.GetFileName(i)
                    )
                );
            }
        }
    }

    private void UpdateTechnologies()
    {
        HashSet<string> whitelist = new HashSet<string>();

        foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selected)
        {
            if (selected.State != MyGameObjectState.Operational)
            {
                continue;
            }

            foreach (string prefab in selected.Orders.TechnologyWhitelist)
            {
                whitelist.Add(prefab);
            }
        }

        foreach (KeyValuePair<string, Button> button in technologiesButtons)
        {
            button.Value.style.display = DisplayStyle.None;
        }

        foreach (string i in whitelist)
        {
            if (technologiesButtons.ContainsKey(i))
            {
                technologiesButtons[i].style.display = DisplayStyle.Flex;
                technologiesButtons[i].SetEnabled(
                    HUD.Instance.ActivePlayer.TechnologyTree.IsUnlocked(i)
                );
            }
        }
    }

    [SerializeField]
    private VisualTreeAsset templateButton;

    private VisualElement bottomPanel;
    private VisualElement infoPanel;

    private Label log;
    private Label info;
    private Button menu;

    private VisualElement orders;
    private VisualElement prefabs;
    private VisualElement technologies;

    private Dictionary<OrderType, Button> ordersButtons = new Dictionary<OrderType, Button>();
    private Dictionary<string, Button> prefabsButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> technologiesButtons = new Dictionary<string, Button>();
}
