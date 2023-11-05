using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Commands_Formations : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Commands_Formations");

        formations = panel.Q<VisualElement>("List_Formations");
    }

    private void Start()
    {
        CreateFormations();
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
            }
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null)
        {
            panel.style.display = DisplayStyle.Flex;
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

    private void OnFormation(Formation formation)
    {
        HUD.Instance.Formation = formation;
    }

    [SerializeField]
    private VisualTreeAsset templateButton;

    private VisualElement panel;

    private VisualElement formations;

    private Dictionary<Formation, Button> formationsButtons = new Dictionary<Formation, Button>();
}
