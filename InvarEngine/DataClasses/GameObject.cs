using System;
using OpenTK;

namespace InvarEngine
{
    class GameObject
    { 

        Renderer renderer;
        Vector3 position;
        Vector3 rotation;
        float scale;

        public Renderer Renderer
        {
            get
            {
                return renderer;
            }
            set
            {
                renderer = value;
            }
        }

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

        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        public GameObject(Vector3 Position, Vector3 Rotation, float Scale)
        {

            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;

            Renderer = new Renderer(this);
            
        } 

        public void Update(Camera Camera)
        {

            Renderer.Draw(Camera);

        }

        public void Destroy()
        {

            throw new NotImplementedException();

        }
    }
}
