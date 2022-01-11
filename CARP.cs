using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;//.Extensions;
using Gurobi;
using System.Globalization; //Get Current Time
using System.Diagnostics; //Get time execution
using System.Drawing; //Allows Draw
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using System.CodeDom;
//using System.Runtime.Remoting.Messaging;

namespace Study_FH
{
    
    class Node
    {
        public int IndName { get; set; }
        public string StrName { get; set; }
        public double CoordX { get; set; }
        public double CoordY { get; set; }
        public bool Deposit { get; set; }

        public Node(double X, double Y, bool Dep = false)
        {
            CoordX = X;
            CoordY = Y;
            Deposit = Dep;
        }

        public void SetIndNameStrName(int index, string name = "00")
        {
            if (name == "00")
            {
                name = index.ToString();
            }
            IndName = index;
            StrName = name;
        }

        public override string ToString()
        {
            return "Node "+ StrName;
        }

        public override bool Equals(object obj)
        {
            return obj is Node node &&
                   //StrName == node.StrName &&
                   CoordX == node.CoordX &&
                   CoordY == node.CoordY &&
                   Deposit == node.Deposit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IndName, StrName, CoordX, CoordY, Deposit);
        }

        public static bool operator ==(Node left, Node right)
        {
            return EqualityComparer<Node>.Default.Equals(left, right);
        }

