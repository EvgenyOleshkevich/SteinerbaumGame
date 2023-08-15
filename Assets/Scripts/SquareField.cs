using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareField : MonoBehaviour
{
    public GameObject vertexPrefab;
    public GameObject edgePrefab;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
