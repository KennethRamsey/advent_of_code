<Query Kind="Program">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>


void Main()
{
    part1and2();
}



void part1and2()
{
    var nums = data2
                .Split(" ")
                .Select(d => long.Parse(d))
                .ToArray()
                .Dump()
                ;

    IEnumerable<long> rocks = nums;

    // part 1
    for (int i = 0; i < 25; i++)
    {
        rocks = rocks.SelectMany(NextRocks);
    }

    rocks.Count().Dump();

    // part 2
    IEnumerable<(long num, long count)> rockPiles = nums
                                                    .GroupBy(n => n)
                                                    .Select(g => (g.Key, (long)g.Count()));
    for (int i = 0; i < 75; i++)
    {
        rockPiles = rockPiles
                    .SelectMany(NextPiles) 
                    .GroupBy(p => p.num) // combine like groups
                    .Select(g => (g.Key, g.Select(p => p.count).Sum()));
    }

    rockPiles
    .Select(p => p.count)
    .Sum()
    .Dump();
}

IEnumerable<(long num, long count)> NextPiles((long num, long count) pile)
{
    return NextRocks(pile.num).Select(n => (n, pile.count));
}

IEnumerable<long> NextRocks(long n)
{
    if (n == 0)
    {
        yield return 1;
        yield break;
    }

    var str = n.ToString();
    if (str.Length % 2 == 0)
    {
        var left = n / Math.Pow(10, str.Length / 2);
        yield return (long)left;

        var right = n % Math.Pow(10, str.Length / 2);
        yield return (long)right;

        yield break;
    }

    yield return n * 2024;
}


// example data
const string data1 = """
125 17
""";

const string data2 = """
27 10647 103 9 0 5524 4594227 902936
""";

const string data3 = """

""";

const string data4 = """

""";

