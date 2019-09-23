using System;
using System.IO;
using System.Linq;
using log4net;
using log4net.Config;
using DHOG_WPF.Util;
using System.Collections.Generic;
using DHOGInputFilesReader.Models;
using DHOG_WPF.Models;
using static DHOG_WPF.DataTypes.Types;
using DHOG_WPF.Converters;

namespace DHOG_WPF.SDDPFilesReader
{
    class InputFilesReader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InputFilesReader));
        private string inputFilesPath;
        private List<FileStructure> filesStructure;
        private FilesReadingParameters filesReadingParameters;
        private int errors;
        private int wanings;

        public int Errors { get => errors; set => errors = value; }
        public int Wanings { get => wanings; set => wanings = value; }

        public InputFilesReader(FilesReadingParameters filesReadingParameters)
        {
            this.filesReadingParameters = filesReadingParameters;
            inputFilesPath = filesReadingParameters.InputFilesPath;
            GlobalContext.Properties["InputDataLogFileName"] = inputFilesPath + "CargaArchivosSDDP.log";
            XmlConfigurator.Configure();
            ReadParametersFromSDDP();
            filesStructure = FilesStructureReader.ReadFilesStructure();
            this.filesReadingParameters = filesReadingParameters;
            Errors = 0;
            Wanings = 0;
        }

        public bool FileExists(String name)
        {
            if (File.Exists(inputFilesPath + name))
                return true;
            else
            {
                log.Error(MessageUtil.FormatMessage("ERROR.InputFileNotFound", inputFilesPath + name, inputFilesPath));
                Errors++;
                return false;
            }
        }

        private void ReadParametersFromSDDP()
        {
            string fileName;
            if (filesReadingParameters.CreateScenario)
                fileName = filesReadingParameters.ScenarioFolderName + "1\\sddp.dat";
            else
                fileName = filesReadingParameters.InputFilesPath + "sddp.dat";

            if (File.Exists(fileName))
            {
                List<string> lines = File.ReadLines(fileName).Skip(4).Take(1).ToList();
                string value = lines[0].Substring(26, 4).Trim();
                if (Int32.TryParse(value, out int model))
                {
                    switch (model)
                    {
                        case 1:
                            filesReadingParameters.Model = Model.Middle;
                            filesReadingParameters.PeriodsQuantity = 52;
                            break;
                        case 2:
                            filesReadingParameters.Model = Model.Long;
                            filesReadingParameters.PeriodsQuantity = 12;
                            break;
                        default:
                            log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, 5, "ETAPA"));
                            break;
                    }
                }
                else
                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, 13, "MES/SEMANA INICIAL"));

                lines = File.ReadLines(fileName).Skip(12).Take(9).ToList();
                value = lines[0].Substring(26, 4).Trim();
                if (Int32.TryParse(value, out int initialPeriod))
                {
                    filesReadingParameters.InitialPeriod = initialPeriod;
                }
                else
                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, 13, "MES/SEMANA INICIAL"));

                value = lines[1].Substring(26, 4).Trim();
                if (Int32.TryParse(value, out int initialYear))
                {
                    filesReadingParameters.InitialYear = initialYear;
                }
                else
                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, 14, "ANO INICIAL"));

                value = lines[2].Substring(26, 4).Trim();
                if (Int32.TryParse(value, out int periodsToLoad))
                {
                    filesReadingParameters.PeriodsToLoad = periodsToLoad;
                }
                else
                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, 15, "NUMERO DE ETAPAS"));

                value = lines[8].Substring(26, 4).Trim();
                if (Int32.TryParse(value, out int blocks))
                {
                    filesReadingParameters.Blocks = blocks;
                }
                else
                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, 21, "NUMERO DE BLOQUES DEMANDA"));
            }
            else
                throw new Exception(MessageUtil.FormatMessage("ERROR.InputFileNotFound", fileName));
        }

        private static int ValidateNumericParameter(Dictionary<string, string> parametersDictionary, string parameterName, string fileName)
        {
            int parameterNumericValue;
            if (parametersDictionary.TryGetValue(parameterName, out string parameterValue))
            {
                if (Int32.TryParse(parameterValue, out parameterNumericValue))
                {
                    return parameterNumericValue;
                }
                else
                {
                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, parameterName, parameterName));
                    return -1;
                }
            }
            else
            {
                log.Error(MessageUtil.FormatMessage("ERROR.LoadParameterNotFound", parameterName));
                return -1;
            }
        }

        private static ValidatedBoolean ValidateBooleanParameter(Dictionary<string, string> parametersDictionary, string parameterName, string fileName)
        {
            ValidatedBoolean validatedBoolean;
            string parameterValue;
            bool parameterBooleanValue;
            if (parametersDictionary.TryGetValue(parameterName, out parameterValue))
            {
                if (bool.TryParse(parameterValue, out parameterBooleanValue))
                {
                    validatedBoolean = new ValidatedBoolean(parameterBooleanValue, true);
                }
                else
                {
                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidBoolean", fileName, parameterName, parameterName));
                    validatedBoolean = new ValidatedBoolean(false, false);
                }
            }
            else
            {
                log.Error(MessageUtil.FormatMessage("ERROR.LoadParameterNotFound", parameterName));
                validatedBoolean = new ValidatedBoolean(false, false);
            }
            return validatedBoolean;
        }

        public FileStructure GetFileStructure(string fileName)
        {
            foreach (FileStructure fileStructure in filesStructure)
                if (fileStructure.Name.Equals(fileName))
                    return fileStructure;

            log.Error(MessageUtil.FormatMessage("ERROR.FileStructureNotFound", fileName));
            Errors++;
            return null;
        }

        public double[,] ReadPeriodicLoad(string colombiafileName, string ecuadorFileName)
        {
            double[,] totalLoad = ReadPeriodicLoad(colombiafileName);
            if (totalLoad != null)
            {
                double[,] ecuadorLoad = ReadPeriodicLoad(ecuadorFileName);
                if (ecuadorLoad != null)
                    for (int i = 0; i < filesReadingParameters.PeriodsToLoad; i++)
                        for (int j = 0; j < filesReadingParameters.Blocks; j++)
                            totalLoad[i, j] += ecuadorLoad[i, j];
                else
                    totalLoad = null;
            }

            return totalLoad;
        }

        public List<Block> ReadLoadBlocks(string fileName)
        {
            List<Block> loadBlocks = new List<Block>();
            if (FileExists(fileName))
            {
                List<string> lines = File.ReadLines(inputFilesPath + fileName).Take(2).ToList();
                if (lines[0].Length == 8 + filesReadingParameters.Blocks * 6)
                {
                    int initialCharacter = 8;
                    for (int i = 0; i < filesReadingParameters.Blocks; i++)
                    {
                        int block;
                        string value = lines[0].Substring(initialCharacter, 6).Trim();
                        if (Int32.TryParse(value, out block))
                        {
                            value = lines[1].Substring(initialCharacter, 6).Trim();
                            double durationFactor;
                            if (Double.TryParse(value, out durationFactor))
                                loadBlocks.Add(new Block(block, durationFactor / 100, 1));
                            else
                            {
                                log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, 2, block));
                                Errors++;
                            }
                            initialCharacter += 6;
                        }
                        else
                        {
                            log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, 1, lines[0].Substring(initialCharacter, 6).Trim()));
                            Errors++;
                        }
                    }
                }
                else
                {
                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidBlocksQuantity", fileName, 1));
                    Errors++;
                }
            }

            double factorDurationSum = loadBlocks.Sum(d => d.DurationFactor);
            if (factorDurationSum != 1)
            {
                log.Error(MessageUtil.FormatMessage("ERROR.InvalidFactorDurationSum", fileName, 2, factorDurationSum));
                Errors++;
            }

            return loadBlocks;
        }

        public double[,] ReadPeriodicLoad(string fileName)
        {
            double[,] load = new double[filesReadingParameters.PeriodsToLoad, filesReadingParameters.Blocks];

            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    try
                    {
                        string[][] matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);

                        int lineNumber = fileStructure.StartLine + 1;
                        if (!Int32.TryParse(matrix[0][0], out int previousYear))
                            previousYear = 0;

                        foreach (string[] row in matrix)
                        {
                            lineNumber++;
                            if (Int32.TryParse(row[0], out int year))
                            {
                                if (year >= filesReadingParameters.InitialYear)
                                {
                                    if (!Int32.TryParse(row[1].Trim(), out int block))
                                    {
                                        log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "Blk"));
                                        Errors++;
                                    }

                                    int period = 0;
                                    int startColumn = fileStructure.HeaderColumns;
                                    if (year == filesReadingParameters.InitialYear)
                                    {
                                        period = 1;
                                        startColumn += filesReadingParameters.InitialPeriod - 1;
                                    }
                                    else
                                        period = filesReadingParameters.PeriodsQuantity - filesReadingParameters.InitialPeriod + 2 + (year - (filesReadingParameters.InitialYear + 1)) * filesReadingParameters.PeriodsQuantity;

                                    double loadToRepeat = -1;
                                    for (int i = startColumn; i < row.Length; i++)
                                    {
                                        if (period <= filesReadingParameters.PeriodsToLoad)
                                        {
                                            if (Double.TryParse(row[i].Trim(), out double value))
                                            {
                                                loadToRepeat = value;
                                                load[period - 1, block - 1] += value;
                                            }
                                            else
                                            {
                                                if (loadToRepeat != -1)
                                                    load[period - 1, block - 1] += loadToRepeat;
                                                else
                                                {
                                                    bool foundLoad = false;
                                                    for (int j = i - 1; j > 0; j--)
                                                    {
                                                        if (Double.TryParse(row[j].Trim(), out loadToRepeat))
                                                        {
                                                            load[period - 1, block - 1] += loadToRepeat;
                                                            foundLoad = true;
                                                            break;
                                                        }
                                                    }

                                                    if (!foundLoad)
                                                    {
                                                        loadToRepeat = -1;
                                                        log.Warn(MessageUtil.FormatMessage("WARNING.PeriodicLoadNotFound", fileName, lineNumber, i));
                                                        Wanings++;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            break;

                                        period++;
                                    }
                                }
                                previousYear = year;
                            }
                            else
                            {
                                log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "A~no"));
                                Errors++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }
                }
            }
            return load;
        }

        public List<River> CreateRivers(string fileName, string[] header, Dictionary<int, string> riversMapping)
        {
            List<River> rivers = new List<River>();

            for (int i = 1; i < header.Length; i++)
            {
                int riverNumber;
                if (Int32.TryParse(header[i].Trim(), out riverNumber))
                {
                    if (!filesReadingParameters.RiversToOmitContains(riverNumber))
                    {
                        string riverName;
                        if (riversMapping.TryGetValue(riverNumber, out riverName))
                            rivers.Add(new River(riverName, filesReadingParameters.PeriodsToLoad));
                        else
                        {
                            rivers.Add(null);
                            log.Warn(MessageUtil.FormatMessage("WARNING.RiverNumberNotFoundInDHOG", fileName, 1, riverNumber));
                            Wanings++;
                        }
                    }
                    else
                        rivers.Add(null);
                }
                else
                {
                    rivers.Add(null);
                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, 1, header[i]));
                    Errors++;
                }
            }

            foreach (string riverName in riversMapping.Select(x => x.Value))
            {
                bool riverFound = false;
                foreach (River river in rivers)
                {
                    if (river != null && river.Name.Equals(riverName))
                    {
                        riverFound = true;
                        break;
                    }
                }

                if (!riverFound)
                {
                    log.Warn(MessageUtil.FormatMessage("WARNING.RiverNameNotFoundInSDDP", fileName, riverName));
                    Wanings++;
                }
            }

            return rivers;
        }

        public List<River> ReadPeriodicRiversInflows(string fileName, Dictionary<int, string> riversMapping)
        {
            List<River> rivers = null;

            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    try
                    {
                        string[][] matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                        string[] header = matrix[0];
                        rivers = CreateRivers(fileName, matrix[0], riversMapping);

                        bool validFile = true;
                        int lineNumber = fileStructure.StartLine + 1;
                        int periodNumber = 0;
                        int initialYear = -1;
                        int endYear = -1;
                        int fileEndYear = -1;

                        if (filesReadingParameters.Model.Equals(Model.Long))
                        {
                            initialYear = filesReadingParameters.InflowsReferenceYear;
                            endYear = initialYear + (filesReadingParameters.PeriodsToLoad / 12);
                            string[] date = matrix[matrix.Length - 1][0].Split('/');
                            Int32.TryParse(date[1], out fileEndYear);
                        }
                        else if (filesReadingParameters.Model.Equals(Model.Middle))
                        {
                            initialYear = filesReadingParameters.InitialYear;
                            endYear = initialYear + (filesReadingParameters.PeriodsToLoad / 52);
                        }

                        foreach (string[] row in matrix)
                        {
                            if (lineNumber > 1)
                            {
                                string[] date = row[0].Split('/');
                                if (Int32.TryParse(date[1], out int year))
                                {
                                    if (Int32.TryParse(date[0], out int period))
                                    {
                                        if (year >= initialYear && year <= endYear)
                                        {
                                            if (year == initialYear)
                                                periodNumber = period - filesReadingParameters.InitialPeriod + 1;
                                            else
                                            {
                                                periodNumber = filesReadingParameters.PeriodsQuantity - filesReadingParameters.InitialPeriod + 1;
                                                periodNumber += filesReadingParameters.PeriodsQuantity * (year - initialYear - 1);
                                                periodNumber += period;
                                            }

                                            if (periodNumber >= 1 && periodNumber <= filesReadingParameters.PeriodsToLoad)
                                            {
                                                bool endOfData = false;
                                                if (filesReadingParameters.Model.Equals(Model.Long) && year == fileEndYear)
                                                {
                                                    if (Double.TryParse(row[1].Trim(), out double value))
                                                        if (value == -99)
                                                            endOfData = true;

                                                    if (endOfData)
                                                    {
                                                        for (int i = 2; i < row.Length; i++)
                                                        {
                                                            if (Double.TryParse(row[i].Trim(), out value))
                                                            {
                                                                if (value != -99)
                                                                {
                                                                    endOfData = false;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (!endOfData)
                                                {
                                                    for (int i = 1; i < row.Length; i++)
                                                    {
                                                        if (Double.TryParse(row[i].Trim(), out double value))
                                                        {
                                                            River river = rivers[i - 1];
                                                            if (river != null)
                                                            {
                                                                if (value != -99)
                                                                    river.PeriodicInflows[periodNumber - 1] = value;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            validFile = false;
                                                            log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, header[i]));
                                                            Errors++;
                                                        }
                                                    }
                                                }
                                                else
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, header[0]));
                                    Errors++;
                                    validFile = false;
                                }
                            }
                            lineNumber++;
                        }

                        /* When creating the inflows for the long term model, if there are pending periods after using the
                         * reference years, the remaining periods are filled with the same information, starting from the
                         * initial reference year.
                        */
                        if (validFile && filesReadingParameters.Model.Equals(Model.Long) && periodNumber < filesReadingParameters.PeriodsToLoad)
                        {
                            int initialPeriod = periodNumber;

                            foreach (River river in rivers)
                            {
                                if (river != null)
                                {
                                    if (river.PeriodicInflows != null)
                                    {
                                        periodNumber = initialPeriod;
                                        int period = 1;
                                        while (periodNumber <= filesReadingParameters.PeriodsToLoad)
                                        {
                                            river.PeriodicInflows[periodNumber - 1] = river.PeriodicInflows[period - 1];
                                            period++;
                                            periodNumber++;
                                        }
                                    }
                                }
                            }
                        }

                        for (int i = 1; i < header.Length; i++)
                        {
                            River river = rivers[i - 1];
                            if (river != null)
                            {
                                int missingPeriods = 0;
                                if (river.PeriodicInflows != null)
                                {
                                    for (int j = 0; j < filesReadingParameters.PeriodsToLoad; j++)
                                        if (river.PeriodicInflows[j] == -1)
                                        {
                                            river.PeriodicInflows[j] = 0;
                                            missingPeriods++;
                                        }
                                }

                                if (missingPeriods > 0)
                                {
                                    log.Warn(MessageUtil.FormatMessage("WARNING.NoInflowValuesForAllPeriods", fileName, river.Name, header[i].Trim(), missingPeriods));
                                    Wanings++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }
                }
            }
            return rivers;
        }

        public void ReadPeriodicPlantsMaintenances(string fileName, Dictionary<string, ConventionalPlant> plantsMapping)
        {
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        int plantPosition = matrix[0].Length - 1;
                        string previousPlantName = "";
                        List<double> initialYearMaintenances = new List<double>(); ;
                        for (int r = 0; r < matrix.Length; r++)
                        {
                            bool newPlant = false;
                            string[] row = matrix[r];
                            string plantNameSDDP = row[plantPosition].Trim();

                            if (plantNameSDDP.Equals(previousPlantName))
                                lineNumber++;
                            else
                            {
                                lineNumber += 3;
                                initialYearMaintenances.Clear();
                                newPlant = true;
                            }
                            previousPlantName = plantNameSDDP;

                            if (plantsMapping.TryGetValue(plantNameSDDP, out ConventionalPlant plant))
                            {
                                if (newPlant)
                                {
                                    plant.PeriodicMaintenances = new double[filesReadingParameters.PeriodsToLoad];

                                    for (int i = 0; i < filesReadingParameters.PeriodsToLoad; i++)
                                    {
                                        initialYearMaintenances.Add(plant.Max);
                                        plant.PeriodicMaintenances[i] = plant.Max;
                                    }
                                }

                                int period = 0;
                                if (Int32.TryParse(row[0], out int year))
                                {
                                    if (year >= filesReadingParameters.InitialYear)
                                    {
                                        int startColumn = fileStructure.HeaderColumns;
                                        if (year == filesReadingParameters.InitialYear)
                                        {
                                            period = 1;
                                            startColumn += filesReadingParameters.InitialPeriod - 1;

                                            if (filesReadingParameters.RepeatMaintenances)
                                            {
                                                for (int i = 1; i < startColumn; i++)
                                                {
                                                    double value;
                                                    if (Double.TryParse(row[i].Trim(), out value))
                                                        initialYearMaintenances[i - 1] = value;
                                                    else
                                                    {
                                                        log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, fileStructure.HeaderColumns + i));
                                                        Errors++;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            period = filesReadingParameters.PeriodsQuantity - filesReadingParameters.InitialPeriod + 2 + (year - (filesReadingParameters.InitialYear + 1)) * filesReadingParameters.PeriodsQuantity;


                                        for (int i = startColumn; i < row.Length - 1; i++)
                                        {
                                            if (period <= filesReadingParameters.PeriodsToLoad)
                                            {
                                                if (Double.TryParse(row[i].Trim(), out double value))
                                                    plant.PeriodicMaintenances[period - 1] = value;
                                                else
                                                {
                                                    plant.PeriodicMaintenances[period - 1] = plant.Max;
                                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, fileStructure.HeaderColumns + i));
                                                    Errors++;
                                                }
                                            }
                                            else
                                                break;

                                            period++;
                                        }
                                    }
                                }
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "A~no"));
                                    Errors++;
                                }


                                string nextPlantName = null;
                                if (r + 1 < matrix.Length)
                                    nextPlantName = matrix[r + 1][plantPosition].Trim();

                                if (!plantNameSDDP.Equals(nextPlantName))
                                {
                                    if (filesReadingParameters.RepeatMaintenances)
                                    {
                                        int initialPeriodNextCalendarYear = filesReadingParameters.PeriodsQuantity - filesReadingParameters.InitialPeriod + 1;
                                        int finalPeriodNextCalendarYear = initialPeriodNextCalendarYear + filesReadingParameters.InitialPeriod - 1;
                                        int initialYearPeriod = 0;
                                        for (int i = initialPeriodNextCalendarYear; i < finalPeriodNextCalendarYear; i++)
                                        {
                                            if (plant.PeriodicMaintenances[i] == 0)
                                                plant.PeriodicMaintenances[i] = initialYearMaintenances[initialYearPeriod];
                                            initialYearPeriod++;
                                        }

                                        for (int i = finalPeriodNextCalendarYear; i < filesReadingParameters.PeriodsToLoad; i++)
                                        {
                                            if (plant.PeriodicMaintenances[i] == 0)
                                                plant.PeriodicMaintenances[i] = plant.PeriodicMaintenances[i - filesReadingParameters.PeriodsQuantity];
                                        }
                                    }
                                }
                            }
                            else
                            {
                                log.Warn(MessageUtil.FormatMessage("WARNING.PlantNotFoundInDHOG", fileName, lineNumber, plantNameSDDP));
                                Wanings++;
                            }
                        }
                    }
                }
            }
        }

        public void ReadPeriodicReservoirsLevels(string fileName, LevelType levelType, Dictionary<string, Reservoir> reservoirsMapping)
        {
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        int reservoirPosition = matrix[0].Length - 1;
                        string previousReservoirName = "";
                        foreach (string[] row in matrix)
                        {
                            bool newReservoir = false;
                            string reservoirName = row[reservoirPosition].Trim();
                            if (reservoirName.Equals(previousReservoirName))
                                lineNumber++;
                            else
                            {
                                lineNumber += 3;
                                newReservoir = true;
                            }
                            previousReservoirName = reservoirName;

                            if (reservoirsMapping.TryGetValue(reservoirName, out Reservoir reservoir))
                            {
                                if (newReservoir && levelType.Equals(LevelType.Min))
                                    reservoir.PeriodicLevels = new PeriodicLevels[filesReadingParameters.PeriodsToLoad];

                                if (Int32.TryParse(row[0], out int year))
                                {
                                    if (year >= filesReadingParameters.InitialYear)
                                    {
                                        int period = 0;
                                        int startColumn = fileStructure.HeaderColumns;
                                        if (year == filesReadingParameters.InitialYear)
                                        {
                                            period = 1;
                                            startColumn += filesReadingParameters.InitialPeriod - 1;
                                        }
                                        else
                                            period = filesReadingParameters.PeriodsQuantity - filesReadingParameters.InitialPeriod + 2 + (year - (filesReadingParameters.InitialYear + 1)) * filesReadingParameters.PeriodsQuantity;

                                        for (int i = startColumn; i < row.Length - 1; i++)
                                        {
                                            if (period <= filesReadingParameters.PeriodsToLoad)
                                            {
                                                if (Double.TryParse(row[i].Trim(), out double level))
                                                {
                                                    if (reservoir.PeriodicLevels[period - 1] == null)
                                                    {
                                                        switch (levelType)
                                                        {
                                                            case LevelType.Min:
                                                                reservoir.PeriodicLevels[period - 1] = new PeriodicLevels(level, reservoir.MaxLevel);
                                                                break;
                                                            case LevelType.Max:
                                                                reservoir.PeriodicLevels[period - 1] = new PeriodicLevels(reservoir.MinLevel, level);
                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        switch (levelType)
                                                        {
                                                            case LevelType.Min:
                                                                reservoir.PeriodicLevels[period - 1].Min = level;
                                                                break;
                                                            case LevelType.Max:
                                                                reservoir.PeriodicLevels[period - 1].Max = level;
                                                                break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, fileStructure.HeaderColumns + i));
                                                    Errors++;
                                                }
                                            }
                                            else
                                                break;

                                            period++;
                                        }
                                    }
                                }
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "A~no"));
                                    Errors++;
                                }
                            }
                            else
                            {
                                log.Warn(MessageUtil.FormatMessage("WARNING.ReservoirNotFoundInDHOG", fileName, lineNumber, reservoirName));
                                Wanings++;
                            }
                        }
                    }
                }
            }
        }


        public void ReadReservoirsLevels(string fileName, List<Reservoir> reservoirs, Dictionary<string, Reservoir> reservoirsMapping)
        {
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        foreach (string[] row in matrix)
                        {
                            lineNumber++;

                            string reservoirName = row[1].Trim();

                            if (reservoirsMapping.TryGetValue(reservoirName, out Reservoir reservoir))
                            {
                                if (Double.TryParse(row[11].Trim(), out double minLevel))
                                    reservoir.MinLevel = minLevel;
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "VMin"));
                                    Errors++;
                                }

                                if (Double.TryParse(row[12].Trim(), out double maxLevel))
                                    reservoir.MaxLevel = maxLevel;
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "VMax"));
                                    Errors++;
                                }

                                if (Double.TryParse(row[13].Trim(), out double initialLevel))
                                {
                                    reservoir.InitialLevel = initialLevel;
                                    reservoir.FinalLevel = initialLevel;
                                }
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "VInic"));
                                    Errors++;
                                }
                            }
                            else
                            {
                                if (Double.TryParse(row[11].Trim(), out double minLevel))
                                {
                                    if (Double.TryParse(row[12].Trim(), out double maxLevel))
                                    {
                                        if (minLevel != 0 || maxLevel != 0)
                                        {
                                            log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "VInic"));
                                            Errors++;
                                        } 
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ReadHydroPlantsProductionFactorAndMax(string fileName, List<ConventionalPlant> plants, Dictionary<string, ConventionalPlant> plantsMapping)
        {
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        foreach (string[] row in matrix)
                        {
                            lineNumber++;

                            string hydroPlantName = row[1].Trim();

                            if (plantsMapping.TryGetValue(hydroPlantName, out ConventionalPlant hydroPlant))
                            {
                                if (Double.TryParse(row[7].Trim(), out double max))
                                    hydroPlant.Max = max;
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "Pot"));
                                    Errors++;
                                }

                                if (Double.TryParse(row[8].Trim(), out double productionFactor))
                                    hydroPlant.ProductionFactor = productionFactor;
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "FPMed"));
                                    Errors++;
                                }
                            }
                            else
                            {
                                if(Double.TryParse(row[7].Trim(), out double power))
                                {
                                    if(power != 0)
                                    {
                                        log.Warn(MessageUtil.FormatMessage("WARNING.PlantNotFoundInDHOG", fileName, lineNumber, hydroPlantName));
                                        Wanings++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ReadFuelsCosts(string fileName, List<Fuel> fuels)
        {
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        List<string> supplyCentersSDDP = new List<string>();
                        foreach (string[] row in matrix)
                        {
                            lineNumber++;

                            string supplyCenter = row[1].Trim();
                            supplyCentersSDDP.Add(supplyCenter);
                            Fuel fuel = fuels.FirstOrDefault(f => f.Name.Equals(supplyCenter));

                            if (fuel != null)
                            {
                                double cost;
                                if (Double.TryParse(row[3].Trim(), out cost))
                                    fuel.Cost = cost;
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "Costo"));
                                    Errors++;
                                }
                            }
                            else
                            {
                                log.Warn(MessageUtil.FormatMessage("WARNING.FuelNotFoundInDHOG", fileName, lineNumber, supplyCenter));
                                Wanings++;
                            }
                        }

                        foreach (Fuel fuel in fuels)
                        {
                            if (!supplyCentersSDDP.Contains(fuel.Name))
                            {
                                log.Warn(MessageUtil.FormatMessage("WARNING.FuelNotFoundInSDDP", fileName, fuel.Name));
                                Wanings++;
                            }
                        }

                    }
                }
            }
        }

        public void ReadPeriodicFuelsCosts(string fileName, List<Fuel> fuels)
        {
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        int supplyCenterPosition = matrix[0].Length - 1;
                        string previousSupplyCenter = "";
                        foreach (string[] row in matrix)
                        {
                            bool newFuel = false;
                            string supplyCenter = row[supplyCenterPosition].Trim();
                            if (supplyCenter.Equals(previousSupplyCenter))
                                lineNumber++;
                            else
                            {
                                newFuel = true;
                                lineNumber += 3;
                            }
                            previousSupplyCenter = supplyCenter;

                            Fuel fuel = fuels.FirstOrDefault(f => f.Name.Equals(supplyCenter));

                            if (fuel != null)
                            {
                                if (newFuel)
                                    fuel.PeriodicCostsAndCapacity = new PeriodicCostsAndCapacity[filesReadingParameters.PeriodsToLoad];

                                int year;
                                if (Int32.TryParse(row[0], out year))
                                {
                                    if (year >= filesReadingParameters.InitialYear)
                                    {
                                        int period = 0;
                                        int startColumn = fileStructure.HeaderColumns;
                                        if (year == filesReadingParameters.InitialYear)
                                        {
                                            period = 1;
                                            startColumn += filesReadingParameters.InitialPeriod - 1;
                                        }
                                        else
                                            period = filesReadingParameters.PeriodsQuantity - filesReadingParameters.InitialPeriod + 2 + (year - (filesReadingParameters.InitialYear + 1)) * filesReadingParameters.PeriodsQuantity;

                                        for (int i = startColumn; i < row.Length - 1; i++)
                                        {
                                            if (period <= filesReadingParameters.PeriodsToLoad)
                                            {
                                                if (Double.TryParse(row[i].Trim(), out double cost))
                                                    fuel.PeriodicCostsAndCapacity[period - 1] = new PeriodicCostsAndCapacity(cost, fuel.Capacity);
                                                else
                                                {
                                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, i));
                                                    Errors++;
                                                }
                                            }
                                            else
                                                break;

                                            period++;
                                        }
                                    }
                                }
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "A~no"));
                                    Errors++;
                                }
                            }
                            else
                            {
                                log.Warn(MessageUtil.FormatMessage("WARNING.FuelNotFoundInDHOG", fileName, lineNumber, supplyCenter));
                                Wanings++;
                            }
                        }

                    }
                }
            }
        }

        public void ReadPeriodicFuelsCapacity(string fileName, List<Fuel> fuels)
        {
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        int supplyCenterPosition = matrix[0].Length - 1;
                        string previousSupplyCenter = "";

                        foreach (string[] row in matrix)
                        {
                            bool newFuel = false;
                            string supplyCenter = row[supplyCenterPosition].Trim();
                            if (supplyCenter.Equals(previousSupplyCenter))
                                lineNumber++;
                            else
                            {
                                newFuel = true;
                                lineNumber += 3;
                            }

                            previousSupplyCenter = supplyCenter;

                            Fuel fuel = fuels.FirstOrDefault(f => f.Name.Equals(supplyCenter));

                            if (fuel != null)
                            {
                                if (newFuel && fuel.PeriodicCostsAndCapacity == null)
                                    fuel.PeriodicCostsAndCapacity = new PeriodicCostsAndCapacity[filesReadingParameters.PeriodsToLoad];

                                int year;
                                if (Int32.TryParse(row[0], out year))
                                {
                                    if (year >= filesReadingParameters.InitialYear)
                                    {
                                        int period = 0;
                                        int startColumn = fileStructure.HeaderColumns;
                                        if (year == filesReadingParameters.InitialYear)
                                        {
                                            period = 1;
                                            startColumn += filesReadingParameters.InitialPeriod - 1;
                                        }
                                        else
                                            period = filesReadingParameters.PeriodsQuantity - filesReadingParameters.InitialPeriod + 2 + (year - (filesReadingParameters.InitialYear + 1)) * filesReadingParameters.PeriodsQuantity;

                                        for (int i = startColumn; i < row.Length - 1; i++)
                                        {
                                            if (period <= filesReadingParameters.PeriodsToLoad)
                                            {
                                                double capacity;
                                                if (Double.TryParse(row[i].Trim(), out capacity))
                                                {
                                                    if (fuel.PeriodicCostsAndCapacity[period - 1] != null)
                                                        fuel.PeriodicCostsAndCapacity[period - 1].Capacity = capacity;
                                                    else
                                                        fuel.PeriodicCostsAndCapacity[period - 1] = new PeriodicCostsAndCapacity(fuel.Cost, capacity);
                                                }
                                                else
                                                {
                                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, i));
                                                    Errors++;
                                                }
                                            }
                                            else
                                                break;

                                            period++;
                                        }
                                    }
                                }
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "A~no"));
                                    Errors++;
                                }
                            }
                            else
                            {
                                log.Warn(MessageUtil.FormatMessage("WARNING.FuelNotFoundInDHOG", fileName, lineNumber, supplyCenter));
                                Wanings++;
                            }
                        }
                    }
                }
            }
        }


        public void ReadFuelContractsCostsAndCapacity(string fileName, List<FuelContract> fuelContracts)
        {
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        List<string> contractNamesSDDP = new List<string>();
                        foreach (string[] row in matrix)
                        {
                            lineNumber++;

                            string name = row[1].Trim();
                            contractNamesSDDP.Add(name);
                            FuelContract fuelContract = fuelContracts.FirstOrDefault(fc => fc.Name.Equals(name));

                            if (fuelContract != null)
                            {
                                if (Double.TryParse(row[3].Trim(), out double cost))
                                    fuelContract.Cost = cost;
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "Costo"));
                                    Errors++;
                                }

                                if (Double.TryParse(row[11].Trim(), out double capacity))
                                {
                                    if (capacity != -1)
                                        fuelContract.Capacity = capacity;
                                    else
                                        fuelContract.Capacity = 0;
                                }
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "disp.max"));
                                    Errors++;
                                }
                            }
                            else
                            {
                                log.Warn(MessageUtil.FormatMessage("WARNING.FuelContractNotFoundInDHOG", fileName, lineNumber, name));
                                Wanings++;
                            }
                        }

                        foreach (FuelContract fuelContract in fuelContracts)
                        {
                            if (!contractNamesSDDP.Contains(fuelContract.Name))
                            {
                                log.Warn(MessageUtil.FormatMessage("WARNING.FuelContractNotFoundInSDDP", fileName, fuelContract.Name));
                                Wanings++;
                            }
                        }
                    }
                }
            }
        }

        public void ReadPeriodicFuelContractsCosts(string fileName, List<FuelContract> fuelContracts)
        {
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        int contractNamePosition = matrix[0].Length - 1;
                        string previousContractName = "";
                        foreach (string[] row in matrix)
                        {
                            bool newContract = false;
                            string contractName = row[contractNamePosition].Trim();
                            if (contractName.Equals(previousContractName))
                                lineNumber++;
                            else
                            {
                                newContract = true;
                                lineNumber += 3;
                            }
                            previousContractName = contractName;

                            FuelContract fuelContract = fuelContracts.FirstOrDefault(fc => fc.Name.Equals(contractName));
                            if (fuelContract != null)
                            {
                                if (newContract)
                                    fuelContract.PeriodicCostsAndCapacity = new PeriodicCostsAndCapacity[filesReadingParameters.PeriodsToLoad];

                                int year;
                                if (Int32.TryParse(row[0], out year))
                                {
                                    if (year >= filesReadingParameters.InitialYear)
                                    {
                                        int period = 0;
                                        int startColumn = fileStructure.HeaderColumns;
                                        if (year == filesReadingParameters.InitialYear)
                                        {
                                            period = 1;
                                            startColumn += filesReadingParameters.InitialPeriod - 1;
                                        }
                                        else
                                            period = filesReadingParameters.PeriodsQuantity - filesReadingParameters.InitialPeriod + 2 + (year - (filesReadingParameters.InitialYear + 1)) * filesReadingParameters.PeriodsQuantity;

                                        for (int i = startColumn; i < row.Length - 1; i++)
                                        {
                                            if (period <= filesReadingParameters.PeriodsToLoad)
                                            {
                                                double cost;
                                                if (Double.TryParse(row[i].Trim(), out cost))
                                                    fuelContract.PeriodicCostsAndCapacity[period - 1] = new PeriodicCostsAndCapacity(cost, fuelContract.Capacity);
                                                else
                                                {
                                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, i));
                                                    Errors++;
                                                }
                                            }
                                            else
                                                break;

                                            period++;
                                        }
                                    }
                                }
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "A~no"));
                                    Errors++;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ReadPeriodicFuelContractsCapacity(string fileName, List<FuelContract> fuelContracts)
        {
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        int contractNamePosition = matrix[0].Length - 1;
                        string previousContractName = "";
                        foreach (string[] row in matrix)
                        {
                            bool newContract = false;
                            string contractName = row[contractNamePosition].Trim();
                            if (contractName.Equals(previousContractName))
                                lineNumber++;
                            else
                            {
                                newContract = true;
                                lineNumber += 3;
                            }
                            previousContractName = contractName;

                            FuelContract fuelContract = fuelContracts.FirstOrDefault(fc => fc.Name.Equals(contractName));

                            if (fuelContract != null)
                            {
                                if (newContract && fuelContract.PeriodicCostsAndCapacity == null)
                                    fuelContract.PeriodicCostsAndCapacity = new PeriodicCostsAndCapacity[filesReadingParameters.PeriodsToLoad];

                                int year;
                                if (Int32.TryParse(row[0], out year))
                                {
                                    int block;
                                    if (Int32.TryParse(row[1], out block))
                                    {
                                        if (year >= filesReadingParameters.InitialYear && block == 1)
                                        {
                                            int period = 0;
                                            int startColumn = fileStructure.HeaderColumns;
                                            if (year == filesReadingParameters.InitialYear)
                                            {
                                                period = 1;
                                                startColumn += filesReadingParameters.InitialPeriod - 1;
                                            }
                                            else
                                                period = filesReadingParameters.PeriodsQuantity - filesReadingParameters.InitialPeriod + 2 + (year - (filesReadingParameters.InitialYear + 1)) * filesReadingParameters.PeriodsQuantity;

                                            for (int i = startColumn; i < row.Length - 1; i++)
                                            {
                                                if (period <= filesReadingParameters.PeriodsToLoad)
                                                {
                                                    double capacity;
                                                    if (Double.TryParse(row[i].Trim(), out capacity))
                                                    {
                                                        if (fuelContract.PeriodicCostsAndCapacity[period - 1] != null)
                                                            fuelContract.PeriodicCostsAndCapacity[period - 1].Capacity = capacity;
                                                        else
                                                            fuelContract.PeriodicCostsAndCapacity[period - 1] = new PeriodicCostsAndCapacity(fuelContract.Cost, capacity);
                                                    }
                                                    else
                                                    {
                                                        log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, i));
                                                        Errors++;
                                                    }
                                                }
                                                else
                                                    break;

                                                period++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "Blk"));
                                        Errors++;
                                    }
                                }
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "A~no"));
                                    Errors++;
                                }
                            }
                        }
                    }
                }
            }
        }

        public Period[] ReadBasicPeriod(double[,] load, double durationFactor)
        {
            Period[] basicPeriod = new Period[filesReadingParameters.PeriodsToLoad];
            int[] durationHours = new int[filesReadingParameters.PeriodsToLoad];
            DateTime[] dates = new DateTime[filesReadingParameters.PeriodsToLoad];

            double rationingCost = ReadRationingCost();
            double[] CAR = ReadCAR();

            DateTime initialDate;
            if (filesReadingParameters.Model.Equals(Model.Long))
            {
                string initialPeriod = filesReadingParameters.InitialPeriod.ToString();
                if (initialPeriod.Length == 1)
                    initialPeriod = "0" + initialPeriod;

                string initialDateString = "01/" + initialPeriod + "/" + filesReadingParameters.InitialYear;
                initialDate = DateTime.ParseExact(initialDateString, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                dates[0] = initialDate;
                durationHours[0] = 24 * DateTime.DaysInMonth(filesReadingParameters.InitialYear, filesReadingParameters.InitialPeriod);

                for (int i = 1; i < filesReadingParameters.PeriodsToLoad; i++)
                {
                    dates[i] = dates[i - 1].AddMonths(1);
                    durationHours[i] = 24 * DateTime.DaysInMonth(dates[i].Year, dates[i].Month);
                }
            }
            else if (filesReadingParameters.Model.Equals(Model.Middle))
            {
                initialDate = DateConverter.FirstDateOfWeekISO8601(filesReadingParameters.InitialYear, filesReadingParameters.InitialPeriod);

                dates[0] = initialDate;
                durationHours[0] = 168;

                for (int i = 1; i < filesReadingParameters.PeriodsToLoad; i++)
                {
                    dates[i] = dates[i - 1].AddDays(7);
                    durationHours[i] = 168;
                }
            }
            for (int i = 0; i < filesReadingParameters.PeriodsToLoad; i++)
            {
                double loadPeriod = load[i, 0] / (durationHours[i] * durationFactor) * 1000;
                basicPeriod[i] = new Period(dates[i], i + 1, loadPeriod, durationHours[i], rationingCost, CAR[i]);
            }

            return basicPeriod;
        }

        public double ReadRationingCost()
        {
            double maxRationingCost = 0;
            string fileName = filesReadingParameters.InputFilesPath + "sddp.dat";
            if (File.Exists(fileName))
            {
                string line = File.ReadLines(fileName).Skip(34).Take(1).First();

                for (int i = 7; i < line.Length; i = i + 16)
                {
                    string value = line.Substring(i, 9).Trim();
                    if (Double.TryParse(value, out double rationingCost))
                    {
                        if (rationingCost > maxRationingCost)
                            maxRationingCost = rationingCost;
                    }
                    else
                        log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, 35, "Costo Deficit"));
                }
            }
            return maxRationingCost;
        }

        public double[] ReadCAR()
        {
            double[] CAR = new double[filesReadingParameters.PeriodsToLoad];
            string fileName;
            if (filesReadingParameters.Model.Equals(Model.Middle))
                fileName = "cariseco.dat";
            else
                fileName = "carimeco.dat";
            
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        foreach (string[] row in matrix)
                        {
                            lineNumber++;
                            if (Int32.TryParse(row[0], out int year))
                            {
                                if (year >= filesReadingParameters.InitialYear)
                                {
                                    int period = 0;
                                    int startColumn = fileStructure.HeaderColumns;
                                    if (year == filesReadingParameters.InitialYear)
                                    {
                                        period = 1;
                                        startColumn += filesReadingParameters.InitialPeriod - 1;
                                    }
                                    else
                                        period = filesReadingParameters.PeriodsQuantity - filesReadingParameters.InitialPeriod + 2 + (year - (filesReadingParameters.InitialYear + 1)) * filesReadingParameters.PeriodsQuantity;

                                    for (int i = startColumn; i < row.Length; i++)
                                    {
                                        if (period <= filesReadingParameters.PeriodsToLoad)
                                        {
                                            if (Double.TryParse(row[i].Trim(), out double CARPeriod))
                                            {
                                                CAR[period - 1] = CARPeriod;
                                            }
                                            else
                                            {
                                                log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, i));
                                                Errors++;
                                            }
                                        }
                                        else
                                            break;

                                        period++;
                                    }
                                }
                            }
                            else
                            {
                                log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "A~no"));
                                Errors++;
                            }
                        }
                    }
                }
            }
            return CAR;
        }

        public void ReadThermalPlantsParameters(string fileName, List<ConventionalPlant> plants, Dictionary<string, ConventionalPlant> plantsMapping)
        {
            if (FileExists(fileName))
            {
                FileStructure fileStructure = this.GetFileStructure(fileName);
                if (fileStructure != null)
                {
                    string[][] matrix = null;
                    try
                    {
                        matrix = TextFilesReader.ConvertFileToMatrix(fileStructure, inputFilesPath + fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                        Errors++;
                    }

                    if (matrix != null)
                    {
                        int lineNumber = fileStructure.StartLine;
                        foreach (string[] row in matrix)
                        {
                            lineNumber++;

                            string thermalPlantName = row[1].Trim();

                            if (plantsMapping.TryGetValue(thermalPlantName, out ConventionalPlant thermalPlant))
                            {
                                if (Double.TryParse(row[5].Trim(), out double min))
                                    thermalPlant.Min = min;
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "GerMin"));
                                    Errors++;
                                }

                                if (Double.TryParse(row[6].Trim(), out double max))
                                    thermalPlant.Max = max;
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "GerMax"));
                                    Errors++;
                                }

                                if (Double.TryParse(row[8].Trim(), out double ih))
                                    thermalPlant.AvailabilityFactor = 1 - (ih / 100);
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "Ih"));
                                    Errors++;
                                }

                                if (Double.TryParse(row[9].Trim(), out double variableCost))
                                    thermalPlant.VariableCost = variableCost;
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "CVaria"));
                                    Errors++;
                                }

                                if (Double.TryParse(row[13].Trim(), out double productionFactor))
                                    thermalPlant.ProductionFactor = productionFactor;
                                else
                                {
                                    log.Error(MessageUtil.FormatMessage("ERROR.InvalidNumber", fileName, lineNumber, "CEsp.1"));
                                    Errors++;
                                }
                            }
                            else
                            {
                                log.Warn(MessageUtil.FormatMessage("WARNING.PlantNotFoundInDHOG", fileName, lineNumber, thermalPlantName));
                                Wanings++;
                            }
                        }
                    }
                }
            }
        }
    }
}
