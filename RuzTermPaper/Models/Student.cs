using System;

namespace RuzTermPaper.Models
{
    public class Student : IEquatable<Student>, IReceiver
    {
        private const ReceiverType receivertype = ReceiverType.email;
        private string email;
        public ReceiverType type => receivertype;

        public object Id { get => email; set => email = (string)value; }

        public bool Equals(Student other) => email.Equals(other.email);
    }
}
