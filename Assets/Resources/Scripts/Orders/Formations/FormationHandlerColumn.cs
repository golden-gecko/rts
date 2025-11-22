using UnityEngine;

public class FormationHandlerColumn : FormationHandler
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

        int row = 0;

        foreach (MyGameObject selected in selectionGroup.Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            Vector3 positionInFormation = new Vector3(row * Config.Formation.Spacing, 0.0f, 0.0f);

            float angle = Utils.Angle(cross);

            positionInFormation = Quaternion.Euler(0.0f, angle, 0.0f) * positionInFormation;

            selected.Move(position + positionInFormation);

            row += 1;
        }
    }
}
