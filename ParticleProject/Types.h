enum AnimationType : uint8_t
{
    Solid = 0,
    Dots = 1,
    Polka = 2,
    Rainbow = 3
};

struct Color
{
public:
    Color(){ }
    
    Color(uint8_t x, uint8_t y, uint8_t z) {SetColor(x,y,z); }

    uint8_t r = 0;
    uint8_t g = 0;
    uint8_t b = 0;
    
    void SetColor(uint8_t x, uint8_t y, uint8_t z) { r=x; g=y; b=z; }
    
    uint32_t GetHex()
    {
        return ((r & 0xff) << 16) + ((g & 0xff) << 8) + (b & 0xff);
    }
    
    String GetHexCode()
    {
        char hexcol[16];

        snprintf(hexcol, sizeof hexcol, "%02x%02x%02x", r, g, b);
        
        return String(hexcol);
    }
    
    void SetColor(String HexCode)
    {
        sscanf(HexCode, "%02x%02x%02x", &r, &g, &b);
    }
};