using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace CodeerFriendlyExample
{
    public class SecureStringClass
    {
        private SecureString password;
        public SecureString Password
        {
            get { return password; }
        }

        public void SetPassword(string password)
        {
            this.password = new SecureString();

            foreach (char c in password)
                this.password.AppendChar(c);

            HasPassword = true;
        }

        public bool HasPassword { get; set; }

        public SecureStringClass(string password)
        {
            SetPassword(password);
        }
    }
}
