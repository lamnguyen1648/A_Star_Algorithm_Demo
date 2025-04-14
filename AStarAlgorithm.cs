namespace AlgorithmDemo;

/*
 * Thuật toán A*
 * Thuật toán tìm đường đi ngắn nhất bằng cách tính và so sánh F(n) giữa các node và đưa ra giải pháp ngắn nhất dựa trên kết quả F(n) đã tính ra
 * Note: giải pháp dưới chưa tính tới chuyện liên kết với F(n) với node ban đầu rất cao nhưng F(n) với các node kế tiếp rất thấp :v
 */
public class AStarAlgorithm
{
    /*
     * Lớp Record - Lưu trữ thông tin trong thời gian thực thi thuật toán
     * Khi thực thi, giá trị F(n) của các node được cập nhật liên tục -> ko nên sử dụng danh sách (List) để lưu trữ thông tin
     */
    private class Record
    {
        public Node Node { get; }
        public Record Parent { get; set;  }
        public double GScore { get; set; }
        public double HScore { get; }
        public double FScore => GScore + HScore;

        //F(n) tự tính theo G(n) và H(n) -> Không cần phải khai báo trong constructor
        public Record(Node node, Record parent, double gScore, double hScore)
        {
            Node = node;
            Parent = parent;
            GScore = gScore;
            HScore = hScore;
            //FScore = GScore + HScore (Viết thế này cũng được)
        }
    }

    /*
     * Function trả về kết quả đường đi với record được truyền vào
     * Nôm na thì tạo 1 danh sách Node mới, đặt node hiện tại là node được truyền vào, tạo vòng lặp với điều kiện điểm khởi đầu không trả về null (Nghĩa là đã đạt tới node khởi đầu), thêm node hiện tại vào danh sách và đặt node hiện tại là node cha của node hiện tại cũ và lặp lại
     * Đến cuối thì vặn ngược danh sách và trả danh sách đó
     */
    private List<Node> ToPath(Record end)
    {
        var path = new List<Node>();
        var current = end;

        while (current != null)
        {
            path.Add(current.Node);
            current = current.Parent;
        }
        
        path.Reverse();
        return path;
    }

    /*
     * Logic thuật toán
     */
    public List<Node> GetPath(Graph graph, Node start, Node end)
    {
        var evaluateList = new List<Record>();
        var passedNodes = new HashSet<Node>();
        var recordDictionary = new Dictionary<Node, Record>();

        var startNode = new Record(start, null, 0, start.HValue);
        evaluateList.Add(startNode);
        recordDictionary[start] = startNode;

        while (evaluateList.Count > 0)
        {
            var current = evaluateList.OrderBy(e => e.FScore).First();
            evaluateList.Remove(current);

            if (current.Node == end)
            {
                return ToPath(current);
            }
            
            passedNodes.Add(current.Node);

            foreach (var edge in current.Node.Edges)
            {
                Node neighbor = edge.Node;
                if (passedNodes.Contains(neighbor)) continue;

                double tmpGScore = current.GScore + edge.GValue;

                bool found = recordDictionary.TryGetValue(neighbor, out var neighborRecord);

                if (!found)
                {
                    neighborRecord = new Record(neighbor, current, tmpGScore, neighbor.HValue);
                    evaluateList.Add(neighborRecord);
                    recordDictionary[neighbor] = neighborRecord;
                }
                else if (neighborRecord != null && tmpGScore < neighborRecord.GScore)
                {
                    neighborRecord.Parent = current;
                    neighborRecord.GScore = tmpGScore;
                }
            }
        }

        return new List<Node>();
    }
}