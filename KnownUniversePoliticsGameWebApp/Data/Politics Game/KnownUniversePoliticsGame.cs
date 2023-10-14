using KnownUniversePoliticsGameWebApp.Data.Subsector_Generator.Regions_of_Space;
using Microsoft.AspNetCore.Components.Web;
using KUP_Simple_Sector_Generator;
using SixLabors.ImageSharp;
using TravellerMapSystem.Tools;

namespace KnownUniversePoliticsGameWebApp.Data.Politics_Game;

public class KnownUniversePoliticsGame : IKUPEventActor
{
    #region Variables

    public string Name => "The Game";
    public static KUPEventService EventService;
    public static KUPFaction GameMaster;

    public int SenderID => 1919991701;
    public int ReceiverID => 1919991701;
    private int _shipIDs = 600000;

    public void AddToEventService()
    {
        KUPEventService.AddActor(this);
    }

    public KUPSector? Sector { get; }

    public bool GameRunning { get; protected set; }
    public List<KUPFaction> Factions { get; }
    public List<KUPPlayer?> Players { get; }
    private int seed = 20001201;
    private static int CurrentAssetId = 0;
    private static int CurrentPOIAssetId = 0;

    private int CurrentRound = 1;
    public List<IKUPAsset> AssetsInPlay { get; } = new List<IKUPAsset>();
    public List<KUPCombatAsset> AssetsToFight { get; set; }

    private readonly KUPDrawSector KupDrawSector;

    public Image SectorImage
    {
        get
        {
            return KupDrawSector.GenerateImage(false, Directory.GetCurrentDirectory() + "/Subsectors/KUPMap.png",
                false);
        }
    }

    #endregion

    #region Constructor

