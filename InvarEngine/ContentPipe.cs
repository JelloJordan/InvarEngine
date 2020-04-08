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

            filePath = "Content/Textures/" + filePath;

            if(!File.Exists(filePath))
            {

                //throw new Exception("File does not exist at '" + filePath + "'");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR : Filename '" + filePath + "' not found!");
                Console.ResetColor();

                filePath = "Content/Textures/" + "ERROR.png";
                pixelated = true;

            }

            Bitmap bmp = new Bitmap(filePath);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int id = GL.GenTexture();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);  
            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, pixelated ? (int)TextureMinFilter.Nearest : (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, pixelated ? (int)TextureMagFilter.Nearest : (int)TextureMagFilter.Linear);

            float maxAniso = 4f;
            GL.GetFloat((GetPName)ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out maxAniso);
            GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, maxAniso);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            return new Texture2D(id, new Vector2(bmp.Width, bmp.Height));

        }

        public static OBJ LoadOBJ(string filePath, float ImportScale ,bool ImportNormals = false)
        {
            filePath = "Content/Models/" + filePath;
            bool ERROR = false;

            if(!File.Exists(filePath))
            {
                //throw new Exception("File does not exist at '" + filePath + "'");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR : Filename '" + filePath + "' not found!");
                Console.ResetColor();

                filePath = "Content/Models/" + "ERROR.Obj";
                ImportScale = 1;
                ERROR = true;
            }

            StreamReader reader = new StreamReader(filePath);
            string TotalContents = reader.ReadToEnd();
            string[] Lines = TotalContents.Split('\n');

            List<uint> IndList = new List<uint>();
            List<int> UVOrder = new List<int>();
            List<int> NormalOrder = new List<int>();

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

                    NormalOrder.Add(Convert.ToInt32(first[2]) - 1);
                    NormalOrder.Add(Convert.ToInt32(second[2]) - 1);
                    NormalOrder.Add(Convert.ToInt32(third[2]) - 1);
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

            List<Vector3> Normals = new List<Vector3>();
            for(int i = 0; i < Lines.Length - 1; i++)
            {
                if(Lines[i][0] == 'v' && Lines[i][1] == 'n')
                {
                    string[] Points = Lines[i].Split(' '); 
                    Normals.Add(new Vector3(float.Parse(Points[1]), float.Parse(Points[2]), float.Parse(Points[3])));
                }
            }//----------NORMALS FINSIHED----------

            List<Vertex> VertList = new List<Vertex>();
            for(int i = 0; i < Lines.Length - 1; i++)
            {
                if(Lines[i][0] == 'v' && Lines[i][1] == ' ')
                {
                    string[] Points = Lines[i].Split(' '); 

                    Vector3 normalPoint = Vector3.Zero;
                    for(int j = 0; j < IndList.Count; j++)
                    {
                        if(IndList[j] == VertList.Count)
                        {
                            normalPoint = Normals[NormalOrder[j]];
                            break;
                        }
                    }

                    Vector2 uvPoint = Vector2.Zero;
                    for(int j = 0; j < IndList.Count; j++)
                    {
                        if(IndList[j] == VertList.Count)
                        {
                            uvPoint = UV[UVOrder[j]];
                            break;
                        }
                    }
                    
                    
                    VertList.Add(new Vertex(new Vector3
                    (
                    float.Parse(Points[1]) * ImportScale,
                    float.Parse(Points[2]) * ImportScale,
                    float.Parse(Points[3]) * ImportScale
                    ), uvPoint, normalPoint));
                }
            }//----------VERTICES FINSIHED----------
            
            return new OBJ(VertList.ToArray(), IndList.ToArray()){ERROR = ERROR};
        }

        public static Material LoadMaterial(string filePath)
        {

            filePath = "Content/Materials/" + filePath;

            if(!File.Exists(filePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR : Filename '" + filePath + "' not found!");
                Console.ResetColor();
                filePath = "Content/Materials/" + "ERROR.txt";
            }

            StreamReader reader = new StreamReader(filePath);
            string TotalContents = reader.ReadToEnd();
            string[] Lines = TotalContents.Split('\n');
            
            return new Material(Lines[0], float.Parse(Lines[1]), float.Parse(Lines[2]));

        }

        public static List<GameObject> LoadScene(string filePath)
        {
            filePath = "Content/Scenes/" + filePath;

            if(!File.Exists(filePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR : Filename '" + filePath + "' not found!");
                Console.ResetColor();
                return new List<GameObject>();
            }

            StreamReader reader = new StreamReader(filePath);
            string TotalContents = reader.ReadToEnd();
            string[] Lines = TotalContents.Split('\n');

            List<GameObject> AllObjects = new List<GameObject>();

            float xOffset = 0;

            for(int j = 0; j < 1; j++)
            {
                
            for(int i = 0; i < Lines.Length - 1; i++)
            {
                if(Lines[i][0] == 'p')
                {
                    string[] Vectors = Lines[i].Split(' '); 

                    GameObject Object = new GameObject
                    (
                        new Vector3(float.Parse(Vectors[1]) + xOffset, float.Parse(Vectors[2]), float.Parse(Vectors[3])), 
                        new Vector3(float.Parse(Vectors[4]), float.Parse(Vectors[5]), float.Parse(Vectors[6])), 
                        float.Parse(Vectors[7])
                    );

                    string[] Paths = Lines[i + 1].Split(' ');

                    Object.Renderer.Bind(Paths[1], Paths[2]);

                    AllObjects.Add(Object);
                }
            }
                xOffset += 10f;
            }

            return AllObjects;
        }
    }
}
