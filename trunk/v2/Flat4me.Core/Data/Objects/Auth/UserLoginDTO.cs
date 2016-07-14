using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.Objects.Auth
{
    public class UserLoginDTO
    {
        public UserLoginDTO()
        {

        }
        public UserLoginDTO(string providerName, string providerKey)
        {
            ProviderName = providerName;
            ProviderKey = providerKey;
        }

        public string ProviderName { get; set; }
        public string ProviderKey { get; set; }
    }
}