        public static bool operator !=(Node left, Node right)
        {
            return !(left == right);
        }

    }
        
    class Edge
    {
        public string EdgeName { get; set; }
        public int EdgeIndex { get; set; }
        public Node Node1 { get; set; }
        public Node Node2 { get; set; }
        public bool Required { get; set; }
        public double DemandByPeriod { get; set; }
        public double Cost { get; set; }
        public List<CombDays> PossibleCombinations { get; set; }

        public Edge(Node Start, Node End, double EstimatedCost, bool Req, double Demand)
        {
            Node1 = Start;
            Node2 = End;
            Required = Req;
            DemandByPeriod = Demand;
            Cost = EstimatedCost;
            EdgeName = $"({Start.StrName}, {End.StrName})";
            //NamesPossibleDays = new List<string>();
            PossibleCombinations = new List<CombDays>();
        }

        public override string ToString()
        {
            return "Edge " + EdgeName;
        }

        public static double CalcularTamanho(Node Start, Node End)
        {
            return ToolBox.Distancia2Pontos(Start.CoordX, End.CoordX, Start.CoordY, End.CoordY);
                       
        }

        public override bool Equals(object obj)
        {
            return obj is Edge edge &&
                   EdgeName == edge.EdgeName &&
                   EdgeIndex == edge.EdgeIndex &&
                   EqualityComparer<Node>.Default.Equals(Node1, edge.Node1) &&
                   EqualityComparer<Node>.Default.Equals(Node2, edge.Node2) &&
                   Required == edge.Required &&
                   DemandByPeriod == edge.DemandByPeriod &&
                   Cost == edge.Cost;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EdgeName, EdgeIndex, Node1, Node2, Required, DemandByPeriod, Cost);
        }

        public static bool operator ==(Edge left, Edge right)
        {
            return EqualityComparer<Edge>.Default.Equals(left, right);
        }

        public static bool operator !=(Edge left, Edge right)
        {
            return !(left == right);
        }

        public List<int> ListDaysInEdgeCombs()
        {
            List<int> Days = new List<int>();
            foreach (CombDays CD in PossibleCombinations)
            {
                Days = Days.Union(CD.ListComb).ToList();
            }
            return Days;
        }
    }
        
    class CombDays
    {
        public int IndexAllComb { get; set; }
        public string NameComb { get; set; }
        public List<int> ListComb { get; set; }
        [JsonIgnore]
        public Dictionary<int,int> IndexEdgeComb { get; set; }

        public CombDays(List<int> listComb)
        {
            NameComb = "C:";
            ListComb = listComb;
            ListComb.Sort();
            foreach (int k in listComb)
            {
                if(k == listComb.Last())
                {
                    NameComb += string.Format("{0}", k);
                }
                else
                {
                    NameComb += string.Format("{0}-", k);
                }
                
            }
            //NameComb += "-";
            //NameComb.Replace("--", null);
            IndexEdgeComb = new Dictionary<int, int>();
        }

        public CombDays(string name, List<int> listComb)
        {
            NameComb = name;
            ListComb = listComb;
            ListComb.Sort();
            IndexEdgeComb = new Dictionary<int, int>();
        }

        public override string ToString()
        {
            return "CD " + NameComb;
        }

        public override bool Equals(object obj)
        {
            return obj is CombDays days &&
                   //IndexAllComb == days.IndexAllComb &&
                   NameComb == days.NameComb &&
                   EqualityComparer<List<int>>.Default.Equals(ListComb, days.ListComb);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NameComb, ListComb);
        }

        public static bool operator ==(CombDays left, CombDays right)
        {
            return EqualityComparer<CombDays>.Default.Equals(left, right);
        }

        public static bool operator !=(CombDays left, CombDays right)
        {
            return !(left == right);
        }
    }
        
    class Vehicle
    {
        public string VehicleName { get; set; }
        public int VehicleIndex { get; set; }
        public double VehicleCapacity { get; set; }

        public Vehicle(int Ind, double Capacity)
        {
            VehicleName = Ind.ToString();
            VehicleIndex = Ind;
            VehicleCapacity = Capacity;
        }

        public override string ToString()
        {
            return "Vehicle " + VehicleName;
        }

        public override bool Equals(object obj)
        {
            return obj is Vehicle vehicle &&
                   VehicleName == vehicle.VehicleName &&
                   VehicleIndex == vehicle.VehicleIndex &&
                   VehicleCapacity == vehicle.VehicleCapacity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(VehicleName, VehicleIndex, VehicleCapacity);
        }
    }
    
    class GDBInstance
    {
        public string Name { get; set; }
        public int NumNodes { get; set; }
        public int NumEdges { get; set; }
        public int NumVehicles { get; set; }
        public int NumPeriods { get; set; }
        public List<Node> Nodes { get; set; }
        public List<Edge> Edges { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<CombDays> AllCombs { get; set; }
        public string Comment { get; set; }
        public string TypeCost { get; set; }
        public double TotalCost { get; set; }
        public List<int> Days { get; set; }
        [JsonIgnore]
        public double[,] CostMatrix { get; set; }
        [JsonIgnore]
        public double[,] DemandMatrix { get; set; }
        [JsonIgnore]
        public double[,,,] DemandQMatrix { get; set; }
        [JsonIgnore]
        public int[,] DaysInCombs { get; set; }
        [JsonIgnore]
        public List<Edge> RequiredEdges { get; set; }
        public GDBInstance(string _fileInst, string type = "gdb")
        {
            Nodes = new List<Node>();
            Vehicles = new List<Vehicle>();
            Edges = new List<Edge>();
            AllCombs = new List<CombDays>();
            switch (type)
            {
                case "gdb":
                    ReadInstanceGBD(_fileInst);
                    break;
                case "my":
                    ReadInstanceMy(_fileInst);
                    break;
                case "json":
                    ReadInstanceJson(_fileInst);
                    break;
            }
            SettingDaysInCombs();
            SettingCostAndDemandMatrix();
            ListingRequiredEdges();
        }
        public bool CheckSameList(CombDays CD)
        {//Test if any CD in AllCombs has the same ListComb
            bool resp = false;
            foreach(CombDays C in AllCombs)
            {
                bool test = CD.NameComb == C.NameComb;
                //MessageBox.Show("C\n"+C.ListComb.GetHashCode().ToString() + '\n' + C.NameComb + 
                //                "\nCD\n" + CD.ListComb.GetHashCode().ToString() + '\n'+ CD.NameComb +
                //                "\n Compara" + test.ToString());
                if (test)
                {
                    resp = true;
                }
            }
            return resp;
        }
        public int GetIndex(CombDays CD)
        {
            int val = -1; 
            if (CheckSameList(CD))
            {
                var IndexAllCombs = from co in AllCombs
                                    where co.NameComb == CD.NameComb
                                    select co;
                val = IndexAllCombs.First().IndexAllComb;
            }
            return val;
        }
        private void ListingRequiredEdges()
        {
            RequiredEdges = new List<Edge>();
            foreach (Edge E in Edges)
            {
                if (E.Required)
                {
                    RequiredEdges.Add(E);
                }
            }
        }
        public List<int> ListDaysInAllCombs()
        {
            List<int> Days = new List<int>();
            foreach (CombDays CD in AllCombs)
            {
                Days = Days.Union(CD.ListComb).ToList();
            }
            return Days;
        }
        private void SettingDaysInCombs()
        {
            Days = ListDaysInAllCombs();
            NumPeriods = Days.Count;
            DaysInCombs = new int[AllCombs.Count, NumPeriods];
            for (int d = 0; d < NumPeriods; d++)
            {
                var indexDay = Days[d];
                foreach (CombDays CD in AllCombs)
                {
                    if (CD.ListComb.Contains(indexDay))
                    {
                        DaysInCombs[CD.IndexAllComb, d] = 1;
                    }
                }
            }
        }
        private Node GetNodeByName(string name)
        {
            var pickN = from nd in Nodes
                        where nd.StrName == name
                        select nd;
            if (pickN.Count() > 0)
            {
                return pickN.First();
            }
            else
            {
                return null;
            }

        }
        private Edge GetEdgeByName(string name)
        {
            var pick = from ed in Edges
                        where ed.EdgeName == name
                        select ed;
            if (pick.Count() > 0)
            {
                return pick.First();
            }
            else
            {
                return null;
            }

        }
        private int GetObjIndexFromName(string name, string obj)
        {
            switch (obj) {
                case "Node":
                case "node":
                    var pickN = from nd in Nodes
                               where nd.StrName == name
                               select nd;
                    if (pickN.Count() > 0)
                    {
                        return pickN.First().IndName;
                    }
                    else
                    {
                        return -1;
                    }
                case "Edge":
                case "edge":
                    var pickE = from ed in Edges
                               where ed.EdgeName == name
                               select ed;
                    if (pickE.Count() > 0)
                    {
                        return pickE.First().EdgeIndex;
                    }
                    else
                    {
                        return -1;
                    }
                default:
                    return -1;
            }

        }   
        private void ReadInstanceJson(string fileInst)
        {
            try
            {
                _ = File.ReadAllText(fileInst);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                System.Windows.Forms.Application.Exit();
            }
            string[] jsonString = File.ReadAllText(fileInst).Replace(",\r", " ").Split('\n');
            char[] charSeparators = new char[] { '\t', ':', '\r', '"', ' ' };
            for (int line = 0; line < jsonString.Length; line++)
            {
                string[] CurrentLine = jsonString[line].TrimStart()
                                                       .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                switch (CurrentLine[0])
                {
                    case "Name":
                        Name = CurrentLine[1];
                        break;
                    case "NumNodes":
                        NumNodes = int.Parse(CurrentLine[1]);
                        break;
                    case "NumEdges":
                        NumEdges = int.Parse(CurrentLine[1]);
                        break;
                    case "NumVehicles":
                        NumVehicles = int.Parse(CurrentLine[1]);
                        break;
                    case "NumPeriods":
                        NumPeriods = int.Parse(CurrentLine[1]);
                        break;
                    case "Comment":
                        Comment = CurrentLine[1];
                        break;
                    case "TypeCost":
                        TypeCost = CurrentLine[1];
                        break;
                    case "TotalCost":
                        TotalCost = int.Parse(CurrentLine[1]);
                        break;
                    case "Nodes":
                        while (Nodes.Count < NumNodes)
                        {
                            line++;
                            string[] subline = jsonString[line].TrimStart()
                                                               .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                            int index = 0;
                            double X = 0;
                            double Y = 0;
                            bool dep = false;

                            while (subline[0] != "}")
                            {
                                switch (subline[0])
                                {
                                    case "IndName":
                                        index = int.Parse(subline[1]);
                                        break;
                                    case "CoordX":
                                        X = double.Parse(subline[1].Replace('.', ','));
                                        break;
                                    case "CoordY":
                                        Y = double.Parse(subline[1].Replace('.', ','));
                                        break;
                                    case "Deposit":
                                        dep = Convert.ToBoolean(subline[1]);
                                        break;
                                }
                                line++;
                                subline = jsonString[line].TrimStart()
                                                          .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                            }
                            Node NewNode = new Node(X, Y, dep);
                            if (!(Nodes.Contains(NewNode)))
                            {
                                NewNode.SetIndNameStrName(Nodes.Count(), subline[1]);
                                Nodes.Add(NewNode);
                            }                            
                        }
                        break;
                    case "Edges":
                        while (Edges.Count < NumEdges) {
                            bool goNext = true;
                            //Vars to create new Edge;
                            int NodeStart = 0;
                            int NodeEnd = 0;
                            double EstimatedCost = 0;
                            bool Req = false;
                            double Demand = 0;
                            int countvar = 0;
                            while (goNext)
                            {
                                line++;
                                string[] subline = jsonString[line].TrimStart()
                                                               .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                                switch (subline[0])
                                {
                                    case "Node1":
                                        line++;
                                        NodeStart = int.Parse(jsonString[line].TrimStart()
                                                               .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries)[1]);
                                        countvar++;
                                        while (jsonString[line].TrimStart()
                                                               .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries)[0] != "}")
                                        {
                                            line++;
                                        }
                                        break;
                                    case "Node2":
                                        line++;
                                        NodeEnd = int.Parse(jsonString[line].TrimStart()
                                                               .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries)[1]);
                                        countvar++;
                                        while (jsonString[line].TrimStart()
                                                               .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries)[0] != "}")
                                        {
                                            line++;
                                        }
                                        break;
                                    case "Required":
                                        Req = Convert.ToBoolean(subline[1]);
                                        countvar++;
                                        break;
                                    case "DemandByPeriod":
                                        Demand = double.Parse(subline[1].Replace('.', ','));
                                        countvar++;
                                        break;
                                    case "Cost":
                                        EstimatedCost = double.Parse(subline[1].Replace('.', ','));
                                        countvar++;
                                        break;
                                    case "PossibleDays":
                                        goNext = false;
                                        break;
                                }
                                if (countvar == 5)
                                {//All parameter was catched
                                    Edge NewEdge = new Edge(Nodes[NodeStart], Nodes[NodeEnd], EstimatedCost, Req, Demand);
                                    if (!(Edges.Contains(NewEdge)))
                                    {
                                        Edges.Add(NewEdge);
                                    }                                    
                                    Edges[^1].EdgeIndex = Edges.Count - 1;
                                    countvar = 0;
                                }
                            }
                            //Catching CombDays
                            char[] combcharSeparators = new char[] { 'C', ':', '-' };
                            while (jsonString[line + 2].TrimStart()
                                                   .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries)[0] == "NameComb")
                            {
                                line += 2;
                                string[] testcombs = jsonString[line].TrimStart()
                                                   .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                                // int testeteste = 0;
                                string NameComb = string.Format("{0}:{1}", testcombs[1], testcombs[2]);
                                List<int> LComb = testcombs[2].Split(combcharSeparators, StringSplitOptions.RemoveEmptyEntries)
                                                     .Where(x => int.TryParse(x, out _))
                                                     .Select(int.Parse)
                                                     .ToList();
                                CombDays CD = new CombDays(LComb);
                                //Avoiding repeat values
                                //Verify If ListComb in any AllCombs CombDays
                                var IndCD = GetIndex(CD);

                                if (IndCD < 0)
                                {//Only add new CombItem to Instance's Comblist
                                    AllCombs.Add(CD);
                                    CD.IndexAllComb = AllCombs.Count - 1;
                                }
                                else
                                {
                                    CD = AllCombs[IndCD];
                                }
                                /*
                                var NamesAllCombs = from co in AllCombs
                                                    where co == CD
                                                    select co;

                                if (NamesAllCombs.Count() == 0)
                                {//Only add new CombItem to Instance's Comblist
                                    AllCombs.Add(CD);
                                    CD.IndexAllComb = AllCombs.Count - 1;
                                }*/

                                //Avoiding repeat values
                                //Verify If ListComb in any AllCombs CombDays
                                var NameECombs = from co in Edges[^1].PossibleCombinations
                                                 where co == CD
                                                 select co;

                                if (NameECombs.Count() == 0)
                                {//Only add new CombItem to Instance's Comblist
                                    Edges[^1].PossibleCombinations.Add(CD);
                                    CD.IndexEdgeComb.Add(Edges.Count - 1, Edges[^1].PossibleCombinations.Count - 1);
                                }
                                line += 3 + LComb.Count;
                            }
                        }
                        break;
                    case "Vehicles":
                        while (Vehicles.Count < NumVehicles)
                        {
                            line++;
                            string[] subline = jsonString[line].TrimStart()
                                                               .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                            int index = 0;
                            double Capac = 0;

                            while (subline[0] != "}")
                            {
                                switch (subline[0])
                                {
                                    case "VehicleIndex":
                                        index = int.Parse(subline[1]);
                                        break;
                                    case "VehicleCapacity":
                                        Capac = double.Parse(subline[1]);
                                        break;
                                }
                                line++;
                                subline = jsonString[line].TrimStart()
                                                          .Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                            }
                            Vehicle NewVehicle = new Vehicle(index, Capac);
                            if (!(Vehicles.Contains(NewVehicle)))
                            {
                                Vehicles.Add(NewVehicle);
                            }
                        }
                        break;
                }
            }
        }
        public void ReadInstanceGBD(string _fileInst)
        {
            try
            {
                string[] AllFile = File.ReadAllLines(_fileInst);                                            
                bool ReqEdge = true;
                int NumLines = AllFile.Length;
                char[] charSeparators = new char[] { ',', ' ', ':', '(', ')', '[', ']' };
                for (int i = 0; i < NumLines; i++)
                {
                    string CurrentLine = AllFile[i].Trim();
                    string[] SplitLine = CurrentLine.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);


                    if (CurrentLine.StartsWith("[")) //Node List chu2
                    {
                        double X = Convert.ToDouble(SplitLine[1]);
                        double Y = Convert.ToDouble(SplitLine[2]);
                        Node NewNode = new Node(X, Y);
                        if (!(Nodes.Contains(NewNode)))
                        {
                            NewNode.SetIndNameStrName(Nodes.Count, SplitLine[0]);
                            Nodes.Add(NewNode);
                        }
                    }
                    else if (CurrentLine.StartsWith("("))
                    {
                        //This is a requerid arest
                        Node NodeStart = GetNodeByName(SplitLine[0]);
                        Node NodeEnd = GetNodeByName(SplitLine[1]);
                        //int IndNodeStart = int.Parse(SplitLine[0]); //-1 if start at 1
                        //int IndNodeEnd = int.Parse(SplitLine[1]); // - 1 if start at 1
                        double Cost = Edge.CalcularTamanho(NodeStart, NodeEnd); //int.Parse(SplitLine[3]);
                        //double Cost = Edge.CalcularTamanho(Nodes[IndNodeStart], Nodes[IndNodeEnd]); //int.Parse(SplitLine[3]);
                        double Dem = double.Parse(SplitLine[5]);
                        List<CombDays> Comb = new List<CombDays>();
                        List<string> CDNames = new List<string>();
                        if (SplitLine.Length > 6)
                        {
                            for (int k = 7; k < SplitLine.Length; k++)
                            {
                                List<int> CombItem = SplitLine[k].Split('-')
                                                             .Where(x => int.TryParse(x, out _))
                                                             .Select(int.Parse)
                                                             .ToList();

                                CombDays CD = new CombDays(CombItem);
                                var IndCD = GetIndex(CD);
                            
                                if (IndCD < 0)
                                {//Only add new CombItem to Instance's Comblist
                                    AllCombs.Add(CD);
                                    CD.IndexAllComb = AllCombs.Count - 1;
                                }
                                else
                                {
                                    CD = AllCombs[IndCD];
                                }
                                //MessageBox.Show("AllCombs " + AllCombs.Count.ToString());
                                if (!(Comb.Contains(CD)))
                                {
                                    Comb.Add(CD); //Add to Edge's CombList
                                }
                            }
                        }
                        Edge NewEdge = new Edge(NodeStart, NodeEnd, Cost, ReqEdge, Dem);
                        //Edge NewEdge = new Edge(Nodes[IndNodeStart], Nodes[IndNodeEnd], Cost, ReqEdge, Dem);
                        if (!(Edges.Contains(NewEdge)))
                        {
                            Edges.Add(NewEdge);
                            Edges[^1].EdgeIndex = Edges.Count - 1;
                            Edges[^1].PossibleCombinations = Comb;
                            //Filling PossibleCombinations Indexes
                            int countcomb = 0;
                            foreach (CombDays C in Edges[^1].PossibleCombinations)
                            {
                                C.IndexEdgeComb.Add(Edges.Count - 1, countcomb);
                                countcomb++;
                            }
                        }                    
                    }
                    else
                    {
                        //Some parameter to process
                        switch (SplitLine[0].ToString())
                        {
                            case "NOMBRE":
                                Name = SplitLine[1].ToString();
                                break;
                            case "VERTICES":
                                if (_fileInst.Contains("chu1"))
                                {
                                    NumNodes = int.Parse(SplitLine[1]);
                                    List<List<double>> NodeList = ToolBox.CriarListaDePontos(NumNodes, 2, 25, 475);
                                    //List<List<double>> NodeList = ToolBox.CriarListaDePontosCirc(NumNodes, 25, 475);
                                    //List<List<double>> NodeList = ToolBox.CriarListaDePontosRect(NumNodes, 15, 15, 450, 450);
                                    for (int n = 0; n < NumNodes; n++)
                                    {
                                        double X = NodeList[n][0];
                                        double Y = NodeList[n][1];
                                        Node NewNode = new Node(X, Y);
                                        if (!(Nodes.Contains(NewNode)))
                                        {
                                            //NewNode.SetIndNameStrName(Nodes.Count);
                                            Nodes.Add(NewNode);
                                            NewNode.SetIndNameStrName(Nodes.Count - 1, Nodes.Count.ToString());
                                        }
                                    }
                                }
                                break;
                            case "VEHICULOS":
                                NumVehicles = int.Parse(SplitLine[1]);
                                for (int v = 0; v < NumVehicles; v++)
                                {
                                    Vehicle NewVehicle = new Vehicle(v, 0);
                                    if (!(Vehicles.Contains(NewVehicle)))
                                    {
                                        Vehicles.Add(NewVehicle);
                                    }
                                }
                                break;
                            case "CAPACIDAD":
                                int capac = int.Parse(SplitLine[1]);
                                foreach (Vehicle v in Vehicles)
                                {
                                    v.VehicleCapacity = capac;
                                }
                                break;
                            case "COMENTARIO":
                                Comment = CurrentLine[(SplitLine[0].Length + 3)..];
                                break;
                            case "TIPO_COSTES_ARISTAS":
                                TypeCost = SplitLine[1];
                                break;
                            case "COSTE_TOTAL_REQ":
                                TotalCost = double.Parse(SplitLine[1]);
                                break;
                            case "LISTA_ARISTAS_REQ":
                                ReqEdge = true;
                                break;
                            case "LISTA_ARISTAS_NO_REQ":
                                ReqEdge = false;
                                break;
                            case "DEPOSITO":
                                //int NodeDep = int.Parse(SplitLine[1]);
                                //Nodes[NodeDep+1].Deposit = true;
                                Node NodeDeposit = GetNodeByName(SplitLine[1]);
                                NodeDeposit.Deposit = true;
                                break;
                            case "PERIODS":
                                NumPeriods = int.Parse(SplitLine[1]);
                                break;
                        }
                    }
                }
                NumEdges = Edges.Count();
                NumNodes = Nodes.Count();
                NumVehicles = Vehicles.Count();
            }
            catch (System.IO.FileNotFoundException e) when (e.Data != null)
            {
                MessageBox.Show("Arquivo " + _fileInst + " não encontrado.", "Read from");
            }
        }
        public void ReadInstanceMy(string _fileInst)
        {
            string[] AllFile = File.ReadAllLines(_fileInst);
            //int NumLines = AllFile.Length;
            char[] charSeparators = new char[] { '\t' };
            string NextLine = "parameter";
            for (int i = 0; i < AllFile.Length; i++)
            {
                string CurrentLine = AllFile[i].TrimStart();
                string[] SplitLine = CurrentLine.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                switch (NextLine)
                {
                    case "parameter":
                        switch (SplitLine[0].ToString())
                        {
                            case "NOMBRE":
                                Name = SplitLine[1].ToString();
                                break;
                            case "COMENTARIO":
                                Comment = SplitLine[1];
                                break;
                            case "TIPO_COSTES_ARISTAS":
                                TypeCost = SplitLine[1];
                                break;
                            case "COSTE_TOTAL_REQ":
                                TotalCost = double.Parse(SplitLine[1]);
                                break;
                            case "Vertices":
                                NumNodes = int.Parse(SplitLine[1]);
                                NextLine = "Title Vertices";
                                break;
                            case "Vehicles":
                                NumVehicles = int.Parse(SplitLine[1]);
                                NextLine = "Title Vehicles";
                                break;
                            case "Edges":
                                NumEdges = int.Parse(SplitLine[1]);
                                NextLine = "Title Edges";
                                break;
                        }
                        break;
                    case "Title Vertices":
                        NextLine = "Vertices";
                        break;
                    case "Title Vehicles":
                        NextLine = "Vehicles";
                        break;
                    case "Title Edges":
                        //Cathing Combinations
                        char[] combcharSeparators = new char[] { 'C', ':', '-' };
                        if (SplitLine.Length > 9)
                        {
                            for (int c = 9; c < SplitLine.Length; c++)
                            {
                                string comb = SplitLine[c];
                                List<int> iterComb = comb.Split(combcharSeparators, StringSplitOptions.RemoveEmptyEntries)
                                                         .Where(x => int.TryParse(x, out _))
                                                         .Select(int.Parse)
                                                         .ToList();
                                CombDays CD = new CombDays(comb, iterComb);
                                //Avoiding repeat values
                                //Verify If ListComb in any AllCombs CombDays
                                var NameAllCombs = from co in AllCombs
                                                   where co == CD
                                                   select co;

                                if (NameAllCombs.Count() == 0)
                                {//Only add new CombItem to Instance's Comblist
                                    AllCombs.Add(CD);
                                    CD.IndexAllComb = AllCombs.Count - 1;
                                }
                            }
                        }
                        NextLine = "Edges";
                        break;
                    case "Vertices":
                        Node NewNode = new Node(
                                           Convert.ToDouble(SplitLine[1]),
                                           Convert.ToDouble(SplitLine[2]),
                                           Convert.ToBoolean(SplitLine[3]));
                        if (!(Nodes.Contains(NewNode)))
                        {
                            NewNode.SetIndNameStrName(Nodes.Count, SplitLine[0]);
                            Nodes.Add(NewNode);
                        }
                        if (Nodes.Count < NumNodes)
                        {
                            NextLine = "Vertices";
                        }
                        else
                        {
                            NextLine = "parameter";
                        }
                        break;
                    case "Vehicles":
                        Vehicle NewVehicle = new Vehicle(int.Parse(SplitLine[0]),
                                                 Convert.ToDouble(SplitLine[1]));
                        if (!(Vehicles.Contains(NewVehicle)))
                        {
                            Vehicles.Add(NewVehicle);
                        }
                        if (Vehicles.Count < NumVehicles)
                        {
                            NextLine = "Vehicles";
                        }
                        else
                        {
                            NextLine = "parameter";
                        }
                        break;
                    case "Edges":
                        Edge NewEdge = new Edge(Nodes[int.Parse(SplitLine[2])],
                                           Nodes[int.Parse(SplitLine[4])],
                                           Convert.ToDouble(SplitLine[8]),
                                           Convert.ToBoolean(SplitLine[6]),
                                           Convert.ToDouble(SplitLine[7]));
                        if (!(Edges.Contains(NewEdge)))
                        {
                            Edges.Add(NewEdge);
                        }
                        if (Edges.Count < NumEdges)
                        {
                            NextLine = "Edges";
                        }
                        else
                        {
                            NextLine = "parameter";
                        }
                        break;
                }
            }
        }
        public void SettingCostAndDemandMatrix()
        {
            CostMatrix = new double[NumEdges, NumEdges];
            DemandMatrix = new double[NumEdges, NumEdges];
            DemandQMatrix = new double[NumEdges, NumEdges, AllCombs.Count, NumPeriods];
            foreach (Edge E in Edges)
            {
                int E1 = E.Node1.IndName;
                int E2 = E.Node2.IndName;
                CostMatrix[E1, E2] = E.Cost;
                CostMatrix[E2, E1] = E.Cost;
                DemandMatrix[E1, E2] = E.DemandByPeriod;
                DemandMatrix[E2, E1] = E.DemandByPeriod;
                foreach(CombDays CD in E.PossibleCombinations)
                {
                    int CDi = CD.IndexAllComb;
                    for(int p=0; p<NumPeriods; p++)
                    {
                        //if (CD.ListComb.Contains(Days[p]))
                        //{
                            DemandQMatrix[E1, E2, CDi, p] = E.DemandByPeriod;
                        //}
                    }
                }
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
            _ = title.Append(string.Format("Vertices\t{0}\n", NumNodes));
            _ = title.Append("Name \t X \t Y \t Deposit \n");
            foreach (Node N in Nodes)
            {
                _ = title.Append(string.Format("{0}\t{1}\t{2}\t{3}\n", N.StrName, N.CoordX, N.CoordY, N.Deposit));
            }
            _ = title.Append(string.Format("Vehicles\t{0}\n", NumVehicles));
            _ = title.Append("Name \t Capacity \n");
            foreach (Vehicle V in Vehicles)
            {
                _ = title.Append(string.Format("{0}\t{1}\n", V.VehicleName, V.VehicleCapacity));
            }
            _ = title.Append(string.Format("Edges\t{0}\n", NumEdges));
            _ = title.Append("EdgeName\tEdgeIndex\tStartNode\tIsStartNodeDeposit\t");
            _ = title.Append("EndNode\tIsEndNodeDeposit\tRequired\tDemandByPeriod\tCost\t");

            //Just Combs Names
            var NameAllCombs = from co in AllCombs
                               select co.NameComb;

            foreach (string CD in NameAllCombs)
            {
                _ = title.Append(CD + " \t");
            }
            byte[] text = new UTF8Encoding(true).GetBytes(title.ToString() + '\n');
            fs.Write(text, 0, text.Length);
            foreach (Edge E in Edges)
            {
                StringBuilder content = new StringBuilder();
                _ = content.Append(string.Format("{0}\t", E.EdgeName));
                _ = content.Append(string.Format("{0}\t", E.EdgeIndex));
                _ = content.Append(string.Format("{0}\t", E.Node1.StrName));
                _ = content.Append(string.Format("{0}\t", E.Node1.Deposit));
                _ = content.Append(string.Format("{0}\t", E.Node2.StrName));
                _ = content.Append(string.Format("{0}\t", E.Node2.Deposit));
                _ = content.Append(string.Format("{0}\t", E.Required));
                _ = content.Append(string.Format("{0}\t", E.DemandByPeriod));
                _ = content.Append(string.Format("{0}\t", E.Cost));

                var NameECombs = from co in E.PossibleCombinations
                                 select co.NameComb;

                foreach (string CDName in NameAllCombs)
                {
                    if (NameECombs.Contains(CDName))
                    {
                        _ = content.Append("True\t");
                    }
                    else
                    {
                        _ = content.Append("False\t");
                    }
                }
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
            foreach (Node N in Nodes)
            {
                float X = Convert.ToSingle(N.CoordX) - PointRadius;
                float Y = Convert.ToSingle(N.CoordY) - PointRadius;
                PointF Top = new PointF(X, Y);
                SizeF Tam = new SizeF(2 * PointRadius, 2 * PointRadius);
                RectangleF Rec = new RectangleF(Top, Tam);
                g.DrawEllipse(drawPen, Rec);

                PointF Cnt = new PointF(X + PointRadius * 13 / 10, Y + PointRadius * 13 / 10);
                g.DrawString(N.StrName, drawFont2, drawBrush, Cnt, format1);
            }

            //Desenho das Arestas
            foreach (Edge E in Edges)
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
        public IEnumerable<Edge> PickEdgeList(Node N1, Node N2)
        {
            foreach (var pe in Edges)
            {
                if (pe.Node1 == N1 && pe.Node2 == N2)
                {
                    yield return pe;
                }
            }
        }
        public bool CheckIsEdge(Node N1, Node N2)
        {//True if Edge (N1,N2) in Edges. False otherwise
            return PickEdgeList(N1, N2).ToList().Count() > 0;
        }
        public int IndexEdge(Node N1, Node N2)
        {
            List<Edge> pick_edge = PickEdgeList(N1, N2).ToList();
            if (pick_edge.Count() == 1)
            {
                return pick_edge.First().EdgeIndex;
            }
            else
            {
                return -1;
            }
        }
        public bool CheckIsEdgeRequired(Node N1, Node N2)
        {//True if Edge (N1,N2) in Edges and is required. False otherwise
            bool resp = false;
            List<Edge> pick_edge = PickEdgeList(N1, N2).ToList();
            int len = pick_edge.Count();
            //bool ver = len > 0;
            if (len > 0)
            {
                resp = pick_edge.First().Required;
            } 
            return resp;
        }
    }

    class CARP
    {
        //Variaveis de geracao do modelo Gurobi
        public  GRBEnv Ambiente; //{ get; set; }
        public GRBModel Modelo; //{ get; set; }

        //Variável de contagem do tempo de execução
        public Stopwatch Cronometro; //{ get; set; }

        //Parametros do Problema;
        public GDBInstance Instance;
        public string Pasta = ToolBox.Path(@"Projects\Chu_1\ModelosLP\");

        public void SolveModel(string NomeArquivo, bool ShowMess)
        {
            string Agora = ToolBox.GetNow();

            //Arquivos do Modelo
            ///string NomeArquivo = "pChu";
            Ambiente = new GRBEnv();

            Modelo = new GRBModel(Ambiente)
            {
                ModelSense = GRB.MINIMIZE
            };

            // Must set LazyConstraints parameter when using lazy constraints
            Modelo.Parameters.LazyConstraints = 1;

            List<(string, int)> IndOrd;
            List<(string, int)> IndInv;

            //Criar Variáveis
            GRBVar[,,,] X = new GRBVar[Instance.NumNodes, Instance.NumNodes, Instance.NumVehicles, Instance.NumPeriods];
            GRBVar[,,,,] L = new GRBVar[Instance.NumNodes, Instance.NumNodes, Instance.NumVehicles, Instance.AllCombs.Count, Instance.NumPeriods];
            GRBVar[,,] Z = new GRBVar[Instance.NumNodes, Instance.NumNodes, Instance.AllCombs.Count];
            //Definir tipo das variaveis e coeficientes da F.O.

            for(int i=0; i<Instance.NumNodes; i++)
            {
                for (int j = i; j < Instance.NumNodes; j++)
                {
                    for (int k = 0; k < Instance.AllCombs.Count; k++)
                    {
                        IndOrd = new List<(string, int)> { ("node", i), ("node", j), ("comb", k) };
                        IndInv = new List<(string, int)> { ("node", j), ("node", i), ("comb", k) };
                        string Zname = ToolBox.ConstraintName("Z", IndOrd);
                        Z[i, j, k] = Modelo.AddVar(0, 1, 0, GRB.BINARY, Zname);
                        Zname = ToolBox.ConstraintName("Z", IndInv);
                        Z[j, i, k] = Modelo.AddVar(0, 1, 0, GRB.BINARY, Zname);
                    }
                    for (int v = 0; v < Instance.NumVehicles; v++)
                    {
                        for (int p = 0; p < Instance.NumPeriods; p++)
                        {
                            IndOrd = new List<(string, int)> { ("node", i), ("node", j), ("vehi", v), ("period", p) };
                            IndInv = new List<(string, int)> { ("node", j), ("node", i), ("vehi", v), ("period", p) };
                            string Xname = ToolBox.ConstraintName("X", IndOrd);
                            X[i, j, v, p] = Modelo.AddVar(0, 1, Instance.CostMatrix[i, j], GRB.BINARY, Xname);
                            Xname = ToolBox.ConstraintName("X", IndInv);
                            X[j, i, v, p] = Modelo.AddVar(0, 1, Instance.CostMatrix[i, j], GRB.BINARY, Xname);
                            for (int k = 0; k < Instance.AllCombs.Count; k++)
                            {
                                IndOrd = new List<(string, int)> { ("node", i), ("node", j), ("vehi", v), ("comb", k), ("period", p) };
                                IndInv = new List<(string, int)> { ("node", j), ("node", i), ("vehi", v), ("comb", k), ("period", p) };
                                string Lname = ToolBox.ConstraintName("L",IndOrd);
                                L[i, j, v, k, p] = Modelo.AddVar(0, 1, 0, GRB.BINARY, Lname);
                                Lname = ToolBox.ConstraintName("L", IndInv);
                                L[j, i, v, k, p] = Modelo.AddVar(0, 1, 0, GRB.BINARY, Lname);
                            }
                        }   
                    }
                }
            }
            /*    
            //foreach (Edge Ed in Instance.Edges)
            //{
                int i = Ed.Node1.IndName;
                int j = Ed.Node2.IndName;
                foreach (CombDays CD in Ed.PossibleCombinations)
                {
                    int k = CD.IndexAllComb;
                    StringBuilder Zname = new StringBuilder();
                    Zname.Append("z_");
                    Zname.Append(i.ToString() + "_");
                    Zname.Append(j.ToString() + "_");
                    Zname.Append(k.ToString());
                    Z[i, j, k] = Modelo.AddVar(0, 1, 0, GRB.BINARY, Zname.ToString());
                }
                foreach (Vehicle Ve in Instance.Vehicles)
                {
                    int v = Ve.VehicleIndex;
                    for (int p = 0; p < Instance.NumPeriods; p++)
                    {
                        StringBuilder Xname = new StringBuilder();
                        Xname.Append("x_");
                        Xname.Append(i.ToString() + "_");
                        Xname.Append(j.ToString() + "_");
                        Xname.Append(v.ToString() + "_");
                        Xname.Append(p.ToString());
                        X[i, j, v, p] = Modelo.AddVar(0, 1, Instance.CostMatrix[i, j], GRB.BINARY, Xname.ToString());
                        Xname.Clear();
                        Xname.Append("x_");
                        Xname.Append(j.ToString() + "_");
                        Xname.Append(i.ToString() + "_");
                        Xname.Append(v.ToString() + "_");
                        Xname.Append(p.ToString());
                        X[j, i, v, p] = Modelo.AddVar(0, 1, Instance.CostMatrix[i, j], GRB.BINARY, Xname.ToString());
                        foreach (CombDays CD in Ed.PossibleCombinations)
                        {
                            int k = CD.IndexAllComb;
                            StringBuilder Lname = new StringBuilder();
                            Lname.Append("l_");
                            Lname.Append(i.ToString() + "_");
                            Lname.Append(j.ToString() + "_");
                            Lname.Append(v.ToString() + "_");
                            Lname.Append(k.ToString() + "_");
                            Lname.Append(p.ToString());
                            L[i, j, v, k, p] = Modelo.AddVar(0, 1, 0, GRB.BINARY, Lname.ToString());
                            Lname.Clear();
                            Lname.Append("l_");
                            Lname.Append(j.ToString() + "_");
                            Lname.Append(i.ToString() + "_");
                            Lname.Append(v.ToString() + "_");
                            Lname.Append(k.ToString() + "_");
                            Lname.Append(p.ToString());
                            L[j, i, v, k, p] = Modelo.AddVar(0, 1, 0, GRB.BINARY, Lname.ToString());
                        }
                    }
                }
            */ //}

            //Restricao Z
            GRBLinExpr expr1 = new GRBLinExpr();
            GRBLinExpr expr2 = new GRBLinExpr();
            foreach (Edge EReq in Instance.RequiredEdges)
            {
                int i = EReq.Node1.IndName;
                int j = EReq.Node2.IndName;
                StringBuilder Constrname = new StringBuilder();
                Constrname.Append("R2_" + EReq.EdgeName.Replace(" ",""));
                expr1.Clear();
                foreach (CombDays CD in EReq.PossibleCombinations)
                {
                    int k = CD.IndexAllComb;
                    expr1.AddTerm(1, Z[i, j, k]);
                }
                Modelo.AddConstr(expr1 == 1, Constrname.ToString());
            }

            //Restricao 3, O total de entradas em um Nó deve ser o total de saídas
            //Garante a continuidade do fluxo
            foreach (Node No in Instance.Nodes)
            {
                int i = No.IndName;
                foreach (Vehicle Ve in Instance.Vehicles)
                {
                    int v = Ve.VehicleIndex;
                    for (int p = 0; p < Instance.NumPeriods; p++)
                    {
                        StringBuilder Constrname = new StringBuilder();
                        Constrname.Append("R3_");
                        Constrname.Append(i.ToString() + "_");
                        Constrname.Append(v.ToString() + "_");
                        Constrname.Append(p.ToString());
                        expr1.Clear();
                        foreach (Edge Ed in Instance.Edges)
                        {
                            if (No == Ed.Node1)
                            {
                                int j = Ed.Node2.IndName;
                                expr1.AddTerm(1, X[i, j, v, p]);
                            } else if (No == Ed.Node2)
                            {
                                int j = Ed.Node1.IndName;
                                expr1.AddTerm(-1, X[j, i, v, p]);
                            }
                        }
                        Modelo.AddConstr(expr1 == 0, Constrname.ToString());
                    }
                }
            }

            //Restrição 4
            foreach (Edge EReq in Instance.RequiredEdges)
            {
                int i = EReq.Node1.IndName;
                int j = EReq.Node2.IndName;
                foreach(Vehicle Ve in Instance.Vehicles)
                {
                    int v = Ve.VehicleIndex;
                    for (int p = 0; p < Instance.NumPeriods; p++)// in EReq.ListDaysInEdgeCombs())
                    {
                        StringBuilder Constrname = new StringBuilder();
                        Constrname.Append("R4a_");
                        Constrname.Append(i.ToString() + "_");
                        Constrname.Append(j.ToString() + "_");
                        Constrname.Append(v.ToString() + "_");
                        Constrname.Append(p.ToString());
                        expr1.Clear();
                        StringBuilder Constrname2 = new StringBuilder();
                        Constrname2.Append("R4b_");
                        Constrname2.Append(j.ToString() + "_");
                        Constrname2.Append(i.ToString() + "_");
                        Constrname2.Append(v.ToString() + "_");
                        Constrname2.Append(p.ToString());
                        expr2.Clear();
                        expr1.AddTerm(-1, X[i, j, v, p]);
                        expr2.AddTerm(-1, X[j, i, v, p]);
                        foreach (CombDays CD in EReq.PossibleCombinations)
                        {
                            int k = CD.IndexAllComb;
                            expr1.AddTerm(1, L[i, j, v, k, p]);
                            expr2.AddTerm(1, L[j, i, v, k, p]);
                        }
                        Modelo.AddConstr(expr1 <= 0, Constrname.ToString());
                        Modelo.AddConstr(expr2 <= 0, Constrname2.ToString());
                    }
                }
            }

            //Restrição 5
            foreach (Edge EReq in Instance.RequiredEdges)
            {
                int i = EReq.Node1.IndName;
                int j = EReq.Node2.IndName;
                foreach (CombDays CD in EReq.PossibleCombinations)
                {
                    int k = CD.IndexAllComb;
                    for (int p = 0; p < Instance.NumPeriods; p++)
                    {
                        StringBuilder Constrname = new StringBuilder();
                        Constrname.Append("R5_");
                        Constrname.Append(i.ToString() + "_");
                        Constrname.Append(j.ToString() + "_");
                        Constrname.Append(k.ToString() + "_");
                        Constrname.Append(p.ToString());
                        expr1.Clear();
                        foreach (Vehicle Ve in Instance.Vehicles)
                        {
                            int v = Ve.VehicleIndex;
                            expr1.AddTerm(1, L[i, j, v, k, p]);
                            expr1.AddTerm(1, L[j, i, v, k, p]);
                        }
                        expr1.AddTerm(-Instance.DaysInCombs[k, p], Z[i, j, k]);
                        Modelo.AddConstr(expr1 ==  0, Constrname.ToString());
                    }
                }
            }

            //Restricao 6
            //Capacidade veiculo
            foreach (Vehicle Ve in Instance.Vehicles)
            {
                int v = Ve.VehicleIndex;
                for(int p = 0; p<Instance.NumPeriods; p++)
                {
                    StringBuilder Constrname = new StringBuilder();
                    Constrname.Append("R6_");
                    Constrname.Append(v.ToString() + "_");
                    Constrname.Append(p.ToString());
                    expr1.Clear();
                    foreach (Edge EReq in Instance.RequiredEdges)
                    {
                        int i = EReq.Node1.IndName;
                        int j = EReq.Node2.IndName;
                        foreach (CombDays CD in EReq.PossibleCombinations)
                        {
                            int k = CD.IndexAllComb;
                            //StringBuilder aval3 = new StringBuilder();
                            //aval3.Append("v = ");
                            //aval3.Append(v);
                            //aval3.Append('\t');
                            //aval3.Append("p = ");
                            //aval3.Append(p);
                            //aval3.Append('\t');
                            //aval3.Append(k);
                            //aval3.Append('\t');
                            //aval3.Append("Q ");
                            //aval3.Append(i);
                            //aval3.Append(j);
                            //aval3.Append(k);
                            //aval3.Append(p);
                            //aval3.Append('\t');
                            //aval3.Append(Instance.DemandQMatrix[i, j, k, p]);
                            //aval3.Append('\t');
                            //aval3.Append("L ");
                            //aval3.Append(i);
                            //aval3.Append(j);
                            //aval3.Append(v);
                            //aval3.Append(k);
                            //aval3.Append(p);
                            //aval3.Append('\t');
                            //aval3.Append("L ");
                            //aval3.Append(j);
                            //aval3.Append(i);
                            //aval3.Append(v);
                            //aval3.Append(k);
                            //aval3.Append(p);
                            //String aval4 = aval3.ToString();
                            expr1.AddTerm(Instance.DemandQMatrix[i, j, k, p], L[i, j, v, k, p]);
                            expr1.AddTerm(Instance.DemandQMatrix[i, j, k, p], L[j, i, v, k, p]);
                        }
                    }
                    if (expr1.Size >0)
                    {
                        Modelo.AddConstr(expr1 <= Ve.VehicleCapacity, Constrname.ToString());
                    }                    
                }
            }
            
            //Restrição 7
            Modelo.SetCallback(new Tsp_cs(X));

            //Escrita do Modelo

            //Escrever Modelo .lp
            ToolBox.GravaModelo("PL", Pasta, NomeArquivo, Agora, Modelo, ShowMess);

            Cronometro = new Stopwatch();
            Cronometro.Start();
            //Otimizar
            Modelo.Optimize();
            Cronometro.Stop();

            //Teste de Erro
            if (!(ToolBox.AvaliaModelo(Modelo.Status, ShowMess).Item1))
            {
                System.Environment.Exit(1);
            }

            StringBuilder _namefile = new StringBuilder();
            _namefile.Append(Pasta);
            _namefile.Append(NomeArquivo);
            _namefile.Append(Agora);
            _namefile.Append("_SolManual.txt");

            List<string> VarsSol = new List<string>
            {
                "Var \t Val"
            };

            for (int i = 0; i < Instance.NumNodes; i++)
            {
                for (int j = i; j < Instance.NumNodes; j++)
                {
                    for (int k = 0; k < Instance.AllCombs.Count; k++)
                    {
                        if (Z[i, j, k].X >0)
                        {
                            IndOrd = new List<(string, int)> { ("node", i), ("node", j), ("comb", k) };
                            VarsSol.Add(ToolBox.ConstraintName("Z", IndOrd) + '\t'+ Z[i, j, k].X.ToString());
                        }
                        if (Z[j, i, k].X > 0)
                        {
                            IndInv = new List<(string, int)> { ("node", j), ("node", i), ("comb", k) };
                            VarsSol.Add(ToolBox.ConstraintName("Z", IndInv) + '\t' + Z[j, i, k].X.ToString());
                        }
                    }
                    for (int v = 0; v < Instance.NumVehicles; v++)
                    {
                        for (int p = 0; p < Instance.NumPeriods; p++)
                        {
                            if (X[i, j, v, p].X > 0)
                            {
                                IndOrd = new List<(string, int)> { ("node", i), ("node", j), ("vehi", v), ("period", p) };
                                VarsSol.Add(ToolBox.ConstraintName("X", IndOrd) + '\t' + X[i, j, v, p].X.ToString());
                            }
                            if (X[j, i, v, p].X > 0)
                            {
                                IndInv = new List<(string, int)> { ("node", j), ("node", i), ("vehi", v), ("period", p) };
                                VarsSol.Add(ToolBox.ConstraintName("X", IndInv) + '\t' + X[j, i, v, p].X.ToString());
                            }
                            for (int k = 0; k < Instance.AllCombs.Count; k++)
                            {
                                if (L[i, j, v, k, p].X > 0)
                                {
                                    IndOrd = new List<(string, int)> { ("node", i), ("node", j), ("vehi", v), ("comb", k), ("period", p) };
                                    VarsSol.Add(ToolBox.ConstraintName("L", IndOrd) + '\t' + L[i, j, v, k, p].X.ToString());
                                }
                                if (L[j, i, v, k, p].X > 0)
                                {
                                    IndInv = new List<(string, int)> { ("node", j), ("node", i), ("vehi", v), ("comb", k), ("period", p) };
                                    VarsSol.Add(ToolBox.ConstraintName("L", IndInv) + '\t' + L[j, i, v, k, p].X.ToString());
                                }
                            }
                        }
                    }
                }
            }

            using (StreamWriter fs = new StreamWriter(_namefile.ToString()))
            {
                foreach(var line in VarsSol)
                {
                    fs.WriteLine(line);
                }
            }

            //Escrever Solução
            ToolBox.GravaModelo("SOL", Pasta, NomeArquivo, Agora, Modelo, ShowMess);
            //Escrever MPS
            ToolBox.GravaModelo("MPS", Pasta, NomeArquivo, Agora, Modelo, ShowMess);
        }
    }
}
