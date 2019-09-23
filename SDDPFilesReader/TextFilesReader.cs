using DHOG_WPF.Models;
using DHOG_WPF.Util;
using System;
using System.IO;
using System.Linq;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.SDDPFilesReader
{
    class TextFilesReader
    {
        public static int CalculateColumns(FileStructure fileStructure, string header)
        {
            int columns = fileStructure.HeaderColumns;
            int headerLenght = header.Length;

            int headerColumnsLenght = 0;
            foreach (int headerColumnWidth in fileStructure.HeaderColumnsWidth)
                headerColumnsLenght += headerColumnWidth;

            headerLenght -= headerColumnsLenght;

            if (fileStructure.Type.Equals(FileType.Periodical))
            {
                int periodicalColumns = headerLenght / fileStructure.PeriodicalColumnsWidth;
                columns += periodicalColumns;
            }

            return columns;
        }
        
        
        public static int CalculateRows(FileStructure fileStructure, int fileLenght)
        {
            int rows = fileLenght - fileStructure.HeaderLines;

            if (fileStructure.SumNeeded) 
                rows -= 2;

            return rows;
        }


        public static int CalculateRows(FileStructure fileStructure, string fileName, int fileLenght)
        {
            int rows = 0;

            string[] lines = File.ReadAllLines(fileName);
            for (int i = fileStructure.HeaderLines; i < fileLenght; i++)
            {
                bool plantInfo = false;
                string line = lines[i];
                if (line.Contains("****"))
                {
                    i += 2;
                    plantInfo = true;
                    while (plantInfo && i < fileLenght)
                    {
                        line = lines[i];
                        if (line.Contains("****"))
                        {
                            plantInfo = false;
                            i--;
                        }
                        else
                        {
                            rows++;
                            i++;
                        }
                    }
                }
            }
            return rows;
        }


        public static string[][] ConvertFileToMatrix(FileStructure fileStructure, string fileName)
        {
            int fileLenght = File.ReadLines(fileName).Count();
            int rows;
            if (fileStructure.DividedByPlants)
                rows = CalculateRows(fileStructure, fileName, fileLenght);
            else
                rows = CalculateRows(fileStructure, fileLenght);

            string header;
            if(fileStructure.DividedByPlants)
                header = File.ReadLines(fileName).Skip(fileStructure.HeaderLines + 1).Take(1).First();
            else
                header = File.ReadLines(fileName).Skip(fileStructure.HeaderLines - 1).Take(1).First();

            int columns = CalculateColumns(fileStructure, header);

            string[][] matrixFile = new string[rows][];

            int row = 0;
            string[] lines = File.ReadAllLines(fileName);
            for (int i = fileStructure.StartLine; i < fileLenght; i++)
            {
                string line = lines[i];
                bool plantInfo = false;
                if (line.Contains("****") && fileStructure.DividedByPlants)
                {
                    string plantName = line.Substring(18, 12);
                    i += 2;
                    plantInfo = true;
                    while (plantInfo && i < fileLenght)
                    {
                        line = lines[i];
                        if (line.Contains("****"))
                        {
                            plantInfo = false;
                            i--;
                        }
                        else
                        {
                            if (line.Length == header.Length)
                            {
                                matrixFile[row] = CreateRow(fileStructure, columns + 1, line);
                                matrixFile[row][columns] = plantName;
                                row++;
                                i++;
                            }
                            else
                                throw new Exception(MessageUtil.FormatMessage("ERROR.InvalidLineSize", fileName, i+1));
                        }
                    }
                }
                else if(line.Contains("****"))
                {
                    i = i + 2;
                    line = lines[i];
                    matrixFile[row] = CreateRow(fileStructure, columns, line);
                    row++;
                }
                else
                {
                    if (line.Length == header.Length)
                    {
                        matrixFile[row] = CreateRow(fileStructure, columns, line);
                        row++;
                    }
                    else
                        throw new Exception(MessageUtil.FormatMessage("ERROR.InvalidLineSize", fileName, i + 1));
                }
            }
            return matrixFile;
        }


        public static string[] CreateRow(FileStructure fileStructure, int columns, string line)
        {
            string[] row = new string[columns];
            int initialCharacter = 0;
            for(int i = 0; i < fileStructure.HeaderColumns; i++)
            {
                int headerColumnWidth = fileStructure.HeaderColumnsWidth.ElementAt(i);
                row[i] = line.Substring(initialCharacter, headerColumnWidth);
                initialCharacter += headerColumnWidth;
                
            }
            
            if (fileStructure.Type.Equals(FileType.Periodical))
            {
                if (fileStructure.DividedByPlants)
                    columns--; // Last column is filled with the plant name

                string valueToRepeat = line.Substring(initialCharacter, fileStructure.PeriodicalColumnsWidth);
                for (int i = fileStructure.HeaderColumns; i < columns; i++)
                {
                    row[i] = line.Substring(initialCharacter, fileStructure.PeriodicalColumnsWidth);
                    initialCharacter += fileStructure.PeriodicalColumnsWidth;

                    if (fileStructure.RepeatPeriodicalValues)
                    {
                        string value = row[i].Trim();
                        if (value.Equals(""))
                            row[i] = valueToRepeat;
                        else
                            valueToRepeat = row[i];
                    }
                }
            }
            
            return row;
        }

    }
}