    public KnownUniversePoliticsGame()
    {
        AddToEventService();
        //Setup assets
        AssetsInPlay = new List<IKUPAsset>();

        //Setup Playesr
        var thomas = new KUPPlayer("Thomas", "password", 100, 10001);
        var pirate = new KUPPlayer("Owen", "BaringSea", 10, 10002);
        var bank = new KUPPlayer("Barbara", "ILUBoobsHigh", 10, 10003);
        var food = new KUPPlayer("Ninno", "123456789", 10, 10004);
        
        
        var grayson = new KUPPlayer("Grayson", "ThirtyYearsWar", 10, 10005);
        var max = new KUPPlayer("Max", "GodIsDead", 10, 10006);
        var solange = new KUPPlayer("Solange", "CeramicsRCool", 10, 10007);
        var alex = new KUPPlayer("Alex", "TyranidsRU", 10, 10008);
        var wes = new KUPPlayer("Wes", "BeerRecBro", 10, 10009);
        var jake = new KUPPlayer("Jake", "GunsWomenDrugs", 10, 10010);
        var finn = new KUPPlayer("Finn", "UCRSpartans", 10, 10011);
        var maya = new KUPPlayer("Maya", "JabezIsCool", 10, 10012);
        var logan = new KUPPlayer("Logan", "Cousins4Life", 10, 10013);
        var malik = new KUPPlayer("Gay", "CumInMyAssBby", 10, 10014);


        Players = new List<KUPPlayer?>()
        {
            thomas,pirate,bank,food,
            grayson, max,solange,alex,wes,jake,finn,maya,logan
        };

        //Setup the subsector
        var generator = new KUPSectorGenerator("KUP Sector", true, seed, this, false);
        generator.Generate();
        Sector = generator.Sector;
        //Set the politics game for location so it can do name lookups.
        KUPLocation.politicsGame = this;

        //the special faction used for handling a lot of game stuff is the game master faction
        GameMaster = new("Game Master", 0, FactionType.GM, 1000000, 10000000,
            GetAssetsFromIDS(new()
            {
                80
            }), thomas);
        
        
        //Generate the factions for the players.
        Factions = new List<KUPFaction>()
        {
            GameMaster,
            new("Bank", 1, FactionType.Bank, 1000000, 10000000,
                GetAssetsFromIDS(new()
                {
                    85
                }), bank),
            new("Food", 2, FactionType.Food, 1000000, 10000000,
                GetAssetsFromIDS(new()
                {
                    87
                }), food),
            new("Pirates", 3, FactionType.Pirates, 0, 0,
                GetAssetsFromIDS(new()
                {
                    67,66,75,71,69,73,74,86,34, 42, 43, 44, 49, 60,
                    56, 439, 448, 260, 175, 31, 26, 35, 15, 181, 184,
                    322, 562, 377, 539,544, 543, 343,348, 347, 332, 523,530,203,
                    215, 424, 587, 624
                }), pirate),
            
            
            
            new KUPFaction("Edyonda-Federation Industrial Alliance",6,FactionType.UFE3, 0,0,
                GetAssetsFromIDS(new ()
                {
                    567,568, 564, 563, 558, 557, 556, 561, 560, 571, 566, 555, 554,
                    552, 551, 550, 549, 548,
                    
                }), wes,
                new ()
                {
                    "You are small but rich, and prefer not to be the one in the light causing war; you just want to cause wars to happen so you can profit off of them!",
                    "You are (in theory) part of the Federation, which is led by Grayson.",
                    "You are the Federation business sector, you want to make money.",
                },
                new ()
                {
                    "You want to ensure the Federation has outposts to spread its fleets all over the place. You, or another Federation aligned player whom you are in a signed and active alliance with, must control the following systems: England (5, 22), Managua (1, 28), Odisha (3, 14), Jill Sheng (11, 9), and Flanders (6, 6), Central Juntland (12, 6),  Ile-De-France (32, 9), Limburg (5, 13), Jalisco (1, 23), and Maharashtra (5, 16), Okayama (3, 23), ",
                    "You seek to cause wars, playing both sides to profit. You win if the vast majority of players are at war at the end of the game."
                }),
            new KUPFaction("Federation Core",4,FactionType.UFE1, 0,0,
            GetAssetsFromIDS(new ()
            {
                628,627, 626,625, 623, 465, 456, 464,455, 452, 449,450, 447, 451, 454, 460,
                463, 462, 459, 458, 457, 461, 600, 601, 603, 604, 607, 608, 614, 615, 616,
                619, 453,441, 436, 435, 440, 442, 443,444, 445,446, 
            }), grayson,
            new ()
            {
                " You're like the Federation from star trek, you try to do good, even if it can't always happen.",
                "You seek to avoid using violence to reach your objects. Only wars when you absolutely have too are acceptable.",
                "The old empire has collapsed, you're the promise of somehting new, something better, something more free."
            },
            new (){
                "You want everyone to be stable and happy at the end of the game. No Wars, everyone should have positive money and influence.",
                "Establish the Space UN as a legislative body democratically passing a consitution as the basis of space law",
                "Don't loose the homeland of freedom: Haryana (38, 21)"
            }),
            
            new KUPFaction("Parliamentary Fleet of Edyonda",5,FactionType.UFE2, 0,0,
                GetAssetsFromIDS(new ()
                {
                    94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107,
                    108, 109, 110, 111, 112, 125, 121, 120, 119, 118, 117,116,
                    115, 114,113, 238, 93, 92, 89, 90, 91, 88, 275, 274, 268, 271,
                    266, 261, 265, 233
                }), alex,
                new ()
                {
                    "You are the Federation fleet; you believe in honour and duty to your state, so long as you believe that state is acting in a manner you believe just.",
                    "You are (in theory) part of the Federation, which is led by Grayson.",
                    "You are the Federation fleet, your purpose is to defend the Federation from all threats, real or perceived."
                },
                new ()
                {
                    "You want to have the largest fleet at the end of the game",
                    "Defend the Federation from all threats, real or perceived.",
                    "Keep control of: Ile-De-France (32, 9), Baden0Wurttembe (29, 2), Indiana (23, 10), and Utah (23, 4)"
                }),
            
            new KUPFaction("Monarch of Edonius",7,FactionType.Vers1, 0,0,
                GetAssetsFromIDS(new ()
                {
                    0,1,2,3,4,5,6,7,8,9,10,12,13,14,16,17,18,19,20, 11,21,
                    23, 25, 30, 163, 168, 158, 159, 154, 155, 156, 160, 161,
                    157, 153, 152, 151, 189, 51
                    
                    /*
                    0, 1, 5, 7, 8, 9, 10, 12, 13, 16,
                    51, 54, 55, 56, 57, 58, 75, 2, 41, 43, 3, 45,
                    4, 48, 50, */
                    
                }), max,
                new ()
                {
                    "You are the Monarch! You need a court and nobles. You must develop a group of fiefs who for whatever reason pay you (in kind is valid)  each round, but to whom you are obligated to defend.",
                    "You must gain control of the the Empires former-capital systems systems: England (5, 22), Okayama (3, 23), and Managua (1, 28)",
                    "You must never lose the following systems, for they are the heartlands of your kingdom: Odisha (3, 14), Jill Sheng (11, 9), and Flanders (6, 6)"
                },
                new ()
                {
                    "You are the Monarch, you rule because you are loved, and becuase you have a big stick. You aren't afraid to use it.",
                    "Lavish living is the only way to show your superiority over other peasants.",
                    "You are the source of honour and just! An insult to you is an insult to your people, and vice versa. Every defeat is an assault against your family name, every victory raises the family to new heights, and as the current head of the family, raises you further to divinity."
                }),
            
            new KUPFaction("Princess of the Sector",8,FactionType.Vers2, 0,0,
                GetAssetsFromIDS(new ()
                {
                    127, 131, 135, 142,147, 150, 149,146, 141,288,
                    289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299,300, 301, 302,
                    303, 304, 305, 308, 309, 310, 285, 286, 287, 282,306, 307, 
                }), solange,
                new ()
                {
                    "You believe you are the illegimate daughter of the Old Emperor, Marcus Tullius Caesar",
                    "You are rebellious and independent by nature. You know your rightful place is to rule, not to be ruled, and anyone who wants to rule you must be made to understand they were wrong for daring to assert their superiority to you. ",
                    "Your people are kind and caring, they want to avoid war as much as possible. The only just war is when the oppressed fights the oppressor. ",
                    "You are the princess; chosen because of your kindness, your people look up to you for justice and as an inspiration for what good can be."
                },
                new ()
                {
                    "You must not be a subordinate in a faction, only an equal at the end of the game.",
                    "You seek to mediate peace and avoid wars, but if war is to break out, you must side with the side which you believe to be the oppressed. At the end of the game is must be well establsihed that you have tried to prevent wars.",
                    "You must never loose the following systems, as they are the heart of your kingdom: New Jersey (40, 14), Uttar Pradesh (38, 15), Zonguldak (38, 7)"
                }),
            
            new KUPFaction("Ancapistan",9,FactionType.Deutchria1, 0,0,
                GetAssetsFromIDS(new ()
                { 
                    62,22,24, 27, 28, 29, 32,33,  68,70,72, 76, 77, 78,
                    82, 83, 84,63, 217,38, 39, 40, 55, 54, 59, 
                    211, 222, 223, 225,52, 53, 57, 58, 46, 45
                }), jake,
                new ()
                {
                    "Contracts are king. People should be writing them, and respecting them. By force if necessary.",
                    "you are for sale. Everything with you is for sale, so long as you can make a profit",
                    "Anacapistan Baby! Fuck the state, insurance companies and money rule everything."
                },
                new ()
                {
                    "Create and force people to respect written contracts.",
                    "Be the richest player by combined assets, physical items bought during the game, and in game money at the end of the game.",
                    "To ensure your ideals can spread and live on Control the following systems: Odisha (3, 14), Jalisco (1, 23), Androy (10, 29), Mariy-El (28, 23), Baja California (35, 17), and Moscow (31, 17)  "
                }),
            new KUPFaction("Church of Sigmar",10,FactionType.Deutchri2, 0,0,
                GetAssetsFromIDS(new ()
                {
                    580, 583, 582, 585, 586, 588, 589, 590, 591, 592, 594, 595,
                    596, 597, 598, 599, 577,578, 579, 576, 575, 574, 408, 414, 413,
                    573, 570, 569, 572, 403, 553, 417
                    }), finn,
                new ()
                {
                    "You broke away from monarchy for an equal society under Sigmoria. Monarchy is wrong and must be toppled",
                    "You are a religious fanatic for the Church of Sigmar, you want to spread the church everywhere",
                    "The church believes SIgmoria is the one true god, and all actions and spread in his name, as he empowers all actions. You are his Sigmarette, a divine agent sent to carry out his will. WHo can feel his spirit and interpret it."
                },
                new ()
                {
                    "Prove you're on top. Get everyone to sign treaties confirming that they will suppress all other religions within their systems.",
                    "reclaim the following systems because they are your holy sites: Central Juntland (12, 6), Limburg (5, 13), and Ile-De-France (32, 9)",
                    "As you hate Monarchy for its placing of people as near God; Destroy all monarchies and princessess on the map."
                }),
            new KUPFaction("Xiao-Ming Sectorial Branch Office",11,FactionType.XiaoMing1, 0,0,
                GetAssetsFromIDS(new ()
                {
                    313, 311, 312, 316, 317, 314, 315, 162, 167, 164, 165, 166, 169, 
                    170, 171, 172, 173, 174, 177, 182, 185,178,186, 187,
                    188, 345, 183, 180, 179, 176, 191, 192
                }), maya,
                new ()
                {
                    "You're this sector's branch of THE megacorporation. You as the middle manager assigned here are looking to make your money and get out",
                    "Money comes before morals",
                    "You're a wolf, they are sheep, and its time to feast."
                },
                new ()
                {
                    "Prove you're the best. Be the richest player at the end of the game; both by goods you have bought/own at the end of the game, and the money you have.",
                    "Dont loose the corporate headquarters, ie the following systems:Limburg (5, 13), Jalisco (1, 23), and Maharashtra (5, 16)",
                }),
            
            new KUPFaction("The New Empire",12,FactionType.Imperial1, 0,0,
                GetAssetsFromIDS(new ()
                {
                    467,466, 471, 318, 324, 470, 469, 472, 468, 474, 476, 478, 477, 479, 480, 481,
                    482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493,495, 496, 498, 319,
                    320, 321, 323, 325, 326, 327, 328, 329, 330,331, 333,334, 339, 475, 473,
                    340, 500, 506, 507, 512, 513, 516, 521, 344,
                }), logan,
                new ()
                {
                    "You are the new empire, the old empire has fallen and is weak, you are here to replace it.",
                    "Pirates are a scourge to the empire, they are worse then all others, destroy them.",
                    "The empire is the greatest thing in the sector, and all others should be made to realize that!"
                },
                new ()
                {
                    "Crush The Old Empire.",
                    "Pirates are a pain, destroy them.",
                    "Be recognized as the Emperor of the Sector by all other players, signed in a treaty.",
                    "Do not loose: England (5, 22), Okayama (3, 23), and Managua (1, 28) as these are the capital systems of the Empire.",
                    
                }),
            
            
            new KUPFaction("The Old Empire",13, FactionType.Imperial3, 5000,8000,
                GetAssetsFromIDS(new ()
                {
                    139,140,143,144,148,138,137,136,134,133,132,129,128,124,
                    123,122, 499, 504, 505, 510, 503, 509, 508, 515, 514, 518,519,511,
                    355, 360, 361, 362, 363, 364,365,366,367, 368,369,370,371,372,373,
                    374,375,376,378, 379,380,381,382,383,384,385,386,387,359, 502, 501,

                }),malik,
                new ()
                {
                    "You are the Empire which rightfully rules this sector and has done so for hundreds of years. All were and one day will again be your subjects.",
                    "Deals are for chumps, only to be adhered to so long as they're good for you",
                    "paper is cheap, but your signature should never be on it."
                },
                new ()
                {
                    "Do not be destroyed",
                    "End the game having been declared the true Emperor by all other players, with recognization on a signed document declaring you as true Emperor",
                    "Crush the false 'New Empire', by retaking the Empires capital systems systems: England (5, 22), Okayama (3, 23), and Managua (1, 28)"
                }),
        };

        SetupBaseRelationships();

        AddShips();

        KupDrawSector = new KUPDrawSector(Sector, Factions, this);


        var totalIncome = AssetsInPlay.Aggregate(0, (h, t) => h + t.MoneyTotal);
        var totalInfluence = AssetsInPlay.Aggregate(0, (h, t) => h + t.InfluenceTotal);
        Console.WriteLine($"Total income: {totalIncome}. Total influence: {totalInfluence}");

        AssetsToFight = new List<KUPCombatAsset>();
    }

