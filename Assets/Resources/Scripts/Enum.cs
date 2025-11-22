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
}

public enum MyGameObjectState
{
    Cursor,
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

public enum PositionHandlerType
{
    Camera,
    Plane,
    Structure,
    Ship,
    Vehicle,
}

public enum ResourceDirection
{
    Both,
    In,
    None,
    Out
}
