namespace Lab1;

public class Samsung : ISmartphone
{
    public string Model { get; }
    public string Color { get; }
    public string OS { get; }
    public string Processor { get; }
    public int RAM { get; }

    public Samsung(string model, string color, string os, string processor, int ram)
    {
        Model = model;
        Color = color;
        OS = os;
        Processor = processor;
        RAM = ram;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Model: {Model}\nColor: {Color}\nOS: {OS}\nProcessor: {Processor}\nRAM: {RAM}");
    }
}