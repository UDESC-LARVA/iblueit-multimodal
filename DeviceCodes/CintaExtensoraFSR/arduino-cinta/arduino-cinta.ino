/* 
 *  Anotações:
 *  Intervalo de medida do sensor: 0 a 1023
 *  Forçando muito consegui simular de 0 a 600 com a respiração
*/

/*
   Cinta Serial Connection
   https://github.com/UDESC-LARVA/IBLUEIT
   https://www.instructables.com/id/FSR-Tutorial/

   Quanto mais pressão aplicar no sensor, menor a resistência.
   A faixa de resistência é realmente muito grande:> 10 MΩ (sem pressão) a ~ 200 Ω (pressão máxima).
   A maioria dos FSRs podem sentir força na faixa de 100g a 10kg.

   Este código faz cerca de 500.000 leituras por minuto, mas envia cerca de 5000 por minuto para o IBIT (cada leitura útil é tirada da média de 100)
   Mudar a Taxa de Transmissão Serial de 115200, para 9600, ou para 250000 não muda a quantidade de leituras, que é sempre cerca de 5000;
   Com o delay de 10 mlissegundos o arduino envia cerca de 2700 leituras por minuto para o IBIT.
   ***Com o delay de 50 mlissegundos o arduino envia cerca de 970 leituras por minuto para o IBIT.
   Com o delay de 100 mlissegundos o arduino envia cerca de 500 leituras por minuto para o IBIT.
   Com o delay de 250 mlissegundos o arduino envia cerca de 228 leituras por minuto para o IBIT.
   Com o delay de 500 mlissegundos o arduino envia cerca de 116 leituras por minuto para o IBIT.

*/

#define SAMPLESIZE 100 //Bloco para média de leituras

int i;
long sum;


//unsigned long time; // Leitura de Tempo
//unsigned long leituras; // Quantidade de Leituras


float ReadSensor()
{ 
  sum = 0;
  for (i = 0; i < SAMPLESIZE; i++)
  {
    sum += analogRead(A0);
  }

  return ((sum / SAMPLESIZE)*-1); // -400 porque é quase o meio das leituras de 0-600; *-1 pra ajustar INS para cima e EXP para baixo.
}



bool isSampling = false;

void ListenCommand(char cmd)
{
  //ECHO
  if (cmd == 'e' || cmd == 'E'){ // Arduino responde o IBIT com "echo"
    Serial.println("echoc");

  //READ SAMPLES
  } else {
    if (cmd == 'r' || cmd == 'R'){ // Arduino envia leituras da cinta ao IBIT 
      isSampling = true;

  //FINISH READ
    } else {
      if (cmd == 'f' || cmd == 'F'){ // Arduino interrompe o envio de leituras
        isSampling = false;
      }
    }
  }
}


void setup(void) 
{
  Serial.begin(115200); // We'll send debugging information via the Serial monitor

//  time = millis(); // Leitura de Tempo
}


void loop(void) 
{
  //#if DEBUG
  //isSampling = false;
  //#endif

  if (Serial.available() > 0)
    ListenCommand(Serial.read());
    
  if (isSampling){
    Serial.println(ReadSensor());
//    leituras += 1; // Contagem de Leituras
    delay(50); // 100 mlissegundos (0,1 segundo)
  }
  
//  if ((millis()-time)>(60000)){  // Leitura de Tempo (60 segundos)
//   Serial.print("Tempo: "); // Leitura de Tempo 
//   Serial.println(millis()-time); // Leitura de Tempo
//   Serial.print("Leituras: "); // Contagem de Leituras
//   Serial.println(leituras); // Leitura de Tempo
//  }
}
