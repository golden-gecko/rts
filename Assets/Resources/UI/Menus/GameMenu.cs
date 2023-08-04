using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        recipes = rootVisualElement.Q<VisualElement>("RecipeList");

        CreateOrders();
        CreatePrefabs();
        CreateTechnologies();
        CreateRecipes();

        Log("");
    }

    private void Update()
    {
        Player activePlayer = HUD.Instance.ActivePlayer;

        if (HUD.Instance.Hovered != null)
        {
            bool ally = HUD.Instance.Hovered.IsAlly(activePlayer);

            info.text = HUD.Instance.Hovered.GetInfo(ally);

            if (ally)
            {
                bottomPanel.style.display = DisplayStyle.Flex;
            }

            infoPanel.style.display = DisplayStyle.Flex;

            if (ally)
            {
                UpdateOrders(HUD.Instance.Hovered);
                UpdatePrefabs(HUD.Instance.Hovered);
                UpdateTechnologies(HUD.Instance.Hovered);
            }
        }
        else if (activePlayer.Selected.Count > 0 && activePlayer.Selected.First() != null)
        {
            info.text = activePlayer.Selected.First().GetInfo(true);

            bottomPanel.style.display = DisplayStyle.Flex;
            infoPanel.style.display = DisplayStyle.Flex;

            UpdateOrders(null);
            UpdatePrefabs(null);
            UpdateTechnologies(null);
        }
        else
        {
            info.text = string.Empty;

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
            OrderType.Produce,
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

    private void CreateRecipes()
    {
        recipes.Clear();

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

    private void UpdateOrders(MyGameObject hovered)
    {
        HashSet<OrderType> whitelist = new HashSet<OrderType>();

        if (hovered != null)
        {
            if (hovered.State == MyGameObjectState.Operational)
            {
                whitelist = new HashSet<OrderType>(hovered.Orders.OrderWhitelist);
            }
        }
        else
        {
            foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selected)
            {
                if (selected.State != MyGameObjectState.Operational)
                {
                    continue;
                }

                whitelist.UnionWith(selected.Orders.OrderWhitelist);
            }
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

    private void UpdatePrefabs(MyGameObject hovered)
    {
        HashSet<string> whitelist = new HashSet<string>();

        if (hovered != null)
        {
            if (hovered.State == MyGameObjectState.Operational)
            {
                whitelist = new HashSet<string>(hovered.Orders.PrefabWhitelist);
            }
        }
        else
        {
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

    private void UpdateTechnologies(MyGameObject hovered)
    {
        HashSet<string> whitelist = new HashSet<string>();

        if (hovered != null)
        {
            if (hovered.State == MyGameObjectState.Operational)
            {
                whitelist = new HashSet<string>(hovered.Orders.TechnologyWhitelist);
            }
        }
        else
        {
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
                    !HUD.Instance.ActivePlayer.TechnologyTree.IsUnlocked(i)
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
    private VisualElement recipes;

    private Dictionary<OrderType, Button> ordersButtons = new Dictionary<OrderType, Button>();
    private Dictionary<string, Button> prefabsButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> technologiesButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> recipeButtons = new Dictionary<string, Button>();
}
