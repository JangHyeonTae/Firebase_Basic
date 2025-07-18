using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpPanel : MonoBehaviour
{
    [SerializeField] private GameObject signUpPanel;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private Button returnButton;

    void Start()
    {
        returnButton.onClick.AddListener(ReturnPanel);
    }

    public void Init(string _text)
    {
        errorText.text = _text;
    }
    private void ReturnPanel()
    {
        signUpPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
