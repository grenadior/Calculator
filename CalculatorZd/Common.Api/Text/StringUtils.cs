using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Common.Api.Types;
using Localization.WebResources.Common;

namespace Common.Api.Text
{
    public class StringUtils
    {
        private static readonly Regex m_regex = new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled);
        private static readonly Regex m_rxHostName = new Regex(@"(?<Start>://)(?<Host>(?<HostName>([^/^:])+)(?<Port>:\d+)?)(?<End>/)?",
          RegexOptions.Compiled);
        private const string m_rxPhoneReplace = "(^|[^a-zA-Zа-яА-Я]{{1}})({0})($|[^a-zA-Zа-яА-Я]{{1}})";
        public const string REGEX_LINKS = "http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?";
        //NOTE: не локализовать
        private static readonly string[] m_numberReplacers = new string[]{"тысяча","тысячи","сто","двести","триста","четыреста","пятьсот","шестьсот","семьсот","восемьсот","девятьсот",
                                                                "десять","двадцать","тридцать","сорок","пятьдесят","шестьдесят","семьдесят","восемьдесят","девяносто",
                                                                "одиннадцать","двенадцать","тринадцать","четырнадцать","пятнадцать","шестнадцать","семнадцать","восемнадцать","девятнадцать",
                                                                "ноль","один","одна","два","две","три","четыре","пять","шесть","восемь","семь","девять"};
        private static readonly string[] m_numberSeparateReplacers = new string[]{"нол","од","дв","тр","чет","пят","шес","шест","сем","вос","восем","дев","девят",
                                                                         "д","ш","т","ч","п",
                                                                         "i","ii","iii","iv","v","vi","vii","viii","ix","x","xi","xii","xiii","xiv","xv","xvi","xvii","xviii","xix","xx","xxi","xxii","xxiii","xxiv","xxv","xxvi","xxvii","xxviii","xxix","xxx","iх","х"};
        private static readonly string[] RegExpSpecialChars = new string[] { ".", "$", "^", "{", "}", "[", "]", "(", "|", ")", "*", "+", "?" };

        private static readonly Dictionary<char, string> TransliterationTable = new Dictionary<char, string>
			                                                                        {
				                                                                        {'А', "A"}, {'а', "a"}, {'Б', "B"}, {'б', "b"},
				                                                                        {'В', "V"}, {'в', "v"}, {'Г', "G"}, {'г', "g"},
				                                                                        {'Д', "D"}, {'д', "d"}, {'Е', "E"}, {'е', "e"},
				                                                                        {'Ё', "E"}, {'ё', "e"}, {'Ж', "Zh"}, {'ж', "zh"},
				                                                                        {'З', "Z"}, {'з', "z"}, {'И', "I"}, {'и', "i"},
				                                                                        {'Й', "I"}, {'й', "i"}, {'К', "K"}, {'к', "k"},
				                                                                        {'Л', "L"}, {'л', "l"}, {'М', "M"}, {'м', "m"},
				                                                                        {'Н', "N"}, {'н', "n"}, {'О', "O"}, {'о', "o"},
				                                                                        {'П', "P"}, {'п', "p"}, {'Р', "R"}, {'р', "r"},
				                                                                        {'С', "S"}, {'с', "s"}, {'Т', "T"}, {'т', "t"},
				                                                                        {'У', "U"}, {'у', "u"}, {'Ф', "F"}, {'ф', "f"},
				                                                                        {'Х', "Kh"}, {'х', "kh"}, {'Ц', "Tc"}, {'ц', "tc"},
				                                                                        {'Ч', "Ch"}, {'ч', "ch"}, {'Ш', "Sh"}, {'ш', "sh"},
				                                                                        {'Щ', "Shch"}, {'щ', "shch"}, {'Ъ', string.Empty},
																						{'ъ', string.Empty}, {'Ы', "Y"}, {'ы', "y"},
																						{'Ь', string.Empty}, {'ь', string.Empty}, {'Э', "E"}, {'э', "e"}, 
																						{'Ю', "Iu"}, {'ю', "iu"}, {'Я', "Ia"}, {'я', "ia"}
			                                                                        };

        public static bool IsDigitString(string p_str)
        {
            for (int i = 0; i < p_str.Length; i++)
                if (!Char.IsDigit(p_str[i]))
                    return false;
            return true;
        }

