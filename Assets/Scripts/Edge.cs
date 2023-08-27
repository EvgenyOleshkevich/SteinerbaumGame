using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
	public SquareField field { get; set; }
	public Vertex vertex1 { get; set; }
    public Vertex vertex2 { get; set; }

	public bool enabled { get; private set; } = true;
	public enum Status { disabled = 0, enabled, selected};
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
			if (CouldBeEnabled())
				currentColor = defaultColor;
			else
			{
				staus = Status.disabled;
				return;
			}
		}
		else if (staus == Status.disabled)
		{
			currentColor = Color.gray;
			if (vertex1.CountEdges() == 0)
				vertex1.SetStatus(Vertex.Status.disabled);
			if (vertex2.CountEdges() == 0)
				vertex2.SetStatus(Vertex.Status.disabled);
		}
			
		GetComponent<Renderer>().material.color = currentColor;
	}

	public bool CouldBeEnabled()
	{
		return vertex1.staus == Vertex.Status.enabled && vertex2.staus == Vertex.Status.enabled;
	}
}
