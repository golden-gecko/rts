using System.Collections.Generic;

public struct ValueGameObjects
{
    public ValueGameObjects(int value = 0)
    {
        Value = value;
        GameObjects = new HashSet<MyGameObject>();
    }

    public void SetValue(int value)
    {
        Value = value;
    }

    public int Value;
    public HashSet<MyGameObject> GameObjects;
}

public class Cell
{
    public Dictionary<Player, ValueGameObjects> Occupied = new Dictionary<Player, ValueGameObjects>();
    public Dictionary<Player, ValueGameObjects> Explored = new Dictionary<Player, ValueGameObjects>();

    public Dictionary<Player, int> VisibleByAntiRadar = new Dictionary<Player, int>();
    public Dictionary<Player, int> VisibleByPower = new Dictionary<Player, int>();
    public Dictionary<Player, int> VisibleByRadar = new Dictionary<Player, int>();
    public Dictionary<Player, int> VisibleBySight = new Dictionary<Player, int>();

    public Dictionary<Player, float> VisibleByPassiveDamage = new Dictionary<Player, float>();
    public Dictionary<Player, float> VisibleByPassivePower = new Dictionary<Player, float>();
    public Dictionary<Player, float> VisibleByPassiveRange = new Dictionary<Player, float>();
}