        public static void CutEnd(ref string p_sStr, char p_cEndChar)
        {
            if (p_sStr.Length == 0)
                return;
            if (p_sStr[p_sStr.Length - 1] == p_cEndChar)
                p_sStr = p_sStr.Substring(0, p_sStr.Length - 1);
        }

        public static string FormatStringArray(string p_sFormatString, string[] p_asTarget)
        {
            string sReturn = String.Empty;
            if (String.IsNullOrEmpty(p_sFormatString))
                p_sFormatString = "{0}";
            if (p_asTarget != null &&
                p_asTarget.Length > 0)
                foreach (string sVal in p_asTarget)
                    sReturn += String.Format(p_sFormatString, sVal);
            return sReturn;
        }

        public static string NumEraser(string p_sSourceStr, int p_iDigitsLimit)
        {
            string TestStr = p_sSourceStr;
            TestStr = Regex.Replace(TestStr, "\\W", "", RegexOptions.Compiled);
            if (Regex.Match(TestStr, "\\d{" + p_iDigitsLimit.ToString() + ",}", RegexOptions.Compiled).Success)
                return Regex.Replace(p_sSourceStr, "\\d", "*");
            else return p_sSourceStr;
        }

        public static string GetNonEmptyFormattedString(string p_sFormat, object p_oValue)
        {
            if (p_oValue != null)
                return GetNonEmptyFormattedString(p_sFormat, p_oValue.ToString());
            return String.Empty;
        }

        public static string GetNonEmptyFormattedString(string p_sFormat, string p_sValue)
        {
            if (!String.IsNullOrEmpty(p_sFormat) &&
                !String.IsNullOrEmpty(p_sValue))
                return String.Format(p_sFormat, p_sValue);
            return String.Empty;
        }

        public static string GetFormattedString(string p_sFormat, string p_sDelimiter, object[] p_args)
        {
            return GetFormattedString(p_sFormat, p_sDelimiter, p_args, TypeConverter.ToString);
        }

        public static string GetFormattedString(string p_sFormat, string p_sDelimiter, object[] p_args, Converter<object, string> converter)
        {
            if (p_args != null &&
                p_args.Length > 0)
            {
                string sMerged = String.Empty;
                for (int i = 0; i < p_args.Length; ++i)
                    if (p_args[i] != null)
                    {
                        string sArg = converter(p_args[i]);
                        if (sArg != String.Empty)
                            sMerged = sMerged.Length > 0
                                        ? String.Format("{0}{1}{2}", sMerged, p_sDelimiter, sArg)
                                        : String.Format("{0}{1}", sMerged, sArg);
                    }
                if (sMerged != String.Empty)
                    return String.Format(p_sFormat, sMerged);
            }
            return String.Empty;
        }

        public enum eQuoteTypes
        {
            Single = 0,
            Double
        }

        public static string Quote(string p_sValue, eQuoteTypes p_eQuoteType)
        {
            switch (p_eQuoteType)
            {
                case eQuoteTypes.Double:
                    return String.Format("\"{0}\"", p_sValue);
                case eQuoteTypes.Single:
                    return String.Format("'{0}'", p_sValue);
                default:
                    return p_sValue;
            }
        }


        public static string AddSmothBR(string inputString, int maxSybolsInWord)
        {
            string outString = inputString;
            if (!String.IsNullOrEmpty(inputString))
            {
                string[] separatedStringArr = outString.Split(new char[] { ' ' });
                for (int strIndex = 0; strIndex < separatedStringArr.Length; strIndex++)
                {
                    if (separatedStringArr[strIndex].Length > maxSybolsInWord)
                    {
                        int separatorsCount = separatedStringArr[strIndex].Length / maxSybolsInWord;
                        string smothString = separatedStringArr[strIndex];
                        for (int addCount = 1; addCount < separatorsCount + 1; addCount++)
                        {
                            smothString = smothString.Insert(maxSybolsInWord * addCount, " &shy; ");
                        }
                        separatedStringArr[strIndex] = smothString;
                    }
                }
                outString = String.Join(" ", separatedStringArr);
            }
            return outString;
        }

        public static string ReplaceNonAlphaNumWithRandom(string p_sInput)
        {
            if (!String.IsNullOrEmpty(p_sInput))
                return m_regex.Replace(p_sInput, new MatchEvaluator(ReplaceNonAlphaNum));
            return String.Empty;
        }

