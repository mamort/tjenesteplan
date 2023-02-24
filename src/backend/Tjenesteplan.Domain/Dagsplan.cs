using System.Collections.Generic;

namespace Tjenesteplan.Domain
{
    public class Dagsplan
    {
        public DagsplanEnum DagsplanId { get; }
        public string Name { get; }
        public bool IsRolling { get; }
        public bool IsSystemDagsplan { get; }

        public static Dagsplan Døgnvakt = new Dagsplan(
            dagsplanId: DagsplanEnum.Døgnvakt,
            isRolling: true
        );
        public static Dagsplan Nattevakt = new Dagsplan(
            DagsplanEnum.Nattevakt, 
            isRolling: true
        );
        public static Dagsplan FriEtterVakt = new Dagsplan(
            DagsplanEnum.FriEtterVakt, 
            isRolling: true
        );
        public static Dagsplan Dagvakt = new Dagsplan(
            DagsplanEnum.Dagvakt, 
            isRolling: true
        );
        public static Dagsplan Avspasering = new Dagsplan(
            DagsplanEnum.Avspasering, 
            isRolling: true
        );
        public static Dagsplan Fordypning = new Dagsplan(
            DagsplanEnum.FordypningForskning, 
            name: "Fordypning/Forskning",
            isRolling: true
        );
        public static Dagsplan Permisjon = new Dagsplan(
            DagsplanEnum.Permisjon, 
            isRolling: false
        );
        public static Dagsplan Ferie = new Dagsplan(
            DagsplanEnum.Ferie,
            isRolling: false
        );
        public static Dagsplan UkesAvspasering = new Dagsplan(
            DagsplanEnum.UkesAvspasering, 
            name: "Ukesavspasering",
            isRolling: false
        );
        public static Dagsplan KursKonferanseMøte = new Dagsplan(
            DagsplanEnum.KursKonferanseMøte, 
            name: "Kurs/Konferanse/Møte",
            isRolling: false
        );
        public static Dagsplan Universitet = new Dagsplan(
            DagsplanEnum.Universitet, 
            name: "Universitet/Intervensjonssentret",
            isRolling: false
        );
        public static Dagsplan Sykdom = new Dagsplan(
            DagsplanEnum.Sykdom, 
            isRolling: false
        );

        public static Dagsplan ForslagTilVaktbytte = new Dagsplan(
            DagsplanEnum.ForslagTilVaktbytte, 
            name: "Forespørsel om vaktbytte",
            isRolling: false,
            isSystemDagsplan: true
        );
        public static Dagsplan ForespørselOmVaktbytte = new Dagsplan(
            DagsplanEnum.ForespørselOmVaktbytte, 
            name: "Forespørsel om vaktbytte",
            isRolling: false,
            isSystemDagsplan: true
        );

        public static Dagsplan ForespørselOmVakansvakt = new Dagsplan(
            DagsplanEnum.ForespørselOmVakansvakt,
            name: "Forespørsel om vakansvakt",
            isRolling: false,
            isSystemDagsplan: true
        );

        public static IReadOnlyList<Dagsplan> AllDagsplaner = new List<Dagsplan>
        {   Døgnvakt,
            Nattevakt,
            FriEtterVakt,
            Dagvakt,
            Avspasering,
            Fordypning,
            Permisjon,
            UkesAvspasering,
            KursKonferanseMøte,
            Universitet,
            Sykdom,
            ForespørselOmVaktbytte,
            ForslagTilVaktbytte,
            ForespørselOmVakansvakt
        };

        private Dagsplan(DagsplanEnum dagsplanId, bool isRolling, string name = null, bool isSystemDagsplan = false)
        {
            DagsplanId = dagsplanId;
            Name = name ?? dagsplanId.ToString();
            IsRolling = isRolling;
            IsSystemDagsplan = isSystemDagsplan;
        }
    }
}
 