using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public int Demensions = 10;
    public Octave[] octaves;
    public float uvScale;

    protected MeshFilter meshFilter;
    protected Mesh mesh;


    [Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }

    void Start()
    {
        mesh = new Mesh();
        mesh.name = gameObject.name;
        mesh.vertices = GenerateVertex();
        mesh.triangles = GenerateTriangle();
        mesh.uv = GenerateUV();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private Vector2[] GenerateUV()
    {
        var uvs = new Vector2[mesh.vertices.Length];
        for (int x = 0; x <= Demensions; x++)
        {
            for (int z = 0; z <= Demensions; z++)
            {
                var vertex = new Vector2((x / uvScale) % 2, (z / uvScale) % 2);
                uvs[MakeIndex(x, z)] = new Vector2(vertex.x <= 1 ? vertex.x : 2 - vertex.x, vertex.y <= 1 ? vertex.y : 2 - vertex.y);
            }
        }

        return uvs;

    }

    private Vector3[] GenerateVertex()
    {
        var verts = new Vector3[(Demensions + 1) * (Demensions + 1)];
        for (int x = 0; x <= Demensions; x++)
        {
            for (int z = 0; z <= Demensions; z++)
            {
                verts[MakeIndex(x, z)] = new Vector3(x, 0, z);
            }
        }
        return verts;
    }

    private int MakeIndex(int x, int z)
    {
        return x * (Demensions + 1) + z;
    }

    private int[] GenerateTriangle()
    {
        var triangles = new int[mesh.vertices.Length * 6];

        for (int x = 0; x < Demensions; x++)
        {
            for (int z = 0; z < Demensions; z++)
            {
                triangles[MakeIndex(x, z) * 6 + 0] = MakeIndex(x, z);
                triangles[MakeIndex(x, z) * 6 + 1] = MakeIndex(x + 1, z + 1);
                triangles[MakeIndex(x, z) * 6 + 2] = MakeIndex(x + 1, z);
                triangles[MakeIndex(x, z) * 6 + 3] = MakeIndex(x, z);
                triangles[MakeIndex(x, z) * 6 + 4] = MakeIndex(x, z + 1);
                triangles[MakeIndex(x, z) * 6 + 5] = MakeIndex(x + 1, z + 1);
            }
        }

        return triangles;
    }


    // Update is called once per frame
    void Update()
    {
        var vertex = mesh.vertices;

        for (int x = 0; x <= Demensions; x++)
        {
            for (int z = 0; z <= Demensions; z++)
            {
                var y = 0.0f;

                for (int o = 0; o < octaves.Length; o++)
                {
                    if (octaves[o].alternate)
                    {
                        var perl = Mathf.PerlinNoise((x * octaves[o].scale.x) / Demensions, ((z * octaves[o].scale.y) / Demensions)) * Math.PI * 2.0f;
                        y += Mathf.Cos((float)perl + octaves[o].speed.magnitude * Time.time) * octaves[o].height;
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x * octaves[o].scale.x + Time.time * octaves[o].speed.x) / Demensions, (z * octaves[o].scale.y + Time.time * octaves[o].speed.y) / Demensions) - 0.5f;
                        y += perl * octaves[o].height;
                    }
                }

                vertex[MakeIndex(x, z)] = new Vector3(x, y, z);
            }

        }

        mesh.vertices = vertex;
        mesh.RecalculateNormals();
    }


    public float GetHeight(Vector3 pos)
    {
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localpos = Vector3.Scale((pos - transform.position), scale);

        var p1 = new Vector3(Mathf.Floor(localpos.x), 0, Mathf.Floor(localpos.z));
        var p2 = new Vector3(Mathf.Floor(localpos.x), 0, Mathf.Ceil(localpos.z));
        var p3 = new Vector3(Mathf.Ceil(localpos.x), 0, Mathf.Floor(localpos.z));
        var p4 = new Vector3(Mathf.Ceil(localpos.x), 0, Mathf.Ceil(localpos.z));

        p1.x = Mathf.Clamp(p1.x, 0, Demensions);
        p2.x = Mathf.Clamp(p2.x, 0, Demensions);
        p3.x = Mathf.Clamp(p3.x, 0, Demensions);
        p4.x = Mathf.Clamp(p4.x, 0, Demensions);
        p1.z = Mathf.Clamp(p1.z, 0, Demensions);
        p2.z = Mathf.Clamp(p2.z, 0, Demensions);
        p3.z = Mathf.Clamp(p3.z, 0, Demensions);
        p4.z = Mathf.Clamp(p4.z, 0, Demensions);

        var max = Mathf.Max(
            Vector3.Distance(p1, localpos),
            Vector3.Distance(p2, localpos),
            Vector3.Distance(p3, localpos),
            Vector3.Distance(p4, localpos) + Mathf.Epsilon);

        var dist = (max - Vector3.Distance(p1, localpos))
            + (max - Vector3.Distance(p2, localpos))
            + (max - Vector3.Distance(p3, localpos))
            + (max - Vector3.Distance(p4, localpos) + Mathf.Epsilon);

        var height =
            mesh.vertices[MakeIndex((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localpos)) +
            mesh.vertices[MakeIndex((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localpos)) +
            mesh.vertices[MakeIndex((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localpos)) +
            mesh.vertices[MakeIndex((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localpos));

        return height * transform.lossyScale.y / dist;
    }
}
