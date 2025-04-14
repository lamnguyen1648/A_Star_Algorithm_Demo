namespace AlgorithmDemo;

/*
Bắt đầu từ lớp Node - sử dụng để lưu giữ thông tin của các node bao gồm tên, giá trị H(n) và liên kết giữa các node (node A nối với các node nào)
*/
public class Node
{
    public char Name { get; private set; }
    public int HValue { get; private set; }
    public List<Edge> Edges { get; }

    //Constructor chỉ bao gồm tên và giá trị H(n), danh sách liên kết của 1 node ban đầu được khởi tạo là 1 danh sách rỗng
    public Node(char name, int hValue)
    {
        Name = name;
        HValue = hValue;
        Edges = new List<Edge>();
    }

    //Function thêm
    public void AddEdge(Node target, int value)
    {
        Edges.Add(new Edge(target, value));
    }
}