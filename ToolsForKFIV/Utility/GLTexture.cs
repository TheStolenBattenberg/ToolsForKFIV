using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL;
using FormatKFIV.Asset;

namespace ToolsForKFIV.Utility
{
    public class GLTexture
    {
        private int _TexHandle;
        public int TexHandle
        {
            get { return _TexHandle; }
            set { _TexHandle =value; }
        }

        public static GLTexture Generate44Grid()
        {
            GLTexture glTex = new GLTexture();

            //Generate Pixels
            byte[] gridTexture = new byte[(4 * 4) * 4];

            for(int i = 0; i < 4; ++i)
            {
                for(int j = 0; j < 4; ++j)
                {
                    int stride = (16 * j) + 4 * i;

                    if((i + j) % 2 == 0)
                    {
                        gridTexture[stride + 0] = 0xD0;
                        gridTexture[stride + 1] = 0xD0;
                        gridTexture[stride + 2] = 0xD0;
                        gridTexture[stride + 3] = 0xFF;
                    }
                    else
                    {
                        gridTexture[stride + 0] = 0x20;
                        gridTexture[stride + 1] = 0x20;
                        gridTexture[stride + 2] = 0x20;
                        gridTexture[stride + 3] = 0xFF;
                    }
                }
            }

            glTex._TexHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, glTex._TexHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);

            GL.TexImage2D<byte>(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 4, 4, 0, PixelFormat.Rgba, PixelType.UnsignedByte, gridTexture);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            return glTex;
        }
        public static GLTexture Generate44White()
        {
            GLTexture glTex = new GLTexture();

            //Generate Pixels
            byte[] whiteTexture = new byte[(4 * 4) * 4];

            for(int i = 0; i < whiteTexture.Length; ++i)
            {
                whiteTexture[i] = 0xFF;
            }

            glTex._TexHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, glTex._TexHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);

            GL.TexImage2D<byte>(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 4, 4, 0, PixelFormat.Rgba, PixelType.UnsignedByte, whiteTexture);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            return glTex;
        }
        public static GLTexture GenerateFromAsset(Texture tex)
        {
            GLTexture glTex = new GLTexture();

            //Get RGBA Version of texture
            Texture.ImageBuffer? bufNullable = tex.GetSubimage(0);
            Texture.ImageBuffer texBuffer;
            if (!bufNullable.HasValue)
            {
                Console.WriteLine("NULL SUBIMAGE LOLLOLOLL;[GFAO");
                return null;
            }
            texBuffer = bufNullable.Value;
            byte[] rgbaTex = tex.GetSubimageAsRGBA(0);

            glTex._TexHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, glTex._TexHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);

            GL.TexImage2D<byte>(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, (int)texBuffer.Width, (int)texBuffer.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, rgbaTex);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            return glTex;
        }
        public static GLTexture GenerateFromSubImage(Texture texture, int subImage)
        {
            //Get RGBA Version of texture
            Texture.ImageBuffer? bufNullable = texture.GetSubimage(subImage);
            Texture.ImageBuffer texBuffer;
            if (!bufNullable.HasValue || bufNullable == null)
            {
                Logger.LogError("GLTEXTURE -> NULL SUBIMAGE, FML? YEAH.");
                return null;
            }
            texBuffer = bufNullable.Value;
            byte[] rgbaTex = texture.GetSubimageAsRGBA(subImage);

            GLTexture glTex = new GLTexture();

            glTex._TexHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, glTex._TexHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, (int)texBuffer.Width, (int)texBuffer.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, rgbaTex);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            return glTex;
        }

        public void Bind(TextureUnit unit = TextureUnit.Texture0, TextureTarget target = TextureTarget.Texture2D)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(target, _TexHandle);
        }
        public void Destroy()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.DeleteTexture(_TexHandle);
        }
    }
}
