using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorMenu : MonoBehaviour
{
    public Slider sizeSlider;
    public Button submitSize;
    public Button backToSelectSize;
    public Button nextToSelect;
    public Button backToEdit;
    public Button play;
    public Button save;
    public Button exit;
    public Button load;
    public TextMeshProUGUI log;
    public SquareField field;
    public CameraSrc camera;

    // Start is called before the first frame update
    void Start()
    {
        field = Instantiate(field);
        camera.field = field;
        SetActive(0);
    }

    private void SetActive(int layer)
    {
        sizeSlider.gameObject.SetActive(layer == 0);
        submitSize.gameObject.SetActive(layer == 0);
        backToSelectSize.gameObject.SetActive(layer == 1);
        nextToSelect.gameObject.SetActive(layer == 1);
        backToEdit.gameObject.SetActive(layer == 2);
        play.gameObject.SetActive(layer == 2);
        save.gameObject.SetActive(layer == 2);
    }

    public void OnSlider()
	{
        submitSize.GetComponentInChildren<TextMeshProUGUI>().text = "submit size " + sizeSlider.value;
        field.SetSize((int)sizeSlider.value);
    }

    public void OnSubmitSize()
	{
        field.SetSize((int)sizeSlider.value);
        SetActive(1);
    }

    public void OnBackToSelectSize()
    {
        SetActive(0);
    }

    public void OnNextToSelect()
    {
        SetActive(2);
    }

    public void OnBackToEdit()
    {
        SetActive(1);
    }

    public void OnPlay()
	{

	}

    public void OnSave()
    {

    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void OnLoad()
    {

    }

}
