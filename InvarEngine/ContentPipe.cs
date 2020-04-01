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

            List<Vector3> VertList = new List<Vector3>();


            for(int i = 0; i < Lines.Length - 1; i++)
            {

                //Console.Write(Lines[i] + "\n");
                //Console.Write("Index Number : " + i + "\n");
              
                if(Lines[i][0] == 'v' && Lines[i][1] == ' ')
                {
                    



                    string[] Points = Lines[i].Split(' '); 
                   
                    

                    /*string[] Points = new string[4]
                    {
                        "v",
                        "1",
                        "1",
                        "1"
                    };*/
                    
                    //Console.Write("Points : " + Points[1] + " " + "\n");
                    /*Console.Write("(" + 
                    Points[1] + ", " + 
                    Points[2] + ", " + 
                    Points[3] + 
                    "\n");*/

                    VertList.Add(new Vector3
                    (
                    float.Parse(Points[1]) * ImportScale,
                    float.Parse(Points[2]) * ImportScale,
                    float.Parse(Points[3]) * ImportScale
                    ));

                    //Console.Write("Converted to : " + VertList[i] + "\n");

                }
                

            }

            

            Vector3[] Vertices = new Vector3[VertList.Count];

            for(int i = 0; i < VertList.Count; i++)
            {
                Vertices[i] = VertList[i];
            }

      





            throw new NotImplementedException();

        }

    }
}
