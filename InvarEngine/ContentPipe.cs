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

                //throw new Exception("File does not exist at '" + filePath + "'");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR : Filename '" + filePath + "' not found!");
                Console.ResetColor();

                filePath = "Content/" + "ERROR.png";
                pixelated = true;

            }

            Bitmap bmp = new Bitmap(filePath);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, pixelated ? (int)TextureMinFilter.Nearest : (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, pixelated ? (int)TextureMagFilter.Nearest : (int)TextureMagFilter.Linear);

            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.Repeat);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.Repeat);

            return new Texture2D(id, new Vector2(bmp.Width, bmp.Height));

        }

        public static OBJ LoadOBJ(string filePath, float ImportScale ,bool ImportNormals = false)
        {
            filePath = "Content/" + filePath;
            bool ERROR = false;

            if(!File.Exists(filePath))
            {
                //throw new Exception("File does not exist at '" + filePath + "'");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR : Filename '" + filePath + "' not found!");
                Console.ResetColor();

                filePath = "Content/" + "ERROR.Obj";
                ImportScale = 1;
                ERROR = true;
            }

            StreamReader reader = new StreamReader(filePath);
            string TotalContents = reader.ReadToEnd();
            string[] Lines = TotalContents.Split('\n');

            List<uint> IndList = new List<uint>();
            List<int> UVOrder = new List<int>();

            for(int i = 0; i < Lines.Length - 1; i++)
            {
                if(Lines[i][0] == 'f')
                {
                    string[] Points = Lines[i].Split(' '); 

                    string[] first = Points[1].Split('/');
                    string[] second = Points[2].Split('/');
                    string[] third = Points[3].Split('/');
    
                    IndList.Add(Convert.ToUInt32(first[0]) - 1);
                    IndList.Add(Convert.ToUInt32(second[0]) - 1);
                    IndList.Add(Convert.ToUInt32(third[0]) - 1);

                    UVOrder.Add(Convert.ToInt32(first[1]) - 1);
                    UVOrder.Add(Convert.ToInt32(second[1]) - 1);
                    UVOrder.Add(Convert.ToInt32(third[1]) - 1);
                }
            }// ----------INDICES FINSIHED----------

            List<Vector2> UV = new List<Vector2>();
            for(int i = 0; i < Lines.Length - 1; i++)
            {
                if(Lines[i][0] == 'v' && Lines[i][1] == 't')
                {
                    string[] Points = Lines[i].Split(' '); 
                    UV.Add(new Vector2(float.Parse(Points[1]), float.Parse(Points[2])));
                }
            }//----------UVS FINSIHED----------

            List<Vertex> VertList = new List<Vertex>();
            for(int i = 0; i < Lines.Length - 1; i++)
            {
                if(Lines[i][0] == 'v' && Lines[i][1] == ' ')
                {
                    string[] Points = Lines[i].Split(' '); 

                    Vector2 uvPoint = Vector2.Zero;
                    for(int j = 0; j < IndList.Count; j++)
                    {

                        if(IndList[j] == VertList.Count)
                        {
                            
                            uvPoint = UV[UVOrder[j]];
                            //Console.WriteLine((VertList.Count + 1) + "/" + (UVOrder[j] + 1) + "/0");
                            break;
                        }

                    }
                    
                    
                    VertList.Add(new Vertex(new Vector3
                    (
                    float.Parse(Points[1]) * ImportScale,
                    float.Parse(Points[2]) * ImportScale,
                    float.Parse(Points[3]) * ImportScale
                    ), uvPoint));
                }
            }//----------VERTICES FINSIHED----------
            
            return new OBJ(VertList.ToArray(), IndList.ToArray()){ERROR = ERROR};
        }
    }
}
