using System;
using System.Text;
using System.Numerics;
using System.ComponentModel;

namespace CS422
{
  public class BigNum
  {
    // for double precision the significand can be 53 bits
    // This is about 16 decimal digits
    // the max number that 53 bits could store is (9,007,199,254,740,992)
    //TODO: make sure to restrict this value to a ceiling of (9,007,199,254,740,992)
    private BigInteger _mantissa;
    //private long _mantissa;

    private int _exponent;

    private bool _sign;

    private bool _undefined;

    public bool IsUndefined { get { return _undefined; } }

    //Properties for testing
    public BigInteger Mantissa { get { return _mantissa; } }

    public int Exponent { get { return _exponent; } }

    public bool Sign { get { return _sign; } }

    public bool Undefined {
      get { return _undefined; }
      set { _undefined = value; } 
    }

    // Constructors
    public BigNum ()
    {
      _undefined = true;
    }

    //My own constructor for use in operators to create a new bignum
    public BigNum (BigInteger mantissa, int exponent)
    {
      _exponent = exponent;
      if (mantissa < 0)
      {
        _sign = true;
        _mantissa = BigInteger.Abs (mantissa);
      }
      else
      {
        _sign = false;
        _mantissa = mantissa;
      }
      _undefined = false;
    }


    public BigNum (string number)
    { 
      if (IsValidNumberString (number))
      {
        //TODO: // the string is a valid number,
        // so initialize the BigNum to an exactly equal value. 
        // There must not be any rounding or truncation...
        Initialize (number);
        _undefined = false;
      }
      else
      {
        throw new ArgumentException ("Invalid argument");
      }
    }

    public BigNum (double value, bool useDoubleToString)
    {
      //Case 1
      if (double.IsNaN (value) || double.IsInfinity (value))
      {
        // then the BigNum is initialized to an undefined value, 
        // regardless of the value of the useDoubleToString parameter. 
        _undefined = true;
      }//Case 2
      else if (useDoubleToString)
      {
        string s = value.ToString ();
        if (IsValidNumberString (s))
        {
          Initialize (s);
          _undefined = false;
        }
        else
        {
          throw new ArgumentException ("Invalid argument");
        }
      }// Case 3
      else
      {
        
      }
    }


    //////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////
    /// Methods
    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////
    //
    public override string ToString ()
    {
      StringBuilder builder = new StringBuilder ();
      StringBuilder zeroBuilder = new StringBuilder ();

      Console.WriteLine ("TOSTRING: ");
      Console.WriteLine ("Mantissa: " + _mantissa);
      Console.WriteLine ("Exponent: " + _exponent);
      Console.WriteLine ("Sign: " + _sign);

      // First check for undefined
      if (_undefined)
      {
        return "undefined";
      }

      // Then check for sign
      if (_sign)
      {
        builder.Append ("-");
      }

      //Console.WriteLine ("ToString(): _exponent: " + _exponent.ToString ());

      if (_exponent == 0) // no decimal
      {
        builder.Append (_mantissa.ToString ());
        return builder.ToString ();
      }

      string temp = _mantissa.ToString ();
      // Note: because of how I create the BigNum,
      // The exponent will always be negative.

      // However we must look out for numbers that are much less than one
      if (Math.Abs (_exponent) > temp.Length)
      {
        zeroBuilder.Append ('.');
        //We must add 'diff' many zeros to the front of the mantissa
        int diff = Math.Abs (temp.Length + _exponent);
        for (int i = 0; i < diff; i++)
        {
          zeroBuilder.Append ('0');
        }
        zeroBuilder.Append (temp);
        return zeroBuilder.ToString ();
      }
      else
      {
        string second = temp.Substring (
                          (temp.Length + _exponent), Math.Abs (_exponent));
        Console.WriteLine ("here");
        string first = temp.Substring (0, (temp.Length + _exponent));

        //append first part
        builder.Append (first);

        //Append decimal
        builder.Append (".");

        // append second part
        builder.Append (second);
        return builder.ToString ();
      }
    }


