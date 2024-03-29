﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using Dice;
using KUP_Simple_Sector_Generator;

class KURpgSubsectorGenerator
{
    public KURPGSubsector Subsector { get; }
    public string Name { get; }
    private int Seed { get; set; }
    private Random RandomNumberGenerator;
    private bool UsingSeed = true;
    private readonly int XSize = 8;
    private readonly int YSize = 10;

    private readonly bool IsPrinting = false;

    public KURpgSubsectorGenerator(string name, bool usingSeed = false, int seed = 01122000, bool isPrinting = false)
    {   
        Seed = seed + name.Aggregate(0, (h,t) => h * ((int) t));
        Name = GetName(Seed);
        //Seed *= Name.Aggregate(0, (h,t) => h * ((int) t)) + name.Aggregate(0, (h,t) => h * ((int) t));
        RandomNumberGenerator = new Random(Seed);
        UsingSeed = usingSeed;
        Subsector = new KURPGSubsector(name, XSize, YSize);
        IsPrinting = isPrinting;
    }

    public KURPGSubsector Generate()
    {
        PlaceSystems();
        DetermineSpacePortQuality();
        GeneratePointsOfInterest();

        return Subsector;
    }
    
    
    private void DetermineSpacePortQuality()
    {
        foreach(var realSystem in Subsector.GetFilledSystems())
        {
            if (realSystem != null)
            {
                var station = new KURPGPrimaryStation(RollDice(), realSystem);
                realSystem.AddPoint(station);
                //if (IsPrinting)
                //Console.WriteLine(
                //  $"System at {realSystem.SystemX},{realSystem.SystemY} has a class {station.SubtypeName}, which means it is: {station.SubTypeDescription}");
            }
        }
    }

    private void GeneratePointsOfInterest()
    {
        foreach (var system in Subsector.GetFilledSystems())
        {
            if (system != null)
            {
                var poiCount = PointOfInterestCount(system);
            
                for(int i =0; i < poiCount; i++){
                    var poi = PointOfInterestTypeAndSubtype(system);
                
                    FleshOutPrimaryStation(system);
                    FleshOutPointsOfInterest(system, poi);
                }
            }
        }
    }
    
    public Task<KURPGSubsector> GenerateAsync()
    {
        PlaceSystems();
        DetermineSpacePortQualityAsync();
        GeneratePointsOfInterestAsync();

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
                total += RandomNumberGenerator.Next(0, numbSides) + 1;
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
        if (SpacePortRating != null)
        {
            switch (SpacePortRating[0])
            {
                case 'A':
                    return 2;
                case 'B':
                    return 1;
                case 'C':
                    return 0;
                case 'D':
                    return -1;
                case 'E':
                    return -2;
                case 'F':
                    return -3;
            }
        }

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
                    Subsector.PlaceSystem(new KURPGFilledSystem(name, x, y, Subsector), x, y);
                    if (IsPrinting)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                       // Console.WriteLine($"Generated a new system at x: {x+1}, y: {y+1}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                else
                {
                    Subsector.PlaceSystem(new KURPGEmptySystem(x, y, Subsector), x, y);
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
        if (_nameList == null || _nameList.Count <= 0)
        {
            _nameListText = File.ReadAllLines(Directory.GetCurrentDirectory() + "/namelist.csv");
        }

        return _nameListText;
    }}
        


    private static bool hasSplitNameList = false;
    private static List<string>? _nameList = null;

    private static List<string>? nameList {get
    {
        if (!hasSplitNameList)
        {
            _nameList = new List<string>();
            _nameList.AddRange(ProvinceNameList);
            _nameList.AddRange(CountryNameList);
            _nameList.AddRange(CityNameList);
            hasSplitNameList = true;
        } 
        return _nameList;
    }}
    
    private static List<string>? CityNameList {get {
            var _nameList = nameListText.Select(x => x.Split(",")[0])
                .Aggregate(new List<string> (), (h, t) =>
                {
                    if (h.Contains(t))
                    {
                        return h;
                    }
                    h.Add(t);
                    return h;
                });
        return _nameList;
    }}

    
    private static List<string>? CountryNameList {get {

            var _nameList = nameListText.Select(x => x.Split(",")[1])
                .Aggregate(new List<string> (), (h, t) =>
                {
                    if (h.Contains(t))
                    {
                        return h;
                    }
                        h.Add(t);
                        return h;
                });
        return _nameList;
    }}
    
