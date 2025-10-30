using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

class Result
{

    /*
     * Complete the 'crosswordPuzzle' function below.
     *
     * The function is expected to return a STRING_ARRAY.
     * The function accepts following parameters:
     *  1. STRING_ARRAY crossword
     *  2. STRING words
     */

    public static List<string> crosswordPuzzle(List<string> crossword, string words)
    {
        char[][] grid = crossword.Select(row => row.ToCharArray()).ToArray();
    
    // Split words and sort by length (longer first for better pruning)
    string[] wordArray = words.Split(';');
    Array.Sort(wordArray, (a, b) => b.Length.CompareTo(a.Length));
    
    // Solve the puzzle
    SolveCrossword(grid, wordArray, 0);
    
    // Convert back to list of strings
    return grid.Select(row => new string(row)).ToList();
}

private static bool SolveCrossword(char[][] grid, string[] words, int wordIndex)
{
    // Base case: all words placed
    if (wordIndex >= words.Length)
        return true;
    
    string word = words[wordIndex];
    
    // Try to place the word in all possible positions
    for (int i = 0; i < 10; i++)
    {
        for (int j = 0; j < 10; j++)
        {
            // Try horizontal placement
            if (CanPlaceHorizontal(grid, word, i, j))
            {
                PlaceHorizontal(grid, word, i, j);
                if (SolveCrossword(grid, words, wordIndex + 1))
                    return true;
                RemoveHorizontal(grid, word, i, j);
            }
            
            // Try vertical placement
            if (CanPlaceVertical(grid, word, i, j))
            {
                PlaceVertical(grid, word, i, j);
                if (SolveCrossword(grid, words, wordIndex + 1))
                    return true;
                RemoveVertical(grid, word, i, j);
            }
        }
    }
    
    return false;
}

private static bool CanPlaceHorizontal(char[][] grid, string word, int row, int col)
{
    // Check if word fits horizontally starting at (row, col)
    if (col + word.Length > 10)
        return false;
    
    // Check if position is valid (either empty or matching existing letters)
    for (int i = 0; i < word.Length; i++)
    {
        char cell = grid[row][col + i];
        if (cell != '-' && cell != word[i])
            return false;
    }
    
    // Additional check: ensure we're not placing in middle of another word
    // Check left boundary
    if (col > 0 && grid[row][col - 1] != '+')
        return false;
    
    // Check right boundary
    if (col + word.Length < 10 && grid[row][col + word.Length] != '+')
        return false;
    
    return true;
}

private static bool CanPlaceVertical(char[][] grid, string word, int row, int col)
{
    // Check if word fits vertically starting at (row, col)
    if (row + word.Length > 10)
        return false;
    
    // Check if position is valid (either empty or matching existing letters)
    for (int i = 0; i < word.Length; i++)
    {
        char cell = grid[row + i][col];
        if (cell != '-' && cell != word[i])
            return false;
    }
    
    // Additional check: ensure we're not placing in middle of another word
    // Check top boundary
    if (row > 0 && grid[row - 1][col] != '+')
        return false;
    
    // Check bottom boundary
    if (row + word.Length < 10 && grid[row + word.Length][col] != '+')
        return false;
    
    return true;
}

private static void PlaceHorizontal(char[][] grid, string word, int row, int col)
{
    for (int i = 0; i < word.Length; i++)
    {
        grid[row][col + i] = word[i];
    }
}

private static void PlaceVertical(char[][] grid, string word, int row, int col)
{
    for (int i = 0; i < word.Length; i++)
    {
        grid[row + i][col] = word[i];
    }
}

private static void RemoveHorizontal(char[][] grid, string word, int row, int col)
{
    for (int i = 0; i < word.Length; i++)
    {
        // Only remove if it was part of this word (not intersecting with another word)
        if (grid[row][col + i] == word[i])
        {
            grid[row][col + i] = '-';
        }
    }
}

private static void RemoveVertical(char[][] grid, string word, int row, int col)
{
    for (int i = 0; i < word.Length; i++)
    {
        // Only remove if it was part of this word (not intersecting with another word)
        if (grid[row + i][col] == word[i])
        {
            grid[row + i][col] = '-';
        }
    }
}

}

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        List<string> crossword = new List<string>();

        for (int i = 0; i < 10; i++)
        {
            string crosswordItem = Console.ReadLine();
            crossword.Add(crosswordItem);
        }

        string words = Console.ReadLine();

        List<string> result = Result.crosswordPuzzle(crossword, words);

        textWriter.WriteLine(String.Join("\n", result));

        textWriter.Flush();
        textWriter.Close();
    }
}
