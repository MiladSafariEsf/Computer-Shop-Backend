using System.ComponentModel.DataAnnotations;

namespace APICH.API.Models.AAA
{
    public class ChangePasswordModel
    {
        [MinLength(8)]
        public string oldPassword { get; set; }
        [MinLength(8)]
        public string newPassword { get; set; }
    }
}
