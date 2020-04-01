using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace InvarEngine
{
    class ContentPipe
    { 

        /// <summary>
        ///
        /// </summary>
        /// <param name = "filePath">Defaults to "Content/"</param>
        /// <param name = "pixelated"></param>
        /// <returns></returns>

        public static Texture2D LoadTexture(string filePath, bool pixelated = false)
        {

            filePath = "Content/" + filePath;

            if(!File.Exists(filePath))
            {

                    throw new Exception("File does not exist at '" + filePath + "'");

            }

            Bitmap bmp = new Bitmap(filePath);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, pixelated ? (int)TextureMinFilter.Nearest : (int)TextureMinFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, pixelated ? (int)TextureMagFilter.Nearest : (int)TextureMagFilter.Linear);

            return new Texture2D(id, new Vector2(bmp.Width, bmp.Height));

        }

        public static OBJ LoadOBJ(string filePath, float ImportScale ,bool ImportNormals = false)
        {

            filePath = "Content/" + filePath;

            if(!File.Exists(filePath))
            {

                throw new Exception("File does not exist at '" + filePath + "'");

            }

            StreamReader reader = new StreamReader(filePath);
            string TotalContents = reader.ReadToEnd();
            string[] Lines = TotalContents.Split('\n');

            List<Vector2> UV = new List<Vector2>();
            for(int i = 0; i < Lines.Length - 1; i++)
            {
                if(Lines[i][0] == 'v' && Lines[i][1] == 't')
                {
        
                    string[] Points = Lines[i].Split(' '); 
                   
                    UV.Add(new Vector2(float.Parse(Points[1]), float.Parse(Points[2])));

        


                }
            }//--UVS DONE ---

            List<Vertex> VertList = new List<Vertex>();
            for(int i = 0; i < Lines.Length - 1; i++)
            {

              
                if(Lines[i][0] == 'v' && Lines[i][1] == ' ')
                {
        
                    string[] Points = Lines[i].Split(' '); 

                    Vector2 uvPoint = UV[VertList.Count];
                   
                    VertList.Add(new Vertex(new Vector3
                    (
                    float.Parse(Points[1]) * ImportScale,
                    float.Parse(Points[2]) * ImportScale,
                    float.Parse(Points[3]) * ImportScale
                    ), uvPoint));

                }
                
                

            } //Vertices Finished

            List<uint> IndList = new List<uint>();

            for(int i = 0; i < Lines.Length - 1; i++)
            {

                if(Lines[i][0] == 'f')
                {

                    string[] Points = Lines[i].Split(' '); 

                    string[] first = Points[1].Split('/');
                    string[] second = Points[2].Split('/');
                    string[] third = Points[3].Split('/');
    

                    //Console.Write(Points[1] + "\n");
                    IndList.Add(Convert.ToUInt32(first[0]) - 1);
                    IndList.Add(Convert.ToUInt32(second[0]) - 1);
                    IndList.Add(Convert.ToUInt32(third[0]) - 1);

                    //Console.Write(first[0] + "/" + second[0] + "/" + third[0] + "\n");


                    

                }

            } // INDICES FINSIHED

            //(rand() % (max- min)) + min

            //Vertex[] Verts = new Vertex[VertList.Count];
            //Verts = new Vertex[VertList.Count];

            //List
            /*
            
            for(int i = 0; i < VertList.Count; i++)
            {   
                Verts[i].position = VertList[i];
                Verts[i].texCoord = new Vector2(0f, 0f);

                //Console.WriteLine(Verts[i].position);
            }
            */
            uint[] Indices = new uint[IndList.Count];

            //Console.WriteLine(IndList.Count);

            for(int i = 0; i < IndList.Count; i++)
            {
                //Console.WriteLine(IndList[i]);
                Indices[i] = IndList[i];
            }

            return new OBJ(VertList.ToArray(), Indices);

      

        }

    }
}
