using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace InvarEngine
{
    class VectorTools
    {
        public static Vector3 GetForward(Vector3 Rotation)
        {
            //Vector3 ReferenceDirection = new Vector3(0f, 0f, 1f); //Transform Forward, same as (0, 0, 0) in rotation

            float Y = Rotation.Y;

            while(Y > 359)
            {
                Y -= 359;
            }
            
            while(Y < 0)
            {

                Y += 359;

            }


            if(Y >= 0f && Y < 90f)
            {

                Y /= 90f;

                return Vector3.Lerp(new Vector3(0f, 0f, 1f), new Vector3(-1f, 0f, 0f), Y);

            }    

            if(Y >= 90f && Y < 180f)
            {
                
                Y -= 90f;
                Y /= 90f;

                return Vector3.Lerp(new Vector3(-1f, 0f, 0f), new Vector3(0f, 0f, -1f), Y);

            }   

            if(Y >= 180f && Y < 270f)
            {
                
                Y -= 180f;
                Y /= 90f;

                return Vector3.Lerp(new Vector3(0f, 0f, -1f), new Vector3(1f, 0f, 0f), Y);

            }  

            if(Y >= 270f && Y < 360f)
            {
                
                Y -= 270f;
                Y /= 90f;

                return Vector3.Lerp(new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), Y);

            }  


            return Vector3.Zero;

        }
    }
}