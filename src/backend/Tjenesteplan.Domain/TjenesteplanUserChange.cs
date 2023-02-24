namespace Tjenesteplan.Domain
{
    public class TjenesteplanUserChange
    {
        public int UserId { get; }
        public string Fullname { get; }
        public TjenesteplanChange Change { get; }

        public TjenesteplanUserChange(int userId, string fullname, TjenesteplanChange change)
        {
            UserId = userId;
            Fullname = fullname;
            Change = change;
        }

    }
}