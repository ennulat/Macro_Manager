using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Macro_Manager.models
{
    public static class User
    {
        private static string currentUser;
        private static SortedList userList;
        public static SortedList UserList
        {
            get { return User.userList; }
            set { User.userList = value; }
        }
        public static string CurrentUser
        {
            get { return User.currentUser; }
            set { User.currentUser = value; }
        }

        public static void loadUserList() {
            if (userList == null) {
                userList = new SortedList();
            }
            userList.Add("Niklas", "password123");
            userList.Add("Johannes", "test");
            userList.Add("Maciek", "password456");
            userList.Add("Yuria", "password789");
        }
      
    }
}
