using SFML.Graphics;
using SFML.System;

namespace Views
{
    public abstract class Node
    {
        public virtual Vector2f Position { get; set; }

        public GroupNode Parent { get; set; }

        public Node(Vector2f position)
        {
            Position = position;
        }

        public abstract Vector2f GetSize();

        public abstract bool IsPointInNode(Vector2f point);

        public virtual void Move(Vector2f offset)
        {
            Position += offset;
        }

        public virtual void Draw(RenderWindow window)
        {
            // Override in derived classes to draw the node
        }
    }
}
