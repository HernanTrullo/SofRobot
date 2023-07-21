using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RobSof.Assets.Scripts.Interface.Articular;

public class Posiciones_robot : MonoBehaviour
{
    public GameObject pos_cartesianas;
    public GameObject pos_articulares;

    public static List<TextMeshProUGUI> pos_art = new List<TextMeshProUGUI>();
    public static List<TextMeshProUGUI> pos_car = new List<TextMeshProUGUI>();

    public static List<float> POS_ART = new List<float>(){0,0,0,0,0,0};
    public static List<float> POS_CAR = new List<float>(){0,0,0,0,0,0};

    private Rangos_arts rangos_arts = new Rangos_arts();

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<6; i++){
            pos_art.Add(pos_articulares.transform.Find("val_art"+(i+1)).GetComponent<TextMeshProUGUI>());
            pos_car.Add(pos_cartesianas.transform.Find("val_art"+(i+1)).GetComponent<TextMeshProUGUI>());

        }
        for (int i=0; i<6; i++){
            // Para las artculciones
            POS_ART[i]= rangos_arts.posiciones_iniciales[i];
            pos_art[i].text = rangos_arts.posiciones_iniciales[i].ToString("0.##");

            // para las cartesianas
            POS_CAR[i]=rangos_arts.posiciones_iniciales_cartesianas[i];
            pos_car[i].text = rangos_arts.posiciones_iniciales_cartesianas[i].ToString("0.##");
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i<6; i++){
            pos_car[i].text = POS_CAR[i].ToString("0.##");
            pos_art[i].text = POS_ART[i].ToString("0.##");
        }
        
    }

}
