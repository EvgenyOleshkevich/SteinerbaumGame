using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareField : MonoBehaviour
{
    public Vertex vertexPrefab;
    public Edge edgePrefab;
    private Vertex[][] allVertexes;
    private Edge[][] allEdges;
    private int size;
    private readonly int minSize = 3;
    private readonly int maxSize = 30;
    public enum Mode { none = 0, selectSize, selectFigure, selectVertex, play };
    public Mode mode { get; set; } = Mode.none;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        GenerateField();
        mode = Mode.selectFigure;
    }

    private void GenerateField()
	{
        allVertexes = new Vertex[maxSize - minSize + 1][];
        allEdges = new Edge[maxSize - minSize + 1][];
        for (int i = minSize; i <= maxSize; ++i)
		{
            GenerateField(i);
            SetActive(i, false);
        }
        size = minSize;
        
        SetActive(size, true);
    }

    private void GenerateField(int size)
    {
        
        int index = size - minSize;
        allVertexes[index] = new Vertex[size * size];
        var vertexes = allVertexes[index];
        allEdges[index] = new Edge[size * (size - 1) * 2];
        var edges = allEdges[index];
        float shift = 1f / (size * 2) - 0.5f;
        float edgeShift = 1f / (size) - 0.5f;
        float height = -0.2f;
        float vertexScale = 1f / (size * 3);
        float edgeScaleX = 1f / size;
        float edgeScaleY = vertexScale / 2 ;

        for (int i = 0; i < vertexes.Length; ++i)
		{
            float x = i % size;
            float y = i / size;
            vertexes[i] = Instantiate(vertexPrefab);
            vertexes[i].field = this;
            vertexes[i].transform.position = new Vector3(shift + x / size, shift + y / size, height);
            vertexes[i].transform.localScale = new Vector3(vertexScale, vertexScale, vertexScale);
            vertexes[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < edges.Length / 2; ++i)
        {
            float x = i % (size - 1);
            float y = i / (size - 1);
            edges[i] = Instantiate(edgePrefab);
            edges[i].field = this;
            edges[i].vertex1 = vertexes[i + i / (size - 1)];
            edges[i].vertex2 = vertexes[i + i / (size - 1) + 1];
            vertexes[i + i / (size - 1)].rightEdge = edges[i];
            vertexes[i + i / (size - 1) + 1].leftEdge = edges[i];

            edges[i].transform.position = new Vector3(edgeShift + x / size, shift + y / size, height);
            edges[i].transform.localScale = new Vector3(edgeScaleX, edgeScaleY, vertexScale);
            edges[i].gameObject.SetActive(false);
        }

        for (int i = edges.Length / 2; i < edges.Length; ++i)
        {
            int schiftIndex = i - edges.Length / 2;
            float x = i % (size);
            float y = (schiftIndex) / (size);
            edges[i] = Instantiate(edgePrefab);
            edges[i].field = this;
            edges[i].vertex1 = vertexes[schiftIndex];
            edges[i].vertex2 = vertexes[schiftIndex + size];
            vertexes[schiftIndex].upperEdge = edges[i];
            vertexes[schiftIndex + size].bottomEdge = edges[i];

            edges[i].transform.position = new Vector3(shift + x / size, edgeShift + y / size, height);
            edges[i].transform.eulerAngles = new Vector3(0, 0, 90);
            edges[i].transform.localScale = new Vector3(edgeScaleX, edgeScaleY, vertexScale);
            edges[i].gameObject.SetActive(false);
        }

    }

    private void SetActive(int size, bool value)
	{
        int index = size - minSize;
		for (int i = 0; i < allEdges[index].Length; ++i)
			allEdges[index][i].gameObject.SetActive(value);

		for (int i = 0; i < allVertexes[index].Length; ++i)
            allVertexes[index][i].gameObject.SetActive(value);
    }

    public void SetSize(int newSize)
	{
        SetActive(size, false);
        SetActive(newSize, true);
        size = newSize;
    }
}
