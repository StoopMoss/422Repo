using System;
using System.Text;
using System.Numerics;

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

    //Properties for testing
    public BigInteger Mantissa { get { return _mantissa; } }

    public int Exponent { get { return _exponent; } }

    public bool Sign { get { return _sign; } }

    // Constructors
    public BigNum ()
    {
    }

    public BigNum (string number)
    { 
      if (IsValidNumberString (number))
      {
        //TODO: // the string is a valid number,
        // so initialize the BigNum to an exactly equal value. 
        // There must not be any rounding or truncation...
        Initialize (number);
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
      }
//      else if (useDoubleToString)
//      {
//        if ()
//        Initialize (value);
//      }

      //Case 2


      //Case
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

      // FIrst check for sign
      if (_sign)
      {
        builder.Append ("-");
      }

      //get the location of the decimal from the exponent
      int index = _exponent * -1;
      Console.WriteLine ("in");

      if (index == 0) // no decimal
      {
        builder.Append (_mantissa.ToString ());
        return builder.ToString ();
      }
      else if()// Move decimal left if exponent is  negative
      {}
      else if ()// Move decimal right if positive
      {
        
      }

      // append all digits before decimal
      builder.Append (_mantissa.ToString ().Substring (0, index));

      // Append decimal
      builder.Append (".");

      // append all digits after decimal
      builder.Append (_mantissa.ToString ()
        .Substring (index, _mantissa.ToString ().Length - index));

      Console.WriteLine ("index: " + index);
      Console.WriteLine ("_exponent: " + _exponent);
      Console.WriteLine ("mantissa: " + _mantissa);
      Console.WriteLine ("sign: " + _sign);

      return builder.ToString ();
    }


    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////
    // Utility Methods 
    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////


    // This is used by the constructor to initialize the members of the class 
    public void Initialize (string number)
    {
      // See if there is a sign
      _sign = (number.Contains ("-")) ? true : false;

      // See if there is a decimal point and where it is
      string str = number; 
      int decimalIndex = number.IndexOf (".");
      if (decimalIndex >= 0)
      {
        // We need to take care of trailing zeros (Leading zeros 
        // seem to be handled by the parse of the string to BigInteger)
        //Console.WriteLine ("string before removal: " + str);
        str = RemoveTrailingZeros (number);
        //Console.WriteLine ("string after removal: " + str);
        
        //then there is a decimal point so create exponent
        _exponent = decimalIndex - (str.Length - 1);
      }

      // Now create the mantissa
      char[] delimiters = { '.', '-' };
      string[] parsedNumber = str.Split (delimiters);
      StringBuilder b = new StringBuilder ();
      foreach (string s in parsedNumber)
      {
        b.Append (s);
      }

      //We now have the mantisa in b
      string r = b.ToString ();

      _mantissa = new BigInteger ();
      _mantissa = BigInteger.Parse (r);
    }

    public string RemoveTrailingZeros (string str)
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


  }
}

