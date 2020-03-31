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

        Vector2[] vertBuffer;
        int VBO;

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

            
            vertBuffer = new Vector2[8]
            {

                new Vector2(-.5f, .5f), new Vector2(0, 0), 
                new Vector2(-.5f, -.5f), new Vector2(0, 1), 
                new Vector2(.5f, -.5f), new Vector2(1, 1), 
                new Vector2(.5f, .5f), new Vector2(1, 0), 

            };

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(Vector2.SizeInBytes * vertBuffer.Length), vertBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

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

            /*
            GL.Begin(PrimitiveType.Quads);
                GL.Color3(Color.White);

                GL.TexCoord2(0, 1);
                GL.Vertex2(0, 0);

                GL.TexCoord2(1, 1);
                GL.Vertex2(1, 0);

                GL.TexCoord2(1, 0);
                GL.Vertex2(1, 1);

                GL.TexCoord2(0, 0);
                GL.Vertex2(0, 1);
            GL.End();            
            */

            //GL.Disable(EnableCap.Texture2D);            //stop drawing textures for this next object

            GL.EnableClientState(ArrayCap.VertexArray);    //We have an array of vertices   
            GL.EnableClientState(ArrayCap.TextureCoordArray);        //We have texture coordinates
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);       //Tell OpenGL to look into memory and find this in the GPU
            GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes * 2, 0); //Our verts are two floats, size in bytes is step size. Start at [0]
            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vector2.SizeInBytes * 2, Vector2.SizeInBytes); //Start at [1] by offsetting by size in bytes
            
            GL.DrawArrays(PrimitiveType.Quads, 0, vertBuffer.Length/2);       //Draw it

            GL.Flush();
            window.SwapBuffers();


        }

        

    }

}