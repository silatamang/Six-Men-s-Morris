using System;

static class State
{
    public const int empty = 0;
    public const int me = 1;
    public const int enemy = 2;
}

static class CSide
{
    public const int outTop = 0;
    public const int outLeft = 1;
    public const int outDown = 2;
    public const int outRight = 3;
    public const int inTop = 4;
    public const int inLeft = 5;
    public const int inDown = 6;
    public const int inRight = 7;
    public const int none = 8;
} 

public class Node
{
    public Node left = null;
    public Node right = null;
    public Node down = null;
    public Node up = null;
    public bool seen = false;
    public int state = State.empty;
    public int position;

    public int EmptyNearbyPosition()
    {
        if (left != null && left.state == State.empty) { return left.position; }
        if (right != null && right.state == State.empty) { return right.position; }
        if (down != null && down.state == State.empty) { return down.position; }
        if (up != null && up.state == State.empty) { return up.position; }

        return -1;
    }

    public int ReturnNearbyPlayerPosition(int state, int notThis1, int notThis2)
    {
        if (left != null && left.state == state && left.position != notThis1 && left.position != notThis2) { return left.position; }
        if (right != null && right.state == state && right.position != notThis1 && right.position != notThis2) { return right.position; }
        if (down != null && down.state == state && down.position != notThis1 && down.position != notThis2) { return down.position; }
        if (up != null && up.state == state && up.position != notThis1 && up.position != notThis2) { return up.position; }

        return -1;
    }
}

public class Side
{
    public Node n1 = null;
    public Node n2 = null;
    public Node n3 = null;

    public void Return2PositionsNotEmpty(ref int one, ref int two)
    {
        if (n1.state == State.empty)
        {
            one = n2.position;
            two = n3.position;
        }
        else if (n2.state == State.empty)
        {
            one = n1.position;
            two = n3.position;
        }
        else
        {
            one = n1.position;
            two = n2.position;
        }
    }

    public bool CheckForPosition(int position)
    {
        if (n1.position == position)
        {
            return true;
        }
        if (n2.position == position)
        {
            return true;
        }
        if (n3.position == position)
        {
            return true;
        }

        return false;
    }

    public int GetOnePlayer(int state)
    {
        if(n1.state == state)
        {
            return n1.position;
        }
        if (n2.state == state)
        {
            return n2.position;
        }
        if (n3.state == state)
        {
            return n3.position;
        }

        return -1;
    }

    public int PlayerCount(int state)
    {
        int count = 0;
        if (n1.state == state) { count++; }
        if (n2.state == state) { count++; }
        if (n3.state == state) { count++; }
        return count;
    }

    public int GetEmpty()
    {
        if(n1.state == State.empty)
        {
            return n1.position;
        }
        else if (n2.state == State.empty)
        {
            return n2.position;
        }
        else if (n3.state == State.empty)
        {
            return n3.position;
        }
        else
        {
            return -1;
        }
    }
}

public class Mode
{
    public const bool place = true;
    public const bool move = false;
}

/*************************************************************/

public class AI
{
    public Node[] nodes;
    public Side[] sides;
    public bool mode = Mode.place;
    public int playCount = 0;
    public Random random;

    public AI()
    {
        random = new Random();
        InitNodeArray();
        InitSides();
        SetUpBoard();
    }

    private void InitSides()
    {
        sides = new Side[8];
        for (int iii = 0; iii < sides.Length; iii++)
        {
            sides[iii] = new Side();
        }

        sides[0].n1 = nodes[7];
        sides[0].n2 = nodes[0];
        sides[0].n3 = nodes[1];

        sides[1].n1 = nodes[1];
        sides[1].n2 = nodes[2];
        sides[1].n3 = nodes[3];

        sides[2].n1 = nodes[3];
        sides[2].n2 = nodes[4];
        sides[2].n3 = nodes[5];

        sides[3].n1 = nodes[5];
        sides[3].n2 = nodes[6];
        sides[3].n3 = nodes[7];

        sides[4].n1 = nodes[15];
        sides[4].n2 = nodes[8];
        sides[4].n3 = nodes[9];

        sides[5].n1 = nodes[9];
        sides[5].n2 = nodes[10];
        sides[5].n3 = nodes[11];

        sides[6].n1 = nodes[11];
        sides[6].n2 = nodes[12];
        sides[6].n3 = nodes[13];

        sides[7].n1 = nodes[13];
        sides[7].n2 = nodes[14];
        sides[7].n3 = nodes[15];
    }


