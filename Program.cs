namespace AlgorithmDemo;

class Program
{
    static Graph CreateGraph()
    {
        Graph graph = new Graph();

        Node nodeA = new Node('a', 21);
        Node nodeB = new Node('b', 14);
        Node nodeC = new Node('c', 18);
        Node nodeD = new Node('d', 18);
        Node nodeE = new Node('e', 5);
        Node nodeF = new Node('f', 8);
        Node nodeZ = new Node('z', 0);

        graph.AddNode(nodeA);
        graph.AddNode(nodeB);
        graph.AddNode(nodeC);
        graph.AddNode(nodeD);
        graph.AddNode(nodeE);
        graph.AddNode(nodeF);
        graph.AddNode(nodeZ);

        graph.AddEdge(nodeA, nodeB, 9);
        graph.AddEdge(nodeA, nodeC, 4);
        graph.AddEdge(nodeA, nodeD, 7);
        graph.AddEdge(nodeB, nodeE, 11);
        graph.AddEdge(nodeC, nodeE, 17);
        graph.AddEdge(nodeC, nodeF, 12);
        graph.AddEdge(nodeD, nodeF, 14);
        graph.AddEdge(nodeE, nodeZ, 5);
        graph.AddEdge(nodeF, nodeZ, 9);

        return graph;
    }

    static int PathCost(List<Node> path)
    {
        int totalCost = 0;

        for (int i = 0; i < path.Count - 1; i++)
        {
            var edge = path[i].Edges.First(e => e.Node == path[i + 1]);
            totalCost += edge.GValue;
        }

        return totalCost;
    }

    static void Main(string[] args)
    {
        Graph graph = CreateGraph();

        Node startNode = graph.GetNode('a');
        Node goalNode = graph.GetNode('z');

        if (startNode == null || goalNode == null)
        {
            Console.WriteLine("Start or goal node not found in graph!");
            return;
        }

        Console.WriteLine(
            $"Finding shortest path from {startNode.Name} to {goalNode.Name} using A* search algorithm...");

        AStarAlgorithm algorithm = new AStarAlgorithm();
        List<Node> path = algorithm.GetPath(graph, startNode, goalNode);

        if (path.Count > 0)
        {
            Console.WriteLine("\nShortest path found:");
            Console.Write(string.Join(" -> ", path.Select(n => n.Name)));

            int totalCost = PathCost(path);
            Console.WriteLine($"\nTotal path cost: {totalCost}");

            Console.WriteLine("\nStep by step:");
            for (int i = 0; i < path.Count - 1; i++)
            {
                var edge = path[i].Edges.First(e => e.Node == path[i + 1]);
                Console.WriteLine($"{path[i].Name} -> {path[i + 1].Name}: {edge.GValue}");
            }
        }
        else
        {
            Console.WriteLine("No path found!");
        }
    }
}