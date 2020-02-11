/*
 * Pitaco Serial Connection - MPX5010DP
 * https://github.com/huenato/iblueit
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
		sum += voutToPa(digitalToVout(analogRead(A2)));

	calibrationValue = sum / SAMPLESIZE;
#endif  
	
	isCalibrated = true;
}

#if MOVING_AVERAGE

float vals[SAMPLESIZE];
long sum = 0;
float value;
float ReadSensor()
{
	value = voutToPa(digitalToVout(analogRead(A2)));

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
		diffPressure += voutToPa(digitalToVout(analogRead(A2)));

	return diffPressure / SAMPLESIZE;
}

#endif

bool isSampling = false;
void ListenCommand(char cmd)
{
	//ECHO
	if (cmd == 'e' || cmd == 'E')
		Serial.println("echo");

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
		Serial.println(ReadSensor() - calibrationValue);
}

/**
   Sensor Transformations
   Range 0.2V = 0 kPa to 4.7V = 10.0 kPa
   https://github.com/AdamVStephen/gem-water-level-gauge/blob/master/WaterLevelSensor/WaterLevelSensor.ino
*/

const float VCC = 5.0;
const float MAX_KPA = 10.0;
const float COEFF_LIN_KPA = 0.09;
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

float voutToPa(float v)
{
	return 1000.0 * voutToKPa(v);
}
