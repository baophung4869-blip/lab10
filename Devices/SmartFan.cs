/****************/
Mã sinh viên: 20237303
Họ tên: Phùng Ngọc Bảo
/****************/

using SmartHomeMini.Contracts;

namespace SmartHomeMini.Devices;

/// <summary>Quạt thông minh hỗ trợ bật/tắt.</summary>
public sealed class SmartFan : IDevice
{
    public string Name { get; }
    public bool IsOn { get; private set; }

    public SmartFan(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên quạt không được để trống.", nameof(name));

        Name = name.Trim();
    }

    public void TurnOn()
    {
        IsOn = true;
        Console.WriteLine($"{Name}: ON");
    }

    public void TurnOff()
    {
        IsOn = false;
        Console.WriteLine($"{Name}: OFF");
    }
}
