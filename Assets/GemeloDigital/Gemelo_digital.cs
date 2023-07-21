using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gemelo_digital : MonoBehaviour
{
    public GameObject art1;
    public GameObject art2;
    public GameObject art3;
    public GameObject art4;
    public GameObject art5;
    public GameObject art6;

    private GameObject [] arts = new GameObject [6];
    private Vector3 rotacion = new Vector3(0,0,0);
    // Start is called before the first frame update
    void Start()
    {
        // Inicialización de las articualaciones 
        arts[0] = art1;
        arts[1] = art2;
        arts[2] = art3;
        arts[3] = art4;
        arts[4] = art5;
        arts[5] = art6;

    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<6; i++){
            rotar_articulación(i, Posiciones_robot.POS_ART[i]);
        }

    }
    public void rotar_articulación(int index_art, float angle){
        if (index_art >=0 && index_art <=2 || index_art==5){
            rotacion.y = -angle;
        }
        else{
            rotacion.y = angle;
        }
        arts[index_art].transform.localRotation = Quaternion.Euler(rotacion);
    }
}
