namespace RuzTermPaper
{

    public class Lecturer : System.IEquatable<Lecturer>
    {
        public string chair { get; set; }
        public int chairOid { get; set; }
        public string fio { get; set; }
        public int lecturerOid { get; set; }
        public string shortFIO { get; set; }

        public bool Equals(Lecturer other) => lecturerOid == other.lecturerOid;

        public override string ToString() => fio;
    }
}
