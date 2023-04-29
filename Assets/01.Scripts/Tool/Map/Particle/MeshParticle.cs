using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using DG.DemiLib;
using Unity.VisualScripting;
using UnityEngine;
using ParticleExtension;
using Unity.VisualScripting.Dependencies.NCalc;
using Random = UnityEngine.Random;

public class MeshParticle : MonoBehaviour
{
    private static MeshParticle _instance;
    public static MeshParticle Instance => _instance;
    
    private const int MAX_QUADS = 1000;

    [System.Serializable]
    public struct UVCoords
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

    public class ParticleMeshData
    {
        public string key;
        public List<Particle> Particles = new();
        public List<UVCoords> UVCoordsList = new();
        public Mesh Mesh;
        public MeshRenderer MeshRenderer;

        public Texture Texture
        {
            get => MeshRenderer.material.mainTexture;
            set
            {
                MeshRenderer.material.mainTexture = value;
               this.MakeUVCoords(key);
            }
        }
        public Vector3[] Vertices;
        public Vector2[] Uvs;
        public int[] Triangles;
        public int Row = 1;
        public int Column = 1;
    }

    public struct ParticleRandomProperties
    {
        public Vector3 minPosition;
        public Vector3 maxPosition;
        public float minRotation;
        public float maxRotation;
        public Vector3 minQuadSize;
        public Vector3 maxQuadSize;

        public Vector3 RandomPos => new Vector3(
            UnityEngine.Random.Range(minPosition.x, maxPosition.x),
            UnityEngine.Random.Range(minPosition.y, maxPosition.y),
            UnityEngine.Random.Range(minPosition.z, maxPosition.z));

        public float RandomRot => UnityEngine.Random.Range(minRotation, maxRotation);

        public Vector3 RandomQuadSize => new Vector3(
            UnityEngine.Random.Range(minQuadSize.x, maxQuadSize.x),
            UnityEngine.Random.Range(minQuadSize.y, maxQuadSize.y),
            UnityEngine.Random.Range(minQuadSize.z, maxQuadSize.z));
    }
    
    [Serializable]
    public class ParticleMeshTextureData
    {
        public string Key;
        public Texture Texture;
        public Color Color;
        public int Row;
        public int Column;
    }

    private Dictionary<string, ParticleMeshData> dataList = new();
    
    [SerializeField] private Material material;
    [SerializeField] private List<ParticleMeshTextureData> textureDataList = new();

    private void Start()
    {
        _instance = this;
        foreach (var textureData in textureDataList)
        {
            AddMesh(textureData.Key);
            SetTexture(textureData.Key, textureData.Texture, textureData.Row, textureData.Column);
            SetColors(textureData.Key, textureData.Color);
        }
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     var particle = new MeshParticle.Particle();
        //     particle.randomProperties = new MeshParticle.ParticleRandomProperties()
        //     {
        //         minPosition = new Vector3(-0.3f, -0.3f),
        //         maxPosition = new Vector3(0.3f, 0.3f),
        //         minRotation = 0,
        //         maxRotation = 360,
        //         minQuadSize = new Vector3(1f, 1f),
        //         maxQuadSize = new Vector3(1f, 1f),
        //     };
        //     var pos = InGame.Player.transform.position;
        //     particle.position = new Vector3(pos.x, pos.z) + particle.randomProperties.RandomPos;
        //     particle.rotation = particle.randomProperties.RandomRot;
        //     particle.quadSize = particle.randomProperties.RandomQuadSize;
        //     particle.skewed = true;
        //     particle.uvIndex = Random.Range(0, 4);
        //     AddParticle("Flash", particle);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Backspace))
        //     {
        //         var particle = new MeshParticle.Particle();
        //         particle.randomProperties = new MeshParticle.ParticleRandomProperties()
        //         {
        //             minPosition = new Vector3(-0.3f, -0.3f),
        //             maxPosition = new Vector3(0.3f, 0.3f),
        //             minRotation = 0,
        //             maxRotation = 360,
        //             minQuadSize = new Vector3(1f, 1f),
        //             maxQuadSize = new Vector3(1f, 1f),
        //         };
        //         var pos = transform.position;
        //         particle.position = new Vector3(pos.x, pos.z) + particle.randomProperties.RandomPos;
        //         particle.rotation = particle.randomProperties.RandomRot;
        //         particle.quadSize = particle.randomProperties.RandomQuadSize;
        //         particle.skewed = true;
        //         particle.uvIndex = Random.Range(0, 2);
        //         AddParticle("Scratch", particle);
        //     }
        UpdateQuad();
    }

