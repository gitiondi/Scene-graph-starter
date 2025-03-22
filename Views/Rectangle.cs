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

    }


    //public class Rectangle
    //{
    //    private RectangleShape _rectangle;
    //    private int _id;

    //    public RectangleShape RectangleShape => _rectangle;

    //    public Vector2f Position
    //    {
    //        get => _rectangle.Position;
    //        set => _rectangle.Position = value;
    //    }

    //    public Rectangle(Vector2f size, Color color, int id)
    //    {
    //        _rectangle = new RectangleShape(size);
    //        _rectangle.FillColor = color;
    //        _id = id;
    //    }

    //    public void Draw(RenderWindow window)
    //    {
    //        window.Draw(_rectangle);
    //    }
    //}
}
