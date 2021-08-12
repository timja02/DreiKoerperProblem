
namespace DreiKoerperproblem
{
    public class Vektor
    {
        private double XKor;
        private double YKor;
        
        public Vektor()
        {

        }
        public Vektor (double x, double y) {
            XKor = x;
            YKor = y;
        }
        public Vektor (Vektor vek) {
            XKor = vek.XKor;
            YKor = vek.YKor;
        }
        public double X 
        {
            get { return XKor; }
            set { XKor = value; }
        }
        public double Y
        {
            get { return YKor; }
            set { YKor = value; }
        }
        public static Vektor Addition(Vektor vek1, Vektor vek2) {
            if (vek1 == null || vek2== null)
                return null;
            return new Vektor(vek1.XKor + vek2.XKor, vek1.YKor + vek2.YKor);            
        }
        public static Vektor Subtraktion(Vektor vek1, Vektor vek2) {
            if (vek1 == null || vek2 == null)
                return null;
            return new Vektor(vek1.XKor - vek2.XKor, vek1.YKor - vek2.YKor);
        }
        public static Vektor Multiplikation(Vektor vek, double val) {
            if (vek == null)
                return null;
            return new Vektor(vek.X * val, vek.Y * val);
        }
        public static Vektor operator+(Vektor vek1, Vektor vek2) {
            if (vek1 == null || vek2 == null)
                return null;
            return Vektor.Addition(vek1, vek2);
        }        
        public static Vektor operator-(Vektor vek1, Vektor vek2) {
            if (vek1 == null || vek2 == null)
                return null;
            return Vektor.Subtraktion(vek1, vek2);
        }
        public static Vektor operator*(Vektor vek, double val) {
            if (vek == null)
                return null;
            return Vektor.Multiplikation(vek, val);
        }
    }
}
