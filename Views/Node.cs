using SFML.Graphics;
using SFML.System;

namespace Views
{
    public abstract class Node : Transformable, Drawable
    {
        private readonly List<Node> _children;
        protected Node? _parent;

        public Node()
        {
            _children = new List<Node>();
            _parent = null;
        }

        public IEnumerable<Node> Children => _children;

        public bool IsRoot => _parent == null;

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            //pass and combine the transform to the children transforms, 
            //so children transforms are calculated based on the root transform.
            states.Transform *= Transform;

            DrawCurrent(target, states);
            DrawChildren(target, states);
        }

        public virtual bool IsPointInNode(Vector2f point)
        {
            return false;
        }

        public void AddChild(Node child)
        {
            child._parent = this;
            _children.Add(child);
        }

        public Node RemoveChild(Node child)
        {
            _children.Remove(child);
            child._parent = null;
            return child;
        }

        public abstract void DrawCurrent(RenderTarget target, RenderStates states);

        public virtual void DrawChildren(RenderTarget target, RenderStates states)
        {
            foreach (var node in _children)
            {
                node.Draw(target, states);
            }
        }
    }
}