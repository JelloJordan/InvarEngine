using System;
using System.Threading;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;

namespace InvarEngine
{
    class Game  //Detach gamelogic from gamewindow script
    {
        public GameWindow window;
        public GameLoop gameLoop;

        
        public static float CurrentFPS = 0f;
        public static int FramesThisSecond = 0;
        Task FrameTracker = new Task(() =>
                {
                    while (true)
                    {   
                        FPSMATH();
                        Thread.Sleep(1000);
                    }
                });

        static void FPSMATH()
        {
            CurrentFPS = FramesThisSecond;
            Console.WriteLine("FPS : " + CurrentFPS);
            FramesThisSecond = 0;
        }

        public Game(GameWindow windowInput)
        {

            this.window = windowInput;

            window.Load += window_Load;
            window.RenderFrame += window_RenderFrame;
            window.UpdateFrame += window_UpdateFrame;
            window.Closing += window_Closing;
            window.Resize += window_Resize;

            //FrameTracker.Start();

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
            //GL.ClearColor(Color.FromArgb(5, 5, 25));
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            

            gameLoop = new GameLoop(window);
            gameLoop.Start();

        }

        void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        void window_UpdateFrame(object sender, FrameEventArgs e)
        {
            
            gameLoop.Update();

            window.CursorVisible = !window.Focused;

            KeyboardState Keyboardinput = Keyboard.GetState();  //gets current keyboard input

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
            FramesThisSecond++;
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit); 

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);


            gameLoop.Draw();


            GL.Flush();
            window.SwapBuffers();
        }

        

    }

}