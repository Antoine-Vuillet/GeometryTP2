using System.IO;
using System.Globalization;
using UnityEngine;
using System.Collections.Generic;

public class EXO1 : MonoBehaviour
{
    [SerializeField]public string path;
    [SerializeField] public List<Vector3> normals;

    void Start()
    {
        string[] content = File.ReadAllLines(Application.dataPath+ path);
        MeshFilter meshfilter = GetComponent<MeshFilter>();
        Mesh newMesh = meshFromOff(content);
        meshfilter.mesh = Instantiate(newMesh);
        normals = calculateNormals2(meshfilter.mesh);
        meshfilter.mesh.normals = normals.ToArray();
        meshfilter.mesh = removeFaces(meshfilter.mesh, new List<int> { 0, 1, 2 });
        ExportWrite(meshfilter.mesh, Application.dataPath + "/ExportedMesh.off");
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


    public List<Vector3> calculateNormals(Mesh mesh)
    {
        normals = new List<Vector3>(new Vector3[mesh.vertexCount]);
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 v0 = mesh.vertices[mesh.triangles[i]];
            Vector3 v1 = mesh.vertices[mesh.triangles[i + 1]];
            normals[mesh.triangles[i]] += Vector3.Cross(v1 - v0, mesh.vertices[mesh.triangles[i + 2]] - v0).normalized;
        }
        return normals;
    }


    //this version required the help of AI
    public List<Vector3> calculateNormals2(Mesh mesh)
    {
        var verts = mesh.vertices;
        var tris = mesh.triangles;
        var normals = new List<Vector3>(new Vector3[verts.Length]);

        for (int i = 0; i < tris.Length; i += 3)
        {
            int i0 = tris[i];
            int i1 = tris[i + 1];
            int i2 = tris[i + 2];

            Vector3 v0 = verts[i0];
            Vector3 v1 = verts[i1];
            Vector3 v2 = verts[i2];

            Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

            // Add the face normal to all three vertices
            normals[i0] += normal;
            normals[i1] += normal;
            normals[i2] += normal;
        }

        // Normalize all accumulated vertex normals
        for (int i = 0; i < normals.Count; i++)
        {
            normals[i] = normals[i].normalized;
        }

        return normals;
    }

    public List<Vector3> calculateFaceNormals(Mesh mesh)
    { 
        var faceNormals = new List<Vector3>(mesh.triangles.Length / 3);
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 v0 = mesh.vertices[mesh.triangles[i]];
            Vector3 v1 = mesh.vertices[mesh.triangles[i + 1]];
            Vector3 v2 = mesh.vertices[mesh.triangles[i + 2]];
            Vector3 n = Vector3.Cross(v1 - v0, v2 - v0);
            faceNormals.Add(n);
        }
        return faceNormals;
    }

    public Mesh removeFaces(Mesh mesh, List<int> facesToRemove)
    {
        List<int> triangles = new List<int>(mesh.triangles);
        facesToRemove.Sort();
        facesToRemove.Reverse();
        foreach (int faceIndex in facesToRemove)
        {
            triangles.RemoveRange(faceIndex * 3, 3);
        }
        mesh.triangles = triangles.ToArray();
        return mesh;
    }

    public void ExportToOff(Mesh mesh, string filepath)
    {
        using (StreamWriter sw = new StreamWriter(append: false, path: filepath))
        {
            sw.WriteLine("OFF");
            sw.WriteLine(mesh.vertexCount + " " + (mesh.triangles.Length / 3) + " 0");
            foreach (Vector3 v in mesh.vertices)
            {
                sw.WriteLine(v.x.ToString(CultureInfo.InvariantCulture) + " " + v.y.ToString(CultureInfo.InvariantCulture) + " " + v.z.ToString(CultureInfo.InvariantCulture));
            }
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                sw.WriteLine("3 " + mesh.triangles[i] + " " + mesh.triangles[i + 1] + " " + mesh.triangles[i + 2]);
            }
        }
    }

    public void ExportWrite(Mesh mesh, string filepath)
    {
        string output = "OFF\n";
        output += mesh.vertexCount + " " + (mesh.triangles.Length / 3) + " 0\n";
        foreach(Vector3 v in mesh.vertices)
        {
            output += v.x.ToString(CultureInfo.InvariantCulture) + " " + v.y.ToString(CultureInfo.InvariantCulture) + " " + v.z.ToString(CultureInfo.InvariantCulture) + "\n";
        }
        for(int i = 0; i < mesh.triangles.Length; i += 3)
        {
            output += "3 " + mesh.triangles[i] + " " + mesh.triangles[i + 1] + " " + mesh.triangles[i + 2] + "\n";
        }
        File.WriteAllText(filepath, output);
    }
}
