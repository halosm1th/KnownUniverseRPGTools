using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using Dice;
using KnownUniversePoliticsGameWebApp.Data;
using KnownUniversePoliticsGameWebApp.Data.Politics_Game;
using KUP_Simple_Sector_Generator;

class KUPSubsectorGenerator
{
    public KUPSubsector Subsector { get; }
    public string Name { get; }
    private int Seed { get; set; }
    private Random RandomNumberGenerator;
    private bool UsingSeed = true;
    private readonly int XSize = 8;
    private readonly int YSize = 10;

    private int DispalyX = 0;
    private int DisplayY;
    private KnownUniversePoliticsGame PoliticsGame;
    private readonly bool IsPrinting = false;

    public KUPSubsectorGenerator(string name,
        KnownUniversePoliticsGame politicsGame, int dispX, int dispY, bool usingSeed = false, int seed = 01122000,
        bool isPrinting = false, int subX = 1, int subY = 1)
    {   
        Seed = seed + name.Aggregate(0, (h,t) => h * ((int) t));
        Name = GetName(Seed);
        //Seed *= Name.Aggregate(0, (h,t) => h * ((int) t)) + name.Aggregate(0, (h,t) => h * ((int) t));
        RandomNumberGenerator = new Random(Seed);
        UsingSeed = usingSeed;
        Subsector = new KUPSubsector(name, XSize, YSize, subX, subY);
        IsPrinting = isPrinting;
        PoliticsGame = politicsGame;
        DispalyX = dispX;
        DisplayY = dispY;
    }

