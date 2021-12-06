using System;
using CsvHelper;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace App
{
    class Person
    {
        public string name { get; set; }
        public string gender { get; set; }
    }
    class Match {
        public string match { get; set; }
        public int percentage { get; set; }

    }
    public class PercentageComparer : IComparer<Match>
    {
        int IComparer<Match>.Compare(Match x, Match y)
        {
            return x.percentage.CompareTo(y.percentage);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //sets for males and females
            SortedSet<string> maleSet = new SortedSet<string>();
            SortedSet<string> femaleSet = new SortedSet<string>();

            //open and read csv file
            try
            {
                using (var reader = new StreamReader("/Users/pholosho/Projects/App/App/file.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    //hold data from csv
                    var people = csv.GetRecords<Person>();

                    //groups data according to the gender provided
                    //and removed any non alphabeticall charecters
                    foreach(Person a in people)
                    {
                        string gender = a.gender.Replace(" ",string.Empty);
                        string name = Regex.Replace(a.name, "[^A-Za-z-]", "");

                        if (gender == "f" && !maleSet.Contains(name)) {
                            femaleSet.Add(name);
                        }
                        if(gender == "m" && !femaleSet.Contains(name))
                        {
                            maleSet.Add(name);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                
                Console.WriteLine(e.ToString());
            }

            //for storing matches
            SortedSet<Match> matches = new SortedSet<Match>(new PercentageComparer());

            //matches maleSet with femaleSet
            foreach (string a in maleSet)
            {
                foreach(string b in femaleSet)
                {
                    Match myMatch = new Match();
                    myMatch.match = a + " match " + b;
                    try {
                        myMatch.percentage = match(a, b);
                        matches.Add(myMatch);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    

                }
            }
            String results = "";
            //store the matches 
            foreach(Match a in matches.Reverse())
            {
              
                //when user score is 80% or above 
                if (a.percentage >= 80) {
                    results += a.match + " " + a.percentage + "%, good match \n";
                }
                else
                {
                    results += a.match + " " + a.percentage + "%  \n";
                }

                
            }
            //prints out the results
            Console.WriteLine(results);

            try {
                File.WriteAllText("/Users/pholosho/Projects/App/App/output.txt", results);
            }catch(Exception e) {
                Console.WriteLine(e);
            }


            


        }

        //method for calculating a match
        public static int match(string name1, string name2)
        {
            string number = "";
            int count = 0;
            string viewed = "";
            int sum;


            string finalSentence = name1 + "matches" + name2;

            //change the case of the sentence
            finalSentence = finalSentence.ToLower();
            for (int i = 0; i < finalSentence.Length; i++)
            {
                if (!viewed.Contains(finalSentence[i]))
                {
                    for (int j = 0; j < finalSentence.Length; j++)
                    {

                        if (finalSentence[i] == finalSentence[j])
                        {
                            count++;

                        }

                    }
                }

                if (count != 0)
                {
                    number += count;

                }

                count = 0;
                viewed += finalSentence[i];
            }

            string tempNumber; ;
            string holder;
            int counter = 0;

            while (number.Length > 2)
            {
                tempNumber = "";
                for (int i = 0; i < (number.Length / 2); i++)
                {
                    sum = int.Parse(number[i].ToString()) + int.Parse(number[number.Length - i - 1].ToString());

                    tempNumber += sum.ToString();

                }

                holder = tempNumber;
                if (number.Length % 2 != 0)
                {
                    tempNumber += number[(holder.Length / 2) + 1].ToString();

                }

                number = tempNumber;

                counter++;


            }
            int actualNumber = int.Parse(number);

            return actualNumber;
        }
    }
}
