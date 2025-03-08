using SFML.Graphics;
using SFML.Window;

namespace Views
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Create the window
            RenderWindow window = new RenderWindow(new VideoMode(1280, 720), "SFML Works!");

            // Limit the framerate to 60 frames per second
            //window.SetFramerateLimit(60);


            window.Closed += (sender, e) => window.Close();

            // Load the image

            string rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            rootPath = Path.Combine(rootPath) + Path.DirectorySeparatorChar;
            string resPath = Path.Combine(rootPath, "resources") + Path.DirectorySeparatorChar;
            string imgPath = Path.Combine(resPath, "images") + Path.DirectorySeparatorChar;
            string imgFile = Path.Combine(imgPath, "dungeon.jpeg");
            imgFile = imgFile.Replace("\\", "/");

            Texture texture = new Texture(imgFile);
            Sprite sprite = new Sprite(texture);

            // Calculate the window dimensions
            float windowWidth = window.Size.X;
            float windowHeight = window.Size.Y;

            // Calculate the border size (10% of window width for left/right, 10% of window height for top/bottom)
            float horizontalBorder = windowWidth * 0.1f;
            float verticalBorder = windowHeight * 0.1f;

            // Calculate the view dimensions (60% of vertical space)
            float viewWidth = windowWidth - 2 * horizontalBorder;
            float viewHeight = windowHeight * 0.6f;

            // Create the View
            View view = new View(new SFML.Graphics.FloatRect(200, 200, 300, 200));

            view.Center = new SFML.System.Vector2f(200, 200);
            
            view.Viewport = new FloatRect(0.25f, 0.25f, 0.5f, 0.5f);

            // Set the View for the window
            window.SetView(view);

            // Limit the framerate to 60 frames per second
            window.SetFramerateLimit(60);

            // Main loop
            while (window.IsOpen)
            {
                // Process events
                window.DispatchEvents();

                // Clear screen
                window.Clear();

                // Draw the sprite
                window.Draw(sprite);

                // Update the window
                window.Display();
            }
        }
    }
}
