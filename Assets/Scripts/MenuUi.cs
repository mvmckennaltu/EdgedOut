using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MenuUi : MonoBehaviour
{
    public GameObject[] titleUI;
    public GameObject[] h2PUI;
    public GameObject[] creditsUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HideMainUI()
    {
        foreach (var item in titleUI)
        {
            item.SetActive(false);
        }
    }
    public void ShowMainUI()
    {
        foreach (var item in titleUI)
        {
            item.SetActive(true);
        }
        foreach (var item in h2PUI)
        {
            item.SetActive(false);
        }
        foreach(var item in creditsUI)
        {
            item.SetActive(false);
        }
    }
    public void ShowH2P()
    {
        HideMainUI();
        foreach(var item in h2PUI)
        {
            item.SetActive(true);
        }
    }
    public void ShowCreditsUI()
    {
        HideMainUI();
        foreach(var item in creditsUI)
        {
            item.SetActive(true);
        }
    }
}
