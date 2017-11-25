using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Date : object
{
    private int Day;
    private int Month;
    private int Year;
    private Days DayOfWeek;

    public Date(int day, int month, int year)
    {
        Day = day + 1;
        Month = month + 1;
        Year = year;
        DayOfWeek = CalculateDayOfTheWeek(this);
    }
    public static Days CalculateDayOfTheWeek(Date date)
    {
        return (Days)((date.GetDay() + Mathf.Floor((float)((2.6 * date.GetMonth()) - .2) - (2 * (date.GetYear() / 100))) + date.GetYear() + (date.GetYear() / 4) + ((date.GetYear() / 100) / 4)) % 7);
    }
    public static string DetermineDateSuffix(int day)
    {
        switch (day)
        {
            case 1:
                return "st";
            case 2:
                return "nd";
            case 3:
                return "rd";
            default:
                return "th";
        }
    }
    public static string GetMonthName(int month)
    {
        Months monthName = (Months)month;
        return monthName.ToString();
    }
    #region Getters
    public int GetDay(bool absolute = false)
    {
        if(absolute)
        {
            return Day;
        }
        else
        {
            return (Day % 7) + 1;
        }
    }
    public int GetMonth()
    {
        return Month;        
    }
    public int GetYear()
    {
        return Year;
    }
    #endregion
    #region ObjectOverrides
    public override string ToString()
    {
        return Day / Month + "/" + Month + "/" + Year;
    }
    public string ToString(bool verbose)
    {
        if(verbose)
        {
            return DayOfWeek + " the " + GetDay() + DetermineDateSuffix(GetDay()) + " of " + GetMonthName(Month) + ", " + Year + "AD";
        }
        else
        {
            return ToString();
        }
    }
    public override bool Equals(object obj)
    {
        if(obj == null)
        {
            return false;
        }
        else if(obj is Date)
        {
            Date dateobj = (Date)obj;
            return Day == dateobj.GetDay() && Month == dateobj.GetMonth() && Year == dateobj.GetYear(); 
        }
        else
        {
            return false;
        }
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion
}

public enum Months
{
    January,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December
}
public enum Days
{
    Sunday,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday
}