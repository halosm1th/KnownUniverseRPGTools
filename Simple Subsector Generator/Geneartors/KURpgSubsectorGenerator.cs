using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using Dice;
using Simple_Subsector_Generator;

class KURpgSubsectorGenerator
{
    public KURpgSubsector Subsector { get; }
    public string Name { get; }
    private int Seed { get; set; }
    private Random r;
    private bool UsingSeed = true;
    private readonly int XSize = 8;
    private readonly int YSize = 10;

    private readonly bool IsPrinting = false;

    public KURpgSubsectorGenerator(string name, bool usingSeed = false, int seed = 01122000, bool isPrinting = false)
    {
        Name = name;
        Seed = seed + name.Aggregate(0, (h,t) => h + ((int) t));
        r = new Random(seed);
        UsingSeed = usingSeed;
        Subsector = new KURpgSubsector(name, XSize, YSize);
        IsPrinting = isPrinting;
    }

    public Task<KURpgSubsector> Generate()
    {
        PlaceSystems();
        DetermineSpacePortQuality();
        GeneratePointsOfInterest();

        return Task.FromResult(Subsector);
    }

    private int RollDice(string diceToRoll = "2d6")
    {
        if (UsingSeed)
        {
            var total = 0;
            var numbDice = Convert.ToInt32(diceToRoll.ToLower().Split('d')[0]);
            var numbSides = Convert.ToInt32(diceToRoll.ToLower().Split('d')[1]);
            for (var dice = 0; dice < numbDice; dice++)
            {
                total += r.Next(0, numbSides) + 1;
            }

            return total;
        }
        else
        {

            return Convert.ToInt32(Roller.Roll(diceToRoll).Value);
        }
    }

    private int SpacePortRatingModifierTable(string? SpacePortRating)
    {
        if (SpacePortRating[0] == 'A') return 2;
        if (SpacePortRating[0] == 'B') return 1;
        if (SpacePortRating[0] == 'C') return 0;
        if (SpacePortRating[0] == 'D') return -1;
        if (SpacePortRating[0] == 'E') return -2;
        if (SpacePortRating[0] == 'F') return -3;

        return 0;
    }

