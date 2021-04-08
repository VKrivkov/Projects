using System;

namespace CipherLib
{
  public abstract class Reflection
  {
    int[] keys = new int[5];
    int count = 0;

    public string ForCrack(Cipher temp)
    {
      if (count < 5)
      {
        Random r = new Random();
        keys[count] = r.Next(1, 7);
        string str = temp.Text;
        string defenced = "";
        for (int i = 0; i < str.Length; i++)
          for (int j = 0; j < keys[count]; j++)
          {
            defenced += (str[i] * j).ToString();
          }
        count++;
        return defenced;
      }
      else
      {
        for (int i = 0; i < count; i++)
        {
          keys[i] = 0;
        }
        count = 0;
        return null;
      }
    }
  }



  ///////////////////////////////


  public class Transfer : Defence
  {
    public DateTime time;
    bool is_Sended = false;
    /// <summary>
    /// /////////////////////////////////////////////////////////
    /// </summary>
    public override int Def { get => base.Def; set { if (is_Sended) base.Def = value; } }

    public Transfer()
    {
      time = new DateTime(2021, 3, 5);
    }
    /// <summary>
    /// ////////////////////////////////////////////////////////
    /// </summary>
    public override void WriteCode()
    {
      is_Sended = true;
      Console.WriteLine(time.Date + "    " + base.GetString());
    }

    public void GetCode(Defence temp)
    {
      is_Sended = true;
      Console.WriteLine(time.Date + "    " + temp.GetString());
    }

    public void GetCode(Defence temp, Cipher k)
    {
      is_Sended = true;
      Console.WriteLine(time.Date + "    " + temp.GetString(k));
    }

    public static Transfer operator ++(Transfer a)
    {
      a.time.AddDays(1);
      return a;
    }

  }
  /////////////////////////////////////////////////////////
  ///

  public class Defence : Cipher
  {
    int def;
    public virtual int Def { get; set; }

    public Defence() : base("default")
    {
      this.def = 100;
    }
    public Defence(int def, string text) : base(text)
    {
      this.def = def;
      base.IsCr = true;
    }

    public string GetString()
    {
      string temp = "";
      string full = "";
      if (base.IsEnc())
      {
        temp = base.Text;
      }
      else
      {
        temp = base.Encrypt();
      }
      for (int i = 0; i < temp.Length; i++)
      {
        full += ((temp[i] + def) * i).ToString();
      }
      return full;
    }

    public string GetString(Cipher temp)
    {
      string a = "";
      string b = "";
      if (temp.IsEnc())
      {
        a = temp.Text;
      }
      else
      {
        a = temp.Encrypt();
      }
      for (int i = 0; i < a.Length; i++)
      {
        b += ((a[i] + def) * i).ToString();
      }
      return b;

    }
    /// <summary>
    /// ////////////////////////////////////////////////////
    /// </summary>
    public virtual void WriteCode()
    {
      Console.WriteLine(GetString());
    }
    /// <summary>
    /// /////////////////////////////////////////////////////
    /// </summary>
    public void GetCode(Transfer t, Cipher k)
    {
      Console.WriteLine(t.time + "    " + GetString(k));
    }
    public void GetCode(Transfer t)
    {
      Console.WriteLine(t.time + "    " + GetString());
    }

  }

  /////////////////////////////////////////////////////////////////
  ///


  public class Cipher : Reflection
  {
    bool isCr = true;
    public bool IsCr { get; set; }
    const string alfabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string fullAlfabet = alfabet + alfabet.ToLower();
    string text = "default";
    int key = 1;
    int data = 1;
    bool isEncrypted = true;
    public string Text { get { return text; } set { if (value.Length < 100) text = value; else text = "default"; } }
    public Cipher()
    {
      text = "default";
      key = 1;
      data = 1;
    }
    public Cipher(string k)
    {

      Text = k;
    }
    public Cipher(string k, int i)
    {
      Text = k;
      key = i;
    }
    public Cipher(string k, int i, int d)
    {
      Text = k;
      key = i;
      data = d;
    }
    //functions
    public string Encrypt()
    {
      string enc = "";
      for (int i = 0; i < text.Length; i++)
      {
        var c = text[i];
        int index = fullAlfabet.IndexOf(c);
        if (index < 0)
          enc += c.ToString();
        else
        {
          var codeIndex = (index + key) % fullAlfabet.Length;
          enc += fullAlfabet[codeIndex];
        }
      }
      text = enc;
      isEncrypted = true;
      return enc;
    }
    public string Decrypt(string text)
    {
      string dec = "";
      for (int i = 0; i < text.Length; i++)
      {
        var c = text[i];
        int index = fullAlfabet.IndexOf(c);
        if (index < 0)
          dec += c.ToString();
        else
        {
          var codeIndex = (index - key) % fullAlfabet.Length;
          dec += fullAlfabet[codeIndex];
        }
      }
      text = dec;
      isEncrypted = true;
      return dec;
    }
    public bool IsEnc()
    {
      return isEncrypted;
    }

    public static Cipher operator *(Cipher t, int i)
    {
      t.key = t.key * i;
      return t;
    }

    public static Cipher operator !(Cipher temp)
    {
      temp.isCr = !temp.isCr;
      return temp;
    }

    public void ForInterception()
    {
      if (isCr)
      {
        text = this.Encrypt();
        text = base.ForCrack(this);
        isEncrypted = true;

      }
    }

  }
}
