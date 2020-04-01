using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace InvarEngine
{
    
    class Renderer
    { 
        GameObject Parent;
        Vertex[] Mesh;
        Texture2D texture;

        public Texture2D Texture
        {
            get
            {

                return texture;

            }
            set
            {
                this.texture = value;
            }
        }

        int VBO;
        uint[] indexBuffer;
        int IBO;

        public Renderer(Vertex[] Mesh, GameObject Parent)
        {

            this.Mesh = Mesh;
            this.Parent = Parent;

        }

        public void Bind()
        {

            Texture = ContentPipe.LoadTexture("Icon.png"); //REMOVE LATER, THIS IS DEFAULT TEXTURE

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * Mesh.Length), Mesh, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            indexBuffer = new uint[4]       //8 size for room for two quads, pass these later on
            {
                0, 1, 2, 3     //right side up quad
            };

            IBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * indexBuffer.Length), indexBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

        }

        public void Draw()
        {

            GL.BindTexture(TextureTarget.Texture2D, Texture.ID);        

            Matrix4 world = Matrix4.CreateTranslation(Parent.Position);
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

        }
        
    }
}
