using SFML.Graphics;
using SFML.System;

namespace Views
{
    public class Rectangle : Node
    {
        private readonly RectangleShape _rectangle;

        public Rectangle(Vector2f size, Color color, int id)
        {
            _rectangle = new RectangleShape(size);
            _rectangle.FillColor = color;
        }

        public override void DrawCurrent(RenderTarget target, RenderStates states)
        {
            target.Draw(_rectangle, states);
        }

        public override bool IsPointInNode(Vector2f point)
        {
            var positionInQuestion = IsRoot ? Position : Position + _parent.Position;
            return point.X >= positionInQuestion.X && point.X <= positionInQuestion.X + _rectangle.Size.X &&
                   point.Y >= positionInQuestion.Y && point.Y <= positionInQuestion.Y + _rectangle.Size.Y;
        }
    }
}
