using System.IO;
using System.Globalization;
using UnityEngine;
using System.Collections.Generic;

public class EXO1 : MonoBehaviour
{
    [SerializeField]public string path;
    void Start()
    {
        string[] content = File.ReadAllLines(Application.dataPath+ path);
        MeshFilter meshfilter = GetComponent<MeshFilter>();
        meshfilter.mesh = meshFromOff(content);
    }

    public Mesh meshFromOff(string[] content)
    {
        Mesh mesh = new Mesh();
        int nbVertices = int.Parse(content[1].Split(' ')[0]);
        int nbFaces = int.Parse(content[1].Split(' ')[1]);
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        string[] line;
        for (int i = 2; i< nbVertices+2; i++)
        {
            line = content[i].Split(' ');
            print(line[0]);
            float x = float.Parse(line[0], CultureInfo.InvariantCulture);
            float y = float.Parse(line[1], CultureInfo.InvariantCulture);
            float z = float.Parse(line[2], CultureInfo.InvariantCulture);
            vertices.Add( new Vector3(x,y,z));
        }
        for (int i = 2 + nbVertices; i < nbFaces + nbVertices+2; i++)
        {
            line = content[i].Split(' ');
            triangles.Add(int.Parse(line[1]));
            triangles.Add(int.Parse(line[2]));
            triangles.Add(int.Parse(line[3]));
        }
        Vector3 center = getcenter(vertices);
        vertices = centerMesh(center, vertices);
        vertices = normalizeMesh(vertices);
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();     
        return mesh;
    }

    public Vector3 getcenter(List<Vector3> vertices)
    {
        Vector3 center = new Vector3(0,0,0);
        for (int i = 0; i < vertices.Count; i++)
        {
            center = center + vertices[i];
        }
        center = center/vertices.Count;
        return center;
    }

    public List<Vector3> centerMesh(Vector3 center, List<Vector3> vertices)
    {
        for (int i = 0; i< vertices.Count; i++)
        {
            vertices[i]= vertices[i] - center;
        }

        return vertices;
    }

    public List<Vector3> normalizeMesh(List<Vector3> vertices)
    {
        float max = 0;
        for(int i = 0; i < vertices.Count; i++)
        {
            if (Mathf.Abs(vertices[i].x) > max)
            {
                max = vertices[i].x;
            }
            if (Mathf.Abs(vertices[i].y) > max)
            {
                max = vertices[i].y;
            }
            if (Mathf.Abs(vertices[i].z) > max)
            {
                max = vertices[i].z;
            }
        }
        for(int i = 0; i < vertices.Count; i++)
        {
            vertices[i] =new Vector3(vertices[i].x / max, vertices[i].y / max, vertices[i].z / max) ;
        }
            return vertices;
    }
}
