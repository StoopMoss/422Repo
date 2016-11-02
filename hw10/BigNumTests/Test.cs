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


    ////////////////////////////////////////////////////////////
    /// The following tests validate the initialization function
    /// of the BigNum class
    [Test]
    public void InitializeWithPlainValue ()
    {
      BigNum num = new BigNum ();
      num.Initialize ("100");
      Console.WriteLine (num.Mantissa.ToString ());
      Assert.IsTrue (num.Mantissa.Equals (100));
      Assert.AreEqual (0, num.Exponent);
      Assert.AreEqual (false, num.Sign);
    }

    [Test]
    public void InitializeWithNegativeValue ()
    {
      BigNum num = new BigNum ();
      num.Initialize ("-100");
      Assert.IsTrue (num.Mantissa.Equals (100));
      Assert.AreEqual (0, num.Exponent);
      Assert.AreEqual (true, num.Sign);
    }

    [Test]
    public void InitializeWithDecimalValue ()
    {
      BigNum num = new BigNum ();
      num.Initialize ("100.111");
      Assert.IsTrue (num.Mantissa.Equals (100111));
      Assert.AreEqual (-3, num.Exponent);
      Assert.AreEqual (false, num.Sign);

      num.Initialize ("10011.1");
      Assert.IsTrue (num.Mantissa.Equals (100111));
      Assert.AreEqual (-1, num.Exponent);
      Assert.AreEqual (false, num.Sign);

      num.Initialize (".100111");
      Assert.IsTrue (num.Mantissa.Equals (100111));
      Assert.AreEqual (-6, num.Exponent);
      Assert.AreEqual (false, num.Sign);
    }

    [Test]
    public void InitializeWithNegativeDecimalValue ()
    {
      BigNum num = new BigNum ();
      num.Initialize ("-100.111");
      Assert.IsTrue (num.Mantissa.Equals (100111));
      Assert.AreEqual (-3, num.Exponent);
      Assert.AreEqual (true, num.Sign);

      num.Initialize ("-10011.1");
      Assert.IsTrue (num.Mantissa.Equals (100111));
      Assert.AreEqual (-1, num.Exponent);
      Assert.AreEqual (true, num.Sign);

      num.Initialize ("-.100111");
      Assert.IsTrue (num.Mantissa.Equals (100111));
      Assert.AreEqual (-6, num.Exponent);
      Assert.AreEqual (true, num.Sign);

    }

    [Test]
    public void InitializeWithLeadingAndTrailingZeroDecimalValues ()
    {
      BigNum num = new BigNum ();
      num.Initialize ("0.100111");
      Assert.IsTrue (num.Mantissa.Equals (100111));
      Assert.AreEqual (-6, num.Exponent);
      Assert.AreEqual (false, num.Sign);

      num.Initialize ("000.100111");
      Assert.IsTrue (num.Mantissa.Equals (100111));
      Assert.AreEqual (-6, num.Exponent);
      Assert.AreEqual (false, num.Sign);
     
      num.Initialize ("00022.1001");
      Assert.IsTrue (num.Mantissa.Equals (221001));
      Assert.AreEqual (-4, num.Exponent);
      Assert.AreEqual (false, num.Sign);

      num.Initialize ("0101.100");
      Console.WriteLine ("mantissa: " + num.Mantissa.ToString ());
      Assert.IsTrue (num.Mantissa.Equals (1011));
      Assert.AreEqual (-1, num.Exponent);
      Assert.AreEqual (false, num.Sign);

      num.Initialize ("-0.100111");
      Assert.IsTrue (num.Mantissa.Equals (100111));
      Assert.AreEqual (-6, num.Exponent);
      Assert.AreEqual (true, num.Sign);
    }

    [Test]
    public void RemoveTralingZeros ()
    {
      BigNum num = new BigNum ();

      string s = "1.10";
      s = num.RemoveTrailingZeros (s);
      Console.WriteLine ("s: " + s);
      Assert.AreEqual ("1.1", s);

      s = "1.100";
      s = num.RemoveTrailingZeros (s);
      Assert.AreEqual ("1.1", s);

      s = "1.1010";
      s = num.RemoveTrailingZeros (s);
      Assert.AreEqual ("1.101", s);

      s = "1.10100";
      s = num.RemoveTrailingZeros (s);
      Assert.AreEqual ("1.101", s);

      s = "1.100100";
      s = num.RemoveTrailingZeros (s);
      Assert.AreEqual ("1.1001", s);
    }

    [Test]
    public void RemoveTralingDecimalPoint ()
    {
      BigNum num = new BigNum ();

      string s = "110.";
      s = num.RemoveTrailingZeros (s);
      Console.WriteLine ("s: " + s);
      Assert.AreEqual ("110", s);
    }


    ////////////////////////////////////////////////////////////////
    /// Tests For required methods
    /// ////////////////////////////////////////////////////////////
    ///

    [Test]
    public void ToStringTestWithIntegerValue ()
    {
      BigNum num = new BigNum ("1");
      string expected = "1";
      string actual = num.ToString ();
    
      Assert.IsFalse (actual.Contains (" "));
      Assert.IsFalse (actual.Contains ("."));
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void ToStringTestWithNegativeIntegerValue ()
    {
      BigNum num = new BigNum ("-1");
      string expected = "-1";
      string actual = num.ToString ();

      Assert.IsFalse (actual.Contains (" "));
      Assert.IsFalse (actual.Contains ("."));
      Assert.IsTrue (actual.Contains ("-"));
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void ToStringTestWithDecimalValue ()
    {
      BigNum num = new BigNum ("1.1");
      string expected = "1.1";
      string actual = num.ToString ();

      Assert.IsFalse (actual.Contains (" "));
      Assert.IsTrue (actual.Contains ("."));
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void ToStringTestWithNegativeDecimalValue ()
    {
      BigNum num = new BigNum ("-1.1");
      string expected = "-1.1";
      string actual = num.ToString ();

      Assert.IsFalse (actual.Contains (" "));
      Assert.IsTrue (actual.Contains ("."));
      Assert.IsTrue (actual.Contains ("-"));
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void ToStringTestWithValueLessThanOne ()
    {
      BigNum num = new BigNum (".1");
      string expected = ".1";
      string actual = num.ToString ();

      Assert.IsFalse (actual.Contains (" "));
      Assert.IsFalse (actual.Contains ("0"));
      Assert.IsTrue (actual.Contains ("."));
      Console.WriteLine ("actual " + actual);
      Assert.AreEqual (expected, actual);
    }



    
    //    [Test]
    //    public void ToStringTestWithRegularString ()
    //    {
    //      BigNum num = new BigNum ("1.1");
    //      string expected = "";
    //      string actual = num.ToString ();
    //
    //      Assert.IsFalse (actual.Contains (" "));
    //      Assert.IsFalse (actual.Contains (" "));
    //    }

    //    [Test]
    //    public void ToStringTestWithNoLeadingZeros ()
    //    {
    //      BigNum num = new BigNum ();
    //      num.Initialize ("001.1");
    //      string expected = "1.1";
    //
    //      string actual = num.ToString ();
    //
    //      Assert.AreEqual (expected, actual);
    //    }

  }
}

