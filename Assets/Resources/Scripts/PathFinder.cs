using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node // : IComparable<Node>
{
    public Teleporter teleporter;
    public Vector3 position;
    public float distanceFromStart;
    public Node parent;

    /*
    public int CompareTo(Node other)
    {
        return distanceFromStart < other.distanceFromStart ? -1 : 1;
    }
    */
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
    public void RegisterTeleporter(Teleporter teleporter)
    {
        Teleporters.Add(teleporter);
    }

    public void UnregisterTeleporter(Teleporter teleporter)
    {
        Teleporters.Remove(teleporter);
    }

    public List<Vector3> GetPath(Vector3 start, Vector3 end)
    {
        MakeGraph(start, end);

        List<Node> queue = new List<Node>();
        List<Node> visited = new List<Node>();

        Teleporter teleporterStart = GetNearest(start);
        Teleporter teleporterEnd = GetNearest(end);

        Node nodeStart = new Node
        {
            teleporter = teleporterStart,
            distanceFromStart = (start - teleporterStart.Parent.Position).sqrMagnitude,
            parent = null,
        };

        Node nodeEnd = new Node
        {
            teleporter = teleporterEnd,
            distanceFromStart = (start - teleporterEnd.Parent.Position).sqrMagnitude,
            parent = null,
        };

        queue.Add(nodeStart);

        while (queue.Count > 0)
        {
            Node node = queue.First();
            queue.Remove(node);

            if (node.teleporter == nodeEnd.teleporter)
            {
                // return MoveBack(node, start, end);
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

            if (node.teleporter.Parent.GetComponent<PowerPlant>().PowerUpTime.Finished == false)
            {
                continue;
            }

            foreach (Teleporter i in node.teleporter.Connections)
            {
                queue.Add(new Node { teleporter = i, distanceFromStart = node.distanceFromStart, parent = node });
            }
        }

        return new List<Vector3>();
    }

    private void MakeGraph(Vector3 start, Vector3 end)
    {
        Nodes.Clear();

        Nodes.Add(new Node { position = start, distanceFromStart = 0.0f });
        Nodes.Add(new Node { position = end, distanceFromStart = 0.0f });

        Edges.Add(new KeyValuePair<int, int>(0, 1));

        foreach (Teleporter teleporter in FindObjectsByType<Teleporter>(FindObjectsSortMode.None))
        {
            int teleporterIndex = Nodes.FindIndex(x => x.teleporter == teleporter);

            if (teleporterIndex == -1)
            {
                Nodes.Add(new Node { teleporter = teleporter, distanceFromStart = 0.0f });

                teleporterIndex = Nodes.Count - 1;
            }

            foreach (Teleporter neighbour in teleporter.Connections)
            {
                int neighbourIndex = Nodes.FindIndex(x => x.teleporter == teleporter);

                if (neighbourIndex == -1)
                {
                    Nodes.Add(new Node { teleporter = teleporter, distanceFromStart = 0.0f });

                    neighbourIndex = Nodes.Count - 1;
                }

                if (Edges.FindIndex(x => x.Value == teleporterIndex && x.Key == neighbourIndex) == -1)
                {
                    Edges.Add(new KeyValuePair<int, int>(teleporterIndex, neighbourIndex));
                }
            }
        }
    }

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

    private List<Node> Nodes = new List<Node>();

    private List<KeyValuePair<int, int>> Edges = new List<KeyValuePair<int, int>>();

    private HashSet<Teleporter> Teleporters = new HashSet<Teleporter>();
}
