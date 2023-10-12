using System.Linq;
using UnityEngine;

public class FormationHandlerColumn : FormationHandler
{
    public override void Execute(SelectionGroup selectionGroup, Vector3 position, bool append = false)
    {
        if (selectionGroup.Items.Count <= 0)
        {
            return;
        }

        Vector3 start = Vector3.zero;

        foreach (MyGameObject selected in selectionGroup.Items)
        {
            start += selected.Position;
        }

        start /= selectionGroup.Items.Count;

        Vector3 direction = (new Vector3(position.x, 0.0f, position.z) - new Vector3(start.x, 0.0f, start.z)).normalized;
        Vector3 cross = Vector3.Cross(Vector3.up, direction).normalized;

        int unitsCount = selectionGroup.Items.Where(x => x.TryGetComponent(out Engine _)).Count();
        int size = Mathf.CeilToInt(Mathf.Sqrt(unitsCount));

        float spacing = 5.0f;
        float row = 0.0f;

        foreach (MyGameObject selected in selectionGroup.Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            Vector3 positionInFormation = new Vector3(-direction.x * row, 0.0f, -direction.z * row);

            selected.Move(position + positionInFormation);

            row += spacing;
        }
    }
}