    private static List<string>? ProvinceNameList { get {
            var _nameList = nameListText.Select(x => x.Split(",")[2])
                .Aggregate(new List<string> (), (h, t) =>
                {
                    if (h.Contains(t))
                    {
                        return h;
                    }
                    h.Add(t);
                    return h;
                });
        return _nameList;
    }}
    
    
    public static string GetProvinceName(int numb)
    {
        Random r = new Random(numb);

        return ProvinceNameList[r.Next(0,ProvinceNameList.Count-1)];
    }
    
    
    public static string GetCountryName(int numb)
    {
        Random r = new Random(numb);

        return CountryNameList[r.Next(0,CountryNameList.Count-1)];
    }

    public static string GetName(int numb)
    {
        Random r = new Random(numb);

        var result = r.Next(0, nameList.Count - 1);
        return nameList[result];
    }
    
    private string GenerateName()
    {
        var name = GetName(RandomNumberGenerator.Next(0, nameList.Count));

        return name;
    }


    private void DetermineSpacePortQualityAsync()
    {
        Parallel.ForEach(Subsector.GetFilledSystems(), realSystem =>
        {
            if (realSystem != null)
            {
                var station = new KURPGPrimaryStation(RollDice(), realSystem);
                realSystem.AddPoint(station);
                //if (IsPrinting)
                //Console.WriteLine(
                //  $"System at {realSystem.SystemX},{realSystem.SystemY} has a class {station.SubtypeName}, which means it is: {station.SubTypeDescription}");
            }
        });
    }

    private void GeneratePointsOfInterestAsync()
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

    private int PointOfInterestCount(KURPGFilledSystem system)
    {
            var roll = RollDice();
            var poi = system.PointsOfInterest
                .First(x => x.GetType() == typeof(KURPGPrimaryStation)) as KURPGPrimaryStation;
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
        
            
            
            //if(IsPrinting) Console.WriteLine($"System @ {system.SystemX},{system.SystemY} w/{((KURPGPrimaryStation) system.PointsOfInterest[0]).SubtypeName} starport gets {result} POI's");

            return result;
    }

