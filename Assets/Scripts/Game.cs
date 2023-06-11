using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        var consumers = new HashSet<MyGameObject>();
        var producers = new HashSet<MyGameObject>();

        foreach (var myGameObject in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            foreach (var recipe in myGameObject.Recipes)
            {
                foreach (var resource in recipe.ToConsume)
                {
                    if (myGameObject.Resources.CanAdd(resource.Name, 1))
                    {
                        consumers.Add(myGameObject);
                    }
                }

                foreach (var resource in recipe.ToProduce)
                {
                    if (myGameObject.Resources.CanRemove(resource.Name, 1))
                    {
                        producers.Add(myGameObject);
                    }
                }
            }
        }
    }
}
