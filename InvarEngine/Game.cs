using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace InvarEngine
{
    class Game
    {

        public GameWindow window;
        //Texture2D texture;

        //Vertex[] vertBuffer;
        //int VBO;
        //uint[] indexBuffer;
        //int IBO; //Hold ID for index buffer object

        Vector3 CameraRotation = new Vector3(0f, 0f, 0f);

        GameObject Test;
        GameObject Floor;

        public Game(GameWindow windowInput)
        {

            this.window = windowInput;

            window.Load += window_Load;
            window.RenderFrame += window_RenderFrame;
            window.UpdateFrame += window_UpdateFrame;
            window.Closing += window_Closing;
            window.Resize += window_Resize;

        }

        void window_Resize(object sender, EventArgs e) //called everytime the window is resized, Stays in a 1:1 ratio
        {
            int NewSize = 0;
            if(window.Width > window.Height)
                NewSize = window.Height;
            else
                NewSize = window.Width;

            //Console.WriteLine("Resized");

            //Console.WriteLine(window.Height);

            //GL.Viewport((window.Width - NewSize)/2, (window.Height - NewSize)/2, NewSize, NewSize);      //maps the rendered output to the dimensions of the window, might change to lock 1280x720
        }

        void window_Load(object sender, EventArgs e)
        {

            GL.ClearColor(Color.FromArgb(5, 5, 25));


            Test = new GameObject(new Vector3(0f, 0f, -5f), new Vector3(0f, 0f, 0f), 1f, true);
            Test.Renderer.Bind();

            Floor = new GameObject(new Vector3(0f, -1f, -2f), new Vector3(0f, 0f, 0f), 1f, true);
            Floor.Renderer.Bind();

            /*
            texture = ContentPipe.LoadTexture("Icon.png");

            
            vertBuffer = new Vertex[4]
            {
                //new Vertex(new Vector2(0,0), new Vector2(0,0)) {Color = Color.Red},  if wanting to set color or other vars
                new Vertex(new Vector2(-.5f, -.5f), new Vector2(0,1)),
                new Vertex(new Vector2(-.5f,  .5f), new Vector2(0,0)),
                new Vertex(new Vector2( .5f,  .5f), new Vector2(1,0)),
                new Vertex(new Vector2( .5f, -.5f), new Vector2(1,1))

                //new Vertex(new Vector2(-.5f,   1f), new Vector2(0,0)),
                //new Vertex(new Vector2( .5f,   1f), new Vector2(0,0))
            };

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * vertBuffer.Length), vertBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            indexBuffer = new uint[4]       //8 size for room for two quads
            {
                0, 1, 2, 3     //right side up quad

                //4, 5, 1, 2      //upsidedown quad
            };

            IBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * indexBuffer.Length), indexBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);*/

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

            //Matrix4 proj = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
            Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80), 1f, 0.1f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);
            
            
            Test.Renderer.Draw();
            Floor.Renderer.Draw();
            

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

            //GL.EnableClientState(ArrayCap.VertexArray);    //We have an array of vertices   
            //GL.EnableClientState(ArrayCap.TextureCoordArray);        //We have texture coordinates
            //GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);       //Tell OpenGL to look into memory and find this in the GPU
            //GL.VertexPointer(2, VertexPointerType.Float, Vector2.SizeInBytes * 2, 0); //Our verts are two floats, size in bytes is step size. Start at [0]
            //GL.TexCoordPointer(2, TexCoordPointerType.Float, Vector2.SizeInBytes * 2, Vector2.SizeInBytes); //Start at [1] by offsetting by size in bytes
            
            //GL.DrawArrays(PrimitiveType.Quads, 0, vertBuffer.Length/2);       //Draw it


            //WORKING CODE----------------------------------------------

            /*
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);        

            Matrix4 world = Matrix4.CreateTranslation(0, 0, -5);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref world);

            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.VertexPointer(2, VertexPointerType.Float, Vertex.SizeInBytes, (IntPtr)0);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, (IntPtr)(Vector2.SizeInBytes * 1));
            GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, (IntPtr)(Vector2.SizeInBytes * 2));

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.DrawElements(PrimitiveType.Quads, indexBuffer.Length, DrawElementsType.UnsignedInt, 0);      //6 vertices but 2 quads 

            */
            //----------------------------------------------


            //GL.DrawArrays(PrimitiveType.Quads, 0, vertBuffer.Length);



            /*world = Matrix4.CreateTranslation(200, 100, 0);         //drawing the object multiple times by offsetting world matrix and drawing same object from memory
            GL.LoadMatrix(ref world);
            GL.DrawArrays(PrimitiveType.Quads, 0, vertBuffer.Length);

            world = Matrix4.CreateTranslation(300, 100, 0);
            GL.LoadMatrix(ref world);
            GL.DrawArrays(PrimitiveType.Quads, 0, vertBuffer.Length);*/


            GL.Flush();
            window.SwapBuffers();


        }

        

    }

}