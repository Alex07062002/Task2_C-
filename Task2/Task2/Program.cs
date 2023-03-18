
using System.Text;

namespace Task2
{
    class File
    {
        static void Main()
        {
            string? path;
            do
            {
                Console.WriteLine("Change the Path to File:");
                path = Console.ReadLine();
            } while (!path.EndsWith(".txt"));
            List<string> rows = OpenFile(path);
            List<Tuple<char, int>> symbols = ReadUniqueSymbols(rows);
            QuickSort(symbols, 0, symbols.Count - 1);
            do
            {
                Console.WriteLine("Change the Path to File with answer:");
                path = Console.ReadLine();
            } while (!path.EndsWith(".txt"));
            WriteInFile(symbols,path);
            foreach (var tuple in symbols)
            {
                Console.Write(tuple.Item1 + " ");
            }
            Console.Write("\n");
            foreach (var tuple in symbols)
            {
                Console.Write(tuple.Item2 + " ");
            }
        }

        static List<string> OpenFile(string path)
        {
            List<string> rows = new List<string>();
            try
            {
                FileInfo fileInfo = new FileInfo(path);
                using (FileStream fileStream = fileInfo.Open(FileMode.Open,
                           FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                            rows.Add(line);
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File is not found!!!\n" + e);
            }

            return rows;
        }

        static List<Tuple<char, int>> ReadUniqueSymbols(List<string> rows)
        {
            HashSet<Tuple<char, int>> symbols = new HashSet<Tuple<char, int>>();
            foreach (string row in rows)
            {
                foreach (char sym in row)
                {
                    symbols.Add(new Tuple<char, int>(sym, sym));
                }
            }

            return symbols.ToList();
        }

        static int SortedTuples(List<Tuple<char, int>> symbols, int low, int high)
        {
            int pivot = symbols[(low + high) / 2].Item2;
            int i = low;
            int j = high;
            while (i < j)
            {
                while (symbols[i].Item2 < pivot) i++;
                while (symbols[j].Item2 > pivot) j--;
                if (i >= j) break;
                (symbols[i], symbols[j]) = (symbols[j], symbols[i]);
            }

            return j;
        }

        static void QuickSort(List<Tuple<char, int>> symbols, int low, int high)
        {
            if (low < high)
            {
                int pivot = SortedTuples(symbols, low, high);
                QuickSort(symbols, low, pivot);
                QuickSort(symbols, pivot + 1, high);
            }
        }

        static void WriteInFile(List<Tuple<char,int>> answer, string path)
        {
            string output = "";
            foreach (var sym in answer)
            {
                output += sym.Item1;
            }
            using (FileStream stream = new FileInfo(path).Create())
            {
                byte[] symbols = new UnicodeEncoding().GetBytes(output);
                stream.Write(symbols,0,symbols.Length);
            }
        }
    }
}