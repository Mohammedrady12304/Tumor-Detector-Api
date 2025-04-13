using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorDetector.Application.Dtos.AccountDtos
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)] //عشان تظهر ال password ***
        [Required]
        public string Password { get; set; }

    }
}
