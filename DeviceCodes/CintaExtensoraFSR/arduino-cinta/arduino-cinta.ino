/* 
 *  Anotações:
 *  Intervalo de medida do sensor: 100g a 10kg
 *  Forçando muito consegui simular de 0 a 400g com a respiração
*/

/*
   Cinta Serial Connection
   https://github.com/UDESC-LARVA/IBLUEIT
   https://www.instructables.com/id/FSR-Tutorial/

   Example sketch for SparkFun's force sensitive resistors
    (https://www.sparkfun.com/products/9375)
    Jim Lindblom @ SparkFun Electronics
    April 28, 2016
  
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

const int FSR_PIN = A0; // Pin connected to FSR/resistor divider

// Measure the voltage at 5V and resistance of your 3.3k resistor, and enter
// their value's below:
const float VCC = 4.98; // Measured voltage of Ardunio 5V line
const float R_DIV = 10000.0; // Measured resistance of 10k resistor


int fsrADC; //FSR ADC
float fsrV; //FSR voutage
float fsrR; //FSR Resistance
float fsrG; //FSR Condutance
float force; //FSR Force (em gramas)
int i;
long sum;


//unsigned long time; // Leitura de Tempo
//unsigned long leituras; // Quantidade de Leituras


float ReadSensor()
{ 
  sum = 0;

  for (i = 0; i < SAMPLESIZE; i++)
  {
    fsrADC = analogRead(FSR_PIN);
    // If the FSR has no pressure, the resistance will be
    // near infinite. So the voltage should be near 0.
    if (fsrADC != 0) // If the analog reading is non-zero
    {
      // Use ADC reading to calculate voltage:
      fsrV = fsrADC * VCC / 1023.0;
      // Use voltage and static resistor value to 
      // calculate FSR resistance:
      fsrR = R_DIV * (VCC / fsrV - 1.0);
      //Serial.println("Resistance: " + String(fsrR) + " ohms");
    
      // Guesstimate force based on slopes in figure 3 of
      // FSR datasheet:
      fsrG = 1.0 / fsrR; // Calculate conductance
      // Break parabolic curve down into two linear slopes:
      if (fsrR <= 600)
        force = (fsrG - 0.00075) / 0.00000032639;
      else
        force =  fsrG / 0.000000642857;

      sum += force;
    }
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
