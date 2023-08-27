using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
	public SquareField field { get; set; }
	public List<Edge> edges;

	public bool enabled { get; private set; } = true;
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
			SetEnabled(!enabled);
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

	public void SetEnabled(bool _enabled)
	{
		if (enabled == _enabled)
			return;
		enabled = _enabled;
		if (enabled)
		{
			if (CountNeiborvertex() > 0)
			{
				currentColor = defaultColor;
				foreach (Edge edge in edges)
					if (edge.CouldBeEnabled())
						edge.SetEnabled(enabled);
			}
			else
			{
				enabled = !enabled;
				return;
			}
		}
		else
		{
			currentColor = Color.gray;
			foreach (Edge edge in edges)
				if (edge.enabled)
					edge.SetEnabled(enabled);
		}
		GetComponent<Renderer>().material.color = currentColor;
	}

	public int CountEdges()
	{
		int count = 0;
		foreach (Edge edge in edges)
			if (edge.enabled)
				++count;

		return count;
	}

	public int CountNeiborvertex()
	{
		int count = 0;

		foreach (Edge edge in edges)
			if (edge &&
				edge.vertex1.enabled &&
				edge.vertex2.enabled)
				++count;
		return count;
	}

}
