using UnityEngine;

class Field
{
}

public class Map : MonoBehaviour
{
    void Start()
    {
        size = 100;
        fields = new Field[size, size];
    }

    void Update()
    {
        
    }
    
    private int size;
    private Field[,] fields;
}
