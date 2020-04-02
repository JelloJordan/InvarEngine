using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace InvarEngine
{
    class ShaderProgram 
    {
        int Handle;
        string VertexShaderSource;
        string FragmentShaderSource;

        Dictionary<string, int> _uniformLocations;

        public ShaderProgram(string vertexPath, string fragmentPath)
        {
            using (StreamReader reader = new StreamReader("Shaders/Shader.vert", Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            using (StreamReader reader = new StreamReader("Shaders/Shader.frag", Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            var VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);
            GL.CompileShader(FragmentShader);
    
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

            
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            _uniformLocations = new Dictionary<string, int>();

             for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                // get the location,
                var location = GL.GetUniformLocation(Handle, key);
                
                // and then add it to the dictionary.
                _uniformLocations.Add(key, location);
            }



        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
           try{
                GL.UseProgram(Handle);
                GL.UniformMatrix4(_uniformLocations[name], true, ref data);
           }catch{}
            
        }

        public void SetVector3(string name, Vector3 data)
        {
            try{
                GL.UseProgram(Handle);
                GL.Uniform3(_uniformLocations[name], ref data);
            }catch{}
        }
    }
}