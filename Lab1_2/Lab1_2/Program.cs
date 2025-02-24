namespace Lab1;

class Program
{
    static void Main(string[] args)
    {
        CreateIPhone("iPhone 15 Pro", "Silver", "iOS 17", "A17 Bionic", 8);

        CreateSamsungPhone("Samsung Galaxy S22", "Black", "Android", "Exynos 9910", 12);
    }

    private static void CreateIPhone(string model, string color, string os, string processor, int ram)
    {
        IPhoneFactory factory = new IPhoneFactory(model, color, os, processor, ram);
        ISmartphone phone = factory.CreatePhone();
        phone.ShowInfo();
    }

    private static void CreateSamsungPhone(string model, string color, string os, string processor, int ram)
    {
        SamsungFactory factory = new SamsungFactory(model, color, os, processor, ram);
        ISmartphone phone = factory.CreatePhone();
        phone.ShowInfo();
    }
}