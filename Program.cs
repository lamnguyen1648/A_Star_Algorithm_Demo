namespace AlgorithmDemo;

class Program
{
    static void Main(string[] args)
    {
        //File đặt trong mục bin/Debug/net9.0/resources/graph.txt
        string filePath = "./resources/graph.txt";
        Graph graph = FileReader.ReadFile(filePath);

        Console.WriteLine("Danh sách các node có trong file:");
        HashSet<char> nodeNames = new HashSet<char>();
        foreach (var node in graph.GetAllNodes())
        {
            nodeNames.Add(node.Name);
            Console.Write($"{node.Name} ");
        }

        Console.WriteLine();

        if (nodeNames.Count == 0)
        {
            Console.WriteLine("File rỗng, vui lòng kiểm tra lại file");
            return;
        }

        Console.WriteLine("Nhập tên node khởi đầu: ");
        char startNode = Console.ReadLine().Trim().ToLower()[0];

        Console.WriteLine("Nhập tên node kết thúc: ");
        char endNode = Console.ReadLine().Trim().ToLower()[0];

        Node start = graph.GetNode(startNode);
        Node end = graph.GetNode(endNode);

        if (start == null || end == null)
        {
            Console.WriteLine("Không tìm thấy node khởi đầu hoặc node kết thúc, vui lòng kiểm tra lại");
            return;
        }

        Console.WriteLine($"Tìm đường đi ngắn nhất từ node {startNode} đến node {endNode}");
        AStarAlgorithm algorithm = new AStarAlgorithm();
        List<Node> path = algorithm.GetPath(graph, start, end);

        if (path.Count > 0)
        {
            List<List<Node>> paths = algorithm.getPathList();
            Console.WriteLine("Danh sách các đường đã đi:");
            for (int i = 0; i < paths.Count; i++)
            {
                int pathCost = AStarAlgorithm.PathCost(paths[i]);
                Console.WriteLine($"Đường đi {i+1}: {string.Join(" -> ", paths[i].Select(n => n.Name))} (Cost: {pathCost})");
            }
            
            Console.WriteLine("Đường đi tối ưu nhất:");
            Console.Write(string.Join(" -> ", path.Select(n => n.Name)));
            
            int totalCost = AStarAlgorithm.PathCost(path);
            Console.Write($" (Cost: {totalCost})\n");
            
            Console.WriteLine("Thứ tự đường đi:");
            for (int i = 0; i < path.Count - 1; i++)
            {
                var edge = path[i].Edges.First(e => e.Node == path[i + 1]);
                Console.WriteLine($"{path[i].Name} -> {path[i + 1].Name}: {edge.GValue}");
            }
        }
        else
        {
            Console.WriteLine("Không tìm thấy đường đi!!!");
        }
    }
}