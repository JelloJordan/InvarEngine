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

        GameObject Test;
        GameObject Floor;
        GameObject Sphere;

        List<GameObject> AllObjects;

        float MouseSensitivity = 0.2f;

        public GameLoop(GameWindow window)
        {
            this.window = window;
        }

        public void Start()
        {
            Test = new GameObject(new Vector3(0f, 0f, -5f), new Vector3(0f, 0f, 180f), .5f, true);
            Test.Renderer.Bind("Cube.obj", "Icon.png");

            Floor = new GameObject(new Vector3(0f, -1f, 0f), new Vector3(0f, 0f, 0f), 5f, true);
            Floor.Renderer.Bind("Quad.obj", "Grass.jpg");

            Sphere = new GameObject(new Vector3(-5f, 0f, 0f), new Vector3(0f, 0f, 0f), 1f, true);
            Sphere.Renderer.Bind("Sphere.obj", "Grass.jpg");
        }

        public void Update()
        {
            KeyboardState Keyboardinput = Keyboard.GetState();  //gets current keyboard input
            MouseState Mouseinput = Mouse.GetCursorState();

            PlayerMovement(Keyboardinput, Mouseinput);
        }

        public void Draw()
        {
            Floor.Renderer.Draw(Camera);
            Test.Renderer.Draw(Camera);
            Sphere.Renderer.Draw(Camera);
        }

        void PlayerMovement(KeyboardState Keyboardinput, MouseState Mouseinput)
        {
            float XDelta = (window.Location.X + window.Width/2) - Mouseinput.X;
            float YDelta = (window.Location.Y + window.Height/2) - Mouseinput.Y;

            Mouse.SetPosition(window.Location.X + window.Width/2f, window.Location.Y + window.Height/2f);

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