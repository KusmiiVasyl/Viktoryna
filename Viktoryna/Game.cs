using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace Viktoryna
{
    class Game
    {       
        private List<User> listUsers = new List<User>();
        private AdminUser admin = new AdminUser();
        private List<List<Question>> listVictorins =new  Question().FileReadVictorins();
        
        public bool IsExit { get; set; }

        public Game()
        {
            IsExit = false;
        }

        private void ShowListVictorins()
        {
            foreach (var item1 in listVictorins)
            {
                foreach (var item2 in item1)
                {
                    item2.ShowQuestion();
                }
            }
        }
        private void TakeUsersFromFile()
        { listUsers = new User().FileReadUser(); }
        private void WriteUsersToFile()
        { new User().FileWriteUser(listUsers); }
        private void SetAdminUser()
        {
            admin = admin.FileReadAdminUser();
            string login, password;
            if (!File.Exists("AdminUsers.bin"))
            {
                Console.Clear();
                Console.WriteLine("________РЕЄСТРАЦIЯ АДМIНКОРИСТУВАЧА________\n\n" +
                                  "Введiть логiн:");
                login = Console.ReadLine();
                Console.WriteLine("Введiть пароль:");
                password = Console.ReadLine();
                admin = new AdminUser(login, password);
                admin.FileWriteAdminUser(admin);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("________АВТОРИЗАЦIЯ АДМIНКОРИСТУВАЧА________\n\n");
                int cnt = 3;
                while (cnt!=0)
                {
                    Console.WriteLine("Введiть логiн:");
                    login = Console.ReadLine();
                    cnt--;
                    if (admin.VarifLogin(login) == true)
                    {
                        cnt = 3;
                        while (cnt != 0)
                        {
                            Console.Clear();
                            Console.WriteLine("Введiть пароль:");
                            password = Console.ReadLine();
                            if (admin.VarifPassword(password) == true)
                            {
                                Console.Clear(); Console.WriteLine("\n\n     Вхiд схвалено!\n\n");
                                System.Threading.Thread.Sleep(1000); AdminEnterToVictorina(); return;
                            }
                            else { Console.WriteLine($"Пароль невiрний! Залишилось {--cnt} спроб");
                                   System.Threading.Thread.Sleep(1000);}
                        }
                        if (cnt == 0) { IsExit = true; return; }
                    }
                    Console.Clear();
                    Console.WriteLine($"Логiн невiрний!!! Залишилось {cnt} спроб.");
                    System.Threading.Thread.Sleep(1000);
                    Console.Clear();
                }
                if (cnt == 0) { IsExit = true; return; }
            }
        } 
        public void VerifyLoginPassword()      //вхід, реєстрація логіну та пароля
        {
            TakeUsersFromFile();
            listVictorins.Add(RandomVictorin());
            Console.Clear();
            string login, password, nickName, birthDay, birthMonth, birthYear;
            Console.WriteLine("Виберiть наступнi дiї:\n"+
                              "___________________________\n\n" +
                              " 1 - вхiд користувача\n" +
                              " 2 - реєстрацiя користувача\n" +
                              " 3 - вихiд з вiкторини\n\n" +
                              " 4 - вхiд для адмiна\n" +
                              "___________________________\n");
            if(Int32.TryParse(Console.ReadLine(),out int res)==true && res == 1)
            {
                Console.Clear();
                Console.WriteLine("    ПIДТВЕРДЖЕННЯ ВХОДУ КОРИСТУВАЧА    ");
                Console.WriteLine("________________________________________\n");
                int cntLogin = 3;
                while (cntLogin!=0)
                {
                    cntLogin--;
                    if (res != 3 && res != 2 && res!=4) Console.Clear(); Console.WriteLine("  Введiть логiн:"); login = Console.ReadLine();
                    for (int i = 0; i < listUsers.Count; i++)
                    {
                        if (listUsers[i].VarifLogin(login) == true)  //якщо логін користувача знайдено
                        {
                            Console.WriteLine("  Введiть пароль:");
                            int cnt = 3;
                            while (cnt != 0)
                            {
                                cnt--;
                                password = Console.ReadLine();
                                if (listUsers[i].VarifPassword(password) == true) { Console.Clear(); Console.WriteLine("\n\n     Вхiд схвалено!\n\n");
                                                                                    System.Threading.Thread.Sleep(2000); UserEnterToVictorina(i);return; } 
                                else Console.WriteLine($"Пароль невiрний! Залишилось {cnt} спроб");
                            }
                            if (cnt == 0) { Console.WriteLine("Користувача не знайдено!!!"); IsExit = true; return; }
                        }

                    }
                        Console.Clear();
                        Console.WriteLine($"\nКористувача з таким логiном не знайдено!!!\n" +
                                            $"___________________________________________________________\n" +
                                            $"Для повторного введення логiна залишилося {cntLogin} спроб!\n" +
                                            $"___________________________________________________________\n" +
                                            $"Для виходу введiть     - 3\n" +
                                            $"Для реєстрацiї введiть - 2\n" +
                                            $"Ввести логiн повторно  - any else key");
                        if (Int32.TryParse(Console.ReadLine(), out res) == true && res == 3) { IsExit = true; return; }
                        else if (res == 2) break;


                    if (res == 2) break;
                }
            }
            if (res == 2)
            {
                Console.Clear();
                Console.WriteLine("    РЕЄСТРАЦIЯ КОРИСТУВАЧА    ");
                Console.WriteLine("______________________________\n");
                Console.WriteLine(" 1.  Введiть логiн:");
                    login = VarifEnterLogin(Console.ReadLine());
                    if (login == null) { IsExit = true;return; }
                Console.WriteLine(" 2.  Введiть пароль:");
                    password = VarifPassword(Console.ReadLine());
                    if (password == null) { IsExit = true; return; }
                Console.WriteLine(" 3.  Введiть iм'я (Nick Name):");
                    nickName = VarifNickName(Console.ReadLine());
                Console.WriteLine(" 4.  Введiть дату народження(_ _._ _._ _ _ _р.):");
                Console.WriteLine("     число мiсяця:");
                    birthDay = Console.ReadLine();
                Console.WriteLine("     номер мiсяця:");
                    birthMonth = Console.ReadLine();
                Console.WriteLine("     рiк:");
                    birthYear = Console.ReadLine();
                listUsers.Add(new User(login,password,nickName,birthDay,birthMonth,birthYear));
                WriteUsersToFile();
                Console.Clear();
                Console.WriteLine("\n\n*****Користувача додано*****\n");
                System.Threading.Thread.Sleep(2000);
                return;
            }
            if(res == 3) { IsExit = true; return; }
            if (res == 4) { SetAdminUser(); }
            else
            {
                Console.WriteLine("Невiрний варiант входу у ВIКТОРИНУ!!!");
                IsExit = true;
            }
        }
        private string VarifNickName(string _nickName) //перевірка чи не співпадають NickName
        {
            if (listUsers.Count != 0)
            {
                for (int i = 0; i < listUsers.Count; i++)
                {
                    if (listUsers[i].VarifNickName(_nickName) == true)
                    {
                        Console.WriteLine("Такий NickName вже iснує!!! Введiть iнший NickName:");
                        _nickName = Console.ReadLine();
                        i--;
                    }
                }
            }
            return _nickName;
        }
        private string VarifPassword(string _password)  //перевірка введеного пароля при реєстрації
        {
            //перевірка на довжину пароля (умова більше 6 символів), три спроби
            int cnt = 0;
            while (_password.Length <= 6)
            {
                cnt++;
                Console.WriteLine("Пароль повинен мiстити бiльше шести символiв!!!\n Введiть пароль:");
                _password = Console.ReadLine();
                if (cnt == 3) return null;
            }
            //перевірка чи введений пароль не дорівнює вже існуючому паролю
            if (listUsers.Count != 0)
            {
                for (int i = 0; i < listUsers.Count; i++)
                {
                    if (listUsers[i].VarifPassword(_password) == true)
                    {
                        Console.WriteLine("Пароль не коректний!!! Введiть iнший пароль:");
                        _password = Console.ReadLine();
                        i--;
                    }
                }
            }
            return _password;
        }
        private string VarifEnterLogin(string _login)  //перевірка введеного логіна при реєстрації
        {
            //перевірка на довжину логіну (умова більше 6 символів), три спроби
            int cnt = 0;
            while (_login.Length <= 6)
            {
                cnt++;
                Console.WriteLine("Логiн повинен мiстити бiльше шести символiв!!!\n Введiть логiн:");
                _login = Console.ReadLine();
                if (cnt == 3) return null; 
            }
            //перевірка чи введений логін не дорівнює вже існуючому логіну
            if(listUsers.Count != 0)
            {
                for (int i = 0; i < listUsers.Count; i++)
                {
                    if (listUsers[i].VarifLogin(_login) == true)
                    {
                        Console.WriteLine("Такий логiн вже iснує!!! Введiть iнше iмя логiна:");
                        _login = Console.ReadLine();
                        i=-1;
                    }
                }

            }
            return _login;
        }
        public static void ShowFirstScreen()   //показ початкового екрану
        {
            Console.WriteLine("\n\n" +
                "____________ВIКТОРИНА____________\n\n\n\n\n" +
                "*******Провiр свої знання********\n\n\n\n\n" +
                " created by student KUSMII VASYL\n");
            Console.ReadKey();
        }
        private void ShowUsers()
        {
            foreach (var item in listUsers)
            {
                Console.WriteLine(item);
            }
        }
        public enum MenuUser
        {
            START_VICTORINA = 1,
            SHOW_RES_LAST_VICTORINA,
            TOP_20,
            CHANGE_SETTINGS,
            EXIT
        }
        private void ShowUserMenu()
        {
            Console.WriteLine("__________МЕНЮ КОРИСТУВАЧА__________\n\n" +
                              " 1 -  стартовать новую викторину;\n" +
                              " 2 -  посмотреть результаты своих прошлых викторин;\n" +
                              " 3 -  посмотреть Топ-20 по конкретной викторине;\n" +
                              " 4 -  изменить настройки. Можно менять пароль и дату рождения;\n" +
                              " 5 -  выход.\n" +
                              "_____________________________________\n");
        }
        private void ShowMenuAdminUser()
        {
            Console.WriteLine("__________МЕНЮ АДМIНКОРИСТУВАЧА__________\n\n" +
                              " 1 -  створити новую викторину;\n" +
                              " 2 -  редагувати вiкторину i її питання;\n" +
                              " 3 -  переглянути правильнi вiдповiдi вiкторин;\n" +
                              " 4 -  iнформацiя по користувачах;\n" +
                              " 5 -  выход;\n\n" +
                              " 6 -  вихiд з адмiн користувача (видалення адмiна);\n" +
                              "_________________________________________\n");
        }
        private void AdminEnterToVictorina()    //дії адміна після входу у меню адміна вікторини
        {
            string variantMenu;
            while (!IsExit)
            {
                Console.Clear();
                ShowMenuAdminUser();
                variantMenu = Console.ReadLine();
                if (variantMenu == "1") { listVictorins.RemoveAt(listVictorins.Count - 1); listVictorins.Add(admin.AdminAddVictorin()); new Question().FileWriteVictorins(listVictorins);return; }
                if (variantMenu == "2") { listVictorins = admin.EditVictorin(listVictorins); listVictorins.RemoveAt(listVictorins.Count - 1); new Question().FileWriteVictorins(listVictorins);return; }
                if (variantMenu == "3") { admin.ShowRightAnswers(listVictorins); }
                if (variantMenu == "4")
                {
                    Console.Clear();
                    int cnt = 0;
                    foreach (var item in listUsers)
                    {
                        cnt++;
                        Console.WriteLine("-------------------------------------------------------------------------\n");
                        Console.WriteLine($"{"№п/п",-5}    {"Логiн",-10} {"Пароль",-15} {"Дата народження",-12}");
                        Console.WriteLine($"{"  " + cnt,-5}    {item.GetLogin(),-10} {item.GetPassword(),-15} {item.GetBirthDay() + "." + item.GetBirthMonth() + "." + item.GetBirthYear(),-12}");
                        Console.WriteLine($"\nNickName -> {item.GetNickName()};\n" +
                             $" \nОстаннi вiкторини:");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"  {"№",-5}{"Point",-11}  {"Назва вiкторини"}");
                        var list = item.GetResVictorinsUser().ToList();
                        int cnt2 = 0;
                        Console.ForegroundColor = ConsoleColor.Green;
                        foreach (var item2 in list)
                        {
                            cnt2++;
                            Console.WriteLine($" {cnt2 + ".",-5}  {item2.Value,-11}  {item2.Key} ");
                        }
                        Console.ResetColor();
                        Console.WriteLine("-------------------------------------------------------------------------\n");
                    }
                    Console.ReadKey();
                }
                if (variantMenu == "5") { IsExit = true; }
                if (variantMenu == "6") { File.Delete("AdminUsers.bin"); admin = null; IsExit = true; Console.Clear(); Console.WriteLine("\n\nАдмiна видалено!!!\n\n"); System.Threading.Thread.Sleep(2000); }
            }
        }
        private void ShowTop20(string nameVictorin,string nickName)
        {
            Console.Clear();
            List<UserToTop> listTopUser = new List<UserToTop>();
            for (int i = 0; i < listUsers.Count; i++)
            {
                
                    var arr = listUsers[i].GetResVictorinsUser().ToArray();
                    foreach (var item in arr)
                    {
                        if (item.Key== nameVictorin)
                        {
                            listTopUser.Add(new UserToTop(listUsers[i].GetNickName(), item.Value, listUsers[i].GetBirthDay(), listUsers[i].GetBirthMonth(), listUsers[i].GetBirthYear()));
                        }
                    }
                  
                
            }
            string str1 = "Place", str2 = "NickName", str3 = "Point", str4 = "Birthday";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n___________________________TOP_______________________________\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Назва вiкторини: \"{nameVictorin}\" \n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"_____________________________________________________________\n" +
                              $"{str1,-8}{str2,-12}{str3,-12}{str4,-8}\n");

            listTopUser.Sort();
            int place = 0;
            foreach (var item in listTopUser)
            {
                if (item.GetNickName() == nickName)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"   {++place,-5}" + item);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"   {++place,-5}" + item);
                }
            }
            Console.WriteLine($"_____________________________________________________________\n");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }
        private void UserEnterToVictorina(int numUser)  //вхід користувача у вікторину після успішної ідентифікації
        {
            Console.Clear();
            MenuUser menuUser = MenuUser.START_VICTORINA;
            while (IsExit!=true) {
                ShowUserMenu();
                if (MenuUser.TryParse(Console.ReadLine(), out menuUser) == true && (int)menuUser > 0 && (int)menuUser < 6)
                {
                    switch (menuUser)
                    {
                        case MenuUser.START_VICTORINA:
                            Console.Clear();
                            Console.WriteLine("Доступнi вiкторини:");
                            for (int j = 0; j < listVictorins.Count; j++)
                            {
                                Console.WriteLine($"{j + 1}.  {listVictorins[j][0].GetNameVictorin};  ");
                            }
                            Console.WriteLine("\nВиберiть номер вiкторини для проходження: ");
                            if (Int32.TryParse(Console.ReadLine(), out int res) != true) break;
                            for (int j = 0; j < listVictorins.Count; j++)
                            {
                                if (res == j + 1)
                                {
                                    listUsers[numUser] = listUsers[numUser].PlayVictorin(listUsers[numUser], listVictorins[j]);
                                    WriteUsersToFile();
                                    ShowTop20(listVictorins[res-1].First().GetNameVictorin, listUsers[numUser].GetNickName());
                                }                                   
                            }
                            break;
                        case MenuUser.SHOW_RES_LAST_VICTORINA:
                            listUsers[numUser].ShowResLastVictorins();
                            break;
                        case MenuUser.TOP_20:
                            Console.Clear();
                            Console.WriteLine("\nДоступнi вiкторини:\n");
                            for (int i = 0; i < listVictorins.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}.  {listVictorins[i][0].GetNameVictorin};  ");
                            }
                            Console.WriteLine("\nВиберiть номер вiкторини для перегляду TOP20: ");
                            if (Int32.TryParse(Console.ReadLine(), out int res2) != true && res2>listVictorins.Count && res2<=0) break;
                            else {
                                ShowTop20(listVictorins[res2-1].First().GetNameVictorin,listUsers[numUser].GetNickName());
                            }
                            break;
                        case MenuUser.CHANGE_SETTINGS:
                            listUsers[numUser] = new User().ChangeSettings(listUsers[numUser]);
                            break;
                        case MenuUser.EXIT:
                            IsExit = true;
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n*****Помилка!!! Невiрно вибрана позицiя з меню.*****\n ");
                    System.Threading.Thread.Sleep(2000);
                    Console.Clear(); Console.Clear();
                }
            }
        }
        private List<Question> RandomVictorin()  //змішана вікторина
        {
            Random ran = new Random();
            int[] arr1 = new int[listVictorins.Count];
            int arr2Size = 20;
            int[] arr2 = new int[arr2Size];
            int tmp;
            for (int i = 0; i < listVictorins.Count; i++)
            {
                tmp = ran.Next(1, listVictorins.Count+1);
                for (int j = 0; j < arr1.Length; j++)
                {
                    if (tmp == arr1[j])
                    {
                        tmp = ran.Next(1, listVictorins.Count+1);
                        j = -1;
                    }
                }
                arr1[i] = tmp;
            }
            for (int i = 0; i < arr2Size; i++)
            {
                tmp = ran.Next(1, arr2Size + 1);
                for (int j = 0; j < arr2.Length; j++)
                {
                    if (tmp == arr2[j])
                    {
                        tmp = ran.Next(1, arr2Size + 1);
                        j = -1;
                    }
                }
                arr2[i] = tmp;
            }
            List<Question> randomVictorin = new List<Question>();
            int cnt = 0;
            for (int j = 0; j < arr2Size; j++)
            {
                Question question = new Question("Змiшана вiкторина", listVictorins[(arr1[cnt]) - 1][(arr2[j]) - 1].GetQuestion, listVictorins[(arr1[cnt]) - 1][(arr2[j]) - 1].GetVarQuestion,
                                                                      listVictorins[(arr1[cnt]) - 1][(arr2[j]) - 1].RightQuestion);
                randomVictorin.Add(question);
                cnt++;
                if (cnt == 3) cnt = 0;
            }
            return randomVictorin;
        }

    }
}
