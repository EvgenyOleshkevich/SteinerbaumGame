using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class EditorMenu : MonoBehaviour
{
    public Slider sizeSlider;
    public Button submitSize;
    public Button backToSelectSize;
    public Button nextToSelect;
    public Button backToEdit;
    public Button play;
    public Button save;
    public Button backToSelectSizeFromPlay;
    public Button exit;
    public Button load;
    public TextMeshProUGUI log;
    public SquareField field;
    public CameraSrc camera;

    // Start is called before the first frame update
    void Start()
    {
        field = Instantiate(field);
        field.log = log;
        camera.field = field;
        SetActive(0);
    }

    private void SetActive(int layer)
    {
        sizeSlider.gameObject.SetActive(layer == 0);
        submitSize.gameObject.SetActive(layer == 0);
        load.gameObject.SetActive(layer == 0);
        backToSelectSize.gameObject.SetActive(layer == 1);
        nextToSelect.gameObject.SetActive(layer == 1);
        backToEdit.gameObject.SetActive(layer == 2);
        play.gameObject.SetActive(layer == 2);
        save.gameObject.SetActive(layer == 2);
        backToSelectSizeFromPlay.gameObject.SetActive(layer == 3);
    }

    public void OnSlider()
    {
        submitSize.GetComponentInChildren<TextMeshProUGUI>().text = "submit size " + sizeSlider.value;
        field.SetSize((int)sizeSlider.value);
    }

    public void OnSubmitSize()
    {
        field.SetSize((int)sizeSlider.value);
        field.SetMode(SquareField.Mode.selectFigure);
        SetActive(1);
    }

    public void OnBackToSelectSize()
    {
        field.SetMode(SquareField.Mode.selectSize);
        SetActive(0);
    }

    public void OnNextToSelect()
    {
        field.SetMode(SquareField.Mode.selectVertex);
        SetActive(2);
    }

    public void OnBackToEdit()
    {
        field.SetMode(SquareField.Mode.selectFigure);
        SetActive(1);
    }

    public void OnPlay()
    {

        field.SetMode(SquareField.Mode.play);
        if (field.mode == SquareField.Mode.play)
            SetActive(3);
    }

    public void OnSave()
    {
        if (field.CountSelectedVertex() < 2)
        {
            log.text = "at least 2 vertexes should be selected";
            return;
        }
        field.Save();
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void OnLoad()
    {
        string path = EditorUtility.OpenFilePanel("Overwrite with txt", "", "txt");
        if (path.Length != 0)
        {
            field.Load(path);
            SetActive(3);
        }

    }
}
