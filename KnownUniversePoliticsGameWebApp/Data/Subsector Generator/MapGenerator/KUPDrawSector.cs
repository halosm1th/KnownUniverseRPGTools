using System;
using System.Text;
using KnownUniversePoliticsGameWebApp.Data;
using Simple_Subsector_Generator;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TravellerMapSystem.Tools
{
    public class KUPDrawSector
    {
        //Dotnet Framework stuff

        private static readonly int xSize = KUPDrawSubsector.xSize*4;
        private static readonly int ySize = KUPDrawSubsector.ySize*4;
        private static readonly int HEIGHT = KUPDrawSubsector.HEIGHT;

        private readonly KURPGSector? _knownUniverseSectorToDraw;
        private readonly List<KUPFaction>? _factions;
        private KnownUniversePoliticsGame PoliticsGame;

        public KUPDrawSector(KURPGSector? knownUniverseSectorToDraw, List<KUPFaction>? factions, KnownUniversePoliticsGame politicsGame)
        {
            _knownUniverseSectorToDraw = knownUniverseSectorToDraw;
            _factions = factions;
            PoliticsGame = politicsGame;
        }

        public Image GenerateImage(bool printSubImages = false, string path = "", bool highVersian = false)
        {
            Console.WriteLine($"Beging to Generate Sector: {_knownUniverseSectorToDraw.Name}");
            Image subsector = CreateGrid();
            for (int x = 0; x < _knownUniverseSectorToDraw.Subsectors.GetLength(0); x++)
            {
                for (int y = 0; y < _knownUniverseSectorToDraw.Subsectors.GetLength(1); y++)
                {
                    var imageToAdd = new KUPDrawSubsector(_knownUniverseSectorToDraw.Subsectors[x,y], PoliticsGame);
                    var image = imageToAdd.GenerateImage(printSubImages,path);
                    var placeX = ((xSize/4) * x) - (x==0? 0 : KUPDrawSubsector.SPACER*(3*x));
                    var placeY = ((ySize/4) * y)- (y==0? 0 : KUPDrawSubsector.SPACER*(5*y));
                    var location = new Point(placeX, placeY);
                    subsector.Mutate(i => i.DrawImage(image,location,1));
                    
                    if (printSubImages)
                    {
                        image.SaveAsPng(path + " " + _knownUniverseSectorToDraw.Subsectors[x,y].Name + ".png");
                    }
                }   
            }

            var rect = new Rectangle(x: 0, y: 0, xSize, ySize);
            subsector.Mutate(i => i.Draw(Color.Black,10,rect));

            Console.WriteLine($"Finished Generating sector: {_knownUniverseSectorToDraw.Name}");
            return subsector;
        }

        private static Image CreateGrid()
        {
            var image = new Image<Rgba64>(xSize-130, ySize-225);
            image.Mutate(x => x.Fill(Color.Black,new Rectangle(x: 0, y: 0, xSize, ySize)));
            return image;
        }
    }
}