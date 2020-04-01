using System;
using OpenTK;

namespace InvarEngine
{
    struct OBJ
    { 
        //Eventually include Mesh, UV Coords, and Normals in 3 different arrays;

        private Vertex[] mesh;
        private uint[] indices;
        
        public Vertex[] Mesh { get { return mesh; } }
        public uint[] Indices { get { return indices; } }

        public OBJ(Vertex[] mesh, uint[] indices)
        {
            this.mesh = mesh;
            this.indices = indices;
        }
    }
}
