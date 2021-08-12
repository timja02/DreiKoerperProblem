using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace DreiKoerperproblem
{
    
    public class Koerper {

        public int MaxSpurLaenge = 50;
        public List<Vektor> Spur = new List<Vektor>();
        
        public int AktuelleSpurLaenge { get; set; }
        public UIElement KoerperUI { get; set; }
        public Vektor AktuellePosition { get; set; }
        public Vektor NeuePosition { get; set; }
        public Vektor Geschwindigkeit { get; set; }
        public Vektor Beschleunigung {get; set; }
        public int Masse { get; set; }
        public int Groesse { get; set; }
        public Brush Farbe { get; set; }
        

        public override string ToString()
        {
            string erg="";

            erg += "X: "   + AktuellePosition.X;
            erg += ", Y: " + AktuellePosition.Y;
            erg += ", VX: " + Geschwindigkeit.X;
            erg += ", VY: " + Geschwindigkeit.Y;
            
            return erg;
            //base.ToString();
        }


    }
}
