using UnityEngine;

public class GameDebug : MonoBehaviour
{
    public static GameDebug Instance { get; private set; }

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        A = transform.Find("A");
        B = transform.Find("B");
        C = transform.Find("C");
    }

    public Transform A { get; private set; }

    public Transform B { get; private set; }

    public Transform C { get; private set; }
}