    private KURPGPointsOfInterest PointOfInterestTypeAndSubtype(KURPGFilledSystem system)
    {
        var typeRoll = RollDice("1d6");
        var subtypeRoll = RollDice("1d8");

        KURPGPointsOfInterest poi = typeRoll switch
        {
            1 => new KURPGPointsOfInterestWorld(subtypeRoll, system),
            2 => new KURPGPointsOfInterestStation(subtypeRoll, system),
            3 => new KURPGPointsOfInterestWreck(subtypeRoll, system),
            4 => new KURPGPointsOfInterestAsteroid(subtypeRoll, system),
            5 => new KURPGPointOfInterestAnomaly(subtypeRoll, system),
            6 => new KURPGPointsOfInterestOther(subtypeRoll, system),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        
        system.AddPoint(poi);
       // if(IsPrinting) Console.WriteLine($"System @ {system.SystemX},{system.SystemY} w/{((KURPGPrimaryStation) system.PointsOfInterest[0]).SubtypeName} now has a {poi.POIType} which is a {poi.SubtypeName}: {poi.SubTypeDescription}");

        return poi;
    }

    private void FleshOutPointsOfInterest(KURPGFilledSystem system, KURPGPointsOfInterest poi)
    {
        if (poi.POIType is KURPGPoiTypes.Planet)
        {
            GeneratePlanetPOI(system, poi as KURPGPointsOfInterestWorld);
        }

        if (poi.POIType is KURPGPoiTypes.Asteroid)
        {
            GenerateAsteorid(system, poi as KURPGPointsOfInterestAsteroid);
        }
        
        
        if (poi.POIType is KURPGPoiTypes.Station)
        {
            GenerateStation(system, poi as KURPGPointsOfInterestStation);
        }
        
        
        if (poi.POIType is KURPGPoiTypes.Wreck)
        {
            GenerateWreck(system, poi as KURPGPointsOfInterestWreck);
        }
        
    }

    private void GenerateStation(KURPGFilledSystem system, KURPGPointsOfInterestStation? poi)
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

    private void GenerateWreck(KURPGFilledSystem system, KURPGPointsOfInterestWreck? poi)
    {
        poi.SizeRoll = GetMainStationModifiedResult(system);
        poi.AliensInsideRoll  = GetMainStationModifiedResult(system);
        poi.LootedRoll = GetMainStationModifiedResult(system);
        /* if (IsPrinting)
        {
            Console.WriteLine($"{poi.POIType} at {system.SystemX},{system.SystemY} is: {poi.SizeMeaning}, {poi.AliensInsideRoll}, {poi.LootedMeaning}");
        } */
    }

    private void GenerateAsteorid(KURPGFilledSystem system, KURPGPointsOfInterestAsteroid? poi)
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

    private void GeneratePlanetPOI(KURPGFilledSystem system, KURPGPointsOfInterestWorld? poi)
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

    private int GetMainStationModifiedResult(KURPGFilledSystem system)
    {
        var roll = RollDice();
        var mainStation = system.PointsOfInterest
            .First(x => x.GetType() == typeof(KURPGPrimaryStation)) as KURPGPrimaryStation;
        var modifierResult = roll + SpacePortRatingModifierTable(mainStation?.SubtypeName);

        return modifierResult;
    }

    
    
    private void FleshOutPrimaryStation(KURPGFilledSystem system)
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

    private int GetHighGovMod(KURPGFilledSystem system)
    {
        var complexPOI = system.PointsOfInterest.Where(x =>
            x.HasComplexInfo
            && x.GetType() != typeof(KURPGPointsOfInterestWreck)
            && x.GetType() != typeof(KURPGPrimaryStation));

        var result = 0;

        foreach (var poi in complexPOI)
        {
            if (complexPOI is KURPGPointsOfInterestWorld)
            {
                if ((complexPOI as KURPGPointsOfInterestWorld).GovernemntRoll >= 9)
                {
                    result++;
                }
            }
            else if (complexPOI is KURPGPointsOfInterestStation)
            {
                if ((complexPOI as KURPGPointsOfInterestStation).TLRoll >= 9)
                {
                    result++;
                }

            }
            else if (complexPOI is KURPGPointsOfInterestAsteroid)
            {

                if ((complexPOI as KURPGPointsOfInterestAsteroid).TLRoll >= 9)
                {
                    result++;
                }
            }
        }

        return result;
    }
    private int GetLowGovMod(KURPGFilledSystem system)
    {
        var complexPoi = system.PointsOfInterest.Where(x =>
            x.HasComplexInfo
            && x.GetType() != typeof(KURPGPointsOfInterestWreck)
            && x.GetType() != typeof(KURPGPrimaryStation));

        var result = 0;

        foreach (var poi in complexPoi)
        {
            switch (poi)
            {
                case KURPGPointsOfInterestWorld world:
                {
                    if (world.GovernemntRoll <= 5)
                    {
                        result++;
                    }

                    break;
                }
                case KURPGPointsOfInterestStation station:
                {
                    if (station.TLRoll <= 5)
                    {
                        result++;
                    }

                    break;
                }
                case KURPGPointsOfInterestAsteroid asteroid:
                {
                    if (asteroid.TLRoll <= 5)
                    {
                        result++;
                    }

                    break;
                }
            }
        }

        return result;
    }
    private int GetHighTechModifier(KURPGFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KURPGPointsOfInterestWreck) 
            && x.GetType() != typeof(KURPGPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KURPGPointsOfInterestWorld world)
            {
                if (world.TLRoll >= 11)
                {
                    result++;
                }
            }else if (poi is KURPGPointsOfInterestStation)
            {
                if ((poi as KURPGPointsOfInterestStation).TLRoll >= 11)
                {
                    result++;
                }
                
            }else if (poi is KURPGPointsOfInterestAsteroid)
            {
                
                if ((poi as KURPGPointsOfInterestAsteroid).TLRoll >= 11)
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
    private int determineLawModifers(KURPGFilledSystem system)
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
    

    private int GetLowPopMod(KURPGFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KURPGPointsOfInterestWreck) 
            && x.GetType() != typeof(KURPGPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KURPGPointsOfInterestWorld)
            {
                if ((poi as KURPGPointsOfInterestWorld).PopulationRangeRoll <= 5)
                {
                    result++;
                }
            }else if (poi is KURPGPointsOfInterestStation)
            {
                if ((poi as KURPGPointsOfInterestStation).PopulationRangeRoll <= 5)
                {
                    result++;
                }
                
            }else if (poi is KURPGPointsOfInterestAsteroid)
            {
                
                if ((poi as KURPGPointsOfInterestAsteroid).PopulationRangeRoll <= 5)
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

    private int GetHighPopMod(KURPGFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KURPGPointsOfInterestWreck) 
            && x.GetType() != typeof(KURPGPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KURPGPointsOfInterestWorld {PopulationRangeRoll: >= 10})
            {
                    result++;
            }else if (poi is KURPGPointsOfInterestStation {PopulationRangeRoll: >= 10})
            {
                    result++;
                
            }else if (poi is KURPGPointsOfInterestAsteroid {PopulationRangeRoll: >= 10})
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

    private int GetLowTechModifier(KURPGFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KURPGPointsOfInterestWreck) 
            && x.GetType() != typeof(KURPGPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KURPGPointsOfInterestWorld {PopulationRangeRoll: <= 5})
            {
                    result++;
            }
            else if (poi is KURPGPointsOfInterestStation {PopulationRangeRoll: <= 5})
            {
                    result++;

            }else if (poi is KURPGPointsOfInterestAsteroid {PopulationRangeRoll: <= 5})
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