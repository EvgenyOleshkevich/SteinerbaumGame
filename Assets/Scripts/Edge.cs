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
	public readonly Color disabledColor = Color.Lerp(Color.gray, Color.green, 0.15f);
	public readonly Color enabledColor = Color.green;
	public readonly Color selectedColor = Color.blue;
	public Color currentColor = Color.green;

	void OnMouseEnter()
	{
		GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, currentColor, 0.7f);
		vertex1.Enter();
		vertex2.Enter();
	}

	void OnMouseExit()
	{
		GetComponent<SpriteRenderer>().color = currentColor;
		vertex1.Exit();
		vertex2.Exit();
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
		if (field.mode == SquareField.Mode.play && status != Status.disabled)
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
			if (CouldBeEnabled())
			{
				currentColor = enabledColor;
				status = _status;
			}
			else
				return;
		}
		else if (status == Status.enabled && _status == Status.disabled)
		{
			var rollbackVertex = new List<Vertex>();
			status = _status;
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
			currentColor = disabledColor;
		}
		else if (status == Status.enabled && _status == Status.selected)
		{
			status = _status;
			currentColor = selectedColor;
			field.IsSolved();
		}
		else if (status == Status.selected && _status == Status.enabled)
		{
			status = _status;
			currentColor = enabledColor;
			field.IsSolved();
		}

		GetComponent<SpriteRenderer>().color = currentColor;
	}

	public void ForceEnable()
	{
		status = Status.enabled;
		currentColor = enabledColor;
		GetComponent<SpriteRenderer>().color = currentColor;
	}

	public void ForceDisable()
	{
		status = Status.disabled;
		currentColor = disabledColor;
		GetComponent<SpriteRenderer>().color = currentColor;
	}

	public void ForceSelect()
	{
		status = Status.selected;
		currentColor = selectedColor;
		GetComponent<SpriteRenderer>().color = currentColor;
	}

	public void ForceSetStatus(Status _status)
	{
		status = _status;
		if (status == Status.disabled)
			currentColor = disabledColor;
		else if (status == Status.enabled)
			currentColor = enabledColor;
		else if (status == Status.selected)
			currentColor = selectedColor;
		GetComponent<SpriteRenderer>().color = currentColor;
	}

	public bool CouldBeEnabled()
	{
		return vertex1.status == Vertex.Status.enabled && vertex2.status == Vertex.Status.enabled;
	}
}
