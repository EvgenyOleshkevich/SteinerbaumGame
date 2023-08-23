using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    public Edge upperEdge { get; set; }
    public Edge bottomEdge { get; set; }
    public Edge leftEdge { get; set; }
    public Edge rightEdge { get; set; }

	private Color color;
	// Start is called before the first frame update
	void Start()
	{
		color = GetComponent<Renderer>().material.color;
	}

	void OnMouseEnter()
	{
		Debug.Log("OnMouseEnter Vertex");
		GetComponent<Renderer>().material.color = Color.Lerp(Color.black, color, 0.7f);
	}

	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = color;
	}

	void OnMouseDown()
	{
		Debug.Log("OnMouseDown Vertex");
	}
}