    public KUPSubsector Generate()
    {
        PlaceSystems();
        DetermineSpacePortQuality();
        GeneratePointsOfInterest();

        return Subsector;
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
        int baseX = DispalyX;
        for (var y = 0; y < YSize; y++)
        {
            for (var x = 0; x < XSize; x++)
            {
                if (RollDice("1d10") > 5)
                {
                    var name = GenerateName();
                    Subsector.PlaceSystem(new KUPFilledSystem(name, x, y, Subsector, ref DispalyX, ref DisplayY), x, y);
                    if (IsPrinting)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                       // Console.WriteLine($"Generated a new system at x: {x+1}, y: {y+1}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                else
                {
                    Subsector.PlaceSystem(new KupEmptySystem(x, y, Subsector, ref DispalyX, ref DisplayY), x, y);
                    if (IsPrinting)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                       // Console.WriteLine($"Generated an empty system at x: {x+1}, y: {y+1}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }

                DispalyX++;
            }

            DispalyX = baseX;
            DisplayY++;
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
            _nameList.AddRange(nameListText.Select(x => x.Split(",")[1]));
            hasSplitNameList = true;
        } 
        return _nameList;
    }}
    
    private static List<string>? CountryNameList {get {
        if (!hasSplitNameList)
        {
            _nameList = nameListText.Select(x => x.Split(",")[1]).ToList();
            hasSplitNameList = true;
        } 
        return _nameList;
    }}
    
    private static List<string>? ProvinceNameList { get {
        if (!hasSplitNameList)
        {
            _nameList = nameListText.Select(x => x.Split(",")[2]).ToList();
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
        var name = GetName(RandomNumberGenerator.Next(0, nameList.Count));

        return name;
    }


    private void DetermineSpacePortQuality()
    {
        foreach(var realSystem in Subsector.GetFilledSystems())
        {
            if (realSystem != null)
            {
                var station = new KupPrimaryStation(RollDice(), realSystem, PoliticsGame.GetNewAssetID());
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
            var poiCount = PointOfInterestCount(system);
            
            for(int i =0; i < poiCount; i++){
                var poi = PointOfInterestTypeAndSubtype(system);
                
                FleshOutPrimaryStation(system);
                FleshOutPointsOfInterest(system, poi);
            }
        }
    }

    private int PointOfInterestCount(KUPFilledSystem system)
    {
            var roll = RollDice();
            var poi = system.PointsOfInterest
                .First(x => x.GetType() == typeof(KupPrimaryStation)) as KupPrimaryStation;
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

    private KUPPointsOfInterest PointOfInterestTypeAndSubtype(KUPFilledSystem system)
    {
        var typeRoll = RollDice("1d6");
        var subtypeRoll = RollDice("1d8");

        KUPPointsOfInterest poi = typeRoll switch
        {
            1 => new KupPointsOfInterestWorld(subtypeRoll, system, PoliticsGame.GetNewAssetID()),
            2 => new KupPointsOfInterestStation(subtypeRoll, system, PoliticsGame.GetNewAssetID()),
            3 => new KupPointsOfInterestWreck(subtypeRoll, system),
            4 => new KupPointsOfInterestAsteroid(subtypeRoll, system, PoliticsGame.GetNewAssetID()),
            5 => new KUPPointOfInterestAnomaly(subtypeRoll, system),
            6 => new KupPointsOfInterestOther(subtypeRoll, system, PoliticsGame.GetNewAssetID()),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        
        system.AddPoint(poi);
       // if(IsPrinting) Console.WriteLine($"System @ {system.SystemX},{system.SystemY} w/{((KURPGPrimaryStation) system.PointsOfInterest[0]).SubtypeName} now has a {poi.POIType} which is a {poi.SubtypeName}: {poi.SubTypeDescription}");

        return poi;
    }

    private void FleshOutPointsOfInterest(KUPFilledSystem system, KUPPointsOfInterest poi)
    {
        if (poi.POIType is KUPPoiTypes.Planet)
        {
            GeneratePlanetPOI(system, poi as KupPointsOfInterestWorld);
            PoliticsGame.NewAsset((poi as KupPointsOfInterestWorld).WorldAsset);
        }

        if (poi.POIType is KUPPoiTypes.Asteroid)
        {
            GenerateAsteorid(system, poi as KupPointsOfInterestAsteroid);
            PoliticsGame.NewAsset((poi as KupPointsOfInterestAsteroid).AsteroidAsset);
        }
        
        
        if (poi.POIType is KUPPoiTypes.Station)
        {
            GenerateStation(system, poi as KupPointsOfInterestStation);
            PoliticsGame.NewAsset((poi as KupPointsOfInterestStation).StationAsset);
        }
        
        
        if (poi.POIType is KUPPoiTypes.Wreck)
        {
            GenerateWreck(system, poi as KupPointsOfInterestWreck);
        }

        if (poi.POIType is KUPPoiTypes.Other)
        {
            PoliticsGame.NewAsset((poi as KupPointsOfInterestOther).OtherAsset);
        }
    }

    private void GenerateStation(KUPFilledSystem system, KupPointsOfInterestStation? poi)
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

    private void GenerateWreck(KUPFilledSystem system, KupPointsOfInterestWreck? poi)
    {
        poi.SizeRoll = GetMainStationModifiedResult(system);
        poi.AliensInsideRoll  = GetMainStationModifiedResult(system);
        poi.LootedRoll = GetMainStationModifiedResult(system);
        /* if (IsPrinting)
        {
            Console.WriteLine($"{poi.POIType} at {system.SystemX},{system.SystemY} is: {poi.SizeMeaning}, {poi.AliensInsideRoll}, {poi.LootedMeaning}");
        } */
    }

    private void GenerateAsteorid(KUPFilledSystem system, KupPointsOfInterestAsteroid? poi)
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

    private void GeneratePlanetPOI(KUPFilledSystem system, KupPointsOfInterestWorld? poi)
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

    private int GetMainStationModifiedResult(KUPFilledSystem system)
    {
        var roll = RollDice();
        var mainStation = system.PointsOfInterest
            .First(x => x.GetType() == typeof(KupPrimaryStation)) as KupPrimaryStation;
        var modifierResult = roll + SpacePortRatingModifierTable(mainStation?.SubtypeName);

        return modifierResult;
    }

    
    
    private void FleshOutPrimaryStation(KUPFilledSystem system)
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
            
            PoliticsGame.NewAsset((station as KupPrimaryStation).PrimaryStationAsset);
        }

        //if(IsPrinting) Console.WriteLine($"Station @ {system.SystemX},{system.SystemY}: " +
        //                                 $"{station.SubtypeName}, {station.TLMeaning}, {station.PopulationMeaning}," +
        //                                 $"{station.LawLevelMeaning}, {station.SizeMeaning}");
    }

    private int GetHighGovMod(KUPFilledSystem system)
    {
        var complexPOI = system.PointsOfInterest.Where(x =>
            x.HasComplexInfo
            && x.GetType() != typeof(KupPointsOfInterestWreck)
            && x.GetType() != typeof(KupPrimaryStation));

        var result = 0;

        foreach (var poi in complexPOI)
        {
            if (complexPOI is KupPointsOfInterestWorld)
            {
                if ((complexPOI as KupPointsOfInterestWorld).GovernemntRoll >= 9)
                {
                    result++;
                }
            }
            else if (complexPOI is KupPointsOfInterestStation)
            {
                if ((complexPOI as KupPointsOfInterestStation).TLRoll >= 9)
                {
                    result++;
                }

            }
            else if (complexPOI is KupPointsOfInterestAsteroid)
            {

                if ((complexPOI as KupPointsOfInterestAsteroid).TLRoll >= 9)
                {
                    result++;
                }
            }
        }

        return result;
    }
    private int GetLowGovMod(KUPFilledSystem system)
    {
        var complexPOI = system.PointsOfInterest.Where(x =>
            x.HasComplexInfo
            && x.GetType() != typeof(KupPointsOfInterestWreck)
            && x.GetType() != typeof(KupPrimaryStation));

        var result = 0;

        foreach (var poi in complexPOI)
        {
            if (complexPOI is KupPointsOfInterestWorld)
            {
                if ((complexPOI as KupPointsOfInterestWorld).GovernemntRoll <= 5)
                {
                    result++;
                }
            }
            else if (complexPOI is KupPointsOfInterestStation)
            {
                if ((complexPOI as KupPointsOfInterestStation).TLRoll <= 5)
                {
                    result++;
                }

            }
            else if (complexPOI is KupPointsOfInterestAsteroid)
            {

                if ((complexPOI as KupPointsOfInterestAsteroid).TLRoll <= 5)
                {
                    result++;
                }
            }
        }

        return result;
    }
    private int GetHighTechModifier(KUPFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KupPointsOfInterestWreck) 
            && x.GetType() != typeof(KupPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KupPointsOfInterestWorld world)
            {
                if (world.TLRoll >= 11)
                {
                    result++;
                }
            }else if (poi is KupPointsOfInterestStation)
            {
                if ((poi as KupPointsOfInterestStation).TLRoll >= 11)
                {
                    result++;
                }
                
            }else if (poi is KupPointsOfInterestAsteroid)
            {
                
                if ((poi as KupPointsOfInterestAsteroid).TLRoll >= 11)
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
    private int determineLawModifers(KUPFilledSystem system)
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
    

    private int GetLowPopMod(KUPFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KupPointsOfInterestWreck) 
            && x.GetType() != typeof(KupPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KupPointsOfInterestWorld)
            {
                if ((poi as KupPointsOfInterestWorld).PopulationRangeRoll <= 5)
                {
                    result++;
                }
            }else if (poi is KupPointsOfInterestStation)
            {
                if ((poi as KupPointsOfInterestStation).PopulationRangeRoll <= 5)
                {
                    result++;
                }
                
            }else if (poi is KupPointsOfInterestAsteroid)
            {
                
                if ((poi as KupPointsOfInterestAsteroid).PopulationRangeRoll <= 5)
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

    private int GetHighPopMod(KUPFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KupPointsOfInterestWreck) 
            && x.GetType() != typeof(KupPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KupPointsOfInterestWorld {PopulationRangeRoll: >= 10})
            {
                    result++;
            }else if (poi is KupPointsOfInterestStation {PopulationRangeRoll: >= 10})
            {
                    result++;
                
            }else if (poi is KupPointsOfInterestAsteroid {PopulationRangeRoll: >= 10})
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

    private int GetLowTechModifier(KUPFilledSystem system)
    {
        var complexPOI =  system.PointsOfInterest.Where(x =>
            x.HasComplexInfo 
            && x.GetType() != typeof(KupPointsOfInterestWreck) 
            && x.GetType() != typeof(KupPrimaryStation));

        var result = 0;
        
        foreach (var poi in complexPOI)
        {
            if (poi is KupPointsOfInterestWorld {PopulationRangeRoll: <= 5})
            {
                    result++;
            }
            else if (poi is KupPointsOfInterestStation {PopulationRangeRoll: <= 5})
            {
                    result++;

            }else if (poi is KupPointsOfInterestAsteroid {PopulationRangeRoll: <= 5})
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