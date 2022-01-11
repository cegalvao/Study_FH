using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics; //Get time execution
using System.Linq;
using System.Text;
using Gurobi;
using System.Diagnostics.PerformanceData;
using System.Windows.Forms;

namespace Study_FH
{
    class FMU : IEquatable<FMU>
    {// Forest Management Units
        public string Name;
        public int IndexAllFMU;
        public double InitialAge;
        public double Area;
        public double CoordX;
        public double CoordY;
        public List<int> FMUsProxInBlock;
        public List<int> FMUsProxOutBlockInPeriod;
        public List<int> FMUsProxConsecutivePeriod;
        public FMU(string name, double initialAge, double area)
        {
            Name = name;
            InitialAge = initialAge;
            Area = area;
            FMUsProxInBlock = new List<int>();
            FMUsProxOutBlockInPeriod = new List<int>();
            FMUsProxConsecutivePeriod = new List<int>();
        }
        public FMU(string name, double initialAge, double area, double X, double Y)
        {
            Name = name;
            InitialAge = initialAge;
            Area = area;
            CoordX = X;
            CoordY = Y;
            FMUsProxInBlock = new List<int>();
            FMUsProxOutBlockInPeriod = new List<int>();
            FMUsProxConsecutivePeriod = new List<int>();
        }
        public void addFMUProxInBlock(int otherindex)
        {
            if (!(FMUsProxInBlock.Contains(otherindex)))
            {
                FMUsProxInBlock.Add(otherindex);
            }
        }
        public void RemoveFMUProxInBlock(int otherindex)
        {
            if (FMUsProxInBlock.Contains(otherindex))
            {
                FMUsProxInBlock.Remove(otherindex);
            }
        }
        public void addFMUProxOutBlockInPeriod(int otherindex)
        {
            if (!(FMUsProxOutBlockInPeriod.Contains(otherindex)))
            {
                FMUsProxOutBlockInPeriod.Add(otherindex);
            }
        }
        public void RemoveFMUProxOutBlockInPeriod(int otherindex)
        {
            if (FMUsProxOutBlockInPeriod.Contains(otherindex))
            {
                FMUsProxOutBlockInPeriod.Remove(otherindex);
            }
        }
        public void addFMUProxConsecutivePeriod(int otherindex)
        {
            if (!(FMUsProxConsecutivePeriod.Contains(otherindex)))
            {
                FMUsProxConsecutivePeriod.Add(otherindex);
            }
        }
        public void removeFMUProxConsecutivePeriod(int otherindex)
        {
            if (FMUsProxConsecutivePeriod.Contains(otherindex))
            {
                FMUsProxConsecutivePeriod.Remove(otherindex);
            }
        }
        //public int IndexBlock;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FMU Name: \t" + Name);
            sb.AppendLine("Index: \t" + IndexAllFMU.ToString());
            sb.AppendLine("Initial Age: \t" + InitialAge.ToString());
            sb.AppendLine("Area: \t" + Area.ToString());
            sb.AppendLine("X: \t" + CoordX.ToString());
            sb.AppendLine("Y: \t" + CoordY.ToString());
            return sb.ToString();
        }
        public string ToTSVString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + "\t");
            sb.Append(IndexAllFMU.ToString() + "\t");
            sb.Append(InitialAge.ToString() + "\t");
            sb.Append(Area.ToString() + "\t");
            sb.Append(CoordX.ToString() + "\t");
            sb.AppendLine(CoordY.ToString());
            return sb.ToString();
        }
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine('{'.ToString());
            sb.AppendLine(ToolBox.InQuotes("Name") + ':' + Name + ',');
            sb.AppendLine(ToolBox.InQuotes("IndexAllFMU") + ':' + IndexAllFMU.ToString() + ',');
            sb.AppendLine(ToolBox.InQuotes("InitialAge") + ':' + InitialAge.ToString() + ',');
            sb.AppendLine(ToolBox.InQuotes("Area") + ':' + Area.ToString() + ',');
            sb.AppendLine(ToolBox.InQuotes("CoordX") + ':' + CoordX.ToString() + ',');
            sb.AppendLine(ToolBox.InQuotes("CoordY") + ':' + CoordY.ToString() + ',');
            sb.Append('}');
            return sb.ToString();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as FMU);
        }
        public bool Equals(FMU other)
        {
            return other != null &&
                   Name == other.Name &&
                   IndexAllFMU == other.IndexAllFMU &&
                   InitialAge == other.InitialAge &&
                   Area == other.Area &&
                   CoordX == other.CoordX &&
                   CoordY == other.CoordY;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, IndexAllFMU, InitialAge, Area, CoordX, CoordY);
        }
        public static bool operator ==(FMU left, FMU right)
        {
            return EqualityComparer<FMU>.Default.Equals(left, right);
        }
        public static bool operator !=(FMU left, FMU right)
        {
            return !(left == right);
        }
    }
    class Block : IEquatable<Block>
    {
        public string Name;
        public int IndexAllBlock;
        public int Period;
        public List<FMU> FMUsInBlock;

        public Block(string name, int indexAllBlock)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IndexAllBlock = indexAllBlock;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FMU Name: \t" + Name);
            sb.AppendLine("Index: \t" + IndexAllBlock.ToString());
            sb.AppendLine("Period: \t" + Period.ToString());
            sb.AppendLine("FMUsInBlock Length: \t" + FMUsInBlock.Count.ToString());
            sb.Append("FMUsInBlock: \t");
            List<int> FIndex = new List<int>();
            foreach (FMU Unit in FMUsInBlock)
            {
                FIndex.Add(Unit.IndexAllFMU);
            }
            sb.Append(ToolBox.GroupingListIntToString(FIndex));
            return sb.ToString();
        }
        public string ToTSVString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + "\t");
            sb.Append(IndexAllBlock.ToString() + "\t");
            sb.Append(Period.ToString() + "\t");
            sb.Append(FMUsInBlock.Count.ToString() + "\t");
            List<int> FIndex = new List<int>();
            foreach (FMU Unit in FMUsInBlock)
            {
                FIndex.Add(Unit.IndexAllFMU);
            }
            sb.Append(ToolBox.GroupingListIntToString(FIndex));
            return sb.ToString();
        }
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine('{'.ToString());
            sb.AppendLine(ToolBox.InQuotes("Name") + ':' + Name + ',');
            sb.AppendLine(ToolBox.InQuotes("IndexAllBlock") + ':' + IndexAllBlock.ToString() + ',');
            sb.AppendLine(ToolBox.InQuotes("Period") + ':' + Period.ToString() + ',');
            sb.AppendLine(ToolBox.InQuotes("FMUsInBlock") + ": [");
            foreach (FMU Unit in FMUsInBlock)
            {
                sb.AppendLine(Unit.ToJson() + ',');
            }
            sb.AppendLine("],");
            sb.Append('}'.ToString());
            return sb.ToString();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Block);
        }
        public bool Equals(Block other)
        {
            return other != null &&
                   Name == other.Name &&
                   IndexAllBlock == other.IndexAllBlock &&
                   Period == other.Period &&
                   EqualityComparer<List<FMU>>.Default.Equals(FMUsInBlock, other.FMUsInBlock);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, IndexAllBlock, Period, FMUsInBlock);
        }

        public static bool operator ==(Block left, Block right)
        {
            return EqualityComparer<Block>.Default.Equals(left, right);
        }
        public static bool operator !=(Block left, Block right)
        {
            return !(left == right);
        }
    }
    class Period : IEquatable<Period>
    {
        public string Name;
        public int IndexPeriod;
        public List<Block> BlocksInPeriod;

        public Period(string name, int indexPeriod)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IndexPeriod = indexPeriod;
            BlocksInPeriod = new List<Block>();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Period: \t" + Name);
            sb.AppendLine("Index: \t" + IndexPeriod.ToString());
            sb.AppendLine("BlocksInPeriod Length: \t" + BlocksInPeriod.Count.ToString());
            if (BlocksInPeriod.Count() > 0)
            {
                sb.Append("BlocksInPeriod: \t");
                List<int> BIndex = new List<int>();
                foreach (Block B in BlocksInPeriod)
                {
                    BIndex.Add(B.IndexAllBlock);
                }
                sb.AppendLine(ToolBox.GroupingListIntToString(BIndex));
            }
            return sb.ToString();
        }
        public string ToTSVString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + "\t");
            sb.Append(IndexPeriod.ToString() + "\t");
            sb.Append(BlocksInPeriod.Count.ToString() + "\t");
            if (BlocksInPeriod.Count() > 0)
            {
                sb.Append("BlocksInPeriod: \t");
                List<int> BIndex = new List<int>();
                foreach (Block B in BlocksInPeriod)
                {
                    BIndex.Add(B.IndexAllBlock);
                }
                sb.Append(ToolBox.GroupingListIntToString(BIndex));
            }
            return sb.ToString();
        }
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine('{'.ToString());
            sb.AppendLine(ToolBox.InQuotes("Name") + ':' + Name + ',');
            sb.AppendLine(ToolBox.InQuotes("IndexPeriod") + ':' + IndexPeriod.ToString() + ',');
            if (BlocksInPeriod.Count() > 0)
            {
                sb.AppendLine(ToolBox.InQuotes("BlocksInPeriod") + ": [");
                foreach (Block B in BlocksInPeriod)
                {
                    sb.AppendLine(B.ToJson() + ',');
                }
                sb.AppendLine("],");
            }
            sb.Append('}');
            return sb.ToString();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Period);
        }
        public bool Equals(Period other)
        {
            return other != null &&
                   Name == other.Name &&
                   IndexPeriod == other.IndexPeriod &&
                   EqualityComparer<List<Block>>.Default.Equals(BlocksInPeriod, other.BlocksInPeriod);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, IndexPeriod, BlocksInPeriod);
        }

        public static bool operator ==(Period left, Period right)
        {
            return EqualityComparer<Period>.Default.Equals(left, right);
        }
        public static bool operator !=(Period left, Period right)
        {
            return !(left == right);
        }
    }
    class FHInstance : IDisposable
    {
        private bool _disposed = false;
        public string Name;// { get; set; }
        public string FileName;
        public int NumFMUs;// { get; set; } 
        public int NumPeriodsHP;
        public int NumSortiments;
        public Dictionary<int, int> NumBlocksByPeriod;
        public int MaxNumBlocks;
        public double MinimalHarvestArea;
        public double MaximalHarvestArea;
        public double MinimalHarvestAge;
        public double VolumeVariation;
        public double MaxDistInBlock;
        public double MaxDistOutBlockInPeriod;
        public double MaxDistConsecutivePeriod;
        public double NewBlockPercent;
        public double[,] FMUDistances;
        public double[,] LiquidPresentValues;
        public double[,,] SortimentVolumes;
        public double[,] SortimentDemands;
        public List<FMU> AllFMUs;
        public List<FMU> FMUsForFirstPeriodHarvest;
        public List<Tuple<int, int>> AdjacentFMUs;
        public List<Period> AllPeriods;
        public List<Block> AllBlocks;
        public StringBuilder Param;

        public FHInstance(string name, string path, string anot, bool TresCSV, decimal[] percents)
        {
            Name = name;
            FileName = name.Replace(' ', '_') + anot.Replace(' ', '_');
            Param = new StringBuilder();
            if (TresCSV)
            {
                ReadInstanceTresCSV(path, percents);
            }
            else
            {
                ReadInstance(path, percents);
            }
        }

        private void ReadInstanceTresCSV(string path, decimal[] percents)
        {
            string DatFile = path + "Dados_" + Name + ".csv";
            string FldFile = path + "Floyd_" + Name + ".csv";
            string MatFile = path + "MatrizAdj_" + Name + ".csv";
            string[] infos = Name.Split('_');

            AllFMUs = new List<FMU>();
            FMUsForFirstPeriodHarvest = new List<FMU>();
            AdjacentFMUs = new List<Tuple<int, int>>();
            AllPeriods = new List<Period>();
            AllBlocks = new List<Block>();

            //First File
            string[] AllDatFile = File.ReadAllLines(DatFile);
            for (int cline = 0; cline < AllDatFile.Length; cline++)
            {
                string[] Line = AllDatFile[cline].Split(';');
                switch (cline) {
                    case 0:
                        NumFMUs = int.Parse(Line[0]);
                        break;
                    case 1:
                        NumPeriodsHP = int.Parse(Line[0]);
                        LiquidPresentValues = new double[NumFMUs, NumPeriodsHP];
                        for (int p = 0; p < NumPeriodsHP; p++)
                        {
                            Period NewP = new Period("period" + p.ToString(), p);
                            if (!(AllPeriods.Contains(NewP)))
                            {
                                AllPeriods.Add(NewP);
                            }
                        }
                        break;
                    case 2:
                        NumSortiments = int.Parse(Line[0]);
                        SortimentVolumes = new double[NumSortiments, NumFMUs, NumPeriodsHP];
                        break;
                    case 3:
                        MaxNumBlocks = int.Parse(Line[0]);
                        break;
                    case 4:
                        break;
                    default:
                        FMU NewUnit = new FMU(Name + Line[0],
                                              ToolBox.ReadDouble(Line[3]),
                                              ToolBox.ReadDouble(Line[4]),
                                              ToolBox.ReadDouble(Line[1]),
                                              ToolBox.ReadDouble(Line[2]));

                        if (!(AllFMUs.Contains(NewUnit)) && GetFMUIndexFromName(NewUnit.Name) < 0)
                        {
                            AllFMUs.Add(NewUnit);
                            NewUnit.IndexAllFMU = AllFMUs.Count - 1;
                        }

                        for (int period = 0; period < NumPeriodsHP; period++)
                        {
                            for (int prod = 0; prod < NumSortiments + 1; prod++)
                            {
                                if (prod < NumSortiments)
                                {
                                    int indexLine = prod * NumPeriodsHP + period + 5;
                                    SortimentVolumes[prod, NewUnit.IndexAllFMU, period] = ToolBox.ReadDouble(Line[indexLine]);
                                }
                                else
                                {
                                    int indexVPL = NumPeriodsHP * NumSortiments + period + 5;
                                    LiquidPresentValues[NewUnit.IndexAllFMU, period] = ToolBox.ReadDouble(Line[indexVPL]);
                                }
                            }
                        }
                        break;
                }
            }

            //Second File
            string[] AllFldFile = File.ReadAllLines(FldFile);
            //Read distances
            double TotalDistance = 0;
            double DeltaDistance;
            double MinDistance = double.MaxValue;
            double MaxDistance = 0;
            FMUDistances = new double[NumFMUs, NumFMUs];

            for (int i = 0; i < AllFldFile.Length; i++)
            {
                string[] dists = AllFldFile[i].Split(';');
                for (int j = 0; j < dists.Length - 1; j++)
                {
                    FMUDistances[i, j] = ToolBox.ReadDouble(dists[j]);
                    if (FMUDistances[i, j] < MinDistance)
                    {
                        MinDistance = FMUDistances[i, j];
                    }
                    if (FMUDistances[i, j] > MaxDistance)
                    {
                        MaxDistance = FMUDistances[i, j];
                    }
                    TotalDistance += FMUDistances[i, j];
                }
            }
            DeltaDistance = MaxDistance - MinDistance;

            //Third File
            string[] AllMatFile = File.ReadAllLines(MatFile);
            for (int i = 0; i < AllMatFile.Length; i++)
            {
                string[] Lines = AllMatFile[i].Split(';');
                for (int j = 0; j < Lines.Length; j++)
                {
                    if (ToolBox.ReadDouble(Lines[j]) > 0)
                    {
                        Tuple<int, int> inds = new Tuple<int, int>(i, j);
                        AdjacentFMUs.Add(inds);
                    }
                }
            }

            MinimalHarvestArea = Convert.ToInt32(infos[4].Replace("aMin", ""));
            MaximalHarvestArea = Convert.ToInt32(infos[3].Replace("aMax", ""));

            double TotalArea = 0;
            Random Aleatorio = new Random(Convert.ToInt32(infos[6].Replace("s", "")));
            int FMUIndexFirst = Convert.ToInt32(Aleatorio.NextDouble() * AllFMUs.Count);
            FMUsForFirstPeriodHarvest.Add(AllFMUs[FMUIndexFirst]);

            foreach (FMU Unit in AllFMUs)
            {
                TotalArea += Unit.Area;
            }


            decimal MedTotalArea = Convert.ToDecimal(TotalArea / (MaxNumBlocks * NumPeriodsHP)) * percents[10];
            SortimentDemands = new double[NumSortiments, NumPeriodsHP];
            double DemandFactor = Convert.ToDouble(percents[2] / 100);

            for (int s = 0; s < NumSortiments; s++)
            {
                foreach (Period P in AllPeriods)
                {
                    int p = P.IndexPeriod;
                    double demand = 0;
                    foreach (FMU Unit in AllFMUs)
                    {
                        int i = Unit.IndexAllFMU;
                        demand += SortimentVolumes[s, i, p];
                    }
                    SortimentDemands[s, p] = DemandFactor * demand;
                }
            }

            VolumeVariation = Convert.ToDouble(percents[3] / 100);
            NewBlockPercent = Convert.ToDouble(percents[4] / 100);
            //double MedDistance = TotalDistance / (NumFMUs * (NumFMUs - 1));
            MaxDistInBlock = Convert.ToDouble(percents[5] / 100) * DeltaDistance + MinDistance;
            MaxDistOutBlockInPeriod = Convert.ToDouble(percents[6] / 100) * DeltaDistance + MinDistance;
            MaxDistConsecutivePeriod = Convert.ToDouble(percents[7] / 100) * DeltaDistance + MinDistance;

            //Lists of distance
            foreach (FMU Unit1 in AllFMUs)
            {
                int i = Unit1.IndexAllFMU;
                foreach (FMU Unit2 in AllFMUs)
                {
                    int j = Unit2.IndexAllFMU;
                    if (i < j)
                    {
                        var distU1U2 = FMUDistances[i, j];
                        if (distU1U2 < MaxDistInBlock)
                        {
                            Unit1.addFMUProxInBlock(j);
                            Unit2.addFMUProxInBlock(i);
                        }
                        else
                        {
                            Unit1.RemoveFMUProxInBlock(j);
                            Unit2.RemoveFMUProxInBlock(i);
                        }
                        if (distU1U2 < MaxDistOutBlockInPeriod)
                        {
                            Unit1.addFMUProxOutBlockInPeriod(j);
                            Unit2.addFMUProxOutBlockInPeriod(i);
                        }
                        else
                        {
                            Unit1.RemoveFMUProxOutBlockInPeriod(j);
                            Unit2.RemoveFMUProxOutBlockInPeriod(i);
                        }
                        if (distU1U2 < 2 * MaxDistConsecutivePeriod)
                        {
                            Unit1.addFMUProxConsecutivePeriod(j);
                            Unit2.addFMUProxConsecutivePeriod(i);
                        }
                        else
                        {
                            Unit1.removeFMUProxConsecutivePeriod(j);
                            Unit2.removeFMUProxConsecutivePeriod(i);
                        }
                    }
                }
            }


            MinimalHarvestAge = Convert.ToDouble(percents[9]);

            string[] namepar = {"TotalArea ",
                                "MinimalHarvestArea ",
                                "MaximalHarvestArea ",
                                "DemandFactor ",
                                "MaxNumBlocks ",
                                "VolumeVariation ",
                                "NewBlockPercent ",
                                "DeltaDistance ",
                                "MaxDistInBlock ",
                                "MaxDistOutBlockInPeriod ",
                                "MaxDistConsecutivePeriod ",
                                "NumSortiments ",
                                "MinimalHarvestAge ",
                                "Distances ",
                                "Delta_Bloco ",
                                "Delta_Ano ",
                                "Delta_Consec ",
                                "FactorHarvestArea "
                                };
            string[] pars = {TotalArea.ToString(),
                             MinimalHarvestArea.ToString(),
                             MaximalHarvestArea.ToString(),
                             DemandFactor.ToString(),
                             MaxNumBlocks.ToString(),
                             VolumeVariation.ToString(),
                             NewBlockPercent.ToString(),
                             DeltaDistance.ToString(),
                             MaxDistInBlock.ToString(),
                             MaxDistOutBlockInPeriod.ToString(),
                             MaxDistConsecutivePeriod.ToString(),
                             NumSortiments.ToString(),
                             MinimalHarvestAge.ToString(),
                             ToolBox.WriteTable(FMUDistances),
                             ToolBox.WriteTable(ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistInBlock))),
                             ToolBox.WriteTable(ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistOutBlockInPeriod))),
                             ToolBox.WriteTable(ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistConsecutivePeriod))),
                             percents[10].ToString()
                               };

            string[] types = {TotalArea.GetType().ToString(),
                             MinimalHarvestArea.GetType().ToString(),
                             MaximalHarvestArea.GetType().ToString(),
                             DemandFactor.GetType().ToString(),
                             MaxNumBlocks.GetType().ToString(),
                             VolumeVariation.GetType().ToString(),
                             NewBlockPercent.GetType().ToString(),
                             DeltaDistance.GetType().ToString(),
                             MaxDistInBlock.GetType().ToString(),
                             MaxDistOutBlockInPeriod.GetType().ToString(),
                             MaxDistConsecutivePeriod.GetType().ToString(),
                             NumSortiments.GetType().ToString(),
                             MinimalHarvestAge.GetType().ToString(),
                             FMUDistances.GetType().ToString(),
                             ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistInBlock)).GetType().ToString(),
                             ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistOutBlockInPeriod)).GetType().ToString(),
                             ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistConsecutivePeriod)).GetType().ToString(),
                             percents[10].GetType().ToString()
                               };
            for (int count = 0; count < namepar.Length; count++)
            {
                Param.Append(string.Format("{0,25}\t{1,10}\t{2,10}\n", namepar[count], pars[count], types[count]));
            }
        }

        private void ReadInstance(string path, decimal[] percents)
        {
            string AdjFile = path + "Adjacency_" + Name + ".csv";
            string CrdFile = path + "Centroids_" + Name + ".csv";
            string PrfFile = path + "Profit_" + Name + ".csv";
            string StdFile = path + "Stands_" + Name + ".csv";
            string VolFile = path + "volume_" + Name + ".csv";

            AllFMUs = new List<FMU>();
            FMUsForFirstPeriodHarvest = new List<FMU>();
            AdjacentFMUs = new List<Tuple<int, int>>();
            AllPeriods = new List<Period>();
            AllBlocks = new List<Block>();

            char[] charSeparators = new char[] { ',', ' ', ':', '(', ')', '[', ']', '\t' };

            //File with UNIT_ID,Unit_area (acres),Unit_age (10 years)
            string[] AllStdFile = File.ReadAllLines(StdFile);
            NumFMUs = AllStdFile.Length - 1;
            MaxNumBlocks = Convert.ToInt32(Convert.ToDouble(percents[8] / 100) * NumFMUs);

            foreach (string Line in AllStdFile)
            {
                if (Line != AllStdFile.First())
                {
                    string CurrentLine = Line.TrimStart();
                    string[] SplitLine = CurrentLine.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                    FMU NewUnit = new FMU(Name + SplitLine[0], ToolBox.ReadDouble(SplitLine[2]), ToolBox.ReadDouble(SplitLine[1]));
                    if (!(AllFMUs.Contains(NewUnit)) && GetFMUIndexFromName(NewUnit.Name) < 0)
                    {
                        AllFMUs.Add(NewUnit);
                        NewUnit.IndexAllFMU = AllFMUs.Count - 1;
                    }
                }
            }

            //File with UNIT_ID,X,Y
            string[] AllCrdFile = File.ReadAllLines(CrdFile);
            foreach (string Line in AllCrdFile)
            {
                if (Line != AllCrdFile.First())
                {
                    string CurrentLine = Line.TrimStart();
                    string[] SplitLine = CurrentLine.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                    FMU Unit = AllFMUs[GetFMUIndexFromName(Name + SplitLine[0])];
                    Unit.CoordX = ToolBox.ReadDouble(SplitLine[1]);
                    Unit.CoordY = ToolBox.ReadDouble(SplitLine[2]);
                }
            }

            //Calc distances
            double TotalDistance = 0;
            FMUDistances = new double[NumFMUs, NumFMUs];
            foreach (FMU I in AllFMUs)
            {
                int i = I.IndexAllFMU;
                foreach (FMU J in AllFMUs)
                {
                    int j = J.IndexAllFMU;
                    if (i < j)
                    {
                        FMUDistances[i, j] = ToolBox.Distancia2Pontos(I.CoordX, J.CoordX, I.CoordY, J.CoordY);
                        FMUDistances[j, i] = FMUDistances[i, j];
                        TotalDistance += FMUDistances[i, j];
                    }
                }
            }

            //File with UNIT_ID,UNIT_adjacent
            string[] AllAdjFile = File.ReadAllLines(AdjFile);
            foreach (string Line in AllAdjFile)
            {
                if (Line != AllAdjFile.First())
                {
                    string CurrentLine = Line.TrimStart();
                    string[] SplitLine = CurrentLine.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                    Tuple<int, int> inds = new Tuple<int, int>(GetFMUIndexFromName(Name + SplitLine[0]), GetFMUIndexFromName(Name + SplitLine[1]));
                    AdjacentFMUs.Add(inds);
                }
            }

            //File With UNIT_ID,profit_no_harvest,profit p1,profit p2,profit p3,profit p4,profit p5
            string[] AllPrfFile = File.ReadAllLines(PrfFile);
            foreach (string Line in AllPrfFile)
            {
                string CurrentLine = Line.TrimStart();
                CurrentLine.Replace("profit p", "");
                string[] SplitLine = CurrentLine.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (Line == AllPrfFile.First())
                {
                    NumPeriodsHP = SplitLine.Count() - 1;
                    LiquidPresentValues = new double[NumFMUs, NumPeriodsHP];
                    for (int p = 0; p < NumPeriodsHP; p++)
                    {
                        Period NewP = new Period(p.ToString(), p);
                        if (!(AllPeriods.Contains(NewP)))
                        {
                            AllPeriods.Add(NewP);
                        }
                    }
                }
                else
                {
                    int IndFMU = GetFMUIndexFromName(Name + SplitLine[0]);
                    for (int j = 1; j < SplitLine.Length; j++)
                    {
                        LiquidPresentValues[IndFMU, j - 1] = ToolBox.ReadDouble(SplitLine[j]);
                    }
                }
            }

            //File With UNIT_ID,volume p1,volume p2,volume p3,volume p4,volume p5
            NumSortiments = 1;
            SortimentVolumes = new double[NumSortiments, NumFMUs, NumPeriodsHP];
            string[] AllVolFile = File.ReadAllLines(VolFile);
            foreach (string Line in AllVolFile)
            {
                if (Line != AllVolFile.First())
                {
                    string CurrentLine = Line.TrimStart();
                    string[] SplitLine = CurrentLine.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                    int IndFMU = GetFMUIndexFromName(Name + SplitLine[0]);
                    for (int j = 1; j < SplitLine.Length; j++)
                    {
                        SortimentVolumes[0, IndFMU, j - 1] = ToolBox.ReadDouble(SplitLine[j]);
                    }
                }
            }

            //Calculated Parameters


            double TotalArea = 0;
            foreach (FMU Unit in AllFMUs)
            {
                TotalArea += Unit.Area;
            }

            decimal MedTotalArea = Convert.ToDecimal(TotalArea / (MaxNumBlocks * NumPeriodsHP)) * percents[10];
            MinimalHarvestArea = Convert.ToInt32(MedTotalArea * percents[0] / 100);
            MaximalHarvestArea = Convert.ToInt32(MedTotalArea * percents[1] / 100);
            SortimentDemands = new double[NumSortiments, NumPeriodsHP];
            double DemandFactor = Convert.ToDouble(percents[2] / 100);

            for (int s = 0; s < NumSortiments; s++)
            {
                foreach (Period P in AllPeriods)
                {
                    int p = P.IndexPeriod;
                    double demand = 0;
                    foreach (FMU Unit in AllFMUs)
                    {
                        int i = Unit.IndexAllFMU;
                        demand += SortimentVolumes[s, i, p];
                    }
                    SortimentDemands[s, p] = DemandFactor * demand;
                }
            }

            VolumeVariation = Convert.ToDouble(percents[3] / 100);
            NewBlockPercent = Convert.ToDouble(percents[4] / 100);
            double MedDistance = TotalDistance / (NumFMUs * (NumFMUs - 1));
            MaxDistInBlock = Convert.ToDouble(percents[5] / 100) * MedDistance;
            MaxDistOutBlockInPeriod = Convert.ToDouble(percents[6] / 100) * MedDistance;
            MaxDistConsecutivePeriod = Convert.ToDouble(percents[7] / 100) * MedDistance;

            //Lists of distance
            foreach (FMU Unit1 in AllFMUs)
            {
                int i = Unit1.IndexAllFMU;
                foreach (FMU Unit2 in AllFMUs)
                {
                    int j = Unit2.IndexAllFMU;
                    if (i < j)
                    {
                        var distU1U2 = FMUDistances[i, j];
                        if (distU1U2 < MaxDistInBlock)
                        {
                            Unit1.addFMUProxInBlock(j);
                            Unit2.addFMUProxInBlock(i);
                        }
                        else
                        {
                            Unit1.RemoveFMUProxInBlock(j);
                            Unit2.RemoveFMUProxInBlock(i);
                        }
                        if (distU1U2 < MaxDistOutBlockInPeriod)
                        {
                            Unit1.addFMUProxOutBlockInPeriod(j);
                            Unit2.addFMUProxOutBlockInPeriod(i);
                        }
                        else
                        {
                            Unit1.RemoveFMUProxOutBlockInPeriod(j);
                            Unit2.RemoveFMUProxOutBlockInPeriod(i);
                        }
                        if (distU1U2 < 2 * MaxDistConsecutivePeriod)
                        {
                            Unit1.addFMUProxConsecutivePeriod(j);
                            Unit2.addFMUProxConsecutivePeriod(i);
                        }
                        else
                        {
                            Unit1.removeFMUProxConsecutivePeriod(j);
                            Unit2.removeFMUProxConsecutivePeriod(i);
                        }
                    }
                }
            }


            MinimalHarvestAge = Convert.ToDouble(percents[9]);

            string[] namepar = {"TotalArea ",
                                "MinimalHarvestArea ",
                                "MaximalHarvestArea ",
                                "DemandFactor ",
                                "MaxNumBlocks ",
                                "VolumeVariation ",
                                "NewBlockPercent ",
                                "MedDistance ",
                                "MaxDistInBlock ",
                                "MaxDistOutBlockInPeriod ",
                                "MaxDistConsecutivePeriod ",
                                "NumSortiments ",
                                "MinimalHarvestAge ",
                                "Distances ",
                                "Delta_Bloco ",
                                "Delta_Ano ",
                                "Delta_Consec ",
                                "FactorHarvestArea "
                                };
            string[] pars = {TotalArea.ToString(),
                             MinimalHarvestArea.ToString(),
                             MaximalHarvestArea.ToString(),
                             DemandFactor.ToString(),
                             MaxNumBlocks.ToString(),
                             VolumeVariation.ToString(),
                             NewBlockPercent.ToString(),
                             MedDistance.ToString(),
                             MaxDistInBlock.ToString(),
                             MaxDistOutBlockInPeriod.ToString(),
                             MaxDistConsecutivePeriod.ToString(),
                             NumSortiments.ToString(),
                             MinimalHarvestAge.ToString(),
                             ToolBox.WriteTable(FMUDistances),
                             ToolBox.WriteTable(ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistInBlock))),
                             ToolBox.WriteTable(ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistOutBlockInPeriod))),
                             ToolBox.WriteTable(ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistConsecutivePeriod))),
                             percents[10].ToString()
                               };

            string[] types = {TotalArea.GetType().ToString(),
                             MinimalHarvestArea.GetType().ToString(),
                             MaximalHarvestArea.GetType().ToString(),
                             DemandFactor.GetType().ToString(),
                             MaxNumBlocks.GetType().ToString(),
                             VolumeVariation.GetType().ToString(),
                             NewBlockPercent.GetType().ToString(),
                             MedDistance.GetType().ToString(),
                             MaxDistInBlock.GetType().ToString(),
                             MaxDistOutBlockInPeriod.GetType().ToString(),
                             MaxDistConsecutivePeriod.GetType().ToString(),
                             NumSortiments.GetType().ToString(),
                             MinimalHarvestAge.GetType().ToString(),
                             FMUDistances.GetType().ToString(),
                             ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistInBlock)).GetType().ToString(),
                             ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistOutBlockInPeriod)).GetType().ToString(),
                             ToolBox.LessThan(FMUDistances,Convert.ToDouble(MaxDistConsecutivePeriod)).GetType().ToString(),
                             percents[10].GetType().ToString()
                               };
            for (int count = 0; count < namepar.Length; count++)
            {
                Param.Append(string.Format("{0,25}\t{1,10}\t{2,10}\n", namepar[count], pars[count], types[count]));
            }

            //AllBlocks.Add(new Block("Inicial", 0));


            //public Dictionary<int, int> NumBlocksByPeriod;
        }

        public int GetFMUIndexFromName(string name)
        {
            var pick = from fmu in AllFMUs
                       where fmu.Name == name
                       select fmu;
            if (pick.Count() > 0)
            {
                return pick.First().IndexAllFMU;
            } else
            {
                return -1;
            }
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;
        }

    }

    class CARP_FH : IDisposable ///forest harvesting
    {
        private bool _disposed = false;
        //Variaveis de geracao do modelo Gurobi
        public GRBEnv Ambiente; //{ get; set; }
        public GRBModel Modelo; //{ get; set; }
        public string NomeArquivo = String.Empty;
        //Variável de contagem do tempo de execução
        public Stopwatch Cronometro; //{ get; set; }

        //Resultado do Modelo
        public Tuple<bool, string, string> resp = new Tuple<bool, string, string>(false, "Modelo Novo não resolvido", "New");
        public double ModelObjVal;
        public Tuple<bool, string, string> resp_relax = new Tuple<bool, string, string>(false, "Modelo Relaxado Novo não resolvido", "New Relax");
        public double ModelObjVal_relax;

        //Parametros do Problema;
        public FHInstance Instance;
        //public string Pasta = ToolBox.Path(@"\Projects\Chu_1\ModelosLP\");

        public string ToJson(string Pasta, string _fileInst)
        {
            return ToolBox.ObjToJson(Instance, Pasta, _fileInst);
        }
        public override string ToString()
        {
            return "Modelo de " + NomeArquivo;
        }
        public GRBLinExpr GrupoExpr02_03(int indexPeriod, int indexSortiment, GRBVar[,,] V, GRBVar[,] Slack, GRBVar[,] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int indexBlock = 0; indexBlock < Instance.MaxNumBlocks; indexBlock++)
            {
                foreach (FMU Unit in Instance.AllFMUs)
                {
                    int indexForestUnit = Unit.IndexAllFMU;
                    expr.AddTerm(Instance.SortimentVolumes[indexSortiment, indexForestUnit, indexPeriod], V[indexForestUnit, indexPeriod, indexBlock]);
                }
            }

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexPeriod, indexSortiment]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexPeriod, indexSortiment]);
            }
            expr.AddConstant(-term);

            return expr;
        }
        public GRBLinExpr GrupoExpr04(int indexForestUnit, GRBVar[,,] V, GRBVar[] Slack, GRBVar[] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int indexBlock = 0; indexBlock < Instance.MaxNumBlocks; indexBlock++)
            {
                foreach (Period P in Instance.AllPeriods)
                {
                    int indexPeriod = P.IndexPeriod;
                    expr.AddTerm(1, V[indexForestUnit, indexPeriod, indexBlock]);
                }
            }

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexForestUnit]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexForestUnit]);
            }
            expr.AddConstant(-term);

            return expr;
        }
        public GRBLinExpr GrupoExpr05_06(int indexPeriod, int indexBlock, GRBVar[,] V1, GRBVar[,,] V2, GRBVar[,] Slack, GRBVar[,] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            foreach (FMU Unit in Instance.AllFMUs)
            {
                int indexForestUnit = Unit.IndexAllFMU;
                expr.AddTerm(Unit.Area, V2[indexForestUnit, indexPeriod, indexBlock]);
            }

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexPeriod, indexBlock]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexPeriod, indexBlock]);
            }
            expr.AddTerm(-term, V1[indexPeriod, indexBlock]);

            return expr;
        }
        public GRBLinExpr GrupoExpr07(int[] indexForestUnits, int indexPeriod, int indexBlock, GRBVar[,,] V, GRBVar[,,,] Slack, GRBVar[,,,] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(Instance.FMUDistances[indexForestUnits[0], indexForestUnits[1]], V[indexForestUnits[0], indexPeriod, indexBlock]);
            expr.AddTerm(Instance.FMUDistances[indexForestUnits[0], indexForestUnits[1]], V[indexForestUnits[1], indexPeriod, indexBlock]);
            expr.AddConstant(-Instance.FMUDistances[indexForestUnits[0], indexForestUnits[1]]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexForestUnits[0], indexForestUnits[1], indexPeriod, indexBlock]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexForestUnits[0], indexForestUnits[1], indexPeriod, indexBlock]);
            }
            expr.AddConstant(-term);

            return expr;

        }
        public GRBLinExpr GrupoExpr08(int[] indexForestUnits, int indexPeriod, int[] indexBlock, GRBVar[,,] V, GRBVar[,,,,] Slack, GRBVar[,,,,] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(Instance.FMUDistances[indexForestUnits[0], indexForestUnits[1]], V[indexForestUnits[0], indexPeriod, indexBlock[0]]);
            expr.AddTerm(Instance.FMUDistances[indexForestUnits[0], indexForestUnits[1]], V[indexForestUnits[1], indexPeriod, indexBlock[1]]);
            expr.AddConstant(-Instance.FMUDistances[indexForestUnits[0], indexForestUnits[1]]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexForestUnits[0], indexForestUnits[1], indexPeriod, indexBlock[0], indexBlock[1]]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexForestUnits[0], indexForestUnits[1], indexPeriod, indexBlock[0], indexBlock[1]]);
            }
            expr.AddConstant(-term);

            return expr;

        }
        public GRBLinExpr GrupoExpr09(Tuple<int, int> indexForestUnits, int indexPeriod, int[] indexBlock, GRBVar[,,] V, GRBVar[,,,,] Slack, GRBVar[,,,,] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, V[indexForestUnits.Item1, indexPeriod, indexBlock[0]]);
            expr.AddTerm(1, V[indexForestUnits.Item2, indexPeriod, indexBlock[1]]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexForestUnits.Item1, indexForestUnits.Item2, indexPeriod, indexBlock[0], indexBlock[1]]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexForestUnits.Item1, indexForestUnits.Item2, indexPeriod, indexBlock[0], indexBlock[1]]);
            }
            expr.AddConstant(-term);

            return expr;

        }
        public GRBLinExpr GrupoExpr10(Tuple<int, int> indexForestUnits, int indexPeriod, int[] indexBlock, GRBVar[,,] V, GRBVar[,,,,] Slack, GRBVar[,,,,] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, V[indexForestUnits.Item1, indexPeriod, indexBlock[0]]);
            expr.AddTerm(1, V[indexForestUnits.Item2, indexPeriod + 1, indexBlock[1]]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexForestUnits.Item1, indexForestUnits.Item2, indexPeriod, indexBlock[0], indexBlock[1]]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexForestUnits.Item1, indexForestUnits.Item2, indexPeriod, indexBlock[0], indexBlock[1]]);
            }
            expr.AddConstant(-term);

            return expr;

        }
        public GRBLinExpr GrupoExpr11A(Tuple<int, int> indexForestUnits, int indexPeriod, int[] indexBlock, GRBVar[,,] V, GRBVar[,,,,] Slack, GRBVar[,,,,] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(Instance.FMUDistances[indexForestUnits.Item1, indexForestUnits.Item2], V[indexForestUnits.Item1, indexPeriod, indexBlock[0]]);
            expr.AddTerm(Instance.FMUDistances[indexForestUnits.Item1, indexForestUnits.Item2], V[indexForestUnits.Item2, indexPeriod + 1, indexBlock[1]]);
            expr.AddConstant(-2 * Instance.MaxDistConsecutivePeriod);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexForestUnits.Item1, indexForestUnits.Item2, indexPeriod, indexBlock[0], indexBlock[1]]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexForestUnits.Item1, indexForestUnits.Item2, indexPeriod, indexBlock[0], indexBlock[1]]);
            }
            expr.AddConstant(-term);

            return expr;

        }
        public GRBLinExpr GrupoExpr11B(Tuple<int, int> indexForestUnits, int indexPeriod, int[] indexBlock, GRBVar[,,] V, GRBVar[,,,,] Slack, GRBVar[,,,,] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(Instance.FMUDistances[indexForestUnits.Item1, indexForestUnits.Item2], V[indexForestUnits.Item1, indexPeriod, indexBlock[0]]);
            expr.AddTerm(Instance.FMUDistances[indexForestUnits.Item1, indexForestUnits.Item2], V[indexForestUnits.Item2, indexPeriod + 1, indexBlock[1]]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexForestUnits.Item1, indexForestUnits.Item2, indexPeriod, indexBlock[0], indexBlock[1]]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexForestUnits.Item1, indexForestUnits.Item2, indexPeriod, indexBlock[0], indexBlock[1]]);
            }
            expr.AddConstant(-term);

            return expr;

        }
        public GRBLinExpr GrupoExpr12(int indexPeriod, GRBVar[,] V, GRBVar[] Slack, GRBVar[] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, V[indexPeriod, 1]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexPeriod]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexPeriod]);
            }
            expr.AddConstant(-term);

            return expr;
        }
        public GRBLinExpr GrupoExpr13(int indexSortiment, int indexPeriod, int indexBlock, GRBVar[,] V1, GRBVar[,,] V2, GRBVar[,,] Slack, GRBVar[,,] Excess, int CoefSlackExcess)
        {
            GRBLinExpr expr = new GRBLinExpr();

            double coef = (1 + Instance.VolumeVariation) * Instance.SortimentDemands[indexSortiment, indexPeriod];

            expr.AddTerm(coef * Instance.NewBlockPercent, V1[indexPeriod, indexBlock]);
            expr.AddConstant(-coef);

            for (int indexBlock2 = 0; indexBlock2 < indexBlock - 1; indexBlock2++)
            {
                foreach (FMU Unit in Instance.AllFMUs)
                {
                    int indexFMU = Unit.IndexAllFMU;
                    expr.AddTerm(Instance.SortimentVolumes[indexSortiment, indexFMU, indexPeriod], V2[indexFMU, indexPeriod, indexBlock2]);
                }
            }

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexSortiment, indexPeriod, indexBlock]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexSortiment, indexPeriod, indexBlock]);
            }//expr.AddConstant(-term);

            return expr;
        }
        public GRBLinExpr GrupoExpr14(FMU Unit, int indexPeriod, int indexBlock, GRBVar[,,] V, GRBVar[,,] Slack, GRBVar[,,] Excess, int CoefSlackExcess)
        {
            GRBLinExpr expr = new GRBLinExpr();

            int indexFMU = Unit.IndexAllFMU;

            double coef = (Unit.InitialAge + indexPeriod - 1 - Instance.MinimalHarvestAge);

            expr.AddTerm(coef, V[indexFMU, indexPeriod, indexBlock]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexFMU, indexPeriod, indexBlock]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexFMU, indexPeriod, indexBlock]);
            }//expr.AddConstant(-term);

            return expr;
        }
        public GRBLinExpr GrupoExpr15(int indexFMU, GRBVar[,,] V, GRBVar[] Slack, GRBVar[] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, V[indexFMU, 0, 0]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexFMU]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexFMU]);
            }
            expr.AddConstant(-term);

            return expr;
        }
        public GRBLinExpr GrupoExpr17(int indexForestUnit, GRBVar[,,] V, GRBVar[] Slack, GRBVar[] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int indexBlock = 0; indexBlock < Instance.MaxNumBlocks; indexBlock++)
            {
                foreach (Period P in Instance.AllPeriods)
                {
                    int indexPeriod = P.IndexPeriod;
                    expr.AddTerm(1, V[indexForestUnit, indexPeriod, indexBlock]);
                }
            }

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexForestUnit]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexForestUnit]);
            }
            expr.AddConstant(-term);

            return expr;
        }
        public GRBLinExpr GrupoExpr18(int indexPeriod, int indexBlock, GRBVar[,] V1, GRBVar[,,] V2, GRBVar[,] Slack, GRBVar[,] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            foreach (FMU Unit in Instance.AllFMUs)
            {
                int indexForestUnit = Unit.IndexAllFMU;
                expr.AddTerm(1, V2[indexForestUnit, indexPeriod, indexBlock]);
            }

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexPeriod, indexBlock]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexPeriod, indexBlock]);
            }
            
            expr.AddTerm(-Instance.AllFMUs.Count, V1[indexPeriod, indexBlock]);
            expr.AddConstant(-term);

            return expr;
        }
        public GRBLinExpr GrupoExpr19(FMU Unit, int indexPeriod, int indexBlock, GRBVar[,] V1, GRBVar[,,] V2, GRBVar[,,] Slack, GRBVar[,,] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            int indexFMU = Unit.IndexAllFMU;

            expr.AddTerm(1, V2[indexFMU, indexPeriod, indexBlock]);
            expr.AddTerm(-1, V1[indexPeriod, indexBlock]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexFMU, indexPeriod, indexBlock]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexFMU, indexPeriod, indexBlock]);
            }
            expr.AddConstant(-term);

            return expr;
        }
        public GRBLinExpr GrupoExpr20(int indexFMU0, int indexFMU1, int indexPeriod, int indexBlock, GRBVar[,,] V, GRBVar[,,,] Slack, GRBVar[,,,] Excess, int CoefSlackExcess, double term)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, V[indexFMU0, indexPeriod, indexBlock]);
            expr.AddTerm(1, V[indexFMU1, indexPeriod, indexBlock]);
            expr.AddConstant(-1);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slack[indexFMU0, indexFMU1, indexPeriod, indexBlock]);
                expr.AddTerm(-CoefSlackExcess, Excess[indexFMU0, indexFMU1, indexPeriod, indexBlock]);
            }
            expr.AddConstant(-term);

            return expr;
        }
        public void CreateModel(string Pasta, bool[] UseConstraints, bool versao, bool BinaryVars, bool[] SlackExcess, double[] RHS, bool ShowMess)
        {
            //Var aux
            List<(string, int)> AuxIndexes;
            string ConstName;
            bool OldVersion = versao;
            int[] CoefSlackExcess = new int[SlackExcess.Length];
            for (int i = 0; i < SlackExcess.Length; i++)
            {
                if (SlackExcess[i])
                {
                    CoefSlackExcess[i] = 1;
                }
                else
                {
                    CoefSlackExcess[i] = 0;
                }
            }
            char VarType = GRB.CONTINUOUS;
            if (BinaryVars)
            {
                VarType = GRB.BINARY;
            }

            //Arquivos do Modelo
            NomeArquivo = "_pFH_" + Instance.FileName;
            string Agora = "(" + ToolBox.GetNow() + ")";
            StringBuilder CountConstraint = new StringBuilder();
            int[] CounterConstraint = new int[25];
            StringBuilder CountName = new StringBuilder();
            Ambiente = new GRBEnv
            {
                LogFile = Pasta + NomeArquivo + Agora + ".log"
            };

            Modelo = new GRBModel(Ambiente)
            {
                ModelSense = GRB.MINIMIZE,
                ModelName = NomeArquivo + Agora
            };
            
            // Must set LazyConstraints parameter when using lazy constraints

            Modelo.Parameters.LazyConstraints = 1;
            
            //Criar Variáveis
            GRBVar[,,] X = new GRBVar[Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,] Alpha = new GRBVar[Instance.NumPeriodsHP, Instance.MaxNumBlocks];

            //Definir tipo das variaveis e coeficientes da F.O.
            foreach (FMU Unit in Instance.AllFMUs)
            {
                int i = Unit.IndexAllFMU;
                for (int b = 0; b < Instance.MaxNumBlocks; b++)
                {
                    foreach (Period P in Instance.AllPeriods)
                    {
                        int p = P.IndexPeriod;
                        AuxIndexes = new List<(string, int)> { ("fmu", i), ("period", p), ("block", b) };
                        string Xname = ToolBox.ConstraintName("X", AuxIndexes);
                        X[i, p, b] = Modelo.AddVar(0, 1, Instance.LiquidPresentValues[i, p], VarType, Xname.ToString());
                    }
                }
            }
    
            foreach(Period P in Instance.AllPeriods)
            {
                int p = P.IndexPeriod;
                for (int b = 0; b < Instance.MaxNumBlocks; b++)
                {
                    AuxIndexes = new List<(string, int)> { ("period", p), ("block", b) };
                    string Alphaname = ToolBox.ConstraintName("Alpha", AuxIndexes);
                    Alpha[p, b] = Modelo.AddVar(0, 1, 0, VarType, Alphaname.ToString());
                }
            }

            //variaveis e coeficientes de falta e excesso            
            GRBVar[,] SlkR02 = new GRBVar[Instance.NumPeriodsHP, Instance.NumSortiments];
            GRBVar[,] ExcR02 = new GRBVar[Instance.NumPeriodsHP, Instance.NumSortiments];
            GRBVar[,] SlkR03 = new GRBVar[Instance.NumPeriodsHP, Instance.NumSortiments];
            GRBVar[,] ExcR03 = new GRBVar[Instance.NumPeriodsHP, Instance.NumSortiments];
            GRBVar[] SlkR04 = new GRBVar[Instance.NumFMUs];
            GRBVar[] ExcR04 = new GRBVar[Instance.NumFMUs];
            GRBVar[,] SlkR05 = new GRBVar[Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,] ExcR05 = new GRBVar[Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,] SlkR06 = new GRBVar[Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,] ExcR06 = new GRBVar[Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,,] SlkR07A = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,,] ExcR07A = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,,] SlkR07B = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,,] ExcR07B = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,,,] SlkR08A = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[,,,,] ExcR08A = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[,,,,] SlkR08B = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[,,,,] ExcR08B = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[,,,,] SlkR09 = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[,,,,] ExcR09 = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[,,,,] SlkR10 = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[,,,,] ExcR10 = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[,,,,] SlkR11A = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[,,,,] ExcR11A = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[,,,,] SlkR11B = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[,,,,] ExcR11B = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks, Instance.MaxNumBlocks];
            GRBVar[] SlkR12 = new GRBVar[Instance.NumPeriodsHP];
            GRBVar[] ExcR12 = new GRBVar[Instance.NumPeriodsHP];
            GRBVar[,,] SlkR13 = new GRBVar[Instance.NumSortiments, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,] ExcR13 = new GRBVar[Instance.NumSortiments, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,] SlkR14 = new GRBVar[Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,] ExcR14 = new GRBVar[Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[] SlkR15 = new GRBVar[Instance.NumFMUs];
            GRBVar[] ExcR15 = new GRBVar[Instance.NumFMUs];
            GRBVar[] SlkR17 = new GRBVar[Instance.NumFMUs];
            GRBVar[] ExcR17 = new GRBVar[Instance.NumFMUs];
            GRBVar[,] SlkR18 = new GRBVar[Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,] ExcR18 = new GRBVar[Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,] SlkR19 = new GRBVar[Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,] ExcR19 = new GRBVar[Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,,] SlkR20 = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            GRBVar[,,,] ExcR20 = new GRBVar[Instance.NumFMUs, Instance.NumFMUs, Instance.NumPeriodsHP, Instance.MaxNumBlocks];
            
            if (UseConstraints[2] && SlackExcess[2])
            {
                for (int p = 0; p < Instance.NumPeriodsHP; p++)
                {
                    for (int s = 0; s < Instance.NumSortiments; s++)
                    {
                        AuxIndexes = new List<(string, int)> { ("period", p), ("product", s) };
                        SlkR02[p, s] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                     ToolBox.ConstraintName("SlkR02", AuxIndexes));
                        ExcR02[p, s] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                     ToolBox.ConstraintName("ExcR02", AuxIndexes));
                    }
                }
            }
            if (UseConstraints[3] && SlackExcess[3])
            {
                for (int p = 0; p < Instance.NumPeriodsHP; p++)
                {
                    for (int s = 0; s < Instance.NumSortiments; s++)
                    {
                        AuxIndexes = new List<(string, int)> { ("period", p), ("product", s) };
                        SlkR03[p, s] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                       ToolBox.ConstraintName("SlkR03", AuxIndexes));
                        ExcR03[p, s] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                        ToolBox.ConstraintName("ExcR03", AuxIndexes));
                    }
                }
            }
            if (UseConstraints[4] && SlackExcess[4])
            {
                for (int i = 0; i < Instance.NumFMUs; i++)
                {
                    SlkR04[i] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                ToolBox.ConstraintName("SlkR04", ("fmu", i)));
                    ExcR04[i] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                 ToolBox.ConstraintName("ExcR04", ("fmu", i)));
                }
            }
            if (UseConstraints[5] && SlackExcess[5])
            {
                for (int p = 0; p < Instance.NumPeriodsHP; p++)
                {
                    for (int b = 0; b < Instance.MaxNumBlocks; b++)
                    {
                        AuxIndexes = new List<(string, int)> { ("period", p), ("block", b) };
                        SlkR05[p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                    ToolBox.ConstraintName("SlkR05", AuxIndexes));
                        ExcR05[p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                    ToolBox.ConstraintName("ExcR05", AuxIndexes));
                    }
                }
            }
            if (UseConstraints[6] && SlackExcess[6])
            {
                for (int p = 0; p < Instance.NumPeriodsHP; p++)
                {
                    for (int b = 0; b < Instance.MaxNumBlocks; b++)
                    {
                        AuxIndexes = new List<(string, int)> { ("period", p), ("block", b) };
                        SlkR06[p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                       ToolBox.ConstraintName("SlkR06", AuxIndexes));
                        ExcR06[p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                        ToolBox.ConstraintName("ExcR06", AuxIndexes));
                    }
                }
            }
            if (UseConstraints[7] && SlackExcess[7])
            {
                for (int i = 0; i < Instance.NumFMUs; i++)
                {
                    for (int j = 0; j < Instance.NumFMUs; j++)
                    {
                        for (int b = 0; b < Instance.MaxNumBlocks; b++)
                        {
                            for (int p = 0; p < Instance.NumPeriodsHP; p++)
                            {
                                AuxIndexes = new List<(string, int)> { ("fmu", i), ("fmu", j), ("period", p), ("block", b) };
                                if (OldVersion)
                                {
                                    SlkR07A[i, j, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                          ToolBox.ConstraintName("SlkR07A", AuxIndexes));
                                    ExcR07A[i, j, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                           ToolBox.ConstraintName("ExcR07A", AuxIndexes));
                                }
                                else
                                {
                                    SlkR07B[i, j, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                          ToolBox.ConstraintName("SlkR07B", AuxIndexes));
                                    ExcR07B[i, j, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                           ToolBox.ConstraintName("ExcR07B", AuxIndexes));
                                }
                            }
                        }
                    }
                }
            }
            if (UseConstraints[8] && SlackExcess[8])
            {
                for (int i = 0; i < Instance.NumFMUs; i++)
                {
                    for (int j = 0; j < Instance.NumFMUs; j++)
                    {
                        for (int p = 0; p < Instance.NumPeriodsHP; p++)
                        {
                            for (int b = 0; b < Instance.MaxNumBlocks; b++)
                            {
                                for (int h = 0; h < Instance.MaxNumBlocks; h++)
                                {
                                    AuxIndexes = new List<(string, int)> { ("fmu", i), ("fmu", j), ("period", p), ("block", b), ("block", h) };
                                    if (OldVersion)
                                    {
                                        SlkR08A[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                               ToolBox.ConstraintName("SlkR08A", AuxIndexes));
                                        ExcR08A[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                               ToolBox.ConstraintName("ExcR08A", AuxIndexes));
                                    }
                                    else
                                    {
                                        SlkR08B[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                               ToolBox.ConstraintName("SlkR08B", AuxIndexes));
                                        ExcR08B[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                               ToolBox.ConstraintName("ExcR08B", AuxIndexes));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (UseConstraints[9] && SlackExcess[9])
            {
                for (int i = 0; i < Instance.NumFMUs; i++)
                {
                    for (int j = 0; j < Instance.NumFMUs; j++)
                    {
                        for (int b = 0; b < Instance.MaxNumBlocks; b++)
                        {
                            for (int p = 0; p < Instance.NumPeriodsHP; p++)
                            {
                                for (int h = 0; h < Instance.MaxNumBlocks; h++)
                                {
                                    AuxIndexes = new List<(string, int)> { ("fmu", i), ("fmu", j), ("period", p), ("block", b), ("block", h) };
                                    SlkR09[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                          ToolBox.ConstraintName("SlkR09", AuxIndexes));
                                    ExcR09[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                          ToolBox.ConstraintName("ExcR09", AuxIndexes));
                                }
                            }
                        }
                    }
                }
            }
            if (UseConstraints[10] && SlackExcess[10])
            {
                for (int i = 0; i < Instance.NumFMUs; i++)
                {
                    for (int j = 0; j < Instance.NumFMUs; j++)
                    {
                        for (int p = 0; p < Instance.NumPeriodsHP; p++)
                        {
                            for (int b = 0; b < Instance.MaxNumBlocks; b++)
                            {
                                for (int h = 0; h < Instance.MaxNumBlocks; h++)
                                {
                                    AuxIndexes = new List<(string, int)> { ("fmu", i), ("fmu", j), ("period", p), ("block", b), ("block", h) };
                                    SlkR10[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                          ToolBox.ConstraintName("SlkR10", AuxIndexes));
                                    ExcR10[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                          ToolBox.ConstraintName("ExcR10", AuxIndexes));
                                }
                            }
                        }
                    }
                }
            }
            if (UseConstraints[11] && SlackExcess[11])
            {
                for (int i = 0; i < Instance.NumFMUs; i++)
                {
                    for (int j = 0; j < Instance.NumFMUs; j++)
                    {
                        for (int p = 0; p < Instance.NumPeriodsHP; p++)
                        {
                            for (int b = 0; b < Instance.MaxNumBlocks; b++)
                            {
                                for (int h = 0; h < Instance.MaxNumBlocks; h++)
                                {
                                    AuxIndexes = new List<(string, int)> { ("fmu", i), ("fmu", j), ("period", p), ("block", b), ("block", h) };
                                    if (OldVersion)
                                    {
                                        SlkR11A[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                               ToolBox.ConstraintName("SlkR11A", AuxIndexes));
                                        ExcR11A[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                               ToolBox.ConstraintName("ExcR11A", AuxIndexes));
                                    }
                                    else
                                    {
                                        SlkR11B[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                                    ToolBox.ConstraintName("SlkR11B", AuxIndexes));
                                        ExcR11B[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                                    ToolBox.ConstraintName("ExcR11B", AuxIndexes));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (UseConstraints[12] && SlackExcess[12])
            {
                for (int p = 0; p < Instance.NumPeriodsHP; p++)
                {
                    SlkR12[p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                            ToolBox.ConstraintName("SlkR12", ("period", p)));
                    ExcR12[p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                 ToolBox.ConstraintName("ExcR12", ("period", p)));
                }
            }
            if (UseConstraints[13] && SlackExcess[13])
            {
                for (int s = 0; s < Instance.NumSortiments; s++)
                {
                    for (int p = 0; p < Instance.NumPeriodsHP; p++)
                    {
                        for (int b = 0; b < Instance.MaxNumBlocks; b++)
                        {
                            AuxIndexes = new List<(string, int)> { ("product", s), ("period", p), ("block", b) };
                            SlkR13[s, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                            ToolBox.ConstraintName("SlkR13", AuxIndexes));
                            ExcR13[s, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                            ToolBox.ConstraintName("ExcR13", AuxIndexes));
                        }
                    }
                }
            }
            if (UseConstraints[14] && SlackExcess[14])
            {
                for (int i = 0; i < Instance.NumFMUs; i++)
                {
                    for (int p = 0; p < Instance.NumPeriodsHP; p++)
                    {
                        for (int b = 0; b < Instance.MaxNumBlocks; b++)
                        {
                            AuxIndexes = new List<(string, int)> { ("fmu", i), ("period", p), ("block", b) };
                            SlkR14[i, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                            ToolBox.ConstraintName("SlkR14", AuxIndexes));
                            ExcR14[i, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                            ToolBox.ConstraintName("ExcR14", AuxIndexes));
                        }
                    }
                }
            }
            if (UseConstraints[15] && SlackExcess[15])
            {
                for (int i = 0; i < Instance.NumFMUs; i++)
                {
                    SlkR15[i] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                              ToolBox.ConstraintName("SlkR15", ("fmu", i)));
                    ExcR15[i] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                              ToolBox.ConstraintName("ExcR15", ("fmu", i)));
                }
            }
            if (UseConstraints[17] && SlackExcess[17])
            {
                for (int i = 0; i < Instance.NumFMUs; i++)
                {
                    SlkR17[i] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                ToolBox.ConstraintName("SlkR17", ("fmu", i)));
                    ExcR17[i] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                ToolBox.ConstraintName("ExcR17", ("fmu", i)));
                }
            }
            if (UseConstraints[18] && SlackExcess[18])
            {
                for (int p = 0; p < Instance.NumPeriodsHP; p++)
                {
                    for (int b = 0; b < Instance.MaxNumBlocks; b++)
                    {
                        AuxIndexes = new List<(string, int)> { ("period", p), ("block", b) };
                        SlkR18[p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                       ToolBox.ConstraintName("SlkR18", AuxIndexes));
                        ExcR18[p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                        ToolBox.ConstraintName("ExcR18", AuxIndexes));
                    }
                }
            }
            if (UseConstraints[19] && SlackExcess[19])
            {
                for (int i = 0; i < Instance.NumFMUs; i++)
                {
                    for (int p = 0; p < Instance.NumPeriodsHP; p++)
                    {
                        for (int b = 0; b < Instance.MaxNumBlocks; b++)
                        {
                            AuxIndexes = new List<(string, int)> { ("fmu", i), ("period", p), ("block", b) };
                            SlkR19[i, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                            ToolBox.ConstraintName("SlkR19", AuxIndexes));
                            ExcR19[i, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                            ToolBox.ConstraintName("ExcR19", AuxIndexes));
                        }
                    }
                }
            }
            if (UseConstraints[20] && SlackExcess[20])
            {
                for (int i = 0; i < Instance.NumFMUs; i++)
                {
                    for (int j = 0; j < Instance.NumFMUs; j++)
                    {
                        for (int b = 0; b < Instance.MaxNumBlocks; b++)
                        {
                            for (int p = 0; p < Instance.NumPeriodsHP; p++)
                            {
                                AuxIndexes = new List<(string, int)> { ("fmu", i), ("fmu", j), ("period", p), ("block", b) };
                                SlkR20[i, j, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                   ToolBox.ConstraintName("SlkR20", AuxIndexes));
                                ExcR20[i, j, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS,
                                                                   ToolBox.ConstraintName("ExcR20", AuxIndexes));
                            }
                        }
                    }
                }
            }

            //GRBLinExpr expr1 = new GRBLinExpr();
            List<(string, int)> IndexConstName = new List<(string, int)>();

            //Constraint 4.2
            CounterConstraint[2] = 0;
            if (UseConstraints[2])
            {
                foreach (Period P in Instance.AllPeriods)
                {
                    int p = P.IndexPeriod;
                    for (int s = 0; s < Instance.NumSortiments; s++)
                    {
                        IndexConstName.Clear();
                        IndexConstName.Add(("period", p));
                        IndexConstName.Add(("sortim", s));
                        ConstName = ToolBox.ConstraintName("R02", IndexConstName);
                        double bR2 = (1 - Instance.VolumeVariation) * Instance.SortimentDemands[s, p];
                        Modelo.AddConstr(
                            GrupoExpr02_03(p, s, X, SlkR02, ExcR02, CoefSlackExcess[2], bR2) >= RHS[2],
                            ConstName
                            );
                        CounterConstraint[2]++;
                        CountName.Append(ConstName + '\n');
                    }
                }
            }
            //Constraint 4.3
            CounterConstraint[3] = 0;
            if (UseConstraints[3]) {
                foreach (Period P in Instance.AllPeriods)
                {
                    int p = P.IndexPeriod;
                    for (int s = 0; s < Instance.NumSortiments; s++)
                    {
                        IndexConstName.Clear();
                        IndexConstName.Add(("period", p));
                        IndexConstName.Add(("sortim", s));
                        ConstName = ToolBox.ConstraintName("R03", IndexConstName);
                        double bR3 = (1 + Instance.VolumeVariation) * Instance.SortimentDemands[s, p];
                        Modelo.AddConstr(
                            GrupoExpr02_03(p, s, X, SlkR03, ExcR03, CoefSlackExcess[3], bR3) <= RHS[3],
                            ConstName
                            );
                        CounterConstraint[3]++;
                        CountName.Append(ConstName + '\n');
                    }
                }
            }
            //Constraint 4.4
            CounterConstraint[4] = 0;
            if (UseConstraints[4]) {
                if (OldVersion)
                {
                    foreach (FMU Unit in Instance.AllFMUs)
                    {
                        int i = Unit.IndexAllFMU;
                        ConstName = ToolBox.ConstraintName("R04", ("fmu", i));
                        Modelo.AddConstr(
                            GrupoExpr04(i, X, SlkR04, ExcR04, CoefSlackExcess[4], 1) <= RHS[4],
                            ConstName
                            );
                        CountName.Append(ConstName + '\n');
                        CounterConstraint[4]++;
                    }
                }
                else
                {
                    foreach (FMU Unit in Instance.AllFMUs)
                    {
                        int i = Unit.IndexAllFMU;
                        ConstName = ToolBox.ConstraintName("R04", ("fmu", i));
                        Modelo.AddConstr(
                            GrupoExpr04(i, X, SlkR04, ExcR04, CoefSlackExcess[4], 1) == RHS[4],
                            ConstName
                            );
                        CountName.Append(ConstName + '\n');
                        CounterConstraint[4]++;
                    }
                }
            }
            //Constraint 4.5
            CounterConstraint[5] = 0;
            if (UseConstraints[5])
            {
                foreach (Period P in Instance.AllPeriods)
                {
                    int p = P.IndexPeriod;
                    for (int b = 0; b < Instance.MaxNumBlocks; b++)
                    {
                        IndexConstName.Clear();
                        IndexConstName.Add(("period", p));
                        IndexConstName.Add(("block", b));
                        ConstName = ToolBox.ConstraintName("R05", IndexConstName);
                        Modelo.AddConstr(
                            GrupoExpr05_06(p, b, Alpha, X, SlkR05, ExcR05, CoefSlackExcess[5], Instance.MaximalHarvestArea) <= RHS[5],
                            ConstName
                            );
                        CountName.Append(ConstName + '\n');
                        CounterConstraint[5]++;
                    }
                }
            }
            //Constraint 4.6
            CounterConstraint[6] = 0;
            if (UseConstraints[6])
            {
                foreach (Period P in Instance.AllPeriods)
                {
                    int p = P.IndexPeriod;
                    for (int b = 0; b < Instance.MaxNumBlocks; b++)
                    {
                        IndexConstName.Clear();
                        IndexConstName.Add(("period", p));
                        IndexConstName.Add(("block", b));
                        ConstName = ToolBox.ConstraintName("R06", IndexConstName);
                        Modelo.AddConstr(
                            GrupoExpr05_06(p, b, Alpha, X, SlkR05, ExcR05, CoefSlackExcess[6], Instance.MinimalHarvestArea) >= RHS[6],
                            ConstName
                            );
                        CountName.Append(ConstName + '\n');
                        CounterConstraint[6]++;
                    }
                }
            }
            //Constraint 4.7
            CounterConstraint[7] = 0;
            if (UseConstraints[7])
            {
                if (OldVersion)
                {
                    foreach (FMU Unit in Instance.AllFMUs)
                    {
                        int i = Unit.IndexAllFMU;
                        for (int j = i + 1; j < Instance.AllFMUs.Count(); j++)
                        {
                            foreach (Period P in Instance.AllPeriods)
                            {
                                int p = P.IndexPeriod;
                                for (int b = 0; b < Instance.MaxNumBlocks; b++)
                                {
                                    IndexConstName.Clear();
                                    IndexConstName.Add(("fmu", i));
                                    IndexConstName.Add(("fmu", j));
                                    IndexConstName.Add(("period", p));
                                    IndexConstName.Add(("block", b));
                                    ConstName = ToolBox.ConstraintName("R07a", IndexConstName);
                                    Modelo.AddConstr(
                                            GrupoExpr07(new int[] { i, j }, p, b, X, SlkR07A, ExcR07A, CoefSlackExcess[7], Instance.MaxDistInBlock) <= RHS[7],
                                            ConstName);
                                    CountName.Append(ConstName + '\n');
                                    CounterConstraint[7]++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (FMU Unit in Instance.AllFMUs)
                    {
                        int i = Unit.IndexAllFMU;
                        foreach (Period P in Instance.AllPeriods)
                        {
                            int p = P.IndexPeriod;
                            //FMUs mais proximos de Delta Bloco
                            foreach (int j in Unit.FMUsProxInBlock)
                            {
                                for (int b = 0; b < Instance.MaxNumBlocks; b++)
                                {
                                    int[] indsFMUS = { i, j };
                                    IndexConstName.Clear();
                                    IndexConstName.Add(("fmu", i));
                                    IndexConstName.Add(("fmu", j));
                                    IndexConstName.Add(("period", p));
                                    IndexConstName.Add(("block", b));
                                    ConstName = ToolBox.ConstraintName("R07b", IndexConstName);
                                    Modelo.AddConstr(
                                        GrupoExpr07(indsFMUS, p, b, X, SlkR07B, ExcR07B, CoefSlackExcess[7], 0) <= RHS[7],
                                        ConstName);
                                    CountName.Append(ConstName + '\n');
                                    CounterConstraint[7]++;
                                }
                            }
                        }
                    }
                }
            }
            //Constraint 4.8
            CounterConstraint[8] = 0;
            if (UseConstraints[8]) {
                if (OldVersion)
                {//   Versão Anterior
                    foreach (FMU Unit in Instance.AllFMUs)
                    {
                        int i = Unit.IndexAllFMU;
                        for (int j = i + 1; j < Instance.AllFMUs.Count(); j++)
                        {
                            foreach (Period P in Instance.AllPeriods)
                            {
                                int p = P.IndexPeriod;
                                for (int b = 0; b < Instance.MaxNumBlocks; b++)
                                {
                                    //int b = B.IndexAllBlock;
                                    for (int h = 0; h < Instance.MaxNumBlocks; h++)
                                    {
                                        if (h != b)
                                        {
                                            int[] indsFMUs = { i, j };
                                            int[] indsBlocks = { b, h };
                                            IndexConstName.Clear();
                                            IndexConstName.Add(("fmu", i));
                                            IndexConstName.Add(("fmu", j));
                                            IndexConstName.Add(("period", p));
                                            IndexConstName.Add(("block", b));
                                            IndexConstName.Add(("block", h));
                                            ConstName = ToolBox.ConstraintName("R08a", IndexConstName);
                                            Modelo.AddConstr(
                                                    GrupoExpr08(indsFMUs, p, indsBlocks, X, SlkR08A, ExcR08A, CoefSlackExcess[8], Instance.MaxDistOutBlockInPeriod) <= RHS[8],
                                                    ConstName);
                                            CountName.Append(ConstName + '\n');
                                            CounterConstraint[8]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {// Versão Nova 4.8
                    foreach (FMU Unit in Instance.AllFMUs)
                    {
                        int i = Unit.IndexAllFMU;
                        foreach (Period P in Instance.AllPeriods)
                        {
                            int p = P.IndexPeriod;
                            //FMUs OutroBloco no Período
                            foreach (int j in Unit.FMUsProxOutBlockInPeriod)
                            {
                                for (int b = 0; b < Instance.MaxNumBlocks; b++)
                                {
                                    for (int h = b + 1; h < Instance.MaxNumBlocks; h++)
                                    {
                                        int[] indsFMUs = { i, j };
                                        int[] indsBlocks = { b, h };
                                        IndexConstName.Clear();
                                        IndexConstName.Add(("fmu", i));
                                        IndexConstName.Add(("fmu", j));
                                        IndexConstName.Add(("period", p));
                                        IndexConstName.Add(("block", b));
                                        IndexConstName.Add(("block", h));
                                        ConstName = ToolBox.ConstraintName("R08b", IndexConstName);
                                        Modelo.AddConstr(
                                            GrupoExpr08(indsFMUs, p, indsBlocks, X, SlkR08B, ExcR08B, CoefSlackExcess[8], 0) <= RHS[8],
                                            ConstName);
                                        CountName.Append(ConstName + '\n');
                                        CounterConstraint[8]++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Constraint 4.9
            CounterConstraint[9] = 0;
            if (UseConstraints[9]) {
                foreach (Tuple<int, int> Par in Instance.AdjacentFMUs)
                {
                    int i = Par.Item1;
                    int j = Par.Item2;
                    foreach (Period P in Instance.AllPeriods)
                    {
                        int p = P.IndexPeriod;
                        for (int b = 0; b < Instance.MaxNumBlocks; b++)
                        {
                            for (int h = b + 1; h < Instance.MaxNumBlocks; h++)
                            {
                                int[] indsBlocks = { b, h };
                                IndexConstName.Clear();
                                IndexConstName.Add(("fmu", i));
                                IndexConstName.Add(("fmu", j));
                                IndexConstName.Add(("period", p));
                                IndexConstName.Add(("block", b));
                                IndexConstName.Add(("block", h));
                                ConstName = ToolBox.ConstraintName("R09", IndexConstName);
                                Modelo.AddConstr(
                                    GrupoExpr09(Par, p, indsBlocks, X, SlkR09, ExcR09, CoefSlackExcess[9], 1) <= RHS[9],
                                    ConstName);
                                CountName.Append(ConstName + '\n');
                                CounterConstraint[9]++;
                            }
                        }
                    }
                }
            }
            //Constraint 4.10
            CounterConstraint[10] = 0;
            if (UseConstraints[10]) {
                foreach (Tuple<int, int> Par in Instance.AdjacentFMUs)
                {
                    int i = Par.Item1;
                    int j = Par.Item2;
                    foreach (Period P in Instance.AllPeriods)
                    {
                        if (P != Instance.AllPeriods.Last())
                        {
                            int p = P.IndexPeriod;
                            for (int b = 0; b < Instance.MaxNumBlocks; b++)
                            {
                                for (int h = b + 1; h < Instance.MaxNumBlocks; h++)
                                {
                                    int[] indsBlocks = { b, h };
                                    IndexConstName.Clear();
                                    IndexConstName.Add(("fmu", i));
                                    IndexConstName.Add(("fmu", j));
                                    IndexConstName.Add(("period", p));
                                    IndexConstName.Add(("block", b));
                                    IndexConstName.Add(("block", h));
                                    ConstName = ToolBox.ConstraintName("R10", IndexConstName);

                                    Modelo.AddConstr(
                                        GrupoExpr10(Par, p, indsBlocks, X, SlkR10, ExcR10, CoefSlackExcess[10], 1) <= RHS[10],
                                        ConstName);
                                    CountName.Append(ConstName + '\n');
                                    CounterConstraint[10]++;
                                }
                            }
                        }
                    }
                }
            }
            //Constraint 4.11
            CounterConstraint[11] = 0;
            if (UseConstraints[11])
            {
                if (OldVersion)
                {// Modo antigo
                    foreach (FMU Unit1 in Instance.AllFMUs)
                    {
                        int i = Unit1.IndexAllFMU;
                        foreach (FMU Unit2 in Instance.AllFMUs)
                        {
                            int j = Unit2.IndexAllFMU;
                            foreach (Period P in Instance.AllPeriods)
                            {
                                int p = P.IndexPeriod;
                                if (P != Instance.AllPeriods.Last())
                                {
                                    for (int b = 0; b < Instance.MaxNumBlocks; b++)
                                    {
                                        for (int h = 0; h < Instance.MaxNumBlocks; h++)
                                        {
                                            Tuple<int, int> indsFMU = new Tuple<int, int>(i, j);
                                            int[] indsBlocks = { b, h };

                                            IndexConstName.Clear();
                                            IndexConstName.Add(("fmu", i));
                                            IndexConstName.Add(("fmu", j));
                                            IndexConstName.Add(("period", p));
                                            IndexConstName.Add(("block", b));
                                            IndexConstName.Add(("block", h));

                                            ConstName = ToolBox.ConstraintName("R11a", IndexConstName);
                                            Modelo.AddConstr(
                                                GrupoExpr11A(indsFMU, p, indsBlocks, X, SlkR11A, ExcR11A, CoefSlackExcess[11], 0) <= RHS[11],
                                                ConstName);
                                            CountName.Append(ConstName + '\n');
                                            CounterConstraint[11]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {//Modelo Novo
                    foreach (FMU Unit in Instance.AllFMUs)
                    {
                        int i = Unit.IndexAllFMU;
                        foreach (Period P in Instance.AllPeriods)
                        {
                            if (P != Instance.AllPeriods.Last())
                            {
                                int p = P.IndexPeriod;
                                //FMUs mais proximos de Delta Bloco
                                foreach (int j in Unit.FMUsProxConsecutivePeriod)
                                {
                                    for (int b = 0; b < Instance.MaxNumBlocks; b++)
                                    {
                                        for (int h = b + 1; h < Instance.MaxNumBlocks; h++)
                                        {
                                            Tuple<int, int> indsFMU = new Tuple<int, int>(i, j);
                                            int[] indsBlocks = { b, h };

                                            IndexConstName.Clear();
                                            IndexConstName.Add(("fmu", i));
                                            IndexConstName.Add(("fmu", j));
                                            IndexConstName.Add(("period", p));
                                            IndexConstName.Add(("block", b));
                                            IndexConstName.Add(("block", h));

                                            ConstName = ToolBox.ConstraintName("R11b", IndexConstName);

                                            Modelo.AddConstr(
                                                GrupoExpr11B(indsFMU, p, indsBlocks, X, SlkR11B, ExcR11B, CoefSlackExcess[11], 1) <= RHS[11],
                                                ConstName);
                                            CountName.Append(ConstName + '\n');
                                            CounterConstraint[11]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Constraint 4.12
            CounterConstraint[12] = 0;
            if (UseConstraints[12])
            {
                foreach (Period P in Instance.AllPeriods)
                {
                    int p = P.IndexPeriod;
                    ConstName = ToolBox.ConstraintName("R12", ("period", p));

                    Modelo.AddConstr(
                        GrupoExpr12(p, Alpha, SlkR12, ExcR12, CoefSlackExcess[12], 1) == RHS[12],
                        ConstName);

                    CountName.Append(ConstName + '\n');
                    CounterConstraint[12]++;
                }
            }
            //Constraint 4.13
            CounterConstraint[13] = 0;
            if (UseConstraints[13])
            {
                for (int s = 0; s < Instance.NumSortiments; s++)
                {
                    foreach (Period P in Instance.AllPeriods)
                    {
                        int p = P.IndexPeriod;
                        for (int b = 1; b < Instance.MaxNumBlocks; b++)
                        {
                            IndexConstName.Clear();
                            IndexConstName.Add(("Sortiment", s));
                            IndexConstName.Add(("period", p));
                            IndexConstName.Add(("block", b));
                            ConstName = ToolBox.ConstraintName("R13", IndexConstName);
                            Modelo.AddConstr(
                                GrupoExpr13(s, p, b, Alpha, X, SlkR13, ExcR13, CoefSlackExcess[13]) <= RHS[13],
                                ConstName);
                            CountName.Append(ConstName + '\n');
                            CounterConstraint[13]++;
                        }
                    }
                }
            }
            //Constraint 4.14
            CounterConstraint[14] = 0;
            if (UseConstraints[14])
            {
                foreach (FMU Unit in Instance.AllFMUs)
                {
                    int i = Unit.IndexAllFMU;
                    foreach (Period P in Instance.AllPeriods)
                    {
                        int p = P.IndexPeriod;
                        for (int b = 0; b < Instance.MaxNumBlocks; b++)
                        {
                            IndexConstName.Clear();
                            IndexConstName.Add(("fmu", i));
                            IndexConstName.Add(("period", p));
                            IndexConstName.Add(("block", b));
                            ConstName = ToolBox.ConstraintName("R14", IndexConstName);
                            Modelo.AddConstr(
                                GrupoExpr14(Unit, p, b, X, SlkR14, ExcR14, CoefSlackExcess[14]) >= RHS[14],
                                ConstName);
                            CountName.Append(ConstName + '\n');
                            CounterConstraint[14]++;
                        }
                    }
                }
            }
            //Constraint 4.15
            CounterConstraint[15] = 0;
            if (UseConstraints[15])
            {
                foreach (FMU Unit in Instance.FMUsForFirstPeriodHarvest)
                {
                    int i = Unit.IndexAllFMU;

                    ConstName = ToolBox.ConstraintName("R15", ("fmu", i));

                    Modelo.AddConstr(
                        GrupoExpr15(i, X, SlkR15, ExcR15, CoefSlackExcess[15], 1) == RHS[15],
                        ConstName);
                    CountName.Append(ConstName + '\n');
                    CounterConstraint[15]++;
                }
            }
            //Constraint 4.17
            CounterConstraint[17] = 0;
            if (UseConstraints[17])
            {
                foreach (FMU Unit in Instance.AllFMUs)
                {
                    int i = Unit.IndexAllFMU;
                    ConstName = ToolBox.ConstraintName("R17", ("fmu", i));
                    Modelo.AddConstr(
                        GrupoExpr17(i, X, SlkR17, ExcR17, CoefSlackExcess[17], 1) == RHS[17],
                        ConstName
                        );
                    CountName.Append(ConstName + '\n');
                    CounterConstraint[17]++;
                }
            }
            //Constraint 4.18
            CounterConstraint[18] = 0;
            if (UseConstraints[18])
            {
                foreach (Period P in Instance.AllPeriods)
                {
                    int p = P.IndexPeriod;
                    for (int b = 0; b < Instance.MaxNumBlocks; b++)
                    {
                        IndexConstName.Clear();
                        IndexConstName.Add(("period", p));
                        IndexConstName.Add(("block", b));
                        ConstName = ToolBox.ConstraintName("R18", IndexConstName);
                        Modelo.AddConstr(
                            GrupoExpr18(p, b, Alpha, X, SlkR18, ExcR18, CoefSlackExcess[18], 0) <= RHS[18],
                            ConstName
                            );
                        CountName.Append(ConstName + '\n');
                        CounterConstraint[18]++;
                    }
                }
            }
            //Constraint 4.19
            CounterConstraint[19] = 0;
            if (UseConstraints[19])
            {
                foreach (FMU Unit in Instance.AllFMUs)
                {
                    int i = Unit.IndexAllFMU;
                    foreach (Period P in Instance.AllPeriods)
                    {
                        int p = P.IndexPeriod;
                        for (int b = 0; b < Instance.MaxNumBlocks; b++)
                        {
                            IndexConstName.Clear();
                            IndexConstName.Add(("fmu", i));
                            IndexConstName.Add(("period", p));
                            IndexConstName.Add(("block", b));
                            ConstName = ToolBox.ConstraintName("R19", IndexConstName);
                            Modelo.AddConstr(
                                GrupoExpr19(Unit, p, b, Alpha, X, SlkR19, ExcR19, CoefSlackExcess[19], 0) <= RHS[19],
                                ConstName);
                            CountName.Append(ConstName + '\n');
                            CounterConstraint[19]++;
                        }
                    }
                }
            }
            //Constraint 4.20
            CounterConstraint[20] = 0;
            if (UseConstraints[20])
            {
                foreach (FMU Unit in Instance.AllFMUs)
                {
                    int i = Unit.IndexAllFMU;
                    for (int j = i + 1; j < Instance.AllFMUs.Count(); j++)
                    {
                        if (Instance.FMUDistances[i, j] > Instance.MaxDistInBlock)
                        {
                            foreach (Period P in Instance.AllPeriods)
                            {
                                int p = P.IndexPeriod;
                                for (int b = 0; b < Instance.MaxNumBlocks; b++)
                                {
                                    IndexConstName.Clear();
                                    IndexConstName.Add(("fmu", i));
                                    IndexConstName.Add(("fmu", j));
                                    IndexConstName.Add(("period", p));
                                    IndexConstName.Add(("block", b));
                                    ConstName = ToolBox.ConstraintName("R20", IndexConstName);
                                    Modelo.AddConstr(
                                            GrupoExpr20(i, j, p, b, X, SlkR20, ExcR20, CoefSlackExcess[20], 1) <= RHS[20],
                                            ConstName);
                                    CountName.Append(ConstName + '\n');
                                    CounterConstraint[20]++;
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < CounterConstraint.Length; i++)
            {
                if (CounterConstraint[i] > 0)
                {
                    CountConstraint.Append("R" + i.ToString() + ":\t " + CounterConstraint[i].ToString() + '\n');
                }
            }

            if (OldVersion) {
                NomeArquivo += "[orig]";
            }
            else
            {
                NomeArquivo += "[adap]";
            }

            StringBuilder gravar = new StringBuilder();
            gravar.Append(CountConstraint.ToString());
            gravar.Append(CountName.ToString());

            //ToolBox.GravaMsgTxt(Pasta, "[all]" + NomeArquivo, Agora, 0, gravar, ShowMess);
            //ToolBox.GravaMsgTxt(Pasta, "[Paramet]" + NomeArquivo, Agora, 0, Instance.Param, ShowMess);
            //ToolBox.GravaMsgTxt(Pasta, "[CountConst]" + NomeArquivo, Agora, 0, CountConstraint, ShowMess);

            CountConstraint.Clear();
            CountName.Clear();
            gravar.Clear();

            GRBModel ModeloRelax = Modelo.Relax();

            List<(GRBModel, bool, string)> Models = new List<(GRBModel, bool, string)> { (Modelo, false, NomeArquivo), (ModeloRelax, true, "(relax)" + NomeArquivo) };

            
            foreach ((GRBModel, bool, string) TM in Models)
            {
                //Chama a Otimização
                OptimizeModel(TM.Item3, Pasta, Agora, TM.Item1, TM.Item2);
            }


            foreach ((GRBModel, bool, string) TM in Models)
            {
                TM.Item1.Dispose();
            }

        }
        public void SolveFromFile(bool ShowMess, string MPSFile, string MSTFile = "")
        {
            string tipo = "FromFile";
            string[] SplitName = MPSFile.Split('\\');
            StringBuilder sbAux = new StringBuilder();
            foreach (string str in SplitName)
            {
                if (SplitName.Last() != str)
                {
                    sbAux.Append(str + "\\");
                }
            }
            string path = sbAux.ToString();

            string[] GetAgora = SplitName.Last().Split(new char[] { '(', ')' });
            sbAux.Clear();
            sbAux.Append(tipo);
            sbAux.Append('(');

            foreach (string ga in GetAgora)
            {
                if ((ga.Contains("_AM")) || (ga.Contains("_PM")))
                {
                    string Agora = ga;
                }
                else if (GetAgora.Last() != ga)
                {
                    sbAux.Append(ga);
                }
            }

            sbAux.Append(')');
            sbAux.Append("solved");

            string problem = sbAux.ToString();
            sbAux.Clear();

            Ambiente = new GRBEnv();

            GRBModel ModFromFile = new GRBModel(Ambiente, MPSFile);
            if (MSTFile.Length > 0)
            {
                ModFromFile.Read(MSTFile);
            }

            OptimizeModel(problem, path, ToolBox.GetNow(), ModFromFile, ShowMess);
        }
        public void OptimizeModel(string NomeArq, string Pasta, string Agora, GRBModel OpModelo, bool ShowMess, bool relax = false)
        {
           
            // Must set LazyConstraints parameter when using lazy constraints

            OpModelo.Parameters.LazyConstraints = 1;

            if (NomeArquivo != NomeArq)
            {
                NomeArquivo = NomeArq;
            }

            List<string> TypeModelsExt = new List<string>{ "PL", "MPS", "HNT", "BAS", "PRM", "ATTR"};
            //Escrita do Modelo
            foreach (string typefile in TypeModelsExt)
            {
                ToolBox.GravaModelo(typefile, Pasta, NomeArquivo, Agora, OpModelo,ShowMess);
            }

            DialogResult result = DialogResult.Yes;
            if (ShowMess)
            {
                result = MessageBox.Show("Calcular?", "Calcular", MessageBoxButtons.YesNo);
            }

            if (result == DialogResult.Yes)
            {
                OpModelo.Optimize();

                resp = ToolBox.AvaliaModelo(OpModelo.Status, ShowMess);
                try
                {
                    ModelObjVal = OpModelo.ObjVal;
                }
                catch (Exception)
                {
                    ModelObjVal = 0;
                    //throw;
                }
                TreatingOptimizeModel(resp, NomeArq, Pasta, Agora, OpModelo, relax);
            }
        }


        public void TreatingOptimizeModel(Tuple<bool, string, string> respfromModel, string NomeArq, string Pasta, string Agora, GRBModel OptModelo, bool ShowMess, bool relax = false)
        {
            //Teste de Erro
            string title;
            if (!(respfromModel.Item1))
            {
                StringBuilder msgErro = new StringBuilder();
                msgErro.Append(respfromModel.Item2);
                if (relax)
                {
                    title = string.Format("(relax-{0}){1}", resp_relax.Item3, NomeArq);
                }
                else
                {
                    title = string.Format("({0}){1}", respfromModel.Item3, NomeArq);
                }
                ToolBox.GravaMsgTxt(Pasta, title, Agora, 0, msgErro, ShowMess);
            }
            else
            {
                if (relax)
                {
                    title = "(relax)_res_" + NomeArq;
                }
                else
                {
                    title = "_res_" + NomeArq;
                }

                List<string> TypeModelsExt = new List<string> { "SOL", "JSON", "HNT", "BAS", "PRM", "ATTR", "MST" };
                //Escrita do Modelo
                foreach (string typefile in TypeModelsExt)
                {
                    ToolBox.GravaModelo(typefile, Pasta, title, Agora, OptModelo, ShowMess);
                }
            }
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                if (Instance != null)
                {
                    try
                    {
                        Instance.Dispose();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
                if (Modelo != null)
                {
                    try
                    {
                        Modelo.Dispose();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                //this.Dispose();
                
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;
        }
    }
}