    public static BigNum operator + (BigNum lhs, BigNum rhs)
    {
      Console.WriteLine ("Plus(): ");
      //Check to see if either parameter is undefined
      if (lhs.IsUndefined || rhs.IsUndefined)
      {
        return new BigNum ();
      }

      //Makesure we have mantissas of the correct sign for operation
      BigInteger leftMantissa = lhs.Mantissa;
      BigInteger rightMantissa = rhs.Mantissa;

      if (lhs.Sign)
      {
        leftMantissa = leftMantissa * -1;
      }

      if (rhs.Sign)
      {
        rightMantissa = rightMantissa * -1;
      }

      // Increase power of smaller BigNum exponent to match the larger
      // Find the difference in powers
      int diff = Math.Abs (lhs.Exponent - rhs.Exponent);
      //Console.WriteLine ("diff: " + diff);
      //Console.WriteLine ("lhs.e: " + lhs.Exponent.ToString () + " rhs.e: " + rhs.Exponent.ToString ());

      int matchingExponent = 0;
      if (Math.Abs (lhs.Exponent) > Math.Abs (rhs.Exponent))
      {
        //Console.WriteLine ("lhs bigger: " + lhs.ToString ());
        rightMantissa = AddTrailingZeros (rhs.Mantissa, diff);  
        matchingExponent = lhs.Exponent;
      }
      else if (Math.Abs (lhs.Exponent) < Math.Abs (rhs.Exponent))
      {
        //Console.WriteLine ("rhs bigger: " + rhs.ToString ());
        leftMantissa = AddTrailingZeros (lhs.Mantissa, diff);  
        matchingExponent = rhs.Exponent;
      }
      else // they must be equal
      {
        //Console.WriteLine ("Exponents equal");
        matchingExponent = rhs.Exponent;
      }

      // Add the new mantissa to the unchanged
      BigInteger resultingMantissa = leftMantissa + rightMantissa;      
//      Console.WriteLine ("matchingExponent: " + matchingExponent);
//      Console.WriteLine ("rightMantissa: " + rightMantissa);
//      Console.WriteLine ("leftMantissa: " + leftMantissa);
//      Console.WriteLine ("resultingMantissa: " + resultingMantissa.ToString ());

      // Use the numbers obtained from the above to create the new BigNum
      BigNum temp = new BigNum (resultingMantissa, matchingExponent);

      //Filter off trailing Zeros using the to string method(sloppy i know)
      string t = temp.ToString ();
      //Console.WriteLine ("tostringChek!! " + t);

      BigNum returnNum = new BigNum (t);
      return returnNum;
    }

    public static BigNum operator - (BigNum lhs, BigNum rhs)
    {
      Console.WriteLine ("Minus(): ");
      //Check to see if either parameter is undefined
      if (lhs.IsUndefined || rhs.IsUndefined)
      {
        return new BigNum ();
      }

      //Makesure we have mantissas of the correct sign for operation
      BigInteger leftMantissa = lhs.Mantissa;
      BigInteger rightMantissa = rhs.Mantissa;

      if (lhs.Sign)
      {
        leftMantissa = leftMantissa * -1;
      }

      if (rhs.Sign)
      {
        rightMantissa = rightMantissa * -1;
      }

      // Increase power of smaller BigNum exponent to match the larger
      // Find the difference in powers
      int diff = Math.Abs (lhs.Exponent - rhs.Exponent);
      Console.WriteLine ("diff: " + diff);
      Console.WriteLine ("lhs.e: " + lhs.Exponent.ToString () + " rhs.e: " + rhs.Exponent.ToString ());

      int matchingExponent = 0;
      if (Math.Abs (lhs.Exponent) > Math.Abs (rhs.Exponent))
      {
        Console.WriteLine ("lhs bigger: " + lhs.ToString ());
        rightMantissa = AddTrailingZeros (rhs.Mantissa, diff);  
        matchingExponent = lhs.Exponent;
      }
      else if (Math.Abs (lhs.Exponent) < Math.Abs (rhs.Exponent))
      {
        Console.WriteLine ("rhs bigger: " + rhs.ToString ());
        leftMantissa = AddTrailingZeros (lhs.Mantissa, diff);  
        matchingExponent = rhs.Exponent;
      }
      else // they must be equal
      {
        Console.WriteLine ("Exponents equal");
        matchingExponent = rhs.Exponent;
      }

      // Add the new mantissa to the unchanged
      BigInteger resultingMantissa = leftMantissa - rightMantissa;      
      Console.WriteLine ("matchingExponent: " + matchingExponent);
      Console.WriteLine ("rightMantissa: " + rightMantissa);
      Console.WriteLine ("leftMantissa: " + leftMantissa);
      Console.WriteLine ("resultingMantissa: " + resultingMantissa.ToString ());

      // Use the numbers obtained from the above to create the new BigNum
      BigNum temp = new BigNum (resultingMantissa, matchingExponent);

      //Filter off trailing Zeros using the to string method(sloppy i know)
      string t = temp.ToString ();
      Console.WriteLine ("tostringChek!! " + t);


      BigNum returnNum = new BigNum (t);
      return returnNum;
    }


