using System;
using OpenTK;
using OpenTK.Graphics;

namespace InvarEngine
{
    class Program
    { 

        static void Main(string[] args)
        {

            GameWindow window = new GameWindow(800, 800, GraphicsMode.Default, "Invar Engine Tech Demo");
            Game game = new Game(window);

            window.Run();
            
        }

    }
}

//dotnet run
