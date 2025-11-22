public class DiplomacyMenu : Menu
{
    public static DiplomacyMenu Instance { get; private set; }

    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        base.Awake();
    }
}
