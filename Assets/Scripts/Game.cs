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
                if (recipe.ToConsume.Count > 0)
                {
                    consumers.Add(myGameObject);
                }

                if (recipe.ToProduce.Count > 0)
                {
                    producers.Add(myGameObject);
                }
            }
        }


    }
}
