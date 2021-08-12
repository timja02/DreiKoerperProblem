using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DreiKoerperproblem
{
    public partial class MainWindow : Window
    {        
        DispatcherTimer Zeit = new DispatcherTimer();        
        List<Koerper> AlleKoerper = new List<Koerper>();

        double SimulationsIntervall = 0.001;
        double Gravitationskonstante = 3.0;
        bool ReflextionAnDerWand = false;
        bool SpurZeichnen = false;

        double Gesamtzeit = 0.0;

        // Zufallszahl Basis für die Farbgebung generieren
        Random ZufallsFarbe = new Random(Environment.TickCount);

        public MainWindow() {

            InitializeComponent();

            Zeit.Interval = TimeSpan.FromSeconds(SimulationsIntervall);
            Zeit.IsEnabled = true;
            // Registrieren der Methode, die bei jedem Tick ausgeführt werden soll
            Zeit.Tick += Simulation;

            InitKoerper();
        }

        private void InitKoerper() {

            /*KoerperHinzufuegen(
                XKoordinate: 250,
                YKoordinate: 80,
                Masse: 50,
                XGeschwindigkeit: 30,
                YGeschwindigkeit: 0);*/

            KoerperHinzufuegen(
                XKoordinate: 100, 
                YKoordinate: 100, 
                Masse: 50,                 
                XGeschwindigkeit: 50, 
                YGeschwindigkeit: -50);

            KoerperHinzufuegen(
                XKoordinate: 400, 
                YKoordinate: 400, 
                Masse: 50,                 
                XGeschwindigkeit: -50,
                YGeschwindigkeit: 50);   
        }

        private void KoerperHinzufuegen(double XKoordinate, double YKoordinate, int Masse, double XGeschwindigkeit, double YGeschwindigkeit) {

            Color C = Color.FromRgb((byte)ZufallsFarbe.Next(0, 255), (byte)ZufallsFarbe.Next(0, 255), (byte)ZufallsFarbe.Next(0, 255));

            Koerper K = new Koerper
            {                
                AktuellePosition = new Vektor(XKoordinate, YKoordinate),
                Masse = Masse,
                Groesse = Masse,
                Geschwindigkeit = new Vektor(XGeschwindigkeit, YGeschwindigkeit),
                Farbe = new SolidColorBrush(C)                
            };

            K.KoerperUI = new Ellipse()
            {
                Width = K.Groesse,
                Height = K.Groesse,
                Fill = new SolidColorBrush(C)                
            };
            
            Zeichenflaeche.Children.Add(K.KoerperUI);

            Canvas.SetLeft(K.KoerperUI, K.AktuellePosition.X - K.Groesse / 2);
            Canvas.SetTop(K.KoerperUI, K.AktuellePosition.Y - K.Groesse / 2);

            Canvas.SetZIndex(K.KoerperUI, K.Masse * -1);

            AlleKoerper.Add(K);
        }
        
        private void Simulation (object sender, EventArgs e) {
            Gesamtzeit += Zeit.Interval.TotalSeconds;
            foreach (Koerper EinzelKoerper1 in AlleKoerper) {
                
                Vektor BeschleunigungKoerper1 = new Vektor();
                foreach (Koerper EinzelKoerper2 in AlleKoerper) {
                    if (object.ReferenceEquals(EinzelKoerper1, EinzelKoerper2)) { continue; }

                    Vektor BeschleunigungKoerper2 = new Vektor();

                    Vektor AbstandKoerper1zuKoerper2 = EinzelKoerper2.AktuellePosition - EinzelKoerper1.AktuellePosition;
                    double AbstandBetrag = Math.Sqrt((AbstandKoerper1zuKoerper2.X * AbstandKoerper1zuKoerper2.X) + (AbstandKoerper1zuKoerper2.Y * AbstandKoerper1zuKoerper2.Y));

                    double GravitationsFaktor = (Gravitationskonstante * ((EinzelKoerper1.Masse * EinzelKoerper2.Masse) / (AbstandBetrag * AbstandBetrag))) / EinzelKoerper1.Masse;                    
                    AbstandKoerper1zuKoerper2 *= GravitationsFaktor;

                    BeschleunigungKoerper2 = AbstandKoerper1zuKoerper2;
                    BeschleunigungKoerper1 += BeschleunigungKoerper2;
                }
                EinzelKoerper1.Geschwindigkeit += BeschleunigungKoerper1;
                EinzelKoerper1.Beschleunigung = BeschleunigungKoerper1;
                
                EinzelKoerper1.NeuePosition = EinzelKoerper1.AktuellePosition + EinzelKoerper1.Geschwindigkeit * Zeit.Interval.TotalSeconds;

            }

            // neu positionieren
            foreach (Koerper K in AlleKoerper) {
                K.Spur.Add(K.AktuellePosition);
                K.AktuelleSpurLaenge += 1;

                K.AktuellePosition = K.NeuePosition;

                if (K.AktuelleSpurLaenge >= K.MaxSpurLaenge) {
                    //Console.WriteLine("Spur länger als " + K.MaxSpurLaenge.ToString());
                    K.Spur.RemoveAt(0);
                    K.AktuelleSpurLaenge -= 1;
                }
            }
            
            /*
            if (Gesamtzeit > 60)
            {
                Console.WriteLine("STOP");
            }
            */

            AllesZeichnen();
        }
        
        private void AllesZeichnen() {
            Anzahl.Text = AlleKoerper.Count.ToString();
            double MittelX = 0;
            double MittelY = 0;

            foreach (Koerper EinzelKoerper in AlleKoerper)
            {
                MittelX += EinzelKoerper.AktuellePosition.X;
                MittelY += EinzelKoerper.AktuellePosition.Y;

                if (ReflextionAnDerWand) {
                    if (EinzelKoerper.AktuellePosition.X - EinzelKoerper.Groesse / 2 <= 0.0 || EinzelKoerper.AktuellePosition.X >= Zeichenflaeche.ActualWidth - EinzelKoerper.Groesse / 2)
                    {
                        EinzelKoerper.Geschwindigkeit.X *= -1;
                    }

                    if (EinzelKoerper.AktuellePosition.Y - EinzelKoerper.Groesse / 2 <= 0.0 || EinzelKoerper.AktuellePosition.Y >= Zeichenflaeche.ActualHeight - EinzelKoerper.Groesse / 2)
                    {
                        EinzelKoerper.Geschwindigkeit.Y *= -1;
                    }
                }
                                
                Canvas.SetLeft(EinzelKoerper.KoerperUI, EinzelKoerper.AktuellePosition.X - EinzelKoerper.Groesse / 2);
                Canvas.SetTop(EinzelKoerper.KoerperUI, EinzelKoerper.AktuellePosition.Y - EinzelKoerper.Groesse / 2);

                if (SpurZeichnen) {
                    // ToDo: Liste der Spur-Positionen anzeigen für eine endliche Spur von MaxSpurLaenge Punkten

                    // altes Verfahren: einfach nur eine Spur zeichnen
                    Ellipse Spur = new Ellipse()
                    {
                        Width = 2,
                        Height = 2,
                        Fill = EinzelKoerper.Farbe
                    };
                    Zeichenflaeche.Children.Add(Spur);

                    // die Spur im Zentrum zeichnen
                    Canvas.SetLeft(Spur, EinzelKoerper.AktuellePosition.X - Spur.Width / 2);
                    Canvas.SetTop(Spur, EinzelKoerper.AktuellePosition.Y - Spur.Width / 2);
                }

            }
            MittelX /= MittelX / AlleKoerper.Count;
            MittelY /= MittelY / AlleKoerper.Count;
           // Canvas.SetLeft(AlleKoerper[0].KoerperUI, MittelX - 10);
            //Canvas.SetTop(AlleKoerper[0].KoerperUI, MittelY-10);


        }


        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key)
            {                                
                // Simulation Start/Stop                
                case Key.Pause:
                    Zeit.IsEnabled = !Zeit.IsEnabled;
                    break;

                // Anwendung schließen
                case Key.Escape:                    
                    Application.Current.Shutdown();
                    break;
            }
        }

        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {                    
            if (int.TryParse(MasseNeu.Text, out int gepruefteMasse)) {
                gepruefteMasse = e.Delta > 0 ? gepruefteMasse += 5 : gepruefteMasse -= 5;
                if (gepruefteMasse <= 10){
                    gepruefteMasse = 10;
                }
                MasseNeu.Text = gepruefteMasse.ToString();                
            }
        }

        private void KoerperErstellen (double X, double Y, int M) {
            KoerperHinzufuegen(
                XKoordinate: X,
                YKoordinate: Y,
                Masse: M,                
                XGeschwindigkeit: 0,
                YGeschwindigkeit: 0);
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (int.TryParse(MasseNeu.Text, out int gepruefteMasse)) {
                KoerperErstellen(Mouse.GetPosition(Zeichenflaeche).X, Mouse.GetPosition(Zeichenflaeche).Y, gepruefteMasse);
            }                
        }

        private void ReflextionWand_Click(object sender, RoutedEventArgs e) {
            ReflextionAnDerWand = !ReflextionAnDerWand;
        }

        private void KoerperNeu_Click(object sender, RoutedEventArgs e) {
            if (int.TryParse(MasseNeu.Text, out int gepruefteMasse)) {
                KoerperErstellen(100, 100, gepruefteMasse);
            }
        }

        private void MitSpurZeichnen_Click(object sender, RoutedEventArgs e) {
            SpurZeichnen = !SpurZeichnen;
        }        
    }
}