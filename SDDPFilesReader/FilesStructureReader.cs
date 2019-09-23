using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using DHOG_WPF.Models;
using log4net;
using DHOG_WPF.Util;
using static DHOG_WPF.DataTypes.Types;

namespace DHOG_WPF.SDDPFilesReader
{
    class FilesStructureReader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FilesStructureReader));
        
        public static List<FileStructure> ReadFilesStructure()
        {
            List<FileStructure> filesStructure = new List<FileStructure>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            try
            {
                assembly = Assembly.GetExecutingAssembly();
                StreamReader fileReader = new StreamReader(assembly.GetManifestResourceStream("DHOG_WPF.Resources.FilesStructure.config"));
                fileReader.ReadLine();

                int lineNumber = 1;
                string line;
                bool validFile = true;
                while ((line = fileReader.ReadLine()) != null)
                {
                    
                    bool validLine = true;
                    lineNumber++;
                    string[] columns = line.Split('\t');

                    int column = 0;

                    try
                    {
                        string name = columns[column];
                        column++;

                        FileType type;
                        if (columns[column].Equals("Basic"))
                            type = FileType.Basic;
                        else if(columns[column].Equals("Periodical"))
                            type = FileType.Periodical;
                        else
                        {
                            type = FileType.Periodical;
                            log.Error(MessageUtil.FormatMessage("ERROR.InvalidFileType", "FilesStructure.config", lineNumber, "File_Type"));
                            validLine = false;
                        }
                        column++;

                        int headerLines;
                        if (!Int32.TryParse(columns[column], out headerLines))
                        {
                            log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", "FilesStructure.config", lineNumber, "Header_Lines"));
                            validLine = false;
                        }
                        column++;

                        bool dividedByPlants = false;
                        if (columns[column].Equals("TRUE"))
                            dividedByPlants = true;
                        else if (columns[column].Equals("FALSE"))
                            dividedByPlants = false;
                        else
                        {
                            log.Error(MessageUtil.FormatMessage("ERROR.InvalidBoolean", "FilesStructure.config", lineNumber, "Divided_By_Plants"));
                            validLine = false;
                        }
                        column++;

                        bool sumNeeded = false;
                        if (columns[column].Equals("TRUE"))
                            sumNeeded = true;
                        else if (columns[column].Equals("FALSE"))
                            sumNeeded = false;
                        else
                        {
                            log.Error(MessageUtil.FormatMessage("ERROR.InvalidBoolean", "FilesStructure.config", lineNumber, "Sum_Needed"));
                            validLine = false;
                        }
                        column++;

                        bool repeatPeriodicalValues = false;
                        if (columns[column].Equals("TRUE"))
                            repeatPeriodicalValues = true;
                        else if (columns[column].Equals("FALSE"))
                            repeatPeriodicalValues = false;
                        else
                        {
                            log.Error(MessageUtil.FormatMessage("ERROR.InvalidBoolean", "FilesStructure.config", lineNumber, "Repeat_Periodical_Values"));
                            validLine = false;
                        }
                        column++;

                        int periodicalColumnsWidth;
                        if (!Int32.TryParse(columns[column], out periodicalColumnsWidth))
                            log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", "FilesStructure.config", lineNumber, "Periodical_Columns_Width"));
                        column++;

                        int headerColumns;
                        if (!Int32.TryParse(columns[column], out headerColumns))
                        {
                            log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", "FilesStructure.config", lineNumber, "Header_Columns"));
                            validLine = false;
                        }
                        column++;

                        List<int> headerColumnsWidth = new List<int>();
                        for (int i = 0; i < headerColumns; i++)
                        {
                            int headerColumnWidht = 0;
                            if (!Int32.TryParse(columns[column], out headerColumnWidht))
                            {
                                log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", "FilesStructure.config", lineNumber, "Header_Column" + (i+1) + "_Width"));
                                validLine = false;
                            }
                            else
                                headerColumnsWidth.Add(headerColumnWidht);

                            column++;
                        }

                        if (validLine)
                            filesStructure.Add(new FileStructure(name, type, headerLines, dividedByPlants, sumNeeded, repeatPeriodicalValues, periodicalColumnsWidth, headerColumns, headerColumnsWidth));
                        else
                            validFile = false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        log.Error(MessageUtil.FormatMessage("ERROR.MissingColumns", "FilesStructure.config", lineNumber));
                        validFile = false;
                    }
                }

                if (!validFile)
                    throw new Exception(MessageUtil.FormatMessage("ERROR.InvalidConfigurationFile", "FilesStructure.config"));

                fileReader.Close();
            }
            catch(ArgumentNullException)
            {
                throw new Exception(MessageUtil.FormatMessage("ERROR.ConfigurationFileNotFound", "FilesStructure.config"));
            }
            return filesStructure;
        }
    }
}
