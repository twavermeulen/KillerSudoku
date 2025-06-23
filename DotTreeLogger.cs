public class DotTreeLogger
{
    private int nodeCounter = 0;
    private StreamWriter writer;
    private Stack<int> stack = new();

    public DotTreeLogger(string filePath)
    {
        writer = new StreamWriter(filePath);
        writer.WriteLine("digraph G {");
    }

    public void PushNode(string label)
    {
        int currentId = nodeCounter++;
        if (stack.Count > 0)
        {
            int parentId = stack.Peek();
            writer.WriteLine($"    node{parentId} -> node{currentId};");
        }
        writer.WriteLine($"    node{currentId} [label=\"{label}\"];");
        stack.Push(currentId);
    }

    public void PopNode()
    {
        stack.Pop();
    }

    public void Close()
    {
        writer.WriteLine("}");
        writer.Close();
    }
}
