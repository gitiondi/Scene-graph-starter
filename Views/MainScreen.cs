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
        private float _rectangleX;
        private float _rectangleY;
        private RectangleShape _rectangle;
        private const float Speed = 100f; // Speed in pixels per second

        public MainScreen()
        {
            _window = new RenderWindow(new VideoMode(1280, 720), "SFML Works!");
            _window.Closed += (sender, e) => _window.Close();

            // Load the image
            string rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            rootPath = Path.Combine(rootPath) + Path.DirectorySeparatorChar;
            string resPath = Path.Combine(rootPath, "resources") + Path.DirectorySeparatorChar;
            string imgPath = Path.Combine(resPath, "images") + Path.DirectorySeparatorChar;
            //string imgFile = Path.Combine(imgPath, "dungeon.jpeg");
            string imgFile = Path.Combine(imgPath, "cartesian_grid.png");
            imgFile = imgFile.Replace("\\", "/");

            _texture = new Texture(imgFile);
            _sprite = new Sprite(_texture);

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
            _rectangleX = _view.Viewport.Left * _window.Size.X;
            _rectangleY = _view.Viewport.Top * _window.Size.Y;

            // Draw a rectangle in the upper left corner of the viewport
            _rectangle = new RectangleShape(new Vector2f(50, 50));
            _rectangle.FillColor = Color.Yellow;


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
                UpdateRectangle();
                _window.Draw(_rectangle);

                // Restore the original view
                _window.SetView(currentView);

                // Update the window
                _window.Display();
            }

            return this;
        }

        private void UpdateRectangle()
        {
            if (!_window.HasFocus())
            {
                return;
            }

            float deltaTime = _clock.Restart().AsSeconds();
            var rectangleSpeed = 10000f;

            if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                _rectangleX -= rectangleSpeed * deltaTime;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                _rectangleX += rectangleSpeed * deltaTime;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up) && Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                _rectangleY -= rectangleSpeed * deltaTime;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down) && Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                _rectangleY += rectangleSpeed * deltaTime;
            }

            _rectangle.Position = new Vector2f(_rectangleX, _rectangleY); // Adjust for the rectangle's size
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
