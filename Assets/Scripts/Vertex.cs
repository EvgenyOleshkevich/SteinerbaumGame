using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
	public SquareField field { get; set; }
	public List<Edge> edges;

	public enum Status { disabled = 0, enabled, selected };
	public Status staus { get; private set; } = Status.enabled;
	public Color defaultColor;
	public Color currentColor;
	// Start is called before the first frame update
	void Start()
	{
		defaultColor = GetComponent<Renderer>().material.color;
		currentColor = defaultColor;
	}

	void OnMouseEnter()
	{
		GetComponent<Renderer>().material.color = Color.Lerp(Color.black, currentColor, 0.7f);
		foreach (Edge edge in edges)
			edge.Enter();
	}

	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = currentColor;
		foreach (Edge edge in edges)
			edge.Exit();
	}

	void OnMouseDown()
	{
		if (field.mode == SquareField.Mode.selectFigure)
		{
			SetStatus(staus == Status.enabled ? Status.disabled : Status.enabled);
		}
	}

	public void Enter()
	{
		GetComponent<Renderer>().material.color = Color.Lerp(Color.black, currentColor, 0.7f);
	}

	public void Exit()
	{
		GetComponent<Renderer>().material.color = currentColor;
	}

	public void SetStatus(Status _status)
	{
		if (staus == _status)
			return;
		staus = _status;
		if (staus == Status.enabled)
		{
			if (CountNeiborVertex() > 0)
			{
				currentColor = defaultColor;
				foreach (Edge edge in edges)
					if (edge.CouldBeEnabled())
						edge.SetStatus(Edge.Status.enabled);
			}
			else
			{
				staus = Status.disabled;
				return;
			}
		}
		else if (staus == Status.disabled)
		{
			currentColor = Color.gray;
			foreach (Edge edge in edges)
				if (edge.enabled)
					edge.SetStatus(Edge.Status.disabled);
		}
		GetComponent<Renderer>().material.color = currentColor;
	}

	public int CountEdges()
	{
		int count = 0;
		foreach (Edge edge in edges)
			if (edge.staus == Edge.Status.enabled)
				++count;

		return count;
	}

	public int CountNeiborVertex()
	{
		int count = 0;

		foreach (Edge edge in edges)
			if (edge.vertex1.staus == Status.enabled &&
				edge.vertex2.staus == Status.enabled)
				++count;
		return count;
	}

}
