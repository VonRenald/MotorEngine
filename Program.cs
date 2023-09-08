using System;
using System.Windows.Input;
using OpenTK;
using OpenTK.Graphics;
// using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;


namespace MonitorEngine
{
    public class Program
    {
        private static GameWindow _window;

        private static int _vertexArray;
        private static int _vertexBuffer;
        private static int _vertexShader;
        private static int _fragmentShader;
        private static int _shaderProgram;

        private static DateTime _startTime = DateTime.Now;       
        private static int indicesLength;
        public static void Main()
        {
            var gameWindowSettings = GameWindowSettings.Default;
            
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(800, 600),
                Title = "Mon Cube OpenGL"
            };

            _window = new GameWindow(gameWindowSettings, nativeWindowSettings);

            _window.Load += OnLoad;
            _window.RenderFrame += OnRenderFrame;
            _window.Resize += OnResize;

            _window.Run();
        }

        private static void OnLoad()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            // Définissez ici vos shaders, vos buffers, etc.
            List<Cube> cubes = new List<Cube>();
            cubes.Add(new Cube(new Vector3(-2f,0f,1f),0.5f));
            cubes.Add(new Cube(new Vector3(-1f,0f,0f),0.5f));
            cubes.Add(new Cube(new Vector3(0f,0f,0f),0.5f));
            cubes.Add(new Cube(new Vector3(1f,0f,0f),0.5f));
            cubes.Add(new Cube(new Vector3(2f,0f,0f),0.5f));
            int lastIndice = 0;
            List<float> verticesList = new List<float>();
            List<int> indicesList = new List<int>();
            foreach(Cube cube in cubes){
                verticesList.AddRange(cube.GetVertices());
                indicesList.AddRange(cube.GetIndices(lastIndice));
                lastIndice += cube.GetIndicesLength();
            }


            float[] vertices = verticesList.ToArray();
            int[] indices = indicesList.ToArray();
            indicesLength = indices.Length;
            
            // Création et liaison du Vertex Array Object (VAO)
            _vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArray);

            // Création et liaison du Vertex Buffer Object (VBO)
            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Création et liaison de l'Element Buffer Object (EBO) pour les indices
            int ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            // Compilation et liaison des shaders (vertex et fragment)
            _vertexShader = LoadShader("vertex_shader.glsl", ShaderType.VertexShader);
            _fragmentShader = LoadShader("fragment_shader.glsl", ShaderType.FragmentShader);

            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, _vertexShader);
            GL.AttachShader(_shaderProgram, _fragmentShader);
            GL.LinkProgram(_shaderProgram);

            // Configuration des attributs de position des sommets
            int vertexPositionLocation = GL.GetAttribLocation(_shaderProgram, "vertexPosition");
            GL.VertexAttribPointer(vertexPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vertexPositionLocation);

            int vertexColorLocation = GL.GetAttribLocation(_shaderProgram, "vertexColor");
            GL.VertexAttribPointer(vertexColorLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(vertexColorLocation);

            GL.Enable(EnableCap.DepthTest);

            // Utilisation du shader program
            GL.UseProgram(_shaderProgram);
        }

        private static void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            double elapsedTime = (DateTime.Now - _startTime).TotalSeconds;

            float fov = MathHelper.DegreesToRadians(45f); // Champ de vision (Field of View) en radians. 45 degrés est couramment utilisé.
            float aspectRatio = (float)_window.Size.X / (float)_window.Size.Y; // Rapport largeur/hauteur de la fenêtre.
            float nearClip = 0.1f; // Distance du plan de clipping proche.
            float farClip = 100f; // Distance du plan de clipping éloigné.

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, nearClip, farClip);
            int location = GL.GetUniformLocation(_shaderProgram, "projection");
            GL.UniformMatrix4(location, false, ref projection);

            // Utilisation d'une matrice d'identité pour le modèle
            Matrix4 identity = Matrix4.Identity;
            location = GL.GetUniformLocation(_shaderProgram, "model");
            GL.UniformMatrix4(location, false, ref identity);

            // Déplacement de la "caméra" (en réalité, déplacement de la scène) vers l'arrière
            
            float radius = 6.0f; // distance de la caméra par rapport au centre du cube
            float camX = (float)(radius * Math.Cos(elapsedTime));
            float camZ = (float)(radius * Math.Sin(elapsedTime));

            Vector3 cameraPosition = new Vector3(camX, camZ, camZ);
            Vector3 target = Vector3.Zero;
            Vector3 up = Vector3.UnitY;// Le vecteur "haut" de la caméra est le long de l'axe Y.
            
            Matrix4 view = Matrix4.LookAt(cameraPosition, target, up);
            location = GL.GetUniformLocation(_shaderProgram, "view");
            GL.UniformMatrix4(location, false, ref view);

            // Dessin du cube
            GL.BindVertexArray(_vertexArray);
            GL.DrawElements(PrimitiveType.Triangles, indicesLength, DrawElementsType.UnsignedInt, 0);
            // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            _window.SwapBuffers();
        }

        private static int LoadShader(string path, ShaderType type)
        {
            string source = System.IO.File.ReadAllText(path);
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            // Vérification des erreurs de compilation
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                Console.WriteLine($"Erreur lors de la compilation du shader ({type}): {infoLog}");
            }

            return shader;
        }
        private static void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, _window.Size.X, _window.Size.Y);
        }

    }
}
