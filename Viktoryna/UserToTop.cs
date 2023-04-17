using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viktoryna
{
    class UserToTop : IComparable<UserToTop>
    {

        private string nickName;
        private string birthDay;
        private string birthMonth;
        private string birthYear;
        private int point;

        public UserToTop(string _nickName, int _point, string _birthDay, string _birthMonth, string _birthYear)
        {
            nickName = _nickName;
            point = _point;
            birthDay = _birthDay;
            birthMonth = _birthMonth;
            birthYear = _birthYear;
        }
        public string GetNickName() => nickName;
        public int CompareTo(UserToTop userToTop)
        {
            if (userToTop == null)
                return 1;
            else
                return userToTop.point.CompareTo(this.point);
        }

        public override string ToString()
        {
            return $"{nickName,-12} {point,-11}{birthDay,-2}.{birthMonth,-2}.{birthYear,-4}";
        }
    }
}
