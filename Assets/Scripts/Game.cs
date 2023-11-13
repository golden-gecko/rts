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

    public void GetGameObject(string name, out MyGameObject myGameObject, out Blueprint blueprint)
    {
        GameObject gameObject = Config.Structures.Where(x => x.name == name).FirstOrDefault();

        if (gameObject && gameObject.TryGetComponent(out myGameObject))
        {
            blueprint = null;

            return;
        }

        gameObject = Config.Units.Where(x => x.name == name).FirstOrDefault();

        if (gameObject && gameObject.TryGetComponent(out myGameObject))
        {
            blueprint = null;

            return;
        }

        blueprint = BlueprintManager.Get(name);

        if (blueprint == null)
        {
            myGameObject = null;

            return;
        }

        myGameObject = null;
        blueprint = null;
    }

    public Config Config { get; private set; }
    public BlueprintManager BlueprintManager { get; private set; }
    public DiplomacyManager DiplomacyManager { get; private set; }
    public DisasterManager DisasterManager { get; private set; }
    public RecipeManager RecipeManager { get; private set; }
    public SkillManager SkillManager { get; private set; }
}
