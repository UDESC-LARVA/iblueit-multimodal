/*
 * Oxímetro Serial Connection - MAX30102
 * Crédito: Jhonatan Thallisson Cabral Néry
 * https://github.com/jhonatantcn/IBLUEIT
 */
#include <Wire.h>
#include "MAX30105.h"           //MAX3010x library
#include "heartRate.h"          //Heart rate calculating algorithm
#include "spo2_algorithm.h"     //SPO2 calculating algorithm

#define DEBUG false

MAX30105 particleSensor;

#if defined(__AVR_ATmega328P__) || defined(__AVR_ATmega168__)
//Arduino Uno doesn't have enough SRAM to store 100 samples of IR led data and red led data in 32-bit format
//To solve this problem, 16-bit MSB of the sampled data will be truncated. Samples become 16-bit data.
uint16_t irBuffer[100]; //infrared LED sensor data
uint16_t redBuffer[100];  //red LED sensor data
#else
uint32_t irBuffer[100]; //infrared LED sensor data
uint32_t redBuffer[100];  //red LED sensor data
#endif

int32_t bufferLength; //data length
int32_t spo2; //SPO2 value
int8_t validSPO2; //indicator to show if the SPO2 calculation is valid
int32_t heartRate; //heart rate value
int8_t validHeartRate; //indicator to show if the heart rate calculation is valid

byte readLED = 13; //Blinks with each data read

int cont;
int sumHeartRate;
int sumSPO2;

bool isSampling = false;
void ListenCommand(char cmd)
{
  //ECHO
  if (cmd == 'e' || cmd == 'E')
    Serial.println("echox");

  //READ SAMPLES
  else if (cmd == 'r' || cmd == 'R')
    isSampling = true;

  //FINISH READ
  else if (cmd == 'f' || cmd == 'F')
  {
    isSampling = false;
  }
}

void setup()
{
  Serial.begin(115200);

  pinMode(readLED, OUTPUT);

  delay(2000);
  // Initialize sensor
  particleSensor.begin(Wire, I2C_SPEED_FAST); //Use default I2C port, 400kHz speed
  particleSensor.setup(); //Configure sensor with default settings
  particleSensor.setPulseAmplitudeRed(0x0A); //Turn Red LED to low to indicate sensor is running
}

void loop()
{
#if DEBUG
  isSampling = true;
#endif

  if (Serial.available() > 0)
    ListenCommand(Serial.read());

  if (isSampling)
    HeartRateandSPO();
}

/**
   Sensor Transformations
   Range 0.2V = 0 kPa to 4.7V = 10.0 kPa
   https://github.com/AdamVStephen/gem-water-level-gauge/blob/master/WaterLevelSensor/WaterLevelSensor.ino
*/
bool firstsample = true;
void HeartRateandSPO()
{
  if (firstsample)
  {
    bufferLength = 100; //buffer length of 100 stores 4 seconds of samples running at 25sps
    //read the first 100 samples, and determine the signal range
    for (byte i = 0 ; i < bufferLength ; i++)
    {
      while (particleSensor.available() == false) //do we have new data?
        particleSensor.check(); //Check the sensor for new data

      redBuffer[i] = particleSensor.getRed();
      irBuffer[i] = particleSensor.getIR();
      particleSensor.nextSample(); //We're finished with this sample so move to next sample
    }

    //calculate heart rate and SpO2 after first 100 samples (first 4 seconds of samples)
    maxim_heart_rate_and_oxygen_saturation(irBuffer, bufferLength, redBuffer, &spo2, &validSPO2, &heartRate, &validHeartRate);
    firstsample = false;
  }
  
  //Taking samples from MAX30102.
  //Dumping the first 25 sets of samples in the memory and shift the last 75 sets of samples to the top
  for (byte i = 25; i < 100; i++)
  {
    redBuffer[i - 25] = redBuffer[i];
    irBuffer[i - 25] = irBuffer[i];
  }

  //take 25 sets of samples before calculating the heart rate.
  for (byte i = 75; i < 100; i++)
  {
    while (particleSensor.available() == false) //do we have new data?
      particleSensor.check(); //Check the sensor for new data

    digitalWrite(readLED, !digitalRead(readLED)); //Blink onboard LED with every data read

    redBuffer[i] = particleSensor.getRed();
    irBuffer[i] = particleSensor.getIR();
    particleSensor.nextSample(); //We're finished with this sample so move to next sample

    if (irBuffer[i] > 7000)  //If a finger is detected
    {
      // Filtro 01
      if ((validHeartRate == 1) && (validSPO2 == 1)) //Se a medida de batimentos é válida, então a soma...
      {
        cont += 1;
        sumHeartRate += heartRate;
        sumSPO2 += spo2;
      }
    }
  }

  if (cont != 0)
  {

    Serial.print(sumHeartRate / cont, DEC);
    Serial.print(",");
    Serial.println(sumSPO2 / cont, DEC);

    cont = 0;
    sumHeartRate = 0;
    sumSPO2 = 0;
  }else{
    Serial.print(0);
    Serial.print(",");
    Serial.println(0);
  }

  //After gathering 25 new samples recalculate HR and SP02
  maxim_heart_rate_and_oxygen_saturation(irBuffer, bufferLength, redBuffer, &spo2, &validSPO2, &heartRate, &validHeartRate);
}
