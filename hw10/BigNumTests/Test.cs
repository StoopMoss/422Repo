using NUnit.Framework;
using System;
using CS422;
using System.Numerics;

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
    public void SecondConstructorTest ()
    {
      double d = 1.7E+3;
      //BigInteger integer = new BigInteger (1333333);

      BigNum num = new BigNum (d, false);

      Assert.IsNotNull (num);
      Console.WriteLine (num.Mantissa);
      Console.WriteLine (num.Exponent);
    }
    //
    //    [Test]
    //    public void DoubleToString ()
    //    {
    //      //double d = 1 / 3;
    //      double d = 1.7E+3;
    //      string test = BigNum.DoubleToString (d);
    //      Assert.NotNull (test);
    //    }

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
    public void InitializeWithDecimalThatHasAllTrailingZeros ()
    {
      BigNum num = new BigNum ();
      num.Initialize ("1.00");
      Assert.IsTrue (num.Mantissa.Equals (1));
      Assert.AreEqual (0, num.Exponent);
      Assert.AreEqual (false, num.Sign);

//      BigNum num = new BigNum ();
//      num.Initialize ("12.00");
//      Assert.IsTrue (num.Mantissa.Equals (12));
//      Assert.AreEqual (0, num.Exponent);
//      Assert.AreEqual (false, num.Sign);
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
    public void InitializeWithValueMuchLessThanOne ()
    {
      BigNum num = new BigNum ();
      num.Initialize (".000000001");
      Assert.IsTrue (num.Mantissa.Equals (1));
      Assert.AreEqual (-9, num.Exponent);
      Assert.AreEqual (false, num.Sign);
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
      string s = "1.10";
      s = BigNum.RemoveTrailingZeros (s);
      Console.WriteLine ("s: " + s);
      Assert.AreEqual ("1.1", s);

      s = "1.100";
      s = BigNum.RemoveTrailingZeros (s);
      Assert.AreEqual ("1.1", s);

      s = "1.1010";
      s = BigNum.RemoveTrailingZeros (s);
      Assert.AreEqual ("1.101", s);

      s = "1.10100";
      s = BigNum.RemoveTrailingZeros (s);
      Assert.AreEqual ("1.101", s);

      s = "1.100100";
      s = BigNum.RemoveTrailingZeros (s);
      Assert.AreEqual ("1.1001", s);
    }

    [Test]
    public void RemoveTralingDecimalPoint ()
    {
      string s = "110.";
      s = BigNum.RemoveTrailingZeros (s);
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

    [Test]
    public void ToStringTestWithValueMuchLessThanOne ()
    {
      BigNum num = new BigNum (".000000001");
      string expected = ".000000001";
      string actual = num.ToString ();

      Assert.IsFalse (actual.Contains (" "));
      Assert.IsTrue (actual.Contains ("0"));
      Assert.IsTrue (actual.Contains ("."));
      Assert.AreEqual (expected, actual);
    }


    [Test]
    public void ToStringTestWithManyDigitValue ()
    {
      BigNum num = new BigNum ("12345.12345");
      string expected = "12345.12345";
      string actual = num.ToString ();

      Assert.IsFalse (actual.Contains (" "));
      Assert.IsFalse (actual.Contains ("0"));
      Assert.IsTrue (actual.Contains ("."));
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void ToStringTestWithNoDecimal ()
    {
      BigNum num = new BigNum ("12345");
      string expected = "12345";
      string actual = num.ToString ();

      Assert.IsFalse (actual.Contains (" "));
      Assert.IsFalse (actual.Contains ("0"));
      Assert.IsFalse (actual.Contains ("."));
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void ToStringWhenUndefined ()
    {
      BigNum num = new BigNum ();
      num.Undefined = true;
      string expected = "undefined";

      string actual = num.ToString ();

      Assert.IsFalse (actual.Contains (" "));
      Assert.IsFalse (actual.Contains ("0"));
      Assert.IsFalse (actual.Contains ("."));
      Console.WriteLine ("actual " + actual);
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void ToStringMakeSureNoLeadingZeros ()
    {
      BigNum num = new BigNum ("0.4");
      string expected = ".4";

      string actual = num.ToString ();

      Assert.IsFalse (actual.Contains (" "));
      Assert.IsFalse (actual.Contains ("0"));
      Assert.IsTrue (actual.Contains ("."));
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void ToStringMakeSureNoUnneededTrailingZeros ()
    {
      BigNum num = new BigNum ("0.400");
      string expected = ".4";

      string actual = num.ToString ();

      Assert.IsFalse (actual.Contains (" "));
      Assert.IsFalse (actual.Contains ("0"));
      Assert.IsTrue (actual.Contains ("."));
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void ToStringAllTrailingZerosAfterDecimal ()
    {
      BigNum num = new BigNum ("1.00");
      string expected = "1";

      string actual = num.ToString ();

      Assert.IsFalse (actual.Contains (" "));
      Assert.IsFalse (actual.Contains ("0"));
      Assert.IsFalse (actual.Contains ("."));
      Assert.AreEqual (expected, actual);
    }


    [Test]
    public void AddTrailingZeros ()
    {
      BigInteger num = BigInteger.Parse ("1");
      BigInteger expected = BigInteger.Parse ("10000");

      BigInteger actual = BigNum.AddTrailingZeros (num, 4);

      Assert.NotNull (actual);
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void AddTrailingZerosWithNegativeNumber ()
    {
      BigInteger num = BigInteger.Parse ("-1");
      BigInteger expected = BigInteger.Parse ("-10000");

      BigInteger actual = BigNum.AddTrailingZeros (num, 4);

      Assert.NotNull (actual);
      Assert.AreEqual (expected, actual);
    }

    //IsStringCorrect Tests
    //    [Test]
    //    public void IsToStringCorrect ()
    //    {
    //      double value = 4.4D;
    //      bool result = BigNum.IsToStringCorrect (value);
    //    }



    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
    /// Operator Tests
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////
    /// Plus Operator Tests
    /////////////////////////////////////////////////////////////////////////
    [Test]
    public void PlusOperatorWithEitherParamUndefinedShouldReturnUndefined ()
    {
      BigNum num1 = new BigNum ();
      num1.Undefined = true;
      BigNum num2 = new BigNum ("2");

      BigNum sum = num1 + num2;
      Assert.IsTrue (sum.IsUndefined);

      BigNum sum2 = num2 + num1;
      Assert.IsTrue (sum2.IsUndefined);
    }

    [Test]
    public void PlusOperator ()
    {
      BigNum num1 = new BigNum ();
      num1.Undefined = true;
      BigNum num2 = new BigNum ("2");

      BigNum sum = num1 + num2;
      Assert.IsTrue (sum.IsUndefined);
    }

    [Test]
    public void PlusOperatorTwoNumbersWithDifferentExponents ()
    {
      BigNum num1 = new BigNum ("1.1");
      BigNum num2 = new BigNum ("1.11");
      BigNum expected = new BigNum (new BigInteger (221), -2);

      BigNum sum = num1 + num2;

      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void PlusOperatorParamsWithEqualExponents ()
    {
      BigNum num1 = new BigNum ("1.1");
      BigNum num2 = new BigNum ("2.1");
      BigNum expected = new BigNum (new BigInteger (32), -1);

      BigNum sum = num1 + num2;

      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void PlusOperatorWithParamsOfEqualPowerResultingInNegativeValue ()
    {
      BigNum num1 = new BigNum ("1.1");
      BigNum num2 = new BigNum ("-2.1");
      BigNum expected = new BigNum (new BigInteger (-1), 0);

      Console.WriteLine ("expected: " + expected);
      BigNum sum = num1 + num2;

      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Console.WriteLine ("hesdfgdfsre");
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void PlusOperatorWithParamsLargerThanLongs ()
    {
      BigNum num1 = new BigNum ("9223372036854775808");
      Console.WriteLine ("Uh yeah");
      BigNum num2 = new BigNum ("9223372036854775808");
      BigInteger I = BigInteger.Parse ("18446744073709551616");
      BigNum expected = new BigNum (I, 0);

      Console.WriteLine ("expected: " + expected);
      BigNum sum = num1 + num2;

      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);

    }

    /////////////////////////////////////////////////////////////////////////
    /// Subtraction Operator Tests
    /////////////////////////////////////////////////////////////////////////
     
    [Test]
    public void SubtractionWithTwoPositivesResultingInPositive ()
    {
      BigNum num1 = new BigNum ("5");
      BigNum num2 = new BigNum ("2");
      BigInteger I = BigInteger.Parse ("3");
      BigNum expected = new BigNum (I, 0);

      BigNum sum = num1 - num2;

      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void SubtractionWithTwoPositivesResultingInNegative ()
    {
      BigNum num1 = new BigNum ("2");
      BigNum num2 = new BigNum ("4");
      BigInteger I = BigInteger.Parse ("-2");
      BigNum expected = new BigNum (I, 0);

      BigNum sum = num1 - num2;

      Console.WriteLine ("hesdf");
      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void SubtractionWithTwoPositiveDecimalsResultingInPositive ()
    {
      BigNum num1 = new BigNum ("4.2");
      BigNum num2 = new BigNum ("2.2");
      BigInteger I = BigInteger.Parse ("2");
      BigNum expected = new BigNum (I, 0);

      BigNum sum = num1 - num2;

      Console.WriteLine ("hesdf");
      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void SubtractionWithTwoPositiveDecimalsResultingInPositiveDecimal ()
    {
      BigNum num1 = new BigNum ("4.2");
      BigNum num2 = new BigNum ("2.1");
      BigInteger I = BigInteger.Parse ("21");
      BigNum expected = new BigNum (I, -1);

      BigNum sum = num1 - num2;

      Console.WriteLine ("hesdf");
      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    //    [Test]
    //    public void SubtractionWithTwoPositiveDecimalsResultingInPositiveDecimal ()
    //    {
    //      BigNum num1 = new BigNum ("4.2");
    //      BigNum num2 = new BigNum ("2.1");
    //      BigInteger I = BigInteger.Parse ("21");
    //      BigNum expected = new BigNum (I, -1);
    //
    //      BigNum sum = num1 - num2;
    //
    //      Console.WriteLine ("hesdf");
    //      Assert.NotNull (sum);
    //      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
    //      Assert.AreEqual (expected.Exponent, sum.Exponent);
    //      Assert.AreEqual (expected.Sign, sum.Sign);
    //      Assert.AreEqual (expected.Undefined, sum.Undefined);
    //    }


    /////////////////////////////////////////////////////////////////////////
    /// Multiplication Operator Tests
    /////////////////////////////////////////////////////////////////////////

    [Test]
    public void Multiplication ()
    {
      BigNum num1 = new BigNum ("4");
      BigNum num2 = new BigNum ("2");
      BigInteger I = BigInteger.Parse ("8");
      BigNum expected = new BigNum (I, 0);

      BigNum sum = num1 * num2;

      Console.WriteLine ("hesdf");
      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void MultiplicationTwoPositiveDecimals ()
    {
      BigNum num1 = new BigNum ("2.2");
      BigNum num2 = new BigNum ("2.2");
      BigInteger I = BigInteger.Parse ("484");
      BigNum expected = new BigNum (I, -2);

      BigNum sum = num1 * num2;

      Console.WriteLine ("hesdf");
      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void MultiplicationOneNegativeNumber ()
    {
      BigNum num1 = new BigNum ("2");
      BigNum num2 = new BigNum ("-2");
      BigInteger I = BigInteger.Parse ("-4");
      BigNum expected = new BigNum (I, 0);

      BigNum sum = num1 * num2;

      Console.WriteLine ("hesdf");
      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }


    /////////////////////////////////////////////////////////////////////////
    /// Division Operator Tests
    /////////////////////////////////////////////////////////////////////////
    /// 
    [Test]
    public void Division ()
    {
      BigNum num1 = new BigNum ("4");
      BigNum num2 = new BigNum ("2");
      BigInteger I = BigInteger.Parse ("2");
      BigNum expected = new BigNum (I, 0);

      BigNum sum = num1 / num2;

      Console.WriteLine ("hesdf");
      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void DivisionWithResultingInADecimal ()
    {
      BigNum num1 = new BigNum ("9");
      BigNum num2 = new BigNum ("2");
      BigInteger I = BigInteger.Parse ("45");
      BigNum expected = new BigNum (I, -1);

      BigNum sum = num1 / num2;

      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void DivisionWithANegative ()
    {
      BigNum num1 = new BigNum ("9");
      BigNum num2 = new BigNum ("-2");
      BigInteger I = BigInteger.Parse ("-45");
      BigNum expected = new BigNum (I, -1);

      BigNum sum = num1 / num2;

      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void DivisionWithTwoDecimals ()
    {
      BigNum num1 = new BigNum ("1.2");
      BigNum num2 = new BigNum ("1.3");
      BigInteger I = BigInteger.Parse ("92307692307692307692307692307");
      BigNum expected = new BigNum (I, -29);

      BigNum sum = num1 / num2;

      Assert.NotNull (sum);
      Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      Assert.AreEqual (expected.Exponent, sum.Exponent);
      Assert.AreEqual (expected.Sign, sum.Sign);
      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void DivisionWithTwoMaxs ()
    {
      BigNum num1 = new BigNum ("9223372036854775808");
      BigNum num2 = new BigNum ("9223372036854775809");
      //BigInteger I = BigInteger.Parse ("");
      //BigNum expected = new BigNum (I, -29);

      BigNum sum = num1 / num2;

      Assert.NotNull (sum);
      //Assert.AreEqual (expected.Mantissa, sum.Mantissa);
      //Assert.AreEqual (expected.Exponent, sum.Exponent);
      //Assert.AreEqual (expected.Sign, sum.Sign);
//      Assert.AreEqual (expected.Undefined, sum.Undefined);
    }

    [Test]
    public void DivisionWith ()
    {
      BigNum num1 = new BigNum ("27");
      BigNum num2 = new BigNum ("7");

      BigNum sum = num1 / num2;

      Assert.NotNull (sum);
    }

    /////////////////////////////////////////////////////////////////////////
    /// > greaterThan Operator Tests
    /////////////////////////////////////////////////////////////////////////

    [Test]
    public void GreaterThanWithInts ()
    {
      BigNum num1 = new BigNum ("1");
      BigNum num2 = new BigNum ("2");

      bool result = (num1 > num2);
      Assert.IsFalse (result);

      result = (num2 > num1);
      Assert.IsTrue (result);
    }

    [Test]
    public void GreaterThanWithDecimals ()
    {
      BigNum num1 = new BigNum ("1.2");
      BigNum num2 = new BigNum ("1.4");

      bool result = (num1 > num2);
      Assert.IsFalse (result);

      result = (num2 > num1);
      Assert.IsTrue (result);
    }

    [Test]
    public void GreaterThanWithEqualPositiveDecimals ()
    {
      BigNum num1 = new BigNum ("1.2");
      BigNum num2 = new BigNum ("1.4");

      bool result = (num1 > num2);
      Assert.IsFalse (result);
    }

    [Test]
    public void GreaterThanWithFirstParamNegative ()
    {
      BigNum num1 = new BigNum ("-1");
      BigNum num2 = new BigNum ("1");

      bool result = (num1 > num2);
      Assert.IsFalse (result);
    }

    [Test]
    public void GreaterThanWithSecondParamNegative ()
    {
      BigNum num1 = new BigNum ("1");
      BigNum num2 = new BigNum ("-1");

      bool result = (num1 > num2);
      Assert.IsTrue (result);
    }

    [Test]
    public void GreaterThanWithBothParamsNegative ()
    {
      BigNum num1 = new BigNum ("-1");
      BigNum num2 = new BigNum ("-2");

      bool result = (num1 > num2);
      Assert.IsTrue (result);

      result = (num2 > num1);
      Assert.IsFalse (result);
    }

    [Test]
    public void GreaterThanWithBothParamsNegativeDecimals ()
    {
      BigNum num1 = new BigNum ("-1.1");
      BigNum num2 = new BigNum ("-1.4");

      bool result = (num1 > num2);
      Assert.IsTrue (result);

      result = (num2 > num1);
      Assert.IsFalse (result);
    }

    [Test]
    public void GreaterThanWithBothParamsEqual ()
    {
      BigNum num1 = new BigNum ("-1.1");
      BigNum num2 = new BigNum ("-1.1");

      bool result = (num1 > num2);
      Assert.IsFalse (result);

      result = (num2 > num1);
      Assert.IsFalse (result);
    }


    /// /////////////////////////////////////////////////////////////////////////
    /// >= Operator Tests
    /////////////////////////////////////////////////////////////////////////


    /// /////////////////////////////////////////////////////////////////////////
    /// < Operator Tests
    /////////////////////////////////////////////////////////////////////////
    [Test]
    public void LessThanWithBothParamsEqual ()
    {
      BigNum num1 = new BigNum ("1");
      BigNum num2 = new BigNum ("1");

      bool result = (num1 < num2);
      Assert.IsFalse (result);
      result = (num2 < num1);
      Assert.IsFalse (result);
    }

    [Test]
    public void LessThanWithOneNegativeParam ()
    {
      BigNum num1 = new BigNum ("1");
      BigNum num2 = new BigNum ("-1");

      bool result = (num1 < num2);
      Assert.IsFalse (result);
      result = (num2 < num1);
      Assert.IsTrue (result);
    }

    [Test]
    public void LessThanWithBothParamsNegativeAndEqual ()
    {
      BigNum num1 = new BigNum ("-1");
      BigNum num2 = new BigNum ("-1");

      bool result = (num1 < num2);
      Assert.IsFalse (result);
      result = (num2 < num1);
      Assert.IsFalse (result);
    }

    [Test]
    public void LessThanWithBothParamsNegative ()
    {
      BigNum num1 = new BigNum ("-1");
      BigNum num2 = new BigNum ("-2");

      bool result = (num1 < num2);
      Assert.IsFalse (result);
      result = (num2 < num1);
      Assert.IsTrue (result);
    }

    [Test]
    public void LessThanWithBothParamsDecimal ()
    {
      BigNum num1 = new BigNum ("1.1");
      BigNum num2 = new BigNum ("1.2");

      bool result = (num1 < num2);
      Assert.IsTrue (result);
      result = (num2 < num1);
      Assert.IsFalse (result);
    }


    [Test]
    public void LessThanWithParamsDecimalOfDifferentPower ()
    {
      BigNum num1 = new BigNum ("100.1");
      BigNum num2 = new BigNum ("1.001");

      bool result = (num1 < num2);
      Assert.IsFalse (result);
      result = (num2 < num1);
      Assert.IsTrue (result);
    }

    [Test]
    public void LessThanWithDecimalDeciding ()
    {
      BigNum num1 = new BigNum ("100.1");
      BigNum num2 = new BigNum ("1.001");

      bool result = (num1 < num2);
      Assert.IsFalse (result);
      result = (num2 < num1);
      Assert.IsTrue (result);
    }


    /// /////////////////////////////////////////////////////////////////////////
    /// <= Operator Tests
    /////////////////////////////////////////////////////////////////////////
    [Test]
    public void LessThanOrEqualTo ()
    {
      BigNum num1 = new BigNum ("1");
      BigNum num2 = new BigNum ("1");
      BigNum num3 = new BigNum ("2");

      bool result = (num1 <= num2);
      Assert.IsTrue (result);

      result = (num1 <= num3);
      Assert.IsTrue (result);
    }


    /// /////////////////////////////////////////////////////////////////////////
    /// IsToStringCorrect Tests
    /////////////////////////////////////////////////////////////////////////
    ///

    /// /////////////////////////////////////////////////////////////////////////
    /// GetLeftHandSide Tests
    /////////////////////////////////////////////////////////////////////////
    [Test]
    public void GetLeftHandSideOfNumberWithDecimal ()
    {
      BigNum num = new BigNum ("12.34");
      BigInteger expected = BigInteger.Parse ("12");

      BigInteger actual = BigNum.GetLeftOfDecimal (num);

      Assert.IsNotNull (actual);
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void GetLeftHandSideOfNumberWithOutDecimal ()
    {
      BigNum num = new BigNum ("124");
      BigInteger expected = BigInteger.Parse ("124");

      BigInteger actual = BigNum.GetLeftOfDecimal (num);

      Assert.IsNotNull (actual);
      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void GetLeftHandSideOfNegativeNumber ()
    {
      BigNum num = new BigNum ("-124");
      BigInteger expected = BigInteger.Parse ("-124");

      BigInteger actual = BigNum.GetLeftOfDecimal (num);

      Assert.IsNotNull (actual);
      Assert.AreEqual (expected, actual);
    }


    /// /////////////////////////////////////////////////////////////////////////
    /// GetRightHandSide Tests
    /////////////////////////////////////////////////////////////////////////
    [Test]
    public void GetRightHandSideOfNumberWithDecimal ()
    {
      BigNum num = new BigNum ("12.34");
      BigInteger expected = BigInteger.Parse ("34");

      BigInteger actual = BigNum.GetRightOfDecimal (num);

      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void GetRightHandSideOfNumberWithOutDecimal ()
    {
      BigNum num = new BigNum ("123");
      BigInteger expected = BigInteger.Parse ("0");

      BigInteger actual = BigNum.GetRightOfDecimal (num);

      Assert.AreEqual (expected, actual);
    }

    [Test]
    public void GetRightHandSideOfNegativeNumberWithDecimal ()
    {
      BigNum num = new BigNum ("-12.34");
      BigInteger expected = BigInteger.Parse ("-34");

      BigInteger actual = BigNum.GetRightOfDecimal (num);

      Assert.AreEqual (expected, actual);
    }


  }
}

