using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Scene_Graph_Starter
{
    public class MainScreen
    {
        private readonly RenderWindow _window;
        private readonly Sprite _sprite;
        private readonly View _view;
        private readonly Clock _clock;
        private const float Speed = 100f; // Speed in pixels per second
        private Vector2f _mouseOffset;
        private Node? _selectedNode;
        private readonly Rectangle _sceneRoot;

        public MainScreen()
        {
            _window = new RenderWindow(new VideoMode(1280, 720), "SFML Works!");
            _window.Closed += (_, _) => _window.Close();
            _sprite = new Sprite(new Texture(LoadBackgroundImage()));
            _selectedNode = null;

            // Set the origin of the sprite to its center
            var spriteCenter = new Vector2f(_sprite.TextureRect.Width / 2f, _sprite.TextureRect.Height / 2f);
            _sprite.Origin = spriteCenter;
            _sprite.Position = new Vector2f(1000, 1000);

            const float viewPortWidth = 0.9f;
            const float viewPortHeight = 0.6f;
            var viewPortAspect = _window.Size.X * viewPortWidth / (_window.Size.Y * viewPortHeight);
            var viewPortRect = new FloatRect(0.05f, 0.1f, viewPortWidth, viewPortHeight);

            var viewWidthAbs = _window.Size.X * viewPortWidth;
            var viewRectHeight = viewWidthAbs / viewPortAspect;
            var viewRect = new FloatRect(1000, 1000, viewWidthAbs, viewRectHeight);

            // Create the View
            _view = new View(viewRect);
            _view.Center = new Vector2f(1000, 1000);
            _view.Viewport = viewPortRect;

            // Calculate the upper left corner of the viewport in window coordinates
            var rectangleX = _view.Viewport.Left * _window.Size.X;
            var rectangleY = _view.Viewport.Top * _window.Size.Y;
            var upperLeftViewportCorner = new Vector2f(rectangleX, rectangleY);

            _sceneRoot = new Rectangle(new Vector2f(50, 50), Color.Yellow, 1);
            _sceneRoot.Position = upperLeftViewportCorner;

            var rectangleRed = new Rectangle(new Vector2f(50, 50), Color.Red, 2);
            rectangleRed.Position = new Vector2f(0, 50);
            _sceneRoot.AddChild(rectangleRed);

            var rectangleBlue = new Rectangle(new Vector2f(50, 50), Color.Blue, 3);
            rectangleBlue.Position = new Vector2f(0, 100);
            _sceneRoot.AddChild(rectangleBlue);

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

                _window.Draw(_sceneRoot);

                // Restore the original view
                _window.SetView(currentView);

                _window.Display();
            }
            return this;
        }

        private void Window_OnMouseMoved(object? sender, MouseMoveEventArgs e)
        {
            var mousePosition = new Vector2f(e.X, e.Y);

            if (_selectedNode != null)
            {
                if(_selectedNode.IsRoot)
                {
                    _selectedNode.Position = mousePosition - _mouseOffset;
                }
                else
                {
                    var relativePos = mousePosition - _sceneRoot.Position;
                    _selectedNode.Position = relativePos - _mouseOffset;
                }
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
            var windowPosition = new Vector2f(e.X, e.Y);

            if (_sceneRoot.IsPointInNode(windowPosition))
            {
                _selectedNode = _sceneRoot;
                _mouseOffset = windowPosition - _sceneRoot.Position;
            }
            else
            {
                foreach (var nodeChild in _sceneRoot.Children)
                {
                    var childPosition = _sceneRoot.Position + nodeChild.Position;
                    if (nodeChild.IsPointInNode(windowPosition))
                    {
                        _selectedNode = nodeChild;
                        _mouseOffset = windowPosition - childPosition;
                        break;
                    }
                }
            }
        }

        private void OnRightMouseButtonPressed(MouseButtonEventArgs e)
        {
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
