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
        OBJ Model;

        int VBO;
        uint[] indexBuffer;
        int IBO;

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

        public Renderer(GameObject Parent)
        {
            this.Parent = Parent;
        }

        public void Bind(string modelFilePath, string textureFilePath)
        {
            
            Model = ContentPipe.LoadOBJ(modelFilePath, 1f);
            if(Model.ERROR)
            {
                Parent.Rotation = Vector3.Zero;
                Texture = ContentPipe.LoadTexture("ERROR.png", true);
            }else
            {
                Texture = ContentPipe.LoadTexture(textureFilePath);
            }
            

            Mesh = Model.Mesh;
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * Mesh.Length), Mesh, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            indexBuffer = Model.Indices;
            IBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * indexBuffer.Length), indexBuffer, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Draw(Camera Camera)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.ID);        

            Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80), 1280f/720f, 0.1f, 100.0f); 
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projectionMatrix);

            Matrix4 viewMatrix =     
                                    Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Camera.Rotation.Y)) *
                                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Camera.Rotation.X)) *
                                    Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Camera.Rotation.Z));

            Matrix4 RotationMatrix =    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Parent.Rotation.X)) * 
                                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Parent.Rotation.Y)) *
                                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Parent.Rotation.Z));

            Matrix4 modelMatrix =   Matrix4.Identity *
                                    Matrix4.CreateScale(Parent.Scale, Parent.Scale, Parent.Scale) *
                                    RotationMatrix * 
                                    Matrix4.CreateTranslation(Parent.Position) * 
                                    Matrix4.CreateTranslation(Camera.Position) *
                                    viewMatrix;

   
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelMatrix);

            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeInBytes, (IntPtr)0);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, (IntPtr)(Vector3.SizeInBytes));
            GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, (IntPtr)(Vector3.SizeInBytes + Vector2.SizeInBytes));

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);      
            GL.DrawElements(PrimitiveType.Triangles, indexBuffer.Length, DrawElementsType.UnsignedInt, 0);   

        }
        
    }
}
