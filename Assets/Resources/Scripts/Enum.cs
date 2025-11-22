public enum DiplomacyState
{
    Ally,
    Enemy,
    Neutral,
}

public enum MyComponentType
{
    Armour,
    Constructor, // TODO: Implement.
    Engine,
    Gun,
    Storage, // TODO: Implement.
    Radar, // TODO: Implement.
}

public enum MyGameObjectState
{
    Operational,
    UnderAssembly,
    UnderConstruction,
}

public enum OrderType
{
    Assemble,
    Attack,
    Construct,
    Destroy,
    Explore,
    Follow,
    Guard,
    Idle,
    Load,
    Move,
    None,
    Patrol,
    Produce,
    Rally,
    Research,
    Skill, // TODO: Rename to verb.
    Stop,
    Transport,
    Unload,
    Wait,
}

public enum PrefabConstructionType
{
    Structure,
    Unit,
}
