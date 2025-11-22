using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    void Start()
    {
        Selected = new List<MyGameObject>();
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
                    foreach (var gameObject in Selected)
                    {
                        gameObject.Move(hitInfo.point);
                    }
                }
                else
                {
                    Selected.Clear();

                    var myGameObject = hitInfo.transform.GetComponentInParent<MyGameObject>();

                    if (myGameObject != null)
                    {
                        Selected.Add(myGameObject);

                        foreach (var item in myGameObject.Resources)
                        {
                            Debug.Log(item.Key + " " + item.Value.Value);
                        }
                    }
                }
            }
        }
    }

    public List<MyGameObject> Selected { get; private set; }
}
