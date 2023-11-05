using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Commands_Recipes : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Commands_Recipes");

        recipes = panel.Q<VisualElement>("List_Recipes");
    }

    private void Start()
    {
        CreateRecipes();
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

                UpdateRecipes(hovered);
            }
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null)
        {
            panel.style.display = DisplayStyle.Flex;

            UpdateRecipes(null);
        }
        else
        {
            panel.style.display = DisplayStyle.None;
        }
    }

    private void CreateRecipes()
    {
        recipes.Clear();

        foreach (string i in RecipeManager.Instance.Recipes.Recipes.Keys)
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

    private void UpdateRecipes(MyGameObject hovered)
    {
        HashSet<string> whitelist = new HashSet<string>();

        if (hovered != null)
        {
            if (hovered.State == MyGameObjectState.Operational)
            {
                whitelist = new HashSet<string>(hovered.Orders.RecipeWhitelist.Recipes.Keys);
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

                foreach (string recipe in selected.Orders.RecipeWhitelist.Recipes.Keys)
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

    private void OnRecipe(string recipe)
    {
        HUD.Instance.ActivePlayer.Selection.Produce(recipe, MyInput.GetShift());
    }

    [SerializeField]
    private VisualTreeAsset templateButton;

    private VisualElement panel;

    private VisualElement recipes;

    private Dictionary<string, Button> recipesButtons = new Dictionary<string, Button>();
}
