using System.Collections.Generic;

public class Cell
{
    public Dictionary<Player, int> Explored = new Dictionary<Player, int>();
    public Dictionary<Player, int> VisibleByPower = new Dictionary<Player, int>();
    public Dictionary<Player, int> VisibleByPowerRelay = new Dictionary<Player, int>();
    public Dictionary<Player, int> VisibleByRadar = new Dictionary<Player, int>();
    public Dictionary<Player, int> VisibleByAntiRadar = new Dictionary<Player, int>();
    public Dictionary<Player, int> VisibleBySight = new Dictionary<Player, int>();
}
