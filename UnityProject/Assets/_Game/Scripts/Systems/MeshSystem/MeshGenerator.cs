using UnityEngine;
using _Game.Systems.Core;

namespace _Game.Systems.MeshSystem
{
    public class MeshGenerator : MonoBehaviour
    {
        public Platform GeneratePlatform(Vector3 dimensions, Material material, Vector3 position)
        {
            GameObject newPlatformObj = new GameObject("Platform");
            newPlatformObj.transform.position = position;
            Platform platform = newPlatformObj.AddComponent<Platform>();
            platform.Initialize(dimensions, material);
            MeshFilter meshFilter = newPlatformObj.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = newPlatformObj.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
        
            Mesh mesh = CreateMesh(dimensions);
            meshFilter.mesh = mesh;
            return platform;
        }

        private Mesh CreateMesh(Vector3 dimensions)
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