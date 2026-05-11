using MtgInventoryManagementApi.MtgInventoryManagement.Service.Providers;
using Shouldly;

namespace MtgInventoryManagement.Service.Tests.Providers;

public class DateTimeProviderTest
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public DateTimeProviderTest()
    {
        _dateTimeProvider = new DateTimeProvider();
    }

    [Fact]
    public void Now_ShouldReturnCurrentDateTime()
    {
        var result = _dateTimeProvider.Now;

        result.ShouldBe(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UtcNow_ShouldReturnCurrentUtcDateTime()
    {
        var result = _dateTimeProvider.UtcNow;

        result.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Today_ShouldReturnCurrentDate()
    {
        var result = _dateTimeProvider.Today;

        result.ShouldBe(DateTime.Today);
    }
}
