/* 
 *  Anotações:
 *  Intervalo de medida do sensor: 0 a 1023
 *  Forçando muito consegui simular de 0 a 600 com a respiração
*/

/*
   Cinta Serial Connection
   https://github.com/jhonatantcn/IBLUEIT
   https://www.instructables.com/id/FSR-Tutorial/

   Quanto mais pressão aplicar no sensor, menor a resistência.
   A faixa de resistência é realmente muito grande:> 10 MΩ (sem pressão) a ~ 200 Ω (pressão máxima).
   A maioria dos FSRs podem sentir força na faixa de 100g a 10kg.
*/

#define SAMPLESIZE 100 //Bloco para média de leituras

int i;
long sum;

float ReadSensor()
{ 
  sum = 0;
  for (i = 0; i < SAMPLESIZE; i++)
  {
    sum += analogRead(A0);
  }

  return ((sum / SAMPLESIZE)-200)*-1; // -200 porque é quase o meio das leituras de 0-600; *-1 pra ajustar INS para cima e EXP para baixo.
}



bool isSampling = false;

void ListenCommand(char cmd)
{
  //ECHO
  if (cmd == 'e' || cmd == 'E'){
    Serial.println("echoc");

  //READ SAMPLES
  } else {
    if (cmd == 'r' || cmd == 'R'){
      isSampling = true;

  //FINISH READ
    } else {
      if (cmd == 'f' || cmd == 'F'){
        isSampling = false;
      }
    }
  }
}


void setup(void) 
{
  Serial.begin(115200); // We'll send debugging information via the Serial monitor
}


void loop(void) 
{
  //#if DEBUG
  //isSampling = false;
  //#endif

  if (Serial.available() > 0)
    ListenCommand(Serial.read());
    
  if (isSampling)
    Serial.println(ReadSensor());
}
