using System;

namespace CS422
{
  public class BigNum
  {
    // for double precision the significand can be 53 bits
    // This is about 16 decimal digits
    // the max number that 53 bits could store is (9,007,199,254,740,992)

    //TODO: make sure to restrict this value to a ceiling of (9,007,199,254,740,992)
    private long _significand;

    private int _exponent;

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
      }
      else
      {
        throw new ArgumentException ("Invalid argument");
      }
    }

    public BigNum (double value, bool useDoubleToString)
    {
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

