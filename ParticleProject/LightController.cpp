#include <LightController.h>

LightObject::LightObject()
{
    Strip = nullptr;
}

LightObject::LightObject(Adafruit_NeoPixel* InStrip)
{
    Strip = InStrip;
}
   
LightObject::~LightObject()
{
}
    
void LightObject::Begin()
{
    if (Strip == nullptr)
        return;

    Strip->begin();
    Strip->show();
    
    SetColor1("FF00FF");
    SetColor2("00FFFF");
}
    
void LightObject::Tick(float DeltaTime)
{
    if (Strip == nullptr)
        return;
    
    TickCounter += DeltaTime;
    
    if (!Power)
    {
        DoSolidTick();
        return;
    }
    
    switch (CurrentAnimation)
    {
        case AnimationType::Solid:
            DoSolidTick();
            break;
        case AnimationType::Dots:
            DoDotTick();
            break;
        case AnimationType::Polka:
            DoPolkaTick();
            break;
        case AnimationType::Rainbow:
            DoRainbowTick();
            break;
    }
}

void LightObject::Update()
{
    UpdateCurrentSpeed();
    CurrentIterator = 0;
    return;
}

Color LightObject::GetChannel1()
{
    return Channel1Color;
}

Color LightObject::GetChannel2()
{
    return Channel2Color;
}

void LightObject::SetColor1(String HexCode)
{
    Channel1Color.SetColor(HexCode);
    Update();
}

void LightObject::SetColor2(String HexCode)
{
    Channel2Color.SetColor(HexCode);
    Update();
}

const int32_t LightObject::GetAnimation()
{
    return CurrentAnimation;
}

void LightObject::SetAnimation(uint8_t NewAnim)
{
    CurrentAnimation = (AnimationType)NewAnim;
    Update();
    
    String animName = "";// = String(CurrentAnimation);
    animName = CurrentAnimation;
    Particle.publish("New Animation: ", animName);
}

void LightObject::SetIntensity(float newIntensity)
{
    Intensity = (double)newIntensity;
    Update();
}

const double LightObject::GetIntensity()
{
    return Intensity;
}

void LightObject::SetSpeed(float value)
{
    AnimationSpeed = (double)value;
    UpdateCurrentSpeed();
}

const double LightObject::GetSpeed()
{
    return AnimationSpeed;
}

const bool LightObject::GetPower()
{
    return Power;
}

void LightObject::SetPower(bool newPower)
{
    Power = newPower;
    
    Update();
}

void LightObject::SetLightsStatic(Color c)
{
    uint8_t r = constrain(round((float)c.r * (Intensity * MaxBrightness)), 0, 255);
    uint8_t g = constrain(round((float)c.g * (Intensity * MaxBrightness)), 0, 255);
    uint8_t b = constrain(round((float)c.b * (Intensity * MaxBrightness)), 0, 255);
    
    for (int i = 0; i < Strip->numPixels(); i++)
    {
        Strip->setPixelColor(i, Strip->Color(r, g, b));
        Strip->show();
    }
}

void LightObject::Rainbow(uint8_t wait) {
  uint16_t i, j;

  for(j=0; j<256; j++) {
    for(i=0; i<Strip->numPixels(); i++) {
      Strip->setPixelColor(i, Wheel((i+j) & 255));
    }
    
    Strip->show();
    delay(wait);
  }
}

uint32_t LightObject::Wheel(byte WheelPos) {
    float n = (Intensity * MaxBrightness);
    
    if(WheelPos < 85) {
        return Strip->Color(round(n * WheelPos * 3), round(n * (255 - WheelPos * 3)), 0);
    } else if(WheelPos < 170) {
        WheelPos -= 85;
        return Strip->Color(round(n * (255 - WheelPos * 3)), 0,round(n * WheelPos * 3));
    } else {
        WheelPos -= 170;
        return Strip->Color(0, round(n * WheelPos * 3), round(n * (255 - WheelPos * 3)));
    }
}

void LightObject::DoSolidTick()
{
    if (CurrentIterator >= Strip->numPixels())
        return;
    
    if (TickCounter > 0.002f)
    {
        //Particle.publish("Ticking", (String)CurrentIterator);
        
        Color c = Power ? Channel1Color : Color(0,0,0);
        
        SetPixelWithIntensity(CurrentIterator, c);
        Strip->show();
        
        TickCounter -= 0.002f;
        CurrentIterator++;
    }
}

void LightObject::DoDotTick()
{
    if (TickCounter > CurrentSpeed)
    {
        Color c;

        for (int i = 0; i < Strip->numPixels(); i++)
        {
            int curVal = i - CurrentIterator;
            
            if (curVal < 0)
                c = Channel1Color;
            else if (curVal == 0)
                c = Channel2Color;
            else
                c = curVal % 10 == 0 ? Channel2Color : Channel1Color;
                
                
            SetPixelWithIntensity(i, c);
        }
        
        Strip->show();
        
        TickCounter -= CurrentSpeed;
        CurrentIterator++;
    }
    
    // % 10 will fail if at the start
    if (CurrentIterator >= 10)
        CurrentIterator -= 10;
}

void LightObject::DoPolkaTick()
{
    if (CurrentIterator >= Strip->numPixels())
        return;
    
    if (TickCounter > CurrentSpeed)
    {
        //Particle.publish("Ticking", (String)CurrentIterator);
        Color c;
        
        if (Power)
            c = CurrentIterator % 2 == 0 ? Channel1Color : Channel2Color;
        else
            c = Color(0, 0, 0);
        
        SetPixelWithIntensity(CurrentIterator, c);
        Strip->show();
        
        TickCounter -= CurrentSpeed;
        CurrentIterator++;
    }
}

void LightObject::DoRainbowTick()
{
    if (CurrentIterator >= 256)
        CurrentIterator -= 256;
        
    if (TickCounter > CurrentSpeed)
    {
        for(int i=0; i<Strip->numPixels(); i++) {
          Strip->setPixelColor(i, Wheel((i+CurrentIterator) & 255));
        }
        
        Strip->show();
        
        TickCounter -= CurrentSpeed;
        CurrentIterator++;
    }
}

void LightObject::UpdateCurrentSpeed()
{
    float currentMaxSpeed = 0.01f;
    switch (CurrentAnimation)
    {
        case AnimationType::Solid:
            currentMaxSpeed = SolidSpeed;
            break;
        case AnimationType::Dots:
            currentMaxSpeed = DotsSpeed;
            break;
        case AnimationType::Polka:
            currentMaxSpeed = PolkaSpeed;
            break;
        case AnimationType::Rainbow:
            currentMaxSpeed = RainbowSpeed;
            break;
    };
    
    CurrentSpeed = ((currentMaxSpeed - MinSpeed) * (1.0f - AnimationSpeed)) + MinSpeed;
    
    Particle.publish("New Speed", String(CurrentSpeed));
}

void LightObject::SetPixelWithIntensity(int i, Color c)
{
    uint8_t r = constrain(round((float)c.r * (Intensity * MaxBrightness)), 0, 255);
    uint8_t g = constrain(round((float)c.g * (Intensity * MaxBrightness)), 0, 255);
    uint8_t b = constrain(round((float)c.b * (Intensity * MaxBrightness)), 0, 255);
    
    Strip->setPixelColor(i, Strip->Color(r, g, b));
}