using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParticle : MonoBehaviour
{
    private const int MAX_QUADS = 1000;
    
    [System.Serializable]
    private struct UVCoords
    {
        public Vector2 UV00;
        public Vector2 UV11;
    }

    [System.Serializable]
    public struct Particle
    {
        public int quadIndex;
        public Vector3 position;
        public float rotation;
        public Vector3 quadSize;
        public bool skewed;
        public int uvIndex;

        public ParticleRandomProperties randomProperties;
    }

    public struct ParticleRandomProperties
    {
        public Vector3 minPosition;
        public Vector3 maxPosition;
        public int minRotation;
        public int maxRotation;
        public Vector3 minQuadSize;
        public Vector3 maxQuadSize;
        
        public Vector3 RandomPos => new Vector3(
            UnityEngine.Random.Range(minPosition.x, maxPosition.x),
            UnityEngine.Random.Range(minPosition.y, maxPosition.y),
            UnityEngine.Random.Range(minPosition.z, maxPosition.z));
        public int RandomRot => UnityEngine.Random.Range(minRotation, maxRotation);
        public Vector3 RandomQuadSize => new Vector3(
            UnityEngine.Random.Range(minQuadSize.x, maxQuadSize.x),
            UnityEngine.Random.Range(minQuadSize.y, maxQuadSize.y),
            UnityEngine.Random.Range(minQuadSize.z, maxQuadSize.z));
    }
    
    [SerializeField]    
    private List<Particle> particleList = new();
        
    [SerializeField]
    private List<UVCoords> uvCoordsList = new();
    
    private Mesh mesh;
    private Texture texture;
    
    [SerializeField]
    private int row = 1;
    [SerializeField]
    private int column = 1;

    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangles;

    public int quadIndex;

    private void Awake()
    {
        mesh = new Mesh();
        
        vertices = new Vector3[MAX_QUADS * 4];
        uvs = new Vector2[MAX_QUADS * 4];
        triangles = new int[MAX_QUADS * 6];
        
        
        GetComponent<MeshFilter>().mesh = mesh;
        texture = GetComponent<MeshRenderer>().material.mainTexture;
        MakeUVCoords();
    }
    private void Update()
    {
        UpdateQuad();
    }

    private void MakeUVCoords()
    {
        var width = texture.width / row;
        var height = texture.height / column;
        for (int i = 1; i <= row; i++)
        {
            for (int j = 1; j <= column; j++)
            {
                var uvCoords = new UVCoords()
                {
                    UV00 = new Vector2((width * (i - 1)) / (float)texture.width, (height * (j - 1)) / (float)texture.height),
                    UV11 = new Vector2((width * i) / (float)texture.width, (height * j) / (float)texture.height)
                };
                uvCoordsList.Add(uvCoords);
            }
        }
    }

    public int AddQuad(Particle particle)
    {
        
        if(quadIndex >= MAX_QUADS)
            return -1;
        particleList.Add(particle);
        var spawnedQuadIndex = quadIndex;
        quadIndex++;

        return spawnedQuadIndex;
    }

    public void UpdateQuad()
    {
        foreach (var particle in particleList)
        {
            var vIndex = particle.quadIndex * 4;
            var vIndex0 = vIndex;
            var vIndex1 = vIndex + 1;
            var vIndex2 = vIndex + 2;
            var vIndex3 = vIndex + 3;

            if (particle.skewed)
            {
                vertices[vIndex0] = particle.position + Quaternion.Euler(0, 0, particle.rotation) * new Vector3(-particle.quadSize.x, -particle.quadSize.y);
                vertices[vIndex1] = particle.position + Quaternion.Euler(0, 0, particle.rotation) * new Vector3(-particle.quadSize.x, +particle.quadSize.y);
                vertices[vIndex2] = particle.position + Quaternion.Euler(0, 0, particle.rotation) * new Vector3(+particle.quadSize.x, +particle.quadSize.y);
                vertices[vIndex3] = particle.position + Quaternion.Euler(0, 0, particle.rotation) * new Vector3(+particle.quadSize.x, -particle.quadSize.y);
            }
            else
            {
                vertices[vIndex0] = particle.position + Quaternion.Euler(0, 0, particle.rotation - 180) * particle.quadSize;
                vertices[vIndex1] = particle.position + Quaternion.Euler(0, 0, particle.rotation - 270) * particle.quadSize;
                vertices[vIndex2] = particle.position + Quaternion.Euler(0, 0, particle.rotation - 0) * particle.quadSize;
                vertices[vIndex3] = particle.position + Quaternion.Euler(0, 0, particle.rotation - 90) * particle.quadSize;
            }
        
            var uvCoords = uvCoordsList[particle.uvIndex];
            uvs[vIndex0] = uvCoords.UV00;
            uvs[vIndex1] = new Vector2(uvCoords.UV00.x, uvCoords.UV11.y);
            uvs[vIndex2] = uvCoords.UV11;
            uvs[vIndex3] = new Vector2(uvCoords.UV11.x, uvCoords.UV00.y);
        
            int tIndex = particle.quadIndex * 6;
        
            triangles[tIndex + 0] = vIndex0;
            triangles[tIndex + 1] = vIndex1;
            triangles[tIndex + 2] = vIndex2;
        
            triangles[tIndex + 3] = vIndex0;
            triangles[tIndex + 4] = vIndex2;
            triangles[tIndex + 5] = vIndex3;
        
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
        }
    }
    
}
