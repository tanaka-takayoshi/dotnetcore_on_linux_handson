
using System.ComponentModel.DataAnnotations;

namespace AspNetCorehandson.ViewModels
{
    public class AuthorEditViewModel
    {
        public string AuthorId { get; set; }

        [Required(ErrorMessage = "著者名（名）は必須入力項目です。")]
        [RegularExpression(@"^[\u0020-\u007e]{1,20}$", ErrorMessage = "著者名（名）は半角 20 文字以内で指定してください。")]
        public string AuthorFirstName { get; set; }

        [Required(ErrorMessage = "著者名（姓）は必須入力項目です。")]
        [RegularExpression(@"^[\u0020-\u007e]{1,40}$", ErrorMessage = "著者名（姓）は半角 40 文字以内で指定してください。")]
        public string AuthorLastName { get; set; }

        [Required(ErrorMessage = "電話番号は必須入力項目です。")]
        [RegularExpression(@"^\d{3} \d{3}-\d{4}$", ErrorMessage = "電話番号は 012 345-6789 のような形式で指定してください。")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "州は必須入力項目です。")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "州は半角大文字 2 文字で指定してください。")]
        public string State { get; set; }
    }
}