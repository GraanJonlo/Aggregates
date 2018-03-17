namespace Domain.Values
{
    public readonly struct Name
    {
        public readonly string FirstName;
        public readonly string LastName;

        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}