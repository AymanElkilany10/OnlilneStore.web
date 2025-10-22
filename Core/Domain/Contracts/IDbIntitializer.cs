using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IDbIntitializer
    {
        public Task InitializeAsync();
        public Task InitializeIdentityAsync();
    }
}
