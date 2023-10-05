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

        panel_Commands = rootVisualElement.Q<VisualElement>("Panel_Commands");
        panel_Info = rootVisualElement.Q<VisualElement>("Panel_Info");
        panel_Log = rootVisualElement.Q<VisualElement>("Panel_Log");
        panel_Menu = rootVisualElement.Q<VisualElement>("Panel_Menu");
        panel_Minimap = rootVisualElement.Q<VisualElement>("Panel_Minimap");
        panel_Orders = rootVisualElement.Q<VisualElement>("Panel_Orders");
        panel_Resources = rootVisualElement.Q<VisualElement>("Panel_Resources");

        value_Info = panel_Info.Q<Label>("Value");
        value_Log = panel_Log.Q<Label>("Value");
        value_Orders = panel_Orders.Q<Label>("Value");
        value_Resources = panel_Resources.Q<Label>("Value");

        menu = panel_Menu.Q<Button>("Menu");
        menu.RegisterCallback<ClickEvent>(ev => OnMenu());

        orders = panel_Commands.Q<VisualElement>("List_Orders");
        prefabs = panel_Commands.Q<VisualElement>("List_Prefabs");
        recipes = panel_Commands.Q<VisualElement>("List_Recipes");
        skills = panel_Commands.Q<VisualElement>("List_Skills");
        technologies = panel_Commands.Q<VisualElement>("List_Technologies");

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

            value_Info.text = HUD.Instance.Hovered.GetInfo(ally);

            panel_Info.style.display = DisplayStyle.Flex;

            if (ally)
            {
                panel_Commands.style.display = DisplayStyle.Flex;
                panel_Orders.style.display = DisplayStyle.Flex;

                value_Orders.text = HUD.Instance.Hovered.GetInfoOrders(ally);

                UpdateOrders(HUD.Instance.Hovered);
                UpdatePrefabs(HUD.Instance.Hovered);
                UpdateTechnologies(HUD.Instance.Hovered);
                UpdateRecipes(HUD.Instance.Hovered);
                UpdateSkills(HUD.Instance.Hovered);
            }
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null)
        {
            value_Info.text = activePlayer.Selection.First().GetInfo(true);
            value_Orders.text = activePlayer.Selection.First().GetInfoOrders(true);

            panel_Commands.style.display = DisplayStyle.Flex;
            panel_Info.style.display = DisplayStyle.Flex;
            panel_Orders.style.display = DisplayStyle.Flex;

            UpdateOrders(null);
            UpdatePrefabs(null);
            UpdateTechnologies(null);
            UpdateRecipes(null);
            UpdateSkills(null);
        }
        else
        {
            value_Info.text = string.Empty;

            panel_Commands.style.display = DisplayStyle.None;
            panel_Info.style.display = DisplayStyle.None;
            panel_Orders.style.display = DisplayStyle.None;
        }

        UpdateResources();
    }

    public void Log(string message)
    {
        value_Log.text = message;
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
            button.text = Utils.FormatName(i);
            button.userData = Enum.Parse<OrderType>(i);

            orders.Add(buttonContainer);
            ordersButtons[Enum.Parse<OrderType>(i)] = button;
        }
    }

    private void CreatePrefabs()
    {
        prefabs.Clear();

        foreach (MyGameObject myGameObject in Resources.LoadAll<MyGameObject>(Config.Asset.Structures))
        {
            string path = Path.Combine(Config.Asset.Structures, myGameObject.name);

            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnConstruct(path));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(Path.GetFileName(path));
            button.userData = path;

            prefabs.Add(buttonContainer);
            prefabsButtons[path] = button;
        }

        foreach (MyGameObject myGameObject in Resources.LoadAll<MyGameObject>(Config.Asset.Units))
        {
            string path = Path.Combine(Config.Asset.Units, myGameObject.name);

            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnAssemble(path));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(Path.GetFileName(path));
            button.userData = path;

            prefabs.Add(buttonContainer);
            prefabsButtons[path] = button;
        }
    }

    private void CreateRecipes()
    {
        recipes.Clear();

        foreach (string i in RecipeManager.Instance.Recipes.Items.Keys)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnRecipe(i));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(i);
            button.userData = i;

            recipes.Add(buttonContainer);
            recipesButtons[i] = button;
        }
    }

    private void CreateSkills()
    {
        skills.Clear();

        foreach (string i in SkillManager.Instance.Skills.Keys)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnUseSkill(i));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(i);
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
            button.text = Utils.FormatName(i);
            button.userData = i;

            technologies.Add(buttonContainer);
            technologiesButtons[i] = button;
        }
    }

    private void OnAssemble(string prefab)
    {
        HUD.Instance.ActivePlayer.Selection.Assemble(prefab, MyInput.GetShift());
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
                activePlayer.Selection.Destroy(MyInput.GetShift());
                break;

            case OrderType.Disable:
                activePlayer.Selection.Disable(MyInput.GetShift());
                break;

            case OrderType.Enable:
                activePlayer.Selection.Enable(MyInput.GetShift());
                break;

            case OrderType.Explore:
                activePlayer.Selection.Explore(MyInput.GetShift());
                break;

            case OrderType.Stop:
                activePlayer.Selection.Stop(MyInput.GetShift());
                break;

            case OrderType.Wait:
                activePlayer.Selection.Wait(MyInput.GetShift());
                break;

            default:
                HUD.Instance.Order = orderType;
                break;
        }
    }

    private void OnResearch(string technology)
    {
        HUD.Instance.ActivePlayer.Selection.Research(technology, MyInput.GetShift());
    }

    private void OnRecipe(string recipe)
    {
        HUD.Instance.ActivePlayer.Selection.Produce(recipe, MyInput.GetShift());
    }

    private void OnUseSkill(string skill)
    {
        HUD.Instance.ActivePlayer.Selection.UseSkill(skill, MyInput.GetShift());
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
                if (selected.State == MyGameObjectState.Operational)
                {
                    whitelist.UnionWith(selected.Orders.OrderWhitelist);
                }
            }
        }

        foreach (Button button in ordersButtons.Values)
        {
            button.style.display = DisplayStyle.None;
        }

        foreach (OrderType i in whitelist)
        {
            if (ordersButtons.TryGetValue(i, out Button button))
            {
                button.style.display = DisplayStyle.Flex;
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
            if (prefabsButtons.TryGetValue(i, out Button button))
            {
                bool enabled = technologyTree.IsDiscovered(Path.GetFileName(i));

                button.style.display = DisplayStyle.Flex;
                button.SetEnabled(enabled);
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
            if (recipesButtons.TryGetValue(i, out Button button))
            {
                button.style.display = DisplayStyle.Flex;
            }
        }
    }

    private void UpdateResources()
    {
        ResourceContainer resources = new ResourceContainer();

        foreach (Storage storage in FindObjectsByType<Storage>(FindObjectsSortMode.None))
        {
            if (storage.GetComponent<MyGameObject>().Player != HUD.Instance.ActivePlayer)
            {
                continue;
            }

            foreach (Resource resource in storage.Resources.Items)
            {
                if (resources.Current(resource.Name) > 0)
                {
                    resources.Add(resource.Name, resource.Current);
                }
                else
                {
                    resources.Init(resource.Name, resource.Current, int.MaxValue);
                }
            }
        }

        string value = String.Empty;

        foreach (Resource resource in resources.Items)
        {
            value += string.Format("{0}: {1}, ", resource.Name, resource.Current);
        }

        panel_Resources.Q<Label>("Value").text = value;
    }

    private void UpdateSkills(MyGameObject hovered)
    {
        Dictionary<string, bool> whitelist = new Dictionary<string, bool>();

        if (hovered != null)
        {
            if (hovered.State == MyGameObjectState.Operational)
            {
                foreach (Skill skill in hovered.Skills.Values)
                {
                    if (whitelist.ContainsKey(skill.Name) == false || whitelist[skill.Name] == false)
                    {
                        whitelist[skill.Name] = skill.Cooldown.Finished && skill.Passive == false;
                    }
                }
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

                foreach (Skill skill in selected.Skills.Values)
                {
                    if (whitelist.ContainsKey(skill.Name) == false || whitelist[skill.Name] == false)
                    {
                        whitelist[skill.Name] = skill.Cooldown.Finished && skill.Passive == false;
                    }
                }
            }
        }

        foreach (Button button in skillsButtons.Values)
        {
            button.style.display = DisplayStyle.None;
        }

        foreach (KeyValuePair<string, bool> i in whitelist)
        {
            if (skillsButtons.TryGetValue(i.Key, out Button button))
            {
                button.style.display = DisplayStyle.Flex;
                button.SetEnabled(i.Value);
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
            if (technologiesButtons.TryGetValue(i, out Button button))
            {
                bool enabled = technologyTree.IsReadyToDiscover(i);

                button.style.display = DisplayStyle.Flex;
                button.SetEnabled(enabled);
            }
        }
    }

    [SerializeField]
    private VisualTreeAsset templateButton;

    private VisualElement panel_Commands;
    private VisualElement panel_Info;
    private VisualElement panel_Log;
    private VisualElement panel_Menu;
    private VisualElement panel_Minimap;
    private VisualElement panel_Orders;
    private VisualElement panel_Resources;

    private Label value_Info;
    private Label value_Log;
    private Label value_Orders;
    private Label value_Resources;

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
