using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Timers;

namespace Viktoryna
{
    [Serializable]
    public class User :ISerializable
    {
        private string login;
        private string password;
        private string nickName;
        private string birthDay;
        private string birthMonth;
        private string birthYear;
        private Dictionary<string, int> resVictorinsUser = new Dictionary<string, int>();

        private Timer timer = new Timer(1000);
        private byte minutes = 20, seconds = 0;
        private int cntQuestion = 0;
        private List<Question> questions = new List<Question>();
        private void SetListQuestion(List<Question> _questions) { questions = _questions; }


        public User() { }

        public User(string _login, string _password, string _nickName, string _birthDay, string _birthMonth, string _birthYear)
        {
            login = _login;
            password = _password;
            nickName = _nickName;
            birthDay = _birthDay;
            birthMonth = _birthMonth;
            birthYear = _birthYear;
        }
        public Dictionary<string, int> GetResVictorinsUser() => resVictorinsUser;
        public string GetNickName() => nickName;
        public string GetBirthDay() => birthDay;
        public string GetBirthMonth() => birthMonth;
        public string GetBirthYear() => birthYear;
        public string GetLogin() => login;
        public string GetPassword() => password;
        public bool VarifLogin(string _login) => login == _login ? true : false;
        public bool VarifPassword(string _password) => password == _password ? true : false;
        public bool VarifNickName(string _nickName) => nickName == _nickName ? true : false;
        public void FileWriteUser(List<User> listUsers)  //зберігання користувачів в файл
        {
            if (listUsers.Count != 0)
            {
                BinaryFormatter binary = new BinaryFormatter();
                try
                {
                    using (Stream fStream = File.Create("Users.bin"))
                    {
                        binary.Serialize(fStream, listUsers);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else return;
        }
        public List<User> FileReadUser()           //читання користувачів з файлу
        {
            List<User> listUsers = new List<User>();
            if (File.Exists("Users.bin"))
            {
                BinaryFormatter binary = new BinaryFormatter();
                try
                {
                    using (Stream fStream = File.OpenRead("Users.bin"))
                    {
                        listUsers = (List<User>)binary.Deserialize(fStream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return listUsers;
        }
        public User ChangeSettings(User user)  //зміна пароля та дати народження користувача
        {
            Console.Clear();
            Console.WriteLine("Введiть новий пароль:");
            string _password = Console.ReadLine();
            while (_password.Length <= 6)
            {
                Console.Clear();
                Console.WriteLine("Пароль повинен мiстити бiльше шести символiв!!!\n Введiть новий пароль:");
                _password = Console.ReadLine();
            }
            Console.Clear();
            user.password = _password;
            Console.WriteLine(" 4.  Введiть дату народження(_ _._ _._ _ _ _р.):");
            Console.WriteLine("     число мiсяця:");
            user.birthDay = Console.ReadLine();
            Console.WriteLine("     номер мiсяця:");
            user.birthMonth = Console.ReadLine();
            Console.WriteLine("     рiк:");
            user.birthYear = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("\n***** Змiна пароля та дати народження збереженi!!! *****\n");
            System.Threading.Thread.Sleep(2000);
            Console.Clear();
            return user;
        }
        public User PlayVictorin(User user, List<Question> _questions)
        {
            var list = resVictorinsUser.ToList();
            foreach (var item in list)
            {
                if (item.Key == _questions.First().GetNameVictorin) { resVictorinsUser.Remove(item.Key); }
            }
            int _point = 0;
            SetListQuestion(_questions);
            string answer;           
                timer.Elapsed += Timer_Elapsed;
                timer.Enabled = true;
                timer.AutoReset = true;
                timer.Start();
            do
            {
                answer = Console.ReadLine();
                if (answer == questions[cntQuestion].RightQuestion) { _point++; cntQuestion++; }
                else { cntQuestion++; }
                if (minutes == 0 && seconds == 0) { cntQuestion = questions.Count; }
            } while (cntQuestion != questions.Count);
            timer.Stop();
            timer.Dispose();
            resVictorinsUser.Add(_questions[0].GetNameVictorin,_point);
            //получает количество правильно отвеченных вопросов
            Console.Clear();
            Console.WriteLine($"\nВсього питань - {_questions.Count};  правильних вiдповiдей - {_point};");
            Console.ReadKey();
            return user;
        }
        public void ShowResLastVictorins()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.WriteLine($"NickName -> {nickName};\n" +
                              $"______________________________________________________________\n" +
                              $" \nОстаннi вiкторини:\n" +
                              $"______________________________________________________________\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine( $"  {"№",-5}{"Point",-11}  {"Назва вiкторини"}") ;
            var list = resVictorinsUser.ToList();
            int cnt = 0;
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var item in list)
            {
                cnt++;
                Console.WriteLine($" {cnt+".",-5}  {item.Value,-11}  {item.Key} ");
            }
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e) 
        {
            Console.Clear();
            if (minutes > 3) { Console.ForegroundColor = ConsoleColor.Green; }
            else { Console.ForegroundColor = ConsoleColor.Red; }
            Console.WriteLine($" TIMER -> {minutes}:{seconds} \n");
            Console.ResetColor();
            Console.WriteLine( $"_________________________________________________________________\n");
            questions[cntQuestion].ShowQuestion();
            if (minutes == 0 && seconds == 0) { timer.Stop(); Console.Clear(); Console.WriteLine("\n\n    Час вийшов!!! \n"); ;return; }
                if (seconds == 0) { minutes--; seconds = 60; }
            seconds--;
        }
        public User(SerializationInfo info, StreamingContext context)
        {
            login = (string)info.GetValue("login", typeof(string));
            password = (string)info.GetValue("password", typeof(string));
            nickName = (string)info.GetValue("nickName", typeof(string));
            birthDay = (string)info.GetValue("birthDay", typeof(string));
            birthMonth = (string)info.GetValue("birthMonth", typeof(string));
            birthYear = (string)info.GetValue("birthYear", typeof(string));
            resVictorinsUser = (Dictionary<string, int>)info.GetValue("resVictorinsUser", typeof(Dictionary<string, int>));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("login", login);
            info.AddValue("password", password);
            info.AddValue("nickName", nickName);
            info.AddValue("birthDay", birthDay);
            info.AddValue("birthMonth", birthMonth);
            info.AddValue("birthYear", birthYear);
            info.AddValue("resVictorinsUser", resVictorinsUser);
        }
    }
}
