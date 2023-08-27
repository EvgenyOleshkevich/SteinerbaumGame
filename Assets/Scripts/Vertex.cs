using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
	public SquareField field { get; set; }
	public List<Edge> edges;

	public enum Status { disabled = 0, enabled, selected };
	public Status status { get; set; } = Status.enabled;
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
			SetStatus(status == Status.enabled ? Status.disabled : Status.enabled);
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
		if (status == _status)
			return;
		status = _status;
		if (status == Status.enabled)
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
				status = Status.disabled;
				return;
			}
		}
		else if (status == Status.disabled)
		{
			var rollbackEdge = new List<Edge>();
			var rollbackVertex = new List<Vertex>();
			foreach (Edge edge in edges)
				if (edge.status != Edge.Status.disabled)
				{
					edge.status = Edge.Status.disabled;
					rollbackEdge.Add(edge);
					if (edge.vertex1 != this && edge.vertex1.CountEdges() == 0)
					{
						edge.vertex1.status = Status.disabled;
						rollbackVertex.Add(edge.vertex1);
					}
					if (edge.vertex2 != this && edge.vertex2.CountEdges() == 0)
					{
						edge.vertex2.status = Status.disabled;
						rollbackVertex.Add(edge.vertex2);
					}
				}
			
			if (field.IsConnected())
			{
				foreach (Edge edge in rollbackEdge)
					edge.ForceDisable();
				foreach (Vertex vertex in rollbackVertex)
					vertex.ForceDisable();

			}
			else
			{
				status = Status.enabled;
				foreach (Edge edge in rollbackEdge)
					edge.status = Edge.Status.enabled;
				foreach (Vertex vertex in rollbackVertex)
					vertex.status = Status.enabled;
				return;
			}
			currentColor = Color.gray;
		}
		GetComponent<Renderer>().material.color = currentColor;
	}

	public void ForceDisable()
	{
		status = Status.disabled;
		currentColor = Color.gray;
		GetComponent<Renderer>().material.color = currentColor;
	}

	public int CountEdges()
	{
		int count = 0;
		foreach (Edge edge in edges)
			if (edge.status == Edge.Status.enabled)
				++count;

		return count;
	}

	public int CountNeiborVertex()
	{
		int count = 0;

		foreach (Edge edge in edges)
			if (edge.vertex1.status == Status.enabled &&
				edge.vertex2.status == Status.enabled)
				++count;
		return count;
	}

}
