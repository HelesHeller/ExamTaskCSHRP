using System;
using System.Collections.Generic;
using System.IO;

namespace ExamTaskCSHRP
{
    public class DictionaryEntry
    {
        public string Word { get; set; }
        public List<string> Translations { get; set; }
    }

    public enum DictionaryType
    {
        EnglishToRussian,
        RussianToEnglish
    }

    public interface IDictionaryManager
    {
        void CreateDictionary(DictionaryType type);
        void AddWord(DictionaryType type, DictionaryEntry entry);
        void ReplaceWord(DictionaryType type, string word, DictionaryEntry newEntry);
        void DeleteWord(DictionaryType type, string word);
        List<string> SearchTranslation(DictionaryType type, string word);
        void ExportToFile(DictionaryType type, string filePath);
    }

    public class DictionaryManager : IDictionaryManager
    {
        private Dictionary<DictionaryType, List<DictionaryEntry>> dictionaries;

        public DictionaryManager()
        {
            dictionaries = new Dictionary<DictionaryType, List<DictionaryEntry>>();
            InitializeDictionaries();
        }

        private void InitializeDictionaries()
        {
            foreach (DictionaryType type in Enum.GetValues(typeof(DictionaryType)))
            {
                dictionaries[type] = new List<DictionaryEntry>();
            }
        }

        public void CreateDictionary(DictionaryType type)
        {
            // Логіка створення словника
            Console.WriteLine($"Dictionary of type {type} created.");
        }

        public void AddWord(DictionaryType type, DictionaryEntry entry)
        {
            // Логіка додавання слова до словника
            dictionaries[type].Add(entry);
            Console.WriteLine($"Word '{entry.Word}' added to the dictionary.");
        }

        public void ReplaceWord(DictionaryType type, string word, DictionaryEntry newEntry)
        {
            // Логіка заміни слова в словнику
            var existingEntry = dictionaries[type].Find(e => e.Word.Equals(word));
            if (existingEntry != null)
            {
                dictionaries[type].Remove(existingEntry);
                dictionaries[type].Add(newEntry);
                Console.WriteLine($"Word '{word}' replaced with '{newEntry.Word}' in the dictionary.");
            }
            else
            {
                Console.WriteLine($"Word '{word}' not found in the dictionary.");
            }
        }

        public void DeleteWord(DictionaryType type, string word)
        {
            // Логіка видалення слова з словника
            var entryToRemove = dictionaries[type].Find(e => e.Word.Equals(word));
            if (entryToRemove != null)
            {
                dictionaries[type].Remove(entryToRemove);
                Console.WriteLine($"Word '{word}' removed from the dictionary.");
            }
            else
            {
                Console.WriteLine($"Word '{word}' not found in the dictionary.");
            }
        }

        public List<string> SearchTranslation(DictionaryType type, string word)
        {
            // Логіка пошуку перекладу слова в словнику
            var translations = new List<string>();
            foreach (var entry in dictionaries[type])
            {
                if (entry.Word.Equals(word))
                {
                    translations.AddRange(entry.Translations);
                }
            }
            return translations;
        }

        public void ExportToFile(DictionaryType type, string filePath)
        {
            // Логіка експорту словника в файл
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var entry in dictionaries[type])
                    {
                        writer.WriteLine($"Word: {entry.Word}");
                        writer.WriteLine("Translations: " + string.Join(", ", entry.Translations));
                        writer.WriteLine("----");
                    }
                }
                Console.WriteLine($"Dictionary exported to '{filePath}' successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting dictionary: {ex.Message}");
            }
        }
    }

    public class Program
    {
        public static void Main()
        {
            IDictionaryManager dictionaryManager = new DictionaryManager();

            // Приклад використання методів словника
            dictionaryManager.CreateDictionary(DictionaryType.EnglishToRussian);

            var entry1 = new DictionaryEntry
            {
                Word = "hello",
                Translations = new List<string> { "привіт", "добрий день" }
            };
            dictionaryManager.AddWord(DictionaryType.EnglishToRussian, entry1);

            var entry2 = new DictionaryEntry
            {
                Word = "world",
                Translations = new List<string> { "світ", "універсум" }
            };
            dictionaryManager.AddWord(DictionaryType.EnglishToRussian, entry2);

            dictionaryManager.ReplaceWord(DictionaryType.EnglishToRussian, "world",
                new DictionaryEntry { Word = "planet", Translations = new List<string> { "планета" } });

            dictionaryManager.DeleteWord(DictionaryType.EnglishToRussian, "hello");

            var translations = dictionaryManager.SearchTranslation(DictionaryType.EnglishToRussian, "planet");
            Console.WriteLine("Translations of 'planet': " + string.Join(", ", translations));

            dictionaryManager.ExportToFile(DictionaryType.EnglishToRussian, "dictionary.txt");
        }
    }
}
