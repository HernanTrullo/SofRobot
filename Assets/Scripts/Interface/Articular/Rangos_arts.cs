using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobSof.Assets.Scripts.Interface.Articular
{
    public class Rangos_arts
    {
        private int art1_min = -90;
        private int art1_max = 90;

        private int art2_min = 0;
        private int art2_max = 135;

        private int art3_min = 100;
        private int art3_max = 270;

        private int art4_min = 180-45;
        private int art4_max = 180+45;

        private int art5_min = -80;
        private int art5_max = 80;

        private int art6_min = 180-45;
        private int art6_max = 180+45;

        private float posx= 0.8f;
        private float posy = 0.0f;
        private float posz = 0.3f;

        private float rotx = 180;
        private float roty = 0;
        private float rotz = 180;


        public int [,] rango_arts = new int[6,2];
        public float [] posiciones_iniciales = new float[6];
        public float [] posiciones_iniciales_cartesianas = new float[6];

        public List<float> po_ini_art_cart;

        private PUMA_modelo pm_mod = new PUMA_modelo();

        public Rangos_arts(){
            rango_arts[0,0] = art1_min;
            rango_arts[0,1] = art1_max;

            rango_arts[1,0] = art2_min;
            rango_arts[1,1] = art2_max;

            rango_arts[2,0] = art3_min;
            rango_arts[2,1] = art3_max;

            rango_arts[3,0] = art4_min;
            rango_arts[3,1] = art4_max;

            rango_arts[4,0] = art5_min;
            rango_arts[4,1] = art5_max;

            rango_arts[5,0] = art6_min;
            rango_arts[5,1] = art6_max;

            posiciones_iniciales_cartesianas[0] = posx;
            posiciones_iniciales_cartesianas[1] = posy;
            posiciones_iniciales_cartesianas[2] = posz;
            posiciones_iniciales_cartesianas[3] = rotx;
            posiciones_iniciales_cartesianas[4] = roty;
            posiciones_iniciales_cartesianas[5] = rotz;
            
            po_ini_art_cart = pm_mod.mgi_puma(posx, posy, posz, rotx, roty, rotz);
             

            for (int j=0; j<6; j++){
                posiciones_iniciales[j] = (rango_arts[j, 1]+rango_arts[j, 0])/2;
            }
        }
    }
}