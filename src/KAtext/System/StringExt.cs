using System.Collections.Generic;
using System.Text;

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>
    /// String extension methods.
    /// </summary>
    public static class StringExt
    {
        #region GeorgianToLatCharMap

        private static readonly IDictionary<char, string> GeorgianToLatCharMap = new Dictionary<char, string>
        {
            {
                'ა', "a"
            },
            {
                'ბ', "b"
            },
            {
                'გ', "g"
            },
            {
                'დ', "d"
            },
            {
                'ე', "e"
            },
            {
                'ვ', "v"
            },
            {
                'ზ', "z"
            },
            {
                'თ', "t"
            },
            {
                'ი', "i"
            },
            {
                'კ', "k"
            },
            {
                'ლ', "l"
            },
            {
                'მ', "m"
            },
            {
                'ნ', "n"
            },
            {
                'ო', "o"
            },
            {
                'პ', "p"
            },
            {
                'ჟ', "zh"
            },
            {
                'რ', "r"
            },
            {
                'ს', "s"
            },
            {
                'ტ', "t"
            },
            {
                'უ', "u"
            },
            {
                'ფ', "p"
            },
            {
                'ქ', "k"
            },
            {
                'ღ', "g"
            },
            {
                'ყ', "k"
            },
            {
                'შ', "sh"
            },
            {
                'ჩ', "ch"
            },
            {
                'ც', "ts"
            },
            {
                'ძ', "dz"
            },
            {
                'წ', "ts"
            },
            {
                'ჭ', "tch"
            },
            {
                'ხ', "kh"
            },
            {
                'ჯ', "j"
            },
            {
                'ჰ', "h"
            }
        };

        #endregion

        /// <summary>
        /// Converts Georgian unicode string to Georgian ASCII
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>Converted string</returns>
        public static string ToGeorgianASCII(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var bytes = Encoding.Unicode.GetBytes(value);

            for (var i = 0; i < bytes.Length; i += 2)
            {
                var code = (bytes[i + 1] << 8) + bytes[i];

                if (code == 0x2116)
                {
                    bytes[i] = 0x0023;
                    bytes[i + 1] = 0;
                    continue;
                }

                if (code < 0x10D0 || code > 0x10F5)
                    continue;

                bytes[i + 1] = 0;

                switch (code)
                {
                    case 0x10F1:
                        bytes[i] = 0xC7;
                        break;
                    case 0x10F2:
                        bytes[i] = 0xCE;
                        break;
                    case 0x10F3:
                        bytes[i] = 0xD5;
                        break;
                    case 0x10F4:
                        bytes[i] = 0xE2;
                        break;
                    case 0x10F5:
                        bytes[i] = 0xE5;
                        break;
                }

                if (code <= 0x10D6)
                {
                    bytes[i] = (byte)(code - 4112);
                    continue;
                }

                if (code >= 0x10D7 && code <= 0x10DC)
                {
                    bytes[i] = (byte)(code - 4111);
                    continue;
                }

                if (code >= 0x10DD && code <= 0x10E2)
                {
                    bytes[i] = (byte)(code - 4110);
                    continue;
                }

                if (code >= 0x10E3 && code <= 0x10EE)
                {
                    bytes[i] = (byte)(code - 4109);
                    continue;
                }

                if (code >= 0x10EF && code <= 0x10F0)
                {
                    bytes[i] = (byte)(code - 4108);
                }
            }

            return Encoding.Unicode.GetString(bytes);
        }

        /// <summary>
        /// Converts Georgian ASCII encoded string to Georgina Unicode
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>Converted string</returns>
        public static string ToGeorgianUnicode(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var bytes = Encoding.Unicode.GetBytes(value);

            const int delta = 0x10D0 - 0xC0;

            for (var i = 0; i < bytes.Length; i += 2)
            {
                var ansi = bytes[i];

                if (bytes[i + 1] != 0 || (ansi < 0xC0 || ansi > 0xE5))
                {
                    continue;
                }

                int code;

                switch (ansi)
                {
                    case 0xC7:
                        code = 0x10F1;
                        break;
                    case 0xCE:
                        code = 0x10F2;
                        break;
                    case 0xD5:
                        code = 0x10F3;
                        break;
                    case 0xE2:
                        code = 0x10F4;
                        break;
                    case 0xE5:
                        code = 0x10F5;
                        break;
                    default:
                        code = -1;
                        break;
                }

                if (code == -1)
                {
                    if (ansi <= 0xC6)
                    {
                        code = delta + ansi;
                    }
                    else if (ansi >= 0xC8 && ansi <= 0xCD)
                    {
                        code = delta + ansi - 1;
                    }
                    else if (ansi >= 0xCF && ansi <= 0xD4)
                    {
                        code = delta + ansi - 2;
                    }
                    else if (ansi >= 0xD6 && ansi <= 0xE1)
                    {
                        code = delta + ansi - 3;
                    }
                    else if (ansi >= 0xE3 && ansi <= 0xE4)
                    {
                        code = delta + ansi - 4;
                    }
                }

                bytes[i] = (byte)code;
                bytes[i + 1] = (byte)(code >> 8);
            }

            return Encoding.Unicode.GetString(bytes);
        }

        /// <summary>
        /// Transliterates Georgian text to Latin
        /// </summary>
        /// <param name="value">The string to transliterate</param>
        /// <param name="capitalizeFirst">Indicates first letter should be capitalized or not</param>
        /// <returns>Transliteraret string</returns>
        public static string TransliterateGeorgianToLat(this string value, bool capitalizeFirst = true)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var output = new StringBuilder(value.Length);

            foreach (var c in value)
            {
                string lat;

                if (GeorgianToLatCharMap.TryGetValue(c, out lat))
                {
                    output.Append(lat);
                }
                else
                {
                    output.Append(c);
                }
            }

            var result = output.ToString();

            return capitalizeFirst ? result.CapitalizeFirst() : result;
        }

        private static string CapitalizeFirst(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var output = new StringBuilder(value.Length).Append(char.ToUpper(value[0]));

            if (value.Length > 1)
            {
                output.Append(value.Substring(1));
            }

            return output.ToString();
        }
    }
}
