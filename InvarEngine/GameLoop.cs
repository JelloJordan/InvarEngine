using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;

namespace InvarEngine
{
    class GameLoop 
    {
        GameWindow window;

        Camera Camera;

        public List<GameObject> AllObjects = new List<GameObject>(0);

        float MouseSensitivity = 0.2f;

        public GameLoop(GameWindow window)
        {
            this.window = window;
        }

        public void Start()
        {   
            AllObjects = ContentPipe.LoadScene("TestScene.txt");
        }

        public void Update()
        {
            KeyboardState Keyboardinput = Keyboard.GetState();  //gets current keyboard input
            MouseState Mouseinput = Mouse.GetCursorState();

            PlayerMovement(Keyboardinput, Mouseinput);
        }

        public void Draw()
        {
            foreach (GameObject Object in AllObjects)
            {
                Object.Update(Camera);
            }
        }

        void PlayerMovement(KeyboardState Keyboardinput, MouseState Mouseinput)
        {
            float XDelta = 0f;
            float YDelta = 0f;

            if(window.Focused)
            {
                XDelta = (window.Location.X + window.Width/2) - Mouseinput.X;
                YDelta = (window.Location.Y + window.Height/2) - Mouseinput.Y;
                Mouse.SetPosition(window.Location.X + window.Width/2f, window.Location.Y + window.Height/2f);
            }

            Camera.Rotation += new Vector3(-YDelta, -XDelta, 0f) * MouseSensitivity;
            if(Camera.Rotation.X < -70f)
                Camera.Rotation = new Vector3(-70f, Camera.Rotation.Y, 0f);
            
            if(Camera.Rotation.X > 70f)
                Camera.Rotation = new Vector3(70f, Camera.Rotation.Y, 0f);
            
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
        }
    }
}