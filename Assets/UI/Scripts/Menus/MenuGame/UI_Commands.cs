using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Commands : UI_Element<UI_Commands>
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Commands");

        formations = panel.Q<VisualElement>("List_Formations");
        orders = panel.Q<VisualElement>("List_Orders");
        prefabs = panel.Q<VisualElement>("List_Prefabs");
        recipes = panel.Q<VisualElement>("List_Recipes");
        skills = panel.Q<VisualElement>("List_Skills");
        technologies = panel.Q<VisualElement>("List_Technologies");
    }

    private void Start()
    {
        CreateFormations();
        CreateOrders();
        CreatePrefabs();
        CreateRecipes();
        CreateSkills();
        CreateTechnologies();
    }

    private void Update()
    {
        Player activePlayer = HUD.Instance.ActivePlayer;
        MyGameObject hovered = HUD.Instance.Hovered;

        if (hovered != null)
        {
            bool ally = hovered.Is(activePlayer, DiplomacyState.Ally);

            if (ally)
            {
                panel.style.display = DisplayStyle.Flex;

                UpdateOrders(hovered);
                UpdatePrefabs(hovered);
                UpdateTechnologies(hovered);
                UpdateRecipes(hovered);
                UpdateSkills(hovered);
            }
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null)
        {
            panel.style.display = DisplayStyle.Flex;

            UpdateOrders(null);
            UpdatePrefabs(null);
            UpdateTechnologies(null);
            UpdateRecipes(null);
            UpdateSkills(null);
        }
        else
        {
            panel.style.display = DisplayStyle.None;
        }
    }

    private void CreateFormations()
    {
        formations.Clear();

        foreach (string i in Utils.GetFormationNames())
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnFormation(Enum.Parse<Formation>(i)));
            button.style.display = DisplayStyle.Flex;
            button.text = Utils.FormatName(i);
            button.userData = Enum.Parse<Formation>(i);

            formations.Add(buttonContainer);
            formationsButtons[Enum.Parse<Formation>(i)] = button;
        }
    }

    private void CreateOrders()
    {
        List<OrderType> forbiddenInUI = new List<OrderType>()
        {
            OrderType.Assemble,
            OrderType.Construct,
            OrderType.GatherResource,
            OrderType.Idle,
            OrderType.Load,
            OrderType.None,
            OrderType.Produce,
            OrderType.Transport,
            OrderType.Unload,
            OrderType.UseSkill,
        };

        orders.Clear();

        foreach (string i in Utils.GetOrderNames())
        {
            if (forbiddenInUI.Contains(Enum.Parse<OrderType>(i)))
            {
                continue;
            }

            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnOrder(Enum.Parse<OrderType>(i)));
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

        foreach (GameObject gameObject in ConfigPrefabs.Instance.Structures)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnConstruct(gameObject.name));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(gameObject.name);
            button.userData = gameObject.name;

            prefabs.Add(buttonContainer);
            prefabsButtons[gameObject.name] = button;
        }

        foreach (GameObject gameObject in ConfigPrefabs.Instance.Units)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnAssemble(gameObject.name));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(Path.GetFileName(gameObject.name));
            button.userData = gameObject.name;

            prefabs.Add(buttonContainer);
            prefabsButtons[gameObject.name] = button;
        }
    }

    private void CreateRecipes()
    {
        recipes.Clear();

        foreach (string i in RecipeManager.Instance.Recipes.Items.Keys)
        {
            TemplateContainer buttonContainer = templateButton.Instantiate();
            Button button = buttonContainer.Q<Button>();

            button.RegisterCallback<ClickEvent>(x => OnRecipe(i));
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

            button.RegisterCallback<ClickEvent>(x => OnUseSkill(i));
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

            button.RegisterCallback<ClickEvent>(x => OnResearch(i));
            button.style.display = DisplayStyle.None;
            button.text = Utils.FormatName(i);
            button.userData = i;

            technologies.Add(buttonContainer);
            technologiesButtons[i] = button;
        }
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

    private void OnAssemble(string prefab)
    {
        HUD.Instance.ActivePlayer.Selection.Assemble(prefab, MyInput.GetShift());
    }

    private void OnConstruct(string prefab)
    {
        HUD.Instance.Order = OrderType.Construct;
        HUD.Instance.Prefab = prefab;
    }

    private void OnFormation(Formation formation)
    {
        HUD.Instance.Formation = formation;
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

    [SerializeField]
    private VisualTreeAsset templateButton;

    private VisualElement panel;

    private VisualElement formations;
    private VisualElement orders;
    private VisualElement prefabs;
    private VisualElement recipes;
    private VisualElement skills;
    private VisualElement technologies;

    private Dictionary<Formation, Button> formationsButtons = new Dictionary<Formation, Button>();
    private Dictionary<OrderType, Button> ordersButtons = new Dictionary<OrderType, Button>();
    private Dictionary<string, Button> prefabsButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> recipesButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> skillsButtons = new Dictionary<string, Button>();
    private Dictionary<string, Button> technologiesButtons = new Dictionary<string, Button>();
}
