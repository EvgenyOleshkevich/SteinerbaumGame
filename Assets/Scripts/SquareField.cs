using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

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
    public TextMeshProUGUI log { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        GenerateField();
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
            vertexes[i + i / (size - 1)].edges.Add(edges[i]);
            vertexes[i + i / (size - 1) + 1].edges.Add(edges[i]);

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
            vertexes[schiftIndex].edges.Add(edges[i]);
            vertexes[schiftIndex + size].edges.Add(edges[i]);

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

    public void SetMode(Mode _mode)
    {
        if (mode == Mode.selectFigure)
        {
            if (_mode == Mode.selectSize)
            {
                int index = size - minSize;
                for (int i = 0; i < allEdges[index].Length; ++i)
                    if (allEdges[index][i].status == Edge.Status.disabled)
                        allEdges[index][i].ForceEnable();

                for (int i = 0; i < allVertexes[index].Length; ++i)
                    if (allVertexes[index][i].status == Vertex.Status.disabled)
                        allVertexes[index][i].ForceEnable();
            }
        }
        else if (mode == Mode.selectVertex)
        {
            if (_mode == Mode.selectFigure)
            {
                int index = size - minSize;
                for (int i = 0; i < allVertexes[index].Length; ++i)
                    if (allVertexes[index][i].status == Vertex.Status.selected)
                        allVertexes[index][i].ForceEnable();
            }
            else if (_mode == Mode.play)
			{
                if (CountSelectedVertex() < 2)
				{
                    log.text = "at least 2 vertexes should be selected";
                    return;
                }
			}
        }
        else if (mode == Mode.play && _mode == Mode.selectSize)
		{
            int index = size - minSize;
            for (int i = 0; i < allEdges[index].Length; ++i)
            {
                allEdges[index][i].gameObject.SetActive(true);
                allEdges[index][i].ForceEnable();
            }

            for (int i = 0; i < allVertexes[index].Length; ++i)
            {
                allVertexes[index][i].gameObject.SetActive(true);
                allVertexes[index][i].ForceEnable();
            }
        }
        mode = _mode;
    }

    public void Save()
    {
        using (StreamWriter writer = new StreamWriter("game.txt"))
        {
            writer.WriteLine(size);

            int index = size - minSize;
            for (int i = 0; i < allVertexes[index].Length; ++i)
                writer.WriteLine(((int)allVertexes[index][i].status));

            for (int i = 0; i < allEdges[index].Length; ++i)
                writer.WriteLine(((int)allEdges[index][i].status));
        }
	}


    public void Load(string path)
    {
		var reader = new StreamReader(path);
		SetSize(int.Parse(reader.ReadLine()));

		int index = size - minSize;
		for (int i = 0; i < allVertexes[index].Length; ++i)
			allVertexes[index][i].ForceSetStatus((Vertex.Status)int.Parse(reader.ReadLine()));

		for (int i = 0; i < allEdges[index].Length; ++i)
			allEdges[index][i].ForceSetStatus((Edge.Status)int.Parse(reader.ReadLine()));
        mode = Mode.play;
    }

        public int CountSelectedVertex()
	{
        int index = size - minSize;
        int countSelected = 0;
        for (int i = 0; i < allVertexes[index].Length; ++i)
            if (allVertexes[index][i].status == Vertex.Status.selected)
                ++countSelected;
        return countSelected;
    }

    public int CountSelectedEdge()
    {
        int index = size - minSize;
        int countSelected = 0;
        for (int i = 0; i < allEdges[index].Length; ++i)
            if (allEdges[index][i].status == Edge.Status.selected)
                ++countSelected;
        return countSelected;
    }

    public bool IsConnected()
	{
        int countEnabled = 0;
        int index = size - minSize;
        Vertex start = null;
        for (int i = 0; i < allVertexes[index].Length; ++i)
            if (allVertexes[index][i].status == Vertex.Status.enabled)
                ++countEnabled;

        for (int i = 0; i < allVertexes[index].Length; ++i)
            if (allVertexes[index][i].status == Vertex.Status.enabled)
            {
                start = allVertexes[index][i];
                break;
            }
        if (countEnabled == 0)
		{
            log.text = "graph can not ba empty";
            return false;
        }
        int dfs_count = DFS(start, Vertex.Status.enabled, Edge.Status.enabled);
        bool res = dfs_count == countEnabled;
        if (!res)
            log.text = "graph should be connected";
        return res;
    }

    public bool IsSolved()
    {
        int countSelected = 0;
        int index = size - minSize;
        Vertex start = null;
        for (int i = 0; i < allVertexes[index].Length; ++i)
            if (allVertexes[index][i].status == Vertex.Status.selected)
                ++countSelected;

        for (int i = 0; i < allVertexes[index].Length; ++i)
            if (allVertexes[index][i].status == Vertex.Status.selected)
            {
                start = allVertexes[index][i];
                break;
            }

        if (countSelected == 0)
        {
            log.text = "graph can not ba empty";
            return false;
        }
        bool res = DFS(start, Vertex.Status.selected, Edge.Status.selected) == countSelected;
        log.text = "Length: " + CountSelectedEdge();
        if (res)
            log.text += "\ngraph is solved connected";
        return res;
    }

    private int DFS(Vertex start, Vertex.Status allowedVertex,  Edge.Status allowedEdge)
    {
        var visited = new HashSet<Vertex>();
        var vertexes = new Stack<Vertex>();
        vertexes.Push(start);
        visited.Add(start);
        int count = 0;

        while (vertexes.Count != 0)
		{
            var vertex = vertexes.Pop();
            if (vertex.status == allowedVertex)
                ++count;

            foreach (Edge edge in vertex.edges)
			{
                if (edge.status == allowedEdge)
                {
                    if (!visited.Contains(edge.vertex1))
                    {
                        vertexes.Push(edge.vertex1);
                        visited.Add(edge.vertex1);
                    }

                    if (!visited.Contains(edge.vertex2))
                    {
                        vertexes.Push(edge.vertex2);
                        visited.Add(edge.vertex2);
                    }
                }
            }
        }
        return count;
    }
}
