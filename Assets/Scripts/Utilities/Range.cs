using System.Collections.Generic;

public class Range : object
{
    private int MinValue;
    private int MaxValue;

    public Range(int minValue, int maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }
    public Range(string stringToConvert)
    {
        string[] values = stringToConvert.Split(new char[] {' '});
        MinValue = int.Parse(values[0]);
        MaxValue = int.Parse(values[2]);
    }
    public bool IsInRange(int value)
    {
        return MinValue <= value && value <= MaxValue;
    }
    public bool IsInRange(float value)
    {
        return MinValue <= value && value <= MaxValue;
    }
    public int DistanceOutside(int value)
    {
        if(IsInRange(value))
        {
            return 0;
        }
        if(value < MinValue)
        {
            return MinValue - value;
        }
        else
        {
            return value - MaxValue;
        }
    }
    #region Getters
    public int GetMinValue()
    {
        return MinValue;
    }
    public int GetMaxValue()
    {
        return MaxValue;
    }
    #endregion
    #region ObjectOverrides
    public static bool operator <(Range range, object obj )
    {
        if (obj == null)
        {
            return false;
        }
        else if (obj is int)
        {
            return (int)obj > range.MinValue;
        }
        else if (obj is float)
        {
            return (float)obj > range.MinValue;
        }
        else if (obj is Range)
        {
            Range rangeObj = obj as Range;
            return rangeObj.MinValue > range.MinValue;
        }
        else
        {
            return false;
        }
    }
    public static bool operator <(object obj, Range range)
    {
        return range > obj;
    }
    public static bool operator >(Range range, object obj)
    {
        if(obj == null)
        {
            return false;
        }
        else if (obj is int)
        {
            return (int)obj < range.MaxValue;
        }
        else if (obj is float)
        {
            return (float)obj < range.MaxValue;
        }
        else if (obj is Range)
        {
            Range rangeObj = obj as Range;
            return rangeObj.MaxValue < range.MaxValue;
        }
        else
        {
            return false;
        }
    }
    public static bool operator >(object obj, Range range)
    {
        return range < obj;
    }
    public static bool operator >=(Range range, object obj)
    {
        return range > obj || range == obj;
    }
    public static bool operator >=(object obj, Range range)
    {
        return range < obj || range == obj;
    }
    public static bool operator <=(Range range, object obj)
    {
        return range < obj || range == obj;
    }
    public static bool operator <=(object obj, Range range)
    {
        return range > obj || range == obj;
    }
    public static bool operator ==(Range range, object obj)
    {
        return range.Equals(obj);
    }
    public static bool operator !=(Range range, object obj)
    {
        return !range.Equals(obj);
    }
    public static bool operator ==(object obj, Range range)
    {
        return range.Equals(obj);
    }
    public static bool operator !=(object obj, Range range)
    {
        return !range.Equals(obj);
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        else if (obj is int)
        {
            return IsInRange((int)obj);
        }
        else if (obj is float)
        {
            return IsInRange((float)obj);
        }
        else if (obj is Range)
        {
            Range rangeObj = obj as Range;
            return (rangeObj.MinValue >= MinValue && rangeObj.MaxValue <= MaxValue);
        }
        else
        {
            return false;
        }
    }
    public static explicit operator Range(int i)
    {
        return new Range(i, i);
    }
    public override string ToString()
    {
        return MinValue + " - " + MaxValue;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion
}

public class RangeComparer : IEqualityComparer<Range>
{
    public bool Equals(Range lhs, Range rhs)
    {
        if (lhs.Equals(rhs) || rhs.Equals(lhs))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetHashCode(Range obj)
    {
        return 0;
    }
}
