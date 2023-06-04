using System;
using System.Text;
using Simple_Subsector_Generator;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TravellerMapSystem.Tools
{
    internal class DrawMegaSector
    {
        //Dotnet Framework stuff

        private static readonly int xSize = ((1050*4)*4)*6;
        private static readonly int ySize = ((1500*4)*4)*6;
        private static readonly int HEIGHT = 140;

        private readonly KURPGMegaSector? _knownUniverseSuperSectorToDraw;

        public DrawMegaSector(KURPGMegaSector? knownUniverseSuperSectorToDraw)
        {
            _knownUniverseSuperSectorToDraw = knownUniverseSuperSectorToDraw;
        }

        public Image GenerateImage(bool highVersian = false)
        {
            Console.WriteLine("Starting Megasector image generation");
            Image subsector = CreateGrid();
            for (int x = 0; x < _knownUniverseSuperSectorToDraw.SuperSectors.GetLength(0); x++)
            {
                for (int y = 0; y < _knownUniverseSuperSectorToDraw.SuperSectors.GetLength(1); y++)
                {
                    var imageToAdd = new DrawSuperSector(_knownUniverseSuperSectorToDraw.SuperSectors[x,y]);
                    var image = imageToAdd.GenerateImage();
                    var placeX = ((1050*4)*4) * x;
                    var placeY = ((1500*4)*4) * y;
                    var location = new Point(placeX, placeY);
                    subsector.Mutate(i => i.DrawImage(image,location,1));
                }   
            }
            return subsector;
        }


        private static int SPACER = 15;
        
        private static Image CreateGrid()
        {
            return new Image<Rgb24>(xSize, ySize);
            
        }
    }
}