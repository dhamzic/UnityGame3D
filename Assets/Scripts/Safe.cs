using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class Safe
    {
        public static string Password = "3728";

        public static bool CheckPassword(string insertedPassword)
        {
            bool correct = false;

            if (insertedPassword == Password)
            {
                correct = true;
            }

            return correct;
        }
    }
}
