using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;

namespace InvarEngine
{
    struct Camera
    {

        Vector3 position;
        Vector3 rotation;
        float fov;

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        public float FOV
        {
            get
            {
                return fov;
            }
            set
            {
                fov = value;
            }
        }

    }

    class Game
    {

        public GameWindow window;
        //Texture2D texture;

        //Vertex[] vertBuffer;
        //int VBO;
        //uint[] indexBuffer;
        //int IBO; //Hold ID for index buffer object

        Camera Camera;

        //Vector3 CameraRotation = new Vector3(0f, 0f, 0f);
        //Vector3 CameraPosition = new Vector3(0f, 0f, 0f);

        GameObject Test;
        GameObject Floor;

        float MouseSensitivity = 0.2f;

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

            GL.Viewport(0, 0, window.Width, window.Height);
            /*
            int NewSize = 0;
            if(window.Width > window.Height)
                NewSize = window.Height;
            else
                NewSize = window.Width;

            //Console.WriteLine("Resized");

            //Console.WriteLine(window.Height);

            GL.Viewport((window.Width - NewSize)/2, (window.Height - NewSize)/2, NewSize, NewSize);      //maps the rendered output to the dimensions of the window, might change to lock 1280x720
            */
        }

        void window_Load(object sender, EventArgs e)
        {

            GL.ClearColor(Color.FromArgb(5, 5, 25));


            Test = new GameObject(new Vector3(0f, 0f, -5f), new Vector3(0f, 0f, 0f), 1f, true);
            Test.Renderer.Bind("Icon.png");

            Floor = new GameObject(new Vector3(0f, -1f, 0f), new Vector3(90f, 0f, 0f), 10f, true);
            Floor.Renderer.Bind("Grass.jpg");

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

            KeyboardState Keyboardinput = Keyboard.GetState();  //gets current keyboard input
            MouseState Mouseinput = Mouse.GetCursorState();
            window.CursorVisible = false;

            float XDelta = (window.Location.X + window.Width/2) - Mouseinput.X;
            float YDelta = (window.Location.Y + window.Height/2) - Mouseinput.Y;

            Mouse.SetPosition(window.Location.X + window.Width/2f, window.Location.Y + window.Height/2f);

            Camera.Rotation += new Vector3(-YDelta, -XDelta, 0f) * MouseSensitivity;
            if(Camera.Rotation.X < -70f)
                Camera.Rotation = new Vector3(-70f, Camera.Rotation.Y, 0f);
            
            if(Camera.Rotation.X > 70f)
                Camera.Rotation = new Vector3(70f, Camera.Rotation.Y, 0f);
            
            

            /*
            if(Keyboardinput.IsKeyDown(Key.D))
            {   
                if(Camera.Rotation.Y >= 359)
                    Camera.Rotation = new Vector3(Camera.Rotation.X, 0, Camera.Rotation.Z);
                else
                    Camera.Rotation += new Vector3(0f, 1f, 0f);
            }

            if(Keyboardinput.IsKeyDown(Key.A))
            {
                if(Camera.Rotation.Y <= 0)
                    Camera.Rotation = new Vector3(Camera.Rotation.X, 359, Camera.Rotation.Z);
                else
                    Camera.Rotation += new Vector3(0f, -1f, 0f);
            }*/

            if(Keyboardinput.IsKeyDown(Key.W))
            {
                Camera.Position += VectorTools.GetForward(Camera.Rotation) * 0.05f;
            }

            if(Keyboardinput.IsKeyDown(Key.A))
            {
                Camera.Position += VectorTools.GetForward(Camera.Rotation + new Vector3(0f, -90f, 0f)) * 0.05f;
            }

            if(Keyboardinput.IsKeyDown(Key.D))
            {
                Camera.Position += VectorTools.GetForward(Camera.Rotation + new Vector3(0f, 90f, 0f)) * 0.05f;
            }

            if(Keyboardinput.IsKeyDown(Key.S))
            {
                Camera.Position += VectorTools.GetForward(Camera.Rotation) * -0.05f;
            }

            if(Keyboardinput.IsKeyDown(Key.Escape))
            {
                window.Exit();
            }

            if(Keyboardinput.IsKeyDown(Key.F11))
            {
                window.Location = new Point(0, 0);
                //window.Size = new Size(DisplayDevice.Default.Width, DisplayDevice.Default.Height);
                window.Size = new Size(1920, 1080);
                GL.Viewport(0, 0, window.Width, window.Height);
            }

        }

        void window_RenderFrame(object sender, FrameEventArgs e)
        {

       
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //Matrix4 proj = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
            
            
            Floor.Renderer.Draw(Camera);
            Test.Renderer.Draw(Camera);

            //Console.WriteLine(window.RenderFrequency);
            
            

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