using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class GameMenu : MonoBehaviour
{
    public static GameMenu Instance { get; private set; }

    void Awake()
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

    void OnEnable()
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
        skills = rootVisualElement.Q<VisualElement>("Skills");

        CreateOrders();
        CreatePrefabs();
        CreateRecipes();
        CreateSkills();
        CreateTechnologies();

        Log("");
    }

    void Update()
    {
        Player activePlayer = HUD.Instance.ActivePlayer;

        if (HUD.Instance.Hovered != null)
        {
            bool ally = HUD.Instance.Hovered.Is(activePlayer, DiplomacyState.Ally);

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
                UpdateRecipes(HUD.Instance.Hovered);
                UpdateSkills(HUD.Instance.Hovered);
            }
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null)
        {
            info.text = activePlayer.Selection.First().GetInfo(true);

            bottomPanel.style.display = DisplayStyle.Flex;
            infoPanel.style.display = DisplayStyle.Flex;

            UpdateOrders(null);
            UpdatePrefabs(null);
            UpdateTechnologies(null);
            UpdateRecipes(null);
            UpdateSkills(null);
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
            OrderType.Load,
            OrderType.None,
            OrderType.Produce,
            OrderType.Transport,
            OrderType.Unload,
            OrderType.UseSkill,
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

        foreach (MyGameObject myGameObject in Resources.LoadAll<MyGameObject>(Config.DirectoryStructures))
        {
            string path = Path.Combine(Config.DirectoryStructures, myGameObject.name);

            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnConstruct(path));
            button.style.display = DisplayStyle.None;
            button.text = Path.GetFileName(path).Replace("_", " ");
            button.userData = path;

            prefabs.Add(buttonContainer);
            prefabsButtons[path] = button;
        }

        foreach (MyGameObject myGameObject in Resources.LoadAll<MyGameObject>(Config.DirectoryUnits))
        {
            string path = Path.Combine(Config.DirectoryUnits, myGameObject.name);

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

    private void CreateRecipes()
    {
        recipes.Clear();

        foreach (string i in Game.Instance.GetComponent<RecipeManager>().Recipes.Items.Keys)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnRecipe(i));
            button.style.display = DisplayStyle.None;
            button.text = i;
            button.userData = i;

            recipes.Add(buttonContainer);
            recipesButtons[i] = button;
        }
    }

    private void CreateSkills()
    {
        skills.Clear();

        foreach (string i in Config.Skills)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnUseSkill(i));
            button.style.display = DisplayStyle.None;
            button.text = i;
            button.userData = i;

            skills.Add(buttonContainer);
            skillsButtons[i] = button;
        }
    }

    private void CreateTechnologies()
    {
        technologies.Clear();

        foreach (string i in HUD.Instance.ActivePlayer.TechnologyTree.Technologies.Keys)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnResearch(i));
            button.style.display = DisplayStyle.None;
            button.text = i;
            button.userData = i;

            technologies.Add(buttonContainer);
            technologiesButtons[i] = button;
        }
    }

    private void OnAssemble(string prefab)
    {
        HUD.Instance.ActivePlayer.Selection.Assemble(prefab, MyInput.IsShift());
    }

    private void OnConstruct(string prefab)
    {
        HUD.Instance.Order = OrderType.Construct;
        HUD.Instance.Prefab = prefab;
    }

    private void OnMenu()
    {
        MainMenu.Instance.gameObject.SetActive(true);
    }

    private void OnOrder(OrderType orderType)
    {
        Player activePlayer = HUD.Instance.ActivePlayer;

        switch (orderType)
        {
            case OrderType.Destroy:
                activePlayer.Selection.Destroy(MyInput.IsShift());
                break;

            case OrderType.Disable:
                activePlayer.Selection.Disable(MyInput.IsShift());
                break;

            case OrderType.Enable:
                activePlayer.Selection.Enable(MyInput.IsShift());
                break;

            case OrderType.Explore:
                activePlayer.Selection.Explore(MyInput.IsShift());
                break;

            case OrderType.Stop:
                activePlayer.Selection.Stop(MyInput.IsShift());
                break;

            case OrderType.Wait:
                activePlayer.Selection.Wait(MyInput.IsShift());
                break;

            default:
                HUD.Instance.Order = orderType;
                break;
        }
    }

    private void OnResearch(string technology)
    {
        HUD.Instance.ActivePlayer.Selection.Research(technology, MyInput.IsShift());
    }

    private void OnRecipe(string recipe)
    {
        HUD.Instance.ActivePlayer.Selection.Produce(recipe, MyInput.IsShift());
    }

    private void OnUseSkill(string skill)
    {
        HUD.Instance.ActivePlayer.Selection.UseSkill(skill, MyInput.IsShift());
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
            foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selection.Items)
            {
                if (selected.State != MyGameObjectState.Operational)
                {
                    continue;
                }

                whitelist.UnionWith(selected.Orders.OrderWhitelist);
            }
        }

        foreach (Button button in ordersButtons.Values)
        {
            button.style.display = DisplayStyle.None;
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
            foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selection.Items)
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

        foreach (Button button in prefabsButtons.Values)
        {
            button.style.display = DisplayStyle.None;
        }

        TechnologyTree technologyTree = HUD.Instance.ActivePlayer.TechnologyTree;

        foreach (string i in whitelist)
        {
            if (prefabsButtons.ContainsKey(i))
            {
                bool enabled = technologyTree.IsUnlocked(Path.GetFileName(i)) && technologyTree.IsDiscovered(Path.GetFileName(i));

                prefabsButtons[i].style.display = DisplayStyle.Flex;
                prefabsButtons[i].SetEnabled(enabled);
            }
        }
    }

    private void UpdateRecipes(MyGameObject hovered)
    {
        HashSet<string> whitelist = new HashSet<string>();

        if (hovered != null)
        {
            if (hovered.State == MyGameObjectState.Operational)
            {
                whitelist = new HashSet<string>(hovered.Orders.RecipeWhitelist.Items.Keys);
            }
        }
        else
        {
            foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selection.Items)
            {
                if (selected.State != MyGameObjectState.Operational)
                {
                    continue;
                }

                if (selected.GetComponent<Producer>() == null)
                {
                    continue;
                }

                foreach (string recipe in selected.Orders.RecipeWhitelist.Items.Keys)
                {
                    whitelist.Add(recipe);
                }
            }
        }

        foreach (Button button in recipesButtons.Values)
        {
            button.style.display = DisplayStyle.None;
        }

        foreach (string i in whitelist)
        {
            if (recipesButtons.ContainsKey(i))
            {
                recipesButtons[i].style.display = DisplayStyle.Flex;
            }
        }
    }

    private void UpdateSkills(MyGameObject hovered)
    {
        HashSet<string> whitelist = new HashSet<string>();

        if (hovered != null)
        {
            if (hovered.State == MyGameObjectState.Operational)
            {
                whitelist = new HashSet<string>(hovered.Skills.Keys);
            }
        }
        else
        {
            foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selection.Items)
            {
                if (selected.State != MyGameObjectState.Operational)
                {
                    continue;
                }

                foreach (string skill in selected.Skills.Keys)
                {
                    whitelist.Add(skill);
                }
            }
        }

        foreach (Button button in skillsButtons.Values)
        {
            button.style.display = DisplayStyle.None;
        }

        foreach (string i in whitelist)
        {
            if (skillsButtons.ContainsKey(i))
            {
                skillsButtons[i].style.display = DisplayStyle.Flex;
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
            foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selection.Items)
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

        foreach (Button button in technologiesButtons.Values)
        {
            button.style.display = DisplayStyle.None;
        }

        TechnologyTree technologyTree = HUD.Instance.ActivePlayer.TechnologyTree;

        foreach (string i in whitelist)
        {
            if (technologiesButtons.ContainsKey(i))
            {
                bool enabled = technologyTree.IsUnlocked(i) && technologyTree.IsDiscovered(i) == false;

                technologiesButtons[i].style.display = DisplayStyle.Flex;
                technologiesButtons[i].SetEnabled(enabled);
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
    private VisualElement recipes;
    private VisualElement skills;
    private VisualElement technologies;

    private Dictionary<OrderType, Button> ordersButtons = new Dictionary<OrderType, Button>();
    private Dictionary<string, Button> prefabsButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> recipesButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> skillsButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> technologiesButtons = new Dictionary<string, Button>();
}
