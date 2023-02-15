using System;

namespace SixMensAI_CS
{
    class Interface
    {
        public static void Test(int rounds)
        {
            Console.WriteLine("Press any key for next move");

            AI b1 = new AI();
            AI b2 = new AI();
            int start = 0;
            int destination = 0;

            for (int iii = 0; iii < rounds; iii++)
            {
                Console.WriteLine();
                Console.WriteLine("Round {0}", iii + 1);

                if (iii != 0)
                {
                    b1.MoveOpp(ref start, ref destination);
                }
                b1.MoveAI(ref start, ref destination);


                b2.MoveOpp(ref start, ref destination);

                Console.WriteLine();
                b2.PrintBoard();
                Console.ReadKey();

                b2.MoveAI(ref start, ref destination);

                Console.WriteLine();
                b2.PrintBoard();
                Console.ReadKey();
            }

            Console.WriteLine();
            Console.WriteLine("Finished");
        }

        public static void Normal()
        {
            Board board = new Board();

            Console.WriteLine("Welcome to Six Men's Morris");
            Console.WriteLine("1. Single Player");
            Console.WriteLine("2. AI vs. AI Network");
            Console.WriteLine("3. AI vs. AI Test");
            String input = Console.ReadLine();
            char cInput = input[0];
            if(cInput == '1')
            {
                
            } else if(cInput == '2')
            {
                Console.WriteLine();
                Console.WriteLine("1. Host");
                Console.WriteLine("2. Connect");
                input = Console.ReadLine();
                cInput = input[0];
                Console.WriteLine();
                if (cInput == '1')
                {
                    Console.WriteLine("Enter IP address.");
                    input = Console.ReadLine();
                    board.Host(input);
                }
                else if (cInput == '2')
                {
                    Console.WriteLine("Enter IP address.");
                    input = Console.ReadLine();
                    board.Connect(input);
                }
            }
            else if (cInput == '3')
            {
                Test(8);
            }
        }



        public static void TestRemove()
        {
            AI b1 = new AI();
            b1.nodes[5].state = State.enemy;
            //b1.nodes[4].state = State.enemy;
            //b1.nodes[3].state = State.enemy;
            b1.PrintBoard();
            b1.RemoveEnemyPiece();
            Console.WriteLine();
            b1.PrintBoard();
        }

        public static void TestMove()
        {
            AI b1 = new AI();
            b1.mode = Mode.move;
            b1.nodes[9].state = State.me;
            b1.nodes[0].state = State.me;
            b1.nodes[11].state = State.me;
            b1.PrintBoard();
            int start = 1000;
            int destination = 1000;
            b1.MoveAI(ref start, ref destination);
            Console.WriteLine();
            b1.PrintBoard();
        }


        static void Main(string[] args)
        {
            Normal();
            //Test(10);
            //TestRemove();
            //TestMove();
            Console.ReadKey();
        }
    }
}