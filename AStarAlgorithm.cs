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
    
    //List chứa các route đã đi
    private List<List<Node>> _exploredPath = new List<List<Node>>();

    //Trả về list
    public List<List<Node>> getPathList()
    {
        return _exploredPath;
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

    //Tính cost của path cụ thể
    public static int PathCost(List<Node> path)
    {
        int cost = 0;

        for (int i = 0; i < path.Count - 1; i++)
        {
            var edge = path[i].Edges.First(e=>e.Node == path[i + 1]);
            cost += edge.GValue;
        }

        return cost;
    }

    /*
     * Logic thuật toán
     */
    public List<Node> GetPath(Graph graph, Node start, Node end)
    {
        var evaluateList = new List<Record>();                      //Danh sách track các record cần được đánh giá
        var passedNodes = new HashSet<Node>();                      //Danh sách các node đã đi qua (Dùng list cũng được nhưng chỉ cần biết node đó đã đi qua hay chưa => dùng HashSet)
        var recordDictionary = new Dictionary<Node, Record>();      //Từ điển record - Dùng để lưu trữ thông tin của record cần được cập nhật liên tục

        //Bắt đầu thuật toán với việc khởi tạo record bắt đầu, thêm record vào trong danh sách và từ điển
        var startNode = new Record(start, null, 0, start.HValue);
        evaluateList.Add(startNode);
        recordDictionary[start] = startNode;

        List<Node> bestPath = null;
        double bestCost = double.MaxValue;

        //Khởi tạo vòng lặp với điều kiện danh sách track có bản ghi/số đếm > 0 (List có sẵn hàm Count)
        while (evaluateList.Count > 0)
        {
            //Lấy ra record đầu tiên với FScore thấp nhất, sau đó record khỏi danh sách cần được đánh giá và thêm vào danh sách các node đã đi qua
            var current = evaluateList.OrderBy(e => e.FScore).First();
            evaluateList.Remove(current);
            passedNodes.Add(current.Node);
            
            //Trong trường hợp node hiện tại là node cần tới, tạm dừng tìm kiếm đường đi, thêm đường đi mới tìm thấy vào danh sách các đường đã đi, tính cost của path vừa đi và so sánh với cost của đường đi tối ưu nhất đã tìm thấy, nếu là đường đi tối ưu nhất thì thay thế
            if (current.Node == end)
            {
                var currentPath = ToPath(current);
                double pathCost = PathCost(currentPath);
                
                _exploredPath.Add(new List<Node>(currentPath));
                if (pathCost < bestCost)
                {
                    bestCost = pathCost;
                    bestPath = currentPath;
                }

                continue;
            }

            //Xét các đường nối tới các node liền kề với node hiện tại
            foreach (var edge in current.Node.Edges)
            {
                Node neighbor = edge.Node;
                //node liền kề này đã đi qua -> bỏ qua sang trường hợp tiếp theo
                if (passedNodes.Contains(neighbor)) continue;

                //Tính điểm G(n) từ node hiện tại đi tới node tiếp theo
                double tmpGScore = current.GScore + edge.GValue;

                //Xét xem node liền kề này đã có trong từ điển chưa
                bool found = recordDictionary.TryGetValue(neighbor, out var neighborRecord);

                //Chưa có -> khởi tạo entry từ điển mới và thêm vào danh sách node cần được đánh giá
                if (!found)
                {
                    neighborRecord = new Record(neighbor, current, tmpGScore, neighbor.HValue);
                    evaluateList.Add(neighborRecord);
                    recordDictionary[neighbor] = neighborRecord;
                }
                //Đã có nhưng G(n) của bản ghi hiện tại < G(n) vừa tính ra -> cập nhật node cha và G(n)
                else if (neighborRecord != null && tmpGScore < neighborRecord.GScore)
                {
                    neighborRecord.Parent = current;
                    neighborRecord.GScore = tmpGScore;
                    
                    //Trong trường hợp node này đã được đánh giá nhưng tìm ra đường đi tối ưu hơn đường đi trước, cần phải đánh giá lại node này
                    if (passedNodes.Contains(neighbor))
                    {
                        passedNodes.Remove(neighbor);
                        evaluateList.Add(neighborRecord);
                    }
                }
            }
        }

        return bestPath ?? new List<Node>();
    }
}