namespace AlgorithmDemo;

/*
Lớp Edge - sử dụng để lưu trữ thông tin node được liên kết tới và giá trị G(n) của liên kết đó (node A tới node B với chiều dài là bao nhiêu)
*/
public class Edge
{
    public Node Node { get; private set; }
    public int GValue { get; private set; }

    public Edge(Node node, int gValue)
    {
        this.Node = node;
        GValue = gValue;
    }
}