using SFML.Graphics;
using SFML.System;

namespace Views
{

    public class Rectangle : Node
    {
        private RectangleShape _rectangle;
        private int _id;

        public RectangleShape RectangleShape => _rectangle;


        public Rectangle(Vector2f size, Color color, int id) : base(new Vector2f())
        {
            _rectangle = new RectangleShape(size);
            _rectangle.FillColor = color;
            _id = id;
        }

        public override void Draw(RenderWindow window)
        {
            _rectangle.Position = Position + Parent.Position;
            window.Draw(_rectangle);
        }

        public override Vector2f GetSize()
        {
            return _rectangle.Size;
        }

        public override bool IsPointInNode(Vector2f point)
        {
            var childPosition = Position + Parent.Position;
            var childSize = GetSize();

            return point.X >= childPosition.X && point.X <= childPosition.X + childSize.X &&
                   point.Y >= childPosition.Y && point.Y <= childPosition.Y + childSize.Y;
        }
    }
}
