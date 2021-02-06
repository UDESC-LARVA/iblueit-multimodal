/*
 * Cinta Serial Connection - Force Sensitive Resistor 0.5″ FSR402
 * Crédito: Jhonatan Thallisson Cabral Néry
 * https://github.com/jhonatantcn/IBLUEIT
 */
 
/* 
 *  Anotações:
 *  Intervalos típicos de medida do sensor: 0 a 1023, ou 100g a 10kg(não linear).
 *  Quanto mais pressão aplicar no sensor, menor a resistência e maior a tensão.
 *  A faixa de resistência é: > 10MΩ (sem pressão) até ~ 200Ω (pressão máxima).
*/

/* 
 *  Credits: (Codes that helped)
 *  FSR - https://www.instructables.com/id/FSR-Tutorial/
 *  Sensor Transformations - http://www.ladyada.net/learn/sensors/fsr.html and https://www.mouser.com/datasheet/2/737/force-sensitive-resistor-fsr-932871.pdf
 *     
*/

#define SAMPLESIZE 100 //Block for average readings
#define FSR_PIN A0 // the FSR and 10K pulldown are connected to a0
#define MOVING_AVERAGE true
#define DEBUG false

bool isCalibrated = true; // original: isCalibrated = false
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
    sum += ForceNewtonToPascal(CondutanceToForceNewton(ResistanceToCondutance(voutToResistance(digitalToVout(Smoothing())))));

  calibrationValue = sum / SAMPLESIZE;
#endif  
  
  isCalibrated = true;
}

//////////////////// SMOOTHING /////////////////////////////////////////////
/*
  Smoothing
  http://www.arduino.cc/en/Tutorial/Smoothing
*/

// Define the number of samples to keep track of. The higher the number, the
// more the readings will be smoothed, but the slower the output will respond to
// the input. Using a constant rather than a normal variable lets us use this
// value to determine the size of the readings array.
const int numReadings = 20;

float readings[numReadings];      // the readings from the analog input
int readIndex = 0;              // the index of the current reading
float total = 0;                  // the running total
float average = 0;                // the average

//int inputPin = A0;

float Smoothing()
{
  // subtract the last reading:
  total = total - readings[readIndex];
  // read from the sensor:
  readings[readIndex] = analogRead(FSR_PIN);
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
  
  //Serial.print("Smoothing: ");
  //Serial.println(average);
  delay(1);        // delay in between reads for stability

  return average;
}

/////////////////////////////////////////////////////////////////


#if MOVING_AVERAGE

float vals[SAMPLESIZE];
float sum = 0.0;
float value;
float ReadSensor()
{
  value = ForceNewtonToPascal(CondutanceToForceNewton(ResistanceToCondutance(voutToResistance(digitalToVout(Smoothing())))));

  for (int i = SAMPLESIZE - 1; i > 0; i--)
  {
    vals[i] = vals[i - 1];
  }

  vals[0] = value;

  sum = 0.0;

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
    diffPressure += ForceNewtonToPascal(CondutanceToForceNewton(ResistanceToCondutance(voutToResistance(digitalToVout(Smoothing())))));

  return diffPressure / SAMPLESIZE;
}

#endif

bool isSampling = false; // Change to "true" to run on Plotter Serial.
void ListenCommand(char cmd)
{
  //ECHO
  if (cmd == 'e' || cmd == 'E')
    Serial.println("echoc");

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

  // initialize all the readings to 0 (SMOOTHING):
  for (int thisReading = 0; thisReading < numReadings; thisReading++) {
    readings[thisReading] = 0;
  }
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
    Serial.println((ReadSensor() - calibrationValue)*-1); //
}

/*
 * Sensor Transformations
*/

const float VCC = 5000.0; // Measured voltage of Ardunio 5V line
const float RESISTOR = 10000.0; // Measured resistance of 10k resistor
const float area = 0.0127; // Active Area of sensor: 12.7mm, or 0.0127m - Datasheet FSR402

float digitalToVout(long fsrReading) //fsrVoltage
{
  return map(fsrReading, 0, 1023, 0, VCC);
}

float voutToResistance(float v)
{
  unsigned long fsrResistance;
  fsrResistance = VCC - v;     // fsrVoltage is in millivolts so 5V = 5000mV
  fsrResistance *= RESISTOR;                // 10K resistor
  fsrResistance /= v;
  
  return fsrResistance;
}

float ResistanceToCondutance(float r)
{
  unsigned long fsrConductance;
  fsrConductance = 1000000.0;           // we measure in micromhos so 
  fsrConductance /= r;
      
  return fsrConductance;
}

float CondutanceToForceNewton(float c)
{
  float fsrForceNewtons;
  if (c <= 1000){
        fsrForceNewtons = c / 80.0;
  }else{
        fsrForceNewtons = c - 1000.0;
        fsrForceNewtons /= 30.0;
  }
  
  return fsrForceNewtons;
}

float ForceNewtonToPascal(float fn)
{
  return fn / area; // http://www.sengpielaudio.com/calculator-pressureunits.htm
}
