public enum DiplomacyState
{
    Ally,
    Enemy,
    Neutral,
}

public enum MyGameObjectMapLayer
{
    Air,
    Terrain,
    Underwater, // TODO: Implement.
    Water,

    Missile, // TODO: Remove.
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
    Both,
    In,
    None,
    Out,
    Store, // TODO: Implement as lower priority then In or Out and remove Both.
}
