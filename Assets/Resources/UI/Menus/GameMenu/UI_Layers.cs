using UnityEngine;
using UnityEngine.UIElements;

public class UI_Layers : MonoBehaviour
{
    void Awake()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement rootVisualElement = uiDocument.rootVisualElement;

        panel = rootVisualElement.Q<VisualElement>("Panel_Layers");

        exploration = panel.Q<Toggle>("Exploration");
        power = panel.Q<Toggle>("Power");
        radar = panel.Q<Toggle>("Radar");
        sight = panel.Q<Toggle>("Sight");

        exploration.RegisterValueChangedCallback(OnToggleExploration);
        power.RegisterValueChangedCallback(OnTogglePower);
        radar.RegisterValueChangedCallback(OnToggleRadar);
        sight.RegisterValueChangedCallback(OnToggleSight);
    }

    private void OnToggleExploration(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Map.Instance.transform.Find("Layers").GetComponent<Projector>().material = Map.Instance.Exploration;

            power.value = false;
            radar.value = false;
            sight.value = false;
        }
        else
        {
            Map.Instance.transform.Find("Layers").GetComponent<Projector>().material = null;
        }
    }

    private void OnTogglePower(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Map.Instance.transform.Find("Layers").GetComponent<Projector>().material = Map.Instance.Power;

            exploration.value = false;
            radar.value = false;
            sight.value = false;
        }
        else
        {
            Map.Instance.transform.Find("Layers").GetComponent<Projector>().material = null;
        }
    }

    private void OnToggleRadar(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Map.Instance.transform.Find("Layers").GetComponent<Projector>().material = Map.Instance.Radar;

            exploration.value = false;
            power.value = false;
            sight.value = false;
        }
        else
        {
            Map.Instance.transform.Find("Layers").GetComponent<Projector>().material = null;
        }
    }

    private void OnToggleSight(ChangeEvent<bool> evt)
    {
        if (evt.newValue)
        {
            Map.Instance.transform.Find("Layers").GetComponent<Projector>().material = Map.Instance.Sight;

            exploration.value = false;
            power.value = false;
            radar.value = false;
        }
        else
        {
            Map.Instance.transform.Find("Layers").GetComponent<Projector>().material = null;
        }
    }

    private VisualElement panel;

    private Toggle exploration;
    private Toggle power;
    private Toggle radar;
    private Toggle sight;
}
