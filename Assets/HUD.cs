using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    void Start()
    {
        selected = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                var result = hitInfo.transform.gameObject;

                if (selected.Contains(result))
                {
                    selected.Remove(result);
                }
                else
                {
                    selected.Add(result);
                }
            }

            Debug.Log(selected.Count);
        }
    }

    private List<GameObject> selected;
}
