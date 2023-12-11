<Query Kind="Program">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

void Main()
{
    part1();
    part2();
}

void part1()
{
    var mapLines = data
                .Split(Environment.NewLine);


    var rows = mapLines.Length;
    var columns = mapLines[0].Length;

    allPipes = new Pipe[rows, columns];

    for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
        {
            allPipes[i, j] = new Pipe(mapLines[i][j], i, j);
        }

    foreach (var p in allPipes) { p.buildCons(); }

    // find start.
    Pipe start = null;
    foreach (var p in allPipes)
    {
        if (p.Symbol == 'S')
        {
            start = p;
            break;
        }
    }

    start.FindConnectionsForStart();

    // count main loop pipes.
    var p1 = start;
    var p2 = start.connects[0];
    var p3 = next(p1, p2);

    var p4 = start;
    var p5 = start.connects[1];
    var p6 = next(p4, p5);

    int count = 2;
    for (; ; )
    {
        if (p3 == p6)
            break;

        p1 = p2;
        p2 = p3;
        p3 = next(p1, p2);

        p4 = p5;
        p5 = p6;
        p6 = next(p4, p5);

        count++;
    }

    count.Dump();
}


Pipe next(Pipe p1, Pipe p2)
{
    return p2.connects.First(cp => cp != p1);
}


static Pipe[,] allPipes = null;


public class Pipe
{
    public char Symbol;
    public int Row, Col;
    public Pipe[] connects = Array.Empty<Pipe>();

    public Pipe(char s, int row, int col)
    {
        this.Symbol = s;
        this.Row = row;
        this.Col = col;
    }

    public void buildCons()
    {
        try
        {
            this.connects = Symbol switch
            {
                '|' => new[] { north(), south() },
                '-' => new[] { east(), west() },
                'L' => new[] { north(), east() },
                'J' => new[] { north(), west() },
                '7' => new[] { south(), west() },
                'F' => new[] { south(), east() },
                '.' => Array.Empty<Pipe>(),
                'S' => Array.Empty<Pipe>(),
            };
        }
        catch { }
    }

    Pipe north() => allPipes[Row - 1, Col];
    Pipe south() => allPipes[Row + 1, Col];
    Pipe east() => allPipes[Row, Col + 1];
    Pipe west() => allPipes[Row, Col - 1];


    public void FindConnectionsForStart()
    {
        foreach (var p in allPipes)
        {
            if (p.connects.Any(c => c.Symbol == 'S'))
            {
                connects = connects.Append(p).ToArray();
                if (connects.Length == 2)
                    break;
            }
        }
    }

    public void fixStartSymbol()
    {
        // only after you've setup start's connections.
        var c1 = connects[0];
        var c2 = connects[1];
        if (c1 == north() && c2 == south()) Symbol = '|';
        if (c1 == east() && c2 == west()) Symbol = '-';
        if (c1 == north() && c2 == east()) Symbol = 'L';
        if (c1 == north() && c2 == west()) Symbol = 'J';
        if (c1 == south() && c2 == west()) Symbol = '7';
        if (c1 == south() && c2 == east()) Symbol = 'F';
    }
}


void part2()
{
    var mapLines = data
                    .Split(Environment.NewLine);
    
    // build pipes Collection.
    var rows = mapLines.Length;
    var columns = mapLines[0].Length;

    allPipes = new Pipe[rows, columns];
    
    for (int i = 0; i < rows; i++)
        for (int j = 0; j < columns; j++)
        {
            allPipes[i, j] = new Pipe(mapLines[i][j], i, j);
        }

    foreach (var p in allPipes) { p.buildCons(); }


    // figure out start stuff.
    Pipe start = null;
    foreach (var p in allPipes)
    {
        if (p.Symbol == 'S')
        { start = p; break; }
    }
    
    start.FindConnectionsForStart();
    start.fixStartSymbol();


    // get all pipes in main loop.
    var p1 = start;
    var p2 = start.connects[0];
    var p3 = next(p1, p2);

    var p4 = start;
    var p5 = start.connects[1];
    var p6 = next(p4, p5);

    var mainPipes = new List<Pipe>() { p1, p2, p3, p5, p6 };
    for (; ; )
    {
        if (p3 == p6)
            break;

        p1 = p2;
        p2 = p3;
        p3 = next(p1, p2);
        mainPipes.Add(p3);

        p4 = p5;
        p5 = p6;
        p6 = next(p4, p5);
        mainPipes.Add(p6);
    }


    // clear out extra pipes.
    foreach (var p in allPipes)
    {
        if (!mainPipes.Contains(p))
        {
            p.Symbol = '.';
        }
    }


    // go 1 row at a time, everytime you cross vertically, you change in/out of loop.
    var inside = false;
    var lastCorner = ' ';
    var area = 0;

    for (int i = 0; i < rows; i++)
    {
        inside = false;

        for (int j = 0; j < columns; j++)
        {
            var pip = allPipes[i, j];

            if (pip.Symbol == '.' && inside) { area++; }

            else if (pip.Symbol == '|') { inside = !inside; }
            else if (lastCorner == ' ' && pip.Symbol == 'F') { lastCorner = 'F'; }
            else if (lastCorner == 'F' && pip.Symbol == 'J') { lastCorner = ' '; inside = !inside; }
            else if (lastCorner == 'F' && pip.Symbol == '7') { lastCorner = ' '; }

            else if (lastCorner == ' ' && pip.Symbol == 'L') { lastCorner = 'L'; }
            else if (lastCorner == 'L' && pip.Symbol == '7') { lastCorner = ' '; inside = !inside; }
            else if (lastCorner == 'L' && pip.Symbol == 'J') { lastCorner = ' '; }
        }
    }

    area.Dump();
}



const string data2 = """
.....
.S-7.
.|.|.
.L-J.
.....
""";

const string data3 = """
..F7.
.FJ|.
SJ.L7
|F--J
LJ...
""";

const string data4 = """
...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........
""";

const string data5 = """
.F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ...
""";


