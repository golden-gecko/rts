using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Resources : UI_Element<UI_Resources>
{
    protected override void Awake()
    {
        base.Awake();

        panel = Root.Q<VisualElement>("Panel_Resources");
        value = panel.Q<Label>("Value");
    }

    private void Update()
    {
        ResourceContainer resourceCotainer = new ResourceContainer();

        foreach (Storage storage in FindObjectsByType<Storage>(FindObjectsSortMode.None))
        {
            if (storage == null)
            {
                continue;
            }
    
            if (storage.GetComponent<MyGameObject>().Player != HUD.Instance.ActivePlayer)
            {
                continue;
            }

            foreach (Resource resource in storage.Resources.Items)
            {
                if (resourceCotainer.Current(resource.Name) > 0)
                {
                    resourceCotainer.Add(resource.Name, resource.Current);
                }
                else
                {
                    resourceCotainer.Init(resource.Name, resource.Current, int.MaxValue);
                }
            }
        }

        string resourceString = String.Empty;

        foreach (Resource resource in resourceCotainer.Items)
        {
            resourceString += string.Format("{0}: {1}, ", resource.Name, resource.Current);
        }

        value.text = resourceString;
    }

    private VisualElement panel;
    private Label value;
}
