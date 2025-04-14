namespace AlgorithmDemo;

/*
Lớp Graph - lưu trử thông tin đồ thị
Hiểu nôm na thì lớp này sử dụng 1 từ điển để lưu trữ thông tin các node có trong đồ thị đó
Từ điển được sử dụng để gán cho 1 biến (hoặc 1 object) nào đó 1 cái key nhằm tìm kiếm nhanh và dễ dàng hơn (giống như từ điển ngoài đời thật dùng để tìm kiếm thông tin/định nghĩa của 1 từ nào đó)
Khai báo: Dictionary<kiểu dữ liệu của khóa - kiểu dữ liệu của thông tin muốn khóa truy cập tới> tên từ điển
*/
public class Graph
{
    private Dictionary<char, Node> _nodes;

    //Constructor cơ bản/Khởi tạo rỗng
    public Graph()
    {
        _nodes = new Dictionary<char, Node>();
    }

    //Thêm node vào đồ thị - Đơn giản là kiểm tra khóa đã tồn tại trong từ điển chưa
    public void AddNode(Node node)
    {
        //Chưa tồn tại -> Thêm entry mới vào từ điển với khóa là tên của node mới thêm, giá trị muốn khóa đó truy cập tới là thông tin của node đó
        if (!_nodes.ContainsKey(node.Name))
        {
            _nodes[node.Name] = node;
        }
    }
    
    //Lấy thông tin của node dựa vào 1 char được truyền vào - Kiểm tra khóa đã tồn đại trong từ điển chưa, chưa => trả về null (rỗng), rồi -> trả về thông tin của node có khóa đã tìm thấy
    public Node GetNode(char label)
    {
        if (_nodes.ContainsKey(label))
            return _nodes[label];
        return null;
    }

    //Thêm liên kết giữa các node - Thêm cả 2 bên (A nối với B và B nối với A) (Thêm 1 lần máy chưa chắc hiểu đâu/Cấu trúc truy xuất nó sẽ lằng nhằng hơn nên thêm 2 lần cho ăn chắc :v)
    public void AddEdge(Node source, Node target, int value)
    {
        source.AddEdge(target, value);
        target.AddEdge(source, value);
    }
}