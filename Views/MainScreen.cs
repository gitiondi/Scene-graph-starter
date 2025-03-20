using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Views
{
    public class MainScreen
    {
        private readonly RenderWindow _window;
        private readonly Sprite _sprite;
        private readonly View _view;
        private readonly Clock _clock;
        private const float Speed = 100f; // Speed in pixels per second
        private Vector2f _mouseOffset;
        private readonly GroupNode _rootNode;
        private Node? _selectedNode;

        public MainScreen()
        {
            _window = new RenderWindow(new VideoMode(1280, 720), "SFML Works!");
            _window.Closed += (sender, e) => _window.Close();

            var imgFile = LoadBackgroundImage();
            var texture = new Texture(imgFile);
            _sprite = new Sprite(texture);

            _selectedNode = null;

            // Set the origin of the sprite to its center
            var spriteCenter = new Vector2f(_sprite.TextureRect.Width / 2f, _sprite.TextureRect.Height / 2f);
            _sprite.Origin = spriteCenter;
            _sprite.Position = new Vector2f(1000, 1000);

            // Calculate the window dimensions
            float windowWidth = _window.Size.X;
            float windowHeight = _window.Size.Y;

            var viewPortWidth = 0.9f;
            var viewPortHeight = 0.6f;
            var viewPortAspect = windowWidth * viewPortWidth / (windowHeight * viewPortHeight);
            var viewPortRect = new FloatRect(0.05f, 0.1f, viewPortWidth, viewPortHeight);

            var viewWidthAbs = windowWidth * viewPortWidth;
            var viewRectWidth = viewWidthAbs;
            var viewRectHeight = viewRectWidth / viewPortAspect;
            var viewRect = new FloatRect(1000, 1000, viewRectWidth, viewRectHeight);

            // Create the View
            _view = new View(viewRect);
            _view.Center = new Vector2f(1000, 1000);
            _view.Viewport = viewPortRect;

            // Calculate the upper left corner of the viewport in window coordinates
            var rectangleX = _view.Viewport.Left * _window.Size.X;
            var rectangleY = _view.Viewport.Top * _window.Size.Y;
            var upperLeftViewportCorner = new Vector2f(rectangleX, rectangleY);

            _rootNode = new GroupNode(new Vector2f());

            var rectangleYellow = new Rectangle(new Vector2f(50, 50), Color.Yellow, 1);
            _rootNode.AddChild(rectangleYellow);
            rectangleYellow.Position = upperLeftViewportCorner;

            var rectangleRed = new Rectangle(new Vector2f(50, 50), Color.Red, 2);
            _rootNode.AddChild(rectangleRed);
            rectangleRed.Position = new Vector2f(upperLeftViewportCorner.X, upperLeftViewportCorner.Y + 50);

            _window.SetView(_view);
            _window.SetFramerateLimit(60);

            _clock = new Clock();
        }

        private static string LoadBackgroundImage()
        {
            string rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            rootPath = Path.Combine(rootPath) + Path.DirectorySeparatorChar;
            string resPath = Path.Combine(rootPath, "resources") + Path.DirectorySeparatorChar;
            string imgPath = Path.Combine(resPath, "images") + Path.DirectorySeparatorChar;
            string imgFile = Path.Combine(imgPath, "dungeon.jpeg");
            //string imgFile = Path.Combine(imgPath, "cartesian_grid.png");
            imgFile = imgFile.Replace("\\", "/");
            return imgFile;
        }

        public MainScreen Execute()
        {
            _window.MouseWheelScrolled += Window_OnMouseWheelScrolled;
            _window.MouseButtonPressed += Window_OnMouseButtonPressed;
            _window.MouseMoved += Window_OnMouseMoved;

            while (_window.IsOpen)
            {
                _window.DispatchEvents();
                _window.Clear();

                UpdateViewCenter();

                _window.Draw(_sprite);

                // Save the current view
                var currentView = _window.GetView();

                // Set the view to the default view
                _window.SetView(_window.DefaultView);

                _rootNode.Draw(_window);

                // Restore the original view
                _window.SetView(currentView);

                _window.Display();
            }
            return this;
        }

        private void Window_OnMouseMoved(object? sender, MouseMoveEventArgs e)
        {
            // Get the mouse position in window coordinates
            var mousePosition = new Vector2f(e.X, e.Y);

            // Convert the mouse position to the default view coordinates
            var worldPosition = _window.MapPixelToCoords(new Vector2i((int)mousePosition.X, (int)mousePosition.Y), _window.DefaultView);

            if (_selectedNode != null)
            {
                _selectedNode.Position = worldPosition - _mouseOffset;
            }
        }

        private void Window_OnMouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {

            switch (e.Button)
            {
                case Mouse.Button.Left:
                    OnLeftMouseButtonPressed(e);
                    break;
                case Mouse.Button.Right:
                    OnRightMouseButtonPressed(e);
                    break;
            }
        }

        private void OnLeftMouseButtonPressed(MouseButtonEventArgs e)
        {
            var worldPosition = _window.MapPixelToCoords(new Vector2i(e.X, e.Y), _window.DefaultView);

            // Check if the mouse position intersects with any of the rectangles
            foreach (var rootNodeChild in _rootNode.Children)
            {
                var rectangle = rootNodeChild as Rectangle;
                var rectPosition = _rootNode.Position + rectangle.Position;
                var rectSize = rectangle.RectangleShape.Size;

                if (worldPosition.X >= rectPosition.X && worldPosition.X <= rectPosition.X + rectSize.X &&
                    worldPosition.Y >= rectPosition.Y && worldPosition.Y <= rectPosition.Y + rectSize.Y)
                {
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                    {
                        _selectedNode = _rootNode;

                        for (var i = 0; i < _rootNode.Children.Count; i++)
                        {
                            var child = _rootNode.Children[i];
                            if (i == 0)
                            {
                                // Store the position of the GroupNode identical to the first child
                                _selectedNode.Position = child.Position;
                                var offset = new Vector2f();
                                child.Position = offset;
                            }
                            else
                            {
                                // Calculate the offset of the child from the GroupNode's position
                                var offset = child.Position - _selectedNode.Position;
                                child.Position = offset; // Store the offset in the child's position
                            }
                        }
                    }
                    else
                    {
                        _selectedNode = rectangle;
                    }
                    _mouseOffset = worldPosition - rectPosition;
                    break;
                }
                _selectedNode = null;
            }
        }

        private void OnRightMouseButtonPressed(MouseButtonEventArgs e)
        {
            switch (_selectedNode)
            {
                case Rectangle rectangle:
                    break;
                case GroupNode groupNode:

                    foreach (var child in groupNode.Children)
                    {
                        child.Position = groupNode.Position + child.Position;
                    }
                    groupNode.Position = new Vector2f();

                    break;
                case null:
                    break;
            }
            _selectedNode = null;
        }

        private void Window_OnMouseWheelScrolled(object? sender, MouseWheelScrollEventArgs e)
        {
            // Calculate the center of the sprite
            var spriteCenter = new Vector2f(_sprite.TextureRect.Width / 2f, _sprite.TextureRect.Height / 2f);

            // Set the origin of the sprite to its center
            _sprite.Origin = spriteCenter;

           //Scale the sprite
           var scale = _sprite.Scale;
           var f = 1 + 0.05f * e.Delta;
           _sprite.Scale = new Vector2f(f * scale.X, f * scale.Y);
        }

        private void UpdateViewCenter()
        {
            if (!_window.HasFocus())
            {
                return;
            }

            var deltaTime = _clock.Restart().AsSeconds();

            if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && !Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                _view.Move(new Vector2f(-Speed * deltaTime, 0));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && !Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                _view.Move(new Vector2f(Speed * deltaTime, 0));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && !Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                _view.Move(new Vector2f(0, -Speed * deltaTime));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && !Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                _view.Move(new Vector2f(0, Speed * deltaTime));
            }

            _window.SetView(_view);
        }
    }
}
