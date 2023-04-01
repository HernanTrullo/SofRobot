using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUMA_modelo
{
    public static float d3 = 0.63198f;
    public static float  r4 = 0.5182f;

    public List<float> mgd_puma(float [] q){

        float C1 = Coseno(q[0]);
        float C2 = Coseno(q[1]);
        float C3 = Coseno(q[2]);
        float C4 = Coseno(q[3]);
        float C5 = Coseno(q[4]);
        float C6 = Coseno(q[5]);
        float S1 = Seno(q[0]);
        float S2 = Seno(q[1]);
        float S3 = Seno(q[2]);
        float S4 = Seno(q[3]);
        float S5 = Seno(q[4]);
        float S6 = Seno(q[5]);
        float C23 = Coseno(q[1] + q[2]);
        float S23 = Coseno(q[1] + q[2]);


        float xa = Coseno(q[0])*((-Seno(q[1] + q[2])*r4 + Coseno(q[1])*d3));
        float ya = Seno(q[0])*((-Seno(q[1] + q[2])*r4 + Coseno(q[1])*d3));
        float za = Coseno(q[1] + q[2])*r4 + Seno(q[1])*d3;

        float sx = - S6*(S4*(C1*C2*C3 - C1*S2*S3) + C4*S1) - C6*(C5*(S1*S4 - C4*(C1*C2*C3 - C1*S2*S3)) + S5*(C1*C2*S3 + C1*C3*S2));
        float sy = C6*(C5*(C4*(C2*C3*S1 - S1*S2*S3) + C1*S4) - S5*(C2*S1*S3 + C3*S1*S2)) - S6*(S4*(C2*C3*S1 - S1*S2*S3) - C1*C4);
        float sz = - C6*(S5*(S2*S3 - C2*C3) - C4*C5*(C2*S3 + C3*S2)) - S4*S6*(C2*S3 + C3*S2);
        float nx = S6*(C5*(S1*S4 - C4*(C1*C2*C3 - C1*S2*S3)) + S5*(C1*C2*S3 + C1*C3*S2)) - C6*(S4*(C1*C2*C3 - C1*S2*S3) + C4*S1);
        float ny = - S6*(C5*(C4*(C2*C3*S1 - S1*S2*S3) + C1*S4) - S5*(C2*S1*S3 + C3*S1*S2)) - C6*(S4*(C2*C3*S1 - S1*S2*S3) - C1*C4);
        float nz = S6*(S5*(S2*S3 - C2*C3) - C4*C5*(C2*S3 + C3*S2)) - C6*S4*(C2*S3 + C3*S2);
        float ax = S5*(S1*S4 - C4*(C1*C2*C3 - C1*S2*S3)) - C5*(C1*C2*S3 + C1*C3*S2);
        float ay = - C5*(C2*S1*S3 + C3*S1*S2) - S5*(C4*(C2*C3*S1 - S1*S2*S3) + C1*S4);
        float az = - C5*(S2*S3 - C2*C3) - C4*S5*(C2*S3 + C3*S2);

        Matrix4x4 rotm = Matrix4x4.identity;
        rotm.m00 = sx; rotm.m01 = nx; rotm.m02 = ax;
        rotm.m10 = sy; rotm.m11 = ny; rotm.m12 = ay;
        rotm.m20 = sz; rotm.m21 = nz; rotm.m22 = az;


        float [] angles = rotm2euler(rotm);
        return new List<float> {xa, ya, za, angles[0], angles[1], angles[2]};
        
    }
    public float [] rotm2euler(Matrix4x4 rotm){
        Vector3 angles = new Vector3();
        float threshold = 0.9999f;

        angles.y = Mathf.Asin(rotm.m20);
        // Checheo del gimbal lock
        if (Mathf.Abs(rotm.m20) > threshold){
            angles.x = (float)Mathf.Atan2(rotm.m01, rotm.m02);
            angles.z = 0.0f;
        }
        else{
            angles.x = (float)Mathf.Atan2(-rotm.m21, rotm.m22);
            angles.z = (float)Mathf.Atan2(-rotm.m10, rotm.m00);
        }

        Vector3 angles_grados = angles*Mathf.Rad2Deg;
        if (angles_grados.x < 0){
            angles_grados.x = 360 + angles_grados.x;
        } 
        if (angles_grados.z < 0){
            angles_grados.z = 360 + angles_grados.z;
        }
        
        return new float[] {angles_grados.x, angles_grados.y, angles_grados.z};
    }

    public List<float> mgi_puma(float posx, float posy, float posz, float rotx, float roty, float rotz){
        Matrix4x4 Mrot = euler2rotm(rotx, roty, rotz);

        float sx= Mrot.m00;
        float sy= Mrot.m10;
        float sz= Mrot.m20;

        float nx= Mrot.m01;
        float ny= Mrot.m11;
        float nz= Mrot.m21;

        float ax= Mrot.m02;
        float ay= Mrot.m12;
        float az= Mrot.m22;

        float Px = posx;
        float Py = posy;
        float Pz = posz;

        float q1 = 0;
        //Calculo de q1:
        if (Px <= Mathf.Epsilon){
            q1 = 0;
        }else{
            q1 = Mathf.Atan2(Py,Px);
        }
        

        //Calculo de q2 y q3:

        float X = -2*Pz*d3;
        float B1 = Px*Coseno(q1) + Py*Seno(q1);
        float Y = -2*B1*d3;
        float Z = Pow(r4,2) - Pow(d3,2) - Pow(Pz,2) - Pow(B1,2);

        float C2 = (Y*Z + X*Sqrt(Pow(X,2) + Pow(Y,2) - Pow(Z,2)))/( Pow(X,2) + Pow(Y,2));
        float S2 = (X*Z - Y*Mathf.Sqrt(Pow(X,2) + Pow(Y,2) - Pow(Z,2)))/(Pow(X,2) + Pow(Y,2));

        float q2 = Mathf.Atan2(S2,C2);

        float S3 = (-Pz*Seno(q2) - B1*Coseno(q2) + d3)/(r4);
        float C3 = (-B1*Seno(q2) + Pz*Coseno(q2))/(r4);
        
        //%S3 = (-Pz*S2 - B1*C2 + d3)/(r4);
        //%C3 = (-B1*S2 + Pz*C2)/(r4);

        float q3 = Mathf.Atan2(S3,C3);

        //Calculo de q4:

        float Hx = Coseno(q2 + q3)*(Coseno(q1)*ax + Seno(q1)*ay) + Seno(q2 + q3)*az;
        float Hy =-Seno(q2 + q3)*(Coseno(q1)*ax + Seno(q1)*ay) + Coseno(q2 + q3)*az;
        float Hz = Seno(q1)*ax - Coseno(q1)*ay;

        float q4 = Mathf.Atan2(Hz,-Hx);
        
        //%q4 = Mathf.Atan2(Hz,-Hx) + pi;

        //Calculo de q5:

        float S5 = -Coseno(q4)*Hx + Seno(q4)*Hz;
        float C5 = Hy;

        float q5 = Mathf.Atan2(S5,C5);
        //Calculo de q6:

        float Fx = Coseno(q2 + q3)*(Coseno(q1)*sx + Seno(q1)*sy) + Seno(q2 + q3)*sz;
        float Fz = Seno(q1)*sx - Coseno(q1)*sy;
        float Gx = Coseno(q2 + q3)*(Coseno(q1)*nx + Seno(q1)*ny) + Seno(q2 + q3)*nz;
        float Gz = Seno(q1)*nx - Coseno(q1)*ny;

        float S6 = -Coseno(q4)*Fz - Seno(q4)*Fx;
        float C6 = -Coseno(q4)*Gz - Seno(q4)*Gx;

        float q6 = Mathf.Atan2(S6,C6);

        if (q3<0){
            q3 = 2*Mathf.PI+q3;
        }

        if (q4<0){
            q4 = 2*Mathf.PI+q4;
        }
        if (q5<0){
            q5 = 2*Mathf.PI+q5;
        }
        if (q6<0){
            q6 = 2*Mathf.PI+q6;
        }

        return new List<float> {q1, q2, q3, q4, q5, q6};
    }

    

    public Matrix4x4 euler2rotm(float rotx, float roty, float rotz){
        Quaternion rotation = Quaternion.Euler(rotx, roty, rotz);
        Matrix4x4 rotm = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        return rotm;
    }

    public void puma_inverso(float t1,float t2,float t3,float t4,float t5,float t6,float QP1,float QP2,float QP3,float QP4,float QP5,float QP6,float QDP1,float QDP2,float QDP3,float QDP4,float QDP5,float QDP6){

        float CX2=0;
        float CX3=0;
        float CX4=0;
        float CX5=0;
        float CX6=0;
        float CY2=0;
        float CY3=0;
        float CY4=0;
        float CY5=0;
        float CY6=0;
        float CZ1=0;
        float CZ2=0;
        float CZ3=0;
        float CZ4=0;
        float CZ5=0;
        float CZ6=0;
        float D3=d3;
        float RL4=0.4f;
        float FS1=0.0f;
        float FS2=0.0f;
        float FS3=0.0f;
        float FS4=0.0f;
        float FS5=0.0f;
        float FS6=0.0f;
        float FV1=0.0f;
        float FV2=0.0f;
        float FV3=0.0f;
        float FV4=0.0f;
        float FV5=0.0f;
        float FV6=0.0f;
        //float FX2=0;
        float FX3=0;
        float FX4=0;
        float FX5=0;
        float FX6=0;
        //float FY2=0;
        float FY3=0;
        float FY4=0;
        float FY5=0;
        float FY6=0;
        float FZ3=0;
        float FZ4=0;
        float FZ5=0;
        float FZ6=0;
        float G3=9.81f;
        //float IA2=0;
        float IA3=0.018f;
        float IA4=0.035f; 
        float IA5=0.035f;
        float IA6=0.04f;
        float MX2R=22.1441f;
        float MX3=1.1985f;
        float MX4=1.1782f;
        float MX5=1.4521f;
        float MX6=1.2446f;
        float MY2=0.1007f;
        float MY3R=11.3783f;
        float MY4R=-3.3816f;
        float MY5R=2.5648f;
        float MY6=0.0214f;
        float XX2R=-13.696f;
        float XX3R=13.9876f;
        float XX4R=3.7334f;
        float XX5R=2.4508f;
        float XX6R=-0.6578f;
        float XY3=0.01784f;
        float XY4=-0.0127f;
        float XY5=0.0770f;
        float XY6=0.0114f;
        float XZ2R=-8.3470f;
        float XZ3=0.7130f;
        float XZ4=1.1710f;
        float XZ5=1.4600f;
        float XZ6=1.2514f;
        float YZ3=0.2010f;
        float YZ4=-0.0347f;
        float YZ5=0.1692f;
        float YZ6=0.0215f;
        float ZZ1R=23.647f;
        float ZZ2R=13.8311f;
        float ZZ3R=14.2033f;
        float ZZ4R=4.2995f;
        float ZZ5R=3.7292f;
        float ZZ6=0.0661f;


        float S1=Seno(t1);
        float S2=Seno(t2);
        float C2=Coseno(t2);
        float S3=Seno(t3);
        float C3=Coseno(t3);
        float S4=Seno(t4);
        float C4=Coseno(t4);
        float S5=Seno(t5);
        float C5=Coseno(t5);
        float S6=Seno(t6);
        float C6=Coseno(t6);

        float No31=QDP1*ZZ1R;
        float WI12=QP1*S2;
        float WI22=C2*QP1;
        float WP12=QDP1*S2 + QP2*WI22;
        float WP22=C2*QDP1 - QP2*WI12;
        float DV112=- Pow(WI12,2);
        float DV222=- Pow(WI22,2);
        float DV332=- Pow(QP2,2);
        float DV122=WI12*WI22;
        float DV132=QP2*WI12;
        float DV232=QP2*WI22;
        float U112=DV222 + DV332;
        float U212=DV122 + QDP2;
        float U232=DV232 - WP12;
        float U312=DV132 - WP22;
        float VP12=-(G3*S2);
        float VP22=-(C2*G3);
        float PIS22=XX2R - ZZ2R;
        float No12=WP12*XX2R + U212*XZ2R + DV232*ZZ2R;
        float No22=DV132*PIS22 + (DV112 - DV332)*XZ2R;
        float No32=-(DV122*XX2R) - U232*XZ2R + QDP2*ZZ2R;
        float WI13=C3*WI12 + S3*WI22;
        float WI23=-(S3*WI12) + C3*WI22;
        float W33=QP2 + QP3;
        float WP13=QP3*WI23 + C3*WP12 + S3*WP22;
        float WP23=-(QP3*WI13) - S3*WP12 + C3*WP22;
        float WP33=QDP2 + QDP3;
        float DV113=- Mathf.Pow(WI13,2);
        float DV223=- Mathf.Pow(WI23,2);
        float DV333=- Mathf.Pow(W33,2);
        float DV123=WI13*WI23;
        float DV133=W33*WI13;
        float DV233=W33*WI23;
        float U113=DV223 + DV333;
        float U123=DV123 - WP33;
        float U133=DV133 + WP23;
        float U213=DV123 + WP33;
        float U223=DV113 + DV333;
        float U233=DV233 - WP13;
        float U313=DV133 - WP23;
        float U323=DV233 + WP13;
        float VSP13=D3*U112 + VP12;
        float VSP23=D3*U212 + VP22;
        float VSP33=D3*U312;
        float VP13=C3*VSP13 + S3*VSP23;
        float VP23=-(S3*VSP13) + C3*VSP23;
        float F13=MX3*U113 + MY3R*U123;
        float F23=MX3*U213 + MY3R*U223;
        float F33=MX3*U313 + MY3R*U323;
        float PIS23=XX3R - ZZ3R;
        float No13=WP13*XX3R - U313*XY3 + U213*XZ3 + (-DV223 + DV333)*YZ3 + DV233*ZZ3R;
        float No23=DV133*PIS23 + U323*XY3 + (DV113 - DV333)*XZ3 - U123*YZ3;
        float No33=-(DV123*XX3R) + (-DV113 + DV223)*XY3 - U233*XZ3 + U133*YZ3 + WP33*ZZ3R;
        float WI14=-(S4*W33) + C4*WI13;
        float WI24=-(C4*W33) - S4*WI13;
        float W34=QP4 + WI23;
        float WP14=QP4*WI24 + C4*WP13 - S4*WP33;
        float WP24=-(QP4*WI14) - S4*WP13 - C4*WP33;
        float WP34=QDP4 + WP23;
        float DV114=-Pow(WI14,2);
        float DV224=-Pow(WI24,2);
        float DV334=-Pow(W34,2);
        float DV124=WI14*WI24;
        float DV134=W34*WI14;
        float DV234=W34*WI24;
        float U114=DV224 + DV334;
        float U124=DV124 - WP34;
        float U134=DV134 + WP24;
        float U214=DV124 + WP34;
        float U224=DV114 + DV334;
        float U234=DV234 - WP14;
        float U314=DV134 - WP24;
        float U324=DV234 + WP14;
        float VSP14=RL4*U123 + VP13;
        float VSP24=RL4*U223 + VP23;
        float VSP34=RL4*U323 + VSP33;
        float VP14=C4*VSP14 - S4*VSP34;
        float VP24=-(S4*VSP14) - C4*VSP34;
        float F14=MX4*U114 + MY4R*U124;
        float F24=MX4*U214 + MY4R*U224;
        float F34=MX4*U314 + MY4R*U324;
        float PIS24=XX4R - ZZ4R;
        float No14=WP14*XX4R - U314*XY4 + U214*XZ4 + (-DV224 + DV334)*YZ4 + DV234*ZZ4R;
        float No24=DV134*PIS24 + U324*XY4 + (DV114 - DV334)*XZ4 - U124*YZ4;
        float No34=-(DV124*XX4R) + (-DV114 + DV224)*XY4 - U234*XZ4 + U134*YZ4 + WP34*ZZ4R;
        float WI15=S5*W34 + C5*WI14;
        float WI25=C5*W34 - S5*WI14;
        float W35=QP5 - WI24;
        float WP15=QP5*WI25 + C5*WP14 + S5*WP34;
        float WP25=-(QP5*WI15) - S5*WP14 + C5*WP34;
        float WP35=QDP5 - WP24;
        float DV115=-Pow(WI15,2);
        float DV225=-Pow(WI25,2);
        float DV335=-Pow(W35,2);
        float DV125=WI15*WI25;
        float DV135=W35*WI15;
        float DV235=W35*WI25;
        float U115=DV225 + DV335;
        float U125=DV125 - WP35;
        float U135=DV135 + WP25;
        float U215=DV125 + WP35;
        float U225=DV115 + DV335;
        float U235=DV235 - WP15;
        float U315=DV135 - WP25;
        float U325=DV235 + WP15;
        float VP15=C5*VP14 + S5*VSP24;
        float VP25=-(S5*VP14) + C5*VSP24;
        float F15=MX5*U115 + MY5R*U125;
        float F25=MX5*U215 + MY5R*U225;
        float F35=MX5*U315 + MY5R*U325;
        float PIS25=XX5R - ZZ5R;
        float No15=WP15*XX5R - U315*XY5 + U215*XZ5 + (-DV225 + DV335)*YZ5 + DV235*ZZ5R;
        float No25=DV135*PIS25 + U325*XY5 + (DV115 - DV335)*XZ5 - U125*YZ5;
        float No35=-(DV125*XX5R) + (-DV115 + DV225)*XY5 - U235*XZ5 + U135*YZ5 + WP35*ZZ5R;
        float WI16=-(S6*W35) + C6*WI15;
        float WI26=-(C6*W35) - S6*WI15;
        float W36=QP6 + WI25;
        float WP16=QP6*WI26 + C6*WP15 - S6*WP35;
        float WP26=-(QP6*WI16) - S6*WP15 - C6*WP35;
        float WP36=QDP6 + WP25;
        float DV116=-Pow(WI16,2);
        float DV226=-Pow(WI26,2);
        float DV336=-Pow(W36,2);
        float DV126=WI16*WI26;
        float DV136=W36*WI16;
        float DV236=W36*WI26;
        float U116=DV226 + DV336;
        float U126=DV126 - WP36;
        float U136=DV136 + WP26;
        float U216=DV126 + WP36;
        float U226=DV116 + DV336;
        float U236=DV236 - WP16;
        float U316=DV136 - WP26;
        float U326=DV236 + WP16;
        float VP16=C6*VP15 + S6*VP24;
        float VP26=-(S6*VP15) + C6*VP24;
        float F16=MX6*U116 + MY6*U126;
        float F26=MX6*U216 + MY6*U226;
        float F36=MX6*U316 + MY6*U326;
        float PIS26=XX6R - ZZ6;
        float No16=WP16*XX6R - U316*XY6 + U216*XZ6 + (-DV226 + DV336)*YZ6 + DV236*ZZ6;
        float No26=DV136*PIS26 + U326*XY6 + (DV116 - DV336)*XZ6 - U126*YZ6;
        float No36=-(DV126*XX6R) + (-DV116 + DV226)*XY6 - U236*XZ6 + U136*YZ6 + WP36*ZZ6;
        float E16=F16 + FX6;
        float E26=F26 + FY6;
        float E36=F36 + FZ6;
        float N16=CX6 + No16 + MY6*VP25;
        float N26=CY6 + No26 - MX6*VP25;
        float N36=CZ6 + No36 - MY6*VP16 + MX6*VP26;
        float FDI16=C6*E16 - E26*S6;
        float FDI36=-(C6*E26) - E16*S6;
        float E15=F15 + FDI16 + FX5;
        float E25=E36 + F25 + FY5;
        float E35=F35 + FDI36 + FZ5;
        float N15=CX5 + C6*N16 + No15 - N26*S6 - MY5R*VP24;
        float N25=CY5 + N36 + No25 + MX5*VP24;
        float N35=CZ5 - C6*N26 + No35 - N16*S6 - MY5R*VP15 + MX5*VP25;
        float FDI15=C5*E15 - E25*S5;
        float FDI35=C5*E25 + E15*S5;
        float E14=F14 + FDI15 + FX4;
        float E24=-E35 + F24 + FY4;
        float E34=F34 + FDI35 + FZ4;
        float N14=CX4 + C5*N15 + No14 - N25*S5 + MY4R*VSP24;
        float N24=CY4 - N35 + No24 - MX4*VSP24;
        float N34=CZ4 + C5*N25 + No34 + N15*S5 - MY4R*VP14 + MX4*VP24;
        float FDI14=C4*E14 - E24*S4;
        float FDI34=-(C4*E24) - E14*S4;
        float E13=F13 + FDI14 + FX3;
        float E23=E34 + F23 + FY3;
        float E33=F33 + FDI34 + FZ3;
        float N13=CX3 + C4*N14 + No13 + FDI34*RL4 - N24*S4 + MY3R*VSP33;
        float N23=CY3 + N34 + No23 - MX3*VSP33;
        float N33=CZ3 - C4*N24 + No33 - FDI14*RL4 - N14*S4 - MY3R*VP13 + MX3*VP23;
        float FDI23=C3*E23 + E13*S3;
        float N12=CX2 + C3*N13 + No12 - N23*S3;
        float N22=CY2 - D3*E33 + C3*N23 + No22 + N13*S3;
        float N32=CZ2 + D3*FDI23 + N33 + No32 - MY2*VP12 + MX2R*VP22;
        float N31=CZ1 + C2*N22 + No31 + N12*S2;

        float GAM1=N31 + FV1*QP1 + FS1*Mathf.Sign(QP1);
        float GAM2=N32 + FV2*QP2 + FS2*Mathf.Sign(QP2);
        float GAM3=N33 + IA3*QDP3 + FV3*QP3 + FS3*Mathf.Sign(QP3);
        float GAM4=N34 + IA4*QDP4 + FV4*QP4 + FS4*Mathf.Sign(QP4);
        float GAM5=N35 + IA5*QDP5 + FV5*QP5 + FS5*Mathf.Sign(QP5);
        float GAM6=N36 + IA6*QDP6 + FV6*QP6 + FS6*Mathf.Sign(QP6);
    }



    ////////////////////////////////////////////////////////////
    // Función que calcula el angulo en radianes
    private float Coseno(float angle){
        if (Mathf.Abs(Mathf.Cos(angle)) < double.Epsilon){
            return 0;
        }
        else{
            return Mathf.Cos(angle);
        }
    }
    private float Seno(float angle){
        if (Mathf.Abs(Mathf.Sin(angle)) < double.Epsilon){
            return 0;
        }
        else{
            return Mathf.Sin(angle);
        }
    }

    private float Pow(float value, float exp){
        return Mathf.Pow(value, exp);
    }
    private float Sqrt(float value){
        return Mathf.Sqrt(value);
    }
}