    public static BigNum operator* (BigNum lhs, BigNum rhs)
    {
      Console.WriteLine ("Multiplication(): ");
      //Check to see if either parameter is undefined
      if (lhs.IsUndefined || rhs.IsUndefined)
      {
        return new BigNum ();
      }

      //Makesure we have mantissas of the correct sign for operation
      BigInteger leftMantissa = lhs.Mantissa;
      BigInteger rightMantissa = rhs.Mantissa;

      if (lhs.Sign)
      {
        leftMantissa = leftMantissa * -1;
      }

      if (rhs.Sign)
      {
        rightMantissa = rightMantissa * -1;
      }

      // Add the new mantissa to the unchanged
      BigInteger resultingMantissa = leftMantissa * rightMantissa;      

      int totalExponent = lhs.Exponent + rhs.Exponent;

      Console.WriteLine ("totalExponent: " + totalExponent);
      Console.WriteLine ("rightMantissa: " + rightMantissa);
      Console.WriteLine ("leftMantissa: " + leftMantissa);
      Console.WriteLine ("resultingMantissa: " + resultingMantissa.ToString ());

      // Use the numbers obtained from the above to create the new BigNum
      BigNum temp = new BigNum (resultingMantissa, totalExponent);

      //Filter off trailing Zeros using the to string method(sloppy i know)
      string t = temp.ToString ();
      Console.WriteLine ("tostringChek!! " + t);


      BigNum returnNum = new BigNum (t);
      return returnNum;

    }

    public static BigNum operator/ (BigNum lhs, BigNum rhs)
    {
      Console.WriteLine ("Division(): ");
      //Check to see if either parameter is undefined
      if (lhs.IsUndefined || rhs.IsUndefined)
      {
        return new BigNum ();
      }

      //Makesure we have mantissas of the correct sign for operation
      BigInteger leftMantissa = lhs.Mantissa;
      BigInteger rightMantissa = rhs.Mantissa;

      if (lhs.Sign)
      {
        leftMantissa = leftMantissa * -1;
      }

      if (rhs.Sign)
      {
        rightMantissa = rightMantissa * -1;
      }

      //For precision multiply the numerator by desired amount 
      BigInteger precision = BigInteger.Parse ("100000000000000000000000000000");
      leftMantissa = leftMantissa * precision;
      
      // Perform division
      BigInteger resultingMantissa = leftMantissa / rightMantissa;      

      // -29 accounts for precision above
      int totalExponent = (lhs.Exponent - 29) - rhs.Exponent;

      // get rid of any new trailing Zeros from precision step above
      resultingMantissa = BigInteger.Parse (
        RemoveTrailingZeros (resultingMantissa.ToString ()));


      Console.WriteLine ("totalExponent: " + totalExponent);
      Console.WriteLine ("rightMantissa: " + rightMantissa);
      Console.WriteLine ("leftMantissa: " + leftMantissa);
      Console.WriteLine ("resultingMantissa: " + resultingMantissa.ToString ());
      Console.WriteLine ("resultingMantissa: " + resultingMantissa);

      // Use the numbers obtained from the above to create the new BigNum

      BigNum temp = new BigNum (resultingMantissa, totalExponent);
      Console.WriteLine ("Quotient " + temp);

      //Filter off trailing Zeros using the to string method(sloppy i know)
      string t = temp.ToString ();
      Console.WriteLine ("tostringChek!! " + t);

      BigNum returnNum = new BigNum (t);
      return returnNum;
    }

    public static bool operator> (BigNum lhs, BigNum rhs)
    {
      //Check to see if either parameter is undefined
      if (lhs.IsUndefined || rhs.IsUndefined)
      {
        throw new ArgumentNullException ();
      }

      // Eliminate by Signs first
      if (lhs.Sign && !rhs.Sign)
      {
        return false;
      }
      else if (!lhs.Sign && rhs.Sign)
      {
        return true;
      }

      BigInteger lhs_LeftOfDecimal = GetLeftOfDecimal (lhs);
      BigInteger rhs_LeftOfDecimal = GetLeftOfDecimal (rhs);
      BigInteger lhs_RightOfDecimal = GetRightOfDecimal (lhs);
      BigInteger rhs_RightOfDecimal = GetRightOfDecimal (rhs);

      // Compare digits left of decimal first
      if (lhs_LeftOfDecimal > rhs_LeftOfDecimal)
      {
        return true;
      }
      else if (lhs_LeftOfDecimal < rhs_LeftOfDecimal)
      {
        return false;
      }
      //otherwise they are equal and we must compare right side
      if (lhs_RightOfDecimal > rhs_RightOfDecimal)
      {
        return true;
      }
      else if (lhs_RightOfDecimal < rhs_RightOfDecimal)
      {
        return false;
      }
      //Must be equal
      return false;
    }


