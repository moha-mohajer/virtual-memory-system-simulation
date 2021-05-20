using System;
using System.Collections.Generic;
using System.IO;

namespace Virtuel_Memory
    {
        enum ePageStatus { E, F, X } //enum type for program status
        class Page // class to clarify the TLB and pages
        {
            private int iPageNo = 0;
            private int iAge = 0;
            private ePageStatus eStatus = ePageStatus.E;
            private int iBlockNo = 0;
            public int PageNumber
            {
                set { iPageNo = value; }
                get { return iPageNo; }
            }
            public int BlockNo
            {
                get { return iBlockNo; }
                set { iBlockNo = value; }
            }
            public ePageStatus Status
            {
                get { return eStatus; }
                set { eStatus = value; }
            }
            public int Age
            {
                get { return iAge; }
                set { iAge = value; }
            }
            public void Print()
            {
                Console.Write("{0,3}", "| ");
                Console.Write("{0,-2}", iPageNo);
                Console.Write(" ");
                Console.Write("{0,2}", iAge);
                Console.Write("  ");
                Console.Write("{0,-1}", eStatus);
                Console.Write(" ");
                Console.Write("{0,3}", iBlockNo);
                Console.WriteLine(" |");
            }
        }
        class Block // Class to use the Disk blog. Also used to managed the Taskes in mutitask process
        {
            private int iBlockNo = 0;
            private string sProgramName = "O";
            private int iTimeToExacute = 0;
            private int iNextBlock = 0;
            private int iTimeSenceBeginning = 0;
            private string sState = "New Entery";
            private int iPageFaultTime = 0;
            public string ProgramName
            {
                get { return sProgramName; }
                set { sProgramName = value; }
            }
            public int TimeToExacute
            {
                get { return iTimeToExacute; }
                set { iTimeToExacute = value; }
            }
            public int NextBlock
            {
                get { return iNextBlock; }
                set { iNextBlock = value; }
            }
            public int BlockNo
            {
                get { return iBlockNo; }
                set { iBlockNo = value; }
            }
            public int TimeSenceBeginning
            {
                get { return iTimeSenceBeginning; }
                set { iTimeSenceBeginning = value; }
            }
            public string State
            {
                get { return sState; }
                set { sState = value; }
            }
            public int PageFaultTime
            {
                get { return iPageFaultTime; }
                set { iPageFaultTime = value; }
            }
            public void Print()
            {
                Console.Write("{0,3}", BlockNo);
                Console.Write("{0,-5}", ".");
                Console.Write("{0,-3}", ProgramName);
                Console.Write(" ");
                Console.Write("{0,3}", TimeToExacute);
                Console.Write(" ");
                Console.WriteLine("{0,5}", NextBlock);
            }
        }
        class Program
        {
            static int iLoopTime; // program loop Time
            static int iTimeSlice; // Time slice time
            static int iPageFaultTime; // Page fault time

            static List<Page> TLB = new List<Page>(); // An Array to use pages in a TLB
            static List<Block> DiskBlocks = new List<Block>(); //An array to make 24 block of disk
            static List<Block> Tasks = new List<Block>(); // An arry to use multitask prociger
            static List<int> iTimeToExacute = new List<int>(); // trac the repitation of executing
            static int iItrationNo; // Varible to count the time programs procces

            static bool TLBFULL() // chech if TLB have eny empty page
            {
                foreach (Page page in TLB)
                {
                    if (page.Status == ePageStatus.E)
                    {
                        return false;
                    }
                }
                return true;
            }
            static void ReadInput1(String fileName) //read the file
            {
                using (TextReader reader = File.OpenText(fileName))
                {
                    int iLinesNo = 0;
                    string sInputs;

                    while (iLinesNo++ <= 3) // seprate the firs 3 entery
                    {
                        sInputs = reader.ReadLine();
                        switch (iLinesNo)
                        {
                            case 1:
                                iLoopTime = Convert.ToInt32(sInputs);
                                break;

                            case 2:
                                iTimeSlice = Convert.ToInt32(sInputs);
                                break;

                            case 3:
                                iPageFaultTime = Convert.ToInt32(sInputs);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            static void PrintOutput1() //Print  first 3 inpout
            {
                Console.WriteLine();
                Console.WriteLine("=========================================");
                Console.WriteLine();
                Console.Write("{0,-20}", "Loop Time:");
                Console.WriteLine("{0,5}", iLoopTime);
                Console.Write("{0,-20}", "Time Slice:");
                Console.WriteLine("{0,5}", iTimeSlice);
                Console.Write("{0,-20}", "Page Fault Time:");
                Console.WriteLine("{0,5}", iPageFaultTime);
            }
            static void ReadDiskBlock(string fileName) //read the file
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    int iLinesNo = 0;
                    string sInputs;

                    while ((sInputs = reader.ReadLine()) != null)
                    {
                        if (iLinesNo++ >= 3) //Skip first 3 input
                        {
                            string[] aBlock = sInputs.Split(' ');

                            DiskBlocks.Add //Creating each Block
                            (
                                new Block
                                {
                                    BlockNo = iLinesNo - 3,
                                    ProgramName = aBlock[0],
                                    TimeToExacute = Convert.ToInt32(aBlock[1]),
                                    NextBlock = Convert.ToInt32(aBlock[2])
                                }
                            );
                        }
                    }
                }
            }
            static void MakeTLB() // Create TLB for wit 8 rows
            {
                for (int i = 0; i < 8; i++)
                {
                    TLB.Add(new Page { PageNumber = i + 1 });
                }
            }
            static void PrintDiskBlock() //Print entire Disk Block (24 line)
            {
                Console.WriteLine();
                Console.WriteLine("=========================================");
                Console.WriteLine();
                for (int i = 0; i < DiskBlocks.Count; i++)
                {
                    DiskBlocks[i].Print();
                }
                Console.WriteLine();
                Console.WriteLine("=========================================");
            }
            static void PrintTLB() // Display the TLB
            {
                Console.WriteLine("{0,17}", "----------------");
                for (int i = 0; i < 8; i++)
                {
                    TLB[i].Print();
                }
                Console.WriteLine("{0,17}", "----------------");
            }
            static void OutPut(Block Task) // print the whole output
            {
                Console.WriteLine();
                Console.Write(" ");
                Console.Write("{0,3}", iItrationNo);
                Console.Write(" ");
                Console.Write("{0,3}", (Task.TimeToExacute));
                Console.Write("  ");
                Console.Write("{0,-1}", Task.ProgramName);
                Console.Write(" ");
                Console.WriteLine("{0,3}", Task.BlockNo);
                PrintTLB();
            }
            static Block SetATask(string sProgramName) // Find First prpgram one by one an set it on multitasking process
            {
                Block Task = new Block();
                foreach (Block block in DiskBlocks)
                {
                    if (block.ProgramName == sProgramName)
                    {
                        return block;
                    }
                }
                return Task;
            }
            static void FirstBlockSToProgress() // Spesifid the name of programs to find for the starting of program
            {
                int iMultiTaskSize = 4;
                for (int i = 0; i < iMultiTaskSize; i++)
                {
                    Tasks.Add(new Block { });
                    iTimeToExacute.Add(new int { });
                }
                Tasks[0] = SetATask("A");
                iTimeToExacute[0] = Tasks[0].TimeToExacute;
                Tasks[1] = SetATask("B");
                iTimeToExacute[1] = Tasks[1].TimeToExacute;
                Tasks[2] = SetATask("C");
                iTimeToExacute[2] = Tasks[2].TimeToExacute;
                Tasks[3] = SetATask("D");
                iTimeToExacute[3] = Tasks[3].TimeToExacute;
            }
            static void CheckTLBSpace()
            {
                if (TLBFULL())// chech if all the pages in TLB is full
                {
                    int IMax = 0;
                    int iOldestPagePNO = 0;
                    foreach (Page page in TLB) // find the oldest page
                    {
                        if (page.Age > IMax)
                        {
                            IMax = page.Age;
                            iOldestPagePNO = page.PageNumber;
                        }
                    }
                    // reset the data of oldest Page on TLB for re use
                    TLB[iOldestPagePNO - 1].Status = ePageStatus.E;
                    TLB[iOldestPagePNO - 1].Age = 0;
                }
            }
            static void programTimeSlice() // Time slice the program have to loop
            {
                for (int i = 0; i < Tasks.Count; i++)
                {
                    State(i);
                    Console.WriteLine("");
                    Console.WriteLine("***************************");
                    Console.WriteLine("{0,20}", Tasks[i].State);
                    Console.WriteLine("***************************");
                    OutPut(Tasks[i]);
                }
            }

            static void State(int i) // State, the program fasing
            {
                switch (Tasks[i].State)
                {
                    case "New Entery":
                        CheckTLBSpace();
                        foreach (Page page in TLB) // make process ready to progress
                        {
                            if (page.Status == ePageStatus.E)
                            {
                                page.Status = ePageStatus.F;
                                page.BlockNo = Tasks[i].BlockNo;
                                Tasks[i].State = "Progressing";
                                break;
                            }
                        }

                        break;

                    case "Progressing":  // State of progreesing
                        int iTimeSliceRminding = iTimeSlice;
                        while (Convert.ToBoolean(iTimeSliceRminding--)) // loop the time slice
                        {
                            if (iTimeToExacute[i] > 0) // Exacute until program have process
                            {
                                iTimeToExacute[i]--;
                                Tasks[i].TimeSenceBeginning++;
                                // iItrationNo++;
                                foreach (Page page in TLB) // change Aging of any page
                                {
                                    if (page.Status != ePageStatus.E)
                                    {
                                        if (page.BlockNo == Tasks[i].BlockNo)
                                        {
                                            page.Age = 0;
                                        }
                                        else if (page.Status != ePageStatus.X)
                                        {
                                            page.Age++;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Tasks[i].State = "Done"; //program finished to prosecc
                                Tasks[i] = DiskBlocks[Tasks[i].NextBlock - 1]; // Time to replace the task
                                iTimeToExacute[i] = Tasks[i].TimeToExacute;
                                CheckTLBSpace();
                                foreach (Page page in TLB) // Check if task is available in TLB
                                {
                                    if (Tasks[i].BlockNo == page.BlockNo)
                                    {
                                        Tasks[i].State = "Progressing";
                                        break;
                                    }
                                }
                                foreach (Page page in TLB)
                                {
                                    if (page.Status == ePageStatus.E) // Page fult happend
                                    {
                                        page.Status = ePageStatus.X;
                                        page.BlockNo = Tasks[i].BlockNo;
                                        Tasks[i].State = "Page fault";
                                        Tasks[i].PageFaultTime = iPageFaultTime;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;

                    case "Page fault": // Process on the Fault time state

                        if (Tasks[i].PageFaultTime <= 0) // releaz the page fauld if PAGE TIME FAULT finishd
                        {
                            Tasks[i].State = "Progressing";
                            foreach (Page page in TLB)
                            {
                                if (page.BlockNo == Tasks[i].BlockNo) // Change the page display
                                {
                                    page.Status = ePageStatus.F;
                                    break;
                                }
                            }

                        }
                        Tasks[i].PageFaultTime -= iTimeSlice;

                        break;

                    default:
                        break;
                }
            }
            static void ProgramLoop() // Program Main Loop
            {
                int iLooped = 0;
                while (iLooped <= iLoopTime)
                {
                    programTimeSlice();
                    Console.WriteLine("=========================================");
                    iLooped++;
                    iItrationNo++;

                }
            }
            static void Main(string[] args)
            {
                string fileName = "DATA";

                ReadInput1(fileName); // Read input and save on variables
                PrintOutput1(); // Print the first output
                ReadDiskBlock(fileName); // Read the file for disk block and save it in an aray of blocks
                PrintDiskBlock(); // print the disk block array
                MakeTLB(); // crating a TLB
                FirstBlockSToProgress(); //Make the program to start with the entry programs
                ProgramLoop(); // Main loop includ all steps
                Console.WriteLine("Press any key to close !!!");
                Console.ReadKey();
            }
        }
    }


