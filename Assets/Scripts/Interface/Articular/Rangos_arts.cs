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

        private int art2_min = -45;
        private int art2_max = 45;

        private int art3_min = 0;
        private int art3_max = 50;

        private int art4_min = 0;
        private int art4_max = 70;

        private int art5_min = 0;
        private int art5_max = 80;

        private int art6_min = 0;
        private int art6_max = 60;


        public int [,] rango_arts = new int[6,2];
        public float [] posiciones_iniciales = new float[6];

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

            for (int j=0; j<6; j++){
                posiciones_iniciales[j] = 0;//(rango_arts[j, 1]+rango_arts[j, 0])/2;
            }
        }
    }
}