namespace SmartX.Api.Models;

public readonly struct SensorValue
{
    public double Value { get; }

    public string Unit { get; }

    public SensorValue(double value, string unit)
    {
        Value = value;
        Unit = unit;
    }

    public static SensorValue operator +(
        SensorValue left,
        SensorValue right)
    {
        ValidateUnits(left, right);

        return new SensorValue(
            left.Value + right.Value,
            left.Unit);
    }

    public static SensorValue operator -(
        SensorValue left,
        SensorValue right)
    {
        ValidateUnits(left, right);

        return new SensorValue(
            left.Value - right.Value,
            left.Unit);
    }

    public static SensorValue operator *(
        SensorValue value,
        double multiplier)
    {
        return new SensorValue(
            value.Value * multiplier,
            value.Unit);
    }

    private static void ValidateUnits(
        SensorValue left,
        SensorValue right)
    {
        if (left.Unit != right.Unit)
        {
            throw new InvalidOperationException(
                "Sensor values must use the same unit.");
        }
    }

    public override string ToString()
    {
        return $"{Value:F2} {Unit}";
    }
}