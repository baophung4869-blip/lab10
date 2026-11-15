/****************/
Mã sinh viên: 20237303
Họ tên: Phùng Ngọc Bảo
/****************/

using SmartHomeMini.Contracts;

namespace SmartHomeMini.Devices;

/// <summary>Đèn thông minh hỗ trợ bật/tắt.</summary>
public sealed class SmartLight : IDevice
{
    public string Name { get; }
    public bool IsOn { get; private set; }

    public SmartLight(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên đèn không được để trống.", nameof(name));

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
