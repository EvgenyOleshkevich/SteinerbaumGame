using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorMenu : MonoBehaviour
{
    public Slider sizeSlider;
    public Button submitSize;
    public Button nextToEdit;
    public Button backToSelectSize;
    public Button nextToSelect;
    public Button backToEdit;
    public Button play;
    public Button save;
    public Button exit;
    public Button load;
    public TextMeshProUGUI log;
    public SquareField field;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(field);
        field = GameObject.FindGameObjectWithTag("Field").GetComponent<SquareField>();
        backToSelectSize.gameObject.SetActive(false);
        nextToSelect.gameObject.SetActive(false);
        backToEdit.gameObject.SetActive(false);
        play.gameObject.SetActive(false);
        save.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSlider()
	{
        submitSize.GetComponentInChildren<TextMeshProUGUI>().text = "submit size " + sizeSlider.value;
    }
}
