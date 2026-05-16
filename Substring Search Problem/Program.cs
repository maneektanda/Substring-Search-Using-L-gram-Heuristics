class Program
{
    static void Main(string[] args)
    {

        // Start with an empty text string, this will be populated later with random chars.
        string text = "";

        // Keeping track of what indicies have been accessed in the text.
        HashSet<int> accessedIndices;
        
        // A pseudo-random number generator, this will be used to create the text below.
        // Requires a seed to start. 
        int seed = 1;
        Random rng = new Random(seed);

        // Clears the HashSet.
        void ResetHashSet()
        {
            accessedIndices = new HashSet<int>();
        }

        // Generates a random string.
        string GenerateDnaSequence(int textLength)
        {    
            ReadOnlySpan<char> possibleDnaChars = ['A', 'C', 'T', 'G'];
            return new string(rng.GetItems(possibleDnaChars, textLength));
        }

        // Records when an index is accessed by adding it to the HashSet. 
        char Text(int textIndex)
        {
            accessedIndices.Add(textIndex);
            return text[textIndex];
        }

        // patternLength will be the length of the pattern that we want to find in the text.
        // textLength will be the length of the text.
        int patternLength = 100;
        int textLength = 1000000;

        // Ensure the HashSet is cleared for the next test run.
        ResetHashSet();

        // Assign a randomly generated string to the text variable declared above.
        text = GenerateDnaSequence(textLength);

        // Choose p so we will find it at a random location.
        // Randomly pick an index between 0 and the last possible index that will still allow the
        // pattern to fit inside the text.
        int patternLocation = rng.Next(0, textLength - patternLength);

        // Now use the existing randomly generated text and the randomly chosen starting index
        // to assign a substring of text to the string variable p. This will be the pattern we
        // need to find.
        string pattern = text[patternLocation..(patternLocation + patternLength)];  

        // Returns the optimal L-gram length.
        int optimalLgram = SubstringSearch.OptimalL(patternLength);

        // Searches the text. Returns the index of where the pattern match starts or -1 if 
        // no matches are found.
        int foundPattern = SubstringSearch.NotSoNaiveHeuristicSubstringSearch(pattern, Text, textLength, optimalLgram);


        Console.WriteLine();
        if (foundPattern == patternLocation)
        {
            Console.WriteLine("Pattern found at correct location.");
        } 
        else if (foundPattern != -1)
        {
            Console.WriteLine("Pattern not found at correct location (may be due to random occurrence of pattern elsewhere).");
            Console.WriteLine($"Pattern found at {foundPattern}.");
        }
        else
        {
            Console.WriteLine("Pattern not found.");
        }
        Console.WriteLine();
        Console.WriteLine($"Text accesses: {accessedIndices.Count()}.");




        // Returns the best Lgram length to use for a given pattern length.
        void LgramTesting()
        {
            Console.WriteLine();
            Console.Write($"Pattern Length = {patternLength}");

            int minCount = int.MaxValue;
            int bestLgram = patternLength;
            for (int lGram = 0; lGram < patternLength; lGram++)
            {
                ResetHashSet();
                int foundPattern = SubstringSearch.NotSoNaiveHeuristicSubstringSearch(pattern, Text, textLength, lGram);
                if (accessedIndices.Count < minCount)
                {
                    minCount = accessedIndices.Count;
                    bestLgram = lGram;    
                }
                
            }
            Console.WriteLine($" Best Lgram = {bestLgram} Text Length = {textLength}");
            Console.WriteLine();
        }




        // Prints the text and highlights where the pattern has been found.
        void PatternHighlighting()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Pattern");
            for (int i = 0; i < pattern.Length; i++)
            {
                Console.WriteLine($"{i} = {pattern[i]}");
            }
            Console.WriteLine($"patternLocation = {patternLocation}");
            Console.WriteLine();

            Console.WriteLine($"foundPattern = {foundPattern}");

            Console.WriteLine();
            Console.WriteLine("Text");
            bool found = false;
            int count = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (i == foundPattern) found = true;
                if (found && count < pattern.Length) {
                    Console.WriteLine($"{i} = {text[i]}  {pattern[count]}"); 
                    count ++;   
                }
                else {
                    Console.WriteLine($"{i} = {text[i]}");   
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
