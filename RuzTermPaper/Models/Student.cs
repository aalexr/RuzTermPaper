using System;

namespace RuzTermPaper.Models
{
    public class Student : Receiver, IEquatable<Student>
    {
        private const ReceiverType receivertype = ReceiverType.email;
        private string email;

        public override object Id { get => email; set => email = (string)value; }

        public override ReceiverType RType => receivertype;

        public bool Equals(Student other) => email.Equals(other.email);
    }
}
