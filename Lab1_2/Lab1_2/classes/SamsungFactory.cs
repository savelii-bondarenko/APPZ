namespace Lab1;

public class SamsungFactory : PhoneFactory
{
    private readonly string _model;
    private readonly string _color;
    private readonly string _os;
    private readonly string _processor;
    private readonly int _ram;

    public SamsungFactory(string model, string color, string os, string processor, int ram)
    {
        _model = model;
        _color = color;
        _os = os;
        _processor = processor;
        _ram = ram;
    }

    public override ISmartphone CreatePhone()
    {
        return new Samsung(_model, _color, _os, _processor, _ram);
    }
}