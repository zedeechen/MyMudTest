﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace GameSample
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CSVElementAttribute : Attribute
    {
        public string key { get; set; }
        public CSVElementAttribute(string key) { this.key = key; }
    }

    public class CSVUtilBase
    {
        public static string[] SYMBOL_LINE = new string[] { "\r\n" };//行分割符
        public static char[] SYMBOL_FIRST = new char[] { ';' };//字段分割符
        public static char[] SYMBOL_SECOND = new char[] { ':' };//字段分割符
        public static char[] SYMBOL_THIRD = new char[] { '*' };//字段分割符
        public static char[] SYMBOL_FOURTH = new char[] { '|' };
        public static char[] SYMBOL_FIFTH = new char[] { '~' };
        public static char[] SYMBOL_SIXTH = new char[] { '-' };
        public static char[] SYMBOL_SEVENTH = new char[] { '#' };
        public static char[] SYMBOL_EIGHTH = new char[] { '&' };
        public static char[] SYMBOL_RESERVE_1 = new char[] { '$' }; // 程序用分隔符
        public static char[] SYMBOL_RESERVE_2 = new char[] { '@' }; // 现用于分割参数，参数里不要用
            
        private static Dictionary<string, int> ParseTitle(string csvTitleString)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            List<string> arr = ReaderLine(csvTitleString);
            for (int i = 0, len = arr.Count; i < len; i++)
            {
                result.Add(arr[i], i);
            }
            return result;
        }

        public static Dictionary<K, List<T>> ParseContentWithGroup<K, T>(string content, string groupKeyMark) where T : new()
        {
            Dictionary<K, List<T>> result = new Dictionary<K, List<T>>();
            string[] lineArr = content.Split(SYMBOL_LINE, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> titleDic = ParseTitle(lineArr[0]);

            List<T> temp;
            for (int i = 1, lineLen = lineArr.Length; i < lineLen; i++)//第一行是title所以i=1开始
            {
                K key = default(K);
                List<string> strArr = ReaderLine(lineArr[i]);

                T instance = System.Activator.CreateInstance<T>();
                UpdateFieldValue<K,T>(ref instance, ref key, titleDic, strArr, groupKeyMark);

                if (!result.TryGetValue(key, out temp))
                {
                    temp = new List<T>();
                    result[key] = temp;
                }
                temp.Add(instance);
            }
            return result;
        }

        private static void UpdateFieldValue<K, T>(ref T instance, ref K key, Dictionary<string, int> titleDic, List<string> strArr, string keyCSVMark)
        {
            FieldInfo[] fieldArr = instance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            for (int k = 0, fieldLen = fieldArr.Length; k < fieldLen; k++)
            {
                FieldInfo field = fieldArr[k];
                //Debug.Log(field.Name);
                System.Object[] attrArr = field.GetCustomAttributes(typeof(CSVElementAttribute), false);
                if (attrArr.Length > 0)
                {
                    CSVElementAttribute csvele = attrArr[0] as CSVElementAttribute;
                    if (!titleDic.ContainsKey(csvele.key))
                    {
                    }
                    string valueString = "";
                    try
                    {
                        valueString = strArr[titleDic[csvele.key]];
                    }
#pragma warning disable 168 // 声明了变量，但从未使用过
                    catch (Exception ex)
#pragma warning restore 168 // 声明了变量，但从未使用过
                    {
                        //Debug.Log("error key: " + csvele.key+" get value is wrong");
                    }

                    try
                    {
                        System.Object valueObject;
                        if (field.FieldType == typeof(sbyte))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(sbyte) : sbyte.Parse(valueString);
                        }
                        else if (field.FieldType == typeof(byte))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(byte) : byte.Parse(valueString);
                        }
                        else if (field.FieldType == typeof(short))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(short) : short.Parse(valueString);
                        }
                        else if (field.FieldType == typeof(ushort))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(ushort) : ushort.Parse(valueString);
                        }
                        else if (field.FieldType == typeof(int))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(int) : int.Parse(valueString);
                        }
                        else if (field.FieldType == typeof(Int64))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(Int64) : Int64.Parse(valueString);
                        }
                        else if (field.FieldType == typeof(uint))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(uint) : uint.Parse(valueString);
                        }
                        else if (field.FieldType == typeof(long))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(long) : long.Parse(valueString);
                        }
                        else if (field.FieldType == typeof(ulong))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(ulong) : ulong.Parse(valueString);
                        }
                        else if (field.FieldType == typeof(float))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(float) : float.Parse(valueString);
                        }
                        else if (field.FieldType.IsEnum)
                        {
                            valueObject = int.Parse(valueString);
                        }
                        else if (field.FieldType == typeof(bool))
                        {
                            valueObject = valueString != "0";
                        }
                        else if (field.FieldType == typeof(string))
                        {
                            valueObject = valueString;
                        }
                        else
                        {
                            throw new Exception("CSVUtil_ERROR: have not process type" + field.FieldType);
                        }
                        field.SetValue(instance, valueObject);

                        if (csvele.key == keyCSVMark)
                        {
                            key = (K)valueObject;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "[[name]]" + field.Name + "[[value]]" + valueString + "[type]" + field.FieldType);
                    }
                }
            }

            PropertyInfo[] propertyInfoArr = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int k = 0, propertyLen = propertyInfoArr.Length; k < propertyLen; k++)
            {
                PropertyInfo pi = propertyInfoArr[k];
                System.Object[] attrArr = pi.GetCustomAttributes(typeof(CSVElementAttribute), false);
                if (attrArr.Length > 0)
                {
                    CSVElementAttribute csvele = attrArr[0] as CSVElementAttribute;
                    if (!titleDic.ContainsKey(csvele.key))
                        Console.WriteLine(csvele.key);
                    string valueString = strArr[titleDic[csvele.key]];

                    try
                    {
                        System.Object valueObject;
                        if (pi.PropertyType == typeof(sbyte))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(sbyte) : sbyte.Parse(valueString);
                        }
                        else if (pi.PropertyType == typeof(byte))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(byte) : byte.Parse(valueString);
                        }
                        else if (pi.PropertyType == typeof(short))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(short) : short.Parse(valueString);
                        }
                        else if (pi.PropertyType == typeof(ushort))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(ushort) : ushort.Parse(valueString);
                        }
                        else if (pi.PropertyType == typeof(int))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(int) : int.Parse(valueString);
                        }
                        else if (pi.PropertyType == typeof(Int64))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(Int64) : Int64.Parse(valueString);
                        }
                        else if (pi.PropertyType == typeof(uint))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(uint) : uint.Parse(valueString);
                        }
                        else if (pi.PropertyType == typeof(long))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(long) : long.Parse(valueString);
                        }
                        else if (pi.PropertyType == typeof(ulong))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(ulong) : ulong.Parse(valueString);
                        }
                        else if (pi.PropertyType == typeof(float))
                        {
                            valueObject = string.IsNullOrEmpty(valueString) ? default(float) : float.Parse(valueString);
                        }
                        else if (pi.PropertyType.IsEnum)
                        {
                            valueObject = int.Parse(valueString);
                        }
                        else if (pi.PropertyType == typeof(bool))
                        {
                            valueObject = valueString != "0";
                        }
                        else if (pi.PropertyType == typeof(string))
                        {
                            valueObject = valueString;
                        }
                        else
                        {
                            throw new Exception("CSVUtil_ERROR: have not process type" + pi.PropertyType);
                        }
                        pi.SetValue(instance, valueObject, null);

                        if (csvele.key == keyCSVMark)
                        {
                            key = (K)valueObject;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "[[name]]" + pi.Name + "[[value]]" + valueString + "[type]" + pi.PropertyType);
                    }
                }
            }
        }

        public static Dictionary<K, T> ParseContent<K, T>(string content, string keyCSVMark) where T : new()
        {
            Dictionary<K, T> result = new Dictionary<K, T>();
            string[] lineArr = content.Split(SYMBOL_LINE, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> titleDic = ParseTitle(lineArr[0]);
            for (int i = 1, lineLen = lineArr.Length; i < lineLen; i++)//第一行是title所以i=1开始
            {
                K key = default(K);
                List<string> strArr = ReaderLine(lineArr[i]);

                T instance = System.Activator.CreateInstance<T>();
                FieldInfo[] fieldArr = instance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                for (int k = 0, fieldLen = fieldArr.Length; k < fieldLen; k++)
                {
                    FieldInfo field = fieldArr[k];
                    //Debug.Log(field.Name);
                    System.Object[] attrArr = field.GetCustomAttributes(typeof(CSVElementAttribute), false);
                    if (attrArr.Length > 0)
                    {
                        CSVElementAttribute csvele = attrArr[0] as CSVElementAttribute;
                        if (!titleDic.ContainsKey(csvele.key))
                        {
                        }
                        string valueString = "";
                        try
                        {
                            valueString = strArr[titleDic[csvele.key]];
                        }
#pragma warning disable 168 // 声明了变量，但从未使用过
                        catch (Exception ex)
#pragma warning restore 168 // 声明了变量，但从未使用过
                        {
                            //Debug.Log("error key: " + csvele.key+" get value is wrong");
                        }

                        try
                        {
                            System.Object valueObject;
                            if (field.FieldType == typeof(sbyte))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(sbyte) : sbyte.Parse(valueString);
                            }
                            else if (field.FieldType == typeof(byte))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(byte) : byte.Parse(valueString);
                            }
                            else if (field.FieldType == typeof(short))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(short) : short.Parse(valueString);
                            }
                            else if (field.FieldType == typeof(ushort))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(ushort) : ushort.Parse(valueString);
                            }
                            else if (field.FieldType == typeof(int))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(int) : int.Parse(valueString);
                            }
                            else if (field.FieldType == typeof(Int64))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(Int64) : Int64.Parse(valueString);
                            }
                            else if (field.FieldType == typeof(uint))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(uint) : uint.Parse(valueString);
                            }
                            else if (field.FieldType == typeof(long))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(long) : long.Parse(valueString);
                            }
                            else if (field.FieldType == typeof(ulong))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(ulong) : ulong.Parse(valueString);
                            }
                            else if (field.FieldType == typeof(float))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(float) : float.Parse(valueString);
                            }
                            else if (field.FieldType.IsEnum)
                            {
                                valueObject = int.Parse(valueString);
                            }
                            else if (field.FieldType == typeof(bool))
                            {
                                valueObject = valueString != "0";
                            }
                            else if (field.FieldType == typeof(string))
                            {
                                valueObject = valueString;
                            }
                            else
                            {
                                throw new Exception("CSVUtil_ERROR: have not process type" + field.FieldType);
                            }
                            field.SetValue(instance, valueObject);

                            if (csvele.key == keyCSVMark)
                            {
                                key = (K)valueObject;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + "[[name]]" + field.Name + "[[value]]" + valueString + "[type]" + field.FieldType);
                        }
                    }
                }

                PropertyInfo[] propertyInfoArr = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                for (int k = 0, propertyLen = propertyInfoArr.Length; k < propertyLen; k++)
                {
                    PropertyInfo pi = propertyInfoArr[k];
                    System.Object[] attrArr = pi.GetCustomAttributes(typeof(CSVElementAttribute), false);
                    if (attrArr.Length > 0)
                    {
                        CSVElementAttribute csvele = attrArr[0] as CSVElementAttribute;
                        if (!titleDic.ContainsKey(csvele.key))
                            Console.WriteLine(csvele.key);
                        string valueString = strArr[titleDic[csvele.key]];

                        try
                        {
                            System.Object valueObject;
                            if (pi.PropertyType == typeof(sbyte))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(sbyte) : sbyte.Parse(valueString);
                            }
                            else if (pi.PropertyType == typeof(byte))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(byte) : byte.Parse(valueString);
                            }
                            else if (pi.PropertyType == typeof(short))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(short) : short.Parse(valueString);
                            }
                            else if (pi.PropertyType == typeof(ushort))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(ushort) : ushort.Parse(valueString);
                            }
                            else if (pi.PropertyType == typeof(int))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(int) : int.Parse(valueString);
                            }
                            else if (pi.PropertyType == typeof(Int64))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(Int64) : Int64.Parse(valueString);
                            }
                            else if (pi.PropertyType == typeof(uint))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(uint) : uint.Parse(valueString);
                            }
                            else if (pi.PropertyType == typeof(long))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(long) : long.Parse(valueString);
                            }
                            else if (pi.PropertyType == typeof(ulong))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(ulong) : ulong.Parse(valueString);
                            }
                            else if (pi.PropertyType == typeof(float))
                            {
                                valueObject = string.IsNullOrEmpty(valueString) ? default(float) : float.Parse(valueString);
                            }
                            else if (pi.PropertyType.IsEnum)
                            {
                                valueObject = int.Parse(valueString);
                            }
                            else if (pi.PropertyType == typeof(bool))
                            {
                                valueObject = valueString != "0";
                            }
                            else if (pi.PropertyType == typeof(string))
                            {
                                valueObject = valueString;
                            }
                            else
                            {
                                throw new Exception("CSVUtil_ERROR: have not process type" + pi.PropertyType);
                            }
                            pi.SetValue(instance, valueObject, null);

                            if (csvele.key == keyCSVMark)
                            {
                                key = (K)valueObject;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + "[[name]]" + pi.Name + "[[value]]" + valueString + "[type]" + pi.PropertyType);
                        }
                    }
                }
                if (result.ContainsKey(key))
                {
                    Console.WriteLine("CSVUtil_ERROR:duplicate col name:" + key);
                }
                result.Add(key, instance);
            }

            return result;
        }

        /// <summary>
        /// 拆分一行csv数据
        /// </summary>
        /// <param name="line"></param>
        /// <param name="expectedReturnValue"></param>
        /// <param name="emptyLineBehavior"></param>
        /// <returns></returns>
        public static List<string> ReaderLine(string line, bool expectedReturnValue = true, EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
        {
            List<string> columns = new List<string>();
            bool result;
            System.IO.Stream stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(line));
            using (CsvFileReader reader = new CsvFileReader(stream, emptyLineBehavior))
            {
                result = reader.ReadRow(columns);
            }
            //Assert(result == expectedReturnValue);
            return columns;
        }
    }
}

