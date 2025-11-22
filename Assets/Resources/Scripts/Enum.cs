public enum DiplomacyState
{
    Ally,
    Enemy,
    Neutral,
}

/*
 * Supported combinations:
 * - Air (missiles),
 * - Hover (planes),
 * - Submerged (submarines),
 * - Terrain (vehicles),
 * - Terrain and Underwater (vehicles),
 * - Terrain and Water (amphibious vehicles or hovercrafts),
 * - Water (ships).
 */
public enum MyGameObjectMapLayer
{
    Air = 0,
    Hover = 5,
    None = 1,
    Submerged = 6, // TODO: Implement submarines.
    Terrain = 2,
    Underwater = 3, // TODO: Rename (for vehicles driving on terrain underwater).
    Water = 4,
}

public enum MyGameObjectVisibilityState
{
    Hidden,
    Radar,
    Visible,
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
    Stock,
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
