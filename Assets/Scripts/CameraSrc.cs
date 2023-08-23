using System;
using UnityEngine;

public class CameraSrc : MonoBehaviour
{
	public SquareField field { get; set; }
	private double fi { get; set; }
	private double psi { get; set; }
	private double radius { get; set; }
	private float zoom = 1;
	private float posMax = 0.5f;
	private float speed = 0.05f;
	private bool drag = false;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			drag = true;
		}
		if (Input.GetMouseButtonUp(1))
		{
			drag = false;
		}
		if (drag)
		{
			float x = -Input.GetAxis("Mouse X") * speed * GetComponent<Camera>().orthographicSize;
			float y = -Input.GetAxis("Mouse Y") * speed * GetComponent<Camera>().orthographicSize;
			transform.position = new Vector3(Math.Max(-posMax, Math.Min(posMax, transform.position.x + x)),
				Math.Max(-posMax, Math.Min(posMax, transform.position.y + y)), -1);
		}
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
		if (mouseWheel > 0.1)
		{
			zoom = Math.Max(0.1f, zoom - 0.1f);
			GetComponent<Camera>().orthographicSize = 0.6f * zoom;
		}
		if (mouseWheel < -0.1)
		{
			zoom = Math.Min(1.5f, zoom + 0.1f);
			GetComponent<Camera>().orthographicSize = 0.6f * zoom;
		}
	}
}
