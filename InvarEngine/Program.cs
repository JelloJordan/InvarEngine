using System;
using OpenTK;
using OpenTK.Graphics;

namespace InvarEngine
{
    class Program
    { 

        static void Main(string[] args)
        {

            GameWindow window = new GameWindow(1280, 720, GraphicsMode.Default, "Invar Engine Tech Demo");
            Game game = new Game(window);

            window.Run();
            
        }

    }
}

//dotnet run

/*
-------TODO------

Swap to using TRI's instead of quads, test with cube mesh
OBJ Importer needs to work with Vertices, Indices, Texture Coordinates, and Normals

*/