/// <summary>
/// Determines how empty lines are interpreted when reading CSV files.
/// These values do not affect empty lines that occur within quoted fields
/// or empty lines that appear at the end of the input file.
/// </summary>
public enum EmptyLineBehavior
{
    /// <summary>
    /// Empty lines are interpreted as a line with zero columns.
    /// </summary>
    NoColumns,
    /// <summary>
    /// Empty lines are interpreted as a line with a single empty column.
    /// </summary>
    EmptyColumn,
    /// <summary>
    /// Empty lines are skipped over as though they did not exist.
    /// </summary>
    Ignore,
    /// <summary>
    /// An empty line is interpreted as the end of the input file.
    /// </summary>
    EndOfFile,
}

/// <summary>
/// Common base class for CSV reader and writer classes.
/// </summary>
public abstract class CsvFileCommon
{
    /// <summary>
    /// These are special characters in CSV files. If a column contains any
    /// of these characters, the entire column is wrapped in double quotes.
    /// </summary>
    protected char[] SpecialChars = new char[] { ',', '"', '\r', '\n' };

    // Indexes into SpecialChars for characters with specific meaning
    private const int DelimiterIndex = 0;
    private const int QuoteIndex = 1;

    /// <summary>
    /// Gets/sets the character used for column delimiters.
    /// </summary>
    public char Delimiter
    {
        get { return SpecialChars[DelimiterIndex]; }
        set { SpecialChars[DelimiterIndex] = value; }
    }

