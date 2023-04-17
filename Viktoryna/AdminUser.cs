using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Viktoryna
{
    [Serializable]
    public class AdminUser : ISerializable
    {
        private string login;
        private string password;
        private List<string> varAnswer;
        public AdminUser() { }
        public AdminUser(string _login, string _password)
        {
            login = _login;
            password = _password;
        }

        public List<Question> AdminAddVictorin()
        {
            List<Question> victorin = new List<Question>();
            string nameVictorina, question, answers;
            byte cntAnswer=0;
            Console.WriteLine("Введiть назву вiкторини:");
            nameVictorina = Console.ReadLine();
            int numQuestion = 0;
            string isExitMenu = "0";
            while (isExitMenu!="1")
            {
                numQuestion++;
                Console.Clear();Console.Clear();
                Console.WriteLine($"Введiть питання №{numQuestion}:");
                question = Console.ReadLine();
                varAnswer = new List<string>();
                Console.WriteLine("Введiть кiлькiсть варiантiв вiдповiдей на дане питання:");
                if (Byte.TryParse(Console.ReadLine(), out cntAnswer))
                {
                    for (int i = 0; i < cntAnswer; i++)
                    {
                        Console.WriteLine($"Введiть {i + 1} варiант вiдповiдi:");
                        varAnswer.Add(Console.ReadLine());
                    }
                }
                Console.Clear();
                Console.WriteLine($"Введiть номери правильних вiдповiдей:");
                answers = Console.ReadLine();
                victorin.Add(new Question(nameVictorina, question, varAnswer, answers));
                Console.Clear();
                Console.WriteLine($"Питання додано до вiкторини {nameVictorina}.\n\n" +
                                  $"    1          - вийти з цього меню;\n" +
                                  $" any else key  - продовжити додавати питання;\n");
                isExitMenu = Console.ReadLine();
            }
            return victorin;
        }
        public List<List<Question>> EditVictorin(List<List<Question>> listVictorins) //редагування вікторини
        {
            string key = " ";
            while (key!="0") {
                Console.Clear();
                Console.WriteLine("________________________________________________________\n" +
                                  "  1 - редагувати вiкторину i її питання;\n" +
                                  "  2 - видалити вiкторину\n" +
                                  "  0 - повернутись до попереднього меню адмiнкористувача;\n" +
                                  "________________________________________________________\n");
                
                key = Console.ReadLine();
                if(key == "2")
                {
                    Console.Clear();
                    Console.WriteLine("________________________________________________________\n" +
                                      "  0 - повернутись до меню адмiнкористувача;\n" +
                                      "________________________________________________________\n");
                    Console.WriteLine("Доступнi вiкторини:");
                    for (int i = 0; i < listVictorins.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}.  {listVictorins[i][0].GetNameVictorin};  ");
                    }
                    Console.WriteLine("\nВиберiть номер вiкторини для видалення: ");
                    if (Int32.TryParse(Console.ReadLine(), out int res) != true) break;
                    if (res == 0) break;
                    for (int i = 0; i < listVictorins.Count; i++)
                    {
                        if (res == i + 1)
                        {
                            Console.Clear();
                            Console.WriteLine($" Ви дiйсно хочете видалити вiкторину -> \"{listVictorins[i][0].GetNameVictorin}\" \n" +
                                      "________________________________________________________\n" +
                                      "  1 - так;\n" +
                                      "  2 - нi\n" +
                                      "________________________________________________________\n");
                            string yes_no = Console.ReadLine();
                            if (yes_no == "1") { listVictorins.RemoveAt(i); Console.Clear();
                                Console.WriteLine("\n*****Вiкторину видалено!*****\n");System.Threading.Thread.Sleep(2000);break;
                            }
                            if (yes_no == "2") { break; }
                        }
                    }
                }
                if (key == "1")
                {
                    Console.Clear();
                    Console.WriteLine("________________________________________________________\n" +
                                      "  0 - повернутись до меню адмiнкористувача;\n" +
                                      "________________________________________________________\n");
                    Console.WriteLine("Доступнi вiкторини:");
                    for (int i = 0; i < listVictorins.Count; i++)
                    {
                        Console.WriteLine($"{i+1}.  {listVictorins[i][0].GetNameVictorin};  ");
                    }
                    Console.WriteLine("\nВиберiть номер вiкторини для редагування: ");
                    if (Int32.TryParse(Console.ReadLine(),out int res)!=true) break;
                    if (res==0 ) break;
                    for (int i = 0; i < listVictorins.Count; i++)
                    {
                        if (res == i+1)
                        {
                            Console.Clear();
                            Console.WriteLine($"________________________________________________________\n" +
                                              $" Назва вiкторини: {listVictorins[i][0].GetNameVictorin}; \n" +
                                              $" Кiлькiсть питань у вiкторинi: {listVictorins[i].Count}\n" +
                                              $"________________________________________________________\n" +
                                              $"  1 - подивитись питання та вiдповiдi;\n" +
                                              $"  2 - редагувати вiкторину(назву, питання, вiдповiдi);\n" +
                                              $"  0 - повернутись до меню адмiнкористувача;\n" +
                                              $"________________________________________________________\n");
                            key = Console.ReadLine();
                            if (key == "0") break;
                            if (key == "1") {
                                Console.Clear();
                                foreach (var item in listVictorins[i])
                                {
                                    item.ShowQuestion();
                                    Console.WriteLine($" Вiрний варiант вiдповiдi: {item.RightQuestion}\n" +
                                        $"___________________________________________________________________________\n");
                                }
                                Console.ReadKey();
                            }
                            if (key == "2") {
                                Console.Clear();
                                Console.WriteLine($"________________________________________________________\n" +
                                              $"  1 - змiнити назву вiкторини;\n" +
                                              $"  2 - змiнити питання та вiдповiдi);\n" +
                                              $"  0 - повернутись до меню адмiнкористувача;\n" +
                                              $"________________________________________________________\n");
                                key = Console.ReadLine();
                                if (key == "0") break;
                                if (key == "1")
                                {
                                    Console.WriteLine("Введiть нову назву вiкторини: ");
                                    listVictorins[i][i].SetNameVictorin(Console.ReadLine());
                                    Console.Clear();
                                    Console.WriteLine("\n*****Назву вікторини успішно змінено!*****\n");
                                    System.Threading.Thread.Sleep(2000);
                                }
                                if (key == "2")
                                {
                                    Console.WriteLine("Введiть нове питання: ");
                                    listVictorins[i][i].SetQuestion(Console.ReadLine());
                                    Console.WriteLine("Введiть кiлькiсть варiантiв вiдповiдей на дане питання:");
                                    if (Byte.TryParse(Console.ReadLine(), out byte cntAnswer))
                                    {
                                        listVictorins[i][i].ClearVarQuestion();
                                        varAnswer = new List<string>();
                                        for (int j = 0; j < cntAnswer; j++)
                                        {
                                            Console.WriteLine($"Введiть {j + 1} варiант вiдповiдi:");
                                            varAnswer.Add(Console.ReadLine());
                                        }
                                        listVictorins[i][i].SetVarQuestion(varAnswer);
                                    }
                                    Console.Clear();
                                    Console.WriteLine($"Введiть номери правильних вiдповiдей:");
                                    listVictorins[i][i].SetRightAnswers(Console.ReadLine());
                                    Console.Clear();
                                    Console.WriteLine("\n*****Питання змiнено!*****\n");
                                    System.Threading.Thread.Sleep(2000);
                                }
                            }
                        }
                    }

                }
            }
            return listVictorins;
        }
        public void ShowRightAnswers(List<List<Question>> listVictorins) //перегляд правильних відповідей на питання
        {
            Console.Clear();
            Console.WriteLine("Доступнi вiкторини:");
            for (int i = 0; i < listVictorins.Count; i++)
            {
                Console.WriteLine($"{i + 1}.  {listVictorins[i][0].GetNameVictorin};  ");
            }
            Console.WriteLine("\nВиберiть номер вiкторини для перегляду вiрних вiдповiдей: ");
            if (Int32.TryParse(Console.ReadLine(), out int res) && res > 0 || res <= listVictorins.Count)
            {
                for (int i = 0; i < listVictorins.Count; i++)
                {
                    if (i == res - 1)
                    {
                        for (int j = 0; j < listVictorins[i].Count; j++)
                        {
                            Console.WriteLine($"{j + 1}.  " + listVictorins[i][j].RightQuestion);
                        }
                    }
                }
                Console.ReadKey();
            }
            else return;
        }
        public void FileWriteAdminUser(AdminUser admin)  //зберігання адмінкористувача в файл
        {
            if (admin != null)
            {
                BinaryFormatter binary = new BinaryFormatter();
                try
                {
                    using (Stream fStream = File.Create("AdminUsers.bin"))
                    {
                        binary.Serialize(fStream, admin);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else return;
        }
        public AdminUser FileReadAdminUser()           //читання адмінкористувача з файлу
        {
            AdminUser admin = new AdminUser();
            if (File.Exists("AdminUsers.bin"))
            {
                BinaryFormatter binary = new BinaryFormatter();
                try
                {
                    using (Stream fStream = File.OpenRead("AdminUsers.bin"))
                    {
                        admin = (AdminUser)binary.Deserialize(fStream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return admin;
        }
        public bool VarifLogin(string _login) => login == _login ? true : false;
        public bool VarifPassword(string _password) => password == _password ? true : false;
        public AdminUser(SerializationInfo info, StreamingContext context)
        {
            login = (string)info.GetValue("login", typeof(string));
            password = (string)info.GetValue("password", typeof(string));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("login", login);
            info.AddValue("password", password);
        }

  

    }
}
