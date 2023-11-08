using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Commands_Skills : UI_Element
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Commands_Skills");

        skills = panel.Q<VisualElement>("List_Skills");
    }

    private void Start()
    {
        CreateSkills();
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

                UpdateSkills(hovered);
            }
        }
        else if (activePlayer.Selection.Count > 0 && activePlayer.Selection.First() != null)
        {
            panel.style.display = DisplayStyle.Flex;

            UpdateSkills(null);
        }
        else
        {
            panel.style.display = DisplayStyle.None;
        }
    }

    private void CreateSkills()
    {
        skills.Clear();

        foreach (string i in Game.Instance.SkillManager.Skills.Keys)
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

    private void OnUseSkill(string skill)
    {
        HUD.Instance.ActivePlayer.Selection.UseSkill(skill, MyInput.GetShift());
    }

    [SerializeField]
    private VisualTreeAsset templateButton;

    private VisualElement panel;

    private VisualElement skills;

    private Dictionary<string, Button> skillsButtons = new Dictionary<string, Button>();
}
