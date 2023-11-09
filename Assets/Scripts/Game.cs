using System.Linq;
using UnityEngine;

public class Game : Singleton<Game>
{
    protected override void Awake()
    {
        base.Awake();

        Config = GetComponent<Config>();
        BlueprintManager = GetComponent<BlueprintManager>();
        DiplomacyManager = GetComponent<DiplomacyManager>();
        DisasterManager = GetComponent<DisasterManager>();
        RecipeManager = GetComponent<RecipeManager>();
        SkillManager = GetComponent<SkillManager>();
    }

    public MyGameObject GetGameObject(string name)
    {
        GameObject gameObject = Config.Structures.Where(x => x.name == name).FirstOrDefault();

        if (gameObject && gameObject.TryGetComponent(out MyGameObject myGameObject))
        {
            return myGameObject;
        }

        gameObject = Config.Units.Where(x => x.name == name).FirstOrDefault();

        if (gameObject && gameObject.TryGetComponent(out myGameObject))
        {
            return myGameObject;
        }

        return null;
    }

    public Config Config { get; private set; }
    public BlueprintManager BlueprintManager { get; private set; }
    public DiplomacyManager DiplomacyManager { get; private set; }
    public DisasterManager DisasterManager { get; private set; }
    public RecipeManager RecipeManager { get; private set; }
    public SkillManager SkillManager { get; private set; }
}