    public static bool operator<= (BigNum lhs, BigNum rhs)
    {
      //Check to see if either parameter is undefined
      if (lhs.IsUndefined || rhs.IsUndefined)
      {
        throw new ArgumentNullException ();
      }

      // Eliminate by Signs first
      if (lhs.Sign && !rhs.Sign)
      {
        return true;
      }
      else if (!lhs.Sign && rhs.Sign)
      {
        return false;
      }

      BigInteger lhs_LeftOfDecimal = GetLeftOfDecimal (lhs);
      BigInteger rhs_LeftOfDecimal = GetLeftOfDecimal (rhs);
      BigInteger lhs_RightOfDecimal = GetRightOfDecimal (lhs);
      BigInteger rhs_RightOfDecimal = GetRightOfDecimal (rhs);

      // Compare digits left of decimal first
      if (lhs_LeftOfDecimal > rhs_LeftOfDecimal)
      {
        return false;
      }
      else if (lhs_LeftOfDecimal < rhs_LeftOfDecimal)
      {
        return true;
      }
      //otherwise they are equal and we must compare right side
      if (lhs_RightOfDecimal > rhs_RightOfDecimal)
      {
        return false;
      }
      else if (lhs_RightOfDecimal < rhs_RightOfDecimal)
      {
        return true;
      }
      //Must be equal
      return true;
    }

    public static bool operator< (BigNum lhs, BigNum rhs)
    {
      //Check to see if either parameter is undefined
      if (lhs.IsUndefined || rhs.IsUndefined)
      {
        throw new ArgumentNullException ();
      }

      // Eliminate by Signs first
      if (lhs.Sign && !rhs.Sign)
      {
        return true;
      }
      else if (!lhs.Sign && rhs.Sign)
      {
        return false;
      }

      BigInteger lhs_LeftOfDecimal = GetLeftOfDecimal (lhs);
      BigInteger rhs_LeftOfDecimal = GetLeftOfDecimal (rhs);
      BigInteger lhs_RightOfDecimal = GetRightOfDecimal (lhs);
      BigInteger rhs_RightOfDecimal = GetRightOfDecimal (rhs);

      // Compare digits left of decimal first
      if (lhs_LeftOfDecimal > rhs_LeftOfDecimal)
      {
        return false;
      }
      else if (lhs_LeftOfDecimal < rhs_LeftOfDecimal)
      {
        return true;
      }
      //otherwise they are equal and we must compare right side
      if (lhs_RightOfDecimal > rhs_RightOfDecimal)
      {
        return false;
      }
      else if (lhs_RightOfDecimal < rhs_RightOfDecimal)
      {
        return true;
      }
      //Must be equal
      return false;
    }

    public static bool operator>= (BigNum lhs, BigNum rhs)
    {
      //Check to see if either parameter is undefined
      if (lhs.IsUndefined || rhs.IsUndefined)
      {
        throw new ArgumentNullException ();
      }

      // Eliminate by Signs first
      if (lhs.Sign && !rhs.Sign)
      {
        return false;
      }
      else if (!lhs.Sign && rhs.Sign)
      {
        return true;
      }

      BigInteger lhs_LeftOfDecimal = GetLeftOfDecimal (lhs);
      BigInteger rhs_LeftOfDecimal = GetLeftOfDecimal (rhs);
      BigInteger lhs_RightOfDecimal = GetRightOfDecimal (lhs);
      BigInteger rhs_RightOfDecimal = GetRightOfDecimal (rhs);

      // Compare digits left of decimal first
      if (lhs_LeftOfDecimal > rhs_LeftOfDecimal)
      {
        return true;
      }
      else if (lhs_LeftOfDecimal < rhs_LeftOfDecimal)
      {
        return false;
      }
      //otherwise they are equal and we must compare right side
      if (lhs_RightOfDecimal > rhs_RightOfDecimal)
      {
        return true;
      }
      else if (lhs_RightOfDecimal < rhs_RightOfDecimal)
      {
        return false;
      }
      //Must be equal
      return true;
    }


    //    public static bool IsToStringCorrect(double value)
    //

    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////
    // Utility Methods 
    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////


    public static bool IsToStringCorrect (double value)
    {
      return false;
    }

