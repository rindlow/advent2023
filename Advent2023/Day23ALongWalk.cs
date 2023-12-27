using System.ComponentModel;
using System.Reflection.Metadata;
using System.Text;
using AnsiColor;
using Microsoft.VisualBasic;

namespace Advent2023;

sealed class TrailNode
{
    public List<Position> Positions { get; set; } = [];
    public HashSet<TrailNode> Edges { get; set; } = [];
    public int Label { get; }
    private static int _labelIndex;
    public TrailNode(Position pos)
    {
        Label = _labelIndex++;
        Positions.Add(pos);
        // Console.WriteLine($"TrailNode {Label} created at {pos}");
    }
    public override string ToString()
    {
        return $"<TrailNode {Label}>";
    }
}
sealed class Trails
{
    private readonly string[] _map;
    private readonly Position _start;
    private readonly Position _end;

    public Trails(string filename)
    {
        _map = File.ReadAllLines(filename);
        _start = new Position(0, _map[0].IndexOf('.'));
        _end = new Position(_map.Length - 1, _map[^1].IndexOf('.'));
    }
    private void Print(Dictionary<Position, TrailNode> nodes)
    {
        foreach (int row in Enumerable.Range(0, _map.Length))
        {
            foreach (int col in Enumerable.Range(0, _map[0].Length))
            {
                if (nodes.TryGetValue(new Position(row, col), out TrailNode? node))
                {
                    Console.Write($"{node.Label.ToString()[^1]}".Bold());
                }
                else
                {
                    Console.Write(_map[row][col]);
                }
            }
            Console.WriteLine();
        }

    }
    private List<Position> Neighbours(Position pos)
    {
        List<Position> neighbours = [];
        if (pos.Row > 0 && _map[pos.Row - 1][pos.Col] != '#')
        {
            neighbours.Add(new Position(pos.Row - 1, pos.Col));
        }
        if (pos.Row < _map.Length - 1 && _map[pos.Row + 1][pos.Col] != '#')
        {
            neighbours.Add(new Position(pos.Row + 1, pos.Col));
        }
        if (pos.Col > 0 && _map[pos.Row][pos.Col - 1] != '#')
        {
            neighbours.Add(new Position(pos.Row, pos.Col - 1));
        }
        if (pos.Col < _map[0].Length - 1 && _map[pos.Row][pos.Col + 1] != '#')
        {
            neighbours.Add(new Position(pos.Row, pos.Col + 1));
        }
        return neighbours;
    }
    public delegate bool Slippery(Position pos1, Position pos2);
    public bool Restricted(Position pos, Position last)
    {
        return _map[pos.Row][pos.Col] switch
        {
            '>' => pos.Col < last.Col,
            '<' => pos.Col >= last.Col,
            'v' => pos.Row < last.Row,
            '^' => pos.Row >= last.Row,
            _ => false,
        };
    }
    public bool Dry(Position pos1, Position pos2)
    {
        return false;
    }
    public HashSet<TrailNode> Graph(Slippery slippery)
    {
        List<TrailNode> graph = [];
        Queue<Position> queue = new();
        queue.Enqueue(_start);
        Dictionary<Position, TrailNode> nodes = [];
        nodes[_start] = new TrailNode(_start);
        Dictionary<Position, Position> cameFrom = [];
        bool onlyBackward = false;
        HashSet<Position> deferred = [];
        while (queue.Count > 0)
        {
            Position current = queue.Dequeue();
            TrailNode currentNode = nodes[current];
            Position? lastPos = null;
            if (cameFrom.TryGetValue(current, out Position last))
            {
                lastPos = last;
                if (slippery(current, last))
                {
                    nodes[last].Edges.Remove(currentNode);
                    Position backTrace = last;
                    while (nodes[backTrace] == currentNode)
                    {
                        nodes.Remove(backTrace);
                        if (cameFrom.TryGetValue(backTrace, out Position bt))
                        {
                            backTrace = bt;
                        }
                    }
                    nodes.Remove(current);
                    onlyBackward = true;
                    continue;
                }
                if (slippery(last, current))
                {
                    currentNode.Edges.Clear();
                }
            }
            IEnumerable<Position> neighbours = from n in Neighbours(current)
                                               where n != lastPos && !currentNode.Positions.Contains(n)
                                               select n;
            switch (neighbours.Count())
            {
                case 0:
                    continue;
                case 1:
                    Position next = neighbours.First();
                    if (slippery(next, current))
                    {
                        continue;
                    }
                    if (nodes.TryGetValue(next, out TrailNode? nextNode))
                    {
                        if (nextNode.Positions.Count == 1)
                        {
                            currentNode.Edges.Add(nextNode);
                        }
                        else
                        {
                            if (!deferred.Contains(current))
                            {
                                queue.Enqueue(current);
                                deferred.Add(current);
                            }
                            else
                            {
                                deferred.Remove(current);
                                foreach (TrailNode edge in nextNode.Edges)
                                {
                                    currentNode.Edges.Add(edge);
                                    edge.Edges.Remove(nextNode);
                                    edge.Edges.Add(currentNode);
                                }
                                foreach (Position pos in nextNode.Positions)
                                {
                                    nodes[pos] = currentNode;
                                    currentNode.Positions.Add(pos);
                                }
                            }
                        }
                        continue;
                    }
                    cameFrom[next] = current;
                    nodes[next] = currentNode;
                    currentNode.Positions.Add(next);
                    queue.Enqueue(next);
                    break;
                default:
                    TrailNode intersection = new(current);
                    nodes[current] = intersection;
                    currentNode.Positions.Remove(current);
                    currentNode.Edges.Add(intersection);
                    if (!slippery(last, current))
                    {
                        intersection.Edges.Add(currentNode);
                    }
                    foreach (Position neighbour in neighbours)
                    {
                        if (slippery(neighbour, current))
                        {
                            continue;
                        }
                        if (nodes.TryGetValue(neighbour, out TrailNode? neighbourNode))
                        {
                            intersection.Edges.Add(neighbourNode);
                            continue;
                        }
                        cameFrom[neighbour] = current;
                        TrailNode newNode = new(neighbour);
                        newNode.Edges.Add(intersection);
                        nodes[neighbour] = newNode;
                        if (!onlyBackward)
                        {
                            intersection.Edges.Add(newNode);
                        }
                        queue.Enqueue(neighbour);
                    }
                    break;
            }
        }
        // Print(nodes);
        return [.. nodes.Values];
    }
    public void GraphViz(HashSet<TrailNode> graph)
    {
        StringBuilder sb = new();
        sb.AppendLine("digraph G {");
        foreach (TrailNode node in graph)
        {
            string shape = "ellipse";
            if (node.Positions.Contains(_start))
            {
                shape = "triangle";
            }
            if (node.Positions.Contains(_end))
            {
                shape = "box";
            }
            sb.AppendLine($"  {node.Label} [label = \"{node.Label} ({node.Positions.Count})\"; shape = \"{shape}\"]");
            foreach (TrailNode edge in node.Edges)
            {
                sb.AppendLine($"  {node.Label} -> {edge.Label}");
            }
        }
        sb.AppendLine("}");
        File.WriteAllText("day23.gv", sb.ToString());
    }
    public int LongestPath(HashSet<TrailNode> graph)
    {
        Console.WriteLine("LongestPath()");
        Dictionary<TrailNode, int> longest = [];
        Queue<TrailNode> queue = new();
        TrailNode start = (from node in graph
                           where node.Positions.Contains(_start)
                           select node).First();
        TrailNode end = (from node in graph
                         where node.Positions.Contains(_end)
                         select node).First();
        queue.Enqueue(start);
        longest[start] = -1;
        while (queue.Count > 0)
        {
            TrailNode current = queue.Dequeue();
            // Console.WriteLine($"looking at node {current.Label}");
            int count = current.Positions.Count + longest[current];
            // Console.WriteLine($"  count = {current.Positions.Count} + {longest[current]} = {count}");
            foreach (TrailNode node in current.Edges)
            {
                // Console.WriteLine($"  examining edge to {node.Label}");
                // Console.Write($"{current.Label}-{node.Label}: ");
                if (longest.TryGetValue(node, out int nodeValue))
                {
                    if (count > nodeValue)
                    {
                        longest[node] = count;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    longest[node] = count;
                }
                queue.Enqueue(node);
            }
        }
        Console.WriteLine($"returning {longest[end] + end.Positions.Count}");
        return longest[end] + end.Positions.Count;
    }
    public static int LongestRec(TrailNode start, TrailNode end, HashSet<TrailNode> visited, string indent)
    {
        if (start == end)
        {
            return start.Positions.Count;
        }
        var edges = from edge in start.Edges where !visited.Contains(edge) select edge;
        if (!edges.Any())
        {
            return -1000000;
        }
        HashSet<TrailNode> v = new(visited) { start };
        List<int> res = (from edge in edges select LongestRec(edge, end, v, $"  {indent}")).ToList();
        return start.Positions.Count + res.Max();
    }
    public int LongestPath2(HashSet<TrailNode> graph)
    {
        TrailNode start = (from node in graph
                           where node.Positions.Contains(_start)
                           select node).First();
        TrailNode end = (from node in graph
                         where node.Positions.Contains(_end)
                         select node).First();
        return LongestRec(start, end, [], "") - 1;
    }
}
public static class Day23ALongWalk
{
    public static int LongestHike(string filename)
    {
        Trails trails = new(filename);
        HashSet<TrailNode> graph = trails.Graph(trails.Restricted);

        trails.GraphViz(graph);
        return trails.LongestPath(graph);
    }
    public static int LongestDryHike(string filename)
    {
        Trails trails = new(filename);
        HashSet<TrailNode> graph = trails.Graph(trails.Dry);

        trails.GraphViz(graph);
        return trails.LongestPath2(graph);
    }
}
