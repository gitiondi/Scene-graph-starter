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
            imgFile = imgFile.Replace("\\", "/");

            _texture = new Texture(imgFile);
            _sprite = new Sprite(_texture);

            // Calculate the window dimensions
            float windowWidth = _window.Size.X;
            float windowHeight = _window.Size.Y;

            // Calculate the border size (10% of window width for left/right, 10% of window height for top/bottom)
            float horizontalBorder = windowWidth * 0.1f;
            float verticalBorder = windowHeight * 0.1f;

            // Create the View
            _view = new View(new FloatRect(150, 150, 500, 400));
            _view.Center = new Vector2f(200, 200);
            _view.Viewport = new FloatRect(0.1f, 0.15f, 0.7f, 0.7f);

            // View is an SFML class that defines a 2D camera. It allows you to control which part of the scene is visible in the window.
            // FloatRect(150, 150, 500, 400) defines the rectangle that the view will initially display. This rectangle starts at the point (150, 150) and has a width of 500 and a height of 400.

            // Center is a property of the View class that sets the center point of the view.
            // Vector2f(200, 200) sets the center of the view to the point (200, 200). This means that the view will be centered around this point in the scene.

            // Viewport is a property of the View class that defines the portion of the window where the view will be displayed.
            // FloatRect(0.15f, 0.15f, 0.7f, 0.7f) specifies the viewport rectangle in normalized coordinates (ranging from 0 to 1). This means:
            // The viewport starts at 15% of the window's width and height from the top-left corner.
            // The viewport covers 70% of the window's width and height.



            // Set the View for the window
            _window.SetView(_view);

            // Limit the framerate to 60 frames per second
            _window.SetFramerateLimit(60);
        }

        public MainScreen execute()
        {
            _window.KeyPressed += Window_OnKeyPressed;

            // Main loop
            while (_window.IsOpen)
            {
                // Process events
                _window.DispatchEvents();

                // Clear screen
                _window.Clear();

                // Draw the sprite
                _window.Draw(_sprite);

                // Update the window
                _window.Display();
            }

            return this;
        }

        private void Window_OnKeyPressed(object? sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Left:
                    _sprite.Position = new Vector2f(_sprite.Position.X + 10, _sprite.Position.Y);
                    break;
                case Keyboard.Key.Right:
                    _sprite.Position = new Vector2f(_sprite.Position.X - 10, _sprite.Position.Y);
                    break;
                case Keyboard.Key.Up:
                    _sprite.Position = new Vector2f(_sprite.Position.X, _sprite.Position.Y + 10);
                    break;
                case Keyboard.Key.Down:
                    _sprite.Position = new Vector2f(_sprite.Position.X, _sprite.Position.Y - 10);
                    break;
            }
        }
    }
}
