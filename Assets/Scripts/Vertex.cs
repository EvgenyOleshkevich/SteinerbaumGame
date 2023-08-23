using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
	public SquareField field { get; set; }
	public Edge upperEdge { get; set; }
	public Edge bottomEdge { get; set; }
	public Edge leftEdge { get; set; }
	public Edge rightEdge { get; set; }

	public bool enabled { get; private set; } = true;
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
/*		if (upperEdge)
			upperEdge.Enter();
		if (bottomEdge)
			bottomEdge.Enter();
		if (leftEdge)
			leftEdge.Enter();
		if (rightEdge)
			rightEdge.Enter();*/
	}

	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = currentColor;
/*		if (upperEdge)
			upperEdge.Exit();
		if (bottomEdge)
			bottomEdge.Exit();
		if (leftEdge)
			leftEdge.Exit();
		if (rightEdge)
			rightEdge.Exit();*/
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
			Debug.Log(CountNeiborvertex());
			if (CountNeiborvertex() > 0)
			{
				currentColor = defaultColor;
				if (upperEdge && upperEdge.CouldBeEnabled())
					upperEdge.SetEnabled(enabled);
				if (bottomEdge && bottomEdge.CouldBeEnabled())
					bottomEdge.SetEnabled(enabled);
				if (leftEdge && leftEdge.CouldBeEnabled())
					leftEdge.SetEnabled(enabled);
				if (rightEdge && rightEdge.CouldBeEnabled())
					rightEdge.SetEnabled(enabled);
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
			if (upperEdge && upperEdge.enabled)
				upperEdge.SetEnabled(enabled);
			if (bottomEdge && bottomEdge.enabled)
				bottomEdge.SetEnabled(enabled);
			if (leftEdge && leftEdge.enabled)
				leftEdge.SetEnabled(enabled);
			if (rightEdge && rightEdge.enabled)
				rightEdge.SetEnabled(enabled);
		}
		GetComponent<Renderer>().material.color = currentColor;
	}

	public int CountEdges()
	{
		int count = 0;
		if (upperEdge && upperEdge.enabled)
			++count;
		if (bottomEdge && bottomEdge.enabled)
			++count;
		if (leftEdge && leftEdge.enabled)
			++count;
		if (rightEdge && rightEdge.enabled)
			++count;
		return count;
	}

	public int CountNeiborvertex()
	{
		int count = 0;
		if (upperEdge &&
			upperEdge.vertex1.enabled &&
			upperEdge.vertex2.enabled)
			++count;
		if (bottomEdge &&
			bottomEdge.vertex1.enabled &&
			bottomEdge.vertex2.enabled)
			++count;
		if (leftEdge &&
			leftEdge.vertex1.enabled &&
			leftEdge.vertex2.enabled)
			++count;
		if (rightEdge &&
			rightEdge.vertex1.enabled &&
			rightEdge.vertex2.enabled)
			++count;
		return count;
	}

}
