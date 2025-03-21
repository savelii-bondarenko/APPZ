namespace Lab1_3.Models;

public class GameRequirements
{
    public GameRequirements(double minProcessorGHz, int minRamGB, int minVideoRamGB, int minHddGB)
    {
        MinProcessorGHz = minProcessorGHz;
        MinRamGB = minRamGB;
        MinVideoRamGB = minVideoRamGB;
        MinHddGB = minHddGB;
    }

    public double MinProcessorGHz { get; set; }
    public int MinRamGB { get; set; }
    public int MinVideoRamGB { get; set; }
    public int MinHddGB { get; set; }

    public override string ToString()
    {
        return $"Процесор: {MinProcessorGHz} GHz, RAM: {MinRamGB} GB, VRAM: {MinVideoRamGB} GB, HDD: {MinHddGB} GB";
    }
}