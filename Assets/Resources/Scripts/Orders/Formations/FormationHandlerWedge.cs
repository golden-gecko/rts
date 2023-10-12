using System.Linq;
using UnityEngine;

public class FormationHandlerWedge : FormationHandler
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

        float column = 0.0f;
        float columnMax = Config.Formation.Spacing;
        float columnOffset = 0.0f;
        float row = 0.0f;

        foreach (MyGameObject selected in selectionGroup.Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            Vector3 positionInFormation = new Vector3(row, 0.0f, column - columnOffset);

            float angle = Utils.Angle(cross);

            positionInFormation = Quaternion.Euler(0.0f, angle, 0.0f) * positionInFormation;

            selected.Move(position + positionInFormation);

            column += Config.Formation.Spacing;

            if (column >= columnMax) // TODO: Is it correct (floating errors)?
            {
                column = 0.0f;
                columnMax += Config.Formation.Spacing * 2.0f;
                columnOffset += Config.Formation.Spacing;
                row += Config.Formation.Spacing;
            }
        }
    }
}
