using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
	public SquareField field { get; set; }
	public List<Edge> edges;

	public enum Status { disabled = 0, enabled, selected };
	public Status status { get; set; } = Status.enabled;
	public Color disabledColor = Color.gray;
	public Color enabledColor;
	public Color selectedColor = Color.magenta;
	public Color currentColor;
	// Start is called before the first frame update
	void Start()
	{
		enabledColor = GetComponent<SpriteRenderer>().color;
		currentColor = enabledColor;
		disabledColor = Color.Lerp(disabledColor, enabledColor, 0.15f);
	}

	void OnMouseEnter()
	{
		GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, currentColor, 0.7f);
		foreach (Edge edge in edges)
			edge.Enter();
	}
	void OnMouseExit()
	{
		GetComponent<SpriteRenderer>().color = currentColor;
		foreach (Edge edge in edges)
			edge.Exit();
	}

	public void Enter()
	{
		GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, currentColor, 0.7f);
	}
	public void Exit()
	{
		GetComponent<SpriteRenderer>().color = currentColor;
	}

	void OnMouseDown()
	{
		if (field.mode == SquareField.Mode.selectFigure)
		{
			SetStatus(status == Status.enabled ? Status.disabled : Status.enabled);
		}
		if (field.mode == SquareField.Mode.selectVertex && status != Status.disabled)
		{
			SetStatus(status == Status.enabled ? Status.selected : Status.enabled);
		}
	}

	public void SetStatus(Status _status)
	{
		if (status == _status)
			return;
		if (status == Status.disabled && _status == Status.enabled)
		{
			Debug.Log(CountNeiborVertex());
			if (CountNeiborVertex() > 0)
			{
				
				status = Status.enabled;
				currentColor = enabledColor;
				foreach (Edge edge in edges)
					if (edge.CouldBeEnabled())
						edge.SetStatus(Edge.Status.enabled);
			}
			else
				return;
		}
		else if (status == Status.enabled && _status == Status.disabled)
		{
			status = _status;
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
			currentColor = disabledColor;
		}
		else if (status == Status.enabled && _status == Status.selected)
		{
			status = _status;
			currentColor = selectedColor;
		}
		else if (status == Status.selected && _status == Status.enabled)
		{
			status = _status;
			currentColor = enabledColor;
		}
		GetComponent<SpriteRenderer>().color = currentColor;
	}

	public void ForceDisable()
	{
		status = Status.disabled;
		currentColor = disabledColor;
		GetComponent<SpriteRenderer>().color = currentColor;
	}

	public void ForceEnable()
	{
		status = Status.enabled;
		currentColor = enabledColor;
		GetComponent<SpriteRenderer>().color = currentColor;
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
			if (edge.vertex1 != this && edge.vertex1.status == Status.enabled ||
				edge.vertex2 != this && edge.vertex2.status == Status.enabled)
				++count;
		return count;
	}

}
