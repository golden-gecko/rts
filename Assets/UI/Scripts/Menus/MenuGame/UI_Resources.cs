using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Resources : UI_Element
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

            if (storage.TryGetComponent(out MyGameObject myGameObject) == false)
            {
                continue;
            }

            if (myGameObject.State != MyGameObjectState.Operational)
            {
                continue;
            }

            if (myGameObject.Player != HUD.Instance.ActivePlayer)
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

        value.text = string.Join(", ", resourceCotainer.Items.Select(x => string.Format("{0}: {1}", x.Name, x.Current)));
    }

    private VisualElement panel;
    private Label value;
}
