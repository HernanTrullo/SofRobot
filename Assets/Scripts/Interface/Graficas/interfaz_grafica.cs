using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class interfaz_grafica : MonoBehaviour
{
    public GameObject panel_graficas;
    public Button btn_boton_graf; 
    private bool se_muestra = true;

    // Aspectos de la interfaz de gráficas
    public TMP_Text lb_y;
    public TMP_Text lb_x;
    public TMP_Text lb_y_min;
    public TMP_Text lb_y_max;
    public TMP_Text lb_x_max;
    public TMP_Text lb_title;

    
    // Se crean las trayectorias que se serán mostradas (Serán los errores articulares y el error cuadrático medio)
    public TMP_Dropdown trayect;
    public LineRenderer line_render_graf;
    private Dictionary<string, List<float>> trayectories = new Dictionary<string, List<float>>();
    private string[] name_trayectory = new string[]{"Articulación 1","Articulación 2",
                                                    "Articulación 3","Articulación 4",
                                                    "Articulación 5","Articulación 6",
                                                    "Error Cuadrático Medio"};
    private float range_graf_y = 360;

    private float pos_ini_y = -180;
    private float pos_ini_x = -230; 

    // Creación de la descripción de las trayectorias
    private string lb_title_str = "Error Aritcular: "; 


    // Start is called before the first frame update
    void Start()
    {
        trayect.onValueChanged.AddListener(plotear);
        btn_boton_graf.onClick.AddListener(mostrar_panel);  
        // Se inicializan los nombres de las trayectorias
        trayect.ClearOptions();
        foreach (var item in name_trayectory)
        {
            trayectories.Add(item, new List<float>());
            trayect.options.Add(new TMP_Dropdown.OptionData(item));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    void mostrar_panel(){
        se_muestra = !se_muestra;
        panel_graficas.SetActive(se_muestra);
    }
    void plotear(int index){
        if (index < name_trayectory.Length-1){
            set_labels("Tiempo: Sec", name_trayectory[index]+": m", lb_title_str+name_trayectory[index]);
        }
        else{
            set_labels("Tiempo: Sec", name_trayectory[index]+ ": m", name_trayectory[index]);
        }

        paint_graf(trayectories[name_trayectory[index]]);
        
    }

    private void paint_graf(List<float> tray){
        line_render_graf.positionCount = 1;

        float pend_y = (range_graf_y)/(tray.Max()-tray.Min());
        float y_0 = pos_ini_y - pend_y*tray.Min();
        
        lb_y_max.text = tray.Max().ToString("0.##");
        lb_y_min.text = tray.Min().ToString("0.##");
        lb_x_max.text = (Trayectoria.TIEMPO_MUESTREO*tray.Count()).ToString("0.##");
        line_render_graf.positionCount = tray.Count;

        for (int i = 0; i<line_render_graf.positionCount; i++){
            Vector3 point = new Vector3(pos_ini_x+i,tray[i]*pend_y + y_0 ,0);
            line_render_graf.SetPosition(i,point);
        }
    }

    void set_title(string lb_title){
        this.lb_title.text = lb_title;
    }
    void set_lb_x(string lb_x){
        this.lb_x.text = lb_x;        
    }

    void set_lb_y (string lb_y){
        this.lb_y.text = lb_y;
    }

    void set_labels (string lb_x, string lb_y, string title){
        set_title(title);
        set_lb_x(lb_x);
        set_lb_y(lb_y);
    }

    public void set_trayectoria(List<float> tray, string name_tray){
        trayectories[name_tray] = tray;
    }

    public void asignar_trays(List<List<float>> tray){
        for (var i = 0; i < tray.Count; i++)
        {
            set_trayectoria(tray[i], name_trayectory[i]);
        }
    }

    public void asignar_ECM(List<float> tray){
        set_trayectoria(tray, name_trayectory[name_trayectory.Length-1]);
    }

}
