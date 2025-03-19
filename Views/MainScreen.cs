using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Views
{
    public class MainScreen
    {
        private RenderWindow _window;
        private readonly Texture _texture;
        private readonly Sprite _sprite;
        private readonly View _view;
        private Clock _clock;

        //private readonly Rectangles _rectangles;
        private Vector2f _upperLeftViewportCorner;

        private const float Speed = 100f; // Speed in pixels per second

        private Vector2f _mouseOffset;

        private GroupNode _rootNode;
        //private GroupNode? _selectedRootNode;

        private Node? _selectedNode;

        public MainScreen()
        {
            _window = new RenderWindow(new VideoMode(1280, 720), "SFML Works!");
            _window.Closed += (sender, e) => _window.Close();

            // Load the image
            string rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            rootPath = Path.Combine(rootPath) + Path.DirectorySeparatorChar;
            string resPath = Path.Combine(rootPath, "resources") + Path.DirectorySeparatorChar;
            string imgPath = Path.Combine(resPath, "images") + Path.DirectorySeparatorChar;
            string imgFile = Path.Combine(imgPath, "dungeon.jpeg");
            //string imgFile = Path.Combine(imgPath, "cartesian_grid.png");
            imgFile = imgFile.Replace("\\", "/");

            _texture = new Texture(imgFile);
            _sprite = new Sprite(_texture);
            _selectedNode = null;

            // Set the origin of the sprite to its center
            var spriteCenter = new Vector2f(_sprite.TextureRect.Width / 2f, _sprite.TextureRect.Height / 2f);
            _sprite.Origin = spriteCenter;
            _sprite.Position = new Vector2f(1000, 1000);

            // Calculate the window dimensions
            float windowWidth = _window.Size.X;
            float windowHeight = _window.Size.Y;

            // Calculate the border size (10% of window width for left/right, 10% of window height for top/bottom)
            float horizontalBorder = windowWidth * 0.1f;
            float verticalBorder = windowHeight * 0.1f;


            var viewPortWidth = 0.9f;
            var viewPortHeight = 0.6f;
            var viewPortAspect = (windowWidth * viewPortWidth) / (windowHeight * viewPortHeight);

            var viewPortRect = new FloatRect(0.05f, 0.1f, viewPortWidth, viewPortHeight);

            var viewWidthAbs = windowWidth * viewPortWidth;

            var viewRectWidth = viewWidthAbs;
            var viewRectHeight = viewRectWidth / viewPortAspect;
            var viewRectAspect = viewRectWidth / viewRectHeight;

            var viewRect = new FloatRect(1000, 1000, viewRectWidth, viewRectHeight);

            // Create the View
            _view = new View(viewRect);
            _view.Center = new Vector2f(1000, 1000);
            _view.Viewport = viewPortRect;

            // View is an SFML class that defines a 2D camera. It allows you to control which part of the scene is visible in the window.
            // FloatRect(150, 150, 500, 400) defines the rectangle that the view will initially display. This rectangle starts at the point (150, 150) and has a width of 500 and a height of 400.

            // Center is a property of the View class that sets the center point of the view.
            // Vector2f(200, 200) sets the center of the view to the point (200, 200). This means that the view will be centered around this point in the scene.

            // Viewport is a property of the View class that defines the portion of the window where the view will be displayed.
            // FloatRect(0.15f, 0.15f, 0.7f, 0.7f) specifies the viewport rectangle in normalized coordinates (ranging from 0 to 1). This means:
            // The viewport starts at 15% of the window's width and height from the top-left corner.
            // The viewport covers 70% of the window's width and height.

            // Calculate the upper left corner of the viewport in window coordinates
            var rectangleX = _view.Viewport.Left * _window.Size.X;
            var rectangleY = _view.Viewport.Top * _window.Size.Y;
            _upperLeftViewportCorner = new Vector2f(rectangleX, rectangleY);

            _rootNode = new GroupNode(new Vector2f());
            // Draw a rectangle in the upper left corner of the viewport
            //_rectangles = new Rectangles();

            //var rect = _rectangles.AddRectangle(new Vector2f(50, 50), Color.Yellow);
            var rectangleYellow = new Rectangle(new Vector2f(50, 50), Color.Yellow, 1);
            _rootNode.AddChild(rectangleYellow);
            rectangleYellow.Position = _upperLeftViewportCorner;

            //rect = _rectangles.AddRectangle(new Vector2f(50, 50), Color.Red);
            var rectangleRed = new Rectangle(new Vector2f(50, 50), Color.Red, 2);
            _rootNode.AddChild(rectangleRed);
            rectangleRed.Position = new Vector2f(_upperLeftViewportCorner.X, _upperLeftViewportCorner.Y + 50);


            // Set the View for the window
            _window.SetView(_view);

            // Limit the framerate to 60 frames per second
            _window.SetFramerateLimit(60);

            // Initialize the clock
            _clock = new Clock();
        }

        public MainScreen execute()
        {
            _window.KeyPressed += Window_OnKeyPressed;
            _window.MouseWheelScrolled += Window_OnMouseWheelScrolled;
            _window.MouseButtonPressed += Window_OnMouseButtonPressed;
            _window.MouseMoved += Window_OnMouseMoved;

            // Main loop
            while (_window.IsOpen)
            {
                // Process events
                _window.DispatchEvents();

                // Clear screen
                _window.Clear();

                // Update the view
                UpdateViewCenter();

                // Draw the sprite
                _window.Draw(_sprite);

                // Save the current view
                var currentView = _window.GetView();

                // Set the view to the default view
                _window.SetView(_window.DefaultView);

                // Position the rectangle in viewport coordinates
                //UpdateRectangle();
                //foreach (var rectangle in _rectangles)
                //{
                //    rectangle.Draw(_window);
                //}

                _rootNode.Draw(_window); //todo debug

                // Restore the original view
                _window.SetView(currentView);

                // Update the window
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


            switch (_selectedNode)
            {
                case Rectangle rectangle:
                    rectangle.Position = worldPosition - _mouseOffset;
                    break;
                case GroupNode groupNode:
                    groupNode.Position = worldPosition - _mouseOffset;
                    break;
                case null:
                    break;
            }

            //if (_selectedRectangle != null)
            //{
            //    // Update the rectangle's position based on the mouse position and the offset
            //    _selectedRectangle.Position = worldPosition - _mouseOffset;
            //}
            //else if (_selectedRootNode != null)
            //{
            //    _selectedRootNode.Position = worldPosition - _mouseOffset;
            //    //_selectedRectangle.Position = worldPosition - _mouseOffset - _selectedRootNode.Position;
            //}
        }


        //private void Window_OnMouseButtonPressed(object? sender, MouseButtonEventArgs e)
        //{
        //    // Get the mouse position in window coordinates
        //    var mousePosition = new Vector2f(e.X, e.Y);

        //    // Convert the mouse position to the default view coordinates
        //    var worldPosition = _window.MapPixelToCoords(new Vector2i((int)mousePosition.X, (int)mousePosition.Y), _window.DefaultView);

        //    // Check if the mouse position intersects with any of the rectangles
        //    foreach (var rectangle in _rectangles)
        //    {
        //        var rectPosition = rectangle.Position;
        //        var rectSize = rectangle.RectangleShape.Size; 

        //        if (worldPosition.X >= rectPosition.X && worldPosition.X <= rectPosition.X + rectSize.X &&
        //            worldPosition.Y >= rectPosition.Y && worldPosition.Y <= rectPosition.Y + rectSize.Y)
        //        {
        //            // The mouse click is on this rectangle
        //            _selectedRectangle = rectangle;
        //            break;
        //        }
        //        else
        //        {
        //            _selectedRectangle = null;
        //        }
        //    }
        //}

        private void Window_OnMouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {

            switch (e.Button)
            {
                case Mouse.Button.Left:
                    OnLeftMouseButtonPressed(e);

                    //if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                    //{
                    //    var mousePosition = new Vector2f(e.X, e.Y);
                    //    var worldPosition = _window.MapPixelToCoords(new Vector2i((int)mousePosition.X, (int)mousePosition.Y), _window.DefaultView);
                    //    _rootNode.Move(worldPosition - _mouseOffset);
                    //}
                    //else
                    //{
                    //    OnLeftMouseButtonPressed(e);
                    //}

                    break;
                case Mouse.Button.Right:
                    OnRightMouseButtonPressed(e);
                    break;
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

        private void OnLeftMouseButtonPressed(MouseButtonEventArgs e)
        {
            // Get the mouse position in window coordinates
            var mousePosition = new Vector2f(e.X, e.Y);

            // Convert the mouse position to the default view coordinates
            var worldPosition = _window.MapPixelToCoords(new Vector2i((int)mousePosition.X, (int)mousePosition.Y), _window.DefaultView);

                // Check if the mouse position intersects with any of the rectangles
                foreach (var rootNodeChild in _rootNode.Children)
                {
                    var rectangle = rootNodeChild as Rectangle;
                    //var rectShape = rect?.RectangleShape;
                    var rectPosition = _rootNode.Position + rectangle.Position;
                    var rectSize = rectangle.RectangleShape.Size;

                //debug todo
                if (rootNodeChild.Parent[0] == rootNodeChild)
                {
                    Debug.WriteLine($"## worldPosition: {worldPosition}");
                    Debug.WriteLine($"## rectPosition: {rectPosition}");
                    Debug.WriteLine($"## ");
                }

                if (worldPosition.X >= rectPosition.X && worldPosition.X <= rectPosition.X + rectSize.X &&
                        worldPosition.Y >= rectPosition.Y && worldPosition.Y <= rectPosition.Y + rectSize.Y)
                    {
                        if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                        {
                            _selectedNode = _rootNode;

                            for (int i = 0; i < _rootNode.Children.Count; i++)
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
                            // The mouse click is on this rectangle
                            _selectedNode = rectangle;
                        }

                        // Calculate the offset
                        _mouseOffset = worldPosition - rectPosition;
                        break;
                    }
                    else
                    {
                        _selectedNode = null;
                    }

                }


                //foreach (var rectangle in _rectangles)
            //{
            //    var rectPosition = rectangle.Position;
            //    var rectSize = rectangle.RectangleShape.Size;

            //    if (worldPosition.X >= rectPosition.X && worldPosition.X <= rectPosition.X + rectSize.X &&
            //        worldPosition.Y >= rectPosition.Y && worldPosition.Y <= rectPosition.Y + rectSize.Y)
            //    {
            //        // The mouse click is on this rectangle
            //        _selectedRectangle = rectangle;

            //        // Calculate the offset
            //        _mouseOffset = worldPosition - rectPosition;
            //        break;
            //    }
            //    else
            //    {
            //        _selectedRectangle = null;
            //    }
            //}
        }


        private void UpdateRectangle()
        {
            if (!_window.HasFocus())
            {
                return;
            }

            float deltaTime = _clock.Restart().AsSeconds();
            var rectangleSpeed = 10000f;

            //var rectangleX = _rectangles[0].Position.X;
            //var rectangleY = _rectangles[0].Position.Y;

            var rectangleX = _rootNode.Position.X;
            var rectangleY = _rootNode.Position.Y;


            if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                rectangleX -= rectangleSpeed * deltaTime;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                rectangleX += rectangleSpeed * deltaTime;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                rectangleY -= rectangleSpeed * deltaTime;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                rectangleY += rectangleSpeed * deltaTime;
            }

            _rootNode.Position = new Vector2f(rectangleX, rectangleY);
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

            float deltaTime = _clock.Restart().AsSeconds();

            if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && !Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                //_view.Center += new Vector2f(-Speed * deltaTime, 0);
                _view.Move(new Vector2f(-Speed * deltaTime, 0));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && !Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                //_view.Center += new Vector2f(Speed * deltaTime, 0);
                _view.Move(new Vector2f(Speed * deltaTime, 0));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && !Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                //_view.Center += new Vector2f(0, -Speed * deltaTime);
                _view.Move(new Vector2f(0, -Speed * deltaTime));
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && !Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                //_view.Center += new Vector2f(0, Speed * deltaTime);
                _view.Move(new Vector2f(0, Speed * deltaTime));
            }

            // Set the updated view back to the window
            _window.SetView(_view);
        }

        private void Window_OnKeyPressed(object? sender, KeyEventArgs e)
        {
            // for future use
        }
    }
}
