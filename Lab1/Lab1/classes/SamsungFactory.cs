namespace Lab1;

public class SamsungFactory : PhoneFactory
{
    string model;
    string color;
    string os;
    string processor;
    int ram;

    public SamsungFactory(string model, string color, string os, string processor, int ram)
    {
        this.model = model;
        this.color = color;
        this.os = os;
        this.processor = processor;
        this.ram = ram;
    }

    public override ISmartphone CreatePhone()
    {
        return new Samsung(model, color, os, processor, ram);
    }
}