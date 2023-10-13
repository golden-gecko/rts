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
        exploration = panel.Q<Toggle>("Exploration");
        power = panel.Q<Toggle>("Power");
        radar = panel.Q<Toggle>("Radar");
        sight = panel.Q<Toggle>("Sight");

        grid.RegisterValueChangedCallback(OnToggleGrid);
        range.RegisterValueChangedCallback(OnToggleRange);
        exploration.RegisterValueChangedCallback(OnToggleExploration);
        power.RegisterValueChangedCallback(OnTogglePower);
        radar.RegisterValueChangedCallback(OnToggleRadar);
        sight.RegisterValueChangedCallback(OnToggleSight);
    }

    private void OnToggleGrid(ChangeEvent<bool> evt)
    {
        Map.Instance.transform.Find("Grid").GetComponent<Projector>().enabled = evt.newValue;
    }

    private void OnToggleRange(ChangeEvent<bool> evt)
    {
        // TODO: Implement.
    }

    private void OnToggleExploration(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(power);
            Disable(radar);
            Disable(sight);
        }

        Set();
    }

    private void OnTogglePower(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(exploration);
            Disable(radar);
            Disable(sight);
        }

        Set();
    }

    private void OnToggleRadar(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(exploration);
            Disable(power);
            Disable(sight);
        }

        Set();
    }

    private void OnToggleSight(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Disable(exploration);
            Disable(power);
            Disable(radar);
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

        if (exploration.value)
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
        else
        {
            projector.material = null;
        }
    }

    private VisualElement panel;

    private Toggle grid;
    public Toggle range; // TODO: Make private.

    private Toggle exploration;
    private Toggle power;
    private Toggle radar;
    private Toggle sight;
}
