using System.Linq;
using UnityEngine;

public class FormationHandlerSquare : FormationHandler
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
        float column = 0.0f;
        float row = 0.0f;
        float offset;

        if (size == 1)
        {
            offset = 0.0f;
        }
        else if (size % 2 == 0)
        {
            offset = -(size / 2) * spacing + spacing / 2.0f;
        }
        else
        {
            offset = -((size - 1) / 2) * spacing;
        }

        foreach (MyGameObject selected in selectionGroup.Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            Vector3 positionInFormation = new Vector3(row, 0.0f, column + offset);

            selected.Move(position + positionInFormation);

            column += spacing;

            if (column >= size * spacing)
            {
                column = 0.0f;
                row += spacing;
            }
        }
    }
}
