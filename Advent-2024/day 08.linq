<Query Kind="Program">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>


void Main()
{
    part1And2();
}

void part1And2()
{
    var lines = data2
                .Split(Environment.NewLine)
                .ToArray()
                .Dump()
                ;

    var width = lines[0].Length;
    var height = lines.Length;

    var antennas = new List<(int, int, char)>();
    for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            var c = lines[y][x];
            if (c != '.')
            {
                antennas.Add((x, y, c));
            }
        }

    antennas.Dump();

    var groups = antennas
                .GroupBy(a => a.Item3)
                .ToArray();

    // part 1
    //var nodes = groups.SelectMany(SimpleNodes).ToArray();
    var nodes = groups.SelectMany(AllNodes).ToArray();

    nodes
    .Distinct()
    .Where(inMap)
    .Count()
    .Dump();

    //////////////
    
    IEnumerable<(int, int)> SimpleNodes(IGrouping<char, (int, int, char)> grouping)
    {
        var groups = grouping.ToArray();
        // get all pairs of items.
        for (int i = 0; i < groups.Length; i++)
            for (int j = i + 1; j < groups.Length; j++)
            {
                var p1 = groups[i];
                var p2 = groups[j];
                var deltaX = p1.Item1 - p2.Item1;
                var deltaY = p1.Item2 - p2.Item2;
                yield return (p1.Item1 + deltaX, p1.Item2 + deltaY);
                yield return (p2.Item1 - deltaX, p2.Item2 - deltaY);
            }
    }

    bool inMap((int, int) point)
    {
        return 0 <= point.Item1 && point.Item1 < width
            && 0 <= point.Item2 && point.Item2 < height;
    }

    IEnumerable<(int, int)> AllNodes(IGrouping<char, (int, int, char)> grouping)
    {
        var groups = grouping.ToArray();
        // get all pairs of items.
        for (int i = 0; i < groups.Length; i++)
            for (int j = i + 1; j < groups.Length; j++)
            {
                var p1 = groups[i];
                var p2 = groups[j];
                var deltaX = p1.Item1 - p2.Item1;
                var deltaY = p1.Item2 - p2.Item2;

                for (var k = 0; ; k++)
                {
                    var node = (p1.Item1 + (deltaX * k), p1.Item2 + (deltaY * k));
                    if (!inMap(node))
                        break;

                    yield return node;
                }
                for (var k = 0; ; k++)
                {
                    var node = (p2.Item1 - (deltaX * k), p2.Item2 - (deltaY * k));
                    if (!inMap(node))
                        break;

                    yield return node;
                }
            }
    }
}

// example data
const string data1 = """
............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............
""";

const string data2 = """
...........6.b....................................
........6................8........................
..Y.......................................o.......
....V...j............B.............c..............
............8.........X.......L...................
.....j..v6.......3.L..................c...........
..Mj.....p3.......b........Z....................J.
..........M...X...................................
V..............v......p.........Z.........c.......
..............3...................................
.......V......U3.............c....................
..........b..v.M.U8...............................
..........j........8.....................J........
..........Y......q........LH..Z...D...........y...
..2Y........PX......6..................BQ.........
...0.Y...............XP...........w...............
.........U.......2...............oH.y.............
0..............9........U.........................
...........P..............W.......z...Oy..........
...................t...p.W..o.............Q.......
.....S.................t.....Q....B...............
S.k..................V..W...p.......H...O......m..
....S.h................W.......................O..
..h..P.2.............Z.............J..............
.........k.......5v.......q...t.s.................
.....Q.....h..........................J...B.......
........0.........l...............................
.S................................................
.............................M....................
2..................e.....o.....y..................
................k.................................
......4......k....t...s.q.........................
.4.......................q........................
.......................z....E.....................
.............0.....d..............................
7..........D........z.............................
.......D..5......7..9.............................
......5..................E........................
D..............K......d..9E..........w.....1..C...
.......K..x.........d....s...........l............
........7......................u...C..............
..K........x..............9..C...u................
4..............s.........................l...T..w.
.......5.....7..................m......T......1...
...........................E...z.m................
......................................u...C.......
.............................em...................
..............................................T...
....................x.......................e.....
.............................1e....w....l.........
""";

const string data3 = """

""";

const string data4 = """

""";

