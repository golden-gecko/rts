using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Teleporter teleporter;
    public Vector3 position;

    // public float distanceFromStart;
    // public Node parent;
}

public class NodeComparer : IEqualityComparer<Node>
{
    public bool Equals(Node x, Node y)
    {
        return x.teleporter == y.teleporter;
    }

    public int GetHashCode(Node obj)
    {
        return obj.teleporter.GetHashCode();
    }
}

public class PathFinder : Singleton<PathFinder>
{
    public List<Order> GetPath(Vector3 start, Vector3 end)
    {
        Teleporter nodeStart = GetNearest(start);
        Teleporter nodeEnd = GetNearest(end);

        if (nodeStart == null || nodeEnd == null)
        {
            return new List<Order>
            {
                Order.Move(end),
            };
        }

        if (nodeStart == nodeEnd)
        {
            return new List<Order>
            {
                Order.Move(end),
            };
        }

        float direct = (end - start).sqrMagnitude;
        float teleport = (nodeStart.Parent.Position - start).sqrMagnitude + (nodeEnd.Parent.Position - end).sqrMagnitude;

        if (direct <= teleport)
        {
            return new List<Order>
            {
                Order.Move(end),
            };
        }

        return new List<Order>
        {
            Order.Teleport(nodeStart.Parent, nodeEnd.Parent),
            Order.Move(end),
        };
    }

    /*
    public List<Node> GetPathUsingConnections(Vector3 start, Vector3 end)
    {
        // MakeGraph(start, end);

        List<Node> queue = new List<Node>();
        List<Node> visited = new List<Node>();

        Teleporter nodeStart = GetNearest(start);
        Teleporter nodeEnd = GetNearest(end);

        if (nodeStart == nodeEnd)
        {
            return new List<Node>
            {
                new Node { position = start },
                new Node { position = end },
            };
        }

        queue.Add(new Node { teleporter = nodeStart });

        while (queue.Count > 0)
        {
            Node node = queue.First();
            queue.Remove(node);

            if (node.teleporter == nodeEnd)
            {
                return MakePath(node, end);
            }

            if (visited.Contains(node, new NodeComparer()))
            {
                continue;
            }

            visited.Add(node);

            if (node.teleporter.Parent.State != MyGameObjectState.Operational || node.teleporter.Parent.Enabled == false)
            {
                continue;
            }

            if (node.teleporter.Parent.GetComponentInChildren<PowerPlant>().PowerUpTime.Finished == false)
            {
                continue;
            }

            foreach (Teleporter i in node.teleporter.Connections)
            {
                queue.Add(new Node { teleporter = i, parent = node });
            }
        }

        return new List<Node>
        {
            new Node { position = start },
            new Node { position = end },
        };
    }

    private void MakeGraph(Vector3 start, Vector3 end)
    {
        Nodes.Clear();
        Edges.Clear();

        // Nodes.Add(new Node { position = start, distanceFromStart = 0.0f });
        // Nodes.Add(new Node { position = end, distanceFromStart = 0.0f });

        // Edges.Add(new KeyValuePair<int, int>(0, 1));

        foreach (Teleporter i in FindObjectsByType<Teleporter>(FindObjectsSortMode.None))
        {
            int teleporterIndex = Nodes.FindIndex(x => x.teleporter == i);

            if (teleporterIndex == -1)
            {
                Nodes.Add(new Node { teleporter = i, distanceFromStart = 0.0f });

                teleporterIndex = Nodes.Count - 1;
            }

            foreach (Teleporter j in i.Connections)
            {
                int neighbourIndex = Nodes.FindIndex(x => x.teleporter == j);

                if (neighbourIndex == -1)
                {
                    Nodes.Add(new Node { teleporter = j, distanceFromStart = 0.0f });

                    neighbourIndex = Nodes.Count - 1;
                }

                if (Edges.FindIndex(x => (x.Value == teleporterIndex && x.Key == neighbourIndex) || (x.Value == neighbourIndex && x.Key == teleporterIndex)) == -1)
                {
                    Edges.Add(new KeyValuePair<int, int>(teleporterIndex, neighbourIndex));
                }
            }
        }

        // Edges.Add(new KeyValuePair<int, int>(0, Nodes.FindIndex(x => x.teleporter == GetNearest(start))));
        // Edges.Add(new KeyValuePair<int, int>(1, Nodes.FindIndex(x => x.teleporter == GetNearest(end))));
    }
    */

            private Teleporter GetNearest(Vector3 position)
    {
        float minDistance = float.MaxValue;
        Teleporter closest = null;

        foreach (Teleporter teleporter in FindObjectsByType<Teleporter>(FindObjectsSortMode.None))
        {
            float distance = (position - teleporter.Parent.Position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = teleporter;
            }
        }

        return closest;
    }

    /*
    private List<Node> MakePath(Node node, Vector3 end)
    {
        List<Node> path = new List<Node>();

        while (node != null)
        {
            path.Insert(0, node);

            node = node.parent;
        }

        path.Add(new Node { position = end });

        return path;
    }
    */

    // private List<Node> Nodes = new List<Node>();

    // private List<KeyValuePair<int, int>> Edges = new List<KeyValuePair<int, int>>();
}
