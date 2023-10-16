public enum DiplomacyState
{
    Ally = 0,
    Enemy = 1,
    Neutral = 2,
}

public enum Formation
{
    None = 0,
    Arrow = 1,
    Column = 2,
    Diamond = 3,
    Line = 4,
    Square = 5,
    Triangle = 6,
    Wedge = 7,
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
    None = 1,
    Terrain = 2,
    Underwater = 3,
    Water = 4,
    Hover = 5,
    Submerged = 6,
}

public enum MyGameObjectVisibilityState
{
    Hidden = 0,
    Radar = 1,
    Visible = 2,
    Explored = 3,
}

public enum MyGameObjectState
{
    Cursor = 0,
    Operational = 1,
    UnderAssembly = 2,
    UnderConstruction = 3,
}

public enum OrderType
{
    Assemble = 0,
    Attack = 1,
    Construct = 2,
    Destroy = 3,
    Disable = 4,
    Enable = 5,
    Explore = 6,
    Follow = 7,
    Gather = 8,
    Guard = 9,
    Idle = 10,
    Load = 11,
    Move = 12,
    None = 13,
    Patrol = 14,
    Produce = 15,
    Rally = 16,
    Research = 17,
    Stock = 18,
    Stop = 19,
    Transport = 20,
    Unload = 21,
    UseSkill = 22,
    Wait = 23,
    Turn = 23,
}

public enum ResourceDirection
{
    In = 0,
    None = 1,
    Out = 2,
    Store = 3,
}