    public void MoveAI(ref int start, ref int destination)
    {
        if (mode == Mode.place)
        {
            Place(ref start, ref destination);
        }
        else
        {
            Move(ref start, ref destination);
        }

        UpdateBoardMyPiece(start, destination);
        PlayCountInc();
    }

    public bool CheckForMillByPosition(int position, int state)
    {
        int side = SearchPositionSide(position);
        if (side != -1)
        {
            int count = sides[side].PlayerCount(state);
            if(count == 3)
            {
                return true;
            }
        }

        return false;
    }

    public int SearchPositionSide(int position) //returns side of give position
    {
        for(int iii = 0; iii < sides.Length; iii++)
        {
            bool temp = sides[iii].CheckForPosition(position);
            if(temp)
            {
                return iii;
            }
        }

        return -1;
    }

    private void Move(ref int start, ref int destination)
    {
        if(CheckForImminentMill(ref start, ref destination))
        {
            UpdateBoardMyPiece(start, destination);
            return;
        }

        //all else fails, random
        RandomMove(ref start, ref destination);

    }

    private void RandomMove(ref int start, ref int destination)
    {
        int position = -1;

        for (int iii = 0; iii < nodes.Length; iii++)
        {
            if(nodes[iii].state == State.me)
            {
                position = nodes[iii].EmptyNearbyPosition();
            }
            if(position >= 0)
            {
                start = nodes[iii].position;
                destination = position;
                return;
            }
        }
    }

    private bool CheckForImminentMill(ref int start, ref int destination)
    {
        int side = CheckFor2AndOther(State.me, State.empty);
        if(side == -1)
        {
            return false;
        }

        int emptyPosition = sides[side].GetEmpty();

        int notThis1 = 0;
        int notThis2 = 0;

        sides[side].Return2PositionsNotEmpty(ref notThis1, ref notThis2);

        int nearByPosition = nodes[emptyPosition].ReturnNearbyPlayerPosition(State.me, notThis1, notThis2);
        if(nearByPosition > -1)
        {
            start = nearByPosition;
            destination = emptyPosition;
            return true;
        }

        return false;
    }

    public int RemoveEnemyPiece()
    {
        int side = CheckForMill(State.enemy);
        if(side != -1)
        {
            int position = sides[side].n1.position;
            nodes[position].state = State.empty;
            return position;
        }

        side = CheckFor2AndOther(State.enemy, State.empty);
        if(side != -1)
        {
            int position = sides[side].GetOnePlayer(State.enemy);
            nodes[position].state = State.empty;
            return position;
        }
        
        //remove any piece
        for(int iii = 0; iii < nodes.Length; iii++)
        {
            if(nodes[iii].state == State.enemy)
            {
                nodes[iii].state = State.empty;
                return iii;
            }
        }

        //shouldn't happen, game over before no enemy pieces
        return -1;
    
    }

    public void RemoveMyPiece(int position)
    {
        nodes[position].state = State.empty;
    }

    public void PrintBoard()
    {
        Console.WriteLine("[{0}]   [{1}]   [{2}]", nodes[7].state, nodes[0].state, nodes[1].state);
        Console.WriteLine("   [{0}][{1}][{2}]", nodes[15].state, nodes[8].state, nodes[9].state);
        Console.WriteLine("[{0}][{1}]   [{2}][{3}]", nodes[6].state, nodes[14].state, nodes[10].state, nodes[2].state);
        Console.WriteLine("   [{0}][{1}][{2}]", nodes[13].state, nodes[12].state, nodes[11].state);
        Console.WriteLine("[{0}]   [{1}]   [{2}]", nodes[5].state, nodes[4].state, nodes[3].state);
    }

    private void UpdateBoardMyPiece(int start, int destination)
    {
        if(start != -1)
        {
            nodes[start].state = State.empty;
        }
        nodes[destination].state = State.me;
    }

