using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
	public SquareField field { get; set; }
	public Vertex vertex1 { get; set; }
    public Vertex vertex2 { get; set; }

	public enum Status { disabled = 0, enabled, selected};
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
		vertex1.Enter();
		vertex2.Enter();
	}

	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = currentColor;
		vertex1.Exit();
		vertex2.Exit();
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
			if (CouldBeEnabled())
				currentColor = defaultColor;
			else
			{
				status = Status.disabled;
				return;
			}
		}
		else if (status == Status.disabled)
		{
			var rollbackVertex = new List<Vertex>();

			if (vertex1.CountEdges() == 0)
			{
				vertex1.status = Vertex.Status.disabled;
				rollbackVertex.Add(vertex1);
			}
			if (vertex2.CountEdges() == 0)
			{
				vertex2.status = Vertex.Status.disabled;
				rollbackVertex.Add(vertex2);
			}
			if (field.IsConnected())
			{
				foreach (Vertex vertex in rollbackVertex)
					vertex.ForceDisable();
			}
			else
			{
				status = Status.enabled;
				foreach (Vertex vertex in rollbackVertex)
					vertex.status = Vertex.Status.enabled;
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

	public bool CouldBeEnabled()
	{
		return vertex1.status == Vertex.Status.enabled && vertex2.status == Vertex.Status.enabled;
	}
}
