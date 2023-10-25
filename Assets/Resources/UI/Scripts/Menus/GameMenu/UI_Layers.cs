using UnityEngine;
using UnityEngine.UIElements;

public class UI_Layers : UI_Element<UI_Layers>
{
    protected override void Awake()
    {
        base.Awake();

        panel = root.Q<VisualElement>("Panel_Layers");
        grid = panel.Q<Toggle>("Grid");
        range = panel.Q<Toggle>("Range");

        occupation = panel.Q<Toggle>("Occupation");
        exploration = panel.Q<Toggle>("Exploration");
        power = panel.Q<Toggle>("Power");
        radar = panel.Q<Toggle>("Radar");
        sight = panel.Q<Toggle>("Sight");

        passiveDamage = panel.Q<Toggle>("PassiveDamage");
        passivePower = panel.Q<Toggle>("PassivePower");
        passiveRange = panel.Q<Toggle>("PassiveRange");

        grid.RegisterValueChangedCallback(OnToggleGrid);
        range.RegisterValueChangedCallback(OnToggleRange);

        occupation.RegisterValueChangedCallback(OnToggleOccupation);
        exploration.RegisterValueChangedCallback(OnToggleExploration);
        power.RegisterValueChangedCallback(OnTogglePower);
        radar.RegisterValueChangedCallback(OnToggleRadar);
        sight.RegisterValueChangedCallback(OnToggleSight);

        passiveDamage.RegisterValueChangedCallback(OnPassiveDamageToggle);
        passivePower.RegisterValueChangedCallback(OnPassivePowerToggle);
        passiveRange.RegisterValueChangedCallback(OnPassiveRangeToggle);
    }

    private void OnToggleGrid(ChangeEvent<bool> evt)
    {
        Map.Instance.transform.Find("Grid").GetComponent<Projector>().enabled = evt.newValue;
    }

    private void OnToggleRange(ChangeEvent<bool> evt)
    {
        foreach (MyGameObject myGameObject in HUD.Instance.ActivePlayer.Selection.Items)
        {
            myGameObject.Indicators.OnToggleRange(evt.newValue);
        }
    }

    private void OnToggleOccupation(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(exploration);
            Disable(power);
            Disable(radar);
            Disable(sight);
            Disable(passiveDamage);
            Disable(passivePower);
            Disable(passiveRange);
        }

        Set();
    }

    private void OnToggleExploration(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(occupation);
            Disable(power);
            Disable(radar);
            Disable(sight);
            Disable(passiveDamage);
            Disable(passivePower);
            Disable(passiveRange);
        }

        Set();
    }

    private void OnTogglePower(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(occupation);
            Disable(exploration);
            Disable(radar);
            Disable(sight);
            Disable(passiveDamage);
            Disable(passivePower);
            Disable(passiveRange);
        }

        Set();
    }

    private void OnToggleRadar(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(occupation);
            Disable(exploration);
            Disable(power);
            Disable(sight);
            Disable(passiveDamage);
            Disable(passivePower);
            Disable(passiveRange);
        }

        Set();
    }

    private void OnToggleSight(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(occupation);
            Disable(exploration);
            Disable(power);
            Disable(radar);
            Disable(passiveDamage);
            Disable(passivePower);
            Disable(passiveRange);
        }

        Set();
    }

    private void OnPassiveDamageToggle(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(occupation);
            Disable(exploration);
            Disable(power);
            Disable(radar);
            Disable(sight);
            Disable(passivePower);
            Disable(passiveRange);
        }

        Set();
    }

    private void OnPassivePowerToggle(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(occupation);
            Disable(exploration);
            Disable(power);
            Disable(radar);
            Disable(sight);
            Disable(passiveDamage);
            Disable(passiveRange);
        }

        Set();
    }

    private void OnPassiveRangeToggle(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(occupation);
            Disable(exploration);
            Disable(power);
            Disable(radar);
            Disable(sight);
            Disable(passiveDamage);
            Disable(passivePower);
        }

        Set();
    }

    private void Disable(Toggle toggle)
    {
        if (toggle.value)
        {
            toggle.value = false;
        }
    }

    private void Set()
    {
        Projector projector = Map.Instance.transform.Find("Layers").GetComponent<Projector>();

        if (occupation.value)
        {
            projector.material = Map.Instance.Occupation;
        }
        else if (exploration.value)
        {
            projector.material = Map.Instance.Exploration;
        }
        else if (power.value)
        {
            projector.material = Map.Instance.Power;
        }
        else if (radar.value)
        {
            projector.material = Map.Instance.Radar;
        }
        else if (sight.value)
        {
            projector.material = Map.Instance.Sight;
        }
        else if (passiveDamage.value)
        {
            projector.material = Map.Instance.PassiveDamage;
        }
        else if (passivePower.value)
        {
            projector.material = Map.Instance.PassivePower;
        }
        else if (passiveRange.value)
        {
            projector.material = Map.Instance.PassiveRange;
        }
        else
        {
            projector.material = null;
        }
    }

    private VisualElement panel;

    private Toggle grid;
    public Toggle range; // TODO: Make private.

    private Toggle occupation;
    private Toggle exploration;
    private Toggle power;
    private Toggle radar;
    private Toggle sight;

    private Toggle passiveDamage;
    private Toggle passivePower;
    private Toggle passiveRange;
}
