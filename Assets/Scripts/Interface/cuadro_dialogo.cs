using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Cuadro_dialogo : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform dialog_cuadro;
    public Button btn_opcion_si;
    public Button btn_opcion_no;
    public UnityEvent btn_si_click_event;
    private bool opcion_si = false;

    void Start()
    {
        btn_opcion_no.onClick.AddListener(delegate{
            dialog_cuadro.gameObject.SetActive(false);
        });

        btn_opcion_si.onClick.AddListener(delegate{
            opcion_si = true;
            btn_si_click_event.Invoke();
            dialog_cuadro.gameObject.SetActive(false);
        }); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnBecameVisible() {
        opcion_si = false;
    }
    public bool marco_si(){
        return opcion_si;
    }
}
