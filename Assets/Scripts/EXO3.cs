using System.IO;
using UnityEngine;

public class EXO3 : MonoBehaviour
{
    public EXO1 ex;
    void Start()
    {
        string[] content = File.ReadAllLines(Application.dataPath + ex.path);
        MeshFilter meshfilter = GetComponent<MeshFilter>();
        meshfilter.mesh = ex.meshFromOff(content);
    }

}
