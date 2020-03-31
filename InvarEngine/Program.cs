using System;
using OpenTK;

namespace InvarEngine
{
    class Program
    { 

        static void Main(string[] args)
        {

            GameWindow window = new GameWindow(800, 600);
            Game game = new Game(window);

            window.Run();
            
        }

    }
}

//dotnet run
