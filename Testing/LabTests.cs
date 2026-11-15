/****************/
Mã sinh viên: 20237303
Họ tên: Phùng Ngọc Bảo
/****************/

using SmartHomeMini.Contracts;
using SmartHomeMini.Devices;
using SmartHomeMini.Patterns;

namespace SmartHomeMini.Testing;

/// <summary>Kiểm thử console độc lập, không cần xUnit/NUnit và không cần NuGet.</summary>
public static class LabTests
{
    private static int passed;
    private static int failed;

    public static void RunAll()
    {
        passed = 0;
        failed = 0;
        Console.WriteLine("=== LAB W10 | AUTOMATED TESTS ===");

        Run("T1", "Factory trả đúng kiểu light/fan/door", TestFactoryTypes);
        Run("T2", "Loại camera chưa hỗ trợ ném ArgumentException", TestUnsupportedCamera);
        Run("T3", "Hai tham chiếu Singleton trỏ cùng đối tượng", TestSingletonReference);
        Run("T4", "GoToWork: tắt đèn, quạt; khóa cửa", TestGoToWork);
        Run("T5", "GoToSleep: bật đèn; tắt quạt; khóa cửa; in mức sáng", TestGoToSleep);
        Run("T6", "Factory không phân biệt hoa thường, khoảng trắng", TestFactoryNormalization);
        Run("T7", "Chỉ cửa cần hành vi khóa/mở khóa", TestLockableContract);

        Console.WriteLine($"KẾT QUẢ: {passed} PASS / {failed} FAIL / {passed + failed} TESTS");
        if (failed > 0)
            Environment.ExitCode = 1;
    }

    private static void Run(string code, string title, Action action)
    {
        try
        {
            action();
            passed++;
            Console.WriteLine($"[PASS] {code}: {title}");
        }
        catch (Exception ex)
        {
            failed++;
            Console.WriteLine($"[FAIL] {code}: {title} | {ex.GetType().Name}: {ex.Message}");
        }
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static void TestFactoryTypes()
    {
        Ensure(DeviceFactory.Create("light", "Light A") is SmartLight, "light must yield SmartLight");
        Ensure(DeviceFactory.Create("fan", "Fan A") is SmartFan, "fan must yield SmartFan");
        Ensure(DeviceFactory.Create("door", "Door A") is SmartDoor, "door must yield SmartDoor");
    }

    private static void TestUnsupportedCamera()
    {
        try
        {
            DeviceFactory.Create("camera", "Camera A");
        }
        catch (ArgumentException)
        {
            return;
        }

        throw new InvalidOperationException("Camera must throw ArgumentException.");
    }

    private static void TestSingletonReference()
    {
        HomeConfiguration c1 = HomeConfiguration.Instance;
        HomeConfiguration c2 = HomeConfiguration.Instance;
        Ensure(ReferenceEquals(c1, c2), "Singleton returned different references.");
    }

    private static void TestGoToWork()
    {
        IDevice light = DeviceFactory.Create("light", "Test light");
        IDevice fan = DeviceFactory.Create("fan", "Test fan");
        SmartDoor door = (SmartDoor)DeviceFactory.Create("door", "Test door");
        CaptureOutput(() => { light.TurnOn(); fan.TurnOn(); door.Unlock(); });

        var facade = new SmartHomeFacade(light, fan, door);
        CaptureOutput(facade.GoToWork);
        Ensure(!light.IsOn && !fan.IsOn && door.IsLocked, "GoToWork states are invalid.");
    }

    private static void TestGoToSleep()
    {
        IDevice light = DeviceFactory.Create("light", "Test light");
        IDevice fan = DeviceFactory.Create("fan", "Test fan");
        SmartDoor door = (SmartDoor)DeviceFactory.Create("door", "Test door");
        var facade = new SmartHomeFacade(light, fan, door);
        int previousLevel = HomeConfiguration.Instance.NightLightLevel;

        try
        {
            HomeConfiguration.Instance.NightLightLevel = 35;
            CaptureOutput(() => { fan.TurnOn(); door.Unlock(); });
            string result = CaptureOutput(facade.GoToSleep);
            Ensure(light.IsOn && !fan.IsOn && door.IsLocked, "GoToSleep states are invalid.");
            Ensure(result.Contains("Night light level: 35%", StringComparison.Ordinal),
                "GoToSleep did not display configured night-light level.");
        }
        finally
        {
            // Tránh làm thay đổi cấu hình chung sau khi kết thúc test.
            HomeConfiguration.Instance.NightLightLevel = previousLevel;
        }
    }

    private static void TestFactoryNormalization()
    {
        IDevice light = DeviceFactory.Create("  LiGhT  ", "Desk light");
        Ensure(light is SmartLight, "Factory did not normalize input.");
    }

    private static void TestLockableContract()
    {
        IDevice light = DeviceFactory.Create("light", "Test light");
        IDevice fan = DeviceFactory.Create("fan", "Test fan");
        IDevice door = DeviceFactory.Create("door", "Test door");
        Ensure(light is not ILockable && fan is not ILockable, "Only door should be lockable.");
        Ensure(door is ILockable, "SmartDoor must implement ILockable.");
        var lockableDoor = (ILockable)door;
        CaptureOutput(() => { lockableDoor.Lock(); lockableDoor.Unlock(); });
        Ensure(!lockableDoor.IsLocked, "Unlock failed.");
    }

    private static string CaptureOutput(Action action)
    {
        TextWriter original = Console.Out;
        using var buffer = new StringWriter();
        try
        {
            Console.SetOut(buffer);
            action();
            return buffer.ToString();
        }
        finally
        {
            Console.SetOut(original);
        }
    }
}
