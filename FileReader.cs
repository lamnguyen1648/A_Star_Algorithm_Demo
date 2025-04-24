namespace AlgorithmDemo;

public class FileReader
{
    public static Graph ReadFile(string filePath)
    {
        Graph graph = new Graph();
        Dictionary<char, Node> nodes = new Dictionary<char, Node>();

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            int lineIndex = 0;

            while (lineIndex < lines.Length &&
                   (string.IsNullOrWhiteSpace(lines[lineIndex]) || lines[lineIndex].StartsWith("#")))
            {
                lineIndex++;
            }

            while (lineIndex < lines.Length && !string.IsNullOrWhiteSpace(lines[lineIndex]))
            {
                string line = lines[lineIndex];

                if (line.StartsWith("#"))
                {
                    lineIndex++;
                    continue;
                }
                
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 2)
                {
                    char nodeName = parts[0][0];
                    int HValue = int.Parse(parts[1]);
                    
                    Node node = new Node(nodeName, HValue);
                    nodes[nodeName] = node;
                    graph.AddNode(node);

                    lineIndex++;
                }
            }

            while (lineIndex < lines.Length && string.IsNullOrWhiteSpace(lines[lineIndex]))
            {
                lineIndex++;
            }

            while (lineIndex < lines.Length)
            {
                string line = lines[lineIndex].Trim();
                
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    lineIndex++;
                    continue;
                }
                
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3)
                {
                    char sourceName = parts[0][0];
                    char targetName = parts[1][0];
                    int GValue = int.Parse(parts[2]);

                    if (nodes.TryGetValue(sourceName, out Node source) &&
                        nodes.TryGetValue(targetName, out Node target))
                    {
                        graph.AddEdge(source, target, GValue);
                    }
                }
                lineIndex++;
            }

            return graph;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi đọc file: ", ex.Message);
            return new Graph();
        }
    }
}