    private void PlaceSystems()
    {
        for (var y = 0; y < YSize; y++)
        {
            for (var x = 0; x < XSize; x++)
            {
                if (RollDice("1d10") > 5)
                {
                    var name = GenerateName();
                    Subsector.PlaceSystem(new KURpgFilledSystem(name, x, y), x, y);
                    if (IsPrinting)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                       // Console.WriteLine($"Generated a new system at x: {x+1}, y: {y+1}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                else
                {
                    Subsector.PlaceSystem(new KURpgEmptySystem(x, y), x, y);
                    if (IsPrinting)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                       // Console.WriteLine($"Generated an empty system at x: {x+1}, y: {y+1}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
            }
        }
    }

    private static string[]? _nameListText { get; set; } = null;

    private static string[]? nameListText {get {
        if (_nameList == null)
        {
            _nameListText = File.ReadAllLines(Directory.GetCurrentDirectory() + "/namelist.csv");
        }

        return _nameListText;
    }}
        


    private static bool hasSplitNameList = false;
    private static List<string>? _nameList = null;

    private static List<string>? nameList {get {
        if (!hasSplitNameList)
        {
            _nameList = nameListText.Select(x => x.Split(",")[0]).ToList();
            hasSplitNameList = true;
        } 
        return _nameList;
    }}
    
    private static List<string>? CountryNameList {get {
        if (!hasSplitNameList)
        {
            _nameList = nameListText.Select(x => x.Split(",")[2]).ToList();
            hasSplitNameList = true;
        } 
        return _nameList;
    }}
    
    private static List<string>? ProvinceNameList { get {
        if (!hasSplitNameList)
        {
            _nameList = nameListText.Select(x => x.Split(",")[1]).ToList();
            hasSplitNameList = true;
        } 
        return _nameList;
    }}
    
    
    public static string GetProvinceName(int numb)
    {
        Random r = new Random(numb);

        return ProvinceNameList[r.Next(0,nameList.Count)];
    }
    
    
    public static string GetCountryName(int numb)
    {
        Random r = new Random(numb);

        return CountryNameList[r.Next(0,nameList.Count)];
    }

    public static string GetName(int numb)
    {
        Random r = new Random(numb);

        return nameList[r.Next(0,nameList.Count)];
    }
    
    private string GenerateName()
    {
        var name = GetName(r.Next(0, nameList.Count));

        return name;
    }


    private void DetermineSpacePortQuality()
    {
        Parallel.ForEach(Subsector.GetFilledSystems(), realSystem =>
        {
            if (realSystem != null)
            {
                var station = new KURpgPrimaryStation(RollDice());
                realSystem.AddPoint(station);
                //if (IsPrinting)
                //Console.WriteLine(
                //  $"System at {realSystem.SystemX},{realSystem.SystemY} has a class {station.SubtypeName}, which means it is: {station.SubTypeDescription}");
            }
        });
    }

    private void GeneratePointsOfInterest()
    {
        Parallel.ForEach (Subsector.GetFilledSystems(), system =>
        {
            var poiCount = PointOfInterestCount(system);
            
            for(int i =0; i < poiCount; i++){
                var poi = PointOfInterestTypeAndSubtype(system);
                
                FleshOutPointsOfInterest(system, poi);
                FleshOutPrimaryStation(system);
            }
        });
    }

    private int PointOfInterestCount(KURpgFilledSystem system)
    {
            var roll = RollDice();
            var poi = system.PointsOfInterest
                .First(x => x.GetType() == typeof(KURpgPrimaryStation)) as KURpgPrimaryStation;
            var modifierResult = roll + SpacePortRatingModifierTable(poi?.SubtypeName);

            var result = modifierResult switch
            {

                <=1 => 1,
                2 => 2,
                3 => 2,
                4 => 2,
                5 => 3,
                6 => 3,
                7 => 3,
                8 => 3,
                9 => 4,
                10 => 4,
                11 => 4,
                >=12 => 5,
            };
        
            
            
            //if(IsPrinting) Console.WriteLine($"System @ {system.SystemX},{system.SystemY} w/{((KURpgPrimaryStation) system.PointsOfInterest[0]).SubtypeName} starport gets {result} POI's");

            return result;
    }

    private KURpgPointsOfInterest PointOfInterestTypeAndSubtype(KURpgFilledSystem system)
    {
        var typeRoll = RollDice("1d6");
        var subtypeRoll = RollDice("1d8");

        KURpgPointsOfInterest poi = typeRoll switch
        {
            1 => new KURpgPointsOfInterestWorld(subtypeRoll),
            2 => new KURpgPointsOfInterestStation(subtypeRoll),
            3 => new KURPG_PointsOfInterest_Wreck(subtypeRoll),
            4 => new KURpgPointsOfInterestAsteroid(subtypeRoll),
            5 => new KURPGPointOfInterestAnomaly(subtypeRoll),
            6 => new KURPG_PointsOfInterest_Other(subtypeRoll),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        
        system.AddPoint(poi);
       // if(IsPrinting) Console.WriteLine($"System @ {system.SystemX},{system.SystemY} w/{((KURpgPrimaryStation) system.PointsOfInterest[0]).SubtypeName} now has a {poi.POIType} which is a {poi.SubtypeName}: {poi.SubTypeDescription}");

        return poi;
    }

    private void FleshOutPointsOfInterest(KURpgFilledSystem system, KURpgPointsOfInterest poi)
    {
        if (poi.POIType is KURpgPoiTypes.Planet)
        {
            GeneratePlanetPOI(system, poi as KURpgPointsOfInterestWorld);
        }

        if (poi.POIType is KURpgPoiTypes.Asteroid)
        {
            GenerateAsteorid(system, poi as KURpgPointsOfInterestAsteroid);
        }
        
        
        if (poi.POIType is KURpgPoiTypes.Station)
        {
            GenerateStation(system, poi as KURpgPointsOfInterestStation);
        }
        
        
        if (poi.POIType is KURpgPoiTypes.Wreck)
        {
            GenerateWreck(system, poi as KURPG_PointsOfInterest_Wreck);
        }
        
    }

    private void GenerateStation(KURpgFilledSystem system, KURpgPointsOfInterestStation? poi)
    {
        poi.TLRoll = GetMainStationModifiedResult(system);
        poi.PopulationRangeRoll = RollDice() + poi.GetTLModifier;
        poi.LawLevelRoll = RollDice() + poi.GetPopModifier;
        poi.GovernemntRoll = RollDice() + poi.GetLawLevelModifier;
        poi.SizeRoll = RollDice();

       // if(IsPrinting) Console.WriteLine($"{poi.POIType} in {system.SystemX},{system.SystemY} stats:" +
       //                                  $"TL: {poi.TLMeaning}, Pop:{poi.PopulationMeaning}, Law Level:{poi.LawLevelMeaning}" +
       //                                  $"Government: {poi.GovernmentMeaning}" +
       //                                  $"Size: {poi.SizeMeaning}");
    }

    private void GenerateWreck(KURpgFilledSystem system, KURPG_PointsOfInterest_Wreck? poi)
    {
        poi.SizeRoll = GetMainStationModifiedResult(system);
        poi.AliensInsideRoll  = GetMainStationModifiedResult(system);
        poi.LootedRoll = GetMainStationModifiedResult(system);
        /* if (IsPrinting)
        {
            Console.WriteLine($"{poi.POIType} at {system.SystemX},{system.SystemY} is: {poi.SizeMeaning}, {poi.AliensInsideRoll}, {poi.LootedMeaning}");
        } */
    }

    private void GenerateAsteorid(KURpgFilledSystem system, KURpgPointsOfInterestAsteroid? poi)
    {
        poi.TLRoll = GetMainStationModifiedResult(system);
        poi.PopulationRangeRoll = RollDice() + poi.GetTLModifier;
        poi.LawLevelRoll = RollDice() + poi.GetPopModifier;
        poi.GovernemntRoll = RollDice() + poi.GetLawLevelModifier;
        poi.SizeRoll = RollDice();
       // if(IsPrinting) Console.WriteLine($"{poi.POIType} in {system.SystemX},{system.SystemY} stats:" +
       //                                  $"TL: {poi.TLMeaning}, Pop:{poi.PopulationMeaning}, Law Level:{poi.LawLevelMeaning}" +
       //                                  $"Government: {poi.GovernmentMeaning}" +
       //                                  $"Size: {poi.SizeMeaning}");
    }

    private void GeneratePlanetPOI(KURpgFilledSystem system, KURpgPointsOfInterestWorld? poi)
    {
        poi.TLRoll = GetMainStationModifiedResult(system);
        poi.PopulationRangeRoll = RollDice() + poi.GetTLModifier;
        poi.LawLevelRoll = RollDice() + poi.GetPopModifier;
        poi.GovernemntRoll = RollDice() + poi.GetLawLevelModifier;
        poi.SizeRoll = RollDice();
        poi.AtmosphereRoll = RollDice();
        poi.GravityRoll = RollDice();
        
        //if(IsPrinting) Console.WriteLine($"{poi.POIType} in {system.SystemX},{system.SystemY} stats:" +
        //                                 $"TL: {poi.TLMeaning}, Pop:{poi.PopulationMeaning}, Law Level:{poi.LawLevelMeaning}" +
        //                                 $"Government: {poi.GovernmentMeaning}" +
        //                                 $"Size: {poi.SizeMeaning}," +
        //                                 $"Atmo: {poi.AtmosphereMeaning}," +
        //                                 $"Grav: {poi.GravityMeaning}");
    }

    private int GetMainStationModifiedResult(KURpgFilledSystem system)
    {
        var roll = RollDice();
        var mainStation = system.PointsOfInterest
            .First(x => x.GetType() == typeof(KURpgPrimaryStation)) as KURpgPrimaryStation;
        var modifierResult = roll + SpacePortRatingModifierTable(mainStation?.SubtypeName);

        return modifierResult;
    }

    
    
    private void FleshOutPrimaryStation(KURpgFilledSystem system)
    {
        if (system.SystemsPrimaryStation != null)
        {
            system.SystemsPrimaryStation.TLRoll = GetMainStationModifiedResult(system)
                                                  + GetHighTechModifier(system) + GetLowTechModifier(system);
            system.SystemsPrimaryStation.PopulationRangeRoll = GetMainStationModifiedResult(system)
                                                               + GetHighPopMod(system) + GetLowPopMod(system);
            system.SystemsPrimaryStation.GovernemntRoll =
                GetMainStationModifiedResult(system) + GetHighGovMod(system) + GetLowGovMod(system);
            system.SystemsPrimaryStation.LawLevelRoll =
                GetMainStationModifiedResult(system) + determineLawModifers(system);
            system.SystemsPrimaryStation.SizeRoll = GetMainStationModifiedResult(system);

            var station = system.SystemsPrimaryStation;
        }

        //if(IsPrinting) Console.WriteLine($"Station @ {system.SystemX},{system.SystemY}: " +
        //                                 $"{station.SubtypeName}, {station.TLMeaning}, {station.PopulationMeaning}," +
        //                                 $"{station.LawLevelMeaning}, {station.SizeMeaning}");
    }

    private int GetHighGovMod(KURpgFilledSystem system)
    {
        var complexPOI = system.PointsOfInterest.Where(x =>
            x.HasComplexInfo
            && x.GetType() != typeof(KURPG_PointsOfInterest_Wreck)
            && x.GetType() != typeof(KURpgPrimaryStation));

        var result = 0;

        foreach (var poi in complexPOI)
        {
            if (complexPOI is KURpgPointsOfInterestWorld)
            {
                if ((complexPOI as KURpgPointsOfInterestWorld).GovernemntRoll >= 9)
                {
                    result++;
                }
            }
            else if (complexPOI is KURpgPointsOfInterestStation)
            {
                if ((complexPOI as KURpgPointsOfInterestStation).TLRoll >= 9)
                {
                    result++;
                }

            }
            else if (complexPOI is KURpgPointsOfInterestAsteroid)
            {

                if ((complexPOI as KURpgPointsOfInterestAsteroid).TLRoll >= 9)
                {
                    result++;
                }
            }
        }

        return result;
    }
    private int GetLowGovMod(KURpgFilledSystem system)
    {
        var complexPOI = system.PointsOfInterest.Where(x =>
            x.HasComplexInfo
            && x.GetType() != typeof(KURPG_PointsOfInterest_Wreck)
            && x.GetType() != typeof(KURpgPrimaryStation));

        var result = 0;

        foreach (var poi in complexPOI)
        {
            if (complexPOI is KURpgPointsOfInterestWorld)
            {
                if ((complexPOI as KURpgPointsOfInterestWorld).GovernemntRoll <= 5)
                {
                    result++;
                }
            }
            else if (complexPOI is KURpgPointsOfInterestStation)
            {
                if ((complexPOI as KURpgPointsOfInterestStation).TLRoll <= 5)
                {
                    result++;
                }

            }
            else if (complexPOI is KURpgPointsOfInterestAsteroid)
            {

                if ((complexPOI as KURpgPointsOfInterestAsteroid).TLRoll <= 5)
                {
                    result++;
                }
            }
        }

        return result;
    }
    private int GetHighTechModifier(KURpgFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KURPG_PointsOfInterest_Wreck) 
            && x.GetType() != typeof(KURpgPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KURpgPointsOfInterestWorld world)
            {
                if (world.TLRoll >= 11)
                {
                    result++;
                }
            }else if (poi is KURpgPointsOfInterestStation)
            {
                if ((poi as KURpgPointsOfInterestStation).TLRoll >= 11)
                {
                    result++;
                }
                
            }else if (poi is KURpgPointsOfInterestAsteroid)
            {
                
                if ((poi as KURpgPointsOfInterestAsteroid).TLRoll >= 11)
                {
                    result++;
                }
            }
        }

        return result switch
        {
            <=0 => 0,
            1 => 1,
            2 => 1,
            3 => 2,
            4 => 2,
            >=5 => 3
        };
    }
    private int determineLawModifers(KURpgFilledSystem system)
    {
        var IlleaglPOICount = system.PointsOfInterest
            .Count(x => x.SubtypeName.Contains("Illegal")
                        || x.SubtypeName.Contains("Terrorist Hideout")
                        || x.SubtypeName.Contains("Black Market"));
        
        var MilitaryPOICount = system.PointsOfInterest
            .Count(x => x.SubtypeName.Contains("Military")
                        || x.SubtypeName.Contains("System Defense")
                        || x.SubtypeName.Contains("Black Ops"));
        var modifier = 0;

        modifier += IlleaglPOICount switch
        {
            <=0 => +1,
            1 => -1,
            2 => -1,
            3 => -2,
            4 => -2,
            >=5 => -3
        };

        modifier += MilitaryPOICount switch
        {
            <=0 => -1,
            1 => +1,
            2 => 1,
            3 => 2,
            4 => 2,
            >=5 => 3
        };

        return modifier;
    }
    

    private int GetLowPopMod(KURpgFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KURPG_PointsOfInterest_Wreck) 
            && x.GetType() != typeof(KURpgPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KURpgPointsOfInterestWorld)
            {
                if ((poi as KURpgPointsOfInterestWorld).PopulationRangeRoll <= 5)
                {
                    result++;
                }
            }else if (poi is KURpgPointsOfInterestStation)
            {
                if ((poi as KURpgPointsOfInterestStation).PopulationRangeRoll <= 5)
                {
                    result++;
                }
                
            }else if (poi is KURpgPointsOfInterestAsteroid)
            {
                
                if ((poi as KURpgPointsOfInterestAsteroid).PopulationRangeRoll <= 5)
                {
                    result++;
                }
            }
        }

