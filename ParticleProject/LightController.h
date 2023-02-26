#include <neopixel.h>
#include "Types.h"

class LightObject
{
public:
    LightObject();
    LightObject(Adafruit_NeoPixel* InStrip);
    ~LightObject();
    
public:
    void Begin();
    void Tick(float DeltaTime);
    void Update();
    
    Color GetChannel1();
    Color GetChannel2();
    void SetColor1(String HexCode);
    void SetColor2(String HexCode);
    
    const int32_t GetAnimation();
    void SetAnimation(uint8_t NewAnim);
    
    void SetIntensity(float newIntensity);
    const double GetIntensity();
    
    void SetSpeed(float value);
    const double GetSpeed();
    
    const bool GetPower();
    void SetPower(bool newPower);
    
    void SetLightsStatic(Color c);
    
private:
    Adafruit_NeoPixel* Strip;
    AnimationType CurrentAnimation = AnimationType::Solid;
    
    Color Channel1Color;
    Color Channel2Color;
    
    double Intensity = 1.0f;
    double AnimationSpeed = 1.0f;
    
    bool Power = false;
    
    const double MaxBrightness = 0.8f;
    
    const float SolidSpeed = 0.2f;
    const float RainbowSpeed = 0.1f;
    const float PolkaSpeed = 0.2f;
    const float DotsSpeed = 0.5f;
    const float MinSpeed = 0.001f;
    
    float CurrentSpeed = 0.004f;
    
    int CurrentIterator = 0;
    
    float TickCounter = 0.0f;
    
private:
    void DoSolidTick();
    void DoRainbowTick();
    void DoDotTick();
    void DoPolkaTick();
    
    void Rainbow(uint8_t wait);
    uint32_t Wheel(byte WheelPos);
    
    void UpdateCurrentSpeed();
    
    void SetPixelWithIntensity(int i, Color c);
};