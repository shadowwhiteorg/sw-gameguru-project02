using UnityEngine;
using _Game.Systems.Core;

namespace _Game.Systems.MeshSystem
{
    public static class MeshGenerator
    {
        public static Mesh CreateMesh(Vector3 dimensions)
        {
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(dimensions.x, 0, 0),
                new Vector3(dimensions.x, dimensions.y, 0),
                new Vector3(0, dimensions.y, 0),
                new Vector3(0, 0, dimensions.z),
                new Vector3(dimensions.x, 0, dimensions.z),
                new Vector3(dimensions.x, dimensions.y, dimensions.z),
                new Vector3(0, dimensions.y, dimensions.z)
            };
            mesh.triangles = new int[]
            {
                0, 2, 1, 0, 3, 2,
                1, 6, 5, 1, 2, 6,
                5, 6, 7, 5, 7, 4,
                4, 7, 3, 4, 3, 0,
                3, 7, 6, 3, 6, 2,
                4, 0, 1, 4, 1, 5
            };
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
