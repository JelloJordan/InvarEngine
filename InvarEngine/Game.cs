using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace InvarEngine
{

    class Game
    {

        public GameWindow window;

        public Game(GameWindow windowInput)
        {

            this.window = windowInput;

            window.Load += window_Load;
            window.RenderFrame += window_RenderFrame;
            window.UpdateFrame += window_UpdateFrame;
            window.Closing += window_Closing;

        }

        void window_Load(object sender, EventArgs e)
        {

            GL.ClearColor(Color.FromArgb(5, 5, 25));

        }

        void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {



        }

        void window_UpdateFrame(object sender, FrameEventArgs e)
        {

          

        }

        void window_RenderFrame(object sender, FrameEventArgs e)
        {

            
            GL.Clear(ClearBufferMask.ColorBufferBit);


            GL.Begin(PrimitiveType.Triangles);
            


            GL.Flush();
            window.SwapBuffers();


        }

        

    }

}