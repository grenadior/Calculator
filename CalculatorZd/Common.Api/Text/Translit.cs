using System;
using System.Globalization;
using System.Text;

namespace Obj
{
	/// <summary>
	/// Summary description for translit.
	/// </summary>
	public class Translit
	{
		static string[] win1251 = new string[64] 
		{
			"A","B","V","G","D","E","Zh","Z","I","J","K","L","M","N","O","P","R","S","T","U","F","H","C","Ch","Sh","Shch","\"","Y","'","E","JU","JA", 
			"a","b","v","g","d","e","zh","z","i","j","k","l","m","n","o","p","r","s","t","u","f","h","c","ch","sh","shch","\"","y","'","e","ju","ja" 
		};

		static byte[] koi8win = new byte[64] 
		{
			0xE1,0xE2,0xF7,0xE7,0xE4,0xE5,0xF6,0xFA,0xE9,0xEA,0xEB,0xEC,0xED,0xEE,0xEF,0xF0,0xF2,0xF3,0xF4,0xF5,0xE6,0xE8,0xE3,0xFE,0xFB,0xFD,0xFF,0xF9,0xF8,0xFC,0xE0,0xF1, 
			0xC1,0xC2,0xD7,0xC7,0xC4,0xC5,0xD6,0xDA,0xC9,0xCA,0xCB,0xCC,0xCD,0xCE,0xCF,0xD0,0xD2,0xD3,0xD4,0xD5,0xC6,0xC8,0xC3,0xDE,0xDB,0xDD,0xDF,0xD9,0xD8,0xDC,0xC0,0xD1 
		};

		public string Win( string text, bool auto, bool noEuropean ) 
		{
			if(CultureInfo.CurrentCulture.TwoLetterISOLanguageName != "ru") 
			{
				if( !noEuropean ) return Win( text );
				else 
				{
					int nonASCII = 0;
					int ABCsymb = 0;
					int code;
					for( int i=0; i<text.Length; i++ ) 
					{
						code = Encoding.GetEncoding(1251).GetBytes( new char[] { (char)text[i] } )[0];
						if( (64<code && code<91) || (95<code && code<123) || code>191 || code==168 || code==184 )
							ABCsymb++; //считаем буквы в тексте
						if( code > 191 || code==168 || code==184  )
							nonASCII++; //не латинские символы
					}
					if( nonASCII/ABCsymb > 0.5 ) return Win( text );
					else return text;
				}
			}
			else return text;
		}

		public string Win( string text, bool auto )
		{
			if(CultureInfo.CurrentCulture.TwoLetterISOLanguageName != "ru") 
			{
				string output="";
				int code;
				for(int i=0; i<text.Length; i++ )
				{
					code = Encoding.GetEncoding(1251).GetBytes( new char[] { (char)text[i] } )[0];
					if( code > 191 ) output += win1251[ code - 192 ];
					else if( code == 168 ) output += "JO";
					else if( code == 184 ) output += "jo";
					else output += text[i];
				}
				return output;
			}
			else return text;
		}

		public string Win( string text ) 
		{
			return Win( text, true );
		}

		public string Koi8( string text ) 
		{
			byte[] output = new byte[text.Length];
			byte[] code = Encoding.GetEncoding(1251).GetBytes(text);
			for(int i=0; i<text.Length; i++ )
			{
				if( code[i] > 191 ) output[i] = koi8win[ code[i] - 192 ];
				else if( code[i] == 168 ) output[i] = 0xB3;
				else if( code[i] == 184 ) output[i] = 0xA3;
				else output[i] = code[i];
			}
			return Encoding.GetEncoding(1251).GetString( output );
		}

		public Translit()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
