using SFML.Graphics;
using SFML.System;

namespace Views
{
    public class Rectangle
    {
        private RectangleShape _rectangle;
        private int _id;

        public Vector2f Position
        {
            get => _rectangle.Position;
            set => _rectangle.Position = value;
        }

        public Rectangle(Vector2f size, Color color, int id)
        {
            _rectangle = new RectangleShape(size);
            _rectangle.FillColor = color;
            _id = id;
        }

        //public void AddRectangle(Vector2f size, Color color)
        //{
        //    _rectangle = new RectangleShape(new Vector2f(50, 50));
        //    _rectangle.FillColor = Color.Yellow;
        //}

        public void Draw(RenderWindow window)
        {
            window.Draw(_rectangle);
        }
    }
}
