using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace API.Model
{

    public class SpelBordConverter : JsonConverter<Kleur[,]>
    {
        public override Kleur[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Kleur[,] bord, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Kleur kleur = bord[y, x];
                    writer.WriteNumber(x + "," + y, kleur.GetHashCode());
                }
            }

            writer.WriteEndObject();
        }
    }

    public class Spel : ISpel
    {
        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }


        [NotMapped]
        private Kleur[,] _bord;

        [NotMapped]
        [JsonConverter(typeof(SpelBordConverter))]
        public Kleur[,] Bord { get => _bord; set => BordAsString = _convert(value); }

        [JsonIgnore]
        public string BordAsString
        {
            get => _convert(_bord);

            set => _bord = _convert(value);
        }

        private static string _convert(Kleur[,] bord)
        {
            string s = "";
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Kleur kleur = bord[x, y];
                    int v;
                    switch (kleur)
                    {
                        default:
                        case Kleur.Geen:
                            v = 0;
                            break;
                        case Kleur.Wit:
                            v = 1;
                            break;
                        case Kleur.Zwart:
                            v = 2;
                            break;
                    }

                    s += v;
                }

                s += ":";
            }

            return s;
        }

        private static Kleur[,] _convert(string str)
        {
            Kleur[,] bord = new Kleur[8, 8];

            int x = 0;
            foreach (string line in str.Split(":"))
            {
                int y = 0;
                foreach (char c in line.ToCharArray())
                {
                    Kleur kleur;

                    switch (c)
                    {
                        default:
                        case '0':
                            kleur = Kleur.Geen;
                            break;
                        case '1':
                            kleur = Kleur.Wit;
                            break;
                        case '2':
                            kleur = Kleur.Zwart;
                            break;
                    }

                    bord[y, x] = kleur;

                    y++;
                }

                x++;
            }

            return bord;
        }

        public Kleur AandeBeurt { get; set; }

        public Spel()
        {
            InitialiseerBord();
        }

        public void InitialiseerBord()
        {
            // Initialiseer
            Bord = new Kleur[8, 8];
            for (int rij = 0; rij < 8; rij++)
            {
                for (int kolom = 0; kolom < 8; kolom++)
                {
                    Bord[rij, kolom] = Kleur.Geen;
                }
            }

            // Standaard zetten
            Bord[3, 3] = Kleur.Wit;
            Bord[4, 4] = Kleur.Wit;
            Bord[4, 3] = Kleur.Zwart;
            Bord[3, 4] = Kleur.Zwart;
        }

        public bool Pas()
        {
            bool pas = true;

            // Kan er nog een zet gemaakt worden door de speler die aan de beurt is
            for (int rij = 0; rij < 8; rij++)
            {
                for (int kolom = 0; kolom < 8; kolom++)
                {
                    if (ZetMogelijk(rij, kolom))
                    {
                        pas = false;
                        break;
                    }
                }
            }

            if (pas)
            {
                AandeBeurt = KleurToggle(AandeBeurt);
            }

            return pas;
        }

        public bool Afgelopen()
        {
            bool afgelopen = true;

            // Kan er nog een zet gemaakt worden door de speler die aan de beurt is
            for (int rij = 0; rij < 8; rij++)
            {
                for (int kolom = 0; kolom < 8; kolom++)
                {
                    if (ZetMogelijk(rij, kolom))
                    {
                        afgelopen = false;
                        break;
                    }
                }
            }

            return afgelopen;
        }

        public Kleur OverwegendeKleur()
        {
            int geen = 0;
            int wit = 0;
            int zwart = 0;

            for (int rij = 0; rij < 8; rij++)
            {
                for (int kolom = 0; kolom < 8; kolom++)
                {
                    switch (Bord[rij, kolom])
                    {
                        case Kleur.Geen: geen++; break;
                        case Kleur.Wit: wit++; break;
                        case Kleur.Zwart: zwart++; break;
                        default: break;
                    }
                }
            }

            Kleur overwegendeKleur = Kleur.Geen;

            if (zwart > wit)
            {
                overwegendeKleur = Kleur.Zwart;
            }
            else if (wit > zwart)
            {
                overwegendeKleur = Kleur.Wit;
            }

            return overwegendeKleur;
        }

        public bool ZetMogelijk(int rijZet, int kolomZet)
        {
            // Buiten bord
            if (!opBord(rijZet, kolomZet))
                return false;

            // Moet Kleur.Geen zijn
            if (Bord[rijZet, kolomZet] != Kleur.Geen)
                return false;

            // Minimaal een omliggende
            int rij, kolom;
            for (rij = -1; rij <= 1; rij++)
                for (kolom = -1; kolom <= 1; kolom++)
                    if (!(rij == 0 && kolom == 0) && IsRichtingEenInsluiting(AandeBeurt, rijZet, kolomZet, rij, kolom))
                        return true;

            // Geen mogelijkheden
            return false;
        }

        public bool DoeZet(int rijZet, int kolomZet)
        {
            bool zetMogelijk = ZetMogelijk(rijZet, kolomZet);

            if (zetMogelijk)
            {
                // Set the disc on the square.
                Bord[rijZet, kolomZet] = AandeBeurt;

                // Flip any flanked opponents.
                int rijVector, kolomVector;
                int rijAankomst, kolomAankomst;
                for (rijVector = -1; rijVector <= 1; rijVector++)
                {
                    for (kolomVector = -1; kolomVector <= 1; kolomVector++)
                    {
                        // Sprake van insluiting?
                        if (!(rijVector == 0 && kolomVector == 0) && IsRichtingEenInsluiting(AandeBeurt, rijZet, kolomZet, rijVector, kolomVector))
                        {
                            rijAankomst = rijZet + rijVector;
                            kolomAankomst = kolomZet + kolomVector;

                            // Verander de kleur naar die van de speler
                            while (Bord[rijAankomst, kolomAankomst] == KleurToggle(AandeBeurt))
                            {
                                Bord[rijAankomst, kolomAankomst] = AandeBeurt;
                                rijAankomst += rijVector;
                                kolomAankomst += kolomVector;
                            }
                        }
                    }
                }

                AandeBeurt = KleurToggle(AandeBeurt);

            }

            return zetMogelijk;
        }

        public Kleur KleurToggle(Kleur kleur)
        {
            Kleur result = Kleur.Geen;
            if (kleur == Kleur.Wit)
            {
                result = Kleur.Zwart;
            }
            else if (kleur == Kleur.Zwart)
            {
                result = Kleur.Wit;
            }
            return result;
        }

        private bool IsRichtingEenInsluiting(Kleur color, int rijVertrek, int kolomVertrek, int rijVector, int kolomVector)
        {
            // Controleer richting op nog steeds op het bord en tegengestelde kleur.
            int rijAankomst = rijVertrek + rijVector;
            int kolomAankomst = kolomVertrek + kolomVector;
            while (opBord(rijAankomst, kolomAankomst) &&
                    Bord[rijAankomst, kolomAankomst] == KleurToggle(color))
            {
                rijAankomst += rijVector;
                kolomAankomst += kolomVector;
            }

            if (!opBord(rijAankomst, kolomAankomst) ||                                                      // Op het bord OF
                (rijAankomst - rijVector == rijVertrek && kolomAankomst - kolomVector == kolomVertrek) ||   // Maar 1 stap verder OF
                Bord[rijAankomst, kolomAankomst] != color)                                                  // Aankomst heeft andere kleur
                return false;

            // Insluiting
            return true;
        }

        private bool opBord(int rijZet, int kolomZet)
        {
            return (rijZet >= 0 && rijZet <= 7) && (kolomZet >= 0 && kolomZet <= 7);
        }

    }
}

