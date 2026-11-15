/****************/
Mã sinh viên: 20237303
Họ tên: Phùng Ngọc Bảo
/****************/

using SmartHomeMini.Contracts;

namespace SmartHomeMini.Devices;

/// <summary>Cửa thông minh: trạng thái nguồn và trạng thái khóa độc lập.</summary>
public sealed class SmartDoor : IDevice, ILockable
{
    public string Name { get; }
    public bool IsOn { get; private set; }
    public bool IsLocked { get; private set; }

    public SmartDoor(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên cửa không được để trống.", nameof(name));

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

    public void Lock()
    {
        IsLocked = true;
        Console.WriteLine($"{Name}: LOCKED");
    }

    public void Unlock()
    {
        IsLocked = false;
        Console.WriteLine($"{Name}: UNLOCKED");
    }
}
