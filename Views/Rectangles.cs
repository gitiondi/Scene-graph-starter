using System.Collections;
using SFML.Graphics;
using SFML.System;

namespace Views
{
    //public class Rectangles : IEnumerable<Rectangle>
    //{
    //    private GroupNode _groupNode;

    //    public Rectangles()
    //    {
    //        _groupNode = new GroupNode(new Vector2f());
    //    }

    //    public Rectangle AddRectangle(Vector2f size, Color color)
    //    {
    //        var rectangle = new Rectangle(size, color, _groupNode.Children.Count);
    //        _groupNode.AddChild(rectangle);
    //        return rectangle;
    //    }

    //    public void Move(Vector2f offset)
    //    {
    //        _groupNode.Move(offset);
    //    }

    //    public void Draw(RenderWindow window)
    //    {
    //        _groupNode.Draw(window);
    //    }

    //    public Rectangle this[int index]
    //    {
    //        get => (Rectangle)_groupNode.Children[index];
    //    }

    //    public IEnumerator<Rectangle> GetEnumerator()
    //    {
    //        return _groupNode.Children.OfType<Rectangle>().GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}



    //public class Rectangles : IEnumerable<Rectangle>
    //{

    //    private List<Rectangle> _rectangles;

    //    public Rectangles()
    //    {
    //        _rectangles = new List<Rectangle>();
    //    }

    //    public Rectangle AddRectangle(Vector2f size, Color color)
    //    {
    //        var rectangle = new Rectangle(size, color, _rectangles.Count);
    //        _rectangles.Add(rectangle);
    //        return rectangle;
    //    }

    //    //Indexer
    //    public Rectangle this[int index]
    //    {
    //        get => _rectangles[index];
    //    }

    //    public IEnumerator<Rectangle> GetEnumerator()
    //    {
    //        foreach (var rectangle in _rectangles)
    //        {
    //            yield return rectangle;
    //        }
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}
}
