using System.Collections.Generic;

namespace Tjenesteplan.Domain
{
    public class LegeSpesialitet
    {
        private static readonly List<LegeSpesialitet> _spesialiteter = new List<LegeSpesialitet>();

        public int Id { get; }
        public string Name { get; }
        public IReadOnlyList<LegeGrenSpesialitet> Grenspesialiteter { get; }

        private LegeSpesialitet(int id, string name, IReadOnlyList<LegeGrenSpesialitet> grenspesialiteter = null)
        {
            Id = id;
            Name = name;
            Grenspesialiteter = grenspesialiteter ?? new List<LegeGrenSpesialitet>();
            _spesialiteter.Add(this);
        }

        public static LegeSpesialitet AkuttOgMottaksmedisin = new LegeSpesialitet(1, "Akutt- og mottaksmedisin");
        public static LegeSpesialitet Allmennmedisin = new LegeSpesialitet(2, "Allmennmedisin"); 
        public static LegeSpesialitet Anestesiologi = new LegeSpesialitet(3, "Anestesiologi");
        public static LegeSpesialitet Arbeidsmedisin = new LegeSpesialitet(4, "Arbeidsmedisin");
        public static LegeSpesialitet BarneOgUngdomspsykiatri = new LegeSpesialitet(5, "Barne- og ungdomspsykiatri");
        public static LegeSpesialitet Barnekirurgi = new LegeSpesialitet(6, "Barnekirurgi");
        public static LegeSpesialitet Barnesykdommer = new LegeSpesialitet(7, "Barnesykdommer");
        public static LegeSpesialitet Blodsykdommer = new LegeSpesialitet(8, "Blodsykdommer");
        public static LegeSpesialitet BrystOgEndokrinkirurgi = new LegeSpesialitet(9, "Bryst- og endokrinkirurgi");
        public static LegeSpesialitet Endokrinologi = new LegeSpesialitet(10, "Endokrinologi");
        public static LegeSpesialitet Fordøyelsessykdommer = new LegeSpesialitet(11, "Fordøyelsessykdommer");
        public static LegeSpesialitet FysikalskMedisinOgRehabilitering = new LegeSpesialitet(12, "Fysikalsk medisin og rehabilitering");
        public static LegeSpesialitet FødselshjelpOgKvinnesykdommer = new LegeSpesialitet(13, "Fødselshjelp og kvinnesykdommer");
        public static LegeSpesialitet GastroenterologiskKirurgi = new LegeSpesialitet(14, "Gastroenterologisk kirurgi");
        public static LegeSpesialitet GenerellKirurgi = new LegeSpesialitet(15, "Generell kirurgi");
        public static LegeSpesialitet Geriatri = new LegeSpesialitet(16, "Geriatri");
        public static LegeSpesialitet Hjertesykdommer = new LegeSpesialitet(17, "Hjertesykdommer");
        public static LegeSpesialitet HudOgVeneriskeSykdommer = new LegeSpesialitet(18, "Hud- og veneriske sykdommer");
        public static LegeSpesialitet ImmunologiOgTransfusjonsmedisin = new LegeSpesialitet(19, "Immunologi og transfusjonsmedisin");
        public static LegeSpesialitet Indremedisin = new LegeSpesialitet(20, "Indremedisin");
        public static LegeSpesialitet Infeksjonssykdommer = new LegeSpesialitet(21, "Infeksjonssykdommer");
        public static LegeSpesialitet Karkirurgi = new LegeSpesialitet(22, "Karkirurgi");
        public static LegeSpesialitet KliniskFarmakologi = new LegeSpesialitet(23, "Klinisk farmakologi");
        public static LegeSpesialitet KliniskNevrofysiologi = new LegeSpesialitet(24, "Klinisk nevrofysiologi");
        public static LegeSpesialitet Lungesykdommer = new LegeSpesialitet(25, "Lungesykdommer");
        public static LegeSpesialitet MaxillofacialKirurgi = new LegeSpesialitet(26, "Maxillofacial kirurgi");      
        public static LegeSpesialitet MedisinskBiokjemi = new LegeSpesialitet(27, "Medisinsk biokjemi");
        public static LegeSpesialitet MedisinskGenetikk = new LegeSpesialitet(28, "Medisinsk genetikk");
        public static LegeSpesialitet MedisinskMikrobiologi = new LegeSpesialitet(29, "Medisinsk mikrobiologi");
        public static LegeSpesialitet Nevrokirurgi = new LegeSpesialitet(30, "Nevrokirurgi");
        public static LegeSpesialitet Nevrologi = new LegeSpesialitet(31, "Nevrologi");
        public static LegeSpesialitet Nukleærmedisin = new LegeSpesialitet(32, "Nukleærmedisin");
        public static LegeSpesialitet Nyresykdommer = new LegeSpesialitet(33, "Nyresykdommer");
        public static LegeSpesialitet Onkologi = new LegeSpesialitet(34, "Onkologi");
        public static LegeSpesialitet OrtopediskKirurgi = new LegeSpesialitet(35, "Ortopedisk kirurgi");
        public static LegeSpesialitet Patologi = new LegeSpesialitet(36, "Patologi");
        public static LegeSpesialitet Plastikkirurgi = new LegeSpesialitet(37, "Plastikkirurgi");
        public static LegeSpesialitet Psykiatri = new LegeSpesialitet(38, "Psykiatri");
        public static LegeSpesialitet Radiologi = new LegeSpesialitet(39, "Radiologi");
        public static LegeSpesialitet Revmatologi = new LegeSpesialitet(40, "Revmatologi");
        public static LegeSpesialitet RusOgAvhengighetsmedisin = new LegeSpesialitet(41, "Rus- og avhengighetsmedisin");
        public static LegeSpesialitet Samfunnsmedisin = new LegeSpesialitet(42, "Samfunnsmedisin");
        public static LegeSpesialitet Thoraxkirurgi = new LegeSpesialitet(43, "Thoraxkirurgi");
        public static LegeSpesialitet Urologi = new LegeSpesialitet(44, "Urologi");
        public static LegeSpesialitet ØreNeseHalssykdommer = new LegeSpesialitet(45, "Øre-nese-halssykdommer");
        public static LegeSpesialitet Øyesykdommer = new LegeSpesialitet(46, "Øyesykdommer");

        public static IReadOnlyList<LegeSpesialitet> Spesialiteter => _spesialiteter;
    }

    public class LegeGrenSpesialitet
    {
        
    }
}
 