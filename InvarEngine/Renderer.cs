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
        Texture2D Texture;
        OBJ Model;
        Material Mat;

        ShaderProgram Shader;

        int VBO;
        uint[] indexBuffer;
        int IBO;

        public Renderer(GameObject Parent)
        {
            this.Parent = Parent;
        }

        public void Bind(string modelFilePath, string materialFilePath)
        {
            Model = ContentPipe.LoadOBJ(modelFilePath, 1f);
            Mat = ContentPipe.LoadMaterial(materialFilePath);
            if(Model.ERROR)
            {
                Parent.Rotation = Vector3.Zero;
                Parent.Scale = 1f;
                Texture = ContentPipe.LoadTexture("ERROR.png", true);
            }else
            {
                Texture = ContentPipe.LoadTexture(Mat.TextureFilePath, Mat.Pixelated);
            }

            Shader = new ShaderProgram("Shader/Shader.vert", "Shader/Shader.frag");

            //Shader.SetVector3("lightPosition", new Vector3(0f,5f,0f));
            Shader.SetFloat("lightStrength", 0f);
            Shader.SetFloat("lightRange", 5f);

            Shader.SetVector3("directionalLightVector", new Vector3(0.5f, 1f, .5f));
            Shader.SetFloat("directionalLightStrength", 1f);
            
            Shader.SetFloat("ambientLightIntensity", .3f);

            //Shader.SetFloat("shineDamper", Mat.ShineDamper);
            //Shader.SetFloat("reflectivity", Mat.Reflectivity);
            
            
            

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
            Matrix4 projectionMatrix =  Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80), 1280f/720f, 0.1f, 100.0f);

            Matrix4 viewMatrix =    Matrix4.CreateTranslation(Camera.Position) *
                                    Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Camera.Rotation.Y)) *
                                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Camera.Rotation.X)) *
                                    Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Camera.Rotation.Z));

            Matrix4 RotationMatrix =    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Parent.Rotation.X)) * 
                                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Parent.Rotation.Y)) *
                                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Parent.Rotation.Z));

            Matrix4 modelMatrix =   Matrix4.Identity *
                                    Matrix4.CreateScale(Parent.Scale, Parent.Scale, Parent.Scale) *
                                    RotationMatrix * 
                                    Matrix4.CreateTranslation(Parent.Position);

            Shader.SetMatrix4("Model", modelMatrix);
            Shader.SetMatrix4("View", viewMatrix);
            Shader.SetMatrix4("Projection", projectionMatrix); 

            Shader.SetVector3("lightPosition", -Camera.Position);

            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false,Vertex.SizeInBytes, (IntPtr)0);           //VERTICE POSITIONS
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false,Vertex.SizeInBytes, (IntPtr)(Vector3.SizeInBytes));   //TEXTURE COORDINATES
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false,Vertex.SizeInBytes, (IntPtr)(Vector3.SizeInBytes + Vector2.SizeInBytes));  //COLOR IN VECTOR4
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false,Vertex.SizeInBytes, (IntPtr)(Vector3.SizeInBytes + Vector2.SizeInBytes + Vector4.SizeInBytes));  //NORMALS IN VECTOR3
            GL.EnableVertexAttribArray(3);

            //GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Texture.ID);    
            
            Shader.Use();
 
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);      
            GL.DrawElements(PrimitiveType.Triangles, indexBuffer.Length, DrawElementsType.UnsignedInt, 0);   

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
            GL.DisableVertexAttribArray(3);

            GL.BindTexture(TextureTarget.Texture2D, 0);

        }
    }
}
