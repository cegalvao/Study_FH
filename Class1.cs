using System;
using System.Diagnostics; //Get time execution
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gurobi;
using System.IO;
using System.Drawing; //Allows Draw
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Study_FH
{
    class R_FMU
    {
        public string Name;
        public double CoordX;
        public double CoordY;
        public int InitialAge;
        public double HarvestAge;
        public double Area;
        public bool IsFMU;
        public int Index;
        public bool IsFirstBlock;
        public List<R_SortVol> VolSortimentsInPeriod;

        public R_FMU(string name, int index, int initialAge = 0, double harvestAge = 0,
                     double area = 0, double x = 0, double y = 0, bool isfmu = true)
        {
            Name = name;
            Index = index;
            InitialAge = initialAge;
            HarvestAge = harvestAge;
            Area = area;
            CoordX = x;
            CoordY = y;
            IsFMU = isfmu;
            IsFirstBlock = false;
            VolSortimentsInPeriod = new List<R_SortVol>();
            Console.WriteLine(String.Format("FMU {0} created", name));
        }
        public IEnumerable<int> FMUsProxOutBlockInPeriod { get; internal set; }
        public IEnumerable<int> FMUsProxInBlock { get; internal set; }
        public IEnumerable<int> FMUsProxConsecutivePeriod { get; internal set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FMU Name: \t" + Name);
            sb.AppendLine(" Ind: \t" + Index.ToString());
            sb.AppendLine(" Initial Age: \t" + InitialAge.ToString());
            sb.AppendLine(" Area: \t" + Area.ToString());
            sb.AppendLine(" X: \t" + CoordX.ToString());
            sb.AppendLine(" Y: \t" + CoordY.ToString());
            sb.AppendLine(" IsFMU: \t" + IsFMU.ToString());
            sb.AppendLine(" VolSort: \t" + VolSortsToString());
            return sb.ToString();
        }
        public string VolSortsToString()
        {
            StringBuilder sb = new StringBuilder();
            if (VolSortimentsInPeriod.Count > 0)
            {
                sb.AppendJoin(";", VolSortimentsInPeriod);
            }
            else
            {
                sb.Append("Empty");
            }
            return sb.ToString();
        }
        public void SortimentsAndTravels (int sort, Period period, decimal TruckCapacity, decimal volume)
        {
            R_SortVol sv = new  R_SortVol(sort, period, TruckCapacity, volume, this);
            ToolBox.AddL(VolSortimentsInPeriod, sv);
        }
        public double GetSortimentVol (int sort, Period period, string parameter) {
            IEnumerable<R_SortVol> pickSV = from sv in VolSortimentsInPeriod
                         where (sv.sortiment == sort) && (sv.period == period)
                         select sv;
            if (pickSV.Any())
            {
                return parameter switch
                {
                    "volume" => pickSV.First().volume,
                    "trucks" => pickSV.First().trucks,
                    "lpv" => pickSV.First().lpv,
                    _ => 0,
                };
            }
            else
            {
                return 0;
            }
        }
        public void UpdateLPV(int sort, Period period, double newlpv)
        {
            IEnumerable<R_SortVol> pickSV = from sv in VolSortimentsInPeriod
                                            where (sv.sortiment == sort) && (sv.period == period)
                                            select sv;
            if (pickSV.Any())
            {
                pickSV.First().SetLPV(newlpv);
            }
        }
        public void UpdateLPV(int sort, Period period, int newlpv)
        {
            IEnumerable<R_SortVol> pickSV = from sv in VolSortimentsInPeriod
                                            where (sv.sortiment == sort) && (sv.period == period)
                                            select sv;
            if (pickSV.Any())
            {
                pickSV.First().SetLPV(newlpv);
            }
        }
        public void UpdateLPV(int sort, Period period, decimal newlpv)
        {
            IEnumerable<R_SortVol> pickSV = from sv in VolSortimentsInPeriod
                                            where (sv.sortiment == sort) && (sv.period == period)
                                            select sv;
            if (pickSV.Any())
            {
                pickSV.First().SetLPV(newlpv);
            }
        }
        public void UpdateLPV(int sort, Period period, string newlpv)
        {
            IEnumerable<R_SortVol> pickSV = from sv in VolSortimentsInPeriod
                                            where (sv.sortiment == sort) && (sv.period == period)
                                            select sv;
            if (pickSV.Any())
            {
                pickSV.First().SetLPV(newlpv);
            }
        }
    }
    class R_Edge
    {
        public string EdgeName;// { get; set; }
        public int EdgeIndex;// { get; set; }
        public R_FMU Node1;// { get; set; }
        public R_FMU Node2;// { get; set; }
        public double DemandByPeriod;// { get; set; }
        public double Cost;// { get; set; }
        public double Lenght;// { get; set; }
        public bool Required;// { get; set; }
        public R_Edge(R_FMU Start, R_FMU End, double lenght = 0, double EstimatedCost = 0, double Demand = 0, bool req = false)
        {
            Node1 = Start;
            Node2 = End;
            DemandByPeriod = Demand;
            Cost = EstimatedCost;
            EdgeName = $"({Start.Name}, {End.Name})";
            Required = req;
            if (lenght > 0)
            {
                Lenght = lenght;
            } else
            {
                Lenght = R_Edge.CalcularTamanho(Start, End);
            }
            EdgeIndex = 0;
        }
        public override string ToString()
        {
            return "Edge " + EdgeName;
        }
        public static double CalcularTamanho(R_FMU Start, R_FMU End)
        {
            return ToolBox.Distancia2Pontos(Start.CoordX, End.CoordX, Start.CoordY, End.CoordY);
        }
    }
    class R_AdjFmu
    {
        public R_FMU Fmu1;
        public R_FMU Fmu2;

        public R_AdjFmu(R_FMU fmu1, R_FMU fmu2)
        {
            Fmu1 = fmu1;
            Fmu2 = fmu2;
        }

        public override string ToString()
        {
            return String.Format("({0},{1})",Fmu1.Name, Fmu2.Name);
        }

    }
    class R_SortVol
    {
        public int fmuind;
        public int sortiment;
        public Period period;
        public double volume;
        public int trucks;
        public double lpv;

        public R_SortVol(int s, Period p, decimal truckCapacity, decimal vol, R_FMU fmu)
        {
            sortiment = s;
            period = p;
            volume = Convert.ToDouble(vol);
            fmuind = fmu.Index;
            trucks = TrucksByFMU(truckCapacity, vol);
            lpv = 0;
        }
        public R_SortVol(int s, Period p, double truckCapacity, double vol, R_FMU fmu)
        {
            sortiment = s;
            period = p;
            volume = vol;
            fmuind = fmu.Index;
            trucks = TrucksByFMU(truckCapacity, vol);
            lpv = 0;
        }
        public R_SortVol(int s, Period p, int truckCapacity, int vol, R_FMU fmu)
        {
            sortiment = s;
            period = p;
            volume = Convert.ToDouble(vol);
            fmuind = fmu.Index;
            trucks = TrucksByFMU(truckCapacity, vol);
            lpv = 0;
        }
        public R_SortVol(int s, Period p, string truckCapacity, string vol, R_FMU fmu)
        {
            sortiment = s;
            period = p;
            volume = ToolBox.ReadDouble(vol);
            fmuind = fmu.Index;
            trucks = TrucksByFMU(truckCapacity, vol);
            lpv = 0;
        }
        public void SetLPV(double LPV)
        {
            lpv = LPV;
        }
        public void SetLPV(int LPV)
        {
            lpv = Convert.ToDouble(LPV);
        }
        public void SetLPV(decimal LPV)
        {
            lpv = Convert.ToDouble(LPV);
        }
        public void SetLPV(string LPV)
        {
            lpv = ToolBox.ReadDouble(LPV);
        }
        public override string ToString()
        {
            return String.Format("\n\t s:{0},p{1}=>Vol:{2},Trucks:{3},LPV:{4}", sortiment, period.IndexPeriod, volume, trucks, lpv);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as R_SortVol);
        }
        public bool Equals(R_SortVol other)
        {
            return other != null &&
                 sortiment == other.sortiment &&
                 period == other.period &&
                 volume == other.volume &&
                 trucks == other.trucks &&
                 lpv == other.lpv;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(sortiment, period, volume, trucks, "sv");
        }
        public static bool operator ==(R_SortVol left, R_SortVol right)
        {
            return EqualityComparer<R_SortVol>.Default.Equals(left, right);
        }
        public static bool operator !=(R_SortVol left, R_SortVol right)
        {
            return !(left == right);
        }
        public static int TrucksByFMU(decimal TruckCapacity, decimal volume)
        {
            if (volume > 0)
            {
                decimal quoc = (volume / Convert.ToDecimal(TruckCapacity));
                int t = Convert.ToInt32(Math.Floor(quoc));

                if ((t * TruckCapacity) < Convert.ToInt32(volume))
                {
                    t += 1;
                }
                return t;
            }
            else
            {
                return 0;
            }
        }
        public static int TrucksByFMU(double TruckCapacity, double volume)
        {
            return TrucksByFMU(Convert.ToDecimal(TruckCapacity), Convert.ToDecimal(volume));
        }
        public static int TrucksByFMU(int TruckCapacity, int volume)
        {
            return TrucksByFMU(Convert.ToDecimal(TruckCapacity), Convert.ToDecimal(volume));
        }
        public static int TrucksByFMU(string TruckCapacity, string volume)
        {
            return TrucksByFMU(ToolBox.ReadDecimal(TruckCapacity), ToolBox.ReadDecimal(volume));
        }
    }
    class R_Instance
    {
        public string Name;// { get; set; }
        public int NumFMUs;// { get; set; }
        public int NumEdges;//{ get; set; }
        public int NumPeriods;// { get; set; }
        public int MaxNumBlocks;//{ get; set; }
        public List<R_FMU> FMUs;// { get; set; }
        public List<R_Edge> Edges;// { get; set; }
        public List<R_AdjFmu> AdjacentFMUs;// { get; set; }
        public string Comment;// { get; set; }
        public string TypeCost;// { get; set; }
        public double TotalCost;// { get; set; }
        public List<int> Days;// { get; set; }
        //[JsonIgnore]
        public double[,] CostMatrix;// { get; set; }
        //[JsonIgnore]
        public double[,] DemandMatrix;// { get; set; }
        //[JsonIgnore]
        //public double[,,,] DemandQMatrix;// { get; set; }
        //[JsonIgnore]
        //public List<Edge> RequiredEdges;// { get; set; }
        public char[] charSeparators = { '\t', ';', ',' };
        public int NumSortiments = 1;//  { get; private set; }
        public int TruckCapacity = 12;//  { get; private set; }
        public double LoadingUnloadingCost = 3;
        public double DemFactor = 0.15;
        public double VolumeVariation;
        //public string[,,] SortimentVolumes;//  { get; private set; } //volume harvested at FMU i in the period p
        public double[,] SortimentDemands;//  { get; private set; } //Sortiment Demands of s in the period p
        //public int[,,] FMUTravels;//  { get; private set; } //number of travels to load the volume of sort s harvested at FMU i in the period p
        //public double[,,] LiquidPresentValues;
        public double[,] FMUDistances;
        public double[,] FMUPredecers;
        public double DeltaDistance;
        public List<Period> AllPeriods;
        //Parameters
        public double MaxDistConsecutivePeriod;
        public int MaintenanceOpeningPeriods;
        public double NewBlockPercent;// { get; internal set; }
        private bool _disposed;

        public double MaxDistOutBlockInPeriod; //{ get; internal set; }
        public double MinimalHarvestAge; //{ get; internal set; }
        public double[] MaximalHarvestArea;
        public double[] MinimalHarvestArea;
        public double MaxDistInBlock;// { get; internal set; }
        public IEnumerable<R_FMU> FMUsForFirstPeriodHarvest;// { get; internal set; }
                                                            //       public StringBuilder Param;// { get; internal set; }
        public double Cost_RoadOpening_ByUnity;
        public double Cost_RoadMaintenance_ByUnity;
        public double Cost_TravelFix_ByUnity;
        public double Cost_TravelsIJ_ByUnity;
        //public double[,,] Cost_RoadOpening;
        //public double[,,] Cost_RoadMaintenance;
        public double[] Cost_RoadOpening_ByPeriod;
        public double[] Cost_RoadMaintenance_ByPeriod;
        public double[] Cost_TravelFix_ByPeriod;
        //public double[,] Cost_TravelsIJ;


        public R_Instance(string _fileInst = "", string type = "csv", string name = "Kitt4")
        {
            Name = name;
            FMUs = new List<R_FMU>();
            Edges = new List<R_Edge>();
            AdjacentFMUs = new List<R_AdjFmu>();
            AllPeriods = new List<Period>();
            //NumPeriods
            NumPeriods = ReadNumPeriods(@"D:\OneDrive - ufpr.br\Visual Studio 2019\ForestInstances\Kitt4\Kitt4_prf_grafo.csv");

            MinimalHarvestArea = new double[NumPeriods];
            MaximalHarvestArea = new double[NumPeriods];

            switch (type)
            {
                case "csv":
                    //Read points
                    ReadPtsCsv(@"D:\OneDrive - ufpr.br\Visual Studio 2019\ForestInstances\Kitt4\Kitt4_pts_grafo.csv");
                    //Read FMU area and age
                    ReadStdCsv(@"D:\OneDrive - ufpr.br\Visual Studio 2019\ForestInstances\Kitt4\Kitt4_std_grafo.csv");
                    //Read FMU's Adjacency
                    ReadAdjCsv(@"D:\OneDrive - ufpr.br\Visual Studio 2019\ForestInstances\Kitt4\Kitt4_adj_grafo.csv");
                    //Read Edges
                    ReadEdgCsv(@"D:\OneDrive - ufpr.br\Visual Studio 2019\ForestInstances\Kitt4\Kitt4_edg_grafo.csv");
                    //Read Estimated Volume
                    ReadVolCsv(@"D:\OneDrive - ufpr.br\Visual Studio 2019\ForestInstances\Kitt4\Kitt4_vol_grafo.csv");
                    //Read Profits
                    ReadPrfCsv(@"D:\OneDrive - ufpr.br\Visual Studio 2019\ForestInstances\Kitt4\Kitt4_prf_grafo.csv");
                    //Read RoadCosts
                    ReadRodCsv(@"D:\OneDrive - ufpr.br\Visual Studio 2019\ForestInstances\Kitt4\Kitt4_rod_grafo.csv");
                    break;
            }
            SetHarvestAreas(NumPeriods, 0, 100);
            SetSortimentDemand(DemFactor);
            SetDistances();
            SettingCostAndDemandMatrix();
            SetFirstPeriodHarvests();
        }
        private void ReadRodCsv(string _fileInst)
        {
            Cost_TravelFix_ByPeriod = new double[NumPeriods];
            Cost_RoadMaintenance_ByPeriod = new double[NumPeriods];
            Cost_RoadOpening_ByPeriod = new double[NumPeriods];

            string[] AllFile = File.ReadAllLines(_fileInst);
            for (int l = 1; l < AllFile.Length; l++)
            {
                string[] SplitLine = ToolBox.SplitFileLine(AllFile[l], charSeparators);
                switch (SplitLine[0].ToString())
                {
                    case "MaintenanceOpeningPeriods":
                        MaintenanceOpeningPeriods = Convert.ToInt32(ToolBox.ReadDouble(SplitLine[1].ToString()));
                        break;
                    case "Cost_RoadOpening_ByUnity":
                        Cost_RoadOpening_ByUnity = ToolBox.ReadDouble(SplitLine[1].ToString());
                        break;
                    case "Cost_RoadMaintenance_ByUnity":
                        Cost_RoadMaintenance_ByUnity = ToolBox.ReadDouble(SplitLine[1].ToString());
                        break;
                    case "Cost_TravelFix_ByUnity":
                        Cost_TravelFix_ByUnity = ToolBox.ReadDouble(SplitLine[1].ToString());
                        break;
                    case "Cost_TravelsIJ_ByUnity":
                        Cost_TravelsIJ_ByUnity = ToolBox.ReadDouble(SplitLine[1].ToString());
                        break;
                    case "Cost_TravelFix_ByPeriod":
                        for (int p = 0; p < NumPeriods; p++)
                        {
                            if (p + 1 < SplitLine.Length)
                            {
                                Cost_TravelFix_ByPeriod[p] = ToolBox.ReadDouble(SplitLine[p + 1].ToString());
                            }
                            else
                            {
                                Cost_TravelFix_ByPeriod[p] = 0;
                            }
                        }
                        break;
                    case "Cost_RoadMaintenance_ByPeriod":
                        for (int p = 0; p < NumPeriods; p++)
                        {
                            if (p + 1 < SplitLine.Length)
                            {
                                Cost_RoadMaintenance_ByPeriod[p] = ToolBox.ReadDouble(SplitLine[p + 1].ToString());
                            }
                            else
                            {
                                Cost_RoadMaintenance_ByPeriod[p] = 0;
                            }
                        }
                        break;
                    case "Cost_RoadOpening_ByPeriod":
                        for (int p = 0; p < NumPeriods; p++)
                        {
                            if (p + 1 < SplitLine.Length)
                            {
                                Cost_RoadOpening_ByPeriod[p] = ToolBox.ReadDouble(SplitLine[p + 1].ToString());
                            }
                            else
                            {
                                Cost_RoadOpening_ByPeriod[p] = 0;
                            }
                        }
                        break;
                }
            }
        }
        public double CalcCost_TravelsIJ(int i, int j)
        {
            return FMUDistances[i, j] * Cost_TravelsIJ_ByUnity + Cost_TravelFix_ByUnity;
        }
        public double CalcCost_RoadMaintenance(int i, int j, int p)
        {
            return FMUDistances[i, j] * Cost_RoadMaintenance_ByUnity + Cost_RoadMaintenance_ByPeriod[p];
        }
        public double CalcCost_RoadOpening(int i, int j, int p)
        {
            return FMUDistances[i, j] * Cost_RoadOpening_ByUnity * Cost_RoadOpening_ByPeriod[p];
        }
        private void SetHarvestAreas(int periods, double minimal, double maximal)
        {
            for (int p = 0; p < periods; p++)
            {
                MinimalHarvestArea[p] = minimal;
                MaximalHarvestArea[p] = maximal;
            }
        }
        private void SetDistances()
        {
            double TotalDistance = 0;
            double MinDistance = double.MaxValue;
            double MaxDistance = double.MinValue;
            (FMUDistances, FMUPredecers) = FW.DistFW(NumFMUs, Edges);

            for (int i = 0; i < NumFMUs; i++)
            {
                for (int j = 0; j < NumFMUs; j++)
                {
                    if (FMUDistances[i, j] < double.MaxValue)
                    {
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
            }
            DeltaDistance = MaxDistance - MinDistance;
        }
        public R_FMU GetNodeByName(string name)
        {
            var pickN = from nd in FMUs
                        where nd.Name == name
                        select nd;
            if (pickN.Any())
            {
                return pickN.First();
            }
            else
            {
                return null;
            }
        }
        public R_FMU GetNodeByIndex(int Ind)
        {
            var pickN = from nd in FMUs
                        where nd.Index == Ind
                        select nd;
            if (pickN.Any())
            {
                return pickN.First();
            }
            else
            {
                return null;
            }
        }
        private R_Edge GetEdgeByName(string name)
        {
            var pick = from ed in Edges
                       where ed.EdgeName == name
                       select ed;
            if (pick.Any())
            {
                return pick.First();
            }
            else
            {
                return null;
            }

        }
        private Period GetPeriodByIndex(int ind)
        {
            var pick = from per in AllPeriods
                       where per.IndexPeriod == ind
                       select per;
            if (pick.Any())
            {
                return pick.First();
            }
            else
            {
                return null;
            }

        }
        public static bool StrToBoolean(char val)
        {
            return !(val == '0');
        }
        public static bool StrToBoolean(string val)
        {
            return !(val == "0");
        }
        public void ReadPtsCsv(string _fileInst)
        {
            string[] AllFile = File.ReadAllLines(_fileInst);

            for (int i = 1; i < AllFile.Length; i++)
            {
                int ind = FMUs.Count;
                string[] SplitLine = ToolBox.SplitFileLine(AllFile[i], charSeparators);
                string a = SplitLine[0];
                bool isfmu = true;
                if (StrToBoolean(SplitLine[3]))
                {
                    MaxNumBlocks += 1;
                }
                else
                {
                    isfmu = false;
                }
                R_FMU NewPt = new R_FMU(a, ind, 0, 0, 0, ToolBox.ReadDouble(SplitLine[1]),
                                        ToolBox.ReadDouble(SplitLine[2]), isfmu);
                ToolBox.AddL(FMUs, NewPt);
            }
            NumFMUs = FMUs.Count;
        }
        public void ReadEdgCsv(string _fileInst)
        {
            string[] AllFile = File.ReadAllLines(_fileInst);

            for (int i = 1; i < AllFile.Length; i++)
            {
                string[] SplitLine = ToolBox.SplitFileLine(AllFile[i], charSeparators);

                R_Edge NewEdge = new R_Edge(GetNodeByName(Convert.ToString(SplitLine[0])),
                                            GetNodeByName(Convert.ToString(SplitLine[1])),
                                            ToolBox.ReadDouble(SplitLine[2]),
                                            ToolBox.ReadDouble(SplitLine[3]));

                ToolBox.AddL(Edges, NewEdge);
            }
            NumEdges = Edges.Count;
        }
        private void ReadPrfCsv(string _fileInst)
        {
            //File With UNIT_ID,profit_no_harvest,profit p1,profit p2,profit p3,profit p4,profit p5
            string[] AllFile = File.ReadAllLines(_fileInst);

            for (int i = 1; i < AllFile.Length; i++)
            {
                string[] SplitLine = ToolBox.SplitFileLine(AllFile[i], charSeparators);

                R_FMU fmu = GetNodeByName(SplitLine[0]);
                for (int j = 1; j < SplitLine.Length; j++)
                {
                    if (SplitLine[j] != "0")
                    {
                        //Expected First ind, s0no, s0p1, s0p2, s0p3, s1no, s1p1, s1p2, s1p3
                        int sort = Convert.ToInt32((j - 1) / AllPeriods.Count);
                        Period peri = GetPeriodByIndex(j - 1 - sort * AllPeriods.Count);

                        fmu.UpdateLPV(sort, peri, SplitLine[j]);
                    }
                }
            }
        }
        private void ReadVolCsv(string _fileInst)
        {
            //File With UNIT_ID,volume p1,volume p2,volume p3,volume p4,volume p5
            string[] AllFile = File.ReadAllLines(_fileInst);

            for (int s = 0; s < NumSortiments; s++)
            {
                for (int i = 1; i < AllFile.Length; i++)
                {//foreach line at file
                    string[] SplitLine = ToolBox.SplitFileLine(AllFile[i], charSeparators);
                    R_FMU fmu = GetNodeByName(SplitLine[0]);
                    for (int j = 0; j < SplitLine.Length; j++)
                    {
                        if (j == 0)
                        {
                            //SV for non-harvesting period
                            fmu.SortimentsAndTravels(s, GetPeriodByIndex(0), 0, 0);
                        }
                        else
                        {
                            if (SplitLine[j] != "0")
                            {
                                Period J = GetPeriodByIndex(j);
                                fmu.SortimentsAndTravels(s, J, TruckCapacity, ToolBox.ReadDecimal(SplitLine[j]));
                            }
                        }
                    }
                }
            }
        }
        private int ReadNumPeriods(string _fileInst)
        {
            string[] SplitLine = ToolBox.SplitFileLine(File.ReadAllLines(_fileInst).First(), charSeparators);
            //Expected First ind, s1no, s1p1, s1p2, s1p3, s2no, s2p1, s2p2, s2p3
            NumPeriods = Convert.ToInt32(SplitLine.Length - 1 / NumSortiments);

            for (int p = 0; p < NumPeriods; p++)
            {
                string name = SplitLine[p + 1][7..];
                Period NewP = new Period(name, p);
                ToolBox.AddL(AllPeriods, NewP);
            }

            return NumPeriods;
        }
        private void ReadAdjCsv(string _fileInst)
        {
            //File with UNIT_ID,UNIT_adjacent
            string[] AllFile = File.ReadAllLines(_fileInst);
            for (int i = 1; i < AllFile.Length; i++)
            {
                string[] SplitLine = ToolBox.SplitFileLine(AllFile[i], charSeparators);
                R_AdjFmu ad = new R_AdjFmu(GetNodeByName(SplitLine[0]), GetNodeByName(SplitLine[1]));
                ToolBox.AddL(AdjacentFMUs, ad);
            }
        }
        private void ReadStdCsv(string _fileInst)
        {
            //File with UNIT_ID,Unit_area (acres),Unit_age (10 years)
            string[] AllFile = File.ReadAllLines(_fileInst);

            for (int i = 1; i < AllFile.Length; i++)
            {
                string[] SplitLine = ToolBox.SplitFileLine(AllFile[i], charSeparators);
                R_FMU fmu = GetNodeByName(SplitLine[0]);
                //Console.WriteLine("Pré: " + fmu.ToString());
                fmu.Area = ToolBox.ReadDouble(SplitLine[1]);
                fmu.InitialAge = Convert.ToInt32(SplitLine[2]);
                //Console.WriteLine("Pós: " + fmu.ToString());
            }
        }
        public void SettingCostAndDemandMatrix()
        {
            CostMatrix = new double[NumEdges, NumEdges];
            DemandMatrix = new double[NumEdges, NumEdges];
            //DemandQMatrix = new double[NumEdges, NumEdges, AllCombs.Count, NumPeriods];
            foreach (R_Edge E in Edges)
            {
                int E1 = E.Node1.Index;
                int E2 = E.Node2.Index;
                CostMatrix[E1, E2] = E.Cost;
                CostMatrix[E2, E1] = E.Cost;
                DemandMatrix[E1, E2] = E.DemandByPeriod;
                DemandMatrix[E2, E1] = E.DemandByPeriod;
                //    foreach (CombDays CD in E.PossibleCombinations)
                //    {
                //        int CDi = CD.IndexAllComb;
                //        for (int p = 0; p < NumPeriods; p++)
                //        {
                //            //if (CD.ListComb.Contains(Days[p]))
                //            //{
                //            DemandQMatrix[E1, E2, CDi, p] = E.DemandByPeriod;
                //            //}
                //        }
                //    }
            }
        }
        public void SetSortimentDemand(double DemandFactor)
        {
            SortimentDemands = new double[NumSortiments, NumPeriods];

            for (int s = 0; s < NumSortiments; s++)
            {
                foreach (Period P in AllPeriods)
                {
                    int p = P.IndexPeriod;
                    double demand = 0;
                    foreach (R_FMU Unit in FMUs)
                    {
                        demand += Unit.GetSortimentVol(s, P, "volume");
                    }
                    SortimentDemands[s, p] = DemandFactor * demand;
                }
            }
        }
        public void SetFirstPeriodHarvests(string param = "rnd")
        {
            switch (param)
            {
                case "age":
                    int maxage = int.MinValue;
                    foreach (R_FMU fmu in FMUs)
                    {
                        if (fmu.IsFMU)
                        {
                            if (fmu.InitialAge > maxage)
                            {
                                maxage = fmu.InitialAge;
                            }
                        }
                    }

                    foreach (R_FMU fmu in FMUs)
                    {
                        if (fmu.InitialAge > 0.95 * maxage)
                        {
                            fmu.IsFirstBlock = true;
                        }

                    }
                    break;
                case "rnd":
                    Random rnd = new Random(0);
                    List<R_FMU> TrueFMUs = new List<R_FMU>();
                    int numFirstFmus = 2;
                    foreach (R_FMU f in FMUs)
                    {
                        if (f.IsFMU)
                        {
                            ToolBox.AddL<R_FMU>(TrueFMUs, f);
                        }
                    }
                    for(int i =0; i<numFirstFmus; i++)
                    {
                        int ind = rnd.Next(TrueFMUs.Count);
                        TrueFMUs[ind].IsFirstBlock = true;
                        MessageBox.Show(String.Format("{0} Primeiro Bloco", TrueFMUs[ind]));
                    }
                    break;                
            }
        }
        public void WriteInstance(string _destination)
        {
            using FileStream fs = File.Create(_destination);
            StringBuilder title = new StringBuilder();
            _ = title.Append(string.Format("NOMBRE \t{0}\n", Name));
            _ = title.Append(string.Format("COMENTARIO \t{0}\n", Comment));
            _ = title.Append(string.Format("TIPO_COSTES_ARISTAS \t{0}\n", TypeCost));
            _ = title.Append(string.Format("COSTE_TOTAL_REQ \t{0}\n", TotalCost));
            _ = title.Append(string.Format("Vertices\t{0}\n", NumFMUs));
            _ = title.Append("Name \t X \t Y \t Deposit \n");
            //foreach (Node N in Nodes)
            //{
            //    _ = title.Append(string.Format("{0}\t{1}\t{2}\t{3}\n", N.StrName, N.CoordX, N.CoordY, N.Deposit));
            //}
            //_ = title.Append(string.Format("Vehicles\t{0}\n", NumVehicles));
            //_ = title.Append("Name \t Capacity \n");
            //foreach (Vehicle V in Vehicles)
            //{
            //    _ = title.Append(string.Format("{0}\t{1}\n", V.VehicleName, V.VehicleCapacity));
            //}
            _ = title.Append(string.Format("Edges\t{0}\n", NumEdges));
            _ = title.Append("EdgeName\tEdgeIndex\tStartNode\tIsStartNodeDeposit\t");
            _ = title.Append("EndNode\tIsEndNodeDeposit\tRequired\tDemandByPeriod\tCost\t");

            //Just Combs Names
            //    var NameAllCombs = from co in AllCombs
            //                       select co.NameComb;

            //    foreach (string CD in NameAllCombs)
            //    {
            //        _ = title.Append(CD + " \t");
            //    }
            byte[] text = new UTF8Encoding(true).GetBytes(title.ToString() + '\n');
            fs.Write(text, 0, text.Length);
            foreach (R_Edge E in Edges)
            {
                StringBuilder content = new StringBuilder();
                _ = content.Append(string.Format("{0}\t", E.EdgeName));
                _ = content.Append(string.Format("{0}\t", E.EdgeIndex));
                _ = content.Append(string.Format("{0}\t", E.Node1.Name));
                //        _ = content.Append(string.Format("{0}\t", E.Node1.Deposit));
                _ = content.Append(string.Format("{0}\t", E.Node2.Name));
                //        _ = content.Append(string.Format("{0}\t", E.Node2.Deposit));
                _ = content.Append(string.Format("{0}\t", E.Required));
                _ = content.Append(string.Format("{0}\t", E.DemandByPeriod));
                _ = content.Append(string.Format("{0}\t", E.Cost));

                //        var NameECombs = from co in E.PossibleCombinations
                //                         select co.NameComb;

                //        foreach (string CDName in NameAllCombs)
                //        {
                //            if (NameECombs.Contains(CDName))
            
                //            {
                //                _ = content.Append("True\t");
                //            }
                //            else
                //            {
                //                _ = content.Append("False\t");
                //            }
                //        }
                byte[] intext = new UTF8Encoding(true).GetBytes(content.ToString() + '\n');
                fs.Write(intext, 0, intext.Length);
            }
        }
        public Bitmap DrawInstance()
        {
            Bitmap Desenho = new Bitmap(500, 500);
            Graphics g = Graphics.FromImage(Desenho);
            g.FillRectangle(Brushes.White, 0, 0, 500, 500);
            Font drawFont1 = new Font("Arial", 7); //Escrita de texto no Desenho
            Font drawFont2 = new Font("Arial", 10); //Escrita de texto no Desenho
            SolidBrush drawBrush = new SolidBrush(Color.Black); //Pincel de Desenho
            Pen drawPen = new Pen(drawBrush); //Caneta para desenhar linha

            StringFormat format1 = new StringFormat(StringFormatFlags.NoClip)
            {
                Alignment = StringAlignment.Center //Uso o formato para alinhar txt no centro
            }; //Formato de String

            float PointRadius = 5; //Tamanho dos Vertices

            //Desenho dos Pontos
            foreach (R_FMU N in FMUs)
            {
                float X = Convert.ToSingle(N.CoordX) - PointRadius;
                float Y = Convert.ToSingle(N.CoordY) - PointRadius;
                PointF Top = new PointF(X, Y);
                SizeF Tam = new SizeF(2 * PointRadius, 2 * PointRadius);
                RectangleF Rec = new RectangleF(Top, Tam);
                g.DrawEllipse(drawPen, Rec);

                PointF Cnt = new PointF(X + PointRadius * 13 / 10, Y + PointRadius * 13 / 10);
                g.DrawString(N.Name, drawFont2, drawBrush, Cnt, format1);
            }

            //Desenho das Arestas
            foreach (R_Edge E in Edges)
            {
                float X1 = Convert.ToSingle(E.Node1.CoordX);
                float Y1 = Convert.ToSingle(E.Node1.CoordY);
                float X2 = Convert.ToSingle(E.Node2.CoordX);
                float Y2 = Convert.ToSingle(E.Node2.CoordY);

                PointF P1 = new PointF(X1, Y1);
                PointF P2 = new PointF(X2, Y2);
                PointF PMed = new PointF((X1 + X2) * 1 / 2, (Y1 + Y2) * 1 / 2);

                g.DrawLine(drawPen, P1, P2);
                g.DrawString(E.Cost.ToString(), drawFont1, drawBrush, PMed, format1);
            }

            return Desenho;
        }
        public IEnumerable<R_Edge> PickEdgeList(R_FMU N1, R_FMU N2)
        {
            foreach (var pe in Edges)
            {
                if (pe.Node1 == N1 && pe.Node2 == N2)
                {
                    yield return pe;
                }
            }
        }
        public bool CheckIsEdge(R_FMU N1, R_FMU N2)
        {//True if Edge (N1,N2) in Edges. False otherwise
            return PickEdgeList(N1, N2).ToList().Count > 0;
        }
        public bool CheckIsDisorientedEdge(R_FMU N1, R_FMU N2)
        {//True if Edge (N1,N2) in Edges. False otherwise
            return this.CheckIsEdge(N1, N2) || this.CheckIsEdge(N2, N1);
        }
        public int IndexEdge(R_FMU N1, R_FMU N2)
        {
            List<R_Edge> pick_edge = PickEdgeList(N1, N2).ToList();
            if (pick_edge.Count == 1)
            {
                return pick_edge.First().EdgeIndex;
            }
            else
            {
                return -1;
            }
        }
        public bool CheckIsEdgeRequired(R_FMU N1, R_FMU N2)
        {//True if Edge (N1,N2) in Edges and is required. False otherwise
            bool resp = false;
            List<R_Edge> pick_edge = PickEdgeList(N1, N2).ToList();
            int len = pick_edge.Count;
            //bool ver = len > 0;
            if (len > 0)
            {
                resp = pick_edge.First().Required;
            }
            return resp;
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
    class ModelRotaFH
    {
        private bool _disposed = false;
        //Variaveis de geracao do modelo Gurobi
        public GRBEnv Ambiente; //{ get; set; }
        public GRBModel Modelo; //{ get; set; }
        public string NomeArquivo = String.Empty;
        public char VarType = GRB.CONTINUOUS;
        public int NumConstrVRP = 6;

        //Variável de contagem do tempo de execução
        public Stopwatch Cronometro; //{ get; set; }

        //Resultado do Modelo
        public Tuple<bool, string, string> resp = new Tuple<bool, string, string>(false, "Modelo Novo não resolvido", "New");
        public double ModelObjVal;
        public Tuple<bool, string, string> resp_relax = new Tuple<bool, string, string>(false, "Modelo Relaxado Novo não resolvido", "New Relax");
        public double ModelObjVal_relax;

        //Parametros do Problema;
        public R_Instance RotaInstance = new R_Instance();

        //GRBVar[,,] Slk_M_Maintenance = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        //GRBVar[,,] Exc_M_Maintenance = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        // z_{ijp} & \leq \sum_{p'\in P_p} w_{ijp'} & \forall i, j \in F, \forall p > 2 \in P_p

        public GRBLinExpr R_M_Maintenance(int fmu_i, int fmu_j, int period_p, GRBVar[,,] W, GRBVar[,,] Z,
                                         GRBVar[,,] Slk, GRBVar[,,] Exc,
                                         int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, Z[fmu_i, fmu_j, period_p]);

            int FirstPer = 0;
            if (period_p > RotaInstance.MaintenanceOpeningPeriods)
            {
                FirstPer = period_p - RotaInstance.MaintenanceOpeningPeriods;
            }

            for (int p = FirstPer; p < period_p; p++)
            {
                expr.AddTerm(-1, W[fmu_i, fmu_j, p]);
                expr.AddTerm(-1, Z[fmu_i, fmu_j, p]);
            }

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i, fmu_j, period_p]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, fmu_j, period_p]);
            }

            expr.AddConstant(-term);

            return expr; //Revisado
        }

        public GRBLinExpr R_M_Maintenance(int fmu_i, int fmu_j, int period_p, GRBVar[,,] W, GRBVar[,,] Z, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, Z[fmu_i, fmu_j, period_p]);

            int FirstPer = 0;
            if (period_p > RotaInstance.MaintenanceOpeningPeriods)
            {
                FirstPer = period_p - RotaInstance.MaintenanceOpeningPeriods;
            }

            for (int p = FirstPer; p < period_p; p++)
            {
                expr.AddTerm(-1, W[fmu_i, fmu_j, p]);
                expr.AddTerm(-1, Z[fmu_i, fmu_j, p]);
            }

            expr.AddConstant(-term);

            return expr; //Revisado
        }
        //GRBVar[,] Slk_M_maint_open = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        //GRBVar[,] Exc_M_maint_open = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        //w_{ ijp}+z_{ ijp}& \leq 1 & \forall i, j \in F, \forall p \in P,  \label{eq:M_manut_abert} 

        public static GRBLinExpr R_M_maint_open(int fmu_i, int fmu_j, int period_p, GRBVar[,,] W, GRBVar[,,] Z,
                                         GRBVar[,,] Slk, GRBVar[,,] Exc,
                                         int CoefSlackExcess = 0, double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, W[fmu_i, fmu_j, period_p]);
            expr.AddTerm(1, Z[fmu_i, fmu_j, period_p]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i, fmu_j, period_p]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, fmu_j, period_p]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }

        public static GRBLinExpr R_M_maint_open(int fmu_i, int fmu_j, int period_p, GRBVar[,,] W, GRBVar[,,] Z, double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, W[fmu_i, fmu_j, period_p]);
            expr.AddTerm(1, Z[fmu_i, fmu_j, period_p]);

            expr.AddConstant(-term);

            return expr; //Revisado
        }

        //GRBVar[,,] Slk_M_vehicle = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        //GRBVar[,,] Exc_M_vehicle = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        //y_{ ijp}& \leq w_ { ijp }+z_{ ijp}& \forall i, j \in F, \forall p \in P,  \label{eq:M_veiculo}  

        public static GRBLinExpr R_M_vehicle(int fmu_i, int fmu_j, int period_p,
                                      GRBVar[,,] W, GRBVar[,,] Z, GRBVar[,,] Y,
                                      GRBVar[,,] Slk, GRBVar[,,] Exc,
                                      int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, Y[fmu_i, fmu_j, period_p]);
            expr.AddTerm(-1, W[fmu_i, fmu_j, period_p]);
            expr.AddTerm(-1, Z[fmu_i, fmu_j, period_p]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i, fmu_j, period_p]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, fmu_j, period_p]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }

        public static GRBLinExpr R_M_vehicle(int fmu_i, int fmu_j, int period_p,
                                      GRBVar[,,] W, GRBVar[,,] Z, GRBVar[,,] Y,
                                      double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, Y[fmu_i, fmu_j, period_p]);
            expr.AddTerm(-1, W[fmu_i, fmu_j, period_p]);
            expr.AddTerm(-1, Z[fmu_i, fmu_j, period_p]);

            expr.AddConstant(-term);

            return expr; //Revisado
        }
        //GRBVar[,,] Slk_M_use = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        //GRBVar[,,] Exc_M_use = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        //y_{ ijp}& \leq u_ { ijp }& \forall i, j \in F, \forall p \in P,  \label{eq:M_uso} 

        public static GRBLinExpr R_M_use(int fmu_i, int fmu_j, int period_p,
                                  GRBVar[,,] Y, GRBVar[,,] U,
                                  GRBVar[,,] Slk, GRBVar[,,] Exc,
                                  int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, Y[fmu_i, fmu_j, period_p]);
            expr.AddTerm(-1, U[fmu_i, fmu_j, period_p]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i, fmu_j, period_p]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, fmu_j, period_p]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }
        public static GRBLinExpr R_M_use(int fmu_i, int fmu_j, int period_p,
                                  GRBVar[,,] Y, GRBVar[,,] U, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, Y[fmu_i, fmu_j, period_p]);
            expr.AddTerm(-1, U[fmu_i, fmu_j, period_p]);

            expr.AddConstant(-term);

            return expr; //Revisado
        }

        //GRBVar[,] Slk_M_acess = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        //GRBVar[,] Exc_M_acess = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        //    \sum_{ b\in B}x_{ ipb}& \leq \sum_{ j\in F}y_{ ijp}& \forall i \in F, \forall p \in P,  \label{eq:M_acesso}  %

        public GRBLinExpr R_M_acess(int fmu_i, int period_p,
                                    GRBVar[,,] X, GRBVar[,,] Y,
                                    GRBVar[,] Slk, GRBVar[,] Exc,
                                    int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
            {
                expr.AddTerm(1, X[fmu_i, period_p, b]);
            }

            for (int j = 0; j < RotaInstance.NumFMUs; j++)
            {
                expr.AddTerm(-1, Y[fmu_i, j, period_p]);
            }

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i, period_p]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, period_p]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }
        public GRBLinExpr R_M_acess(int fmu_i, int period_p,
                                    GRBVar[,,] X, GRBVar[,,] Y, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();
            
            for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
            {
                expr.AddTerm(1, X[fmu_i, period_p, b]);
            }
            for (int j = 0; j < RotaInstance.NumFMUs; j++)
            {
                expr.AddTerm(-1, Y[fmu_i, j, period_p]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }

        //GRBVar[,] Slk_M_flow = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        //GRBVar[,] Exc_M_flow = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods];
        //    \sum_{ j\in F}u_{ jip}+ \varepsilon_{ ip}\left(\sum_{ b\in B}x_{ ipb}\right) & = \sum_{ j\in F}u_{ ijp}&
        //    \forall i \in F, \\ \forall p \in P  \label{ eq: M_fluxo}  %

        public GRBLinExpr R_M_flow(int fmu_i, Period period_p,
                                   GRBVar[,,] X, GRBVar[,,] U,
                                   GRBVar[,] Slk, GRBVar[,] Exc,
                                   int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            R_FMU Unit = RotaInstance.GetNodeByIndex(fmu_i);

            for (int j = 0; j < RotaInstance.NumFMUs; j++)
            {
                expr.AddTerm(1, U[j, fmu_i, period_p.IndexPeriod]);
                expr.AddTerm(-1, U[fmu_i, j, period_p.IndexPeriod]);
            }

            for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
            {
                for (int s = 0; s < RotaInstance.NumSortiments; s++)
                {
                    expr.AddTerm(Unit.GetSortimentVol(s, period_p, "trucks"), X[fmu_i, period_p.IndexPeriod, b]);
                }
            }

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i, period_p.IndexPeriod]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, period_p.IndexPeriod]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }


        public GRBLinExpr R_M_flow(int fmu_i, Period period_p,
                                   GRBVar[,,] X, GRBVar[,,] U, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            R_FMU Unit = RotaInstance.GetNodeByIndex(fmu_i);

            for (int j = 0; j < RotaInstance.NumFMUs; j++)
            {
                expr.AddTerm(1, U[j, fmu_i, period_p.IndexPeriod]);
                expr.AddTerm(-1, U[fmu_i, j, period_p.IndexPeriod]);
            }

            for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
            {
                for (int s = 0; s < RotaInstance.NumSortiments; s++)
                {
                    expr.AddTerm(Unit.GetSortimentVol(s, period_p, "trucks"), X[fmu_i, period_p.IndexPeriod, b]);
                }
            }

            expr.AddConstant(-term);

            return expr; //Revisado
        }
        //GRBVar[] Slk_M_AgC_qtharvest = new GRBVar[RotaInstance.NumFMUs];
        //GRBVar[] Exc_M_AgC_qtharvest = new GRBVar[RotaInstance.NumFMUs];
        //    \sum_{ b\in B}\sum_{ p\in P}x_{ ipb}& \leq 1 & \forall i \in F \label{eq:M_AgC_qtcolh} %

        public GRBLinExpr R_M_AgC_qtharvest(int fmu_i, GRBVar[,,] X,
                                            GRBVar[] Slk, GRBVar[] Exc,
                                            int CoefSlackExcess = 0, double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
            {
                for (int p = 0; p < RotaInstance.NumPeriods; p++)
                {
                    expr.AddTerm(1, X[fmu_i, p, b]);
                }
            }

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }

        public GRBLinExpr R_M_AgC_qtharvest(int fmu_i, GRBVar[,,] X, double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
            {
                for (int p = 0; p < RotaInstance.NumPeriods; p++)
                {
                    expr.AddTerm(1, X[fmu_i, p, b]);
                }
            }

            expr.AddConstant(-term);

            return expr; //Revisado
        }
        //GRBVar[] Slk_M_AgC_initialblock = new GRBVar[RotaInstance.NumFMUs];
        //GRBVar[] Exc_M_AgC_initialblock = new GRBVar[RotaInstance.NumFMUs];
        //&x_{ i11} = 1 & \forall i\in F_1 \label{eq:M_AgC_inibloco}

        public static GRBLinExpr R_M_AgC_initialblock(int fmu_i, GRBVar[,,] X,
                                               GRBVar[] Slk, GRBVar[] Exc,
                                               int CoefSlackExcess = 0, double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, X[fmu_i, 0, 0]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }
        public static GRBLinExpr R_M_AgC_initialblock(int fmu_i, GRBVar[,,] X,
                                                double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, X[fmu_i, 0, 0]);

            expr.AddConstant(-term);

            return expr; //Revisado
        }

        //GRBVar[,,] Slk_M_AgC_ageblock = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];
        //GRBVar[,,] Exc_M_AgC_ageblock = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];
        // (n_i + p - 1)x_{ ipb} \geq \eta_i x_ { ipb }
        // \forall i \in F, \forall p \in P, \\ \forall b \in B_p  \label{ eq: M_AgC_idadebloco}%\\[2mm]

        public static GRBLinExpr R_M_AgC_ageblock(int fmu_i, int period_p, int block_b,
                                           double FMU_InitialAge, double FMU_HarvestAge,
                                           GRBVar[,,] X, GRBVar[,,] Slk, GRBVar[,,] Exc,
                                           int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(FMU_InitialAge + period_p - 1 - FMU_HarvestAge, X[fmu_i, period_p, block_b]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i, period_p, block_b]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, period_p, block_b]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }

        public static GRBLinExpr R_M_AgC_ageblock(int fmu_i, int period_p, int block_b,
                                           double FMU_InitialAge, double FMU_HarvestAge,
                                           GRBVar[,,] X, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();
            expr.AddTerm(FMU_InitialAge + period_p - 1 - FMU_HarvestAge, X[fmu_i, period_p, block_b]);
            expr.AddConstant(-term);

            return expr; //Revisado
        }

        //GRBVar[,] Slk_M_AgC_varvolmin = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods];
        //GRBVar[,] Exc_M_AgC_varvolmin = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods];
        //& \sum_{ b\in B}\sum_{ i \in F}\upsilon_{ isp}x_{ ipb} \geq(1 -\sigma)d_{ sp}
        //& \forall s\in S, \forall p\in P \label{eq:M_AgC_varvolmin}%\\

        public GRBLinExpr R_M_AgC_varvolmin(int sort_s, Period period_p,
                                            GRBVar[,,] X, GRBVar[,] Slk, GRBVar[,] Exc,
                                            int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();
            for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
            {
                for (int i = 0; i < RotaInstance.NumFMUs; i++) {
                    R_FMU Unit = RotaInstance.GetNodeByIndex(i);
                    expr.AddTerm(Unit.GetSortimentVol(sort_s, period_p, "volume"), X[i, period_p.IndexPeriod, b]);
                }
            }

            expr.AddConstant(-(1 - RotaInstance.DemFactor) * RotaInstance.DemandMatrix[sort_s, period_p.IndexPeriod]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[sort_s, period_p.IndexPeriod]);
                expr.AddTerm(-CoefSlackExcess, Exc[sort_s, period_p.IndexPeriod]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }

        public GRBLinExpr R_M_AgC_varvolmin(int sort_s, Period period_p,
                                            GRBVar[,,] X, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();
            for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
            {
                for (int i = 0; i < RotaInstance.NumFMUs; i++)
                {
                    R_FMU Unit = RotaInstance.GetNodeByIndex(i);
                    expr.AddTerm(Unit.GetSortimentVol(sort_s, period_p, "volume"), X[i, period_p.IndexPeriod, b]);
                }
            }

            expr.AddConstant(-(1 - RotaInstance.DemFactor) * RotaInstance.DemandMatrix[sort_s, period_p.IndexPeriod]);

            expr.AddConstant(-term);

            return expr; //Revisado
        }

        //GRBVar[,] Slk_M_AgC_varvolmax = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods];
        //GRBVar[,] Exc_M_AgC_varvolmax = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods];
        //& \sum_{ b\in B}\sum_{ i \in F}\upsilon_{ isp}x_{ ipb} \leq(1 +\sigma)d_{ sp}& \forall s\in S, \forall p\in P \label{eq:M_AgC_varvolmax}%\\

        public GRBLinExpr R_M_AgC_varvolmax(int sort_s, Period period_p, GRBVar[,,] X, GRBVar[,] Slk, GRBVar[,] Exc, int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();
            for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
            {
                for (int i = 0; i < RotaInstance.NumFMUs; i++)
                {
                    R_FMU Unit = RotaInstance.GetNodeByIndex(i);
                    expr.AddTerm(Unit.GetSortimentVol(sort_s, period_p, "volume"), X[i, period_p.IndexPeriod, b]);
                }
            }

            expr.AddConstant(-(1 + RotaInstance.DemFactor) * RotaInstance.DemandMatrix[sort_s, period_p.IndexPeriod]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[sort_s, period_p.IndexPeriod]);
                expr.AddTerm(-CoefSlackExcess, Exc[sort_s, period_p.IndexPeriod]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }

        public GRBLinExpr R_M_AgC_varvolmax(int sort_s, Period period_p, GRBVar[,,] X, 
                                            double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();
            for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
            {
                for (int i = 0; i < RotaInstance.NumFMUs; i++)
                {
                    R_FMU Unit = RotaInstance.GetNodeByIndex(i);
                    expr.AddTerm(Unit.GetSortimentVol(sort_s, period_p, "volume"), X[i, period_p.IndexPeriod, b]);
                }
            }
            expr.AddConstant(-(1 + RotaInstance.DemFactor) * RotaInstance.DemandMatrix[sort_s, period_p.IndexPeriod]);
            expr.AddConstant(-term);

            return expr; //Revisado
        }
        //GRBVar[,] Slk_M_AgC_maxarea = new GRBVar[RotaInstance.MaxNumBlocks, RotaInstance.NumPeriods];
        //GRBVar[,] Exc_M_AgC_maxarea = new GRBVar[RotaInstance.MaxNumBlocks, RotaInstance.NumPeriods];
        //    & \sum_{i \in F} a_i x_{ipb} \leq \overline{A_p}\alpha_{pb} & \forall p \in P, \forall b \in B_p \label{eq:M_AgC_maxarea}%\\

        public GRBLinExpr R_M_AgC_maxarea(int period_p, int block_b,
                                          GRBVar[,,] X, GRBVar[,] A,
                                          GRBVar[,] Slk, GRBVar[,] Exc,
                                          int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int i = 0; i < RotaInstance.NumFMUs; i++)
            {
                R_FMU fmu = RotaInstance.GetNodeByIndex(i);
                expr.AddTerm(fmu.Area, X[i, period_p, block_b]);
            }

            expr.AddTerm(-RotaInstance.MaximalHarvestArea[period_p], A[period_p, block_b]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[block_b, period_p]);
                expr.AddTerm(-CoefSlackExcess, Exc[block_b, period_p]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }


        public GRBLinExpr R_M_AgC_maxarea(int period_p, int block_b,
                                          GRBVar[,,] X, GRBVar[,] A,
                                          double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int i = 0; i < RotaInstance.NumFMUs; i++)
            {
                R_FMU fmu = RotaInstance.GetNodeByIndex(i);
                expr.AddTerm(fmu.Area, X[i, period_p, block_b]);
            }

            expr.AddTerm(-RotaInstance.MaximalHarvestArea[period_p], A[period_p, block_b]);

            expr.AddConstant(-term);

            return expr; //Revisado
        }
        //GRBVar[,] Slk_M_AgC_minarea = new GRBVar[RotaInstance.MaxNumBlocks, RotaInstance.NumPeriods];
        //GRBVar[,] Exc_M_AgC_minarea = new GRBVar[RotaInstance.MaxNumBlocks, RotaInstance.NumPeriods];
        //    & \sum_{i\in F} a_i x_{ipb} \geq \underline{A_p}\alpha_{pb} & \forall p \in P, \forall b \in B_p \label{eq:M_AgC_minarea}%\\

        public GRBLinExpr R_M_AgC_minarea(int period_p, int block_b,
                                          GRBVar[,,] X, GRBVar[,] A,
                                          GRBVar[,] Slk, GRBVar[,] Exc,
                                          int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int i = 0; i < RotaInstance.NumFMUs; i++)
            {
                R_FMU fmu = RotaInstance.GetNodeByIndex(i);
                expr.AddTerm(fmu.Area, X[i, period_p, block_b]);
            }

            expr.AddTerm(-RotaInstance.MinimalHarvestArea[period_p], A[period_p, block_b]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[block_b, period_p]);
                expr.AddTerm(-CoefSlackExcess, Exc[block_b, period_p]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }

        public GRBLinExpr R_M_AgC_minarea(int period_p, int block_b,
                                          GRBVar[,,] X, GRBVar[,] A,
                                          double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int i = 0; i < RotaInstance.NumFMUs; i++)
            {
                R_FMU fmu = RotaInstance.GetNodeByIndex(i);
                expr.AddTerm(fmu.Area, X[i, period_p, block_b]);
            }

            expr.AddTerm(-RotaInstance.MinimalHarvestArea[period_p], A[period_p, block_b]);
            expr.AddConstant(-term);

            return expr; //Revisado
        }

        //GRBVar[] Slk_B_AgC_firstblock = new GRBVar[RotaInstance.NumPeriods];
        //GRBVar[] Exc_B_AgC_firstblock = new GRBVar[RotaInstance.NumPeriods];
        //    & \alpha_{p1} = 1 & \forall p \in P \label{eq:B_AgC_primbloco}%\\

        public static GRBLinExpr R_B_AgC_firstblock(int period_p, GRBVar[,] A,
                                             GRBVar[] Slk, GRBVar[] Exc,
                                             int CoefSlackExcess = 0, double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, A[period_p, 0]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[period_p]);
                expr.AddTerm(-CoefSlackExcess, Exc[period_p]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }

        public static GRBLinExpr R_B_AgC_firstblock(int period_p, GRBVar[,] A,
                                             double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, A[period_p, 0]);

            expr.AddConstant(-term);

            return expr; //Revisado
        }
        //GRBVar[,,] Slk_B_AgC_createblock = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];
        //GRBVar[,,] Exc_B_AgC_createblock = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];
        //    \displaystyle \alpha_{pb} \leq \dfrac{1}{\tau}\PG{\dfrac{(1+\sigma)d_{sp} - \sum_{h=1}^{b-1}\sum_{i\in F}\upsilon_{isp}x_{iph} }{(1+\sigma)d_{sp}}} \nonumber
        //    \forall s\in S, \forall p \in P, b = 2,\ldots, |B| \label{eq:B_AgC_criabloco}%\\
        //    \somat{h}{1}{b-1} \sum_{i\in F} \upsilon_{isp}x_{iph} + \tau(1+\sigma)d_{sp} \alpha_{pb} \leq (1+\sigma)d_{sp}
        //    & \forall s\in S, \forall p \in P, b = 2,\ldots, |B|

        public GRBLinExpr R_B_AgC_createblock(int sort_s, Period period_p, int block_b,
                                              GRBVar[,,] X, GRBVar[,] A,
                                              GRBVar[,,] Slk, GRBVar[,,] Exc,
                                              int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int h = 0; h < block_b; h++)
            {
                for (int i = 0; i < RotaInstance.NumFMUs; i++)
                {
                    R_FMU Unit = RotaInstance.GetNodeByIndex(i);
                    expr.AddTerm(Unit.GetSortimentVol(sort_s, period_p, "volume"), X[i, period_p.IndexPeriod, h]);
                }
            }

            expr.AddTerm(RotaInstance.NewBlockPercent * (1 + RotaInstance.DemFactor) * RotaInstance.DemandMatrix[sort_s, period_p.IndexPeriod], A[period_p.IndexPeriod, block_b]);

            expr.AddConstant(-(1 + RotaInstance.DemFactor) * RotaInstance.DemandMatrix[sort_s, period_p.IndexPeriod]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[sort_s, period_p.IndexPeriod, block_b]);
                expr.AddTerm(-CoefSlackExcess, Exc[sort_s, period_p.IndexPeriod, block_b]);
            }
            expr.AddConstant(-term);

            return expr;//Revisado
        }
        public GRBLinExpr R_B_AgC_createblock(int sort_s, Period period_p, int block_b,
                                              GRBVar[,,] X, GRBVar[,] A,
                                              double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            for (int h = 0; h < block_b; h++)
            {
                for (int i = 0; i < RotaInstance.NumFMUs; i++)
                {
                    R_FMU Unit = RotaInstance.GetNodeByIndex(i);
                    expr.AddTerm(Unit.GetSortimentVol(sort_s, period_p, "volume"), X[i, period_p.IndexPeriod, h]);
                }
            }

            expr.AddTerm(RotaInstance.NewBlockPercent * (1 + RotaInstance.DemFactor) * RotaInstance.DemandMatrix[sort_s, period_p.IndexPeriod], A[period_p.IndexPeriod, block_b]);

            expr.AddConstant(-(1 + RotaInstance.DemFactor) * RotaInstance.DemandMatrix[sort_s, period_p.IndexPeriod]);
            expr.AddConstant(-term);

            return expr;//Revisado
        }
        //GRBVar[,,,] Slk_B_AgC_nearblock = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];
        //GRBVar[,,,] Exc_B_AgC_nearblock = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];
        //    & \delta_{ij}\PG{x_{ipb}+x_{jpb}-1} \leq \Delta_{b}
        //    \forall i \in F, \forall j > i \in F, \\ \forall p \in P, \forall b \in B_p \label{eq:B_AgC_proxbloco}

        public GRBLinExpr R_B_AgC_nearblock(int fmu_i, int fmu_j, int period_p, int block_b,
                                            GRBVar[,,] X, GRBVar[,,,] Slk, GRBVar[,,,] Exc,
                                            int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_i, period_p, block_b]);
            expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_j, period_p, block_b]);
            expr.AddConstant(-RotaInstance.FMUDistances[fmu_i, fmu_j]);
            expr.AddConstant(-RotaInstance.MaxDistInBlock);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i, fmu_j, period_p, block_b]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, fmu_j, period_p, block_b]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }
        public GRBLinExpr R_B_AgC_nearblock(int fmu_i, int fmu_j, int period_p, int block_b,
                                            GRBVar[,,] X, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_i, period_p, block_b]);
            expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_j, period_p, block_b]);
            expr.AddConstant(-RotaInstance.FMUDistances[fmu_i, fmu_j]);
            expr.AddConstant(-RotaInstance.MaxDistInBlock);

            expr.AddConstant(-term);

            return expr; //Revisado
        }

        //GRBVar[,,,,] Slk_B_AgC_nearyear = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
        //GRBVar[,,,,] Exc_B_AgC_nearyear = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
        //     \delta_{ij}\PG{x_{ipb}+x_{jph}-1} \leq \Delta_{p}
        //     \forall i \in F, \forall j > i \in F, \forall p \in P\\ \forall b \in B_p, h \neq b \in B_p \label{eq:B_AgC_proxano}

        public GRBLinExpr R_B_AgC_nearyear(int fmu_i, int fmu_j, int period_p, int block_b, int block_h,
                                           GRBVar[,,] X, GRBVar[,,,,] Slk, GRBVar[,,,,] Exc,
                                           int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_i, period_p, block_b]);
            expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_j, period_p, block_h]);
            expr.AddConstant(-RotaInstance.FMUDistances[fmu_i, fmu_j]);
            expr.AddConstant(-RotaInstance.MaxDistOutBlockInPeriod);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i, fmu_j, period_p, block_b, block_h]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, fmu_j, period_p, block_b, block_h]);
            }
            expr.AddConstant(-term);

            return expr; //Revisado
        }
        public GRBLinExpr R_B_AgC_nearyear(int fmu_i, int fmu_j, int period_p, int block_b, int block_h,
                                           GRBVar[,,] X, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_i, period_p, block_b]);
            expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_j, period_p, block_h]);
            expr.AddConstant(-RotaInstance.FMUDistances[fmu_i, fmu_j]);
            expr.AddConstant(-RotaInstance.MaxDistOutBlockInPeriod);

            expr.AddConstant(-term);

            return expr; //Revisado
        }

        //GRBVar[,,,,] Slk_B_AgC_sequence = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
        //GRBVar[,,,,] Exc_B_AgC_sequence = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
        //    x_{ipb}+x_{jph} \leq 1
        //    \begin{matrix} \forall (i,j) \in A, \forall p \in P\\ \forall b \in B_p, h = b+1,\ldots,|B| \label{eq:B_AgC_seq}

        public static GRBLinExpr R_B_AgC_sequence(int fmu_i, int fmu_j, int period_p, int block_b, int block_h,
                                           GRBVar[,,] X, GRBVar[,,,,] Slk, GRBVar[,,,,] Exc,
                                           int CoefSlackExcess = 0, double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, X[fmu_i, period_p, block_b]);
            expr.AddTerm(1, X[fmu_j, period_p, block_h]);

            if (CoefSlackExcess != 0)
            {
                expr.AddTerm(CoefSlackExcess, Slk[fmu_i, fmu_j, period_p, block_b, block_h]);
                expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, fmu_j, period_p, block_b, block_h]);
            }
            expr.AddConstant(-term);

            return expr; //Revisada
        }

        public static GRBLinExpr R_B_AgC_sequence(int fmu_i, int fmu_j, int period_p, int block_b, int block_h,
                                           GRBVar[,,] X, double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();

            expr.AddTerm(1, X[fmu_i, period_p, block_b]);
            expr.AddTerm(1, X[fmu_j, period_p, block_h]);

            expr.AddConstant(-term);

            return expr; //Revisada
        }

        //GRBVar[,,,,] Slk_B_AgC_consecutive = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
        //GRBVar[,,,,] Exc_B_AgC_consecutive = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
        //    & x_{ipb}+x_{j(p+1)h} \leq 1
        //    & \forall (i,j) \in A, \forall b \in B_p,  \forall h \in B_p, \\ p = 1,\ldots,HP-1 \label{eq:B_AgC_consec}

        public GRBLinExpr R_B_AgC_consecutive(int fmu_i, int fmu_j, int period_p, int block_b, int block_h,
                                              GRBVar[,,] X, GRBVar[,,,,] Slk, GRBVar[,,,,] Exc,
                                              int CoefSlackExcess = 0, double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();
            if (period_p < RotaInstance.NumPeriods - 1)
            {
                expr.AddTerm(1, X[fmu_i, period_p, block_b]);
                expr.AddTerm(1, X[fmu_j, period_p + 1, block_h]);

                if (CoefSlackExcess != 0)
                {
                    expr.AddTerm(CoefSlackExcess, Slk[fmu_i, fmu_j, period_p, block_b, block_h]);
                    expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, fmu_j, period_p, block_b, block_h]);
                }
                expr.AddConstant(-term);
            }
            return expr; //Revisado
        }
        public GRBLinExpr R_B_AgC_consecutive(int fmu_i, int fmu_j, int period_p, int block_b, int block_h,
                                              GRBVar[,,] X, double term = 1)
        {
            GRBLinExpr expr = new GRBLinExpr();
            if (period_p < RotaInstance.NumPeriods - 1)
            {
                expr.AddTerm(1, X[fmu_i, period_p, block_b]);
                expr.AddTerm(1, X[fmu_j, period_p + 1, block_h]);

                expr.AddConstant(-term);
            }
            return expr; //Revisado
        }
        //GRBVar[,,,,] Slk_B_AgC_nearconsec = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
        //GRBVar[,,,,] Exc_B_AgC_nearconsec = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
        //    & \delta_{ij}\PG{x_{ipb}+x_{j(p+1)h}} \leq 2\Delta_{c}
        //    & \forall i,j \in F, \forall b \in B_p, \forall h \in B_p, \\  p = 1, \ldots, HP-1 \label{eq:B_AgC_proxconsec}

        public GRBLinExpr R_B_AgC_nearconsec(int fmu_i, int fmu_j, int period_p, int block_b, int block_h,
                                             GRBVar[,,] X, GRBVar[,,,,] Slk, GRBVar[,,,,] Exc,
                                             int CoefSlackExcess = 0, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            if (period_p < RotaInstance.NumPeriods - 1)
            {
                expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_i, period_p, block_b]);
                expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_j, period_p + 1, block_h]);

                if (CoefSlackExcess != 0)
                {
                    expr.AddTerm(CoefSlackExcess, Slk[fmu_i, fmu_j, period_p, block_b, block_h]);
                    expr.AddTerm(-CoefSlackExcess, Exc[fmu_i, fmu_j, period_p, block_b, block_h]);
                }
                expr.AddConstant(-2 * RotaInstance.MaxDistConsecutivePeriod);
                expr.AddConstant(-term);
            }
            return expr;//Revisado
        }

        public GRBLinExpr R_B_AgC_nearconsec(int fmu_i, int fmu_j, int period_p, int block_b, int block_h,
                                             GRBVar[,,] X, double term = 0)
        {
            GRBLinExpr expr = new GRBLinExpr();

            if (period_p < RotaInstance.NumPeriods - 1)
            {
                expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_i, period_p, block_b]);
                expr.AddTerm(RotaInstance.FMUDistances[fmu_i, fmu_j], X[fmu_j, period_p + 1, block_h]);

                expr.AddConstant(-2 * RotaInstance.MaxDistConsecutivePeriod);
                expr.AddConstant(-term);
            }
            return expr;//Revisado
        }

        public void CreateModelHyb(string Pasta, int numConstraints, bool ShowMess = true,
                                bool ModelSenseMax = false, bool BinaryVars = true, bool SlackExcessVars = true,
                                bool OldVersion = false, string Agora = null, bool Hybrid = true,
                                char[,,] XVar = null, char[,,] AlphaVar = null, bool log=false)
        {
            bool[] UseConstraints = new bool[numConstraints];
            bool[] SlackExcess = new bool[numConstraints];
            double[] RHS = new double[numConstraints];
            for (int i = 0; i < numConstraints; i++)
            {
                UseConstraints[i] = true;
                SlackExcess[i] = SlackExcessVars;
                RHS[i] = 0;
            }
            CreateModelHyb(Pasta, UseConstraints, SlackExcess, RHS,
                           ShowMess, ModelSenseMax, BinaryVars,
                           OldVersion, Agora, Hybrid, XVar, AlphaVar, log);
        }

        public void CreateModelHyb(string Pasta, bool[] UseConstraints, bool[] SlackExcess,
                                double[] RHS, bool ShowMess, bool ModelSenseMax = false,
                                bool BinaryVars = true, bool OldVersion = false, string Agora = null,
                                bool Hybrid = true, char[,,] XVar = null, char[,,] AlphaVar = null,
                                bool log = false, bool regtxt = false)
        {
            //Hybrid:
            //true: Calculate Harvest Scheduling & VRP. XVar null & AlphaVar null
            //False: Separated optimization
            //  if XVar and AlphaVar is null, calculate Harvest Scheduling
            //  else, calculate VRP with XVar & AlphaVar values.

            bool HarvestSchedulingOptimize = true;
            StringBuilder[] logtxt = new StringBuilder[10];
            String[] NameConstraints = { "M_Maintenance", "M_Maint_open", "M_vehicle", "M_use", "M_acess", "M_flow", "M_AgC_qtharvest", "M_AgC_initialblock", "M_AgC_ageblock", "M_AgC_varvolmin", "M_AgC_varvolmax", "M_AgC_maxarea", "M_AgC_minarea", "B_AgC_firstblock", "B_AgC_createblock", "B_AgC_nearblock", "B_AgC_nearyear", "B_AgC_sequence", "B_AgC_consecutive", "B_AgC_nearconsec" };
            for (int i = 0; i < logtxt.Length; i++)
            {
                logtxt[i] = new StringBuilder();
            }
            if (Agora == null)
            {
                Agora = "(" + ToolBox.GetNow() + ")";
            }

            if (Hybrid)
            {
                XVar = null;
                AlphaVar = null;
                for (int uc = 0; uc < UseConstraints.Length; uc++)
                {
                    UseConstraints[uc] = true;
                }
            }
            else
            {
                if (XVar != null && AlphaVar != null)
                {
                    HarvestSchedulingOptimize = false;
                }
                for (int uc = 0; uc < UseConstraints.Length; uc++)
                {
                    if (uc < NumConstrVRP)
                    {
                        UseConstraints[uc] = !HarvestSchedulingOptimize;
                    }
                    else
                    {
                        UseConstraints[uc] = HarvestSchedulingOptimize;
                    }
                }
            }

            //Var aux
            List<(string, int)> AuxIndexes;
            List<bool> tests;

            //string ConstName;
            string ConstName;
            int[] CoefSlackExcess = new int[SlackExcess.Length];
            for (int i = 0; i < SlackExcess.Length; i++)
            {
                CoefSlackExcess[i] = 0;
                if (SlackExcess[i])
                {
                    CoefSlackExcess[i] = 1;
                }
            }

            if (BinaryVars)
            {
                VarType = GRB.BINARY;
            }

            //Arquivos do Modelo
            if (Hybrid)
            {
                NomeArquivo = "_Hyb_" + RotaInstance.Name;
            } else
            {
                if (HarvestSchedulingOptimize)
                {
                    NomeArquivo = "_HSO_" + RotaInstance.Name;
                } else
                {
                    NomeArquivo = "_VRP_" + RotaInstance.Name;
                }
            }
            if (log)
            {
                ToolBox.AppendLineSbCapacity(logtxt, "Nome: " + NomeArquivo);
                ToolBox.AppendLineSbCapacity(logtxt, "Data: " + Agora);
            }
            //StringBuilder CountConstraint = new StringBuilder();
            int[] CounterConstraint = new int[UseConstraints.Length];
            string[] NameVar = {"X_FMUHarvesting", "W_RoadOpening", "Z_RoadMaintenance", "Y_RoadUsed", "U_Travels", "Alpha"};
            int[] CounterVar = new int[NameVar.Length];
            //StringBuilder CountName = new StringBuilder();

            using StreamWriter fileall = new StreamWriter(String.Format("{0}[all]{1}{2}{3}.txt", Pasta, NomeArquivo, Agora, 0));
            using StreamWriter fileCountConst = new StreamWriter(String.Format("{0}[CountConst]{1}{2}{3}.txt", Pasta, NomeArquivo, Agora, 0));

            Ambiente = new GRBEnv
            {
                LogFile = Pasta + NomeArquivo + Agora + ".log"
            };

            Modelo = new GRBModel(Ambiente)
            {
                ModelSense = GRB.MAXIMIZE,
                ModelName = NomeArquivo + Agora
            };

            if (ModelSenseMax)
            {
                Modelo.ModelSense = GRB.MAXIMIZE;
            }

            // Must set LazyConstraints parameter when using lazy constraints

            Modelo.Parameters.LazyConstraints = 1;

            //Create Variables
            //X_FMUHarvesting(i,p,b) = 1 if fmu i is harvested at p period and block b; 0 c.c.
            GRBVar[,,] X_FMUHarvesting = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];

            //W_RoadOpening(i,j,p) = 1 if there is an open road connecting stands i and j in period p; 0 c.c.
            GRBVar[,,] W_RoadOpening = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];

            //Z_RoadMaintenance(i,j,p) = 1 if is maintained the road connecting stands i and j in period p; 0 c.c.
            GRBVar[,,] Z_RoadMaintenance = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];

            //Y_RoadUse(i,j,p) = 1 if is used the road connecting stands i and j in period p; 0 c.c.
            GRBVar[,,] Y_RoadUsed = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];

            //U_Travels(i,j,p) = amount of vehicles from i to j in period p;
            GRBVar[,,] U_Travels = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];

            //Alpha(p,b) = 1 if is created block b in period p; 0 c.c.
            GRBVar[,] Alpha = new GRBVar[RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];

            //Variaveis Falta e excesso

            GRBVar[,,] Slk_M_Maintenance = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
            GRBVar[,,] Exc_M_Maintenance = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];

            GRBVar[,,] Slk_M_maint_open = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
            GRBVar[,,] Exc_M_maint_open = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];

            GRBVar[,,] Slk_M_vehicle = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
            GRBVar[,,] Exc_M_vehicle = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];

            GRBVar[,,] Slk_M_use = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];
            GRBVar[,,] Exc_M_use = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods];

            GRBVar[,] Slk_M_acess = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods];
            GRBVar[,] Exc_M_acess = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods];

            GRBVar[,] Slk_M_flow = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods];
            GRBVar[,] Exc_M_flow = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods];

            GRBVar[] Slk_M_AgC_qtharvest = new GRBVar[RotaInstance.NumFMUs];
            GRBVar[] Exc_M_AgC_qtharvest = new GRBVar[RotaInstance.NumFMUs];

            GRBVar[] Slk_M_AgC_initialblock = new GRBVar[RotaInstance.NumFMUs];
            GRBVar[] Exc_M_AgC_initialblock = new GRBVar[RotaInstance.NumFMUs];

            GRBVar[,,] Slk_M_AgC_ageblock = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];
            GRBVar[,,] Exc_M_AgC_ageblock = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];

            GRBVar[,] Slk_M_AgC_varvolmin = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods];
            GRBVar[,] Exc_M_AgC_varvolmin = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods];

            GRBVar[,] Slk_M_AgC_varvolmax = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods];
            GRBVar[,] Exc_M_AgC_varvolmax = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods];

            GRBVar[,] Slk_M_AgC_maxarea = new GRBVar[RotaInstance.MaxNumBlocks, RotaInstance.NumPeriods];
            GRBVar[,] Exc_M_AgC_maxarea = new GRBVar[RotaInstance.MaxNumBlocks, RotaInstance.NumPeriods];

            GRBVar[,] Slk_M_AgC_minarea = new GRBVar[RotaInstance.MaxNumBlocks, RotaInstance.NumPeriods];
            GRBVar[,] Exc_M_AgC_minarea = new GRBVar[RotaInstance.MaxNumBlocks, RotaInstance.NumPeriods];

            GRBVar[] Slk_B_AgC_firstblock = new GRBVar[RotaInstance.NumPeriods];
            GRBVar[] Exc_B_AgC_firstblock = new GRBVar[RotaInstance.NumPeriods];

            GRBVar[,,] Slk_B_AgC_createblock = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];
            GRBVar[,,] Exc_B_AgC_createblock = new GRBVar[RotaInstance.NumSortiments, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];

            GRBVar[,,,] Slk_B_AgC_nearblock = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];
            GRBVar[,,,] Exc_B_AgC_nearblock = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks];

            GRBVar[,,,,] Slk_B_AgC_nearyear = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
            GRBVar[,,,,] Exc_B_AgC_nearyear = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];

            GRBVar[,,,,] Slk_B_AgC_sequence = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
            GRBVar[,,,,] Exc_B_AgC_sequence = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];

            GRBVar[,,,,] Slk_B_AgC_consecutive = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
            GRBVar[,,,,] Exc_B_AgC_consecutive = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];

            GRBVar[,,,,] Slk_B_AgC_nearconsec = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];
            GRBVar[,,,,] Exc_B_AgC_nearconsec = new GRBVar[RotaInstance.NumFMUs, RotaInstance.NumFMUs, RotaInstance.NumPeriods, RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks];

            //Variables type and Objective Function coefficients
            //Unit Harvesting
            if (log)
            {
                ToolBox.AppendLineSbCapacity(logtxt, "Vars X");
            }
            foreach (R_FMU Unit in RotaInstance.FMUs)
            {
                int i = Unit.Index;
                if (log)
                {
                    ToolBox.AppendLineSbCapacity(logtxt, String.Format("FMU {0}", Unit));
                }
                foreach (Period P in RotaInstance.AllPeriods)
                {
                    int p = P.IndexPeriod;
                    double LPV = 0;
                    if (Unit.IsFMU)
                    {
                        for (int s = 0; s < RotaInstance.NumSortiments; s++)
                        {
                            //add liquid profit value for harvest Unit
                            LPV += Unit.GetSortimentVol(s, P, "lpv");
                            if (Hybrid || !HarvestSchedulingOptimize)
                            {
                                //discounting minimum number of trucks if Unit is harvest at P times relative costs (load/unload)
                                LPV -= Unit.GetSortimentVol(s, P, "trucks") * RotaInstance.Cost_TravelFix_ByPeriod[p];
                            }
                        }
                    }

                    if (log)
                    {
                        ToolBox.AppendLineSbCapacity(logtxt, String.Format("FMU {0}\t P {1}: LPV: {2}", Unit.Index, P.IndexPeriod, LPV));
                    }

                    for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
                    {
                        AuxIndexes = new List<(string, int)> { ("I", i), ("P", p), ("B", b) };
                        string Xname = ToolBox.ConstraintName("X", AuxIndexes);
                        if (log)
                        {
                            ToolBox.AppendLineSbCapacity(logtxt, Xname);
                        }
                        else
                        {
                            X_FMUHarvesting[i, p, b] = Modelo.AddVar(0, 1, LPV, VarType, Xname.ToString());
                        }
                        CounterVar[0]++;
                    }
                }
                
            }
            //Block creation
            if (log)
            {
                ToolBox.AppendLineSbCapacity(logtxt, "Block Vars");
            }
            foreach (Period P in RotaInstance.AllPeriods)
            {
                int p = P.IndexPeriod;
                for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
                {
                    AuxIndexes = new List<(string, int)> { ("P", p), ("B", b) };
                    string Alphaname = ToolBox.ConstraintName("Alpha", AuxIndexes);
                    if (log)
                    {
                        ToolBox.AppendLineSbCapacity(logtxt, Alphaname);
                    }
                    else
                    {
                        Alpha[p, b] = Modelo.AddVar(0, 1, 0, VarType, Alphaname.ToString());
                    }
                        CounterVar[5]++;
                }
            }
            //VRP Variables
            if (log)
            {
                ToolBox.AppendLineSbCapacity(logtxt, "VRP Vars");
            }
            foreach (R_FMU Unit1 in RotaInstance.FMUs)
            {
                int i = Unit1.Index;
                foreach (R_FMU Unit2 in RotaInstance.FMUs)
                {
                    int j = Unit2.Index;
                    foreach (Period P in RotaInstance.AllPeriods)
                    {
                        int p = P.IndexPeriod;
                        AuxIndexes = new List<(string, int)> { ("I", i), ("J", j), ("P", p) };
                        string Wname = ToolBox.ConstraintName("W_ROpen", AuxIndexes);
                        string Zname = ToolBox.ConstraintName("Z_Maint", AuxIndexes);
                        string Yname = ToolBox.ConstraintName("Y_RUsed", AuxIndexes);
                        string Uname = ToolBox.ConstraintName("U_Trave", AuxIndexes);
                        if (log)
                        {
                            ToolBox.AppendLineSbCapacity(logtxt, Wname + "\n" + Zname + "\n" + Yname+ "\n" + Uname);
                        }
                        else
                        {
                            W_RoadOpening[i, j, p] = Modelo.AddVar(0, 1, -RotaInstance.CalcCost_RoadOpening(i, j, p), GRB.BINARY, Wname.ToString());
                            Z_RoadMaintenance[i, j, p] = Modelo.AddVar(0, 1, -RotaInstance.CalcCost_RoadMaintenance(i, j, p), GRB.BINARY, Zname.ToString());
                            Y_RoadUsed[i, j, p] = Modelo.AddVar(0, 1, 0, GRB.BINARY, Yname.ToString());
                            U_Travels[i, j, p] = Modelo.AddVar(0, GRB.INFINITY, -RotaInstance.CalcCost_TravelsIJ(i, j), GRB.INTEGER, Uname.ToString());
                        }
                        CounterVar[1]++;//W
                        CounterVar[2]++;//Z
                        CounterVar[3]++;//Y
                        CounterVar[4]++;//U
                    }
                }
            }
            // Slack & Excess Variables
            tests = new List<bool> {};
            for(int bo = 0; bo<11; bo++)
            {
                tests.Add(UseConstraints[bo] && SlackExcess[bo]);
            }

            if (ToolBox.Any(tests))//Slack & Excess Constraints 0 to 10
            {
                if (log)
                {
                    ToolBox.AppendLineSbCapacity(logtxt, "Slack & Excess Vars");
                }
                for (int i = 0; i < RotaInstance.NumFMUs; i++)
                {
                    for (int p = 0; p < RotaInstance.NumPeriods; p++)
                    {
                        for (int j = 0; j < RotaInstance.NumFMUs; j++)
                        {
                            AuxIndexes = new List<(string, int)> { ("I", i), ("J", j), ("P", p) };
                            // M_Maintenance
                            if (UseConstraints[0] && SlackExcess[0])
                            {
                                //z_{ijp} & \leq \sum_{p'\in P_p} w_{ijp'} & \forall i,j \in F, \forall p > 2 \in P_p
                                string Varname = ToolBox.ConstraintName("_M_Maintenance", AuxIndexes);
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk & Exc {0}", Varname));
                                }
                                else
                                {
                                    Slk_M_Maintenance[i, j, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                    Exc_M_Maintenance[i, j, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                }
                            }
                            // M_Maint_open
                            if (UseConstraints[1] && SlackExcess[1])
                            {
                                //w_{ijp} + z_{ijp} & \leq 1  & \forall i,j \in F, \forall p \in P,  \label{eq:M_manut_abert} 
                                string Varname = ToolBox.ConstraintName("_M_maint_open", AuxIndexes);
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                }
                                else
                                {
                                    Slk_M_maint_open[i, j, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                    Exc_M_maint_open[i, j, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                }
                            }
                            // M_vehicle
                            if (UseConstraints[2] && SlackExcess[2])
                            {
                                //y_{ijp} & \leq w_{ijp} + z_{ijp}  & \forall i,j \in F, \forall p \in P,  \label{eq:M_veiculo}  
                                string Varname = ToolBox.ConstraintName("_M_vehicle", AuxIndexes);
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                }
                                else
                                {
                                    Slk_M_vehicle[i, j, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                    Exc_M_vehicle[i, j, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                }
                            }
                            // M_use
                            if (UseConstraints[3] && SlackExcess[3])
                            {
                                //y_{ijp} & \leq u_{ijp}  & \forall i,j \in F, \forall p \in P,  \label{eq:M_uso} 
                                string Varname = ToolBox.ConstraintName("_M_use", AuxIndexes);
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                }
                                else
                                {
                                    Slk_M_use[i, j, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                    Exc_M_use[i, j, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                }
                            }
                        }

                        // M_AgC_ageblock
                        if (UseConstraints[8] && SlackExcess[8])
                        {
                            //(n_i+p-1)x_{ipb} & \geq \eta_i x_{ipb} &
                            //\forall i \in F, \forall p \in P, \\ \forall b \in B_p  \label{eq:M_AgC_idadebloco}
                            for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
                            {
                                AuxIndexes = new List<(string, int)> { ("I", i), ("P", p), ("B", b) };
                                string Varname = ToolBox.ConstraintName("_M_AgC_ageblock", AuxIndexes);
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                }
                                else
                                {
                                    Slk_M_AgC_ageblock[i, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                    Exc_M_AgC_ageblock[i, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                }
                            }
                        }
                        AuxIndexes = new List<(string, int)> { ("I", i), ("P", p) };
                        // M_acess
                        if (UseConstraints[4] && SlackExcess[4])
                        {
                            //\sum_{b\in B} x_{ipb} & \leq \sum_{j\in F} y_{ijp}  & \forall i \in F, \forall p \in P,  \label{eq:M_acesso}}
                            string Varname = ToolBox.ConstraintName("_M_acess", AuxIndexes);
                            if (log)
                            {
                                ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                            }
                            else
                            {
                                Slk_M_acess[i, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                Exc_M_acess[i, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                            }
                        }
                        // M_flow
                        if (UseConstraints[5] && SlackExcess[5])
                        {
                            //\sum_{j\in F} u_{jip} + \varepsilon_{ip}\left(\sum_{b\in B} x_{ipb}\right) = \sum_{j\in F} u_{ijp}
                            // \forall i \in F, \\ \forall p \in P   \label{eq:M_fluxo}
                            string Varname = ToolBox.ConstraintName("_M_flow", AuxIndexes);
                            if (log)
                            {
                                ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                            }
                            else
                            {
                                Slk_M_flow[i, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                Exc_M_flow[i, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                            }
                        }
                        if(i == RotaInstance.NumFMUs - 1)
                        {
                            for (int s = 0; s < RotaInstance.NumSortiments; s++)
                            {
                                AuxIndexes = new List<(string, int)> { ("S", s), ("P", p) };
                                // M_AgC_varvolmin
                                if (UseConstraints[9] && SlackExcess[9])
                                {
                                    //\sum_{b\in B}\sum_{i \in F} \upsilon_{isp}x_{ipb}  \geq (1-\sigma)d_{sp}
                                    //\forall s\in S, \forall p\in P \label{eq:M_AgC_varvolmin}
                                    string Varname = ToolBox.ConstraintName("_M_AgC_varvolmin", AuxIndexes);
                                    if (log)
                                    {
                                        ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                    }
                                    else
                                    {
                                        Slk_M_AgC_varvolmin[s, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                        Exc_M_AgC_varvolmin[s, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                    }
                                }
                                // M_AgC_varvolmax
                                if (UseConstraints[10] && SlackExcess[10])
                                {
                                    //\sum_{b\in B}\sum_{i \in F} \upsilon_{isp}x_{ipb} \leq (1+\sigma)d_{sp}
                                    //\forall s\in S, \forall p\in P \label{eq:M_AgC_varvolmax}
                                    string Varname = ToolBox.ConstraintName("_M_AgC_varvolmax", AuxIndexes);
                                    if (log)
                                    {
                                        ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                    }
                                    else
                                    {
                                        Slk_M_AgC_varvolmax[s, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                        Exc_M_AgC_varvolmax[s, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                    }
                                }
                            }
                        }                    
                    }
                    AuxIndexes = new List<(string, int)> { ("I", i) };
                    // M_AgC_qtharvest
                    if (UseConstraints[6] && SlackExcess[6])
                    {
                        //\sum_{b\in B}\sum_{p\in P} x_{ipb} & \leq 1 & \forall i \in F \label{eq:M_AgC_qtcolh}
                        string Varname = ToolBox.ConstraintName("_M_AgC_qtharvest", AuxIndexes);
                        if (log)
                        {
                            ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                        }
                        else
                        {
                            Slk_M_AgC_qtharvest[i] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                            Exc_M_AgC_qtharvest[i] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                        }
                    }
                    // M_AgC_initialblock
                    if (UseConstraints[7] && SlackExcess[7])
                    {
                        //x_{i11} & = 1 & \forall i\in F_1 \label{eq:M_AgC_inibloco}

                        string Varname = ToolBox.ConstraintName("_M_AgC_initialblock", AuxIndexes);
                        if (log)
                        {
                            ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                        }
                        else
                        {
                            Slk_M_AgC_initialblock[i] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                            Exc_M_AgC_initialblock[i] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                        }
                    }
                }
            }


            tests = new List<bool> { };
            for (int bo = 11; bo < 20; bo++)
            {
                tests.Add(UseConstraints[bo] && SlackExcess[bo]);
            }
            if (ToolBox.Any(tests))//Slack & Excess Constraints 11 to 20
            {
                for (int p = 0; p < RotaInstance.NumPeriods; p++)
                {
                    for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
                    {
                        AuxIndexes = new List<(string, int)> { ("P", p), ("B", b) };
                        // M_AgC_maxarea
                        if (UseConstraints[11] && SlackExcess[11])
                        //\sum_{i \in F} a_i x_{ipb} & \leq \overline{A_p}\alpha_{pb}
                        //\forall p \in P, \forall b \in B_p \label{eq:M_AgC_maxarea}
                        {
                            string Varname = ToolBox.ConstraintName("_M_AgC_maxarea", AuxIndexes);
                            if (log)
                            {
                                ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                            }
                            else
                            {
                                Slk_M_AgC_maxarea[b, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                Exc_M_AgC_maxarea[b, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                            }
                        }
                        // M_AgC_minarea
                        if (UseConstraints[12] && SlackExcess[12])
                        {
                            //\sum_{i\in F} a_i x_{ipb} & \geq \underline{A_p}\alpha_{pb}
                            //\forall p \in P, \forall b \in B_p \label{eq:M_AgC_minarea}
                            string Varname = ToolBox.ConstraintName("_M_AgC_minarea", AuxIndexes);
                            if (log)
                            {
                                ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                            }
                            else
                            {
                                Slk_M_AgC_minarea[b, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                Exc_M_AgC_minarea[b, p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                            }
                        }
                        //Creating Blocks
                        if (b == 0)
                        {//First Block
                            // B_AgC_firstblock
                            if (UseConstraints[13] && SlackExcess[13])
                            {
                                //\alpha_{p1} & = 1 & \forall p \in P \label{eq:B_AgC_primbloco}
                                string Varname = ToolBox.ConstraintName("_M_AgC_firstblock", AuxIndexes);
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                }
                                else
                                {
                                    Slk_B_AgC_firstblock[p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                    Exc_B_AgC_firstblock[p] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                }
                            }
                        }
                        else
                        {//other blocks
                         // B_AgC_createblock
                            if (UseConstraints[14] && SlackExcess[14])
                            {
                                //\somat{h}{1}{b-1} \somat{i}{1}{N} \upsilon_{isp}x_{iph} + \tau(1+\sigma)d_{sp} \alpha_{pb} \leq (1+\sigma)d_{sp}
                                //& \forall s\in S, \forall p \in P, b = 2,\ldots, |B| %\label{eq:B_AgC_criabloco}
                                for (int s = 0; s < RotaInstance.NumSortiments; s++)
                                {
                                    AuxIndexes = new List<(string, int)> { ("S", s), ("P", p), ("B", b) };
                                    string Varname = ToolBox.ConstraintName("_B_AgC_createblock", AuxIndexes);
                                    if (log)
                                    {
                                        ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                    }
                                    else
                                    {
                                        Slk_B_AgC_createblock[s, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                        Exc_B_AgC_createblock[s, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                    }
                                }
                            }
                        }
                        //distances - nearblock, nearyear, sequence, consecutive
                        if (ToolBox.Any(new List<bool>{ UseConstraints[15] && SlackExcess[15],
                                                        UseConstraints[16] && SlackExcess[16],
                                                        UseConstraints[17] && SlackExcess[17],
                                                        UseConstraints[18] && SlackExcess[18],
                                                        UseConstraints[19] && SlackExcess[19]}))
                        { 
                            for (int i = 0; i < RotaInstance.NumFMUs; i++)
                            {
                                for (int j = 0; j < RotaInstance.NumFMUs; j++)
                                {
                                    AuxIndexes = new List<(string, int)> { ("I", i), ("J", j), ("P", p), ("B", b) };
                                    // B_AgC_nearblock
                                    if (UseConstraints[15] && SlackExcess[15])
                                    {
                                        //\delta_{ij}\PG{x_{ipb}+x_{jpb}-1} & \leq \Delta_{b}
                                        //\forall i \in F, \forall j > i \in F, \\ \forall p \in P, \forall b \in B_p \label{eq:B_AgC_proxbloco}
                                        string Varname = ToolBox.ConstraintName("_B_AgC_nearblock", AuxIndexes);
                                        if (log)
                                        {
                                            ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                        }
                                        else
                                        {
                                            Slk_B_AgC_nearblock[i, j, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                            Exc_B_AgC_nearblock[i, j, p, b] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                        }
                                    }

                                    for (int h = 0; h < RotaInstance.MaxNumBlocks; h++)
                                    {
                                        AuxIndexes = new List<(string, int)> { ("I", i), ("J", j), ("P", p), ("B", b), ("H", h) };
                                        // B_AgC_nearyear
                                        //\delta_{ij}\PG{x_{ipb}+x_{jph}-1} & \leq \Delta_{a}
                                        //\forall i \in F, \forall j > i \in F, \forall p \in P, \forall b,h \in B_p, h \neq b \label{eq:B_AgC_proxano}
                                        // RotaInstance.NumFMUs, RotaInstance.NumFMUs,  RotaInstance.NumPeriods,  RotaInstance.MaxNumBlocks, RotaInstance.MaxNumBlocks 
                                        if (h != b && UseConstraints[16] && SlackExcess[16])
                                        {
                                            string Varname = ToolBox.ConstraintName("_B_AgC_nearyear", AuxIndexes);
                                            if (log)
                                            {
                                                ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                            }
                                            else
                                            {
                                                Slk_B_AgC_nearyear[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                                Exc_B_AgC_nearyear[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                            }
                                        }
                                        // B_AgC_sequence 
                                        if (h > b && UseConstraints[17] && SlackExcess[17])
                                        {
                                            //x_{ipb}+x_{jph} &\leq 1
                                            //\forall i \in F, \forall j \in A_i,\\ \forall p \in P, \forall b \in B_p, \\ h = b+1,\ldots,|B| \label{eq:B_AgC_seq}
                                            string Varname = ToolBox.ConstraintName("_B_AgC_sequence", AuxIndexes);
                                            if (log)
                                            {
                                                ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                            }
                                            else
                                            {
                                                Slk_B_AgC_sequence[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                                Exc_B_AgC_sequence[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                            }
                                        }
                                        // B_AgC_consecutive 
                                        if (UseConstraints[18] && SlackExcess[18])
                                        {
                                            //x_{ipb}+x_{j(p+1)h} &\leq 1
                                            //\forall i \in F, \forall j \in A_i,\\ \forall b \in B_p,  \forall h \in B_p, \\ p = 1,\ldots,HP-1 \label{eq:B_AgC_consec}
                                            string Varname = ToolBox.ConstraintName("_B_AgC_consecutive", AuxIndexes);
                                            if (log)
                                            {
                                                ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                            }
                                            else
                                            {
                                                Slk_B_AgC_consecutive[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                                Exc_B_AgC_consecutive[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                            }
                                        }
                                        // B_AgC_nearconsec
                                        if (UseConstraints[19] && SlackExcess[19])
                                        {
                                            //\delta_{ij}\PG{x_{ipb}+x_{j(p+1)h}} \leq 2\Delta_{c}
                                            //\forall i,j \in F, \forall b \in B_p, \forall h \in B_p,  p = 1, \ldots, HP-1 \label{eq:B_AgC_proxconsec}%
                                            string Varname = ToolBox.ConstraintName("_B_AgC_nearconsec", AuxIndexes);
                                            if (log)
                                            {
                                                ToolBox.AppendLineSbCapacity(logtxt, String.Format("Slk e Exc{0}", Varname));
                                            }
                                            else
                                            {
                                                Slk_B_AgC_nearconsec[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Slk" + Varname);
                                                Exc_B_AgC_nearconsec[i, j, p, b, h] = Modelo.AddVar(0, GRB.INFINITY, 0, GRB.CONTINUOUS, "Exc" + Varname);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            //Constraints

            if (log)
            {
                ToolBox.AppendLineSbCapacity(logtxt, "Constraints");
            }
            foreach (Period P in RotaInstance.AllPeriods)
            {
                int p = P.IndexPeriod;
                foreach (R_FMU I in RotaInstance.FMUs)
                {
                    int i = I.Index;
                    foreach (R_FMU J in RotaInstance.FMUs)
                    {
                        int j = J.Index;
                        if (I != J)
                        {
                            // M_Maintenance
                            if (UseConstraints[0] && P.IndexPeriod > 2)
                            {
                                //z_{ijp} & \leq \sum_{p'\in P_p} w_{ijp'} & \forall i,j \in F, \forall p > 2 \in P_p
                                ConstName = ToolBox.ConstraintName("R00_M_Maintenance",
                                    new List<(string, int)> { ("I", i), ("J", j), ("P", p) });
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                                }
                                else
                                {
                                    if (CoefSlackExcess[0] == 0)
                                    {
                                        Modelo.AddConstr(R_M_Maintenance(i, j, p, W_RoadOpening, Z_RoadMaintenance,
                                                                     0) <= 0,
                                                                     ConstName);
                                    }
                                    else
                                    {
                                        Modelo.AddConstr(R_M_Maintenance(i, j, p, W_RoadOpening, Z_RoadMaintenance,
                                                                     Slk_M_Maintenance, Exc_M_Maintenance, CoefSlackExcess[0], 0) <= 0,
                                                                     ConstName);
                                    }
                                    fileall.WriteLine(ConstName);
                                    
                                }
                                CounterConstraint[0]++;
                            }
                            // M_Maint_open
                            if (UseConstraints[1])
                            {
                                ConstName = ToolBox.ConstraintName("R01_M_Maint_open",
                                    new List<(string, int)> { ("I", i), ("J", j), ("P", p) });
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                                }
                                else
                                {
                                    if (CoefSlackExcess[1] == 0)
                                    {
                                        Modelo.AddConstr(R_M_maint_open(i, j, p, W_RoadOpening, Z_RoadMaintenance,
                                                                     1) <= 0,
                                                    ConstName);
                                    }
                                    else
                                    {
                                        Modelo.AddConstr(R_M_maint_open(i, j, p, W_RoadOpening, Z_RoadMaintenance,
                                                              Slk_M_Maintenance, Exc_M_Maintenance, CoefSlackExcess[1], 1) <= 0,
                                              ConstName);
                                    }                                        
                                    fileall.WriteLine(ConstName);
                                }
                                CounterConstraint[1]++;
                            }
                            // M_vehicle
                            if (UseConstraints[2])
                            {
                                //y_{ijp} & \leq w_{ijp} + z_{ijp}  & \forall i,j \in F, \forall p \in P,  \label{eq:M_veiculo}  
                                ConstName = ToolBox.ConstraintName("R02_M_vehicle",
                                    new List<(string, int)> { ("I", i), ("J", j), ("P", p) });
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                                }
                                else
                                {
                                    if (CoefSlackExcess[2] == 0)
                                    {
                                        Modelo.AddConstr(R_M_vehicle(i, j, p, W_RoadOpening, Z_RoadMaintenance, Y_RoadUsed, 0) <= 0,
                                                     ConstName);
                                    }
                                    else
                                    {
                                        Modelo.AddConstr(R_M_vehicle(i, j, p, W_RoadOpening, Z_RoadMaintenance, Y_RoadUsed,
                                                                 Slk_M_vehicle, Exc_M_vehicle, CoefSlackExcess[2], 0) <= 0,
                                                     ConstName);
                                    }
                                    fileall.WriteLine(ConstName);
                                }
                                    CounterConstraint[2]++;
                            }
                            // M_use
                            if (UseConstraints[3])
                            {
                                //y_{ijp} & \leq u_{ijp}  & \forall i,j \in F, \forall p \in P,  \label{eq:M_uso}
                                ConstName = ToolBox.ConstraintName("R03_M_use",
                                    new List<(string, int)> { ("I", i), ("J", j), ("P", p) });
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                                }
                                else
                                {
                                    if (CoefSlackExcess[3] == 0)
                                    {
                                        Modelo.AddConstr(R_M_use(i, j, p, Y_RoadUsed, U_Travels,
                                                                0) <= 0,
                                                    ConstName);
                                    }
                                    else
                                    {
                                        Modelo.AddConstr(R_M_use(i, j, p, Y_RoadUsed, U_Travels,
                                                                 Slk_M_use, Exc_M_use, CoefSlackExcess[3], 0) <= 0,
                                                     ConstName);
                                    }
                                    fileall.WriteLine(ConstName);
                                }
                                    CounterConstraint[3]++;
                            }
                        }
                    }
                    // M_acess
                    if (UseConstraints[4])
                    {
                        //\sum_{b\in B} x_{ipb} & \leq \sum_{j\in F} y_{ijp}  & \forall i \in F, \forall p \in P,  \label{eq:M_acesso}  
                        ConstName = ToolBox.ConstraintName("R04_M_acess",
                                    new List<(string, int)> { ("I", i), ("P", p) });
                        if (log)
                        {
                            ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                        }
                        else
                        {
                            if (CoefSlackExcess[4] == 0)
                            {
                                Modelo.AddConstr(R_M_acess(i, p, X_FMUHarvesting, Y_RoadUsed) <= 0,
                                                ConstName);
                            }
                            else
                            {
                                Modelo.AddConstr(R_M_acess(i, p, X_FMUHarvesting, Y_RoadUsed, Slk_M_acess, Exc_M_acess,
                                                           CoefSlackExcess[4], 0) <= 0,
                                                 ConstName);
                            }
                            fileall.WriteLine(ConstName);
                        }
                            CounterConstraint[4]++;
                    }
                    // M_flow
                    if (UseConstraints[5])
                    {
                        //\sum_{j\in F} u_{jip} + \varepsilon_{ip}\left(\sum_{b\in B} x_{ipb}\right) & = \sum_{j\in F} u_{ijp}
                        // \forall i \in F, \\ \forall p \in P  \label{eq:M_fluxo}
                        ConstName = ToolBox.ConstraintName("R05_M_flow",
                                    new List<(string, int)> { ("I", i), ("P", p) });
                        if (log)
                        {
                            ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                        }
                        else
                        {
                            if (CoefSlackExcess[5] == 0)
                            {
                                Modelo.AddConstr(R_M_flow(i, P, X_FMUHarvesting, U_Travels) == 0,
                                             ConstName);
                            }
                            else
                            {
                                Modelo.AddConstr(R_M_flow(i, P, X_FMUHarvesting, U_Travels, Slk_M_flow, Exc_M_flow,
                                                       CoefSlackExcess[5]) == 0,
                                             ConstName);
                            }
                            fileall.WriteLine(ConstName);
                        }
                            CounterConstraint[5]++;
                    }
                    if (p == 0)
                    {
                        // M_AgC_qtharvest
                        if (UseConstraints[6])
                        {
                            //\sum_{b\in B}\sum_{p\in P} x_{ipb} & \leq 1
                            //\forall i \in F \label{eq:M_AgC_qtcolh}
                            ConstName = ToolBox.ConstraintName("R06_M_AgC_qtharvest", ("I", i));
                            if (log)
                            {
                                ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                            }
                            else
                            {
                                if (CoefSlackExcess[6] == 0)
                                {
                                    Modelo.AddConstr(R_M_AgC_qtharvest(i, X_FMUHarvesting, 1) <= 0,
                                                    ConstName);
                                }
                                else
                                {
                                    Modelo.AddConstr(R_M_AgC_qtharvest(i, X_FMUHarvesting, Slk_M_AgC_qtharvest, Exc_M_AgC_qtharvest,
                                                                    CoefSlackExcess[6], 1) <= 0,
                                                    ConstName);
                                }
                                fileall.WriteLine(ConstName);
                            }
                                CounterConstraint[6]++;
                        }
                        // M_AgC_initialblock
                        if (UseConstraints[7])
                        {
                            //x_{i11} & = 1 & \forall i\in F_1 \label{eq:M_AgC_inibloco}
                            if (I.IsFirstBlock)
                            {
                                ConstName = ToolBox.ConstraintName("R07_M_AgC_initialblock", ("I", i));
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                                }
                                else
                                {
                                    if (CoefSlackExcess[7] == 0)
                                    {
                                        Modelo.AddConstr(R_M_AgC_initialblock(i, X_FMUHarvesting) == 0,
                                                        ConstName);
                                    }
                                    else
                                    {
                                        Modelo.AddConstr(R_M_AgC_initialblock(i, X_FMUHarvesting, Slk_M_AgC_initialblock, Exc_M_AgC_initialblock,
                                                                            CoefSlackExcess[7]) == 0,
                                                        ConstName);
                                    }
                                    fileall.WriteLine(ConstName);
                                }
                                    CounterConstraint[7]++;
                            }
                        }
                    }
                    for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
                    {
                        // M_AgC_ageblock
                        if (UseConstraints[8])
                        {
                            //(n_i+p-1)x_{ipb} & \geq \eta_i x_{ipb}
                            //& \forall i \in F, \forall p \in P, \forall b \in B_p  \label{eq:M_AgC_idadebloco}
                            ConstName = ToolBox.ConstraintName("R08_M_AgC_ageblock",
                                    new List<(string, int)> { ("I", i), ("P", p), ("B", b) });
                            if (log)
                            {
                                ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                            }
                            else
                            {
                                if (CoefSlackExcess[8]==0) {
                                Modelo.AddConstr(R_M_AgC_ageblock(i, p, b, I.InitialAge, I.HarvestAge, X_FMUHarvesting ) >= 0,
                                             ConstName); }
                                else
                                {
                                Modelo.AddConstr(R_M_AgC_ageblock(i, p, b, I.InitialAge, I.HarvestAge, X_FMUHarvesting,
                                                              Slk_M_AgC_ageblock, Exc_M_AgC_ageblock, CoefSlackExcess[8]) >= 0,
                                             ConstName);
                                }
                                fileall.WriteLine(ConstName);
                            }
                                CounterConstraint[8]++;
                        }
                    }
                }
                for (int s = 0; s < RotaInstance.NumSortiments; s++)
                {
                    // M_AgC_varvolmin
                    if (UseConstraints[9])
                    {
                        //  \sum_{b\in B}\sum_{i \in F} \upsilon_{isp}x_{ipb} & \geq (1-\sigma)d_{sp}
                        //  & \forall s\in S, \forall p\in P \label{eq:M_AgC_varvolmin}
                        ConstName = ToolBox.ConstraintName("R09_M_AgC_varvolmin",
                                    new List<(string, int)> { ("S", s), ("P", p) });
                        if (log)
                        {
                            ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                        }
                        else
                        {
                            if (CoefSlackExcess[9]==0)
                            {
                            Modelo.AddConstr(R_M_AgC_varvolmin(s, P, X_FMUHarvesting) >= 0,
                                         ConstName);
                            }
                            else
                            {
                            Modelo.AddConstr(R_M_AgC_varvolmin(s, P, X_FMUHarvesting, Slk_M_AgC_varvolmin, Exc_M_AgC_varvolmin,
                                                           CoefSlackExcess[9]) >= 0,
                                         ConstName);
                            }
                            fileall.WriteLine(ConstName);
                        }
                            CounterConstraint[9]++;
                    }
                    // M_AgC_varvolmax
                    if (UseConstraints[10])
                    {
                        //  \sum_{b\in B}\sum_{i \in F} \upsilon_{isp}x_{ipb} & \leq (1+\sigma)d_{sp}
                        //  & \forall s\in S, \forall p\in P \label{eq:M_AgC_varvolmax}}
                        ConstName = ToolBox.ConstraintName("R10_M_AgC_varvolmax",
                                    new List<(string, int)> { ("S", s), ("P", p) });
                        if (log)
                        {
                            ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                        }
                        else
                        {
                            if (CoefSlackExcess[10] == 0)
                            {
                                Modelo.AddConstr(R_M_AgC_varvolmax(s, P, X_FMUHarvesting) <= 0,
                                             ConstName);
                            }
                            else
                            {
                                Modelo.AddConstr(R_M_AgC_varvolmax(s, P, X_FMUHarvesting, Slk_M_AgC_varvolmax, Exc_M_AgC_varvolmax,
                                                               CoefSlackExcess[10]) <= 0,
                                             ConstName);
                            }
                            fileall.WriteLine(ConstName);
                        }
                            CounterConstraint[10]++;
                    }
                }
                for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
                {
                    // M_AgC_maxarea
                    if (UseConstraints[11])
                    {
                        // \sum_{i \in F} a_i x_{ipb} & \leq \overline{A_p}\alpha_{pb}
                        // & \forall p \in P, \forall b \in B_p \label{eq:M_AgC_maxarea}
                        ConstName = ToolBox.ConstraintName("R11_M_AgC_maxarea",
                                new List<(string, int)> { ("P", p), ("B", b) });
                        if (log)
                        {
                            ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                        }
                        else
                        {
                            if (CoefSlackExcess[11] == 0)
                            {
                                Modelo.AddConstr(R_M_AgC_maxarea(p, b, X_FMUHarvesting, Alpha) <= 0,
                                ConstName);
                            }
                            else
                            {
                                Modelo.AddConstr(R_M_AgC_maxarea(p, b, X_FMUHarvesting, Alpha,
                                Slk_M_AgC_maxarea, Exc_M_AgC_maxarea, CoefSlackExcess[11]) <= 0,
                                ConstName);
                            }
                            fileall.WriteLine(ConstName);
                        }
                            CounterConstraint[11]++;
                    }
                    // M_AgC_minarea
                    if (UseConstraints[12])
                    {
                        //      \sum_{i\in F} a_i x_{ipb} & \geq \underline{A_p}\alpha_{pb}
                        //      & \forall p \in P, \forall b \in B_p \label{eq:M_AgC_minarea}}
                        ConstName = ToolBox.ConstraintName("R12_M_AgC_minarea",
                                    new List<(string, int)> { ("P", p), ("B", b) });
                        if (log)
                        {
                            ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                        }
                        else
                        {
                            if (CoefSlackExcess[12] == 0)
                            {
                                Modelo.AddConstr(R_M_AgC_minarea(p, b, X_FMUHarvesting, Alpha) >= 0,
                                ConstName);
                            }
                            else
                            {
                                Modelo.AddConstr(R_M_AgC_minarea(p, b, X_FMUHarvesting, Alpha,
                                Slk_M_AgC_minarea, Exc_M_AgC_minarea, CoefSlackExcess[12]) >= 0,
                                ConstName);
                            }
                            fileall.WriteLine(ConstName);
                        }
                            CounterConstraint[12]++;
                    }
                    if (b == 0)
                    {
                        // B_AgC_firstblock
                        if (UseConstraints[13])
                        {
                            ConstName = ToolBox.ConstraintName("R13_B_AgC_firstblock", ("P", p));
                            if (log)
                            {
                                ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                            }
                            else
                            {
                                if (CoefSlackExcess[13] == 0)
                                {
                                    Modelo.AddConstr(R_B_AgC_firstblock(p, Alpha) == 0,
                                    ConstName);
                                }
                                else
                                {
                                    Modelo.AddConstr(R_B_AgC_firstblock(p, Alpha, Slk_B_AgC_firstblock,
                                    Exc_B_AgC_firstblock, CoefSlackExcess[13]) == 0,
                                    ConstName);
                                }
                                fileall.WriteLine(ConstName);
                            }
                                CounterConstraint[13]++;
                        }
                    }
                    else
                    {
                        for (int s = 0; s < RotaInstance.NumSortiments; s++)
                        {
                            // B_AgC_createblock
                            if (UseConstraints[14])
                            {
                                //      \somat{h}{1}{b-1} \somat{i \in F} \upsilon_{isp}x_{iph} + \tau(1+\sigma)d_{sp} \alpha_{pb} \leq (1+\sigma)d_{sp}
                                //      & \forall s\in S, \forall p \in P, b = 2,\ldots, |B| %\label{eq:B_AgC_criabloco}
                                ConstName = ToolBox.ConstraintName("R14_B_AgC_createblock",
                                        new List<(string, int)> { ("S", s), ("P", p), ("B", b) });
                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                                }
                                else
                                {
                                    if (CoefSlackExcess[14] == 0)
                                    {
                                        Modelo.AddConstr(R_B_AgC_createblock(s, P, b, X_FMUHarvesting, Alpha) <= 0,
                                        ConstName);
                                    }
                                    else
                                    {
                                        Modelo.AddConstr(R_B_AgC_createblock(s, P, b, X_FMUHarvesting, Alpha,
                                        Slk_B_AgC_createblock, Exc_B_AgC_createblock, CoefSlackExcess[14]) <= 0,
                                        ConstName);
                                    }
                                    fileall.WriteLine(ConstName);
                                }
                                    CounterConstraint[14]++;
                            }
                        }
                    }
                }
            }
            
            // B_AgC_nearblock
            if (UseConstraints[15])
            {
                //      \delta_{ij}\PG{x_{ipb}+x_{jpb}-1} & \leq \Delta_{b}
                //      & \forall i \in F, \forall j > i \in F, \forall p \in P, \forall b \in B_p \label{eq:B_AgC_proxbloco}
                for (int i = 0; i < RotaInstance.NumFMUs - 1; i++)
                {
                    R_FMU fmui = RotaInstance.GetNodeByIndex(i);
                    if (fmui.IsFMU)
                    {
                        for (int j = i + 1; j < RotaInstance.NumFMUs; j++)
                        {
                            R_FMU fmuj = RotaInstance.GetNodeByIndex(j);
                            if (fmuj.IsFMU)
                            {
                                for (int p = 0; p < RotaInstance.NumPeriods; p++)
                                {
                                    for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
                                    {
                                        ConstName = ToolBox.ConstraintName("R15_B_AgC_nearblock",
                                            new List<(string, int)> { ("I", i), ("J", j), ("P", p), ("B", b) });
                                        if (log)
                                        {
                                            ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                                        }
                                        else
                                        {
                                            if (CoefSlackExcess[15] == 0)
                                            {
                                                Modelo.AddConstr(R_B_AgC_nearblock(i, j, p, b, X_FMUHarvesting) <= 0,
                                                ConstName);
                                            }
                                            else
                                            {
                                                Modelo.AddConstr(R_B_AgC_nearblock(i, j, p, b, X_FMUHarvesting, Slk_B_AgC_nearblock,
                                                Exc_B_AgC_nearblock, CoefSlackExcess[15]) <= 0,
                                                ConstName);
                                            }
                                            fileall.WriteLine(ConstName);
                                        }
                                            CounterConstraint[15]++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // B_AgC_nearyear
            if (UseConstraints[16])
            {
                //  \delta_{ij}\PG{x_{ipb}+x_{jph}-1} & \leq \Delta_{a}
                //  & \forall i \in F, \forall j > i \in F, \\ \forall p \in P, \forall b,h \in B_p,\\ h \neq b \label{eq:B_AgC_proxano}
                for (int i = 0; i < RotaInstance.NumFMUs - 1; i++)
                {
                    R_FMU fmui = RotaInstance.GetNodeByIndex(i);
                    if (fmui.IsFMU)
                    {
                        for (int j = i + 1; j < RotaInstance.NumFMUs; j++)
                        {
                            R_FMU fmuj = RotaInstance.GetNodeByIndex(j);
                            if (fmuj.IsFMU)
                            {
                                for (int p = 0; p < RotaInstance.NumPeriods; p++)
                                {
                                    for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
                                    {
                                        for (int h = 0; h < RotaInstance.MaxNumBlocks; h++)
                                        {
                                            if (h != b)
                                            {
                                                ConstName = ToolBox.ConstraintName("R16_B_AgC_nearyear",
                                                    new List<(string, int)> { ("I", i), ("J", j), ("P", p), ("B", b), ("H", h) });

                                                if (log)
                                                {
                                                    ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                                                }
                                                else
                                                {
                                                    if (CoefSlackExcess[16] == 0)
                                                    {
                                                        Modelo.AddConstr(R_B_AgC_nearyear(i, j, p, b, h, X_FMUHarvesting) <= 0,
                                                        ConstName);
                                                    }
                                                    else
                                                    {
                                                        Modelo.AddConstr(R_B_AgC_nearyear(i, j, p, b, h, X_FMUHarvesting,
                                                        Slk_B_AgC_nearyear, Exc_B_AgC_nearyear, CoefSlackExcess[16]) <= 0,
                                                        ConstName);
                                                    }
                                                    fileall.WriteLine(ConstName);
                                                }
                                                    CounterConstraint[16]++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // B_AgC_sequence & B_AgC_consecutive 
            foreach (R_AdjFmu adjs in RotaInstance.AdjacentFMUs)
            {
                R_FMU fmui = adjs.Fmu1;
                R_FMU fmuj = adjs.Fmu2;
                int i = fmui.Index;
                int j = fmuj.Index;
                if (UseConstraints[17]) // B_AgC_sequence 
                {
                    //  x_{ipb}+x_{jph} &\leq 1
                    //  \forall i \in F, \forall j \in A_i,\\ \forall p \in P, \forall b \in B_p, \\ h = b+1,\ldots,|B| \label{eq:B_AgC_seq}
                    for (int p = 0; p < RotaInstance.NumPeriods; p++)
                    {
                        for (int b = 0; b < RotaInstance.MaxNumBlocks - 1; b++)
                        {
                            for (int h = b + 1; h < RotaInstance.MaxNumBlocks; h++)
                            {
                                ConstName = ToolBox.ConstraintName("R17_B_AgC_sequence",
                                    new List<(string, int)> { ("I", i), ("J", j), ("P", p), ("B", b), ("H", h) });

                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                                }
                                else
                                {
                                    if (CoefSlackExcess[17] == 0)
                                    {
                                        Modelo.AddConstr(R_B_AgC_sequence(i, j, p, b, h,
                                        X_FMUHarvesting) <= 0,
                                        ConstName);
                                    }
                                    else
                                    {
                                        Modelo.AddConstr(R_B_AgC_sequence(i, j, p, b, h,
                                        X_FMUHarvesting, Slk_B_AgC_sequence,
                                        Exc_B_AgC_sequence, CoefSlackExcess[17]) <= 0,
                                        ConstName);
                                    }
                                    fileall.WriteLine(ConstName);
                                }
                                    CounterConstraint[17]++;
                            }
                        }
                    }
                }
                // B_AgC_consecutive 
                if (UseConstraints[18])
                {
                    //  x_{ipb}+x_{j(p+1)h} &\leq 1
                    //  & \forall i \in F, \forall j \in A_i, \forall b \in B_p,  \forall h \in B_p, \\ p = 1,\ldots,HP-1 \label{eq:B_AgC_consec}

                    for (int p = 0; p < RotaInstance.NumPeriods - 1; p++)
                    {
                        for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
                        {
                            for (int h = 0; h < RotaInstance.MaxNumBlocks; h++)
                            {
                                ConstName = ToolBox.ConstraintName("R18_B_AgC_consecutive",
                                    new List<(string, int)> { ("I", i), ("J", j), ("P", p), ("B", b), ("H", h) });

                                if (log)
                                {
                                    ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                                }
                                else
                                {
                                    if (CoefSlackExcess[18] == 0)
                                    {
                                        Modelo.AddConstr(R_B_AgC_consecutive(i, j, p, b, h, X_FMUHarvesting) <= 0,
                                        ConstName);
                                    }
                                    else
                                    {
                                        Modelo.AddConstr(R_B_AgC_consecutive(i, j, p, b, h, X_FMUHarvesting,
                                        Slk_B_AgC_consecutive, Exc_B_AgC_consecutive,
                                        CoefSlackExcess[18]) <= 0,
                                        ConstName);
                                    }
                                    fileall.WriteLine(ConstName);
                                }
                                    CounterConstraint[18]++;
                            }
                        }
                    }
                }
            }
            // B_AgC_nearconsec
            if (UseConstraints[19])
            {
                //  \delta_{ij}\PG{x_{ipb}+x_{j(p+1)h}} & \leq 2\Delta_{c}
                //  & \forall i,j \in F, \forall b \in B_p, \forall h \in B_p, p = 1, \ldots, HP-1 \label{eq:B_AgC_proxconsec}
                for (int i = 0; i < RotaInstance.NumFMUs - 1; i++)
                {
                    R_FMU fmui = RotaInstance.GetNodeByIndex(i);
                    if (fmui.IsFMU)
                    {
                        for (int j = i + 1; j < RotaInstance.NumFMUs; j++)
                        {
                            R_FMU fmuj = RotaInstance.GetNodeByIndex(j);
                            if (fmuj.IsFMU)
                            {
                                for (int p = 0; p < RotaInstance.NumPeriods - 1; p++)
                                {
                                    for (int b = 0; b < RotaInstance.MaxNumBlocks; b++)
                                    {
                                        for (int h = 0; h < RotaInstance.MaxNumBlocks; h++)
                                        {
                                            ConstName = ToolBox.ConstraintName("R19_B_AgC_nearconsec",
                                                    new List<(string, int)> { ("I", i), ("J", j), ("P", p), ("B", b), ("H", h) });

                                            if (log)
                                            {
                                                ToolBox.AppendLineSbCapacity(logtxt, ConstName);
                                            }
                                            else
                                            {
                                                if (CoefSlackExcess[19] == 0)
                                                {
                                                    Modelo.AddConstr(R_B_AgC_nearconsec(i, j, p, b, h, X_FMUHarvesting) <= 0,
                                                        ConstName);
                                                }
                                                else
                                                {
                                                    Modelo.AddConstr(R_B_AgC_nearconsec(i, j, p, b, h, X_FMUHarvesting,
                                                    Slk_B_AgC_nearconsec, Exc_B_AgC_nearconsec, CoefSlackExcess[19]) <= 0,
                                                        ConstName);
                                                }
                                                fileall.WriteLine(ConstName);
                                            }
                                                CounterConstraint[19]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            if (log)
            {
                Int64 val = 0;
                for(int i=0; i < CounterVar.Length; i++)
                {
                    ToolBox.AppendLineSbCapacity(logtxt, string.Format("{0}\t{1}", NameVar[i], CounterVar[i]));
                }
                ToolBox.AppendLineSbCapacity(logtxt, string.Format("Total Vars: {0}\n", val));
                val = 0;
                for (int i = 0; i < CounterConstraint.Length; i++)
                {
                    if (CounterConstraint[i] > 0)
                    {
                        ToolBox.AppendLineSbCapacity(logtxt,string.Format("R{0:00}\t{1}:\t {2}", i.ToString(), NameConstraints[i], CounterConstraint[i].ToString()));
                    }
                    else
                    {
                        ToolBox.AppendLineSbCapacity(logtxt, string.Format("R{0:00}\t{1}:\t Empty", i.ToString(), NameConstraints[i]));
                    }
                    val += CounterConstraint[i];
                }
                ToolBox.AppendLineSbCapacity(logtxt, string.Format("Total: {0}", val));
                val = 0;
            }
            else
            {
                for (int i = 0; i < CounterConstraint.Length; i++)
                {
                    if (CounterConstraint[i] > 0)
                    {
                        fileCountConst.WriteLine(string.Format("R{0}:\t {1}", i.ToString(), CounterConstraint[i].ToString()));
                    }
                }
            }

            if (OldVersion)
            {
                NomeArquivo += "[orig]";
            }
            else
            {
                NomeArquivo += "[adap]";
            }

//            ToolBox.GravaMsgTxt(Pasta, "[all]" + NomeArquivo, Agora, 0, gravar, ShowMess);
 //           if (!log)
//            {
//               ToolBox.GravaMsgTxt(Pasta, "[Paramet]" + NomeArquivo, Agora, 0, RotaInstance.Param, ShowMess);
//            }
//            ToolBox.GravaMsgTxt(Pasta, "[CountConst]" + NomeArquivo, Agora, 0, CountConstraint, ShowMess);

            GRBModel ModeloRelax = Modelo.Relax();

            List<(GRBModel, bool, string)> Models = new List<(GRBModel, bool, string)> { (Modelo, false, NomeArquivo), (ModeloRelax, true, "(relax)" + NomeArquivo) };

            
            foreach ((GRBModel, bool, string) TM in Models)
            {
                if (log)
                {
                    ToolBox.AppendLineSbCapacity(logtxt, String.Format("Model {0}, ShowMess {1}", TM.Item1.ToString(), TM.Item2));

                }
                else
                {
                    //Chama a Otimização
                    OptimizeModel(TM.Item3, Pasta, Agora, TM.Item1, TM.Item2);
                }
            }

            if (log)
            {
                foreach(R_FMU fmu in RotaInstance.FMUs)
                {
                    ToolBox.AppendLineSbCapacity(logtxt, fmu.ToString());
                }
            }


            if (log)
            {
                string _namefile = string.Format(@"{0}{1}_log.txt", Pasta, Agora);
                using StreamWriter fs = new StreamWriter(_namefile);
                for (int l=0; l< logtxt.Length; l++)
                {
                    fs.Write(String.Format("{0} - L:{1}\n", l,  logtxt[l].Capacity));
                }
                foreach (StringBuilder lt in logtxt)
                {
                    fs.Write(lt);
                }
            }

            foreach ((GRBModel, bool, string) TM in Models)
            {
                TM.Item1.Dispose();
            }
        }

        public void OptimizeModel(string NomeArq, string Pasta, string Agora, GRBModel OpModelo,
                                  bool ShowMess, bool relax = false)
        {

            // Must set LazyConstraints parameter when using lazy constraints

            OpModelo.Parameters.LazyConstraints = 1;

            if (NomeArquivo != NomeArq)
            {
                NomeArquivo = NomeArq;
            }

            List<string> TypeModelsExt = new List<string> { "PL", "MPS", "HNT", "BAS", "PRM", "ATTR" };
            //Escrita do Modelo
            foreach (string typefile in TypeModelsExt)
            {
                ToolBox.GravaModelo(typefile, Pasta, NomeArquivo, Agora, OpModelo, ShowMess);
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

        public void TreatingOptimizeModel(Tuple<bool, string, string> respfromModel,
                                          string NomeArq, string Pasta, string Agora,
                                          GRBModel OptModelo, bool ShowMess, bool relax = false)
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
                if (RotaInstance != null)
                {
                    try
                    {
                        RotaInstance.Dispose();
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