        private static string ReplaceNonAlphaNum(Match p_match)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", String.Empty);
            return guid.Substring(new Random().Next(0, guid.Length - 1), 1);
        }

        public static string BreakText(string text, string matches, int maxLength, string replacement, bool force)
        {
            return BreakText(text, matches.ToCharArray(), maxLength, replacement, force);
        }

        /// <summary>
        /// Breaks the text inserting the given replacement after the matching symbols
        /// </summary>
        /// <param name="text">The input text</param>
        /// <param name="matches">Symbols after which the break symbol can be inserted</param>
        /// <param name="maxLength">Max length of the string</param>
        /// <param name="replacement">The breaking symbol ("&shy;" or "br")</param>
        /// <param name="force">Force the break if no matches found</param>
        /// <returns></returns>
        public static string BreakText(string text, char[] matches, int maxLength, string replacement, bool force)
        {
            if (!String.IsNullOrEmpty(text) &&
              text.Length > maxLength)
            {
                int iStartPos = 0;
                int iEndPos = maxLength;
                int iRepLen = replacement.Length;

                while (iEndPos < text.Length)
                {
                    int iIndex = text.Substring(iStartPos, iEndPos - iStartPos).LastIndexOfAny(matches);
                    if (iIndex != -1)
                    {
                        iIndex += iStartPos;
                        text = text.Insert(iIndex + 1, replacement);
                        iStartPos = iIndex + iRepLen + 1;
                    }
                    else if (force)
                    {
                        text = text.Insert(maxLength + iStartPos, replacement);
                        iStartPos += maxLength + iRepLen + 1;
                    }
                    else
                    {
                        iIndex = text.Substring(maxLength).IndexOfAny(matches);
                        if (iIndex != -1)
                        {
                            iIndex += iStartPos;
                            text = text.Insert(iIndex + 1, replacement);
                            iStartPos = iIndex + iRepLen + 1;
                        }
                        else
                            return text;
                    }
                    iEndPos = iStartPos + maxLength;
                }
            }
            return text;
        }

        public static string ShortenString(string p_str, int p_iNum)
        {
            if (p_iNum <= 0 || p_iNum >= p_str.Length)
                return p_str;
            else
                return p_str.Substring(0, p_iNum);
        }

        public static string ShortenStringWithEllipsis(string p_str, int p_iNum)
        {
            string sEllipsis = "...";
            string sShorten = ShortenString(p_str, p_iNum);
            if (sShorten.Length < p_str.Length)
                return sShorten + sEllipsis;
            else
                return sShorten;
        }

        public static string GetGuidStringWithoutDashes()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string GetStringDef(string p_value, string p_default)
        {
            if (!String.IsNullOrEmpty(p_value))
                return p_value;
            else
                return p_default;
        }

        /// <summary>
        /// Replaces host name with the given string in the specified url
        /// </summary>
        /// <param name="p_sUrl"></param>
        /// <param name="p_sHostName"></param>
        /// <returns></returns>
        public static string ReplaceHostName(string p_sUrl, string p_sHostName)
        {
            if (!String.IsNullOrEmpty(p_sUrl) &&
              !String.IsNullOrEmpty(p_sHostName))
            {
                Match m = m_rxHostName.Match(p_sUrl);
                if (m.Success)
                {
                    Group grp = m.Groups["Host"];
                    p_sUrl = p_sUrl.Remove(grp.Index, grp.Length).Insert(grp.Index, p_sHostName);
                }
            }
            return p_sUrl;
        }

        // ported from Obj project
        /// <summary>
        /// Fixes up URLs that include the ~ starting character and expanding 
        /// to a full server relative path
        /// the URL to fix up
        /// </summary>
        public static string FixupUrl(string Url)
        {
            if (Url.StartsWith("~"))
                return (HttpContext.Current.Request.ApplicationPath +
                    Url.Substring(1)).Replace("//", "/");

            return HttpContext.Current.Server.UrlEncode(Url);
        }

        public static string FormatSize(long size)
        {
            const string format = " {0}";
            if (size >= 1048576)
                return String.Format("{0:0.#}", (double)size / 1048576) + String.Format(format, Strings.MB);
            if (size >= 1024)
                return String.Format("{0:0.#}", ((double)size / 1024)) + String.Format(format, Strings.KB);
            return size + String.Format(format, Strings.Byte);
        }

        public static string FormatNameSize(string name, long size)
        {
            return String.Format("{0} ({1})", name, FormatSize(size));
        }

        public static string ReplacePhones(string input)
        {
            if (String.IsNullOrEmpty(input)) return String.Empty;
            string output = input;
            int countReplacers = 0;
            int countSeparateReplacers = 0;
            int countDigits = 0;
            for (int i = 0; i < m_numberReplacers.Length; i++)
            {
                countReplacers += new Regex(m_numberReplacers[i].ToLower()).Matches(input.ToLower()).Count;
            }

            for (int i = 0; i < m_numberSeparateReplacers.Length; i++)
            {
                string temp = input;
                int currCount = 0;
                string pattern = String.Format(m_rxPhoneReplace, m_numberSeparateReplacers[i].ToLower());

                MatchCollection mcReplacers =
                    new Regex(pattern).Matches(input.ToLower());
                currCount += mcReplacers.Count;
                foreach (Match match in mcReplacers)
                    temp = Regex.Replace(temp, Regex.Escape(match.Value), match.Value.Replace(m_numberSeparateReplacers[i], "*"), RegexOptions.IgnoreCase);

                if (currCount != 0)
                    currCount += new Regex(pattern).Matches(temp).Count;

                countSeparateReplacers += currCount;
            }
            countReplacers += countSeparateReplacers;

            for (int i = 0; i < 10; i++)
            {
                countDigits += new Regex(i.ToString()).Matches(input).Count;
            }
            if (countDigits > 4 || countReplacers > 2)
            {
                for (int i = 0; i < 10; i++)
                {
                    output = Regex.Replace(output, i.ToString(), "*");
                }
                for (int i = 0; i < m_numberReplacers.Length; i++)
                {
                    output = Regex.Replace(output, m_numberReplacers[i], "*", RegexOptions.IgnoreCase);
                }
                for (int i = 0; i < m_numberSeparateReplacers.Length; i++)
                {
                    string pattern = String.Format(m_rxPhoneReplace, m_numberSeparateReplacers[i].ToLower());
                    MatchCollection mcReplacers = new Regex(pattern, RegexOptions.IgnorePatternWhitespace).Matches(input.ToLower());
                    foreach (Match match in mcReplacers)
                    {
                        output = Regex.Replace(output, Regex.Escape(match.Value), match.Value.Replace(m_numberSeparateReplacers[i], "*"), RegexOptions.IgnoreCase);
                    }
                    foreach (Match match in mcReplacers)
                    {
                        output = Regex.Replace(output, Regex.Escape(match.Value), match.Value.Replace(m_numberSeparateReplacers[i], "*"), RegexOptions.IgnoreCase);
                    }
                }
            }
            return output;
        }

        public static string PrepareStringForRegExp(string str)
        {
            for (int i = 0; i < RegExpSpecialChars.Length; i++)
                str = str.Replace(RegExpSpecialChars[i], "\\" + RegExpSpecialChars[i]);
            return str;
        }

        public static string Utf8ToAscii(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            byte[] c = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1251), b);
            return Encoding.UTF8.GetString(c);
        }

        public static string ReplaceMultipleWhitespaces(string str)
        {
            string trimmedStr = Regex.Replace(str, @"\s{2,}", " ");
            trimmedStr = trimmedStr.Trim();
            return trimmedStr;
        }

        public static string CR2BR(string str)
        {
            return (!String.IsNullOrEmpty(str) ? str.Replace("\r", "<br/>") : String.Empty);
        }
        /// <summary>
        /// Очищает строковый параметр от тегов [] и того что внутри
        /// </summary>
        /// <param name="source">строковый параметр</param>
        /// <returns></returns>
        public static string CleanStringFromTags(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '[')
                {
                    inside = true;
                    continue;
                }
                if (let == ']')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
        /// <summary>
        /// Ощищает строковый параметр от заданных тэгов
        /// </summary>
        /// <param name="source">строка</param>
        /// <param name="quoteStart">нач. тэг</param>
        /// <param name="quoteEnd">кон. тэг</param>
        /// <returns></returns>
        public static string CleanStringFromTags(string source, string quoteStart, string quoteEnd)
        {
            int startIndex = source.LastIndexOf(quoteStart, StringComparison.InvariantCultureIgnoreCase);
            int sourceLength = source.Length;
            while (startIndex > -1)
            {
                int endIndex = source.IndexOf(quoteEnd, startIndex, StringComparison.InvariantCultureIgnoreCase);
                if (endIndex > -1)
                {
                    source = CutOffPieces(source, startIndex, endIndex - startIndex + quoteEnd.Length);
                }
                else
                {
                    source = CutOffPieces(source, startIndex, quoteStart.Length);
                }
                startIndex = source.LastIndexOf(quoteStart, StringComparison.InvariantCultureIgnoreCase);
            }
            return source;
        }

        private static string CutOffPieces(string source, int piecesIndex, int piecesLength)
        {
            int strLength = source.Length;
            if (strLength < piecesIndex + piecesLength)
            {
                return string.Empty;
            }
            return source.Substring(0, piecesIndex) +
                   source.Substring(piecesIndex + piecesLength, strLength - piecesIndex - piecesLength);
        }

        //public unsafe static string ReplaceYo(string text)
        //{
        //    const char lcYo = 'ё', lcII = 'й', ucYo = 'Ё', ucII = 'Й',
        //               lcE = 'е', lcI = 'и', ucE = 'Е', ucI = 'И';
        //    if (String.IsNullOrEmpty(text)) return text;
        //    int i, startAt = -1;
        //    fixed (char* chRef = text)
        //    {
        //        for (i = 0; i < text.Length; i++)
        //        {
        //            if (chRef[i] == lcII || chRef[i] == lcYo || chRef[i] == ucII || chRef[i] == ucYo)
        //            {
        //                startAt = i;
        //                break;
        //            }
        //        }
        //    }
        //    if (startAt == -1)
        //        return text;

        //    string result = String.Copy(text);
        //    fixed (char* chRef = result)
        //    {
        //        for (i = startAt; i < result.Length; i++)
        //        {
        //            switch (chRef[i])
        //            {
        //                case lcII:
        //                    chRef[i] = lcI;
        //                    break;
        //                case lcYo:
        //                    chRef[i] = lcE;
        //                    break;
        //                case ucII:
        //                    chRef[i] = ucI;
        //                    break;
        //                case ucYo:
        //                    chRef[i] = ucE;
        //                    break;
        //            }
        //        }
        //    }
        //    return result;
        //}

        public static string FormatTypeName(string docTypeId, string docName)
        {
            return String.Format("{0}|{1}", docTypeId, docName);
        }

        public static string FormatTypeName(int docTypeId, string docName)
        {
            return FormatTypeName(docTypeId.ToString(), docName);
        }

        public static KeyValuePair<int, string> ParseTypeName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new KeyValuePair<int, string>(0, String.Empty);
            }
            string[] s = value.Split('|');
            int id;
            if (s.Length > 1 && Int32.TryParse(s[0], out id))
            {
                return new KeyValuePair<int, string>(id, s[1]);
            }
            return new KeyValuePair<int, string>(0, String.Empty);
        }

        public static string GetFormattedID(int ID)
        {
            NumberFormatInfo formatInfo = new NumberFormatInfo();
            formatInfo.CurrencyDecimalDigits = 0;
            formatInfo.CurrencySymbol = "";
            formatInfo.CurrencyGroupSeparator = " ";
            formatInfo.CurrencyGroupSizes = new int[1] { 3 };
            return ID.ToString("C", formatInfo);
        }

        public static bool IsWordBreakChar(char c)
        {
            return Char.IsWhiteSpace(c) || Char.IsPunctuation(c) || Char.IsSeparator(c);
        }

        //private static unsafe int IndexOfHtmlEncodingChars(string s, int startPos)
        //{
        //    int num = s.Length - startPos;
        //    fixed (char* str = s)
        //    {
        //        char* chPtr = str;
        //        char* chPtr2 = chPtr + startPos;
        //        while (num > 0)
        //        {
        //            char ch = chPtr2[0];
        //            if (ch <= '>')
        //            {
        //                switch (ch)
        //                {
        //                    case '<':
        //                    case '>':
        //                    case '"':
        //                    case '&':
        //                        return (s.Length - num);
        //                }
        //            }
        //            else if ((ch >= '\x00a0') && (ch < '\x0100'))
        //            {
        //                return (s.Length - num);
        //            }
        //            chPtr2++;
        //            num--;
        //        }
        //    }
        //    return -1;
        //}

        //public static string HtmlEncode(string s)
        //{
        //    if (s == null)
        //    {
        //        return null;
        //    }
        //    int num = IndexOfHtmlEncodingChars(s, 0);
        //    if (num == -1)
        //    {
        //        return s;
        //    }
        //    StringBuilder builder = new StringBuilder(s.Length + 5);
        //    int length = s.Length;
        //    int startIndex = 0;
        //    while (true)
        //    {
        //        if (num > startIndex)
        //        {
        //            builder.Append(s, startIndex, num - startIndex);
        //        }
        //        char ch = s[num];
        //        if (ch > '>')
        //        {
        //            builder.Append("&#");
        //            builder.Append(((int)ch).ToString(NumberFormatInfo.InvariantInfo));
        //            builder.Append(';');
        //        }
        //        else
        //        {
        //            char ch2 = ch;
        //            switch (ch2)
        //            {
        //                case '<':
        //                    builder.Append("&lt;");
        //                    break;

        //                case '>':
        //                    builder.Append("&gt;");
        //                    break;

        //                case '&':
        //                    builder.Append("&amp;");
        //                    break;

        //                case '"':
        //                    builder.Append("&quot;");
        //                    break;
        //            }
        //        }
        //        startIndex = num + 1;
        //        if (startIndex < length)
        //        {
        //            num = IndexOfHtmlEncodingChars(s, startIndex);
        //            if (num < 0)
        //            {
        //                builder.Append(s, startIndex, length - startIndex);
        //                break;
        //            }
        //        }
        //        else
        //            break;
        //    }
        //    return builder.ToString();
        //}

        public static Guid ExtractGuidFromFileName(string name)
        {
            Guid result;
            string[] arr = name.Split('.');
            foreach (string s in arr)
            {
                if (Guid.TryParse(s, out result))
                    return result;
            }
            return Guid.NewGuid();
        }

        /// <summary>
        /// Returns an original file name in the "name.ext" format, without guid
        /// </summary>
        /// <param name="name">File name in one of following formats: name.ext.guid.ext or name.guid.ext</param>
        /// <returns></returns>
        public static string GetOriginalFileName(string name)
        {
            string[] arr = name.Split('.');
            int len = arr.Length;
            // old format: name.ext.guid.ext
            if (len >= 4)
            {
                return String.Format("{0}.{1}", arr[0], arr[1]);
            }
            // new format: name.guid.ext
            if (arr.Length >= 3)
            {
                return String.Format("{0}.{1}", arr[0], arr[2]);
            }
            return name;
        }

        public static string ConvertTextLinksToHtml(string text)
        {
            Regex regx =
                new Regex(
                    REGEX_LINKS,
                    RegexOptions.IgnoreCase);
            MatchCollection mactches = regx.Matches(text);
            foreach (Match match in mactches)
            {
                text = text.Replace(match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>");
            }
            return text;
        }
        private static char[] deniedSymbols = new char[] { '!', '@', '#', '$', '%', '^', '&', '-', '_', '=', '+', '(', ')', '[', ']', '~', '{', '}', ';', '`', '№', '\'' };
        private static char[] swapSymbols = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2' };
        /// <summary>
        /// Очищает название файла от символов
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CleanFileNameFromDeniedSymbols(string fileName)
        {
            for (int i = 0; i < deniedSymbols.Length; i++)
            {
                fileName = fileName.Replace(deniedSymbols[i], swapSymbols[i]);
            }
            return fileName;
        }

        public static string RemoveHtmlTags(string input)
        {
            var regex = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.Singleline);
            return regex.Replace(input, string.Empty);
        }

        /// <summary>
        /// Makes transliteration transformation for text specified in <see cref="text"/> parameter.
        /// </summary>
        /// <param name="text">Text to transliteration transformation.</param>
        /// <returns>If <see cref="text"/> parameter is null or empty string then returns original value of this parameter, otherwise returns transformed text.</returns>
        public static string TransliterateText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var stringBuilderCapacity = text.Length * 2; // to avoid extra rearrange and copy internal array in StringBuilder.
            var stringBuilder = new StringBuilder(stringBuilderCapacity);
            foreach (var @char in text)
            {
                string transliterationValue;
                if (TransliterationTable.TryGetValue(@char, out transliterationValue))
                {
                    stringBuilder.Append(transliterationValue);
                }
                else
                {
                    stringBuilder.Append(@char);
                }
            }
            return stringBuilder.ToString();
        }

        public static string ChangeNewlineToSpace(string text)
        {
            if (text == null)
                return null;
            text = text.Replace(Environment.NewLine, " ");
            return text;
        }
    }
}