const string data = """
7-LJ.FF7J7-F-F--F7-.LL|7.LFJ--7F.J.L--FF-F77-F7|7F-F-777.JJ7|.J--F7JF-7-7-F-F-J.F-FF--7..|.LFJ77.77|F7FJ-77|F77FFL|7.|-7.|.7JFF--7|.|7-F--77
|..L|LJJ|7-L7L77|JFFLL7|7|J|L7|7FLL.L-LF7||7LJ-J-J|JL|-F-J-F777LL||.F|.L7|JJL||.|-FJ7FL|F|.LLL77F-7LF|JJLJ-.F7-LJJ||-|F|FLL|-F|L||JLL7FJJ-L7
|77||F.FF-J|FJF-J|F7FFJJJL-7L|L--JLF|7F|FJ|J7F7|L7|7JF.F77FJL-7|L|L77F--LF-FFJJ-LJF|J-LL|L|7FLL-7|-JLL.J.FF7J||LL777-FF7J-JJFFJFF.J7LFLJLF7|
|F|J-|---7F-JFL7.F|.L7|-F-F7F|7JLF7L-F-7L7|7-LJL7F7FF--JL7L7F-JF-JFJ7JJFF7|LJ7F-J|LJ-..L|-|7|JF||J..|.-.F77JL-JFFL||7|LJL7.|FL.7|FL7-F7F7|7F
7||7JF7J|-L7F|.JJ||FJL..F-JL7|JFFL|J7L7L7|L7.|7.JLF7L--7FJFJ|F7L-7|FF.FF|LJJJL-J.-J7L-77J||-F---|--|JJ7-F|77F7.F7F|LL77|L7-FL-F|J-FFJ|7||L-J
L7|F-|JF|-JJ.-J|L|7L-JF-L-7FJF777L|L-7|FJL7|-F-7JF||F7FJ|FJFJ|||FJL-77JF7.JJ.|F|-LJL.L77-FL-|.||F7.L|||.-JF-7J7LJLJ-J|LJJ|FF.L-J-FJ.FL-7LF-F
|-J|||.L7FJ.F|-FJ|L7|FF-F-JL7||F7F7-LFJ||FJ|F7|LF-J|||L7||FJFJ|FJF--JF7|L77.L7LL.|FL7JFJ7|JJL-J7JF|7FL||..|L|JL7.||7L|J|7.-.FF.L77L-L-7J-7.J
LJ7L--||.|.|FJFJF7FLL7L7L--7||L-77.FFL7L7L7LJL-7L-7LJL-JLJL7L7LJFJLF-JLJFJ7.LJJ.JL|.L7L-7|J.|-LJ-J|LF-LJ-7L77.LL7F|F.J.F-77-J..FL7-|JF|J7L7|
LJ-||J.L-FJ|JJ|.||7|7|7|FF7|||F-JF7-F7L7L7L7F--JF7L7F7F---7L7|F-JF7L-7F-J|7--7|77.F7.J.L-7.FLJ.FJ-7|J||J.--LJ7L.FL||.|FJL7-.JF7J-L-JJF-JF|FJ
FL777.L.|L|LJJL-L-7F-L-F-JLJLJL77||FJ|FJFJFJ|F7FJ|FJ|||7F7L7||L--JL7FJL7F7|-||77F-JF7.F7.|7JFF-7LFJJLL7..F||L77-FLL-F-77.|F7LF-J7.|FFFJ-F|J7
||LJ|7LJ77L.FF-7-|7J|L-L7F7F7F7L7||L7|L7L7|FJ|||FJL-JLJFJL-J||F----J|F7LJ|7.-7LF7-|||F777J||F7-FJ.|L7.|-FF|77F-.||.||FJ-7-|L|L77L-F-JLJLL|-L
|J.77F-.F7J7|.|JJ.JLF-FJLJ||LJL7LJL7||||FJ|L-JLJL7F-7F7L---7LJ|LF7F7||L--JF7JF7||F7|LJL-77FFJL7J|FL7J7LFFJJ|JJFF-77F|L7.F7|F7.||JJ|7.|.|LJF|
FJ-LFLJ7F7-JLFL7.77.F7JF77LJF--JF--J||FJL7|F-----JL7|||7F-7|F7L7|||LJL7F-7|L7||||||L---7L--JF-J|FL.F7--F7-FLJFLL7|F7|FJ-7J7J.7FF.L|-77L|.77J
|7J|FJJFJLFL-FJ|FL-F||FJ|F77L--7L7F7|LJF7LJL-7F7F7FJLJL7L7|LJL7LJLJF--J|FJL7||||LJL7F7.|F---JF7-|.FJ|F||L-7|F-JF||||||F7|-LJ-JL---JFL-.7.|LF
|7.|7|FJ7L7----J7FFFJ|L7LJ|7F7||FJ|||F7|L----J||||L7F--JFJL7F-JF---J|F7||F7||||L-7FJ|L7||LF77|L-77L7|7FL7FJF7.FFJ||||LJ|F7||L7-|J7F7J.-|F|.|
7-JJ|FL-JF7LJLFLF7|L7L7L-7L-JL-JL-J|LJ||LF--7FJ|||FJ|F-7|F-JL-7L--7F7|||LJLJ|||F7|L7L7LJL7||FJF-JF7||F7FJL7|L--JFJ|||F-J||-F.F7|.|L|.7FJ-|-L
J-L.||L7L7L7-FLFJ||-L7|F7L--7F-7F--JFFJL7L7FJL7||LJFJL7LJL-7F7|F7FJ|LJLJF---J|||LJFJ|L--7|||L7|7FJ||LJLJF-JL--7FJFJLJL--J|-|L||-7LF|7F77LL||
LLLLJL-|.|7.F|LLLF-7FJ||L--7||FJ|F7F7L-7L7|L7L||L7FJF7L-7F-J||LJ||FJF---JF7FFJ|L7FJF7F--JLJL-JL-JFJL-7F7|F7F--J|FJF7F----J7|JL|FL--JF|-F7JL7
..|--J7.FJ---|F|LL7|L7|L--7|LJL7||LJ|F7|FJ|FJFJL7||FJ|F7||.FJ|F-J|L7|F7F-JL7L7|FJ||||L-7F-------7|F7-||LJ||L--7LJFJ|L---7F7J-FJFJ.J-FJL|7-|F
7-LJL7-.|J7L|.F7F7||F|L7F-JL7F-JLJF-J|LJL7||FJF-J|LJFJ|LJL7L7|L-7L7|LJ||F--JFJ|L7L7|L7FJL-7F7F7.LJ||FJL-7|L7F7L7FJFL-7F7LJ|-F7--.JJJL-7.F-F.
F7|-.F7FL7F--7.F|LJL-JFJL7F-JL---7|F7|F--J||L7|F7|F-JJL--7L-JL7FL7||F-J|L-7FJFJ7L7|L7||F--J|LJL-7L|||F--J|FJ||-||F7F7LJ|F-JFJ|.|.||.--J-|LF.
LF7J.J7JLJJF|-F-JF---7L--JL7F7F--JLJ||L7LFJL7|LJ||L-7F7F7L-7F7|F-J|||FFJF-J|FJ.F7|L-J|||LF7|F---JFJ|||F7FJL7|L-J|||||F7LJFL|FJ7F77.FJF|7|||7
.LJ-JLF-7JFF-7L--JF7FL---7FJ|||F---7||FJFJF-J|F-JL7FJ||||F7LJ||L-7||L7|FJF7|L-7||L7F7LJL7|||L-7F7L7LJLJ||F-JL--7LJ||||L-7FFJL--7L7.|.7JLJ-L-
7J|JJ-LFJ.FL7L7-F7||F7F--JL-J|LJF7FJLJ|.L7L-7||F7FJ|FJ||||L-7|L7||||FJ|L-J|L7FJ||-LJL-7FJ|LJF7LJ|FL-7F7LJ|-F7LFJF-J|||F-JFJF7F-J.|.LFJ-7|LJJ
L7L7JFF|F7J.L7L-JLJLJLJF----7L7FJ|L7F-JF7|F-J|||LJFJL7LJ|L-7||FJFJLJL-JF--JFJL7|L7F---JL7|F-JL-7L7F7||L-7|FJ|FJFJF7|||L7FJFJ|L-7-7JL|LJLLJJJ
LJJ..|F7FJ7F-JF7F7F7F7FJF7F7|FJL7L7||F7|||L-7|LJF7|F7L-7|JFJ||L-JF7F---JF-7|F-J|FJL---7FJ|L-7F7L-J|||L-7LJL7|L7|FJLJLJFJ|FJFL--JJ7.7-J7.|FLF
|..-.FJ.F7FL-7|LJLJLJLJFJLJLJL7|L7LJ||LJ||F-JL7FJLJ|L--JL7|FJL-7FJLJF7F-JFJ|L-7||F7F--JL7|F-J||F7FJ||F-J-F7||FJ||F---7L-JL-7F7||||-|LJ--F-.-
LL-FLJ|7FJ||-LJF---7F7.L-----7|F-JF-JL7FJ|L7JFJL7F7L--7F7|||-F7|L7F7||L-7L7|F-J||||L--7FJ||F-J||LJFJ||F7FJ|||L7||L--7L7F---J|L--77F|FF7L7-F|
||F7J||-FJL7JL|L7F7LJL7F7F7F7||L-7L-7FJL7|FJFJF-J||-F7||LJ|L7|||FJ|LJL-7|FJ||JFJ|||F7.|L-JLJF-JL7FJ-||||L7|||FJLJF--JFJL--7||F--J|FJF7J|L.F|
F|-|L7J7||7LF---J|L7F7LJLJLJLJL--JF-JL-7||L7L7L-7|L-J||L7-L7|||||FJF7F-J||FJ|FJFJ||||FJF---7L7F-J|F7||||FJLJLJF-7|F-7L---7L-JL7F7F7.-7-J.F7|
||JLL7.|--7JL7F-7|JLJL---7F7F7F7F7L--7FJ||FJFJF7|L--7||FJF-JLJ|||L7|LJF-J|L7LJFJFJLJ|L7L7F7L-JL7FJ||||||L-7F-7L7LJ|FJFF-7L--7FJ|L-7.||JL7|JL
-L-.FJ-LJ|L-7LJL||F------J|LJLJLJL7F-JL7LJ|FJFJLJF--J||L7L---7|||FJL7FJF-JJL7FJFJF7FJFJFJ|L7F7FJL7|||||L-7|L7L-J.FJL-7|FJ7F7LJJ|F-J-|.F7JL77
.|J7FJ.LF|FLF7F7LJ|F7F7F7FJF----7.|L--7L-7||FJF7LL-7FJ|FJ7F7FJ|||L7FJ|FJF7F7||FJFJLJ-L7L7|FJ||L-7|||||L-7||FJF7F7L7F-J||F7|L---JL777L------7
-FF--.J.LJL7|LJ|F7LJLJLJLJ|L7F-7L-JF--JF7|||L7|L7F7|L7||F7|||FJ|L-J|FJL7|LJ|||L7L7F--7L7|||FJL7FJ||||L7FJ||L-JLJL-JL--JLJLJF7F7F-JF7-L-|JL-|
.||JL|J||L|FL-7|||F-7F7F7F7LLJLL--7|F77|||||FJ|FJ|||FJ||||||||LL--7||F-JL-7|||||FJL7FJJ|||||F-J|FJ||L7|L7|L7F-----7F---7F7FJLJ|L--J|F|LL.|||
--JL7|.7-F7|-FJ|||L7LJLJLJL-------JLJ|FJ||||L7|L7|||L7|LJ||LJL7F7FJ||L7F7FJ||L7||F7|L7FJ||LJL7FJ|FJ|FJL-JL-JL----7LJFF7LJLJF-7|F7F-J-F-LFJ|7
|-JL|--J|LLF7L7LJL7L----------------7||F||||.|L7|||L7LJF-J|F--J||L7||FJ|||L|L7LJ|||L7|L7|L7F-JL7|L7||F7F---------JLF7|L--7FJFJLJ|L7J-|7.LLLJ
JJ|FJJJ-7JF|L7L-7FJLF--7F7F7F7F7F---J|L7||LJFJFJ||L7L7LL7FJ|F7FJ|FJ|||FJ|L7|FJF-J||FJL7|L7|L-7FJ|FJ|LJ|L-7F-----7F7|||F--J|FJF7FL-JLFJ7F7FL7
|FF77|.F77FJFJF-JL-7L-7LJLJLJLJ|L---7|FJ|L7JL7L7||-L7L7FJL7|||L7||FJ|||||FJLJFL7FJ|L7FJ|F|L7FJ|FJ|FJF-JF-J|F----J|LJLJL-7FJL-J|F7|.F-JLF-7-|
LFJLJ-FJL7L7L7L---7|F7L---7F--7L----JLJFJFJF7|FJ|L7FJFJ|F7||||FJLJL-J||FJ|F----JL7|FJ|FJFJFJL7||FJL7L-7L--JL7F7F-JF-7F-7LJF7F-J|L-77JJJ.FF.|
JL-7L7L7FJ|L7L7F7FJLJ|F--7LJF7L-7F---7FJFJFJLJL7|FJL7|J||LJLJ|L---7F-J||FJ|F-7F7FJ|L7LJLL7|-FJ||L-7L--JF7F-7LJLJF-JFJL7L--JLJF7|F-J|L7FFF-77
LF.LJ|.|||F7|FJ||L--7||F7L--JL--J|F-7|L-J|L7F7FJLJF7|L7|L---7|F-7FJL-7||L7||FJ||L7L7L-7F-JL7L-JL7FJF---JLJ-L-7F-JF7L--JF-7-F7|||L--77L-J7JL7
.|FLFF-JL-JLJL-JL---J|LJL-7F7F7F-JL7|L-7F7FJ||L7F7|LJFJL7F-7||L7LJF7L|||FJLJL7|L-JFL7FJL-7FJF---JL7L-----7F-7|L--JL---7L7L-JLJLJF--JJ.|||7L|
FLF.LL7F7F----7F7F--7L7F77LJLJLJF--JL--J||L-JL7LJ|L7FJ-FJL7LJ|FJFFJL-J|LJ.F--JL----7||F--JL7L-7F-7|F-----JL7|L7F7F-7F-JFJF7F7F7FJ|.L|-FJ|7-7
LJ|-L7LJ||F-7|LJLJF7L7LJL------7L-------J|FF7FJF-JFJL-7|F7L7FJL-7L---7L7F-JF--7F--7||||F-7FJF-JL7||L7F7F---JL7LJLJ7LJF-JFJLJLJLJF77L7LL-|.|F
.F77.-.|LJL7L7F---JL7|F--7F--7FJLF---7F-7L-J|L7L-7L7F-J||L7|L7F7L7F-7L-J|F-JF-JL-7LJ||LJFJ|FJF-7|||FLJ||F-7F7L----7F7L7FJF---7F7||7--.|7|.-J
J7|.F|F7.F7L7LJF-7F7LJL-7LJF-JL-7L--7||-L7F7L7|F-JFJL7JLJFJ|-|||FJL7L---J|F7L-7F7L-7LJF-JFJ|FJLLJ||F--J|L7LJ|F----J|L-JL-JF--J|LJL-7JF-7L7J7
L|L-7-F77|||L--JL||L----JF-JF--7|F--JLJF7LJL-JLJF7L-7|F7-L-JFJ||L7FJF-7F7LJ|F-J||F-JF7L-7|L|L7F--J||F--J7L-7|L-----JF7F--7|F7L|F---J-FJ|L7L|
LL..LF||FJL---7F7||F--7F7L-7|F-JLJF--7FJ|F7F7F7FJL-7|LJL7F--JFJL7||FJFJ|L7FJ|F7||L--J|F-JL7|FJL-7FJLJF7F7F-JL-------J|L-7|LJL-JL--7J-F7|-JFJ
7-J.LFJLJF---7LJ|||L-7LJ|F7LJL7F7FJF-J|FJ|||||||F--JL--7||F-7L-7LJ||FJFJFJ|FJ||||F7F7||F-7||L7F-J|F7FJLJLJF7F-------7|F7||F----7F-J7JL-.7JL7
J.L-FL---JF7-L-7|LJF7L7FJ|||F7||LJ|L--JL-JLJLJLJL-----7||LJFJF7L-7|||FJJL7|L7|||||||LJLJ|LJL-JL--J||L-----JLJF------JLJLJLJF7F-J||F7.7---7||
LL-LLF-7F7|L---JL7J||FJ|FJ|FJ|LJF----7F-7F7F----------JLJF7|FJ|F7||||L--7LJFJ|||||LJ7F-7F7F7F--7F7|L77F77F--7L------7F-----J|L-7L-JL-77|.LL7
L7.|.L7|||L-7F--7|FJLJFJL7LJFJF7L--7-LJ|LJ|L--7F7F7F-----J|||FJ||||||F7FJF-JFJ||||F--JFJ|LJ|L-7||||FJFJL7L-7|F--7F--J|F--7F7L-7|F7F7FJ.L-7LL
.|FFFFJLJL-7LJF-J|L--7|F7L-7L-JL7F7L----7LL7F-J|LJ||F-7F-7|LJ|FJ|||||||L7L7FJFJ|LJL--7|FL7FJF-JLJLJL-JF7L--J|L-7|L---JL7.LJ|F-J||LJLJJ..L--|
FF---JF7F-7L--JF7L---JLJL7FJF--7LJL--7F7L7FJL--JF7LJL7LJFJL-7|L7LJ|||||FJL||FJFJF----JL--JL-JF-7F7F-7FJ|F--7|F7||F---7FJF77LJF7LJ|F7FF77..FJ
LL-7F7|LJ-L7F7FJL7F---7F-JL-JF-JF7F7|LJL-JL---7FJ|F-7L-7L---J|FJF-J||||L-7LJL7L7|F--7F-7F7F-7L7LJLJ.LJJLJF-JLJLJLJF-7|L-JL---JL7F7||FJL77.L7
FFL|||L7|F7LJLJF-J|F--JL-----JF7|LJL7FF-----7FJ|LLJFJF7L---7LLJJL7FJLJL7FJF7F|FJLJF7LJ|LJ||FJFJ|F7F-----7L------7J|FJ|F7F7F----J|||||F-J|.FL
FF-LJL-J-|L-7F-JF7|L7F7F-7F-7FJLJF--JFJF----JL7L-7JL7||F---JFF--7||LFJJLJ-|L-J|F--JL-----J|L7|FFJLJF-7F7L-------JFJ|LLJLJLJF--7FJLJLJ|-F7-F7
|L|7|F7F7L-7|L-7|||FLJ|L7|L7||F--JF--JFJLF---7L-7L-7|||L---7FJF-J||.|LFJJ-L7F7|L7F-7F7F7F7L7||FJF7FJFJ|L----7F--7L7L----7F7|F-JL7F---JFJ|-F7
.F---J|||F7|L--J|LJF--JFJL-J||L7F-JF-7L--JF--JF7L--JLJ|F---JL7L-7LJ.|..FF-LLJLJFLJFJ|||LJ|FJLJL7||L7L7|F---7LJF7|FJF----J|||L-7FJL----JFJ7|J
FL-7F7LJLJ||F--7L77L7F7L-7F7LJ|LJF-J-L----JFF-J|F-7F7LLJF--7FJF7L77|.FF7JF.L|.F7.FL7|||F-J|F--7|||FJFJ||F-7L--JLJ|FJF7F7FJ||F-J|F------J7F|7
FLFJ|L7F-7LJ|JFJFJF7LJ|F7||L----7L-7F7F7F7F7|F-JL7LJL-77|F-JL7|L-J-J-FL-7.L.-L|L7J7LJ||L--JL-7||||||L7|LJFJJF----J|FJ|||L7||L7FJ|F7F7F7F7JF7
|JL7|LLJFJF7|FJFJ-|L7FJ||||F7F-7L-7LJLJLJLJLJ|F7FJF---JFJL---JL-77FF-LJ.L..FLL|FJ7FF7LJJF7F--JLJ|LJF7LJF-JF7|F----J|FJ|L7|LJFJL7LJLJLJLJL7L-
L77LJF--JFJ||L7|F7L7LJFJ||||LJFJF7|F7F----7F-J|||FJF7F7|F----7F7L---7.L..F-F-.||F7FJ|F7FJ|L--7F7|F-JL77L-7|LJL-----JL7|FJ|F-JF-JF--7F---7|-L
F|LF7L--7|LLJ|LJ||||F7|7LJLJF-JFJ|LJLJF--7LJJFJLJL-JLJLJL---7||L----J7LF7.||JFJLJLJFJ||L7|F-7LJLJL-7FJF7FJ|F-7F-----7LJL-JL7FJF7L-7|L-7FJ|FL
F77||JJ7LJF7F-7FJ|FJ|||F----JF7|FJF---JF7L7F-JF-7F---7F-7F--J|L------77L7-LFFL7F--7L-JL7|||FJF7-F--JL7||L-JL7|L----7|F--7F7LJFJ|F-J|F7|L7|7J
F--J|F77LL||L7LJFJL-JLJL7F---JLJL-JF---JL-JL7FJ.|L--7|L7LJF77L7F7F7F-J7|L--F-7LJF-JF7F7LJ|||FJL7L7F-7LJ|.F--JL7F7F7||L-7|||F7L7||F7||LJ|||7|
L--7|||F7FJL7|F7L7F-7LF7LJF-7F7F7F7L---7F7F-JL-7L---JL-JF7||F7LJLJLJ-L|-|7FL7L7J|F7|||L-7LJLJF7L7LJFJF7L-J7F7FLJLJLJ|F-JLJ||L7||LJ||L7J-LJJJ
F--JLJ|||L-7|LJL7||FJFJL7FL7||LJLJL----J|||F---JF-7F-7F-JLJLJL7FJJJJF-|..-F-L7L-J|||||F-JF7F7|L7L7|L7|L--7FJL------7|L-7F-JL7|||F-JL-J-||.||
L7F7F7LJL--JL7F7|LJ|FJF-JF-J|L----------J||L---7|FJ|FJL7F--7F-J7..|.L-FJ77.F||F7FJLJLJL--J||LJ7L7L7.LJFF7LJF7F7F---JL-7|L7F-JLJ|L-----7FJFL|
L||LJL7F----7|||L-7||FJF7L-7|F7F---------J|F--7LJL-JL7-||F7LJJ7|FF|7LFJL|-.F-LJ||F-7F7F---JL7F7FL7L----JL--JLJLJ|F7F7L|L7||F7F-JF7F---J7-7L-
.LJJL|LJF7F7|LJL--JLJL-JL--J|||L-----7F7F-JL-7L------JFJLJL-77F7LF7|7LJF||FL7L|LJL7|||L-----J||F-JF--7F-----7F7F-JLJL7L-J|LJ|L--J|L-7F7J7F7|
FL|JFF--J||LJF---7F7F7F7F7F7|||F-----J||L-7F-JFF-----7|F-7F-JFJ|FJL-77|7L-.LF.F---J|||F7F7F-7||L--JF7LJF7F-7LJLJF---7|F-7|F-JF7F7L-7LJ|F7.L7
F7|F-L7F7LJF7|F--J|||||||LJ|LJLJF7F---JL--JL7F7|F----J|L7||F7|FJ|F7FJ-L77.7J|FL---7LJLJLJLJFJ||F---JL--J|L7L7F-7L--7|||FJLJF-J|||F7|F-J||7-J
LJ.|-FLJ|F7|LJL7F-JLJLJ||F-JF7F7|||F7F7F7F-7LJLJL-7F7FJFJLJ|LJL-J|LJJ--|FF|7F-----JF7F7F7F7L-J|L-7F7F7F7L-J.LJ|L---JLJ|L7F7L7FJ|LJ|LJF-JL7-J
.|F-FLF-J|||F--JL7F----J||F7|LJ|||||LJLJ|L7L------J|||FJF7FJF7F-7|F|J77L7LL7L--7F--J||||LJL7F7L7.LJLJLJL---7F7F7F7F7F7|FJ|L7||FJF7L--JF--J-J
F|..LJL7FJ||L7F-7|L7F7F7|LJLJF7||LJL7F7-L7L-7F-----JLJL-JLJFJLJ.||-J.L|J7J.LL|FLJJF-J|||F-7LJL7L7.F7F7F----J||||||||||||||FJ|LJFJL---7L-7L|J
|.|7.|LLJFJ|FLJFJL7||LJLJ|F-7||LJF--J|L-7L-7LJF------------JF--7LJ|F7F7F7-7.LFF7-FL7FJLJL7L---JFJFJ||LJF----JLJLJLJLJLJL-JL7|F-JF--7JL7FJ7L7
7-JJ77L|LL-JF--JF7|||F-7F7L7LJL-7L-7FJF7L7J|F7|F7F7F7F7LF-7|L-7|-F7|||L7|F-7LL|L7F7LJF7F7L----7L7L7|L-7|F-7F-7F-7F-7F-7F--7LJL--JF7L-7||F|FF
--LJJF.7.F7JL--7|LJ||L7LJL-JF7F7L-7LJFJL-JFJ||LJLJLJLJL7L7L7F7||FJLJ||FJ-L7|F7|FJ||F7|||||F7F7|FJFJL7LLJL7LJFJ|FJ||LJFJL7FJF7F-7FJL--JLJ-|F7
JJFL-J-F-JL----J|F-J|FL7F7F7|||L-7L--JF7F7L7||F7F-7F7F7L-JFJ||||L--7||L7F-J|||||-|LJLJLJL-JLJLJL-JF-JF---JF7L-JL7L--7L7FJL-J|L7|L-----7F--JJ
LF--.LFL--7F-7F7|L7FJF7LJLJLJLJF-JF---JLJ|FJ||||L7LJ|||F-7L-JLJL7F7|LJFJL7FJ|LJL-JF7F7F-7F-7F7F7F7|F7L----JL7F7F|F-7L7|L---7|FJ|F--7F7L7J|.F
.J.F-7LLF-J|J||||FJL7|L------7FJF7|F-----JL-J|||FJF-J||L7L--7F7FJ||L-7L7FJL7L-7F-7|||||FJ|-LJLJLJ|LJL-------J|L7|L7|FJL7F7FJ|L7||F7LJL-J-7.7
F7F-L77||F7|FJ|LJL-7|L7F--7F7|L-JLJL------7F-J||L7L-7|L7L-7FJ|LJ7||-FJFJ|F-JF7||LLJLJLJL-JF----7FJF7F----7F--JFJ|FJ||F7LJ||7L-J|LJL---7|-L7|
LFL.LF--J|||L7L-7F-JL7LJF-J||L------7F7F--JL--J|FJFFJL7L7FJ|FJF--JL7L7L7||F7||LJF----7F7F7L---7LJFJLJF7F-J|F--J7LJ-LJ|L7F|L--7FJF----7L7.-J|
F7LF-L-7FJ|L7L--JL7F7|F-JF-JL7F7F7F7LJLJ.F7F--7LJF-JF7|LLJFLJ-L---7|FL7LJLJ||L-7L-7F-J||||F-7FL--JF--J|L--JL7F7F-7.F-JFJFJF-7|L7L---7L-J-JJ|
J|7|FL.LJ.L7|F----J|LJL-7|F--J|||LJL--7F7|||F-JF7L--J|L--7F7F7F---JL7LL--7FJL7FJF7||F-J|||L7L-----JF-7L-----J|||FJFJF7L7L-J-LJFJF7F7L--77-.L
||FJJF-|-JJLJL-----JF--7||L-7FJ||F---7LJ||||L--JL7-F7L---J||||L----7L7|F7||F7|L7||||L-7LJL7L7F--7F7|FJF-----7|LJL7|FJL-J-F-7F7L7||||F7FJJJ|.
FL--77L|-7.|J|FLF--7L-7|LJF-J||LJL--7|F-J|||F----JFJ|F7F7L|LJL7.F7FL7|FJ||||||FJ||||F7L-7FJ-LJF7LJLJL7|F----J|F--J||F-7F7L7LJ|JLJLJ||||.J7|7
7.|LJJF|7FF7--F-JF7||FJL-7L-7|F-7F7FJLJF-JLJL-7F-7|FJ|||L7|F--JFJL--J|L7||||||L-JLJ|||F-JL7.F-JL-7-F-J|L-7F--JL7F-JLJFJ|L-JF-JF---7||||J.-77
|-|J7.FJLL|L7.L--J||FJF7FJF7LJL7LJ|L---JF7F---J|FJ|L7|||FJ||F-7L--7F7L7|LJ||LJF----J||L7F7L7L7F-7L7L7FJF7LJF7F7LJF---JL|F--JF-JF--JLJLJFFJL7
7JFJ|F7|FLL7|F--77||L-J||J|L---JF7L-----J|L--7FJL7L7LJLJL-JLJFJF-7LJL7LJF-JL-7L7F-7FJ|LLJ|FJFJ|LL7L7LJFJL-7|||L7FJFF7JFJL---JF7L7.L|JJF--.-J
J|L7|LJ7||F|LJF7L-JL---JL7L-----JL------7L---JL7FJFJF----7F-7L7L7|F-7L7FJ|F-7L7||FJL7|F7FJ|-L-JF7L7L--JF-7LJLJFJL--JL7L7F----J|FJ-LLJFL-|-LJ
LF7--L-F|7FL-7|L--------7L--------------JF---7FJL7|FJF7F7LJ7L-JFJ|L7|7|L-7L7L-J||L7FJLJ|L7L-7F-JL7L----JFL7F-7L------JFJL----7LJ.F7J-JFF--7.
|L|.LLFJL7J-L||F--------JF--7F7F-7F-7F7F7L7F-JL-7LJL7|||L-7F7F7L7|FJL-JF-J7L--7LJFJL--7L-JF-JL7F7|LF7FF7F-J|FJ.F7LF---JF-----JF7-|J7J.-FJFJ.
L--J.|L-7|LLFJ||F--------JF-J|LJFJ|FJ|LJL-JL7F--JF--J|||F-J|LJ|FJ|L--7FJF7F7F7|F-JF7F7L--7|F-7LJ||FJL-J|L--JL7FJL-JF7F7L--7F7FJL77.|FJJLL-.F
.F7JF|.L7F-.L7|||F---7F-7-L--JF-JFJL-JF---7FJL--7L-7FJ||L7.L-7|L7|.F-JL-J||LJ|||F7|LJL7F-JLJFJJFJLJF7F7L--7F7||F7F7|LJ|F7FJ|||F-J|L-F7|LJ-L7
.||JL.-JFFJ7LLJLJL--7|L7|F7F-7L-7|F--7L--7LJJF--JF-J|FJ|FJF7FJL7|L7L-7F-7||F7LJ|||L7F-JL-7F-J.FJF-7|LJ|F-7LJLJ||LJ|L-7|||L-JLJL7F--LJJ-F-7-|
-|JFJFJFF7JFFF------JL-JLJ|L7|F7LJL-7|F-7L7F7L7F7L-7|L7|L7||L7FJL7L7FJL7LJLJL7FJ|L7|L7F7F||-F7L-JFJL7FJ|LL----JL7FL7FJ|||F7F7F7L77.F|7-LJL.|
|J|L7|LF||7.FL7F----7F-7F7L-JLJL-7F-JLJFJFJ||-LJ|F-J|FJL7|||FJ|F7|FJL-7|F7F7F||FJFJ|FJ||FJL-JL--7L--JL-JF----7F7L-7|L7LJLJLJLJL-J7F|J|7J7-F|
L7-|F7|FJ|77LL||F--7LJ7|||F-7F--7|L---7L-JF||F7FJL7|||J||||||FJ|||L7.FJLJ|||FJ||FJFJL7|LJF------JF77F7F7L---7||L--JL-JF---------7|F7FF7-7-LF
||FF7F7L7L7|F|LJL-7L---J|||FJ|F-JL----JF7F7|LJ|L-7L-JL7FJLJ||L-J|L7L7|F--J|||FJ|L7L7FJL-7L7F7F7F7||FJLJL7F--J|L7F77F--JF7F7F7F--JFJL-J|7L-LL
FF-JLJL7L7|F77LF7FJF----JLJL-JL--7F77F7||||L7FJF-JF---JL-7FJL7F-JFL7||L--7|||||L7|L||F7J|FJ|LJ|||||L---7|L--7L7LJ|FJF7FJ||||LJF--JF--7|JFFLL
LL-7F-7L7|LJ|F7||L-JF-----7F7F7F7LJL7|||||L-JL7L-7|F7F--7|L7FJL7F7FJ||F--J|LJ|F-JL7||||FJL7L-7||||L7F7FJL---JL|F7LJFJ|L7|||L--JF7FJJLLJ-LL7.
|.L||-L7LJF-J||||F-7L----7||LJLJL---J|||||F---JF7||||L-7||FJL7FJ|||FJ||-F7L7FJL-7FJ||||L7FJF7|||||FJ||L-----7FJ||F7L7L-JLJL-7F7|LJJ.LLJ7LLLF
7-J||.F|F7L-7||||L7|F7FF7||L--7F7F7F7|LJ||L7F-7|||LJ|F-J|||F7||FJLJ|FJL7|L-J|F-7|L-J|||FJ|FJ||||||L7||F7F---JL-JLJL-JJF7LF7F|||L7|LL7FF-7|F|
LL-LJ-FJ||F7LJLJL-JLJL-JLJL---J|LJ|||L7FJ|FJL7||||F-JL7FJ|LJ|||L-7L||F-JL7F-JL7|L-7J||||FJL7LJLJ||L||LJ|L-7LF---------JL-JL7||L7L-7F--J|L77J
7-FL|FL-J||L-7F7F7F-7F7F7F7F--7|F-J||FJ|FJ|F7||||||F7FJ|FJF-J||F-JFJ||F7FJ|F7FJ|F7L7|||||F7L---7||FJL-7|F7L7|F----7F-7F-7F-JLJF|F7|-JJ.L7|77
F-LJFF.|J||JL||||||FJ|||||LJF-J||F7||L7||FJ||||||||||L7|L7L7FJ|L-7L7|||||FJ|||FJ||FJ||LJLJL7F--JLJ|F--J||L7LJL---7||FLJ-|L--7F7LJLJ7J.F-F|J|
.7..JJFJ-||F-J|LJLJL7|||LJF-JF7|||||L-JLJL7|LJ||LJ|||FJ|FJJLJ|L7FJFJ|||LJL7|||L7|||FJL7F---JL-7F7FJL-7F|||L7F-7F-JLJF7F7|F-7LJL7LL.F-FJF---7
|L77L--|.LJ|F-J7F7F7LJLJF-JF7||||||L-7LF--JL-7||F-J||L7||F-----JL7L-J||F--J|||FJ|||L7FJL--7F7FJ||L7F7L7LJF7|L7|L--7|||||LJFJF7FJFJ-|.L7L-FFJ
|7|JJ7F-7-FJL7F-JLJL-7F7L-7|LJ|||||F7|FJF-7F7|LJ|F-J|FJ||L7F7F7F7L--7LJL--7|LJL7|||FJL--7LLJ||FJL-J|L7L-7|LJFJ|F--JFJLJL-7L-JLJF||.|FJ-JF-JJ
|LJ.7-|..LL--JL--7S-7LJL--JL7-|||||||||FJ7LJ|L-7||FFJ|F|L7LJ|||||F-7L----7|L-7FJ|LJL7F-7L-7FJ||F-7FJJL7FJL-7|-|L---JF--7FJF7F--7-7JJ|L|F7FF|
FL.F-.LJ-7J|.J.F-J|LL7F7F7F7|FJ|LJLJ||LJF7F-JF7||L7L7L7L7|F-J|LJ||FJF-7F7|L-7|L-JF--JL7|F-JL7|LJFJ|F-7|L-7J|L7L7F7F7L-7|L-JLJF-JLLFLJLLJ--F|
|.-77L|7.LLJ-|LL--JF-J|LJLJ||L7L-7F-J|F-JLJF7||||FJFJFJFLJL7FJ.FJ|L7L7|||L-7LJF--JF-7FJ||LF-JL77L7||FJL7FJFJFJFJ|LJL--J|F7F--JJ.F||FL7.F|JFJ
L|7||-L--7.|.F.F---JF7|F-7J|L7|F7||F7||F---J||LJ||FJFJFF---JL-7L7|FJFJ|||F7L-7L--7|FJ|||L7|F7FJF-JLJL7.LJL|FJLL7L7F---7|||L---7-LL77.L7L7-LJ
LLFLJ7.J.LJ.J.FJF7F7|LJL7L-JFJ||||||||||F---JL7|LJL7L-7L---7F7L7||L7L7LJ|||F7L7F-J|L7|FJFJ||||-L-7F7FJF7F7||F7|L7LJF7FJLJL7F7FJ7J|LLFL-7|F||
F.-7F-7-JJLFJFL-J||LJF77L7F7|.LJLJ||||||L7F7F7L---7|F7|F---J|L7|LJ7L7L7J||LJL-J|F7||LJ|FJ7||||F--J||L-JLJ||LJL-7|F7||L---7||LJ.77LF--.L|7-|7
-7|--|J7.||LJFJ7|||F-JL7F|||L7F-7-||LJ||7LJLJL-7F-JLJLJL--7FJ7|L-7F-JFJFJL----7||LJF7FJL-7||LJL7F7||F-7F7|L--7FJ|||||F7F-J||JLFLL-|J|..|.7LL
|J-..LL|--F.-J|F-J||F-7L-J|L7LJFJFJL7FJL7F-----JL---7F7FF-JL-7L7FJL-7L7L7F7F-7|||F-JLJF7FJ|L7F7LJ|||L7LJ|L--7|L7LJLJ|||L77LJF-.LF7.F7.FJ-F.|
.|7LL7.J7.-7.--L7FJ||FJF-7|FJF7L7L7FJ|F7|L--7F-7F7F-J|L-JF-7FJFJL--7|FJFJ|||L|||||F7F7||L7L7LJ|F-J||FJF-JF-7|L-J||-||||FJF7L|.FF|J-F7JL|J.7-
--7LLL-J|7.|L7|.LJFLJ|FJJLJ|FJ|FJ|LJ|||||F--JL7LJ|L-7L7F-J||||L--7FJ||FJFJ|L7||||LJLJLJ|FJ7L7FJ|F7||L7|F-JFJL----77LLJ|L-JL7J7LF|JJ.-7F7F7.|
|FL7.J7-7.-LJL7-|J7-FJL-7F-J|FJL7F7F7LJLJL---7L-7|F7|FJ|F--J|F-7FJ|FJ||FJFJFJ||LJF-----JL7F-JL7||LJ|FJLJF7L--7F7FJF|LLL7F-7|.7FFF---JJ..J7.-
JJF-JFJ7|..|-LF.|7JJL7F-JL-7||F7LJLJL7F------JF-J||||L7|L--7LJFJL7||FJ|L7L7|.||F7L7F-7F-7||F7FJLJ.FJL---JL-77||LJJF7J||LJ-LJ7J|LJ..L|F77|LL.
L-J-||F7-7.7-JF7.|JF-J|F---J|||L-7F7FJL------7L-7|||L7LJ7F-JF-JF-J|LJF|FJFJL7|||L-J|-|L7||LJ|L---7L7F7F-7F7L7||J.LL7FF-7L-|FJ7.FJLFFLJL--L|7
F||FLJF7F77LF7|.FF-L-7||F7F7||L-7LJLJ.FF-----JF7|||L-JJF-JF7L77L-7L-7FJL7L7FJLJL7F7|FJFJ|L-7|F7F-J-||||FJ||FJLJJ-JLJ|JJ|JJFL7L.L7|.7J|LJLF-7
|LJ7F77|L|7.|JJL7L-J-||LJ||||L--J7JL--FL-7F--7|||||F---JF7|L7L-7FJF-J|F-JF|L7JF-J||||FJ|L7FJ||LJ7F-J||||FJ|L7|.L-7LLL7FLJ.|7L---L7.|.|JL-7FL
7FL7J|||-|-F.L7||JJF-||JJ|||L--7.|||7|F--JL7FJ|||||L7F--J|L7|F-JL7L-7LJ7LFL-J-L-7|LJ|L-7FJ|FJL7F-JF7|LJLJFL7|7.L--FJFJJ.77L7-77..LF.FL-L|L7J
--||LF|..J-J7F-7||FLFLJ-F|||F--JJ|FL-7L7F7FJ|FJ|||L7LJF--JFJ|L--7L7FJF|--J|J.F|7LJ|FJF7|L7|L7FJ|F7||L-7F7F7LJL-7|F|FJJFLL7-LF|-777L-J7|FF7J7
.-JFF|.LJJJ7FJ|-|FJFFJ-F-||LJ7LL-F|FL7LLJ||.LJFJ||FJF7L--7|7L7F-J-|L77J|J7|.F7JFF--JFJ||FJL7||LLJ||L-7||LJL7.|-L-7LJ-F|LJ.77FL7LJJ-LL7-J||F7
L|FLL77.LLL|LF|7FJ.LL|F|L||JJ-J.7LF-7F-7FJL7F7L7|||FJL---JL-7|L-7-L-JF7|.-J7|F-FJF-7L7|||F-JLJJF-JL-7|LJF7FJF77FL-JL7|J7J.7-JL|J|J7.F|7.F-LL
|L-FJJL7-|7|--7FJJJ.||FL-LJL|F7FF..|FL7LJF7LJL7|||||F----7F-J|F7L--7F|L7-LL|J|LL-JJL7|||LJJ.JJF|F-7FJ|F7|LJ.7|.7J7.7LJF|.F.L--L-JFLFFJJF-7.|
7JJJJ.LF---JFJL--|.FJFL7-JJ7-F-JJ..FF-JF7|L7F7|||||||F---JL-7LJL7F7L-JFJ.||.-7LJFJ-LLJ||7J7FJLFLJJLJ||||||7L-L7LJ-JLF-|J7FF.|..L-7.|L7-LJF7J
L-JJFF7L7JL-J|FJFL7|-7|.|FL|JJL|--L|L--J|L7LJLJLJLJLJ|F7F7F7|JF7||L7F-JJF|.|J||-J-7L|FLJJJFF77.L|L|-LLJLJJ-LLJLJ.F|FJJ.FJL||7.F|.7FFFJ7L|JJ7
F|J.FF7JL-7JFFJL-7FJ7J7-L-7.7---.F|LJ|.FL-JJJ||J.|J|LLJ|||||L-J|LJ||L7J|-|FL-7J7FF|F7-LLJF-J|LF7JFL.F|.J|7.7.7|L7-JLJ..||-FJLFJ|.L-JJ7F.F..-
F|F---7-F--.L|JFFJJ|.LJ-7L--77LJFJ7F7--J|J7|L7|F--FJFLJ||||L7F7L--7L-J-L-L7.|LL77-LF7.|LLJ|FJ.J-FJ.J7||.J77JFF7J|||L|JF7-7L-|LJJ7J.LJF|---|7
JLJ7.LLJJF-77|-L7LL7-.|FJ7L7LF7LF|LL7F|JF7|J-FL-7.JF-|-LJLJFJ||F-7|JJLL||FJF7.LLJJ.|L-JF7.F7L7L--J7FJ7F7FJF-7LJ-LJJ-J7F|LJ-FJ.L..F|J.L..J7FJ
LF-F.JFF-F-LJJ-LF.J..FF7LFJ|.LL77-7L77J-|JJ7.J-LFF-7.7JLJ.F|FJ|L7LJ.F--JJL77L|-J|-FJ7|L77-F|7L-JJF|FL-LL7|JFF77FJJL--7-L-L-L.FLFJ|F.|.FL|.J.
|.L77.L-7|||F|J.7-7F7||F-.LFJ7|L|J.LL7L7|-FLJJF-JLL.-777|FFLJL|FJ-LF-LL|.FLJ-|LFF7|.-J.7J-LJ|.7FL-JLJ-L|7JLFJ.|7L77||JFL-.|7.LJF7---7-FJ|FJ7
F|.LJ.|LFFJJ77-JJ-LL-JLF.FJ.LLF-JF77JL7-L-7JJ-JLLFJ.|.L--JJJ-LLJ.L-JL--L-JJJ-J--L-F.|L--7J.7J-LL...|J-L---L|J|JJ.LF-|.F-L.-J.L.L|.L.L-|.F|.J
""";