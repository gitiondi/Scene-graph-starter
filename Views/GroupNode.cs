using SFML.Graphics;
using SFML.System;
using Views;

public class GroupNode : Node
{
    private List<Node> _children;

    public List<Node> Children => _children;

    public GroupNode(Vector2f position) : base(position)
    {
        _children = new List<Node>();
    }

    public void AddChild(Node child)
    {
        child.Parent = this;
        _children.Add(child);
    }

    public override Vector2f GetSize()
    {
        throw new NotImplementedException();
    }

    public override void Draw(RenderWindow window)
    {
        foreach (var child in _children)
        {
            child.Draw(window);
        }
    }

    public Node this[int index]
    {
        get => _children[index];
    }

}
