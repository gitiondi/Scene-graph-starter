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
            _view = new View(new FloatRect(200, 200, 300, 200));
            _view.Center = new SFML.System.Vector2f(200, 200);
            _view.Viewport = new FloatRect(0.25f, 0.25f, 0.5f, 0.5f);

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