    private void AddShips()
    {
        /*
        //Game
        //ID 3
        */
        AddPirateShips();
        //ID 0, 1, and 2
        //AddGmBankShips();
        
        //Federation

        //ID 4
        AddGraysonShips();
        
        AddWesShips();
        
        AddAlexShips();

        //Vers
        AddMaxShips();
        AddSolangeShips();
        
        //Deutch
        AddJakeShips();
        AddFinnShips();
        
        //Empire
        AddLoganShips();
        AddMalikShips();
        
        //Xiao-Ming
        AddMayaShips();
        
    }

    private void AddPirateShips()
    {
        var owenSHips = 380082;
        var owen = Factions.First(x => x.FactionID == 3);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(19, 28),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(19, 29),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(23, 30),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(36, 17),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(33, 20),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(29, 9),owen , CombatAssetSize.Large,
            _shipIDs++, owenSHips++), owen);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(19, 17),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(18, 6),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(18, 7),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(18, 8),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(12, 1),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(12, 2),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(12, 3),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(4, 8),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(5, 17),owen , CombatAssetSize.Medium,
            _shipIDs++, owenSHips++), owen);
    }

    private void AddGraysonShips()
    {
        var graysonShips = 480082;
        var grayson = Factions.First(x => x.FactionID == 4);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(38, 21),grayson , CombatAssetSize.Station,
            _shipIDs++, graysonShips++), grayson);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(35, 17),grayson , CombatAssetSize.Station,
            _shipIDs++, graysonShips++), grayson);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(34, 19),grayson , CombatAssetSize.Station,
            _shipIDs++, graysonShips++), grayson);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(32, 22),grayson , CombatAssetSize.Station,
            _shipIDs++, graysonShips++), grayson);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(33, 26),grayson , CombatAssetSize.Station,
            _shipIDs++, graysonShips++), grayson);
        
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(34, 27),grayson , CombatAssetSize.Medium,
            _shipIDs++, graysonShips++), grayson);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(33, 25),grayson , CombatAssetSize.Medium,
            _shipIDs++, graysonShips++), grayson);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(33, 26),grayson , CombatAssetSize.Medium,
            _shipIDs++, graysonShips++), grayson);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(38, 17),grayson , CombatAssetSize.Medium,
            _shipIDs++, graysonShips++), grayson);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(39, 18),grayson , CombatAssetSize.Medium,
            _shipIDs++, graysonShips++), grayson);

        
    }

    private void AddWesShips()
    {
        var wesShips = 680082;
        var wes = Factions.First(x => x.FactionID == 6);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(22, 26),wes , CombatAssetSize.Station,
            _shipIDs++, wesShips++), wes);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(23, 29),wes , CombatAssetSize.Station,
            _shipIDs++, wesShips++), wes);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(24, 31),wes , CombatAssetSize.Station,
            _shipIDs++, wesShips++), wes);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(23, 32),wes , CombatAssetSize.Station,
            _shipIDs++, wesShips++), wes);

        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(25, 29),wes , CombatAssetSize.Small,
            _shipIDs++, wesShips++), wes);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(24, 30),wes , CombatAssetSize.Small,
            _shipIDs++, wesShips++), wes);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(23, 31),wes , CombatAssetSize.Small,
            _shipIDs++, wesShips++), wes);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(20, 29),wes , CombatAssetSize.Small,
            _shipIDs++, wesShips++), wes);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(20, 28),wes , CombatAssetSize.Small,
            _shipIDs++, wesShips++), wes);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(21, 26),wes , CombatAssetSize.Small,
            _shipIDs++, wesShips++), wes);
        
    }

    private void AddAlexShips()
    {
        //Ile-De-France (32, 9), Baden0Wurttembe (29, 2), Indiana (23, 10), and Utah (23, 4)
        var alexShis = 580082;
        var alex = Factions.First(x => x.FactionID == 5);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(32, 9),alex , CombatAssetSize.Station,
            _shipIDs++, alexShis++), alex);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(29, 2),alex , CombatAssetSize.Station,
            _shipIDs++, alexShis++), alex);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(23, 10),alex , CombatAssetSize.Station,
            _shipIDs++, alexShis++), alex);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(23, 4 ),alex , CombatAssetSize.Station,
            _shipIDs++, alexShis++), alex);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(23, 5),alex , CombatAssetSize.Large,
            _shipIDs++, alexShis++), alex);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(23, 6),alex , CombatAssetSize.Large,
            _shipIDs++, alexShis++), alex);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(33, 7),alex , CombatAssetSize.Large,
            _shipIDs++, alexShis++), alex);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(32, 8),alex , CombatAssetSize.Large,
            _shipIDs++, alexShis++), alex);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(31, 3),alex , CombatAssetSize.Large,
            _shipIDs++, alexShis++), alex);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(31, 4),alex , CombatAssetSize.Large,
            _shipIDs++, alexShis++), alex);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(31, 1),alex , CombatAssetSize.Large,
            _shipIDs++, alexShis++), alex);
    }

    private void AddSolangeShips()
    {
        //New Jersey (40, 14), Uttar Pradesh (38, 15), Zonguldak (38, 7)
        var solangeShips = 880082;
        var solange = Factions.First(x => x.FactionID == 8);
        //Station at Attack fleet
        NewAsset(new KUPCombatAsset(
            new KUPLocation(40, 14),solange , CombatAssetSize.Station,
            _shipIDs++, solangeShips++), solange);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(38, 15),solange , CombatAssetSize.Station,
            _shipIDs++, solangeShips++), solange);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(38, 7),solange , CombatAssetSize.Station,
            _shipIDs++, solangeShips++), solange);
        
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(34, 8),solange , CombatAssetSize.Large,
            _shipIDs++, solangeShips++), solange);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(35, 13),solange , CombatAssetSize.Large,
            _shipIDs++, solangeShips++), solange);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(37, 15),solange , CombatAssetSize.Large,
            _shipIDs++, solangeShips++), solange);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(39, 16),solange , CombatAssetSize.Large,
            _shipIDs++, solangeShips++), solange);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(40, 16),solange , CombatAssetSize.Large,
            _shipIDs++, solangeShips++), solange);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(36, 13),solange , CombatAssetSize.Large,
            _shipIDs++, solangeShips++), solange);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(35, 8),solange , CombatAssetSize.Large,
            _shipIDs++, solangeShips++), solange);
        
    }

    private void AddMaxShips()
    {
        var maxShips = 780082;
        var max = Factions.First(x => x.FactionID == 7);
        //Station defense
        NewAsset(new KUPCombatAsset(
            new KUPLocation(3, 14),max , CombatAssetSize.Station,
            _shipIDs++, maxShips++), max);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(11, 9),max , CombatAssetSize.Station,
            _shipIDs++, maxShips++), max);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(6, 6),max , CombatAssetSize.Station,
            _shipIDs++, maxShips++), max);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(9, 6),max , CombatAssetSize.Large,
            _shipIDs++, maxShips++), max);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(3, 13),max , CombatAssetSize.Large,
            _shipIDs++, maxShips++), max);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(3, 10),max , CombatAssetSize.Large,
            _shipIDs++, maxShips++), max);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(5, 9),max , CombatAssetSize.Large,
            _shipIDs++, maxShips++), max);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(7, 8),max , CombatAssetSize.Large,
            _shipIDs++, maxShips++), max);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(5, 2),max , CombatAssetSize.Large,
            _shipIDs++, maxShips++), max);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(5, 4),max , CombatAssetSize.Large,
            _shipIDs++, maxShips++), max);
        
    }

    private void AddJakeShips()
    {
        var jakeShips = 980082;
        var jake = Factions.First(x => x.FactionID == 9);
        //Station at Attack fleet
        NewAsset(new KUPCombatAsset(
            new KUPLocation(19, 9),jake , CombatAssetSize.Small,
            _shipIDs++, jakeShips++), jake);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(19, 7),jake , CombatAssetSize.Small,
            _shipIDs++, jakeShips++), jake);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(18, 6),jake , CombatAssetSize.Small,
            _shipIDs++, jakeShips++), jake);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(16, 6),jake , CombatAssetSize.Small,
            _shipIDs++, jakeShips++), jake);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(18, 10),jake , CombatAssetSize.Small,
            _shipIDs++, jakeShips++), jake);
        
        //Big guns
        NewAsset(new KUPCombatAsset(
            new KUPLocation(11, 3),jake , CombatAssetSize.Large,
            _shipIDs++, jakeShips++), jake);

        NewAsset(new KUPCombatAsset(
            new KUPLocation(9, 3),jake , CombatAssetSize.Large,
            _shipIDs++, jakeShips++), jake);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(7, 2),jake , CombatAssetSize.Large,
            _shipIDs++, jakeShips++), jake);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(12, 5),jake , CombatAssetSize.Large,
            _shipIDs++, jakeShips++), jake);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(12, 6),jake , CombatAssetSize.Large,
            _shipIDs++, jakeShips++), jake);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(21, 6),jake , CombatAssetSize.Large,
            _shipIDs++, jakeShips++), jake);
    }

    private void AddFinnShips()
    {
        var finnShips = 1080082;
        var finn = Factions.First(x => x.FactionID == 10);
        //Station at Veneto
        NewAsset(new KUPCombatAsset(
            new KUPLocation(22, 25),finn , CombatAssetSize.Station,
            _shipIDs++, finnShips++), finn);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(31, 31),finn , CombatAssetSize.Station,
            _shipIDs++, finnShips++), finn);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(28, 23),finn , CombatAssetSize.Station,
            _shipIDs++, finnShips++), finn);
        
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(29, 22),finn , CombatAssetSize.Medium,
            _shipIDs++, finnShips++), finn);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(31, 26),finn , CombatAssetSize.Medium,
            _shipIDs++, finnShips++), finn);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(32, 27),finn , CombatAssetSize.Medium,
            _shipIDs++, finnShips++), finn);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(32, 28),finn , CombatAssetSize.Medium,
            _shipIDs++, finnShips++), finn);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(25, 27),finn , CombatAssetSize.Medium,
            _shipIDs++, finnShips++), finn);
    }

    private void AddMalikShips()
    {
        
        var malikShips = 1380082;
        var malik = Factions.First(x => x.FactionID == 13);
        //South Start fleet
        NewAsset(new KUPCombatAsset(
            new KUPLocation(33, 2),malik , CombatAssetSize.Large,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(33, 3),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(33, 4),malik , CombatAssetSize.Large,
            _shipIDs++, malikShips++), malik);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(38, 5),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(38, 4),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(39, 4),malik , CombatAssetSize.Large,
            _shipIDs++, malikShips++), malik);
        
        //North east fleet
        NewAsset(new KUPCombatAsset(
            new KUPLocation(9, 27),malik , CombatAssetSize.Large,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(9 ,28),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(10, 29),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(11, 29),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(12, 29),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(9, 30),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(9, 31),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(8, 32),malik , CombatAssetSize.Large,
            _shipIDs++, malikShips++), malik);
        
        //Middle fleets
        NewAsset(new KUPCombatAsset(
            new KUPLocation(16, 24),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(15, 17),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(13, 20),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(20, 18),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(19, 24),malik , CombatAssetSize.Medium,
            _shipIDs++, malikShips++), malik);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(20, 24),malik , CombatAssetSize.Large,
            _shipIDs++, malikShips++), malik);
    }

    private void AddLoganShips()
    {
        var loganShips = 1280082;
        var logan = Factions.First(x => x.FactionID == 12);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(11, 27),logan , CombatAssetSize.Large,
            _shipIDs++, loganShips++), logan);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(10, 26),logan , CombatAssetSize.Large,
            _shipIDs++, loganShips++), logan);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(8, 28),logan , CombatAssetSize.Large,
            _shipIDs++, loganShips++), logan);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(4, 20),logan , CombatAssetSize.Large,
            _shipIDs++, loganShips++), logan);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(3, 23),logan , CombatAssetSize.Large,
            _shipIDs++, loganShips++), logan);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(8, 22),logan , CombatAssetSize.Large,
            _shipIDs++, loganShips++), logan);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(10, 24),logan , CombatAssetSize.Large,
            _shipIDs++, loganShips++), logan);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(6, 19),logan , CombatAssetSize.Large,
            _shipIDs++, loganShips++), logan);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(5, 19),logan , CombatAssetSize.Large,
            _shipIDs++, loganShips++), logan);
    }

    private void AddMayaShips()
    {

        var mayaShips = 1180082;
        var maya = Factions.First(x => x.FactionID == 11);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(8, 10),maya , CombatAssetSize.Large,
            _shipIDs++, mayaShips++), maya);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(11, 17),maya , CombatAssetSize.Large,
            _shipIDs++, mayaShips++), maya);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(4, 16),maya , CombatAssetSize.Large,
            _shipIDs++, mayaShips++), maya);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(2, 17),maya , CombatAssetSize.Large,
            _shipIDs++, mayaShips++), maya);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(6, 16),maya , CombatAssetSize.Large,
            _shipIDs++, mayaShips++), maya);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(9, 15),maya , CombatAssetSize.Large,
            _shipIDs++, mayaShips++), maya);
        
        NewAsset(new KUPCombatAsset(
            new KUPLocation(1, 23),maya , CombatAssetSize.Station,
            _shipIDs++, mayaShips++), maya);
        NewAsset(new KUPCombatAsset(
            new KUPLocation(5, 13),maya , CombatAssetSize.Station,
            _shipIDs++, mayaShips++), maya);
        
    }

    private void SetupBaseRelationships()
    {
        foreach (var faction in Factions)
        {
            faction.SetStartingFactionRelationships(Factions
                .Where(x => x.FactionID != faction.FactionID).ToList());
        }
    }

    public string GetLocationName(int x, int y)
    {
        return Sector.GetSystemName(x, y);
    }

    #endregion

    #region Assets

    public IKUPAsset? GetAssetFromID(int assetID)
    {
        return AssetsInPlay.Find(x => x.assetID == assetID);
    }
    
    public List<IKUPAsset?> GetAssetsFromIDS(List<int> assetIDs)
    {
        var assets = new List<IKUPAsset?>();

        foreach (var assetID in assetIDs)
        {
            assets.Add(GetAssetFromID(assetID));
        }

        return assets;
    }

    public int GetNewAssetID()
    {
        return CurrentAssetId++;
    }


    
    public int GetNewPOIAssetID()
    {
        return CurrentPOIAssetId++;
    }
    
    public void NewAsset(IKUPAsset asset)
    {
        if (asset.Controller != null) NewAsset(asset, asset.Controller);
        else AssetsInPlay.Add(asset);
    }

    public void NewAsset(IKUPAsset asset, KUPFaction faction)
    {
        AssetsInPlay.Add(asset);
        asset.Controller = faction;
        faction.AddAsset(asset);
    }

    public void DestroyAsset(IKUPAsset asset, KUPFaction faction)
    {
        AssetsInPlay.Remove(asset);
        faction.DestroyAsset(asset);
    }

    #endregion

    public void EndOfTurn()
    {
        EventService.AddEvent(new IKUPMessageEvent(SenderID, -1,
            $"Round {CurrentRound} has ended. Come to the dining room."));
        foreach (var faction in Factions)
        {
            faction.Update();
        }

        HandleCombats();

        RemoveDestroyedAssets();

        CurrentRound++;
    }

    private List<KUPCombatAsset> CombatAssets => AssetsInPlay.OfType<KUPCombatAsset>().ToList();
    
    private void HandleCombats()
    {
        //Have all ships attempt to fight so we don't need a fight commnat
        //TODO make fight command easier to issue so you can just do that instead
        AssetsToFight = CombatAssets;
        foreach(var asset in AssetsToFight){
            if (asset.HP > 0)
            {
                if (CombatAssets
                    .Any(x => x.Location == asset.Location
                              && x.AtWar(asset)
                              && x.HP > 0))
                {
                    var targetShip = CombatAssets
                        .First(x => x.Location == asset.Location
                                    && x.AtWar(asset)
                                    && x.HP > 0);

                    EventService.AddEvent(
                        new KUPShipDamagedEvent(asset.SenderID, targetShip.ReceiverID, asset.AttackPower));
                }
            }
        }
        
        AssetsToFight = new List<KUPCombatAsset>();
    }

    private void RemoveDestroyedAssets()
    {
        var toBeDestroyed = new List<KUPCombatAsset>();
        foreach (var asset in AssetsInPlay.Where(x => x.GetType() == typeof(KUPCombatAsset)))
        {
            if (((KUPCombatAsset) asset).HP <= 0)
            {
                toBeDestroyed.Add(asset as KUPCombatAsset);
            }
        }

        foreach (var ass in toBeDestroyed)
        {
            DestroyAsset(ass, ass.Controller);
        }
    }

    #region Events

    public void ProcessEvent(IKUPEvent evnt)
    {
        evnt.RunEvent(this,EventService);
    }

    public void BuildShip(KUPLocation buildLoc, KUPFaction buildFaction, CombatAssetSize Size)
    {
        NewAsset(new KUPCombatAsset(buildLoc, buildFaction, Size, GetNewAssetID(), _shipIDs++),
            buildFaction);
    }

    public int GetOperationCost(KUPOPerationSize evntOperationSize, KUPOperationType evntOperationNumber)
    {
        var amount = evntOperationSize switch
        {
            KUPOPerationSize.Small => 10,
            KUPOPerationSize.Medium => 25,
            KUPOPerationSize.Large => 50,
            _ => 50,
        };

        amount *= evntOperationNumber switch
        {
            KUPOperationType.InfluenceAttack => 2,
            KUPOperationType.MoneyAttack => 3,
            KUPOperationType.MilitaryAttack => 5,
            _ => 2,
        };

        return amount;
    }

    public int GetAmountOfOperationDamage(KUPOPerationSize evntOperationSize)
        => evntOperationSize switch
        {
            KUPOPerationSize.Small => 25,
            KUPOPerationSize.Medium => 75,
            KUPOPerationSize.Large => 125,
            _ => 250,
        };

    public void DepositMoney(IKUPEventActor transferee, int evntAmountOfMoney)
    {
        if (transferee.GetType() == typeof(KUPPlayer))
        {
            ((KUPPlayer) transferee).PersonalFunds += evntAmountOfMoney;
        }

        if (transferee.GetType() == typeof(KUPFaction))
        {
            ((KUPFaction) transferee).Money += evntAmountOfMoney;
        }
    }

    public void WithdrawMoney(IKUPEventActor transferer, int evntAmountOfMoney)
    {
        if (transferer.GetType() == typeof(KUPPlayer))
        {
            ((KUPPlayer) transferer).PersonalFunds -= evntAmountOfMoney;
        }

        if (transferer.GetType() == typeof(KUPFaction))
        {
            ((KUPFaction) transferer).Money -= evntAmountOfMoney;
        }
    }

    #endregion

    public KUPFaction GetFaction(string name)
    {
        return Factions.First(x => x.Name == name);
    }

    public KUPFaction GetFaction(int id)
    {
        return Factions.First(x => x.FactionID == id);
    }


    public bool CouldCaptureSystem(KUPFilledSystem system, KUPFaction who)
    {
        if (system.SysetmAsset.Controller == null ||
            system.SysetmAsset.Controller.FactionType == FactionType.Unclaimed ||
            system.SysetmAsset.Controller == who) return false;
        var locX = system.DisplayX;
        var locY = system.DisplayY;
        var ships = AssetsInPlay.Where(x => x.Location.SystemX == locX && x.Location.SystemY == locY);
        return ships.Any(x => x.Controller == system.SysetmAsset.Controller);

    }

    public void AdminTranferAssets(KUPFaction targetFaction, List<IKUPAsset> assetsToTransfer)
    {
        //First transfer all the selects assets to the GM from their respective owners.
        foreach (var asset in assetsToTransfer)
        {
            EventService.AddEvent(new KUPAssetTransferEvent(asset.Controller.SenderID,
                GameMaster.ReceiverID,new List<int>(){asset.assetID}));
        }
        
        //Then transfer the assets from the GM to the target player.
            EventService.AddEvent(
                new KUPAssetTransferEvent(GameMaster.SenderID,targetFaction.FactionID,
                    assetsToTransfer.Select(x => x.assetID).ToList()));
        
    }

    public void SetPlayerFaction(string name, int factionId)
    {
        var player = Players.First(x => x.Name == name);
        var faction = GetFaction(factionId);
        
        EventService.AddEvent(
            new KUPPlayerChangeFactionEvent(player.SenderID,faction.ReceiverID));
    }

    public List<IKUPLocationAsset> GetBuildLocations(KUPFaction? faction)
    {
        var locations = new List<IKUPLocationAsset>();
        if (faction != null)
            foreach (var location in faction.LocationAssets)
            {
                var tradeCodes = location.TradeCodes;
                if (faction.FactionType == FactionType.Pirates)
                {
                    if (tradeCodes.Contains(KURPGTradeCodes.Bh)
                        || tradeCodes.Contains(KURPGTradeCodes.Is)
                        || tradeCodes.Contains(KURPGTradeCodes.Io))
                    {
                        locations.Add(location);
                    }
                }
                else
                {
                    if (tradeCodes.Contains(KURPGTradeCodes.Ms)
                        || tradeCodes.Contains(KURPGTradeCodes.Mb))
                    {
                        locations.Add(location);
                    }
                }
            }

        return locations;
    }

    public void CreateNewFaction(string name, int facID, FactionType factionType,
        int money, int influence, List<IKUPAsset> assets, KUPPlayer? player, List<string> summary,
        List<string> goals)
    {
        var faction = new KUPFaction(name,facID,factionType,money,influence,assets,player,summary,goals);
        Factions.Add(faction);
        
        var oldFac = player.ChangeFaction(faction);       
        oldFac?.NewPlayer(null);
        //Put the assigned player in charge of the new faction and assign the old faction to no one.

    }
}