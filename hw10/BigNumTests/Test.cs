using NUnit.Framework;
using System;
using CS422;

namespace BigNumTests
{
  [TestFixture]
  public class Test
  {
    private static string _validNumberStringPlain = "100";
    private static string _validNumberStringWithMinus = "-100";
    private static string _validNumberStringWithDecimal = "100.100";
    private static string _validNumberStringWithAll = "-100.333";

    private static string[] _validConstructorStrings = {
      _validNumberStringPlain,
      _validNumberStringWithDecimal,
      _validNumberStringWithMinus,
      _validNumberStringWithAll
    };

    private static string _emptyString = "";
    private static string _invalidNumberStringWithMinusInWrongSpot = "1-00";
    private static string _invalidNumberStringWithTooManyminuses = "--1000";
    private static string _invalidNumberStringTwoDecimals = ".100.333";
    private static string _invalidNumberStringTwoDecimalsOneAtEnd = "100.333.";
    private static string _invalidNumberStringNonDigitInString = "100a00";

    private static string[] _invalidConstructorStrings = {
      _emptyString,
      _invalidNumberStringWithMinusInWrongSpot,
      _invalidNumberStringWithTooManyminuses,
      _invalidNumberStringTwoDecimals,
      _invalidNumberStringTwoDecimalsOneAtEnd,
      _invalidNumberStringNonDigitInString
    };

    [Test]
    public void SingleParamConstructorWithValidString ()
    {
      foreach (string s in _validConstructorStrings)
      {
        BigNum num = new BigNum (s);
        Assert.NotNull (num);
      }
    }

    [Test]
    public void SingleParamConstructorWithInvalidString ()
    {
      foreach (string s in _invalidConstructorStrings)
      {
        Assert.Throws<ArgumentException> (() => new BigNum (s));
      }
    }

    [Test]
    public void IsValidNumberStringWithValidStrings ()
    {
      bool result = false;
      foreach (string s in _validConstructorStrings)
      {        
        result = BigNum.IsValidNumberString (s);
        Assert.IsTrue (result);
      }
    }

    [Test]
    public void IsValidNumberStringWithInvalidStrings ()
    {
      bool result = true;
      foreach (string s in _invalidConstructorStrings)
      {        
        Console.WriteLine ("s = " + s);
        result = BigNum.IsValidNumberString (s);
        Assert.IsFalse (result);
      }
    }

  }
}

