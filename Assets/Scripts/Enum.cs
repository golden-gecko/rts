public enum DamageType
{
    Kinetic = 0,
    Laser = 1,
    Fire = 2, // TODO: Implement.
}

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

public enum GameState
{
    Game = 0,
    Editor = 1,
    Menu = 2,
}

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

public enum OrderState
{
    None = 0,
    GoToSource = 1,
    GoToTarget = 2,
    Open = 3,
    Close = 4,
    GoToEntrance = 5,
    GoToExit = 6,
    Teleport = 7,
}

public enum OrderType
{
    Assemble = 0,
    AttackObject = 1,
    Construct = 2,
    Destroy = 3,
    Disable = 4,
    Enable = 5,
    Explore = 6,
    Follow = 7,
    GatherObject = 8,
    GuardObject = 9,
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
    Turn = 24,
    AttackPosition = 25,
    GuardPosition = 26,
    GatherResource = 27,
    Teleport = 28,
}

public enum ResourceDirection
{
    In = 0,
    None = 1,
    Out = 2,
    Store = 3,
}
