using System.Collections;
using SFML.Graphics;
using SFML.System;

namespace Views
{
    public class Rectangles : IEnumerable<Rectangle>
    {

        private List<Rectangle> _rectangles;

        public Rectangles()
        {
            _rectangles = new List<Rectangle>();
        }

        public Rectangle AddRectangle(Vector2f size, Color color)
        {
            var rectangle = new Rectangle(size, color, _rectangles.Count);
            _rectangles.Add(rectangle);
            return rectangle;
        }

        //Indexer
        public Rectangle this[int index]
        {
            get => _rectangles[index];
        }

        public IEnumerator<Rectangle> GetEnumerator()
        {
            foreach (var rectangle in _rectangles)
            {
                yield return rectangle;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
