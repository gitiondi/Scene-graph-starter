using SFML.Graphics;
using SFML.System;

namespace Views
{
    public class Rectangles
    {
        private RectangleShape _rectangle;

        public Vector2f Position
        {
            get => _rectangle.Position;
            set => _rectangle.Position = value;
        }

        public Rectangles()
        {
        }

        public void AddRectangle(Vector2f size, Color color)
        {
            _rectangle = new RectangleShape(new Vector2f(50, 50));
            _rectangle.FillColor = Color.Yellow;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(_rectangle);
        }
    }
}
