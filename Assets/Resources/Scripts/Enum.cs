public enum DiplomacyState
{
    Ally,
    Enemy,
    Neutral,
}

public enum MyGameObjectMapLayer
{
    Air,
    None,
    Terrain,
    Underwater, // TODO: Implement.
    Water,
}

public enum MyGameObjectState
{
    Cursor,
    Destroyed,
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
    Disable,
    Enable,
    Explore,
    Follow,
    Gather,
    Guard,
    Idle,
    Load,
    Move,
    None,
    Patrol,
    Produce,
    Rally,
    Research,
    Stop,
    Transport,
    Unload,
    UseSkill,
    Wait,
}

public enum ResourceDirection
{
    In,
    None,
    Out,
    Store,
}
