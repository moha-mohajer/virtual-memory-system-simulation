# virtual-memory-system-simulation
 A program, which simulates a virtual memory system.
 
 !!! This program was an assignment for the Philips university Cyprus. all the right reserved !!!

The system will be a multitasking one with four processes, A, B, C, and D having equal
priorities, each running for a time period equal to “time slice”, one of the inputs. In other
words, each process runs for “time slice” units of time before the next process starts

However, in case of a page fault, the process that has caused this fault does not run
while waiting for the block to be loaded to memory. The time it takes to load a block
from disk to memory is called “page fault time” and it is greater than the time slice.

The system will have the following characteristics:
Page size = block size
Memory
Memory will be divided into 8 pages.
Disk
The disk will be divided into 24 blocks. Each block will contain the following
information:
1. Program name to which this block belongs.
2. Time to execute the program segment inside the block
3. Next block.
The “next block” information will indicate which block needs to be loaded in memory
for the program owning the block to continue.
TLB
Each row of the TLB will be indicating the status of a page in Memory and it will have
the following entries:
1. Age of page in memory.
2. Whether a page is empty (E), it is occupied by a block (F), or there is a page fault in
progress (X).
3. Block number loaded in page.
Input
The input will be a file having the following entries:
The number of times the program will loop : integer
The time slice for executing a process (before switching to the next process) : integer
The time it takes for a page fault : integer
Twenty four lines, one per block of disk, containing the following information
Program name : A, B, C, D
Time for executing program in block : integer
Next block : 1..24

Program,time to execute,next block
Output:
The output will contain two parts.
The first part will contain the following information:
1. Iteration number
2. Time since the beginning of the program
3. Process running
4. Block number
The second part will contain the entries of the TLB

Disk
A,1,1
C,1,10
D,6,20
D,1,22
D,1,21
B,2,7
B,1,19
A,3,24
B,23,18
C,80,23
A,15,17
A,3,14
A,9,13
B,8,16
C,3,15
A,6,12
B,5,11
B,23,8
A,5,9
A,20,6
A,10,5
A,1,4
A,8,3
A,4,2
Output:
The output will contain two parts.
The first part will contain the following information:
1. Iteration number
2. Time since the beginning of the program
3. Process running
4. Block number
The second part will contain the entries of the TLB
