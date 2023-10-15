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

        Vector3 start = selectionGroup.First().Position;
        Vector3 direction = (new Vector3(position.x, 0.0f, position.z) - new Vector3(start.x, 0.0f, start.z)).normalized;
        Vector3 cross = Vector3.Cross(Vector3.up, direction).normalized;

        int unitsCount = selectionGroup.Items.Where(x => x.TryGetComponent(out Engine _)).Count();
        int size = Mathf.CeilToInt(Mathf.Sqrt(unitsCount));

        int column = 0;
        int row = 0;

        float columnOffset = (size % 2 == 0) ? (size / 2.0f * Config.Formation.Spacing - Config.Formation.Spacing / 2.0f) : ((size - 1) / 2.0f * Config.Formation.Spacing);

        foreach (MyGameObject selected in selectionGroup.Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            Vector3 positionInFormation = new Vector3(row * Config.Formation.Spacing, 0.0f, column * Config.Formation.Spacing - columnOffset);

            float angle = Utils.Angle(cross);

            positionInFormation = Quaternion.Euler(0.0f, angle, 0.0f) * positionInFormation;

            selected.Move(position + positionInFormation);

            column += 1;

            if (column >= size)
            {
                column = 0;
                row += 1;
            }
        }
    }
}
