using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Application.DTOs
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
