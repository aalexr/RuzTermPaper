namespace RuzTermPaper
{
    public class Group : System.IEquatable<Group>
    {
        public static int receivertype = 3;
        public int chairOid { get; set; }
        public int course { get; set; }
        public string faculty { get; set; }
        public int facultyOid { get; set; }
        public string formOfEducation { get; set; }
        public int groupOid { get; set; }
        public string number { get; set; }
        public string speciality { get; set; }

        public bool Equals(Group other) => groupOid == other.groupOid;

        public override string ToString() => number;
    }
}
