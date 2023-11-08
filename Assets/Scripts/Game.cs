public class Game : Singleton<Game>
{
    protected override void Awake()
    {
        base.Awake();

        BlueprintManager = GetComponent<BlueprintManager>();
        DiplomacyManager = GetComponent<DiplomacyManager>();
        DisasterManager = GetComponent<DisasterManager>();
        RecipeManager = GetComponent<RecipeManager>();
        SkillManager = GetComponent<SkillManager>();
    }

    public BlueprintManager BlueprintManager { get; private set; }
    public DiplomacyManager DiplomacyManager { get; private set; }
    public DisasterManager DisasterManager { get; private set; }
    public RecipeManager RecipeManager { get; private set; }
    public SkillManager SkillManager { get; private set; }
}
