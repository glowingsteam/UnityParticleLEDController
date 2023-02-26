#include "LightController.h"
#include "Particle.h"

#define PIXEL_PIN A0
#define PIXEL_COUNT 300
#define PIXEL_TYPE WS2812B

#define LAST_ANIM_INDEX 3

Adafruit_NeoPixel Strip(PIXEL_COUNT, PIXEL_PIN, PIXEL_TYPE);
LightObject Controller(&Strip);

int SetIntensity(String input);
int SetChannel1(String Input);
int SetChannel2(String Input);
int SetSpeed(String Input);
int SetAnimation(String Input);

String Channel1 = "";
String Channel2 = "";
double CurrentIntensity = 100;
double CurrentSpeed = 50;
int32_t CurrentAnimation = 0;
bool LightsOn = false;

unsigned long LastTick = 0;

void setup() {
    Controller.Begin();
    
    Particle.function("SetChannel1", SetChannel1);
    Particle.function("SetChannel2", SetChannel2);
    Particle.function("SetIntensity", SetIntensity);
    Particle.function("SetSpeed", SetSpeed);
    Particle.function("SetAnimation", SetAnimation);
    Particle.function("Power", SetPower);

    Channel1 = Controller.GetChannel1().GetHexCode();
    Channel2 = Controller.GetChannel2().GetHexCode();
    CurrentIntensity = Controller.GetIntensity();
    CurrentSpeed = Controller.GetSpeed();
    CurrentAnimation = Controller.GetAnimation();
    LightsOn = Controller.GetPower();
    
    Particle.variable("Channel1", Channel1);
    Particle.variable("Channel2", Channel2);
    Particle.variable("Intensity", CurrentIntensity);
    Particle.variable("Speed", CurrentSpeed);
    Particle.variable("Animation", CurrentAnimation);
    Particle.variable("Power", LightsOn);
}

void loop() {
    float deltaTime = (float)(millis()-LastTick) / 1000.0f;
    
    Controller.Tick(deltaTime);
    
    LastTick = millis();
}

int SetChannel1(String Input)
{
    Controller.SetColor1(Input);
    Channel1 = Controller.GetChannel1().GetHexCode();
    
    return 1;
}

int SetChannel2(String Input)
{
    Controller.SetColor2(Input);
    Channel2 = Controller.GetChannel2().GetHexCode();
    
    return 1;
}

int SetIntensity(String Input)
{
    float Intensity = constrain(Input.toFloat(), 0.0f, 1.0f);
    Controller.SetIntensity(Intensity);
    CurrentIntensity = Controller.GetIntensity();
    
    return 1;
}

int SetSpeed(String Input)
{
    float Speed = constrain(Input.toFloat(), 0.0f, 1.0f);
    Controller.SetSpeed(Speed);
    CurrentSpeed = Controller.GetSpeed();
    
    return 1;
}

int SetAnimation(String Input)
{
    uint8_t Anim = constrain(Input.toInt(), 0, LAST_ANIM_INDEX);
    Controller.SetAnimation(Anim);
    CurrentAnimation = (uint8_t)Controller.GetAnimation();
    
    return 1;
}

int SetPower(String Input)
{
    LightsOn = constrain(Input.toInt(), 0, 1) == 1;
    Controller.SetPower(LightsOn);
    
    return 1;
}