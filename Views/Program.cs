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
            window.SetFramerateLimit(60);


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
