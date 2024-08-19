using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities.Helpers
{
    public class StringHelper
    {
        protected StringHelper() { }

        /// <summary>
        /// encrypt password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncryptPassword(string password)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] hashBytes = encoding.GetBytes(password);

            // Compute the SHA-1 hash
            var sha1 = SHA1.Create();

            byte[] cryptPassword = sha1.ComputeHash(hashBytes);

            return BitConverter.ToString(cryptPassword);
        }

        /// <summary>
        /// Bo cac tag html, tra ve thuan text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="isRemove"></param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// </Modified>
        public static string RemoveDangerousChars(string text, bool isRemove = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text)) return string.Empty;

                // Xóa khoảng trắng đầu cuối
                text = text.Trim();
                text = WebUtility.HtmlDecode(text);

                //Xóa nhiều hơn 1 khoảng trắng ở giữa các ký tự
                var r = new Regex(@"\s+");
                text = r.Replace(text, @" ");

                // Remove Html Tag
                text = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);

                //Nếu remove những ký tự nguy hiểm , nếu không nghĩa là chấp nhận cho nhập HTML nghĩa là chấp nhận ký tự nguy hiểm
                if (isRemove)
                {
                    //Mới thêm sonnl 01/11/2018
                    text = Pattern.Replace(text, string.Empty);
                    text = text.Replace("/", "|").Replace("\\", "|").Replace("[&]", "&").Replace("&", "[&]");
                }
            }
            catch
            {
                return string.Empty;
            }
            return text;
        }

        private static readonly Regex Pattern = new Regex("[<>\"']");//|[\n]{2}

        /// <summary>
        /// Generates the random string.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="chars">The chars.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// </Modified>
        public static string GenerateRandomString(int length, string chars)
        {
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Uni2KD2(string strKD)
        {
            string retVal = strKD;
            try
            {
                string[] arKD = {"a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                                "a", "a", "a", "a", "a", "a", "e", "e", "e", "e", "e", "e", "e", "e",
                                                "e", "e", "e", "d", "i", "i", "i", "i", "i", "o", "o", "o", "o", "o", "o",
                                                "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "u", "u", "u",
                                                "u", "u", "u", "u", "u", "u", "u", "u", "y", "y", "y", "y", "y",
                                                "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A",
                                                "A", "A", "A", "A", "A", "A", "E", "E", "E", "E", "E", "E", "E", "E",
                                                "E", "E", "E", "D", "I", "I", "I", "I ", "I ", "O", "O", "O", "O", "O", "O",
                                                "O", "O", "O", "O", "O", "O", "O", "O", "O", "O", "O", "U", "U", "U",
                                                "U", "U", "U", "U", "U", "U", "U", "U", "Y", "Y", "Y", "Y", "Y"};

                string[] arUni = {"à", "á", "ả", "ã", "ạ", "ầ", "ấ", "ẩ", "ẫ", "ậ", "â",
                                                 "ằ", "ắ", "ẳ", "ẵ", "ặ", "ă", "è", "é", "ẻ", "ẽ", "ẹ", "ề", "ế", "ể",
                                                 "ễ", "ệ", "ê", "đ", "ì", "í", "ỉ", "ĩ", "ị", "ò", "ó", "ỏ", "õ", "ọ", "ồ",
                                                 "ố", "ổ", "ỗ", "ộ", "ô", "ờ", "ớ", "ở", "ỡ", "ợ", "ơ", "ù", "ú", "ủ",
                                                 "ũ", "ụ", "ừ", "ứ", "ử", "ữ", "ự", "ư", "ỳ", "ý", "ỷ", "ỹ", "ỵ",
                                                 "À", "Á", "Ả", "Ã", "Ạ", "Ầ", "Ấ", "Ẩ", "Ẫ", "Ậ", "Â",
                                                 "Ằ", "Ắ", "Ẳ", "Ẵ", "Ặ", "Ă", "È", "É", "Ẻ", "Ẽ", "Ẹ", "Ề", "Ế", "Ể",
                                                 "Ễ", "Ệ", "Ê", "Đ", "Ì", "Í", "Ỉ", "Ĩ ", "Ị ", "Ò", "Ó", "Ỏ", "Õ", "Ọ", "Ồ",
                                                 "Ố", "Ổ", "Ỗ", "Ộ", "Ô", "Ờ", "Ớ", "Ở", "Ỡ", "Ợ", "Ơ", "Ù", "Ú", "Ủ",
                                                 "Ũ", "Ụ", "Ừ", "Ứ", "Ử", "Ữ", "Ự", "Ư", "Ỳ", "Ý", "Ỷ", "Ỹ", "Ỵ"};

                for (int i = 0; i < arUni.Length; i++)
                    retVal = retVal.Replace(arUni[i], arKD[i]);
                while (retVal.Contains("  "))
                    retVal = retVal.Replace("  ", " ");

                retVal = retVal.Replace("_", "");
            }
            catch
            {
                retVal = strKD;
            }
            return retVal;
        }

        public static readonly string Unicode = @"aAàÀảẢãÃáÁạẠăĂằẰẳẲẵẴắẮặẶâÂầẦẩẨẫẪấẤậẬbBcCdDđĐeEèÈẻẺẽẼéÉẹẸêÊềỀểỂễỄếẾệỆ fFgGhHiIìÌỉỈĩĨíÍịỊjJkKlLmMnNoOòÒỏỎõÕóÓọỌôÔồỒổỔỗỖốỐộỘơƠờỜởỞỡỠớỚợỢpPqQrRsStTu UùÙủỦũŨúÚụỤưƯừỪửỬữỮứỨựỰvVwWxXyYỳỲỷỶỹỸýÝỵỴzZ";

        public static readonly string REGEX_SPECIAL_CHARATER = @"^[^<>&""']*$";

        /// <summary>
        /// Các ký tự đặc biệt cho phép
        /// </summary>
        public static readonly string AllowSpecialCharacters = "`~!@#$%^*()\\-_+={}[\\]:;|\\\\,.?";

        public static readonly string REGEX_PHONE_NUMBER = @"^(84|0[3|5|7|8|9]|02[0-9])+([0-9]{8})\b$";

        public static readonly string REGEX_EMAIL = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

        public static readonly string REGEX_PASSWORD = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[!*@#$%^&+=]).*$";

        public static bool ValidPhoneNumer(string phoneNumber, string lengthAndPrefixPhoneNumber = "10-09,086,088,089,020,032,033,034,035,036,037,038,039,070,079,077,076,078,083,084,085,081,082,056,058,059")
        {
            Regex rx = new Regex("^[0-9]+$");

            // Nhập chưa ký tự
            if (!rx.IsMatch(phoneNumber))
            {
                return false;
            }

            // Chỉ lấy lại kí tự số
            phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Nhập linh tinh là biến
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }

            // Danh sach do dai sdt va dau so tuong ung
            var lstlengthAndPrefixNumberStr = lengthAndPrefixPhoneNumber.Split(';');

            var lstValid = lstlengthAndPrefixNumberStr.Select(item => item.Split('-')).
                ToDictionary(lf => int.Parse(lf[0]), lf => lf[1].Split(',').ToList());

            // So dien thoai co do dai tuong ung va dau so tuong ung voi do dai
            if (lstValid.Where(valid => phoneNumber.Length == valid.Key).
                Any(valid => valid.Value.Any(pre => phoneNumber.IndexOf(pre, StringComparison.Ordinal) == 0)))
            {
                return true;
            }

            return false;
        }


        public static string FindStringDupplicate(string str1, string str2)
        {
            // Chuyển đổi chuỗi số thành mảng các số nguyên
            var nums1 = str1.Split(',').ToList();
            var nums2 = str2.Split(',').ToList();

            // Tạo HashSet để lưu các số trong nums1
            var set1 = new HashSet<string>(nums1);

            // Tạo danh sách để lưu các số trùng nhau
            var commonNumbers = new List<string>();

            // Duyệt qua nums2 để tìm các số trùng nhau
            foreach (var num in nums2)
            {
                if (set1.Contains(num))
                {
                    commonNumbers.Add(num);
                }
            }

            // Xây dựng chuỗi kết quả từ danh sách các số trùng nhau
            string result = string.Join(",", commonNumbers);

            return result;
        }

    }
}
