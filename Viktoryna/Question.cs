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
    public class Question : ISerializable
    {
        private string nameVictorin;
        private string question;
        private List<string> varQuestion;
        private string rightAnswers;


        public string GetNameVictorin => nameVictorin;
        public string GetQuestion => question;
        public List<string> GetVarQuestion => varQuestion;
        public void SetNameVictorin(string _nameVictorin) { nameVictorin = _nameVictorin; }
        public void SetQuestion(string _question) { question = _question; }
        public void SetVarQuestion(List<string> _varQuestion) { varQuestion = _varQuestion; }
        public void SetRightAnswers(string _rightAnswers) { rightAnswers = _rightAnswers; }
        public void ClearVarQuestion() => varQuestion.Clear();
        public string RightQuestion => rightAnswers;

        public Question() { }
        public Question(string _nameVictorin,string _question, List<string> _varQuestion, string _rightAnswers)
        {
            nameVictorin = _nameVictorin;
            question = _question;
            varQuestion = _varQuestion;
            rightAnswers = _rightAnswers;
        }

        public void FileWriteVictorins(List<List<Question>> listVictorins)  //зберігання вікторин в файл
        {
            if (listVictorins.Count != 0)
            {
                BinaryFormatter binary = new BinaryFormatter();
                try
                {
                    using (Stream fStream = File.Create("Victorins.bin"))
                    {
                        binary.Serialize(fStream, listVictorins);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else return;
        }
        public List<List<Question>> FileReadVictorins()           //читання вікторин з файлу
        {
            List<List<Question>> listVictorins = new List<List<Question>>();
            if (File.Exists("Victorins.bin"))
            {
                BinaryFormatter binary = new BinaryFormatter();
                try
                {
                    using (Stream fStream = File.OpenRead("Victorins.bin"))
                    {
                        listVictorins = (List<List<Question>>)binary.Deserialize(fStream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return listVictorins;
        }
        public Question(SerializationInfo info, StreamingContext context)
        {
            nameVictorin = (string)info.GetValue("nameVictorin", typeof(string));
            question = (string)info.GetValue("question", typeof(string));
            varQuestion = (List<string>)info.GetValue("varQuestion", typeof(List<string>));
            rightAnswers = (string)info.GetValue("answers", typeof(string));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("nameVictorin", nameVictorin);
            info.AddValue("question", question);
            info.AddValue("varQuestion", varQuestion);
            info.AddValue("answers", rightAnswers);
        }
        public void ShowQuestion()
        {
            Console.WriteLine( $"Назва вiкторини:  {nameVictorin}\n" +
                               $"___________________________________________________________________________\n" +
                               $"{question}\n");
            for (int i = 0; i < varQuestion.Count; i++)
            {
                Console.WriteLine("     "+varQuestion[i]);
            }
            Console.WriteLine("___________________________________________________________________________\n");
        }
    }
}
