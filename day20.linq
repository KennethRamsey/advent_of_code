<Query Kind="Program">
  <Namespace>System.Threading.Tasks.Dataflow</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>


async Task Main()
{
    await part1();
    await part2();
}

async Task part1()
{
    Modules = data
                .Split(Environment.NewLine)
                .Select(line => new Module(line))
                .ToDictionary(l => l.Name, l => l);

    // init parent/child
    foreach (var kv in Modules)
        foreach (var cName in kv.Value.ChildNames)
        {
            kv.Value.Childs.Add(Modules[cName]);
            Modules[cName].Parents.Add(kv.Value);
        }


    var btnlimit = 1000;
    var btnCount = 1;

    Processor = new ActionBlock<(Module sender, Module receiver)>(msg =>
    {
        msg.receiver.ProcessSignalFrom(msg.sender);

        if (Processor.InputCount == 0)
        {
            if (btnCount < btnlimit)
            {
                btnCount++;
                Modules["button"].ProcessSignalFrom(null);
            }
            else
            {
                Processor.Complete();
            }
        }
    });


    Modules["button"].ProcessSignalFrom(null);

    await Processor.Completion;

    var all = Modules
                .Select(m => (m.Value.LowCount, m.Value.HighCount))
                .Dump();

    (all.Select(a => a.LowCount).Sum()
    * all.Select(a => a.HighCount).Sum()).Dump();

}


static Dictionary<string, Module> Modules = null;
static ActionBlock<(Module, Module)> Processor = null;

enum ModType
{
    none,
    FF,
    con
}

record Module
{
    public string line, Name;
    public ModType ModType;
    public string[] ChildNames;

    public bool ON;
    public bool LastPulseLOW = true;
    public bool LastHIGH() => !LastPulseLOW;

    public int LowCount, HighCount;

    // fill in later.
    public List<Module> Parents = new();
    public List<Module> Childs = new();

    public Module(string line)
    {
        this.line = line;
        var seg = line.Split(" -> ");

        this.ModType = seg[0][0] switch
        {
            '%' => ModType.FF,
            '&' => ModType.con,
            _ => ModType.none,
        };

        this.Name = ModType switch
        {
            ModType.none => seg[0],
            _ => seg[0].Substring(1),
        };

        this.ChildNames = seg[1].Split(", ")
                            .WhereNot(string.IsNullOrWhiteSpace)
                            .ToArray();
    }

    public void SendLow()
    {
        LowCount += ChildNames.Length;
        LastPulseLOW = true;
        foreach (var c in Childs)
            Processor.Post((this, c));
    }
    public void SendHigh()
    {
        HighCount += ChildNames.Length;
        LastPulseLOW = false;
        foreach (var c in Childs)
            Processor.Post((this, c));
    }

    internal void ProcessSignalFrom(Module sender)
    {
        switch (ModType)
        {
            case ModType.FF:
                if (sender.LastPulseLOW)
                {
                    ON = !ON;
                    if (ON) SendHigh();
                    else SendLow();
                }
                break;

            // if high for all inputs, then low, else high.
            case ModType.con:
                if (Parents.All(p => p.LastHIGH()))
                    SendLow();
                else
                    SendHigh();
                break;

            default:
                if (Childs.Count > 0) // btn sends low, but output should not.
                    SendLow();
                break;
        }
    }
}


async Task part2()
{
    Modules = data
              .Split(Environment.NewLine)
              .Select(line => new Module(line))
              .ToDictionary(l => l.Name, l => l);

    // init parent/child
    foreach (var kv in Modules)
        foreach (var cName in kv.Value.ChildNames)
        {
            kv.Value.Childs.Add(Modules[cName]);
            Modules[cName].Parents.Add(kv.Value);
        }


    var btnCount = 1;

    // parents of main rx:  mz, sh, jf, bh

    string SUBPOINT = "bh"; // ran for each from above.

    int high = 0;

    Processor = new ActionBlock<(Module sender, Module receiver)>(msg =>
    {
        msg.receiver.ProcessSignalFrom(msg.sender);

        if (Processor.InputCount == 0)
        {
            var mod = Modules[SUBPOINT];

            if (mod.HighCount > high)
            {
                high = mod.HighCount;
                (btnCount, high).ToString().Dump();
            }

            btnCount++;
            Modules["button"].ProcessSignalFrom(null);
        }
    });


    Modules["button"].ProcessSignalFrom(null);

    await Processor.Completion;
}


const string data2 = """
button -> broadcaster
broadcaster -> a, b, c
%a -> b
%b -> c
%c -> inv
&inv -> a
""";

const string data3 = """
output -> 
button -> broadcaster
broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con -> output
""";

const string data = """
rx -> 
button -> broadcaster
%cd -> jx, gh
%bk -> jp, cn
%px -> xc, hg
%tv -> gh, xl
&xc -> bm, zq, jf, hg, bd, hn
%bd -> px
&bh -> mf
%dx -> cn, rb
%vv -> pp, gh
broadcaster -> cx, zq, tv, rh
%rb -> cn, qr
&jf -> mf
%jd -> mm
%cx -> xd, cn
%zs -> cz
%hn -> bm
%xr -> bd, xc
&mf -> rx
%zq -> kg, xc
&cn -> sh, jd, cx, tc, xd
%cs -> xj
%fb -> tc, cn
%mm -> cn, bk
%sq -> th, hz
%sz -> vx
%xl -> gh, sz
%vm -> gh, vv
%jp -> cn
%qr -> cn, jd
%bq -> xc, zv
&sh -> mf
%gz -> gs, hz
%qc -> qg, xc
%hg -> bq
%dt -> sq, hz
%xj -> fz
%qs -> gh
%fz -> hz, zs
%qg -> xc
%pp -> qs, gh
%zv -> xc, qc
%rh -> hz, mr
&gh -> tv, lk, sz, bh, vx
%th -> hz
&mz -> mf
%bm -> xr
%lk -> pg
%jx -> lk, gh
&hz -> xj, cs, zs, rh, mz
%tc -> dx
%mr -> hz, gz
%xd -> jk
%pg -> vm, gh
%kg -> hn, xc
%gs -> cs, hz
%vx -> cd
%cz -> hz, dt
%jk -> cn, fb
""";