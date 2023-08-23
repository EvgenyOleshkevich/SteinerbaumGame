using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public Vertex vertex1 { get; set; }
    public Vertex vertex2 { get; set; }

	private Color color;
    // Start is called before the first frame update
    void Start()
    {
		color = GetComponent<Renderer>().material.color;
	}

	void OnMouseEnter()
	{
		GetComponent<Renderer>().material.color = Color.Lerp(Color.black, color, 0.7f);
	}

	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = color;
	}

	void OnMouseDown()
	{
	}
}
