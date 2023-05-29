using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    void Start()
    {
        selected = new List<MyGameObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                if (hitInfo.transform.tag == "Terrain")
                {
                    foreach (var gameObject in selected)
                    {
                        gameObject.Order = OrderType.Move;
                        gameObject.Target = hitInfo.point;
                    }
                }
                else
                {
                    var result = hitInfo.transform.GetComponent<MyGameObject>();

                    if (selected.Contains(result))
                    {
                        selected.Remove(result);
                    }
                    else
                    {
                        selected.Add(result);
                    }
                }
            }
        }
    }

    private List<MyGameObject> selected;
}