    /// <summary>
    /// Gets/sets the character used for column quotes.
    /// </summary>
    public char Quote
    {
        get { return SpecialChars[QuoteIndex]; }
        set { SpecialChars[QuoteIndex] = value; }
    }
}

/// <summary>
/// Class for reading from comma-separated-value (CSV) files
/// </summary>
public class CsvFileReader : CsvFileCommon, IDisposable
{
    // Private members
    private StreamReader Reader;
    private string CurrLine;
    private int CurrPos;
    private EmptyLineBehavior EmptyLineBehavior;

    /// <summary>
    /// Initializes a new instance of the CsvFileReader class for the
    /// specified stream.
    /// </summary>
    /// <param name="stream">The stream to read from</param>
    /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
    public CsvFileReader(Stream stream,
        EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
    {
        Reader = new StreamReader(stream);
        EmptyLineBehavior = emptyLineBehavior;
    }

    /// <summary>
    /// Initializes a new instance of the CsvFileReader class for the
    /// specified file path.
    /// </summary>
    /// <param name="path">The name of the CSV file to read from</param>
    /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
    public CsvFileReader(string path,
        EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
    {
        Reader = new StreamReader(path);
        EmptyLineBehavior = emptyLineBehavior;
    }

    /// <summary>
    /// Reads a row of columns from the current CSV file. Returns false if no
    /// more data could be read because the end of the file was reached.
    /// </summary>
    /// <param name="columns">Collection to hold the columns read</param>
    public bool ReadRow(List<string> columns)
    {
        // Verify required argument
        if (columns == null)
            throw new ArgumentNullException("columns");

        ReadNextLine:
        // Read next line from the file
        CurrLine = Reader.ReadLine();
        CurrPos = 0;
        // Test for end of file
        if (CurrLine == null)
            return false;
        // Test for empty line
        if (CurrLine.Length == 0)
        {
            switch (EmptyLineBehavior)
            {
                case EmptyLineBehavior.NoColumns:
                    columns.Clear();
                    return true;
                case EmptyLineBehavior.Ignore:
                    goto ReadNextLine;
                case EmptyLineBehavior.EndOfFile:
                    return false;
            }
        }

        // Parse line
        string column;
        int numColumns = 0;
        while (true)
        {
            // Read next column
            if (CurrPos < CurrLine.Length && CurrLine[CurrPos] == Quote)
                column = ReadQuotedColumn();
            else
                column = ReadUnquotedColumn();
            // Add column to list
            if (numColumns < columns.Count)
                columns[numColumns] = column;
            else
                columns.Add(column);
            numColumns++;
            // Break if we reached the end of the line
            if (CurrLine == null || CurrPos == CurrLine.Length)
                break;
            // Otherwise skip delimiter
            //Debug.Assert(CurrLine[CurrPos] == Delimiter);
            CurrPos++;
        }
        // Remove any unused columns from collection
        if (numColumns < columns.Count)
            columns.RemoveRange(numColumns, columns.Count - numColumns);
        // Indicate success
        return true;
    }

    /// <summary>
    /// Reads a quoted column by reading from the current line until a
    /// closing quote is found or the end of the file is reached. On return,
    /// the current position points to the delimiter or the end of the last
    /// line in the file. Note: CurrLine may be set to null on return.
    /// </summary>
    private string ReadQuotedColumn()
    {
        // Skip opening quote character
        //Debug.Assert(CurrPos < CurrLine.Length && CurrLine[CurrPos] == Quote);
        CurrPos++;

        // Parse column
        StringBuilder builder = new StringBuilder();
        while (true)
        {
            while (CurrPos == CurrLine.Length)
            {
                // End of line so attempt to read the next line
                CurrLine = Reader.ReadLine();
                CurrPos = 0;
                // Done if we reached the end of the file
                if (CurrLine == null)
                    return builder.ToString();
                // Otherwise, treat as a multi-line field
                builder.Append(Environment.NewLine);
            }

            // Test for quote character
            if (CurrLine[CurrPos] == Quote)
            {
                // If two quotes, skip first and treat second as literal
                int nextPos = (CurrPos + 1);
                if (nextPos < CurrLine.Length && CurrLine[nextPos] == Quote)
                    CurrPos++;
                else
                    break;  // Single quote ends quoted sequence
            }
            // Add current character to the column
            builder.Append(CurrLine[CurrPos++]);
        }

        if (CurrPos < CurrLine.Length)
        {
            // Consume closing quote
            //Debug.Assert(CurrLine[CurrPos] == Quote);
            CurrPos++;
            // Append any additional characters appearing before next delimiter
            builder.Append(ReadUnquotedColumn());
        }
        // Return column value
        return builder.ToString();
    }

    /// <summary>
    /// Reads an unquoted column by reading from the current line until a
    /// delimiter is found or the end of the line is reached. On return, the
    /// current position points to the delimiter or the end of the current
    /// line.
    /// </summary>
    private string ReadUnquotedColumn()
    {
        int startPos = CurrPos;
        CurrPos = CurrLine.IndexOf(Delimiter, CurrPos);
        if (CurrPos == -1)
            CurrPos = CurrLine.Length;
        if (CurrPos > startPos)
            return CurrLine.Substring(startPos, CurrPos - startPos);
        return String.Empty;
    }

    // Propagate Dispose to StreamReader
    public void Dispose()
    {
        Reader.Dispose();
    }
}

/// <summary>
/// Class for writing to comma-separated-value (CSV) files.
/// </summary>
public class CsvFileWriter : CsvFileCommon, IDisposable
{
    // Private members
    private StreamWriter Writer;
    private string OneQuote = null;
    private string TwoQuotes = null;
    private string QuotedFormat = null;

    /// <summary>
    /// Initializes a new instance of the CsvFileWriter class for the
    /// specified stream.
    /// </summary>
    /// <param name="stream">The stream to write to</param>
    public CsvFileWriter(Stream stream)
    {
        Writer = new StreamWriter(stream);
    }

    /// <summary>
    /// Initializes a new instance of the CsvFileWriter class for the
    /// specified file path.
    /// </summary>
    /// <param name="path">The name of the CSV file to write to</param>
    public CsvFileWriter(string path)
    {
        Writer = new StreamWriter(path);
    }

    /// <summary>
    /// Writes a row of columns to the current CSV file.
    /// </summary>
    /// <param name="columns">The list of columns to write</param>
    public void WriteRow(List<string> columns)
    {
        // Verify required argument
        if (columns == null)
            throw new ArgumentNullException("columns");

        // Ensure we're using current quote character
        if (OneQuote == null || OneQuote[0] != Quote)
        {
            OneQuote = String.Format("{0}", Quote);
            TwoQuotes = String.Format("{0}{0}", Quote);
            QuotedFormat = String.Format("{0}{{0}}{0}", Quote);
        }

        // Write each column
        for (int i = 0; i < columns.Count; i++)
        {
            // Add delimiter if this isn't the first column
            if (i > 0)
                Writer.Write(Delimiter);
            // Write this column
            if (columns[i].IndexOfAny(SpecialChars) == -1)
                Writer.Write(columns[i]);
            else
                Writer.Write(QuotedFormat, columns[i].Replace(OneQuote, TwoQuotes));
        }
        Writer.WriteLine();
    }

    // Propagate Dispose to StreamWriter
    public void Dispose()
    {
        Writer.Dispose();
    }

}
