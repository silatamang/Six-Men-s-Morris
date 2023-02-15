using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixMensAI_CS
{

    static class State
    {
        public const int empty = 0;
        public const int me = 1;
        public const int enemy = 2;
    }

    class Board
    {
        Network network;
        NetworkClient networkClient;
        AI ai;

        int delay = 1000;


        public void Host(String IP)
        {
            ai = new AI();
            network = new Network();
            network.Host(IP);
            GameLoop("host");
        }

        public void Connect(String input)
        {
            ai = new AI();
            networkClient = new NetworkClient();
            networkClient.ConnectServer(input);
            GameLoop("client");
        }

        public void GameLoop(String first)
        {
            int start = -1;
            int destination = -1;
            int remove = -1;
            int roundCount = 0;

            if(first == "client")
            {
                System.Threading.Thread.Sleep(5000);
                while (true) {
                    roundCount++;
                    Console.WriteLine("Round {0}", roundCount);
                    Console.WriteLine();
                    ai.MoveAI(ref start, ref destination);
                    if(ai.CheckForMillByPosition(destination, State.me))
                    {
                        remove = ai.RemoveEnemyPiece();
                    } else {
                        remove = -1;
                    }
                    ai.PrintBoard();
                    networkClient.SendMessage(start, destination, remove);
                    System.Threading.Thread.Sleep(delay);
                    //enemy move
                    while (!networkClient.GetMessage(ref start, ref destination, ref remove)) { }
                    ai.MoveOpp(ref start, ref destination);
                    if(remove != -1)
                    {
                        ai.RemoveMyPiece(remove);
                    }
                    else
                    {
                        remove = -1;
                    }
                    ai.PrintBoard();
                    System.Threading.Thread.Sleep(delay);
                }

            } else if (first == "host")
            {
                networkClient = new NetworkClient();
                while (true)
                {
                    
                    roundCount++;
                    Console.WriteLine("Round {0}", roundCount);
                    Console.WriteLine();
                    //enemy move
                    while (!network.GetMessage(ref start, ref destination, ref remove)) { }
                    ai.MoveOpp(ref start, ref destination);
                    //Console.WriteLine(start, destination);
                    if (remove != -1)
                    {
                        ai.RemoveMyPiece(remove);
                    } else
                    {
                        remove = -1;
                    }
                    ai.PrintBoard();
                    System.Threading.Thread.Sleep(delay);
                    //my moves
                    ai.MoveAI(ref start, ref destination);
                    if (ai.CheckForMillByPosition(destination, State.me))
                    {
                        remove = ai.RemoveEnemyPiece();
                    } else {
                        remove = -1;
                    }
                    network.SendMessage(start, destination, remove);
                    ai.PrintBoard();
                    System.Threading.Thread.Sleep(delay);
                }
            }
        }
    }
}
