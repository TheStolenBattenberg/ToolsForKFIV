using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ToolsForKFIV.Utility
{
    public class GLShader
    {
        private int _ProgramHandle;
        public int ProgramHandle
        {
            get { return _ProgramHandle; }
            private set { _ProgramHandle = value; }
        }
        
        public GLShader(string vsPath, string fsPath)
        {
            string vsSource, fsSource;
            string result;

            //Read Vs/Fs Shader Sources
            try
            {
                vsSource = File.ReadAllText(vsPath);
                fsSource = File.ReadAllText(fsPath);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return;
            }


            //Create and Compile Vertex Shader
            int vShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vShader, vsSource);
            GL.CompileShader(vShader);

            result = GL.GetShaderInfoLog(vShader);
            if(result != String.Empty)
            {
                Logger.LogError("SHADER ERROR! Vertex shader could not be compiled.");
                Logger.LogInfo(result);

                //Clean Up
                GL.DeleteShader(vShader);
                return;
            }

            //Create and Compile Fragment Shader
            int fShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fShader, fsSource);
            GL.CompileShader(fShader);

            result = GL.GetShaderInfoLog(fShader);
            if (result != String.Empty)
            {
                Logger.LogError("SHADER ERROR! Fragment shader could not be compiled.");
                Logger.LogInfo(result);

                //Clean Up
                GL.DeleteShader(vShader);
                GL.DeleteShader(fShader);
                return;
            }

            //Create GL Program
            ProgramHandle = GL.CreateProgram();
            GL.AttachShader(ProgramHandle, vShader);
            GL.AttachShader(ProgramHandle, fShader);
            GL.LinkProgram(ProgramHandle);

            //Clean up
            GL.DetachShader(ProgramHandle, vShader);
            GL.DetachShader(ProgramHandle, fShader);
            GL.DeleteShader(vShader);
            GL.DeleteShader(fShader);

        }

        public void Destroy()
        {
            GL.UseProgram(0);
            GL.DeleteShader(_ProgramHandle);
        }
        public void Bind()
        {
            GL.UseProgram(_ProgramHandle);
        }

        public void SetUniformMat4(string uniformName, Matrix4 mat, bool transpose)
        {
            int uniLocation = GL.GetUniformLocation(_ProgramHandle, uniformName);
            GL.UniformMatrix4(uniLocation, transpose, ref mat);
        }
        public void SetUniformInt1(string uniformName, int value)
        {
            int uniLocation = GL.GetUniformLocation(_ProgramHandle, uniformName);
            GL.Uniform1(uniLocation, value);
        }
    }
}