    public void AddMesh(string key)
    {
        var obj = new GameObject(key);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        var meshFilter = obj.AddComponent<MeshFilter>();
        var meshRenderer = obj.AddComponent<MeshRenderer>();
        var mesh = new Mesh();
        meshFilter.mesh = mesh;
        meshRenderer.material = material;
        var data = new ParticleMeshData
        {
            key = key,
            Mesh = mesh,
            MeshRenderer = meshRenderer,
            Vertices = new Vector3[MAX_QUADS * 4],
            Uvs = new Vector2[MAX_QUADS * 4],
            Triangles = new int[MAX_QUADS * 6]
        };

        dataList.Add(key, data);
    }

    public void SetTexture(string key, Texture texture, int row, int column)
    {
        dataList[key].Row = row;
        dataList[key].Column = column;
        dataList[key].Texture = texture;
    }
    
    public void SetColors(string textureDataKey, Color textureDataColor)
    {
        dataList[textureDataKey].MeshRenderer.material.SetColor("_Color", textureDataColor);
    }
    
    public void AddParticle(string key, Particle particle)
    {
        particle.quadIndex = dataList[key].Particles.Count;
        dataList[key].Particles.Add(particle);
    }

    public void UpdateQuad()
    {
        foreach (var pData in dataList.Values)
        {
            foreach (var particle in pData.Particles)
            {
                var vIndex = particle.quadIndex * 4;
                var vIndex0 = vIndex;
                var vIndex1 = vIndex + 1;
                var vIndex2 = vIndex + 2;
                var vIndex3 = vIndex + 3;

                if (particle.skewed)
                {
                    pData.Vertices[vIndex0] = particle.position + Quaternion.Euler(0, 0, 90 - particle.rotation) *
                        new Vector3(-particle.quadSize.x, -particle.quadSize.y);
                    pData.Vertices[vIndex1] = particle.position + Quaternion.Euler(0, 0, 90 - particle.rotation) *
                        new Vector3(-particle.quadSize.x, +particle.quadSize.y);
                    pData.Vertices[vIndex2] = particle.position + Quaternion.Euler(0, 0, 90 - particle.rotation) *
                        new Vector3(+particle.quadSize.x, +particle.quadSize.y);
                    pData.Vertices[vIndex3] = particle.position + Quaternion.Euler(0, 0, 90 - particle.rotation) *
                        new Vector3(+particle.quadSize.x, -particle.quadSize.y);
                }
                else
                {
                    pData.Vertices[vIndex0] = particle.position +
                                              Quaternion.Euler(0, 0, 90 - particle.rotation - 180) * particle.quadSize;
                    pData.Vertices[vIndex1] = particle.position +
                                              Quaternion.Euler(0, 0, 90 - particle.rotation - 270) * particle.quadSize;
                    pData.Vertices[vIndex2] = particle.position +
                                              Quaternion.Euler(0, 0, 90 - particle.rotation - 0) * particle.quadSize;
                    pData.Vertices[vIndex3] = particle.position +
                                              Quaternion.Euler(0, 0, 90 - particle.rotation - 90) * particle.quadSize;
                }

                var uvCoords = pData.UVCoordsList[particle.uvIndex];
                pData.Uvs[vIndex0] = uvCoords.UV00;
                pData.Uvs[vIndex1] = new Vector2(uvCoords.UV00.x, uvCoords.UV11.y);
                pData.Uvs[vIndex2] = uvCoords.UV11;
                pData.Uvs[vIndex3] = new Vector2(uvCoords.UV11.x, uvCoords.UV00.y);

                int tIndex = particle.quadIndex * 6;

                pData.Triangles[tIndex + 0] = vIndex0;
                pData.Triangles[tIndex + 1] = vIndex1;
                pData.Triangles[tIndex + 2] = vIndex2;

                pData.Triangles[tIndex + 3] = vIndex0;
                pData.Triangles[tIndex + 4] = vIndex2;
                pData.Triangles[tIndex + 5] = vIndex3;

                pData.Mesh.vertices = pData.Vertices;
                pData.Mesh.uv = pData.Uvs;
                pData.Mesh.triangles = pData.Triangles;
            }
        }
    }
}

namespace ParticleExtension
{
    using static MeshParticle;
    public static class ParticleMethodClass
    {
        public static void MakeUVCoords(this ParticleMeshData data, string key)
        {
            var width = data.Texture.width / data.Row;
            var height = data.Texture.height / data.Column;
            for (int i = 1; i <= data.Row; i++)
            {
                for (int j = 1; j <= data.Column; j++)
                {
                    var uvCoords = new UVCoords()
                    {
                        UV00 = new Vector2((width * (i - 1)) / (float)data.Texture.width, (height * (j - 1)) / (float)data.Texture.height),
                        UV11 = new Vector2((width * i) / (float)data.Texture.width, (height * j) / (float)data.Texture.height)
                    };
                    data.UVCoordsList.Add(uvCoords);
                }
            }
        }
    }
}