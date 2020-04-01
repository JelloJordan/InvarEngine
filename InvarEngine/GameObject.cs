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

        bool hasRenderer;

        public GameObject(Vector3 Position, Vector3 Rotation, float Scale, bool hasRenderer = true)
        {

            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
            this.hasRenderer = hasRenderer;

            Vertex[] tempDefaultMesh = new Vertex[4]    //QUAD --REMOVE LATER
            {
                new Vertex(new Vector2(-.5f, -.5f), new Vector2(0,1)),
                new Vertex(new Vector2(-.5f,  .5f), new Vector2(0,0)),
                new Vertex(new Vector2( .5f,  .5f), new Vector2(1,0)),
                new Vertex(new Vector2( .5f, -.5f), new Vector2(1,1))
            };

            if(hasRenderer)
                Renderer = new Renderer(tempDefaultMesh, this);
            
        } 
    }
}
