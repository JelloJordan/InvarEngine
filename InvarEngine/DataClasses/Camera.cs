using System;
using OpenTK;

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
}