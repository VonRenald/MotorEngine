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
    class Cube
    {
        public Cube(OpenTK.Mathematics.Vector3 center_ ,float size_ = 1f)
        {
            
            Center = center_;
            this.Size = size_;

            for(int i=0; i< vertices.Length;i+=6)
            {
                vertices[i] = vertices[i]*Size+Center.X;
                vertices[i+1] = vertices[i+1]*Size+Center.Y;
                vertices[i+2] = vertices[i+2]*Size+Center.Z;
            }
            
        }

        public float[] GetVertices() { return vertices;}
        public int GetVerticesLength() {return vertices.Length;}
        public int[] GetIndices(int shift=0) { 
            if(shift == 0) return indices;
            int[] newIndices = new int[indices.Length];
            for(int i=0; i<indices.Length; i++)
                newIndices[i]=indices[i]+shift;
            return newIndices;
        }
        public int GetIndicesLength(){return indices.Length;}
        private float Size;
        private Vector3 Center = Vector3.Zero;
        public static int IndiceNumber = 24;
        private float[] vertices = 
        {
            // Face avant                           // Couleur
            -0.5f, -0.5f,  0.5f,  /* Bas gauche */  1.0f, 0.0f, 0.0f, // rouge
            0.5f, -0.5f,  0.5f,  /* Bas droit */    1.0f, 0.0f, 0.0f,
            0.5f,  0.5f,  0.5f,  /* Haut droit */   1.0f, 0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  /* Haut gauche */ 1.0f, 0.0f, 0.0f,

            // Face arrière
            -0.5f, -0.5f, -0.5f,  /* Bas gauche */  0.0f, 1.0f, 0.0f,
            0.5f, -0.5f, -0.5f,  /* Bas droit */    0.0f, 1.0f, 0.0f,
            0.5f,  0.5f, -0.5f,  /* Haut droit */   0.0f, 1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  /* Haut gauche */ 0.0f, 1.0f, 0.0f,

            // Face supérieure
            -0.5f,  0.5f,  0.5f,  /* Avant gauche */0.0f, 0.0f, 1.0f,
            0.5f,  0.5f,  0.5f,  /* Avant droit */  0.0f, 0.0f, 1.0f,
            0.5f,  0.5f, -0.5f,  /* Arrière droit */0.0f, 0.0f, 1.0f,
            -0.5f,  0.5f, -0.5f, /* Arrièregauche */0.0f, 0.0f, 1.0f,

            // Face inférieure
            -0.5f, -0.5f,  0.5f,  /* Avant gauche */0.5f, 0.5f, 0.0f,
            0.5f, -0.5f,  0.5f,  /* Avant droit */  0.5f, 0.5f, 0.0f,
            0.5f, -0.5f, -0.5f,  /* Arrière droit */0.5f, 0.5f, 0.0f,
            -0.5f, -0.5f, -0.5f,/* Arrière gauche */0.5f, 0.5f, 0.0f,

            // Face gauche
            -0.5f, -0.5f, -0.5f, /* Bas arrière */  0.0f, 0.5f, 0.5f,
            -0.5f, -0.5f,  0.5f, /* Bas avant */    0.0f, 0.5f, 0.5f,
            -0.5f,  0.5f,  0.5f, /* Haut avant */   0.0f, 0.5f, 0.5f,
            -0.5f,  0.5f, -0.5f, /* Haut arrière */ 0.0f, 0.5f, 0.5f,

            // Face droite
            0.5f, -0.5f, -0.5f,  /* Bas arrière */  0.5f, 0.0f, 0.5f,
            0.5f, -0.5f,  0.5f,  /* Bas avant */    0.5f, 0.0f, 0.5f,
            0.5f,  0.5f,  0.5f,  /* Haut avant */   0.5f, 0.0f, 0.5f,
            0.5f,  0.5f, -0.5f,  /* Haut arrière */ 0.5f, 0.0f, 0.5f
        };
        private int[] indices = 
        {
            // Face avant
            0, 1, 2,
            2, 3, 0,

            // Face arrière
            4, 5, 6,
            6, 7, 4,

            // Face supérieure
            8, 9, 10,
            10, 11, 8,

            // Face inférieure
            12, 13, 14,
            14, 15, 12,

            // Face gauche
            16, 17, 18,
            18, 19, 16,

            // Face droite
            20, 21, 22,
            22, 23, 20
        };
    }
}