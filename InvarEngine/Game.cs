using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace InvarEngine
{

    class Game
    {

        public GameWindow window;
        Texture2D texture;

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

            texture = ContentPipe.LoadTexture("Icon.png");

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

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            GL.Begin(PrimitiveType.Triangles);

           
            GL.Color3(Color.Red);
            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);

            GL.Color3(Color.Green);
            GL.TexCoord2(1, 0);
            GL.Vertex2(1, 1);

            GL.Color3(Color.Blue);
            GL.TexCoord2(1, 1);
            GL.Vertex2(-1, 1);

            GL.End();            


            GL.Flush();
            window.SwapBuffers();


        }

        

    }

}