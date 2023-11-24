using Nova;
using System.Collections.Generic;
using UnityEngine;

public class UIGraphMeshNova : MonoBehaviour
{

    [SerializeField] private List<Vector3> vertices;
    [SerializeField] private List<Vector2> uvs;
    [SerializeField] private List<int> triangles;
    [SerializeField] private Mesh mesh;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] UIBlock graphContainer;

    void MakeGraph()
    {

        mesh = new Mesh();

        bool flip = true;

        for (int i = 0; i < vertices.Count - 2; i++)
        {

            if (flip)
            {

                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(i + 2);

                flip = !flip;

            }
            else
            {

                triangles.Add(i + 1);
                triangles.Add(i);
                triangles.Add(i + 2);

                flip = !flip;

            }

        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        meshFilter.sharedMesh = mesh;

    }

    public void GetVertices()
    {

        vertices.Clear();
        triangles.Clear();
        uvs.Clear();

        float uvX = 1 / this.gameObject.GetComponent<UIBlock>().Size.X.Value;
        float uvY = 1 / this.gameObject.GetComponent<UIBlock>().Size.Y.Value;

        vertices.Add(Vector3.zero);
        uvs.Add(Vector3.zero);

        for (int i = 0; i < graphContainer.gameObject.transform.childCount; i++)
        {

            if (graphContainer.transform.GetChild(i).transform.GetChild(0).GetComponent<UIBlock2D>().Visible == true)
            {

                vertices.Add(new Vector3(graphContainer.gameObject.transform.GetChild(i).GetComponent<UIBlock>().Position.X.Value, graphContainer.gameObject.transform.GetChild(i).GetComponent<UIBlock>().Position.Y.Value));
                uvs.Add(new Vector2(graphContainer.gameObject.transform.GetChild(i).GetComponent<UIBlock>().Position.X.Value * uvX, graphContainer.gameObject.transform.GetChild(i).GetComponent<UIBlock>().Position.Y.Value * uvY));
                vertices.Add(new Vector3(graphContainer.gameObject.transform.GetChild(i).GetComponent<UIBlock>().Position.X.Value, 0));
                uvs.Add(new Vector2(graphContainer.gameObject.transform.GetChild(i).GetComponent<UIBlock>().Position.X.Value * uvX, 0));

            }

        }

        vertices.Add(new Vector3(this.gameObject.GetComponent<UIBlock>().Size.X.Value, 0));
        uvs.Add(new Vector2(1, 0));

        MakeGraph();

    }
}
