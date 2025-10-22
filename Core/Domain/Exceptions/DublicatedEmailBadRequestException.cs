using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DublicatedEmailBadRequestException(string email)  : BadRequestException($"Email '{email}' is already in use.")
    {
    }
}