        return result switch
        {
            <=0 => 0,
            1 => 1,
            2 => 1,
            3 => 2,
            4 => 2,
            >=5 => 3
        };
    }

    private int GetHighPopMod(KURpgFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KURPG_PointsOfInterest_Wreck) 
            && x.GetType() != typeof(KURpgPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KURpgPointsOfInterestWorld {PopulationRangeRoll: >= 10})
            {
                    result++;
            }else if (poi is KURpgPointsOfInterestStation {PopulationRangeRoll: >= 10})
            {
                    result++;
                
            }else if (poi is KURpgPointsOfInterestAsteroid {PopulationRangeRoll: >= 10})
            {
                
                    result++;
            }
        }

        return result switch
        {
            <=0 => 0,
            1 => 1,
            2 => 1,
            3 => 2,
            4 => 2,
            >=5 => 3
        };
    }

    private int GetLowTechModifier(KURpgFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KURPG_PointsOfInterest_Wreck) 
            && x.GetType() != typeof(KURpgPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KURpgPointsOfInterestWorld {PopulationRangeRoll: <= 5})
            {
                    result++;
            }
            else if (poi is KURpgPointsOfInterestStation {PopulationRangeRoll: <= 5})
            {
                    result++;

            }else if (poi is KURpgPointsOfInterestAsteroid {PopulationRangeRoll: <= 5})
            {
                result++;
            }
        }

        return result switch
        {
            <=0 => 0,
            1 => 1,
            2 => 1,
            3 => 2,
            4 => 2,
            >=5 => 3
        };
    }


    private static readonly Encoding Utf8Encoder = Encoding.GetEncoding(
        "UTF-8",
        new EncoderReplacementFallback(string.Empty),
        new DecoderExceptionFallback()
    );
    
    public void WriteToFile(string name, string directoryToWriteTo)
    {

        var utf8Text = Utf8Encoder.GetString(Utf8Encoder.GetBytes(Subsector.ToString()));
        
        var st = utf8Text;
        
        var fs = File.OpenWrite(directoryToWriteTo + $"/{name}.md");
        var sw = new StreamWriter(fs);
        foreach (var s in st.Split('\n'))
        {
            sw.WriteLine(s);
            //if(IsPrinting) Console.WriteLine($"Writing: ("+ s + ") to file.");
            sw.Flush();
        }
        sw.Close();
        fs.Close();

    }
}