namespace Lab1;

public interface ISmartphone
{
    string Model { get; }
    string Color { get; }
    string OS { get; }
    string Processor { get; }
    int RAM { get; }

    void ShowInfo();
}