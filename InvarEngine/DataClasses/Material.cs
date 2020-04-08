using System;
using OpenTK;

namespace InvarEngine
{
    struct Material
    { 

        private string textureFilePath;
        private float shineDamper;
        private float reflectivity;
        private bool pixelated;

        public string TextureFilePath { get { return textureFilePath; } set { textureFilePath = value;} }
        public float ShineDamper { get { return shineDamper; } }
        public float Reflectivity { get { return reflectivity; } }
        public bool Pixelated { get { return pixelated;}}

        public Material(string textureFilePath, float shineDamper, float reflectivity, bool pixelated)
        {

            this.textureFilePath = textureFilePath;
            this.shineDamper = shineDamper;
            this.reflectivity = reflectivity;
            this.pixelated = pixelated;

        }
    }
}
