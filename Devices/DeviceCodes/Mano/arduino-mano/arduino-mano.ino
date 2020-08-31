/*
 * Mano Serial Connection - MPX5700AP
 * https://github.com/UDESC-LARVA/IBLUEIT
 * 
 * RANGE do Sensor MPX5700AP p/ pressão absoluta
 * Vai de 0 a 700KPA, mas consegui dividir o range entre positivo e negativo
 * Max~: 5000 Pa
 * Stop: 0.0 Pa
 * Min~: -5000 Pa
 */

#define SAMPLESIZE 100
#define MOVING_AVERAGE true
#define DEBUG false

bool isCalibrated = false;
float calibrationValue = 0.0;
void Calibrate()
{

#if MOVING_AVERAGE
  for(int i = 0; i < SAMPLESIZE;i++)
  {
    //band-aid fix: this will force the sensor to populate the moving average array before using it
    ReadSensor();
  }
  calibrationValue = ReadSensor();
#else
  float sum = 0.0;

  for (i = 0; i < SAMPLESIZE; i++)
    sum += voutToKPa(digitalToVout(analogRead(A2)));

  calibrationValue = sum / SAMPLESIZE;
#endif  
  
  isCalibrated = true;
}


//////////////////// SMOOTHING 01 /////////////////////////////////////////////
/*
  Smoothing
  http://www.arduino.cc/en/Tutorial/Smoothing
*/

// Define the number of samples to keep track of. The higher the number, the
// more the readings will be smoothed, but the slower the output will respond to
// the input. Using a constant rather than a normal variable lets us use this
// value to determine the size of the readings array.
const int numReadings = 100;

float readings[numReadings];    // the readings from the analog input
int readIndex = 0;              // the index of the current reading
float total = 0.0;              // the running total
float average = 0.0;            // the average

float Smoothing(float valuetosmooth)
{
  // subtract the last reading:
  total = total - readings[readIndex];
  // read from the sensor:
  readings[readIndex] = valuetosmooth;
  // add the reading to the total:
  total = total + readings[readIndex];
  // advance to the next position in the array:
  readIndex = readIndex + 1;

  // if we're at the end of the array...
  if (readIndex >= numReadings) {
    // ...wrap around to the beginning:
    readIndex = 0;
  }

  // calculate the average:
  average = total / numReadings;
  // send it to the computer as ASCII digits
  delay(1);        // delay in between reads for stability

  return average;
}

/////////////////////////////////////////////////////////////////


#if MOVING_AVERAGE

float vals[SAMPLESIZE];
long sum = 0;
float value;
float ReadSensor()
{
  value = voutToKPa(digitalToVout(analogRead(A2)));

  for (int i = SAMPLESIZE - 1; i > 0; i--)
  {
    vals[i] = vals[i - 1];
  }

  vals[0] = value;

  sum = 0;

  for (int i = 0; i < SAMPLESIZE; i++)
  {
    sum = sum + vals[i];
  }

  return sum / SAMPLESIZE;
}

#else

float diffPressure = 0.0;
int i = 0;
float ReadSensor()
{
  diffPressure = 0.0;

  for (i = 0; i < SAMPLESIZE; i++)
    diffPressure += voutToKPa(digitalToVout(analogRead(A2)));

  return diffPressure / SAMPLESIZE;
}

#endif

bool isSampling = false; //true to plotter
void ListenCommand(char cmd)
{
  //ECHO
  if (cmd == 'e' || cmd == 'E')
    Serial.println("echom");

  //READ SAMPLES
  else if (cmd == 'r' || cmd == 'R')
    isSampling = true;

  //FINISH READ
  else if (cmd == 'f' || cmd == 'F')
  {
    isSampling = false;
    isCalibrated = false;
  }

  //CALIBRATE
  else if (cmd == 'c' || cmd == 'C')
    isCalibrated = false;
}

void setup() 
{ 
  Serial.begin(115200);
}


void loop()
{
#if DEBUG
  isSampling = true;
#endif

  if (Serial.available() > 0)
    ListenCommand(Serial.read());

  if (isSampling && !isCalibrated)
    Calibrate();

  if (isSampling && isCalibrated)
    Serial.println(Smoothing(ReadSensor() - calibrationValue)*1000.0); // *1000.0 = KPa to Pa
}

/**
   Sensor Transformations
   Range 0.2V = 0 kPa to 4.7V = 700.0 kPa
   https://github.com/AdamVStephen/gem-water-level-gauge/blob/master/WaterLevelSensor/WaterLevelSensor.ino
*/

const float VCC = 5.0;
const float MAX_KPA = 700.0;
const float COEFF_LIN_KPA = 0.0012858;
const float COEFF_OFFSET_KPA = 0.04;
// Min Vout 0.2 for standard values above
const float MIN_VOUT = (VCC * COEFF_OFFSET_KPA);
// Max Vout 4.7 for standard values above
const float MAX_VOUT = (VCC * ((COEFF_LIN_KPA * MAX_KPA) + COEFF_OFFSET_KPA));

float voutToKPa(float v)
{
  return (v - MIN_VOUT) / (VCC * COEFF_LIN_KPA);
}

float digitalToVout(long d)
{
  return (VCC * d) / 1023.0;
}

//float voutToPa(float v)
//{
//  return 1000.0 * voutToKPa(v); 
//}
