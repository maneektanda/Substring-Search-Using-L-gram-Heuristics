public class SubstringSearch {

    // Returns the last index j where p[j] != t[i+j], or -1
    static private int LastUnequalChar(string pattern, Func<int, char> Text, int textIndex)
    {
        // Iterate from end of the pattern to the start.
        for (int patternIndex = pattern.Length - 1; patternIndex >= 0; patternIndex--) {

            // Return the first index where they differ. This will be how much we fast forward by.
            if (Text(textIndex + patternIndex) != pattern[patternIndex]) { return patternIndex; }
        }

        // Otherwise return -1 if they match.
        return -1;
    }



    
    // Returns the first index textIndex where pattern == text[textIndex..(textIndex + pattern.Length)].
    // text is accessed via a function rather than array using Text.
    static public int NotSoNaiveHeuristicSubstringSearch(string pattern, Func<int, char> Text, int textLength, int optimalLgram)
    {
        // Compute all Lgrams.
        HashSet<string> lGrams = Lgrams(pattern, optimalLgram);

        // A HashSet for each char in the pattern.
        var patternChars = new HashSet<char>(pattern);

        // The starting index for checking the text.
        int textIndex = 0;

        // The amount we can move forward if the pattern isn't found.
        int moveForwardAmount = 0;

        // Keep looping while it's still possible for the pattern to be in the text.
        while (textIndex <= textLength - pattern.Length) {

            // Locate the last position of char inequality. Returns -1 if all chars equal i.e pattern is found.
            // Index returned refers to the patterns' indicies, not the texts' indicies.

            int lastUnequalIndex = LastUnequalChar(pattern, Text, textIndex);
            if (lastUnequalIndex == -1) return textIndex;

            // If this is the last check and there's still no match, return.
            if (textIndex == textLength - pattern.Length) return -1;

            // If there's no match, then get the Lgram from the end of the current window, but start from the very
            // next unchecked char that's outside of the current window.
            string textLgram = GetTextLgram(textIndex, pattern.Length, optimalLgram, Text);
            
            if (!lGrams.Contains(textLgram)) {
                // Fast forward to second char of text Lgram.
                moveForwardAmount = pattern.Length - optimalLgram + 2;
            }

            // If the unequal char is in the pattern, just move across one so we don't miss a possible match.
            else {
                moveForwardAmount = 1;
            }

            // Update the starting index for the next check.
            textIndex += moveForwardAmount;
        }
        return -1;
    }




    // Get the Lgram of the text, but starts one over from the very end of the current window.
    public static string GetTextLgram(int textIndex, int patternLength, int optimalLgram, Func<int, char> Text)
    {
        string textLgram = "";

        int startingIndex = textIndex + patternLength - optimalLgram + 1;

        for (int textLetter = startingIndex; textLetter < startingIndex + optimalLgram; textLetter++)
        {
            textLgram += Text(textLetter);
        }

        return textLgram;
    }




    // Computes all possible Lgrams for a pattern given the optimal Lgram length.
    public static HashSet<string> Lgrams(string pattern, int optimalLgram)
    {
        HashSet<string> allLgrams = new HashSet<string>();

        for (int letter = 0; letter <= pattern.Length - optimalLgram; letter++)
        {
            allLgrams.Add(pattern[letter..(letter + optimalLgram)]);
        }

        return allLgrams;
    }




    // Returns the optimal value of l for the given pattern length m
    static public int OptimalL(int patternLength)
    {
        // Implement your solution here
        return 3;
    }





















    // // Template implementation.
    // // Returns the first index i where p == t[i..(i+p.Length)]
    // // t is accessed via function rather than array
    // static public int NotSoNaiveHeuristicSubstringSearch(string pattern, Func<int, char> Text, int textLength, int optimalLgram)
    // {
    //     // Modify this function to use l-grams

    //     // A HashSet for each char in the pattern.
    //     var patternChars = new HashSet<char>(pattern);

    //     // The starting index for checking the text.
    //     int textIndex = 0;

    //     // The amount we can skip forward if the pattern isn't found.
    //     int indiciesToSkip = 0;

    //     // Keep looping while it's still possible for the pattern to be in the text.
    //     while (textIndex <= textLength - pattern.Length) {

    //         // Locate the last position of char inequality. Returns -1 if all chars equal i.e pattern is found.
    //         // Index returned refers to the patterns' indicies, not the texts' indicies.
    //         int lastUnequalIndex = LastUnequalChar(pattern, Text, textIndex);
    //         if (lastUnequalIndex == -1) return textIndex;

    //         // Skipping forward if last unequal char is not in the pattern.
    //         if (!patternChars.Contains(Text(textIndex + lastUnequalIndex))) {

    //             // Skip past the point where the char would be included in the next check.
    //             indiciesToSkip = lastUnequalIndex + 1;
    //         }

    //         // If the unequal char is in the pattern, just move across one so we don't miss a possible match.
    //         else {
    //             indiciesToSkip = 1;
    //         }

    //         // Update the starting index for the next check.
    //         textIndex += indiciesToSkip;
    //     }
    //     return -1;
    // }
}