    public static BigInteger AddTrailingZeros (BigInteger num, int numberOfZeros)
    {
      StringBuilder b = new StringBuilder ();
      b.Append (num.ToString ());

      for (int i = 0; i < numberOfZeros; i++)
      {
        b.Append ("0");
      }
      BigInteger result = BigInteger.Parse (b.ToString ());
      return result;
    }


    // This is used by the constructor to initialize the members of the class
    public void Initialize (string number)
    {
      Console.WriteLine ("INITIALIZE: ");
      //Console.WriteLine ("number: " + number);

      // See if there is a sign
      _sign = (number.Contains ("-")) ? true : false;

      // See if there is a decimal point and where it is
      string str = number; 
      int decimalIndex = number.IndexOf (".");
      if (decimalIndex >= 0)
      {
        // We need to take care of trailing zeros (Leading zeros 
        // seem to be handled by the parse of the string to BigInteger)
//        Console.WriteLine ("string before removal: " + str);
        str = RemoveTrailingZeros (number);
//        Console.WriteLine ("string after removal: " + str);
        
        //Console.WriteLine ("decimalIndex " + decimalIndex);
//        Console.WriteLine ("str.Length " + str.Length);
        //then there is a decimal point so create exponent
        if (decimalIndex == str.Length)
        {
          //only characters cut off the end were zeros so
          // exponent should be zero
          _exponent = 0;          
        }
        else
        {
          _exponent = decimalIndex - (str.Length - 1);
        }
      }

      // Now create the mantissa
      char[] delimiters = { '.', '-' };
      string[] parsedNumber = str.Split (delimiters);
      StringBuilder b = new StringBuilder ();
      foreach (string s in parsedNumber)
      {
        b.Append (s);
      }

      //We now have the mantissa in b
      string r = b.ToString ();
      Console.WriteLine ("r: " + r);

      _mantissa = new BigInteger ();
      _mantissa = BigInteger.Parse (r);
    }

    public static string RemoveTrailingZeros (string str)
    {
      StringBuilder builder = new StringBuilder ();
      StringBuilder final = new StringBuilder ();

      // first see if there is a decimal otherwise no operation is needed
      if (!str.Contains ("."))
      {
        return str;
      }

      //store first and second parts of the string (second includes the decimal)
      int index = str.IndexOf (".");
      final.Append (str.Substring (0, index));
      builder.Append (str.Substring (index, str.Length - index));
      //Console.WriteLine ("length: " + str.Length.ToString ());
      //Console.WriteLine ("first part of string " + final.ToString ());
      //Console.WriteLine ("second part of string " + builder.ToString ());

      //Iterate from the end of the string until a non zero digit, or a decimal 
      // is encountered
      int i = builder.Length - 1;
      string partial = builder.ToString ();
      while (partial [i] == '0')
      {        
        //Console.WriteLine ("Partial[i]: " + partial [i]);
        i--;
      }

      //See if the last element compared is a decimal
      if (partial [i] == '.')
      {
        // then everything after the decimal was a zero
        // dont append anything
      }
      else
      {
        string s = builder.ToString ().Substring (0, i + 1);
        final.Append (s);
      }

      return final.ToString ();
    }

    public static bool IsValidNumberString (string number)
    {
      int decimalAmount = 0;
      int i = 0;

      //TODO: Ramdom characters will be accepted through current scheme

      if (String.IsNullOrEmpty (number) || number.Contains (" "))
      {
        return false;
      }

      foreach (char c in number)
      {
        if (Char.IsLetter (c))
        {
          return false;
        }
        else if ('.' == c)
        {
          decimalAmount++;
          if (decimalAmount > 1)
          {
            return false;
          }
        }
        else if ('-' == c && i > 0)
        {          
          return false;
        }
        i++;
      }
      return true;
    }


    public static BigInteger GetLeftOfDecimal (BigNum num)
    {
      string temp = num.ToString ();

      if (temp.Contains ("."))
      {
        int index = temp.IndexOf (".");
        temp = temp.Substring (0, index);
        Console.WriteLine ("temp: " + temp);

        return BigInteger.Parse (temp);      
      }
      // the whole number is left of the decimal
      return BigInteger.Parse (num.ToString ());
    }

    public static BigInteger GetRightOfDecimal (BigNum num)
    {
      string temp = num.ToString ();

      if (temp.Contains ("."))
      {
        int index = temp.IndexOf (".");
        index++; // so as not to include the decimal in the substring
        temp = temp.Substring (index, temp.Length - index);
        BigInteger big = BigInteger.Parse (temp);
        if (num.Sign)
        {
          // make return value negative for ease of use later
          big = big * -1;
        }
        return big;            
      }
      // there is no right side of decimal
      return 0;
    }

  }
}

