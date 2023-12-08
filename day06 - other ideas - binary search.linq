<Query Kind="Program" />

void Main()
{
    part1();
    part2();
}

void part1()
{
    var nums = data
                .Split(Environment.NewLine)
                .Select(x => x.Split(':')[1].Trim()
                                .Split(' ')
                                .Select(x => x.Trim())
                                .WhereNot(string.IsNullOrWhiteSpace)
                                .Select(int.Parse)
                )
                .ToArray();

    var races = nums[0].Zip(nums[1])
                   .Select(x => (time: x.First, dist: x.Second))
                   .Dump("races");
    races
    .Select(x => winCounts(x.time, x.dist)).Dump()
    .Aggregate((x, y) => x * y)
    .Dump("sum");
}


// binary search is cool, but lot's of code.
// list apparenly has a binarySearch, but hard to use, you have to implement a class.

long binSearch(long start, long end, Func<long, long> test)
{
    for (; ; )
    {
        var mid = (start + end) / 2;

        var compare = test(mid);
        if (compare == -1)
        {
            if (mid == start)
                throw new Exception("never found");

            start = mid;
            continue;
        }
        if (compare == 1)
        {
            if (mid == end)
                throw new Exception("never found");

            end = mid;
            continue;
        }
        return mid;
    }
}


long winCounts(long time, long distRecord)
{
    long shipDist(long holdtime)
    {
        var goTime = time - holdtime;
        var speed = holdtime;
        return goTime * speed;
    }

    long first(long hold)
    {
        var sum = (shipDist(hold) > distRecord ? 1 : 0)
                + (shipDist(hold - 1) > distRecord ? 1 : 0);

        return sum switch
        {
            0 => -1,
            1 => 0,
            2 => 1
        };
    };

    long last(long hold)
    {
        var sum = (shipDist(hold) > distRecord ? 1 : 0)
                + (shipDist(hold + 1) > distRecord ? 1 : 0);

        return sum switch
        {
            2 => -1,
            1 => 0,
            0 => 1
        };
    };

    var start = binSearch(start: 0, end: time / 2, first);
    var end = binSearch(start: time / 2, end: time, last);

    return end - start + 1; // 3-4 is 2 nums, so would be 4 - 3 + 1
}

void part2()
{
    var nums = data
                .Split(Environment.NewLine)
                .Select(x => x.Split(':')[1].Replace(" ", ""))
                .Select(long.Parse)
                .ToArray()
                .Dump("big race");

    winCounts(nums[0], nums[1])
    .Dump("counts");
}



const string data2 = """
Time:      7  15   30
Distance:  9  40  200
""";

const string data = """
Time:        46     68     98     66
Distance:   358   1054   1807   1080
""";