    private void Place(ref int start, ref int destination)
    {
        //check for my imminent mill
        int side = CheckFor2AndOther(State.me, State.empty);
        if (side != -1)
        {
            destination = sides[side].GetEmpty();
            start = -1;
            return;
        }

        //check for enemy imminent mill
        side = CheckFor2AndOther(State.enemy, State.empty);
        if (side != -1)
        {
            destination = sides[side].GetEmpty();
            start = -1;
            return;
        }

        //check for just one of me and two empty spots
        side = CheckFor2AndOther(State.empty, State.me);
        if (side != -1)
        {
            destination = sides[side].GetEmpty();
            start = -1;
            return;
        }

        //all else fails, random
        start = -1;
        destination = RandomPosition();
        return;
    }

    private int RandomPosition()
    {
        int position;

        do
        {
            position = random.Next(16);
        }
        while (nodes[position].state != State.empty);

        return position;
    }


    private int CheckFor2AndOther(int state, int state2) //returns side
    {
        for (int iii = 0; iii < sides.Length; iii++)
        {
            if (sides[iii].PlayerCount(state) == 2 && sides[iii].PlayerCount(state2) == 1)
            {
                return iii;
            }
        }

        return -1;
    }

    private int CheckForMill(int state) //returns side
    {
        for(int iii = 0; iii < sides.Length; iii++)
        {
            if (sides[iii].PlayerCount(state) == 3)
            {
                return iii;
            }
        }

        return -1;
    }

    public void MoveOpp(ref int start, ref int destination)
    {
        if (start != -1)
        {
            nodes[start].state = State.empty;
        }

        nodes[destination].state = State.enemy;
    }

    private void PlayCountInc()
    {
        playCount++;
        if(playCount == 6)
        {
            mode = Mode.move;
        }
    }

    private void InitNodeArray()
    {
        nodes = new Node[16];

        for(int iii = 0; iii < nodes.Length; iii++)
        {
            nodes[iii] = new Node();
            nodes[iii].position = iii;
        }
    }

    private void ClearSeen()
    {
        for (int iii = 0; iii < nodes.Length; iii++)
        {
            nodes[iii].seen = false;
        }
    }

    private void SetUpBoard()
    {
        nodes[0].up = null;
        nodes[0].right = nodes[1];
        nodes[0].down = nodes[8];
        nodes[0].left = nodes[7];

        nodes[1].up = null;
        nodes[1].right = null;
        nodes[1].down = nodes[2];
        nodes[1].left = nodes[0];

        nodes[2].up = nodes[1];
        nodes[2].right = null;
        nodes[2].down = nodes[3];
        nodes[2].left = nodes[10];

        nodes[3].up = nodes[2];
        nodes[3].right = null;
        nodes[3].down = null;
        nodes[3].left = nodes[4];

        nodes[4].up = nodes[12];
        nodes[4].right = nodes[3];
        nodes[4].down = null;
        nodes[4].left = nodes[5];

        nodes[5].up = nodes[6];
        nodes[5].right = nodes[4];
        nodes[5].down = null;
        nodes[5].left = null;

        nodes[6].up = nodes[7];
        nodes[6].right = nodes[14];
        nodes[6].down = nodes[5];
        nodes[6].left = null;

        nodes[7].up = nodes[0];
        nodes[7].right = null;
        nodes[7].down = nodes[6];
        nodes[7].left = null;

        nodes[8].up = nodes[0];
        nodes[8].right = nodes[9];
        nodes[8].down = null;
        nodes[8].left = nodes[15];

        nodes[9].up = null;
        nodes[9].right = null;
        nodes[9].down = nodes[10];
        nodes[9].left = nodes[8];

        nodes[10].up = nodes[9];
        nodes[10].right = nodes[2];
        nodes[10].down = nodes[11];
        nodes[10].left = null;

        nodes[11].up = nodes[10];
        nodes[11].right = null;
        nodes[11].down = null;
        nodes[11].left = nodes[12];

        nodes[12].up = null;
        nodes[12].right = nodes[11];
        nodes[12].down = nodes[4];
        nodes[12].left = nodes[13];

        nodes[13].up = nodes[14];
        nodes[13].right = nodes[12];
        nodes[13].down = null;
        nodes[13].left = null;

        nodes[14].up = nodes[15];
        nodes[14].right = null;
        nodes[14].down = nodes[13];
        nodes[14].left = nodes[6];

        nodes[15].up = null;
        nodes[15].right = nodes[8];
        nodes[15].down = nodes[14];
        nodes[15].left = null;
    }
}
