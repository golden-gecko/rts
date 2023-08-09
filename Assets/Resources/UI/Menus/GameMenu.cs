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
        skills = rootVisualElement.Q<VisualElement>("Skills");

        CreateOrders();
        CreatePrefabs();
        CreateTechnologies();
        CreateRecipes();
        CreateSkills();

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
                UpdateRecipes(HUD.Instance.Hovered);
                UpdateSkills(HUD.Instance.Hovered);
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

    private void CreateRecipes()
    {
        // TODO: Put this somewhere.
        List<string> _recipies = new List<string>()
        {
            "Metal using coal",
            "Metal using wood",
        };

        recipes.Clear();

        foreach (string i in _recipies)
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
        // TODO: Put this somewhere.
        List<string> _skills = new List<string>()
        {
            "Damage",
            "Repair",
        };

        skills.Clear();

        foreach (string i in _skills)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(ev => OnSkill(i));
            button.style.display = DisplayStyle.None;
            button.text = i;
            button.userData = i;

            skills.Add(buttonContainer);
            skillsButtons[i] = button;
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
    }

    private void OnMenu()
    {
        MainMenu.Instance.gameObject.SetActive(true);
    }

    private void OnOrder(OrderType orderType)
    {
        switch (orderType)
        {
            case OrderType.Destroy:
                HUD.Instance.Destroy();
                break;

            case OrderType.Explore:
                HUD.Instance.Explore();
                break;

            case OrderType.Stop:
                HUD.Instance.Stop();
                break;

            case OrderType.Wait:
                HUD.Instance.Wait();
                break;

            default:
                HUD.Instance.Order = orderType;
                break;
        }
    }

    private void OnResearch(string technology)
    {
        HUD.Instance.Research(technology);
    }

    private void OnRecipe(string recipe)
    {
        HUD.Instance.Produce(recipe);
    }

    private void OnSkill(string skill)
    {
        HUD.Instance.Skill(skill);
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

    private void UpdateRecipes(MyGameObject hovered)
    {
        HashSet<string> whitelist = new HashSet<string>();

        if (hovered != null)
        {
            if (hovered.State == MyGameObjectState.Operational)
            {
                whitelist = new HashSet<string>(hovered.Recipes.Items.Keys);
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

                foreach (string recipe in selected.Recipes.Items.Keys)
                {
                    whitelist.Add(recipe);
                }
            }
        }

        foreach (KeyValuePair<string, Button> button in recipesButtons)
        {
            button.Value.style.display = DisplayStyle.None;
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
            foreach (MyGameObject selected in HUD.Instance.ActivePlayer.Selected)
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

        foreach (KeyValuePair<string, Button> button in skillsButtons)
        {
            button.Value.style.display = DisplayStyle.None;
        }

        foreach (string i in whitelist)
        {
            if (skillsButtons.ContainsKey(i))
            {
                skillsButtons[i].style.display = DisplayStyle.Flex;
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
    private VisualElement skills;
    
    private Dictionary<OrderType, Button> ordersButtons = new Dictionary<OrderType, Button>();
    private Dictionary<string, Button> prefabsButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> technologiesButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> recipesButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> skillsButtons = new Dictionary<string, Button>();
}
