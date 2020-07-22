using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;

namespace GuessEngine
{
    /// <summary>
    /// Главный класс программы. Через него нужно должен обращаться интерфейс
    /// </summary>
    public partial class GuessDriver
    {
        public GuessDriver(IOPort Port)
        {
            // Инициализируем список слов
            wordsList = new List<string>(  wordStorage.ToUpper().Split(new char[] { '\r','\n' }, StringSplitOptions.RemoveEmptyEntries));

            // Класс-посредник для вывода результатов в интерфейс
            _port = Port;
        }

        private static bool filterWords(string word, string Letters)
        {
            // Берём набор буков
            for (int i=0;i<Letters.Length;i++)
            {
                // И перебираем набор побуквенно
                int wordPosition = word.IndexOf(Letters[i]);
                if (wordPosition < 0)
                    continue;
                // Если в слове есть буква - удаляем её
                word = word.Remove(wordPosition,1);
            }

            // Если из слова не удалились все буквы после прохода по набору - это означает, что в слове есть буквы, не входящие в набор
            //   а по заданным нами правилам, все буквы слова должны входить в набор (Letters данном случае)
            if (word.Length != 0)
                return false;
            return true;
        }

        private List<string> getWords(string Template, string Letters)
        {
            string modifiedPattern = "^" + Template.Replace('*', '.') + "$";

            // Сначала отфильтруем слова по шаблону
            List<string> Result = wordsList.Where( item => Regex.Match(item, modifiedPattern).Success).ToList<string>();
            
            // А потом отфильтруем эти слова по набору буков, который у нас есть
            Result = Result.Where(item => filterWords( item,Letters )).ToList<string>();

            return Result;
        }
        
        public async void findWords(string Template, string Letters)
        {
            // Асинхронно запускаем поиск слов
            List<string> _list = await Task.Run(() => getWords(Template, Letters));
            if (_list.Count==0)
            {
                _port.printFindedData(new List<string>() { "Ничего не найдено" });
            }
            else
            {
                _port.printFindedData(_list);
            }
            
        }

        private IOPort _port;
        private List<string> wordsList = null;

    }
}