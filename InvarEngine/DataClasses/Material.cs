using System;
using OpenTK;

namespace InvarEngine
{
    struct Material
    { 

        private string textureFilePath;
        private float shineDamper;
        private float reflectivity;

        public string TextureFilePath { get { return textureFilePath; } set { textureFilePath = value;} }
        public float ShineDamper { get { return shineDamper; } }
        public float Reflectivity { get { return reflectivity; } }

        public Material(string textureFilePath, float shineDamper, float reflectivity)
        {

            this.textureFilePath = textureFilePath;
            this.shineDamper = shineDamper;
            this.reflectivity = reflectivity;

        }
    }
}
